SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_epoch BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', GETUTCDATE()) * 1000;

IF OBJECT_ID('tempdb..#cjs_source') IS NOT NULL DROP TABLE #cjs_source;
CREATE TABLE #cjs_source (
  [rn] INT,
  [material_code] NVARCHAR(100),
  [model_code] NVARCHAR(100),
  [destination_code] NVARCHAR(100)
);

-- 源表原样全量装入（行数 = PP_SapModelDest；禁止擅自去重改行数）
INSERT INTO #cjs_source ([rn], [material_code], [model_code], [destination_code])
SELECT
  S.rn,
  LTRIM(RTRIM(S.[D_SAP_DEST_Z001])),
  LTRIM(RTRIM(S.[D_SAP_DEST_Z002])),
  LTRIM(RTRIM(S.[D_SAP_DEST_Z003]))
FROM (
  SELECT *, ROW_NUMBER() OVER (ORDER BY [D_SAP_DEST_Z001], [D_SAP_DEST_Z002], [D_SAP_DEST_Z003]) AS rn
  FROM [Sap_Data].[dbo].[PP_SapModelDest]
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #cjs_source);
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapModelDest]);

-- 全量同步：临时表行数必须等于 SAP 源表行数
IF @source_count <> @sap_raw_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'SAP源行数与装入不一致: sap=', @sap_raw_count, N', loaded=', @source_count);
  THROW 50003, @src_msg, 1;
END;

-- 业务键 = Material+Model+Destination；源内重复则无法 1:1 落目标表（与 Tenant+Material+Model+Destination 一致）
IF EXISTS (
  SELECT 1
  FROM #cjs_source
  GROUP BY [material_code], [model_code], [destination_code]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT([material_code], N' / ', [model_code], N' / ', [destination_code], N' x', COUNT(*))
  FROM #cjs_source
  GROUP BY [material_code], [model_code], [destination_code]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#cjs_delta') IS NOT NULL DROP TABLE #cjs_delta;
CREATE TABLE #cjs_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  tenant_code NVARCHAR(20),
  company_code NVARCHAR(20),
  change_by BIGINT,
  sort_order_old INT,
  sort_order_new INT,
  ext_field_old NVARCHAR(MAX),
  ext_field_new NVARCHAR(MAX),
  remark_old NVARCHAR(MAX),
  remark_new NVARCHAR(MAX)
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_model_destination]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);

-- 存在则仅在需恢复软删或默认字段偏离时 UPDATE；无变化不写入 #cjs_delta
MERGE INTO [takt_logistics_materials_model_destination] AS T
USING (
  SELECT
    @base_epoch + S.[rn] AS [id],
    S.[rn],
    S.[material_code],
    S.[model_code],
    S.[destination_code]
  FROM #cjs_source S
) AS S
ON T.[tenant_code] = @tenant_code
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
AND LTRIM(RTRIM(T.[model_code])) = S.[model_code]
AND LTRIM(RTRIM(T.[destination_code])) = S.[destination_code]
WHEN MATCHED AND (
  T.[is_deleted] <> 0
  OR T.[sort_order] <> 0
  OR LTRIM(RTRIM(ISNULL(T.[ext_field], N''))) <> N'{}'
  OR LTRIM(RTRIM(ISNULL(T.[remark], N''))) <> N'幂等更新'
) THEN
  UPDATE SET
    T.[sort_order] = 0,
    T.[ext_field] = '{}',
    T.[remark] = N'幂等更新',
    T.[updated_by] = @sync_user_id,
    T.[updated_at] = @now,
    T.[is_deleted] = 0
WHEN NOT MATCHED THEN
  INSERT (
    [id],
    [material_code],
    [material_name],
    [model_code],
    [model_name],
    [destination_code],
    [destination_name],
    [sort_order],
    [tenant_code],
    [ext_field],
    [remark],
    [created_by],
    [created_at],
    [updated_by],
    [updated_at],
    [is_deleted]
  )
  VALUES (
    S.[id],
    S.[material_code],
    '',
    S.[model_code],
    '',
    S.[destination_code],
    '',
    0,
    @tenant_code,
    '{}',
    N'幂等更新',
    @sync_user_id,
    @now,
    @sync_user_id,
    @now,
    0
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  @tenant_code,
  @company_code,
  @sync_user_id,
  DELETED.[sort_order], INSERTED.[sort_order],
  DELETED.[ext_field], INSERTED.[ext_field],
  DELETED.[remark], INSERTED.[remark]
INTO #cjs_delta(
  rn, oper_type, id,
  tenant_code, company_code, change_by,
  sort_order_old, sort_order_new,
  ext_field_old, ext_field_new,
  remark_old, remark_new
);

-- 孤儿软删：目标有而源没有时才软删；存在更新/不存在插入已由 MERGE 完成
IF OBJECT_ID('tempdb..#soft_deleted_rows') IS NOT NULL DROP TABLE #soft_deleted_rows;
CREATE TABLE #soft_deleted_rows (
  [id] BIGINT,
  [material_code] NVARCHAR(100),
  [model_code] NVARCHAR(100),
  [destination_code] NVARCHAR(100)
);

UPDATE T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT
  INSERTED.[id],
  INSERTED.[material_code],
  INSERTED.[model_code],
  INSERTED.[destination_code]
INTO #soft_deleted_rows ([id], [material_code], [model_code], [destination_code])
FROM [takt_logistics_materials_model_destination] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #cjs_source S
    WHERE S.[material_code] = LTRIM(RTRIM(T.[material_code]))
      AND S.[model_code] = LTRIM(RTRIM(T.[model_code]))
      AND S.[destination_code] = LTRIM(RTRIM(T.[destination_code]))
  );

DECLARE @delete_count INT = @@ROWCOUNT;
DECLARE @soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @soft_deleted_keys = STRING_AGG(
  CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    ISNULL([material_code], N''), N'/',
    ISNULL([model_code], N''), N'/',
    ISNULL([destination_code], N'')
  ),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_model_destination]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_model_destination]
  WHERE [tenant_code] = @tenant_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_model_destination]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 1
);

IF @target_count <> @source_count
BEGIN
  DECLARE @count_msg NVARCHAR(200) = CONCAT(
    N'有效行数不一致: source=', @source_count, N', active=', @target_count);
  THROW 50002, @count_msg, 1;
END;

INSERT INTO [takt_statistics_logging_delta_log] (
  [id],[oper_type],[table_name],[primary_key_id],
  [before_data],[after_data],[diff_data],[sql_statement],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[tenant_code],[company_code],
  [ext_field_json],[remark],[created_by],[created_at]
)
SELECT
  @base_epoch + d.rn,
  d.oper_type,
  N'takt_logistics_materials_model_destination',
  d.id,
  ISNULL((
    SELECT
      d.sort_order_old AS [sort_order],
      d.ext_field_old AS [ext_field],
      d.remark_old AS [remark]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  (
    SELECT
      d.sort_order_new AS [sort_order],
      d.ext_field_new AS [ext_field],
      d.remark_new AS [remark]
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  ISNULL((
    SELECT
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.sort_order_old AS NVARCHAR), 'null') END AS [sort_order.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.sort_order_new AS NVARCHAR), 'null') END AS [sort_order.new],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(d.remark_old, 'null') END AS [remark.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(d.remark_new, 'null') END AS [remark.new]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  N'MERGE SAP Model Destination Sync',
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,0,
  d.tenant_code,d.company_code,'{}',N'SAP_SYNC',d.change_by,@now
FROM #cjs_delta d;

DECLARE @insert_count INT = (SELECT COUNT(*) FROM #cjs_delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #cjs_delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"sap_raw":' + CAST(@sap_raw_count AS NVARCHAR)
  + N',"source":' + CAST(@source_count AS NVARCHAR)
  + N',"target_before":' + CAST(@target_before AS NVARCHAR)
  + N',"target_after":' + CAST(@target_count AS NVARCHAR)
  + N',"target_physical":' + CAST(@target_physical AS NVARCHAR)
  + N',"soft_deleted":' + CAST(@soft_deleted AS NVARCHAR)
  + N',"insert":' + CAST(@insert_count AS NVARCHAR)
  + N',"update":' + CAST(@update_count AS NVARCHAR)
  + N',"unchanged":' + CAST(@unchanged_count AS NVARCHAR)
  + N',"soft_delete_this_run":' + CAST(@delete_count AS NVARCHAR)
  + N',"soft_delete_keys":"' + REPLACE(@soft_deleted_keys, N'"', N'''') + N'"}';

INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[created_by],[created_at]
)
VALUES (
  @base_epoch + 1,
  N'SYSTEM_SAP_SYNC',
  N'SAP_SYNC',
  N'机种-目的地管理',
  N'exec_sql_merge',
  'SQL',
  N'/sync/sap/model_destination',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@sync_user_id,@now
);

-- Quartz 执行器读取此结果集写入 ExecuteMessage / quartz-.log
SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @sap_raw_count AS [sap_raw_count],
  @source_count AS [source_count],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @target_physical AS [target_physical],
  @soft_deleted AS [soft_deleted],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count],
  @soft_deleted_keys AS [soft_deleted_keys];
