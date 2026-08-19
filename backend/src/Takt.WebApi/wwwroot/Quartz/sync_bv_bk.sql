SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};
DECLARE @now DATETIME = GETDATE();
DECLARE @model_updated INT = 0;
DECLARE @type_updated INT = 0;
DECLARE @model_skipped_clash INT = 0;

-- =============================================================================
-- QT_SYNC_BV_BK：BOM 物料成本主表回填（❌ 仅空字段，禁止全局覆盖）
-- 目标：takt_logistics_manufacturing_bom_material_cost
-- 机种 model_code：仅当 LTRIM(ISNULL(model_code,''))='' 时写入
-- 物料类型 material_type：仅当 LTRIM(ISNULL(material_type,''))='' 时写入
--   （已有值含 FERT/HALB/ROH 等一律保留，❌ 不按主数据全局改写）
-- 来源：model_destination；material_type=general_material 优先，其次同工厂 material_plant
-- 物料码匹配：trim + 18 位纯数字截末 10（对齐 TaktStringHelper.NormalizeSapNumericMaterialCode）
-- 改机种若撞唯一键则跳过；主数据无匹配则跳过
-- 本次写入合并到 ext_field：$._bk.bv = { at, model_code?, material_type? }
-- 建议：在 QT_SYNC_MAT / MATPLT / MDL / BV 之后执行
-- =============================================================================

IF OBJECT_ID('tempdb..#mat_key') IS NOT NULL DROP TABLE #mat_key;
IF OBJECT_ID('tempdb..#mdl') IS NOT NULL DROP TABLE #mdl;
IF OBJECT_ID('tempdb..#gmt') IS NOT NULL DROP TABLE #gmt;
IF OBJECT_ID('tempdb..#mpt') IS NOT NULL DROP TABLE #mpt;

-- 物料码归一化键（18 位纯数字 → 末 10 位，否则 trim）
;WITH keys AS (
  SELECT DISTINCT
    CASE
      WHEN LEN(LTRIM(RTRIM(t.[product_code]))) = 18
        AND LTRIM(RTRIM(t.[product_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(t.[product_code])), 10)
      ELSE LTRIM(RTRIM(t.[product_code]))
    END AS [mat_key]
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost] AS t
  WHERE t.[tenant_code] = @tenant_code
    AND t.[company_code] = @company_code
    AND t.[is_deleted] = 0
    AND LTRIM(RTRIM(ISNULL(t.[product_code], N''))) <> N''
    AND (
      LTRIM(RTRIM(ISNULL(t.[model_code], N''))) = N''
      OR LTRIM(RTRIM(ISNULL(t.[material_type], N''))) = N''
    )
)
SELECT [mat_key]
INTO #mat_key
FROM keys
WHERE [mat_key] IS NOT NULL AND [mat_key] <> N'';

-- 机种：同一物料码多行时取 sort_order 最小、再 id 最小（稳定）
;WITH src AS (
  SELECT
    CASE
      WHEN LEN(LTRIM(RTRIM(d.[material_code]))) = 18
        AND LTRIM(RTRIM(d.[material_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(d.[material_code])), 10)
      ELSE LTRIM(RTRIM(d.[material_code]))
    END AS [mat_key],
    LEFT(LTRIM(RTRIM(d.[model_code])), 40) AS [model_code],
    ROW_NUMBER() OVER (
      PARTITION BY
        CASE
          WHEN LEN(LTRIM(RTRIM(d.[material_code]))) = 18
            AND LTRIM(RTRIM(d.[material_code])) NOT LIKE '%[^0-9]%'
          THEN RIGHT(LTRIM(RTRIM(d.[material_code])), 10)
          ELSE LTRIM(RTRIM(d.[material_code]))
        END
      ORDER BY d.[sort_order], d.[id]
    ) AS rn
  FROM [dbo].[takt_logistics_materials_model_destination] AS d
  WHERE d.[tenant_code] = @tenant_code
    AND d.[is_deleted] = 0
    AND LTRIM(RTRIM(ISNULL(d.[material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(d.[model_code], N''))) <> N''
)
SELECT [mat_key], [model_code]
INTO #mdl
FROM src
WHERE rn = 1
  AND EXISTS (SELECT 1 FROM #mat_key k WHERE k.[mat_key] = src.[mat_key]);

-- 通用物料类型（租户级）
;WITH src AS (
  SELECT
    CASE
      WHEN LEN(LTRIM(RTRIM(g.[material_code]))) = 18
        AND LTRIM(RTRIM(g.[material_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(g.[material_code])), 10)
      ELSE LTRIM(RTRIM(g.[material_code]))
    END AS [mat_key],
    LEFT(LTRIM(RTRIM(g.[material_type])), 4) AS [material_type],
    ROW_NUMBER() OVER (
      PARTITION BY
        CASE
          WHEN LEN(LTRIM(RTRIM(g.[material_code]))) = 18
            AND LTRIM(RTRIM(g.[material_code])) NOT LIKE '%[^0-9]%'
          THEN RIGHT(LTRIM(RTRIM(g.[material_code])), 10)
          ELSE LTRIM(RTRIM(g.[material_code]))
        END
      ORDER BY g.[id]
    ) AS rn
  FROM [dbo].[takt_logistics_materials_general_material] AS g
  WHERE g.[tenant_code] = @tenant_code
    AND g.[is_deleted] = 0
    AND LTRIM(RTRIM(ISNULL(g.[material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(g.[material_type], N''))) <> N''
)
SELECT [mat_key], [material_type]
INTO #gmt
FROM src
WHERE rn = 1
  AND EXISTS (SELECT 1 FROM #mat_key k WHERE k.[mat_key] = src.[mat_key]);

-- 工厂物料类型（公司 + 工厂 + 物料）
;WITH src AS (
  SELECT
    LTRIM(RTRIM(p.[plant_code])) AS [plant_code],
    CASE
      WHEN LEN(LTRIM(RTRIM(p.[material_code]))) = 18
        AND LTRIM(RTRIM(p.[material_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(p.[material_code])), 10)
      ELSE LTRIM(RTRIM(p.[material_code]))
    END AS [mat_key],
    LEFT(LTRIM(RTRIM(p.[material_type])), 4) AS [material_type],
    ROW_NUMBER() OVER (
      PARTITION BY
        LTRIM(RTRIM(p.[plant_code])),
        CASE
          WHEN LEN(LTRIM(RTRIM(p.[material_code]))) = 18
            AND LTRIM(RTRIM(p.[material_code])) NOT LIKE '%[^0-9]%'
          THEN RIGHT(LTRIM(RTRIM(p.[material_code])), 10)
          ELSE LTRIM(RTRIM(p.[material_code]))
        END
      ORDER BY p.[id]
    ) AS rn
  FROM [dbo].[takt_logistics_materials_material_plant] AS p
  WHERE p.[tenant_code] = @tenant_code
    AND p.[company_code] = @company_code
    AND p.[is_deleted] = 0
    AND LTRIM(RTRIM(ISNULL(p.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(p.[material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(p.[material_type], N''))) <> N''
)
SELECT [plant_code], [mat_key], [material_type]
INTO #mpt
FROM src
WHERE rn = 1
  AND EXISTS (SELECT 1 FROM #mat_key k WHERE k.[mat_key] = src.[mat_key]);

-- ---------- 仅回填空机种（已有 model_code 不改；撞唯一键则跳过） ----------
;WITH cand AS (
  SELECT
    t.[id],
    m.[model_code] AS [new_model_code],
    CASE
      WHEN EXISTS (
        SELECT 1
        FROM [dbo].[takt_logistics_manufacturing_bom_material_cost] AS x
        WHERE x.[tenant_code] = t.[tenant_code]
          AND x.[company_code] = t.[company_code]
          AND LTRIM(RTRIM(x.[plant_code])) = LTRIM(RTRIM(t.[plant_code]))
          AND LTRIM(RTRIM(x.[model_code])) = m.[model_code]
          AND LTRIM(RTRIM(x.[product_code])) = LTRIM(RTRIM(t.[product_code]))
          AND LTRIM(RTRIM(x.[costing_period])) = LTRIM(RTRIM(t.[costing_period]))
          AND x.[id] <> t.[id]
          AND x.[is_deleted] = 0
      ) THEN 1 ELSE 0
    END AS [clash]
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost] AS t
  INNER JOIN #mdl AS m
    ON m.[mat_key] = CASE
      WHEN LEN(LTRIM(RTRIM(t.[product_code]))) = 18
        AND LTRIM(RTRIM(t.[product_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(t.[product_code])), 10)
      ELSE LTRIM(RTRIM(t.[product_code]))
    END
  WHERE t.[tenant_code] = @tenant_code
    AND t.[company_code] = @company_code
    AND t.[is_deleted] = 0
    -- 仅空：禁止覆盖已有机种
    AND (t.[model_code] IS NULL OR LTRIM(RTRIM(t.[model_code])) = N'')
)
UPDATE t
SET
  t.[model_code] = c.[new_model_code],
  t.[ext_field] = LEFT(x.[new_ext], 4000),
  t.[updated_by] = @sync_user_id,
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_manufacturing_bom_material_cost] AS t
INNER JOIN cand AS c ON c.[id] = t.[id]
CROSS APPLY (
  SELECT
    CASE
      WHEN ISJSON(NULLIF(LTRIM(RTRIM(ISNULL(t.[ext_field], N''))), N'')) = 1
      THEN LTRIM(RTRIM(t.[ext_field]))
      ELSE N'{}'
    END AS [base_ext]
) AS e0
CROSS APPLY (
  SELECT
    CASE
      WHEN JSON_QUERY(e0.[base_ext], N'$._bk') IS NULL
      THEN JSON_MODIFY(e0.[base_ext], N'lax $._bk', JSON_QUERY(N'{}'))
      ELSE e0.[base_ext]
    END AS [with_bk]
) AS e1
CROSS APPLY (
  SELECT
    CASE
      WHEN JSON_QUERY(e1.[with_bk], N'$._bk.bv') IS NULL
      THEN JSON_MODIFY(e1.[with_bk], N'lax $._bk.bv', JSON_QUERY(N'{}'))
      ELSE e1.[with_bk]
    END AS [with_bv]
) AS e2
CROSS APPLY (
  SELECT
    JSON_MODIFY(
      JSON_MODIFY(e2.[with_bv], N'lax $._bk.bv.at', CONVERT(VARCHAR(19), @now, 126)),
      N'lax $._bk.bv.model_code',
      c.[new_model_code]) AS [new_ext]
) AS x
WHERE c.[clash] = 0
  AND (t.[model_code] IS NULL OR LTRIM(RTRIM(t.[model_code])) = N'');

SET @model_updated = @@ROWCOUNT;

SELECT @model_skipped_clash = COUNT(*)
FROM (
  SELECT
    t.[id]
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost] AS t
  INNER JOIN #mdl AS m
    ON m.[mat_key] = CASE
      WHEN LEN(LTRIM(RTRIM(t.[product_code]))) = 18
        AND LTRIM(RTRIM(t.[product_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(t.[product_code])), 10)
      ELSE LTRIM(RTRIM(t.[product_code]))
    END
  WHERE t.[tenant_code] = @tenant_code
    AND t.[company_code] = @company_code
    AND t.[is_deleted] = 0
    AND (t.[model_code] IS NULL OR LTRIM(RTRIM(t.[model_code])) = N'')
    AND EXISTS (
      SELECT 1
      FROM [dbo].[takt_logistics_manufacturing_bom_material_cost] AS x
      WHERE x.[tenant_code] = t.[tenant_code]
        AND x.[company_code] = t.[company_code]
        AND LTRIM(RTRIM(x.[plant_code])) = LTRIM(RTRIM(t.[plant_code]))
        AND LTRIM(RTRIM(x.[model_code])) = m.[model_code]
        AND LTRIM(RTRIM(x.[product_code])) = LTRIM(RTRIM(t.[product_code]))
        AND LTRIM(RTRIM(x.[costing_period])) = LTRIM(RTRIM(t.[costing_period]))
        AND x.[id] <> t.[id]
        AND x.[is_deleted] = 0
    )
) ClashRows;

-- ---------- 仅回填空物料类型（已有 material_type 不改；通用优先，其次工厂） ----------
UPDATE t
SET
  t.[material_type] = LEFT(LTRIM(RTRIM(COALESCE(g.[material_type], p.[material_type]))), 4),
  t.[ext_field] = LEFT(x.[new_ext], 4000),
  t.[updated_by] = @sync_user_id,
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_manufacturing_bom_material_cost] AS t
LEFT JOIN #gmt AS g
  ON g.[mat_key] = CASE
    WHEN LEN(LTRIM(RTRIM(t.[product_code]))) = 18
      AND LTRIM(RTRIM(t.[product_code])) NOT LIKE '%[^0-9]%'
    THEN RIGHT(LTRIM(RTRIM(t.[product_code])), 10)
    ELSE LTRIM(RTRIM(t.[product_code]))
  END
LEFT JOIN #mpt AS p
  ON p.[plant_code] = LTRIM(RTRIM(t.[plant_code]))
 AND p.[mat_key] = CASE
    WHEN LEN(LTRIM(RTRIM(t.[product_code]))) = 18
      AND LTRIM(RTRIM(t.[product_code])) NOT LIKE '%[^0-9]%'
    THEN RIGHT(LTRIM(RTRIM(t.[product_code])), 10)
    ELSE LTRIM(RTRIM(t.[product_code]))
  END
CROSS APPLY (
  SELECT
    CASE
      WHEN ISJSON(NULLIF(LTRIM(RTRIM(ISNULL(t.[ext_field], N''))), N'')) = 1
      THEN LTRIM(RTRIM(t.[ext_field]))
      ELSE N'{}'
    END AS [base_ext]
) AS e0
CROSS APPLY (
  SELECT
    CASE
      WHEN JSON_QUERY(e0.[base_ext], N'$._bk') IS NULL
      THEN JSON_MODIFY(e0.[base_ext], N'lax $._bk', JSON_QUERY(N'{}'))
      ELSE e0.[base_ext]
    END AS [with_bk]
) AS e1
CROSS APPLY (
  SELECT
    CASE
      WHEN JSON_QUERY(e1.[with_bk], N'$._bk.bv') IS NULL
      THEN JSON_MODIFY(e1.[with_bk], N'lax $._bk.bv', JSON_QUERY(N'{}'))
      ELSE e1.[with_bk]
    END AS [with_bv]
) AS e2
CROSS APPLY (
  SELECT
    JSON_MODIFY(
      JSON_MODIFY(e2.[with_bv], N'lax $._bk.bv.at', CONVERT(VARCHAR(19), @now, 126)),
      N'lax $._bk.bv.material_type',
      LEFT(LTRIM(RTRIM(COALESCE(g.[material_type], p.[material_type]))), 4)) AS [new_ext]
) AS x
WHERE t.[tenant_code] = @tenant_code
  AND t.[company_code] = @company_code
  AND t.[is_deleted] = 0
  -- 仅空：禁止覆盖已有物料类型（含 FERT/HALB 等）
  AND (t.[material_type] IS NULL OR LTRIM(RTRIM(t.[material_type])) = N'')
  AND LTRIM(RTRIM(ISNULL(COALESCE(g.[material_type], p.[material_type]), N''))) <> N'';

SET @type_updated = @@ROWCOUNT;

DROP TABLE #mat_key;
DROP TABLE #mdl;
DROP TABLE #gmt;
DROP TABLE #mpt;

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'bv_bk' AS NVARCHAR(40)) AS [scope],
  @tenant_code AS [tenant_code],
  @company_code AS [company_code],
  @model_updated AS [model_code_updated],
  @type_updated AS [material_type_updated],
  @model_skipped_clash AS [model_code_skipped_unique_clash],
  (@model_updated + @type_updated) AS [total_updated_rows];
