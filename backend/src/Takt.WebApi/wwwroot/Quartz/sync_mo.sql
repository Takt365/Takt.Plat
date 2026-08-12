SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

IF OBJECT_ID('tempdb..#order_source') IS NOT NULL DROP TABLE #order_source;
CREATE TABLE #order_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(100),
  [prod_order_code] NVARCHAR(100),
  [material_code] NVARCHAR(100),
  [material_description] NVARCHAR(40),
  [prod_batch] NVARCHAR(100),
  [prod_order_qty] DECIMAL(18,4),
  [produced_qty] DECIMAL(18,4),
  [unit_of_measure] NVARCHAR(20),
  [actual_start_date] DATE,
  [priority] INT,
  [routing_code] NVARCHAR(100),
  [prod_order_type] NVARCHAR(100)
);

-- 源表原样全量装入（行数 = PP_SapOrders；禁止擅自去重改行数）
INSERT INTO #order_source
SELECT
  S.rn,
  @base_id + S.rn,
  LTRIM(RTRIM(S.[D_SAP_COOIS_C001])),
  LTRIM(RTRIM(S.[D_SAP_COOIS_C002])),
  LTRIM(RTRIM(S.[D_SAP_COOIS_C003])),
  CAST(N'' AS NVARCHAR(40)),
  LTRIM(RTRIM(S.[D_SAP_COOIS_C004])),
  COALESCE(TRY_CAST(S.[D_SAP_COOIS_C005] AS DECIMAL(18,4)), 0),
  COALESCE(TRY_CAST(S.[D_SAP_COOIS_C006] AS DECIMAL(18,4)), 0),
  CASE WHEN ISNULL(S.[D_SAP_COOIS_C004], '') LIKE '%||%' THEN 'EA' ELSE 'PC' END,
  TRY_CAST(S.[D_SAP_COOIS_C007] AS DATE),
  3,
  LTRIM(RTRIM(S.[D_SAP_COOIS_C008])),
  LTRIM(RTRIM(S.[D_SAP_COOIS_C009]))
FROM (
  SELECT *, ROW_NUMBER() OVER (ORDER BY [D_SAP_COOIS_C002]) AS rn
  FROM [Sap_Data].[dbo].[PP_SapOrders]
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

UPDATE H
SET H.[material_description] = LEFT(ISNULL(MP.[material_description], N''), 40)
FROM #order_source H
OUTER APPLY (
  SELECT TOP (1) LTRIM(RTRIM(P.[material_description])) AS [material_description]
  FROM [takt_logistics_materials_material_plant] P
  WHERE P.[tenant_code] = @tenant_code
    AND P.[company_code] = @company_code
    AND P.[is_deleted] = 0
    AND LTRIM(RTRIM(P.[plant_code])) = H.[plant_code]
    AND LTRIM(RTRIM(P.[material_code])) = H.[material_code]
    AND LTRIM(RTRIM(ISNULL(P.[material_description], N''))) <> N''
  ORDER BY P.[id]
) MP;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #order_source);
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapOrders]);

-- 全量同步：临时表行数必须等于源表行数
IF @source_count <> @sap_raw_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'源行数与装入不一致: source=', @sap_raw_count, N', loaded=', @source_count);
  THROW 50003, @src_msg, 1;
END;

-- 唯一键 = Plant+ProdOrderType+ProdOrderCode+Material；源内重复则无法 1:1 落目标表
IF EXISTS (
  SELECT 1
  FROM #order_source
  GROUP BY [plant_code], [prod_order_type], [prod_order_code], [material_code]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT(
      [plant_code], N' / ', [prod_order_type], N' / ', [prod_order_code], N' / ', [material_code], N' x', COUNT(*))
  FROM #order_source
  GROUP BY [plant_code], [prod_order_type], [prod_order_code], [material_code]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#order_delta') IS NOT NULL DROP TABLE #order_delta;
CREATE TABLE #order_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(100),
  prod_order_code NVARCHAR(100),
  material_code_old NVARCHAR(100),
  material_code_new NVARCHAR(100),
  prod_order_qty_old DECIMAL(18,4),
  prod_order_qty_new DECIMAL(18,4),
  produced_qty_old DECIMAL(18,4),
  produced_qty_new DECIMAL(18,4),
  unit_of_measure_old NVARCHAR(20),
  unit_of_measure_new NVARCHAR(20),
  actual_start_date_old DATE,
  actual_start_date_new DATE,
  routing_code_old NVARCHAR(100),
  routing_code_new NVARCHAR(100),
  prod_order_type_old NVARCHAR(100),
  prod_order_type_new NVARCHAR(100)
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_aps_production_order]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);

-- ON 与唯一索引一致：Tenant+Company+Plant+ProdOrderType+ProdOrderCode+Material
MERGE INTO [takt_logistics_manufacturing_aps_production_order] AS T
USING #order_source AS S
ON T.[tenant_code] = @tenant_code
AND T.[company_code] = @company_code
AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
AND LTRIM(RTRIM(T.[prod_order_type])) = S.[prod_order_type]
AND LTRIM(RTRIM(T.[prod_order_code])) = S.[prod_order_code]
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
WHEN MATCHED AND (
  T.[is_deleted] <> 0
  OR LTRIM(RTRIM(ISNULL(T.[material_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_description], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[prod_batch], N''))) <> LTRIM(RTRIM(ISNULL(S.[prod_batch], N'')))
  OR ROUND(T.[prod_order_qty], 4) <> ROUND(S.[prod_order_qty], 4)
  OR ROUND(T.[produced_qty], 4) <> ROUND(S.[produced_qty], 4)
  OR LTRIM(RTRIM(ISNULL(T.[unit_of_measure], N''))) <> LTRIM(RTRIM(ISNULL(S.[unit_of_measure], N'')))
  OR ISNULL(T.[actual_start_date], '1900-01-01') <> ISNULL(S.[actual_start_date], '1900-01-01')
  OR LTRIM(RTRIM(ISNULL(T.[routing_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[routing_code], N'')))
  OR T.[priority] <> S.[priority]
) THEN
  UPDATE SET
    T.[material_description] = S.[material_description],
    T.[prod_batch] = S.[prod_batch],
    T.[prod_order_qty] = S.[prod_order_qty],
    T.[produced_qty] = S.[produced_qty],
    T.[unit_of_measure] = S.[unit_of_measure],
    T.[actual_start_date] = S.[actual_start_date],
    T.[routing_code] = S.[routing_code],
    T.[priority] = S.[priority],
    T.[updated_by] = @sync_user_id,
    T.[updated_at] = @now,
    T.[culture_code] = @culture_code,
    T.[is_deleted] = 0
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[prod_order_code],[material_code],[material_description],[prod_batch],
    [prod_order_qty],[produced_qty],[unit_of_measure],
    [actual_start_date],[actual_end_date],[priority],[work_center],
    [routing_code],[serial_code],[prod_order_type],
    [planned_order_id],[aps_order_id],
    [planned_start_time],[planned_end_time],[order_status],
    [tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
  )
  VALUES (
    S.[id],S.[plant_code],S.[prod_order_code],S.[material_code],S.[material_description],S.[prod_batch],
    S.[prod_order_qty],S.[produced_qty],S.[unit_of_measure],
    S.[actual_start_date],NULL,S.[priority],'',
    S.[routing_code],'',S.[prod_order_type],
    NULL,NULL,NULL,NULL,1,
    @tenant_code,@company_code,@culture_code,'{}',N'mismatch update',
    @sync_user_id,@now,@sync_user_id,@now,0
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[prod_order_code],
  DELETED.[material_code], INSERTED.[material_code],
  DELETED.[prod_order_qty], INSERTED.[prod_order_qty],
  DELETED.[produced_qty], INSERTED.[produced_qty],
  DELETED.[unit_of_measure], INSERTED.[unit_of_measure],
  DELETED.[actual_start_date], INSERTED.[actual_start_date],
  DELETED.[routing_code], INSERTED.[routing_code],
  DELETED.[prod_order_type], INSERTED.[prod_order_type]
INTO #order_delta(
  rn, oper_type, id, plant_code, prod_order_code,
  material_code_old, material_code_new,
  prod_order_qty_old, prod_order_qty_new,
  produced_qty_old, produced_qty_new,
  unit_of_measure_old, unit_of_measure_new,
  actual_start_date_old, actual_start_date_new,
  routing_code_old, routing_code_new,
  prod_order_type_old, prod_order_type_new
);

-- update work_center
;WITH wc_src AS (
  SELECT
    LTRIM(RTRIM([D_SAP_ZPBLD_Z002])) AS material_code,
    LTRIM(RTRIM([D_SAP_ZPBLD_Z003])) AS z003,
    LTRIM(RTRIM([D_SAP_ZPBLD_Z004])) AS z004
  FROM [Sap_Data].[dbo].[PP_SapManhour]
),
wc_agg AS (
  SELECT
    material_code,
    -- 与实体 WorkCenter Length=500 对齐；多中心 code||desc 汇总可能超长
    LEFT(STRING_AGG(
  CAST(
    z003 + N'||' + z004
  AS NVARCHAR(MAX)),
  N';'
), 500) AS work_center
  FROM wc_src
  GROUP BY material_code
)
UPDATE T
SET
  T.[work_center] = W.work_center,
  T.[updated_at] = @now
FROM [takt_logistics_manufacturing_aps_production_order] T
JOIN wc_agg W
  ON LTRIM(RTRIM(T.[material_code])) = W.material_code
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND ISNULL(T.[work_center], '') <> ISNULL(W.work_center, '');

DECLARE @wc_upd INT = @@ROWCOUNT;

-- update serial_code
;WITH ser_agg AS (
  SELECT
    LTRIM(RTRIM([D_SAP_SER05_C002])) AS prod_order_code,
    MIN([D_SAP_SER05_C004]) + '~' + MAX([D_SAP_SER05_C004]) AS serial_code
  FROM [Sap_Data].[dbo].[PP_SapOrderSerial]
  WHERE isDelete = 0
  GROUP BY [D_SAP_SER05_C002]
)
UPDATE T
SET
  T.[serial_code] = S.serial_code,
  T.[updated_at] = @now
FROM [takt_logistics_manufacturing_aps_production_order] T
JOIN ser_agg S
  ON LTRIM(RTRIM(T.[prod_order_code])) = S.prod_order_code
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND ISNULL(T.[serial_code], '') <> ISNULL(S.serial_code, '');

DECLARE @ser_upd INT = @@ROWCOUNT;

-- 孤儿软删：目标有而源没有时才软删；存在更新/不存在插入已由 MERGE 完成
IF OBJECT_ID('tempdb..#soft_deleted_rows') IS NOT NULL DROP TABLE #soft_deleted_rows;
CREATE TABLE #soft_deleted_rows (
  [id] BIGINT,
  [plant_code] NVARCHAR(100),
  [prod_order_type] NVARCHAR(100),
  [prod_order_code] NVARCHAR(100),
  [material_code] NVARCHAR(100)
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
  INSERTED.[plant_code],
  INSERTED.[prod_order_type],
  INSERTED.[prod_order_code],
  INSERTED.[material_code]
INTO #soft_deleted_rows ([id], [plant_code], [prod_order_type], [prod_order_code], [material_code])
FROM [takt_logistics_manufacturing_aps_production_order] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #order_source S
    WHERE S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[prod_order_type] = LTRIM(RTRIM(T.[prod_order_type]))
      AND S.[prod_order_code] = LTRIM(RTRIM(T.[prod_order_code]))
      AND S.[material_code] = LTRIM(RTRIM(T.[material_code]))
  );

DECLARE @delete_count INT = @@ROWCOUNT;
DECLARE @soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    ISNULL([plant_code], N''), N'/',
    ISNULL([prod_order_type], N''), N'/',
    ISNULL([prod_order_code], N''), N'/',
    ISNULL([material_code], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_aps_production_order]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_aps_production_order]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_aps_production_order]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
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
  [ext_field],[remark],[created_by],[created_at]
)
SELECT
  @base_id + d.rn,
  d.oper_type,
  N'takt_logistics_manufacturing_aps_production_order',
  d.id,
  ISNULL((
    SELECT
      d.material_code_old AS [material_code],
      d.prod_order_qty_old AS [prod_order_qty],
      d.produced_qty_old AS [produced_qty],
      d.unit_of_measure_old AS [unit_of_measure],
      d.actual_start_date_old AS [actual_start_date],
      d.routing_code_old AS [routing_code],
      d.prod_order_type_old AS [prod_order_type]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  (
    SELECT
      d.material_code_new AS [material_code],
      d.prod_order_qty_new AS [prod_order_qty],
      d.produced_qty_new AS [produced_qty],
      d.unit_of_measure_new AS [unit_of_measure],
      d.actual_start_date_new AS [actual_start_date],
      d.routing_code_new AS [routing_code],
      d.prod_order_type_new AS [prod_order_type]
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  ISNULL((
    SELECT
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(d.material_code_old, 'null') END AS [material_code.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(d.material_code_new, 'null') END AS [material_code.new],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.prod_order_qty_old AS NVARCHAR), 'null') END AS [prod_order_qty.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.prod_order_qty_new AS NVARCHAR), 'null') END AS [prod_order_qty.new],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.produced_qty_old AS NVARCHAR), 'null') END AS [produced_qty.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.produced_qty_new AS NVARCHAR), 'null') END AS [produced_qty.new]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  N'MERGE Order Sync',
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,0,
  @tenant_code,@company_code,'{}',N'SYNC',@sync_user_id,@now
FROM #order_delta d;

DECLARE @insert_count INT = (SELECT COUNT(*) FROM #order_delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #order_delta WHERE oper_type = 'UPDATE');
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
  + N',"soft_delete_keys":"' + REPLACE(@soft_deleted_keys, N'"', N'''') + N'"'
  + N',"work_center":' + CAST(@wc_upd AS NVARCHAR)
  + N',"serial_code":' + CAST(@ser_upd AS NVARCHAR) + N'}';



INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[created_by],[created_at]
)
VALUES (
  @base_id + 1,
  N'SYSTEM_SYNC',
  N'SYNC',
  N'order management',
  N'exec_sql_merge',
  'SQL',
  N'/sync/order',
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
  @sap_raw_count AS [source_raw_count],
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
