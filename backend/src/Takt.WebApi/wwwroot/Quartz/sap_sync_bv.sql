SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体 TaktBomMaterialCost 业务字段一致
-- Sap_Data.dbo.takt_logistics_manufacturing_bom_material_cost → 当前租户库同名表
-- 业务唯一键：Plant+Model+Product+CostingPeriod

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [model_code] NVARCHAR(40),
  [model_monthly_average_cost] DECIMAL(18,5),
  [product_code] NVARCHAR(20),
  [product_description] NVARCHAR(40),
  [product_monthly_cost] DECIMAL(18,5),
  [currency_code] NVARCHAR(3),
  [costing_period] NVARCHAR(7),
  [costing_date] DATETIME,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [ext_field_json] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [updated_by] BIGINT
);

-- 源表装入：
-- 1) 18 位纯数字产品截末 10 位
-- 2) costing_period 空则由 costing_date 推导 yyyy-MM
-- 3) 按唯一键去重（产品月成本 → 机种月均成本 较大者优先）
INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[model_code],
  S.[model_monthly_average_cost],
  S.[product_code],
  S.[product_description],
  S.[product_monthly_cost],
  S.[currency_code],
  S.[costing_period],
  S.[costing_date],
  @tenant_code,
  @company_code,
  '{}',
  '',
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY
        N.[plant_code], N.[model_code], N.[product_code], N.[costing_period]
    ) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LTRIM(RTRIM(R.[model_code])) AS [model_code],
      ROUND(COALESCE(TRY_CAST(R.[model_monthly_average_cost] AS DECIMAL(18,8)), 0), 5) AS [model_monthly_average_cost],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
          AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
        ELSE LTRIM(RTRIM(R.[product_code]))
      END AS [product_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[product_description])), ''), '') AS [product_description],
      ROUND(COALESCE(TRY_CAST(R.[product_monthly_cost] AS DECIMAL(18,8)), 0), 5) AS [product_monthly_cost],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[currency_code])), ''), '') AS [currency_code],
      ISNULL(
        NULLIF(LTRIM(RTRIM(R.[costing_period])), ''),
        CONVERT(NVARCHAR(7), COALESCE(TRY_CAST(R.[costing_date] AS DATETIME), @now), 126)
      ) AS [costing_period],
      COALESCE(TRY_CAST(R.[costing_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS [costing_date],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(R.[plant_code])),
          LTRIM(RTRIM(R.[model_code])),
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
              AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
            ELSE LTRIM(RTRIM(R.[product_code]))
          END,
          ISNULL(
            NULLIF(LTRIM(RTRIM(R.[costing_period])), ''),
            CONVERT(NVARCHAR(7), COALESCE(TRY_CAST(R.[costing_date] AS DATETIME), @now), 126)
          )
        ORDER BY
          ROUND(COALESCE(TRY_CAST(R.[product_monthly_cost] AS DECIMAL(18,8)), 0), 5) DESC,
          ROUND(COALESCE(TRY_CAST(R.[model_monthly_average_cost] AS DECIMAL(18,8)), 0), 5) DESC,
          COALESCE(TRY_CAST(R.[costing_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) DESC
      ) AS dup_rn
    FROM [Sap_Data].[dbo].[takt_logistics_manufacturing_bom_material_cost] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[model_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[product_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #st_source);
DECLARE @sap_raw_count INT = (
  SELECT COUNT(*)
  FROM [Sap_Data].[dbo].[takt_logistics_manufacturing_bom_material_cost] R
  WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[model_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[product_code], N''))) <> N''
);
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LTRIM(RTRIM(R.[model_code])) AS [model_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
          AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
        ELSE LTRIM(RTRIM(R.[product_code]))
      END AS [product_code],
      ISNULL(
        NULLIF(LTRIM(RTRIM(R.[costing_period])), ''),
        CONVERT(NVARCHAR(7), COALESCE(TRY_CAST(R.[costing_date] AS DATETIME), @now), 126)
      ) AS [costing_period]
    FROM [Sap_Data].[dbo].[takt_logistics_manufacturing_bom_material_cost] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[model_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[product_code], N''))) <> N''
    GROUP BY
      LTRIM(RTRIM(R.[plant_code])),
      LTRIM(RTRIM(R.[model_code])),
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
          AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
        ELSE LTRIM(RTRIM(R.[product_code]))
      END,
      ISNULL(
        NULLIF(LTRIM(RTRIM(R.[costing_period])), ''),
        CONVERT(NVARCHAR(7), COALESCE(TRY_CAST(R.[costing_date] AS DATETIME), @now), 126)
      )
  ) K
);
DECLARE @dedupe_dropped INT = @sap_raw_count - @sap_key_count;

IF @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', sap_raw=', @sap_raw_count, N', dedupe_dropped=', @dedupe_dropped);
  THROW 50003, @src_msg, 1;
END;

IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY [plant_code], [model_code], [product_code], [costing_period]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(800);
  SELECT TOP 1
    @dup_key = CONCAT(
      [plant_code], N' / ', [model_code], N' / ', [product_code], N' / ',
      [costing_period], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [plant_code], [model_code], [product_code], [costing_period]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(4),
  model_code NVARCHAR(40),
  product_code NVARCHAR(20),
  costing_period NVARCHAR(7),
  tenant_code NVARCHAR(3),
  company_code NVARCHAR(4),
  change_by BIGINT,
  model_monthly_average_cost_old DECIMAL(18,5),
  model_monthly_average_cost_new DECIMAL(18,5),
  product_monthly_cost_old DECIMAL(18,5),
  product_monthly_cost_new DECIMAL(18,5),
  currency_code_old NVARCHAR(3),
  currency_code_new NVARCHAR(3),
  costing_date_old DATETIME,
  costing_date_new DATETIME
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_manufacturing_bom_material_cost] AS T
USING #st_source AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
AND LTRIM(RTRIM(T.[model_code])) = S.[model_code]
AND LTRIM(RTRIM(T.[product_code])) = S.[product_code]
AND LTRIM(RTRIM(T.[costing_period])) = S.[costing_period]
WHEN MATCHED AND (
  T.[is_deleted] <> 0
  OR ROUND(T.[model_monthly_average_cost], 5) <> ROUND(S.[model_monthly_average_cost], 5)
  OR LTRIM(RTRIM(ISNULL(T.[product_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[product_description], N'')))
  OR ROUND(T.[product_monthly_cost], 5) <> ROUND(S.[product_monthly_cost], 5)
  OR LTRIM(RTRIM(ISNULL(T.[currency_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[currency_code], N'')))
  OR T.[costing_date] <> S.[costing_date]
) THEN
  UPDATE SET
    T.[model_monthly_average_cost] = S.[model_monthly_average_cost],
    T.[product_description] = S.[product_description],
    T.[product_monthly_cost] = S.[product_monthly_cost],
    T.[currency_code] = S.[currency_code],
    T.[costing_date] = S.[costing_date],
    T.[ext_field_json] = S.[ext_field_json],
    T.[remark] = S.[remark],
    T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = 0,
    T.[deleted_by] = NULL,
    T.[deleted_at] = NULL
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[model_code],[model_monthly_average_cost],
    [product_code],[product_description],[product_monthly_cost],
    [currency_code],[costing_period],[costing_date],
    [tenant_code],[company_code],[ext_field_json],[remark],
    [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
  )
  VALUES (
    S.[id],S.[plant_code],S.[model_code],S.[model_monthly_average_cost],
    S.[product_code],S.[product_description],S.[product_monthly_cost],
    S.[currency_code],S.[costing_period],S.[costing_date],
    S.[tenant_code],S.[company_code],S.[ext_field_json],S.[remark],
    S.[updated_by],@now,S.[updated_by],@now,0
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[model_code],
  INSERTED.[product_code],
  INSERTED.[costing_period],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  INSERTED.[updated_by],
  DELETED.[model_monthly_average_cost], INSERTED.[model_monthly_average_cost],
  DELETED.[product_monthly_cost], INSERTED.[product_monthly_cost],
  DELETED.[currency_code], INSERTED.[currency_code],
  DELETED.[costing_date], INSERTED.[costing_date]
INTO #delta(
  rn, oper_type, id, plant_code, model_code, product_code, costing_period,
  tenant_code, company_code, change_by,
  model_monthly_average_cost_old, model_monthly_average_cost_new,
  product_monthly_cost_old, product_monthly_cost_new,
  currency_code_old, currency_code_new,
  costing_date_old, costing_date_new
);

IF OBJECT_ID('tempdb..#soft_deleted_rows') IS NOT NULL DROP TABLE #soft_deleted_rows;
CREATE TABLE #soft_deleted_rows (
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [model_code] NVARCHAR(40),
  [product_code] NVARCHAR(20),
  [costing_period] NVARCHAR(7)
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
  INSERTED.[model_code],
  INSERTED.[product_code],
  INSERTED.[costing_period]
INTO #soft_deleted_rows ([id], [plant_code], [model_code], [product_code], [costing_period])
FROM [takt_logistics_manufacturing_bom_material_cost] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[model_code] = LTRIM(RTRIM(T.[model_code]))
      AND S.[product_code] = LTRIM(RTRIM(T.[product_code]))
      AND S.[costing_period] = LTRIM(RTRIM(T.[costing_period]))
  );

DECLARE @delete_count INT = @@ROWCOUNT;
DECLARE @soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @soft_deleted_keys = STRING_AGG(
  CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    ISNULL([plant_code], N''), N'/',
    ISNULL([model_code], N''), N'/',
    ISNULL([product_code], N''), N'/',
    ISNULL([costing_period], N'')
  ),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost]
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
  [ext_field_json],[remark],[created_by],[created_at]
)
SELECT
  @base_id + d.rn,
  d.oper_type,
  N'takt_logistics_manufacturing_bom_material_cost',
  d.id,
  ISNULL((
    SELECT
      d.model_monthly_average_cost_old AS [model_monthly_average_cost],
      d.product_monthly_cost_old AS [product_monthly_cost],
      d.currency_code_old AS [currency_code]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  (
    SELECT
      d.model_monthly_average_cost_new AS [model_monthly_average_cost],
      d.product_monthly_cost_new AS [product_monthly_cost],
      d.currency_code_new AS [currency_code]
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  ISNULL((
    SELECT
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.model_monthly_average_cost_old AS NVARCHAR), 'null') END AS [model_monthly_average_cost.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.model_monthly_average_cost_new AS NVARCHAR), 'null') END AS [model_monthly_average_cost.new],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.product_monthly_cost_old AS NVARCHAR), 'null') END AS [product_monthly_cost.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.product_monthly_cost_new AS NVARCHAR), 'null') END AS [product_monthly_cost.new]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  N'MERGE SAP BomMaterialCost Sync',
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,0,
  d.tenant_code,d.company_code,'{}',N'SAP_SYNC',d.change_by,@now
FROM #delta d;

DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"sap_raw":' + CAST(@sap_raw_count AS NVARCHAR)
  + N',"source":' + CAST(@source_count AS NVARCHAR)
  + N',"sap_keys":' + CAST(@sap_key_count AS NVARCHAR)
  + N',"dedupe_dropped":' + CAST(@dedupe_dropped AS NVARCHAR)
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
  N'SYSTEM_SAP_SYNC',
  N'SAP_SYNC',
  N'BOM物料成本汇总',
  N'exec_sql_merge',
  'SQL',
  N'/sync/sap/bom-material-cost',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @sap_raw_count AS [sap_raw_count],
  @source_count AS [source_count],
  @sap_key_count AS [sap_key_count],
  @dedupe_dropped AS [dedupe_dropped],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @target_physical AS [target_physical],
  @soft_deleted AS [soft_deleted],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count],
  @soft_deleted_keys AS [soft_deleted_keys];
