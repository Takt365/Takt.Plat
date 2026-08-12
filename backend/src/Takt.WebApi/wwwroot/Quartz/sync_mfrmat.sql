SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体 TaktManufacturerMaterial 一致（租户级，无 company_code）
-- {{SourceDatabase}}.dbo.takt_logistics_procurement_manufacturer_material → 当前租户库同名表
-- 业务唯一键：InternalMaterialCode+MaterialCode（与租户库唯一索引 Tenant+InternalMaterialCode+MaterialCode 对齐）

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [vendor_code] NVARCHAR(20),
  [vendor_short_name] NVARCHAR(40),
  [supplier_code] NVARCHAR(10),
  [supplier_short_name] NVARCHAR(40),
  [material_type] NVARCHAR(4),
  [material_group] NVARCHAR(9),
  [internal_material_code] NVARCHAR(20),
  [material_code] NVARCHAR(20),
  [material_description] NVARCHAR(40),
  [manufacturer_material_code] NVARCHAR(40),
  [manufacturer_material_description] NVARCHAR(40),
  [manufacturer_material_specification] NVARCHAR(70),
  [tenant_code] NVARCHAR(3),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [is_deleted] INT,
  [updated_by] BIGINT
);

-- 源表装入：
-- 1) 物料码含全角括号的不参与同步
-- 2) 18 位纯数字本厂/内部物料码截末 10 位（制造商物料码 Length=40，不截断）
-- 3) 按唯一键 InternalMaterialCode+MaterialCode 去重
INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[vendor_code],
  S.[vendor_short_name],
  S.[supplier_code],
  S.[supplier_short_name],
  S.[material_type],
  S.[material_group],
  S.[internal_material_code],
  S.[material_code],
  S.[material_description],
  S.[manufacturer_material_code],
  S.[manufacturer_material_description],
  S.[manufacturer_material_specification],
  @tenant_code,
  '{}',
  '',
  S.[is_deleted],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY N.[internal_material_code], N.[material_code]
    ) AS rn
  FROM (
    SELECT
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[vendor_code], N''))), 20), N'') AS [vendor_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[vendor_short_name], N''))), 40), N'') AS [vendor_short_name],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[supplier_code], N''))), 10), N'') AS [supplier_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[supplier_short_name], N''))), 40), N'') AS [supplier_short_name],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_type], N''))), 4), N''), N'HERS') AS [material_type],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_group], N''))), 9), N''), N'') AS [material_group],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[internal_material_code]))) = 18
          AND LTRIM(RTRIM(R.[internal_material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[internal_material_code])), 10)
        ELSE LTRIM(RTRIM(R.[internal_material_code]))
      END AS [internal_material_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END AS [material_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_description], N''))), 40), N''), N'') AS [material_description],
      LEFT(LTRIM(RTRIM(ISNULL(R.[manufacturer_material_code], N''))), 40) AS [manufacturer_material_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[manufacturer_material_description], N''))), 40), N''), N'') AS [manufacturer_material_description],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[manufacturer_material_specification], N''))), 70), N'') AS [manufacturer_material_specification],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[internal_material_code]))) = 18
              AND LTRIM(RTRIM(R.[internal_material_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[internal_material_code])), 10)
            ELSE LTRIM(RTRIM(R.[internal_material_code]))
          END,
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
              AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
            ELSE LTRIM(RTRIM(R.[material_code]))
          END
        ORDER BY
          LEN(ISNULL(LTRIM(RTRIM(R.[manufacturer_material_code])), N'')) DESC,
          LEN(ISNULL(LTRIM(RTRIM(R.[material_description])), N'')) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_manufacturer_material] R
    WHERE LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[manufacturer_material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) NOT LIKE N'%）%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #st_source);
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_manufacturer_material]);
DECLARE @sap_excluded_paren INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_manufacturer_material] R
  WHERE LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) LIKE N'%（%'
     OR LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) LIKE N'%）%'
     OR LTRIM(RTRIM(ISNULL(R.[material_code], N''))) LIKE N'%（%'
     OR LTRIM(RTRIM(ISNULL(R.[material_code], N''))) LIKE N'%）%'
);
DECLARE @sap_eligible_count INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_manufacturer_material] R
  WHERE LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[manufacturer_material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) NOT LIKE N'%（%'
    AND LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) NOT LIKE N'%）%'
    AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
    AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
);
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[internal_material_code]))) = 18
          AND LTRIM(RTRIM(R.[internal_material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[internal_material_code])), 10)
        ELSE LTRIM(RTRIM(R.[internal_material_code]))
      END AS [internal_material_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END AS [material_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_manufacturer_material] R
    WHERE LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[manufacturer_material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[internal_material_code], N''))) NOT LIKE N'%）%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
    GROUP BY
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[internal_material_code]))) = 18
          AND LTRIM(RTRIM(R.[internal_material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[internal_material_code])), 10)
        ELSE LTRIM(RTRIM(R.[internal_material_code]))
      END,
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END
  ) K
);
DECLARE @dedupe_dropped INT = @sap_eligible_count - @sap_key_count;

IF @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', sap_raw=', @sap_raw_count, N', excluded_paren=', @sap_excluded_paren);
  THROW 50003, @src_msg, 1;
END;

IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY [internal_material_code], [material_code]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT(
      [internal_material_code], N' / ',
      [material_code], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [internal_material_code], [material_code]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  internal_material_code NVARCHAR(20),
  material_code NVARCHAR(20),
  manufacturer_material_code NVARCHAR(40),
  tenant_code NVARCHAR(3),
  change_by BIGINT,
  manufacturer_material_code_old NVARCHAR(40),
  manufacturer_material_code_new NVARCHAR(40),
  material_description_old NVARCHAR(40),
  material_description_new NVARCHAR(40),
  supplier_code_old NVARCHAR(10),
  supplier_code_new NVARCHAR(10)
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_manufacturer_material]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_procurement_manufacturer_material] AS T
USING #st_source AS S
ON T.[tenant_code] = S.[tenant_code]
AND LTRIM(RTRIM(T.[internal_material_code])) = S.[internal_material_code]
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[vendor_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[vendor_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[vendor_short_name], N''))) <> LTRIM(RTRIM(ISNULL(S.[vendor_short_name], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[supplier_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[supplier_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[supplier_short_name], N''))) <> LTRIM(RTRIM(ISNULL(S.[supplier_short_name], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[material_type], N''))) <> S.[material_type]
  OR LTRIM(RTRIM(ISNULL(T.[material_group], N''))) <> S.[material_group]
  OR LTRIM(RTRIM(ISNULL(T.[material_description], N''))) <> S.[material_description]
  OR LTRIM(RTRIM(ISNULL(T.[manufacturer_material_code], N''))) <> S.[manufacturer_material_code]
  OR LTRIM(RTRIM(ISNULL(T.[manufacturer_material_description], N''))) <> S.[manufacturer_material_description]
  OR LTRIM(RTRIM(ISNULL(T.[manufacturer_material_specification], N''))) <> LTRIM(RTRIM(ISNULL(S.[manufacturer_material_specification], N'')))
) THEN
  UPDATE SET
    T.[vendor_code] = S.[vendor_code],
    T.[vendor_short_name] = S.[vendor_short_name],
    T.[supplier_code] = S.[supplier_code],
    T.[supplier_short_name] = S.[supplier_short_name],
    T.[material_type] = S.[material_type],
    T.[material_group] = S.[material_group],
    T.[material_description] = S.[material_description],
    T.[manufacturer_material_code] = S.[manufacturer_material_code],
    T.[manufacturer_material_description] = S.[manufacturer_material_description],
    T.[manufacturer_material_specification] = S.[manufacturer_material_specification],
    T.[ext_field] = S.[ext_field],
    T.[remark] = S.[remark],
    T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    T.[deleted_at] = CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT (
    [id],[vendor_code],[vendor_short_name],[supplier_code],[supplier_short_name],
    [material_type],[material_group],[internal_material_code],[material_code],[material_description],
    [manufacturer_material_code],[manufacturer_material_description],[manufacturer_material_specification],[tenant_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[vendor_code],S.[vendor_short_name],S.[supplier_code],S.[supplier_short_name],
    S.[material_type],S.[material_group],S.[internal_material_code],S.[material_code],S.[material_description],
    S.[manufacturer_material_code],S.[manufacturer_material_description],S.[manufacturer_material_specification],S.[tenant_code],S.[ext_field],S.[remark],
    S.[updated_by],@now,S.[updated_by],@now,
    S.[is_deleted],
    CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[internal_material_code],
  INSERTED.[material_code],
  INSERTED.[manufacturer_material_code],
  INSERTED.[tenant_code],
  INSERTED.[updated_by],
  DELETED.[manufacturer_material_code], INSERTED.[manufacturer_material_code],
  DELETED.[material_description], INSERTED.[material_description],
  DELETED.[supplier_code], INSERTED.[supplier_code]
INTO #delta(
  rn, oper_type, id, internal_material_code, material_code, manufacturer_material_code,
  tenant_code, change_by,
  manufacturer_material_code_old, manufacturer_material_code_new,
  material_description_old, material_description_new,
  supplier_code_old, supplier_code_new
);

IF OBJECT_ID('tempdb..#soft_deleted_rows') IS NOT NULL DROP TABLE #soft_deleted_rows;
CREATE TABLE #soft_deleted_rows (
  [id] BIGINT,
  [internal_material_code] NVARCHAR(20),
  [material_code] NVARCHAR(20)
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
  INSERTED.[internal_material_code],
  INSERTED.[material_code]
INTO #soft_deleted_rows ([id], [internal_material_code], [material_code])
FROM [takt_logistics_procurement_manufacturer_material] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[internal_material_code] = LTRIM(RTRIM(T.[internal_material_code]))
      AND S.[material_code] = LTRIM(RTRIM(T.[material_code]))
  );

DECLARE @delete_count INT = @@ROWCOUNT;
DECLARE @soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    ISNULL([internal_material_code], N''), N'/',
    ISNULL([material_code], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
IF LEN(@soft_deleted_keys) > 2000
BEGIN
  SET @soft_deleted_keys = LEFT(@soft_deleted_keys, 2000) + N'...(+more)';
END;
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_manufacturer_material]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_manufacturer_material]
  WHERE [tenant_code] = @tenant_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_manufacturer_material]
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
  [ext_field],[remark],[created_by],[created_at]
)
SELECT
  @base_id + d.rn,
  d.oper_type,
  N'takt_logistics_procurement_manufacturer_material',
  d.id,
  ISNULL((
    SELECT
      d.manufacturer_material_code_old AS [manufacturer_material_code],
      d.material_description_old AS [material_description],
      d.supplier_code_old AS [supplier_code]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  (
    SELECT
      d.manufacturer_material_code_new AS [manufacturer_material_code],
      d.material_description_new AS [material_description],
      d.supplier_code_new AS [supplier_code]
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  ISNULL((
    SELECT
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(d.manufacturer_material_code_old, 'null') END AS [manufacturer_material_code.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(d.manufacturer_material_code_new, 'null') END AS [manufacturer_material_code.new],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(d.material_description_old, 'null') END AS [material_description.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(d.material_description_new, 'null') END AS [material_description.new]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  N'MERGE ManufacturerMaterial Sync',
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,0,
  d.tenant_code,@company_code,'{}',N'SYNC',d.change_by,@now
FROM #delta d;

DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"sap_raw":' + CAST(@sap_raw_count AS NVARCHAR)
  + N',"excluded_paren":' + CAST(@sap_excluded_paren AS NVARCHAR)
  + N',"sap_eligible":' + CAST(@sap_eligible_count AS NVARCHAR)
  + N',"sap_keys":' + CAST(@sap_key_count AS NVARCHAR)
  + N',"dedupe_dropped":' + CAST(@dedupe_dropped AS NVARCHAR)
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
  @base_id + 1,
  N'SYSTEM_SYNC',
  N'SYNC',
  N'制造商物料',
  N'exec_sql_merge',
  'SQL',
  N'/sync/manufacturer-material',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @sap_raw_count AS [source_raw_count],
  @sap_excluded_paren AS [excluded_paren],
  @sap_eligible_count AS [sap_eligible_count],
  @sap_key_count AS [sap_key_count],
  @dedupe_dropped AS [dedupe_dropped],
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
