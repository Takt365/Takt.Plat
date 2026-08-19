SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};
DECLARE @now DATETIME = GETDATE();
DECLARE @costing_period NVARCHAR(7) = NULLIF(LTRIM(RTRIM(N'{{CostingPeriod}}')), N'');
IF @costing_period IS NULL
  SET @costing_period = CONVERT(CHAR(7), GETDATE(), 126);

-- =============================================================================
-- QT_SYNC_BC_PCB_SECT_BK：BOM 明细 PCB SECT 整树回填 pcb_sect_indicator
-- 对齐 TaktBomMaterialCostItemLineCostHelper.CollectPcbSectHierarchyRows
--   + TryApplyPcbSectIndicatorMark（写入 X；已有 X 跳过）
-- 树根：component_description 含「PCB SECT」（大小写不敏感），或已标 pcb_sect_indicator=X
-- 子孙：同一工厂+规范化产品码+核算日树上，展开序下深度更深的后代行
-- 展开序：ProductCode → LineNumber → BomLevel 前导点数 → BomLevel 数字段 → BomLevel 原文
-- 产品码：18 位纯数字截末 10 位（与 NormalizeProductCodeForTree 一致）
-- 核算月：ExecuteParams.costingPeriod（yyyy-MM）；空则当月
-- 仅目标库；不读源库；建议在 QT_SYNC_BC 之后执行
-- =============================================================================

IF OBJECT_ID('tempdb..#bc_pcb_src') IS NOT NULL DROP TABLE #bc_pcb_src;
IF OBJECT_ID('tempdb..#bc_pcb_mark') IS NOT NULL DROP TABLE #bc_pcb_mark;

CREATE TABLE #bc_pcb_src (
  [rn] INT NOT NULL,
  [id] BIGINT NOT NULL,
  [plant_code] NVARCHAR(4) NOT NULL,
  [product_norm] NVARCHAR(20) NOT NULL,
  [cost_day] DATE NOT NULL,
  [product_code] NVARCHAR(20) NOT NULL,
  [line_number] INT NOT NULL,
  [bom_depth] INT NOT NULL,
  [bom_level_num] INT NOT NULL,
  [bom_level] NVARCHAR(20) NOT NULL,
  [is_pcb_node] BIT NOT NULL,
  [already_marked] BIT NOT NULL
);

CREATE TABLE #bc_pcb_mark (
  [id] BIGINT NOT NULL PRIMARY KEY
);

INSERT INTO #bc_pcb_src (
  [rn], [id], [plant_code], [product_norm], [cost_day], [product_code],
  [line_number], [bom_depth], [bom_level_num], [bom_level], [is_pcb_node], [already_marked]
)
SELECT
  ROW_NUMBER() OVER (
    ORDER BY
      x.[plant_code],
      x.[product_norm],
      x.[cost_day],
      x.[product_code],
      x.[line_number],
      x.[bom_depth],
      x.[bom_level_num],
      x.[bom_level],
      x.[id]
  ) AS [rn],
  x.[id],
  x.[plant_code],
  x.[product_norm],
  x.[cost_day],
  x.[product_code],
  x.[line_number],
  x.[bom_depth],
  x.[bom_level_num],
  x.[bom_level],
  x.[is_pcb_node],
  x.[already_marked]
FROM (
  SELECT
    t.[id],
    LTRIM(RTRIM(ISNULL(t.[plant_code], N''))) AS [plant_code],
    CASE
      WHEN LEN(LTRIM(RTRIM(ISNULL(t.[product_code], N'')))) = 18
        AND LTRIM(RTRIM(t.[product_code])) NOT LIKE N'%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(t.[product_code])), 10)
      ELSE LTRIM(RTRIM(ISNULL(t.[product_code], N'')))
    END AS [product_norm],
    CAST(t.[costing_date] AS DATE) AS [cost_day],
    LTRIM(RTRIM(ISNULL(t.[product_code], N''))) AS [product_code],
    t.[line_number],
    bl.[bom_level],
    bl.[bom_depth],
    bl.[bom_level_num],
    CASE
      WHEN CHARINDEX(N'PCB SECT', UPPER(LTRIM(RTRIM(ISNULL(t.[component_description], N''))))) > 0
        OR UPPER(LTRIM(RTRIM(ISNULL(t.[pcb_sect_indicator], N'')))) = N'X'
      THEN CAST(1 AS BIT)
      ELSE CAST(0 AS BIT)
    END AS [is_pcb_node],
    CASE
      WHEN UPPER(LTRIM(RTRIM(ISNULL(t.[pcb_sect_indicator], N'')))) = N'X'
      THEN CAST(1 AS BIT)
      ELSE CAST(0 AS BIT)
    END AS [already_marked]
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] AS t
  CROSS APPLY (
    SELECT
      LTRIM(RTRIM(ISNULL(t.[bom_level], N''))) AS [bom_level]
  ) AS b0
  CROSS APPLY (
    SELECT
      b0.[bom_level],
      /* 前导点数：1→0，.1→1，..2→2；空层级靠后 */
      CASE
        WHEN b0.[bom_level] = N'' THEN 2147483647
        WHEN PATINDEX(N'%[^.]%', b0.[bom_level]) = 0 THEN LEN(b0.[bom_level])
        ELSE PATINDEX(N'%[^.]%', b0.[bom_level]) - 1
      END AS [bom_depth],
      /* 去掉前导点后的首段数字；非数字靠后 */
      CASE
        WHEN b0.[bom_level] = N'' THEN 2147483647
        ELSE ISNULL(
          TRY_CAST(
            CASE
              WHEN PATINDEX(N'%[^.]%', b0.[bom_level]) = 0 THEN N''
              WHEN PATINDEX(
                N'%[^0-9]%',
                SUBSTRING(b0.[bom_level], PATINDEX(N'%[^.]%', b0.[bom_level]), 40)
              ) = 0
              THEN SUBSTRING(b0.[bom_level], PATINDEX(N'%[^.]%', b0.[bom_level]), 40)
              ELSE LEFT(
                SUBSTRING(b0.[bom_level], PATINDEX(N'%[^.]%', b0.[bom_level]), 40),
                PATINDEX(
                  N'%[^0-9]%',
                  SUBSTRING(b0.[bom_level], PATINDEX(N'%[^.]%', b0.[bom_level]), 40)
                ) - 1)
            END AS INT),
          2147483647)
      END AS [bom_level_num]
  ) AS bl
  WHERE t.[tenant_code] = @tenant_code
    AND t.[company_code] = @company_code
    AND t.[is_deleted] = 0
    AND t.[costing_date] IS NOT NULL
    AND CONVERT(CHAR(7), t.[costing_date], 126) = @costing_period
    AND LTRIM(RTRIM(ISNULL(t.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(t.[product_code], N''))) <> N''
) AS x;

DECLARE @scanned INT = (SELECT COUNT(*) FROM #bc_pcb_src);

DECLARE @cur_rn INT = 1;
DECLARE @max_rn INT = ISNULL((SELECT MAX([rn]) FROM #bc_pcb_src), 0);
DECLARE @prev_plant NVARCHAR(4) = N'';
DECLARE @prev_product NVARCHAR(20) = N'';
DECLARE @prev_cost DATE = '19000101';
DECLARE @id BIGINT;
DECLARE @plant NVARCHAR(4);
DECLARE @product NVARCHAR(20);
DECLARE @cost_day DATE;
DECLARE @depth INT;
DECLARE @is_node BIT;
DECLARE @under BIT;

DECLARE @anc TABLE (
  [seq] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
  [depth] INT NOT NULL,
  [under_pcb] BIT NOT NULL
);

WHILE @cur_rn <= @max_rn
BEGIN
  SELECT
    @id = s.[id],
    @plant = s.[plant_code],
    @product = s.[product_norm],
    @cost_day = s.[cost_day],
    @depth = s.[bom_depth],
    @is_node = s.[is_pcb_node]
  FROM #bc_pcb_src AS s
  WHERE s.[rn] = @cur_rn;

  IF @plant <> @prev_plant
    OR @product <> @prev_product
    OR @cost_day <> @prev_cost
  BEGIN
    DELETE FROM @anc;
    SET @prev_plant = @plant;
    SET @prev_product = @product;
    SET @prev_cost = @cost_day;
  END;

  WHILE EXISTS (SELECT 1 FROM @anc WHERE [depth] >= @depth)
  BEGIN
    DELETE FROM @anc
    WHERE [seq] = (SELECT MAX([seq]) FROM @anc);
  END;

  SET @under = CASE WHEN EXISTS (SELECT 1 FROM @anc WHERE [under_pcb] = 1) THEN 1 ELSE 0 END;

  IF @under = 1 OR @is_node = 1
  BEGIN
    INSERT INTO #bc_pcb_mark ([id]) VALUES (@id);
  END;

  INSERT INTO @anc ([depth], [under_pcb])
  VALUES (@depth, CASE WHEN @under = 1 OR @is_node = 1 THEN 1 ELSE 0 END);

  SET @cur_rn = @cur_rn + 1;
END;

DECLARE @pcb_sect_count INT = (SELECT COUNT(*) FROM #bc_pcb_mark);
DECLARE @unchanged INT = (
  SELECT COUNT(*)
  FROM #bc_pcb_mark AS m
  INNER JOIN #bc_pcb_src AS s ON s.[id] = m.[id]
  WHERE s.[already_marked] = 1
);

UPDATE t
SET
  t.[pcb_sect_indicator] = N'X',
  t.[updated_by] = @sync_user_id,
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] AS t
INNER JOIN #bc_pcb_mark AS m ON m.[id] = t.[id]
WHERE UPPER(LTRIM(RTRIM(ISNULL(t.[pcb_sect_indicator], N'')))) <> N'X';

DECLARE @updated INT = @@ROWCOUNT;

DROP TABLE #bc_pcb_src;
DROP TABLE #bc_pcb_mark;

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'bc_pcb_sect_bk' AS NVARCHAR(40)) AS [scope],
  @tenant_code AS [tenant_code],
  @company_code AS [company_code],
  @costing_period AS [costing_period],
  @scanned AS [scanned_row_count],
  @pcb_sect_count AS [pcb_sect_row_count],
  @updated AS [updated_row_count],
  @unchanged AS [unchanged_row_count];
