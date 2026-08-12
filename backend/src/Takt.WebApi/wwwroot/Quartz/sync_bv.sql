SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体 TaktBomMaterialCost 业务字段一致
-- {{SourceDatabase}}.dbo.takt_logistics_manufacturing_bom_material_cost → 当前租户库同名表
-- 业务唯一键：Plant+Model+Product+CostingPeriod
-- 流程：①源表装入（含 is_deleted 原样）→ ②空物料类型用 TaktGeneralMaterial 回填
--       → ③空机种用 TaktModelDestination 回填 → MERGE（is_deleted 0/1 原样）→ 机种月均

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [model_code] NVARCHAR(40),
  [model_monthly_average_cost] DECIMAL(18,5),
  [material_type] NVARCHAR(4),
  [product_code] NVARCHAR(20),
  [product_description] NVARCHAR(40),
  [product_monthly_cost] DECIMAL(18,5),
  [currency_code] NVARCHAR(3),
  [costing_period] NVARCHAR(7),
  [costing_date] DATETIME,
  [is_deleted] INT,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [updated_by] BIGINT
);

-- 源表装入 + 主数据回填：
-- 1) 先按源表同步业务字段（含 is_deleted 原样 0/1；plant/product 必填；model/material_type 可空）
--    ❌ 禁止把源 is_deleted=1 改成 0；❌ 禁止漏装已软删源行
-- 2) 物料类型：源空时按 ProductCode → TaktGeneralMaterial.MaterialType 回填；仍空才默认 FERT
-- 3) 机种编码：源空时按 ProductCode → TaktModelDestination.ModelCode 回填（18 位纯数字截末 10）
-- 4) 回填后仍无机种的行跳过
-- 5) costing_period 空则由 costing_date 推导 yyyy-MM
-- 6) 按唯一键 Plant+Model+Product+CostingPeriod 去重（同键优先保留 is_deleted=0）
-- 前置：须已同步全局物料（QT_SYNC_MAT）与机种目的地（QT_SYNC_MDL）
IF OBJECT_ID('tempdb..#st_enriched') IS NOT NULL DROP TABLE #st_enriched;
CREATE TABLE #st_enriched (
  [plant_code] NVARCHAR(4),
  [source_model_code] NVARCHAR(40),
  [model_code] NVARCHAR(40),
  [source_material_type] NVARCHAR(4),
  [material_type] NVARCHAR(4),
  [model_monthly_average_cost] DECIMAL(18,5),
  [product_code] NVARCHAR(20),
  [product_description] NVARCHAR(40),
  [product_monthly_cost] DECIMAL(18,5),
  [currency_code] NVARCHAR(3),
  [costing_period] NVARCHAR(7),
  [costing_date] DATETIME,
  [is_deleted] INT,
  [was_model_backfilled] BIT,
  [was_material_type_backfilled] BIT
);

INSERT INTO #st_enriched
SELECT
  B.[plant_code],
  B.[source_model_code],
  COALESCE(NULLIF(B.[source_model_code], N''), NULLIF(D.[model_code], N''), N'') AS [model_code],
  B.[source_material_type],
  COALESCE(
    NULLIF(B.[source_material_type], N''),
    NULLIF(G.[material_type], N''),
    N'FERT'
  ) AS [material_type],
  B.[model_monthly_average_cost],
  B.[product_code],
  B.[product_description],
  B.[product_monthly_cost],
  B.[currency_code],
  B.[costing_period],
  B.[costing_date],
  B.[is_deleted],
  CASE
    WHEN NULLIF(B.[source_model_code], N'') IS NULL
      AND NULLIF(D.[model_code], N'') IS NOT NULL
    THEN 1
    ELSE 0
  END AS [was_model_backfilled],
  CASE
    WHEN NULLIF(B.[source_material_type], N'') IS NULL
      AND NULLIF(G.[material_type], N'') IS NOT NULL
    THEN 1
    ELSE 0
  END AS [was_material_type_backfilled]
FROM (
  SELECT
    LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[model_code])), N''), N'') AS [source_model_code],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[material_type])), N''), N'') AS [source_material_type],
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
    CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted]
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_bom_material_cost] R
  WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[product_code], N''))) <> N''
) B
-- 空机种：ProductCode → TaktModelDestination.ModelCode
LEFT JOIN (
  SELECT
    CASE
      WHEN LEN(LTRIM(RTRIM(M.[material_code]))) = 18
        AND LTRIM(RTRIM(M.[material_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(M.[material_code])), 10)
      ELSE LTRIM(RTRIM(M.[material_code]))
    END AS [material_key],
    LTRIM(RTRIM(M.[model_code])) AS [model_code],
    ROW_NUMBER() OVER (
      PARTITION BY
        CASE
          WHEN LEN(LTRIM(RTRIM(M.[material_code]))) = 18
            AND LTRIM(RTRIM(M.[material_code])) NOT LIKE '%[^0-9]%'
          THEN RIGHT(LTRIM(RTRIM(M.[material_code])), 10)
          ELSE LTRIM(RTRIM(M.[material_code]))
        END
      ORDER BY M.[sort_order], M.[id]
    ) AS rn
  FROM [takt_logistics_materials_model_destination] M
  WHERE M.[tenant_code] = @tenant_code
    AND ISNULL(M.[is_deleted], 0) = 0
    AND LTRIM(RTRIM(ISNULL(M.[material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(M.[model_code], N''))) <> N''
) D ON D.[material_key] = B.[product_code] AND D.rn = 1
-- 空物料类型：ProductCode → TaktGeneralMaterial.MaterialType（❌ 非 MaterialPlant）
LEFT JOIN (
  SELECT
    CASE
      WHEN LEN(LTRIM(RTRIM(GM.[material_code]))) = 18
        AND LTRIM(RTRIM(GM.[material_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(GM.[material_code])), 10)
      ELSE LTRIM(RTRIM(GM.[material_code]))
    END AS [material_key],
    LTRIM(RTRIM(GM.[material_type])) AS [material_type],
    ROW_NUMBER() OVER (
      PARTITION BY
        CASE
          WHEN LEN(LTRIM(RTRIM(GM.[material_code]))) = 18
            AND LTRIM(RTRIM(GM.[material_code])) NOT LIKE '%[^0-9]%'
          THEN RIGHT(LTRIM(RTRIM(GM.[material_code])), 10)
          ELSE LTRIM(RTRIM(GM.[material_code]))
        END
      ORDER BY GM.[id]
    ) AS rn
  FROM [takt_logistics_materials_general_material] GM
  WHERE GM.[tenant_code] = @tenant_code
    AND ISNULL(GM.[is_deleted], 0) = 0
    AND LTRIM(RTRIM(ISNULL(GM.[material_code], N''))) <> N''
) G ON G.[material_key] = B.[product_code] AND G.rn = 1;

INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[model_code],
  CAST(0 AS DECIMAL(18,5)),
  S.[material_type],
  S.[product_code],
  S.[product_description],
  S.[product_monthly_cost],
  S.[currency_code],
  S.[costing_period],
  S.[costing_date],
  S.[is_deleted],
  @tenant_code,
  @company_code,
  @culture_code,
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
      E.[plant_code],
      E.[model_code],
      E.[material_type],
      E.[product_code],
      E.[product_description],
      E.[product_monthly_cost],
      E.[currency_code],
      E.[costing_period],
      E.[costing_date],
      E.[is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          E.[plant_code],
          E.[model_code],
          E.[product_code],
          E.[costing_period]
        ORDER BY
          E.[is_deleted] ASC,
          E.[product_monthly_cost] DESC,
          E.[costing_date] DESC
      ) AS dup_rn
    FROM #st_enriched E
    WHERE NULLIF(E.[model_code], N'') IS NOT NULL
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #st_source);
DECLARE @table_total INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_bom_material_cost]
);
DECLARE @source_deleted_count INT = (
  SELECT COUNT(*)
  FROM #st_source
  WHERE [is_deleted] = 1
);
DECLARE @plant_product_count INT = (SELECT COUNT(*) FROM #st_enriched);
DECLARE @model_backfilled INT = (
  SELECT COUNT(*)
  FROM #st_enriched
  WHERE [was_model_backfilled] = 1
);
DECLARE @material_type_backfilled INT = (
  SELECT COUNT(*)
  FROM #st_enriched
  WHERE [was_material_type_backfilled] = 1
);
DECLARE @skipped_no_model INT = (
  SELECT COUNT(*)
  FROM #st_enriched
  WHERE NULLIF([model_code], N'') IS NULL
);
DECLARE @skipped_empty INT = (@table_total - @plant_product_count) + @skipped_no_model;
DECLARE @sap_raw_count INT = @plant_product_count - @skipped_no_model;
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      E.[plant_code],
      E.[model_code],
      E.[product_code],
      E.[costing_period]
    FROM #st_enriched E
    WHERE NULLIF(E.[model_code], N'') IS NOT NULL
    GROUP BY
      E.[plant_code],
      E.[model_code],
      E.[product_code],
      E.[costing_period]
  ) K
);
DECLARE @dedupe_dropped INT = @sap_raw_count - @sap_key_count;

IF @batch_size = 0 AND @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', sap_raw=', @sap_raw_count, N', dedupe_dropped=', @dedupe_dropped,
    N', model_backfilled=', @model_backfilled,
    N', material_type_backfilled=', @material_type_backfilled);
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
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[culture_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[culture_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[material_type], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_type], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[product_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[product_description], N'')))
  OR ROUND(T.[product_monthly_cost], 5) <> ROUND(S.[product_monthly_cost], 5)
  OR LTRIM(RTRIM(ISNULL(T.[currency_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[currency_code], N'')))
  OR T.[costing_date] <> S.[costing_date]
) THEN
  UPDATE SET
    T.[material_type] = S.[material_type],
    T.[product_description] = S.[product_description],
    T.[product_monthly_cost] = S.[product_monthly_cost],
    T.[currency_code] = S.[currency_code],
    T.[costing_date] = S.[costing_date],
    T.[culture_code] = S.[culture_code],
    T.[ext_field] = S.[ext_field],
    T.[remark] = S.[remark],
        T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    T.[deleted_at] = CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[model_code],[model_monthly_average_cost],[material_type],
    [product_code],[product_description],[product_monthly_cost],
    [currency_code],[costing_period],[costing_date],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[model_code],S.[model_monthly_average_cost],S.[material_type],
    S.[product_code],S.[product_description],S.[product_monthly_cost],
    S.[currency_code],S.[costing_period],S.[costing_date],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
    S.[updated_by],@now,S.[updated_by],@now,
    S.[is_deleted],
    CASE WHEN S.[is_deleted] = 1 THEN S.[updated_by] ELSE NULL END,
    CASE WHEN S.[is_deleted] = 1 THEN @now ELSE NULL END
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

-- 按 工厂 + 物料类型 + 机种 + 核算期间 重算机种月均（产品月成本>0 的算术平均）
;WITH avg_src AS (
  SELECT
    LTRIM(RTRIM([plant_code])) AS [plant_code],
    LTRIM(RTRIM([material_type])) AS [material_type],
    LTRIM(RTRIM([model_code])) AS [model_code],
    LTRIM(RTRIM([costing_period])) AS [costing_period],
    ROUND(AVG(CASE WHEN [product_monthly_cost] > 0 THEN [product_monthly_cost] END), 5) AS [avg_cost]
  FROM [takt_logistics_manufacturing_bom_material_cost]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
    AND NULLIF(LTRIM(RTRIM([model_code])), N'') IS NOT NULL
    AND NULLIF(LTRIM(RTRIM([material_type])), N'') IS NOT NULL
  GROUP BY
    LTRIM(RTRIM([plant_code])),
    LTRIM(RTRIM([material_type])),
    LTRIM(RTRIM([model_code])),
    LTRIM(RTRIM([costing_period]))
)
UPDATE T
SET
  T.[model_monthly_average_cost] = ISNULL(A.[avg_cost], 0),
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
FROM [takt_logistics_manufacturing_bom_material_cost] T
INNER JOIN avg_src A
  ON LTRIM(RTRIM(T.[plant_code])) = A.[plant_code]
  AND LTRIM(RTRIM(T.[material_type])) = A.[material_type]
  AND LTRIM(RTRIM(T.[model_code])) = A.[model_code]
  AND LTRIM(RTRIM(T.[costing_period])) = A.[costing_period]
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND ROUND(T.[model_monthly_average_cost], 5) <> ROUND(ISNULL(A.[avg_cost], 0), 5);

DECLARE @average_updated INT = @@ROWCOUNT;

DECLARE @soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    ISNULL([plant_code], N''), N'/',
    ISNULL([model_code], N''), N'/',
    ISNULL([product_code], N''), N'/',
    ISNULL([costing_period], N'')
  )
  AS NVARCHAR(MAX)),
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
DECLARE @source_active_count INT = (
  SELECT COUNT(*)
  FROM #st_source
  WHERE [is_deleted] = 0
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

-- 有效行：源 is_deleted=0 ↔ 目标 is_deleted=0（❌ 勿用含软删的 @source_count 对比 active）
IF @target_count <> @source_active_count
BEGIN
  DECLARE @count_msg NVARCHAR(300) = CONCAT(
    N'有效行数不一致: source_active=', @source_active_count,
    N', active=', @target_count,
    N', source_total=', @source_count,
    N', source_deleted=', @source_deleted_count);
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
  N'MERGE BomMaterialCost Sync',
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,0,
  d.tenant_code,d.company_code,'{}',N'SYNC',d.change_by,@now
FROM #delta d;

DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"table_total":' + CAST(@table_total AS NVARCHAR)
  + N',"sap_raw":' + CAST(@sap_raw_count AS NVARCHAR)
  + N',"skipped_empty":' + CAST(@skipped_empty AS NVARCHAR)
  + N',"skipped_no_model":' + CAST(@skipped_no_model AS NVARCHAR)
  + N',"model_backfilled":' + CAST(@model_backfilled AS NVARCHAR)
  + N',"material_type_backfilled":' + CAST(@material_type_backfilled AS NVARCHAR)
  + N',"source_deleted":' + CAST(@source_deleted_count AS NVARCHAR)
  + N',"average_updated":' + CAST(@average_updated AS NVARCHAR)
  + N',"source":' + CAST(@source_count AS NVARCHAR)
  + N',"source_active":' + CAST(@source_active_count AS NVARCHAR)
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
  N'SYSTEM_SYNC',
  N'SYNC',
  N'BOM物料成本汇总',
  N'exec_sql_merge',
  'SQL',
  N'/sync/bom-material-cost',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @table_total AS [source_raw_count],
  @source_count AS [source_count],
  @source_active_count AS [source_active_count],
  @skipped_empty AS [skipped_empty_count],
  @skipped_no_model AS [skipped_no_model_count],
  @model_backfilled AS [model_backfilled_count],
  @material_type_backfilled AS [material_type_backfilled_count],
  @source_deleted_count AS [source_deleted_count],
  @average_updated AS [average_updated_count],
  @dedupe_dropped AS [dedupe_dropped],
  @sap_raw_count AS [sap_raw_count],
  @sap_key_count AS [sap_key_count],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @target_physical AS [target_physical],
  @soft_deleted AS [soft_deleted],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count],
  @soft_deleted_keys AS [soft_deleted_keys];
