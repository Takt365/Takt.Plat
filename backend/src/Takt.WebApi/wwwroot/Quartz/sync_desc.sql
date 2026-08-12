SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @now DATETIME = GETDATE();
DECLARE @sales_updated INT = 0;
DECLARE @purchase_updated INT = 0;
DECLARE @model_desc_updated INT = 0;
DECLARE @model_name_updated INT = 0;

-- =============================================================================
-- QT_SYNC_DESC：回填采销价格 / 机种目的地描述（仅空字段）
-- 来源：takt_logistics_materials_material_description
--   采销 MaterialDescription：优先 zh-CN，其次 Z1，再次 ja-JP（按 material_code，Length=40）
--   机种 MaterialDescription：仅 ja-JP（按 material_code，Length=40）——不要用 Z1
--   机种 ModelName：仅 Z1（按 model_code 匹配描述表 material_code，Length=80）——不要用 ja-JP
-- 规则：目标字段已有值则跳过；无可用描述则跳过
-- 建议：在 QT_SYNC_PUP / QT_SYNC_SP / QT_SYNC_MDL 之后执行
-- =============================================================================

IF OBJECT_ID('tempdb..#mat_desc') IS NOT NULL DROP TABLE #mat_desc;

SELECT
  LTRIM(RTRIM(md.[material_code])) AS [material_code],
  MAX(CASE WHEN md.[culture_code] = N'zh-CN' THEN md.[material_description] END) AS [zh_desc],
  MAX(CASE WHEN md.[culture_code] = N'Z1' THEN md.[material_description] END) AS [z1_desc],
  MAX(CASE WHEN md.[culture_code] = N'ja-JP' THEN md.[material_description] END) AS [ja_desc]
INTO #mat_desc
FROM [dbo].[takt_logistics_materials_material_description] AS md
WHERE md.[tenant_code] = @tenant_code
  AND md.[is_deleted] = 0
  AND md.[culture_code] IN (N'zh-CN', N'Z1', N'ja-JP')
  AND md.[material_code] IS NOT NULL
  AND LTRIM(RTRIM(md.[material_code])) <> N''
GROUP BY LTRIM(RTRIM(md.[material_code]));

-- ---------- 销售价格 MaterialDescription ----------
UPDATE t
SET
  t.[material_description] = LEFT(LTRIM(RTRIM(COALESCE(m.[zh_desc], m.[z1_desc], m.[ja_desc]))), 40),
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_sales_price] AS t
INNER JOIN #mat_desc AS m
  ON LTRIM(RTRIM(t.[material_code])) = m.[material_code]
WHERE t.[tenant_code] = @tenant_code
  AND t.[company_code] = @company_code
  AND t.[is_deleted] = 0
  AND (t.[material_description] IS NULL OR LTRIM(RTRIM(t.[material_description])) = N'')
  AND COALESCE(m.[zh_desc], m.[z1_desc], m.[ja_desc]) IS NOT NULL
  AND LTRIM(RTRIM(COALESCE(m.[zh_desc], m.[z1_desc], m.[ja_desc]))) <> N'';

SET @sales_updated = @@ROWCOUNT;

-- ---------- 采购价格 MaterialDescription ----------
UPDATE t
SET
  t.[material_description] = LEFT(LTRIM(RTRIM(COALESCE(m.[zh_desc], m.[z1_desc], m.[ja_desc]))), 40),
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_procurement_purchase_price] AS t
INNER JOIN #mat_desc AS m
  ON LTRIM(RTRIM(t.[material_code])) = m.[material_code]
WHERE t.[tenant_code] = @tenant_code
  AND t.[company_code] = @company_code
  AND t.[is_deleted] = 0
  AND (t.[material_description] IS NULL OR LTRIM(RTRIM(t.[material_description])) = N'')
  AND COALESCE(m.[zh_desc], m.[z1_desc], m.[ja_desc]) IS NOT NULL
  AND LTRIM(RTRIM(COALESCE(m.[zh_desc], m.[z1_desc], m.[ja_desc]))) <> N'';

SET @purchase_updated = @@ROWCOUNT;

-- ---------- 机种目的地 MaterialDescription：仅 ja-JP（按 material_code） ----------
UPDATE t
SET
  t.[material_description] = LEFT(LTRIM(RTRIM(d.[material_description])), 40),
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_materials_model_destination] AS t
INNER JOIN [dbo].[takt_logistics_materials_material_description] AS d
  ON d.[tenant_code] = @tenant_code
  AND d.[is_deleted] = 0
  AND LTRIM(RTRIM(d.[material_code])) = LTRIM(RTRIM(t.[material_code]))
  AND d.[culture_code] = N'ja-JP'
WHERE t.[tenant_code] = @tenant_code
  AND t.[is_deleted] = 0
  AND (t.[material_description] IS NULL OR LTRIM(RTRIM(t.[material_description])) = N'')
  AND LTRIM(RTRIM(ISNULL(d.[material_description], N''))) <> N'';

SET @model_desc_updated = @@ROWCOUNT;

-- ---------- 机种目的地 ModelName：仅 Z1（按 model_code → material_description.material_code） ----------
UPDATE t
SET
  t.[model_name] = LEFT(LTRIM(RTRIM(d.[material_description])), 80),
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_materials_model_destination] AS t
INNER JOIN [dbo].[takt_logistics_materials_material_description] AS d
  ON d.[tenant_code] = @tenant_code
  AND d.[is_deleted] = 0
  AND LTRIM(RTRIM(d.[material_code])) = LTRIM(RTRIM(t.[model_code]))
  AND d.[culture_code] = N'Z1'
WHERE t.[tenant_code] = @tenant_code
  AND t.[is_deleted] = 0
  AND (t.[model_name] IS NULL OR LTRIM(RTRIM(t.[model_name])) = N'')
  AND LTRIM(RTRIM(ISNULL(d.[material_description], N''))) <> N'';

SET @model_name_updated = @@ROWCOUNT;

DROP TABLE #mat_desc;

SELECT
  @tenant_code AS [tenant_code],
  @company_code AS [company_code],
  @sales_updated AS [sales_updated_rows],
  @purchase_updated AS [purchase_updated_rows],
  @model_desc_updated AS [model_material_description_updated_rows],
  @model_name_updated AS [model_name_updated_rows],
  (@sales_updated + @purchase_updated + @model_desc_updated + @model_name_updated) AS [total_updated_rows];
