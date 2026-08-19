SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};
DECLARE @now DATETIME = GETDATE();
DECLARE @updated INT = 0;

-- =============================================================================
-- QT_SYNC_BC_BK：BOM 物料成本明细采购价回填（对齐 TaktBomCalculatePurchasePriceHelper）
-- 唯一写入目标：takt_logistics_manufacturing_bom_material_cost_item
--   purchase_organization ← 采购价格主表 plant_code（采购组织=工厂码）
--   purchase_group        ← 主表 purchase_group
--   supplier_code         ← 主表 supplier_code
--   net_purchase_price    ← 条件行/数量等级/价值等级解析净价（5 位）
--   purchase_price_unit   ← 条件行 price_unit（≤0 则 1）
--   purchase_currency_code← 条件行 condition_currency_code
-- 匹配：BOM 组件 material_key ↔ 采购价格 material_key（不按工厂过滤）
-- 选价：仅 ValidFrom≤核算日取最晚（例：6/1 与 7/1 → 核算 6/30 用 6/1，核算 7/31 用 7/1）
-- ❌ 不用未来 ValidFrom；无 ≤核算日 价格则跳过（不回填 0）
-- 仅空回填：已有采购组织/组/供应商/货币/净价≠0/价格单位>1 一律保留
-- 本次写入合并到 ext_field：$._bk.bc（含 price_info=定价记录号：供应商：有效起始日：价格）
-- 同序：PB00 / Id 大优先
-- 条件行：未作废；PB00 优先，再按 purchase_price_seq / id
-- 净价：scale_basis=C 或（非 B 且有数量等级）→ 数量等级；否则有价值等级 → 价值等级；否则条件行 price
-- 建议：在 QT_SYNC_BC / QT_SYNC_PUP 之后执行
-- =============================================================================

IF OBJECT_ID('tempdb..#bom') IS NOT NULL DROP TABLE #bom;
IF OBJECT_ID('tempdb..#hdr_pick') IS NOT NULL DROP TABLE #hdr_pick;
IF OBJECT_ID('tempdb..#item_pick') IS NOT NULL DROP TABLE #item_pick;
IF OBJECT_ID('tempdb..#fill') IS NOT NULL DROP TABLE #fill;

;WITH bom AS (
  SELECT
    t.[id] AS [bom_id],
    CASE
      WHEN LEN(LTRIM(RTRIM(t.[component_code]))) = 18
        AND LTRIM(RTRIM(t.[component_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(t.[component_code])), 10)
      ELSE LTRIM(RTRIM(t.[component_code]))
    END AS [mat_key],
    CAST(t.[component_quantity] AS DECIMAL(18,5)) AS [component_quantity],
    CAST(t.[costing_date] AS DATE) AS [cost_day]
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] AS t
  WHERE t.[tenant_code] = @tenant_code
    AND t.[company_code] = @company_code
    AND t.[is_deleted] = 0
    AND LTRIM(RTRIM(ISNULL(t.[component_code], N''))) <> N''
)
SELECT *
INTO #bom
FROM bom
WHERE [mat_key] IS NOT NULL AND [mat_key] <> N'';

;WITH hdr AS (
  SELECT
    h.[id] AS [header_id],
    LTRIM(RTRIM(h.[plant_code])) AS [plant_code],
    CASE
      WHEN LEN(LTRIM(RTRIM(h.[material_code]))) = 18
        AND LTRIM(RTRIM(h.[material_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(h.[material_code])), 10)
      ELSE LTRIM(RTRIM(h.[material_code]))
    END AS [mat_key],
    LEFT(LTRIM(RTRIM(ISNULL(h.[purchase_group], N''))), 3) AS [purchase_group],
    LEFT(LTRIM(RTRIM(h.[supplier_code])), 10) AS [supplier_code],
    LEFT(LTRIM(RTRIM(h.[purchase_price_code])), 20) AS [purchase_price_code],
    CAST(h.[valid_from] AS DATE) AS [valid_from_d],
    CASE WHEN UPPER(LTRIM(RTRIM(h.[price_type]))) = N'PB00' THEN 0 ELSE 1 END AS [pb00_rank]
  FROM [dbo].[takt_logistics_procurement_purchase_price] AS h
  WHERE h.[tenant_code] = @tenant_code
    AND h.[company_code] = @company_code
    AND h.[is_deleted] = 0
    AND LTRIM(RTRIM(ISNULL(h.[material_code], N''))) <> N''
),
joined AS (
  SELECT
    b.[bom_id],
    b.[component_quantity],
    b.[cost_day],
    h.[header_id],
    h.[plant_code] AS [purchase_organization],
    h.[purchase_group],
    h.[supplier_code],
    h.[purchase_price_code],
    h.[valid_from_d],
    h.[pb00_rank]
  FROM #bom AS b
  INNER JOIN hdr AS h
    ON h.[mat_key] = b.[mat_key]
   AND h.[valid_from_d] <= b.[cost_day]
),
ranked AS (
  SELECT
    j.*,
    ROW_NUMBER() OVER (
      PARTITION BY j.[bom_id]
      ORDER BY
        j.[valid_from_d] DESC,
        j.[pb00_rank] ASC,
        j.[header_id] DESC
    ) AS [rn]
  FROM joined AS j
)
SELECT
  [bom_id],
  [component_quantity],
  [header_id],
  [purchase_organization],
  [purchase_group],
  [supplier_code],
  [purchase_price_code],
  [valid_from_d]
INTO #hdr_pick
FROM ranked
WHERE [rn] = 1;

;WITH items AS (
  SELECT
    i.[id] AS [item_id],
    i.[purchase_price_id] AS [header_id],
    LTRIM(RTRIM(ISNULL(i.[scale_basis], N''))) AS [scale_basis],
    CAST(i.[price] AS DECIMAL(18,5)) AS [item_price],
    CASE WHEN i.[price_unit] <= 0 THEN 1 ELSE i.[price_unit] END AS [purchase_price_unit],
    LEFT(LTRIM(RTRIM(i.[condition_currency_code])), 3) AS [purchase_currency_code],
    ROW_NUMBER() OVER (
      PARTITION BY i.[purchase_price_id]
      ORDER BY
        CASE WHEN UPPER(LTRIM(RTRIM(i.[price_type]))) = N'PB00' THEN 0 ELSE 1 END,
        i.[purchase_price_seq] ASC,
        i.[id] ASC
    ) AS [rn]
  FROM [dbo].[takt_logistics_procurement_purchase_price_item] AS i
  WHERE i.[tenant_code] = @tenant_code
    AND i.[company_code] = @company_code
    AND i.[is_deleted] = 0
    AND i.[is_obsolete] = 0
)
SELECT
  h.[bom_id],
  h.[component_quantity],
  h.[header_id],
  h.[purchase_organization],
  h.[purchase_group],
  h.[supplier_code],
  h.[purchase_price_code],
  h.[valid_from_d],
  i.[item_id],
  i.[scale_basis],
  i.[item_price],
  i.[purchase_price_unit],
  i.[purchase_currency_code]
INTO #item_pick
FROM #hdr_pick AS h
INNER JOIN items AS i
  ON i.[header_id] = h.[header_id]
 AND i.[rn] = 1;

;WITH qty_cnt AS (
  SELECT
    p.[item_id],
    COUNT(1) AS [cnt]
  FROM #item_pick AS p
  INNER JOIN [dbo].[takt_logistics_procurement_purchase_price_scale_quantity] AS q
    ON q.[purchase_price_item_id] = p.[item_id]
   AND q.[tenant_code] = @tenant_code
   AND q.[company_code] = @company_code
   AND q.[is_deleted] = 0
   AND q.[is_obsolete] = 0
  GROUP BY p.[item_id]
),
val_cnt AS (
  SELECT
    p.[item_id],
    COUNT(1) AS [cnt]
  FROM #item_pick AS p
  INNER JOIN [dbo].[takt_logistics_procurement_purchase_price_scale_value] AS v
    ON v.[purchase_price_item_id] = p.[item_id]
   AND v.[tenant_code] = @tenant_code
   AND v.[company_code] = @company_code
   AND v.[is_deleted] = 0
   AND v.[is_obsolete] = 0
  GROUP BY p.[item_id]
),
qty_price AS (
  SELECT
    p.[item_id],
    p.[component_quantity],
    COALESCE(
      (
        SELECT TOP (1) CAST(q.[price] AS DECIMAL(18,5))
        FROM [dbo].[takt_logistics_procurement_purchase_price_scale_quantity] AS q
        WHERE q.[purchase_price_item_id] = p.[item_id]
          AND q.[tenant_code] = @tenant_code
          AND q.[company_code] = @company_code
          AND q.[is_deleted] = 0
          AND q.[is_obsolete] = 0
          AND q.[scale_quantity] <= p.[component_quantity]
        ORDER BY q.[scale_quantity] DESC, q.[id] DESC
      ),
      (
        SELECT TOP (1) CAST(q.[price] AS DECIMAL(18,5))
        FROM [dbo].[takt_logistics_procurement_purchase_price_scale_quantity] AS q
        WHERE q.[purchase_price_item_id] = p.[item_id]
          AND q.[tenant_code] = @tenant_code
          AND q.[company_code] = @company_code
          AND q.[is_deleted] = 0
          AND q.[is_obsolete] = 0
        ORDER BY q.[scale_quantity] ASC, q.[id] ASC
      )
    ) AS [scale_price]
  FROM #item_pick AS p
),
val_price AS (
  SELECT
    p.[item_id],
    p.[component_quantity],
    COALESCE(
      (
        SELECT TOP (1) CAST(v.[price] AS DECIMAL(18,5))
        FROM [dbo].[takt_logistics_procurement_purchase_price_scale_value] AS v
        WHERE v.[purchase_price_item_id] = p.[item_id]
          AND v.[tenant_code] = @tenant_code
          AND v.[company_code] = @company_code
          AND v.[is_deleted] = 0
          AND v.[is_obsolete] = 0
          AND v.[scale_value] <= p.[component_quantity]
        ORDER BY v.[scale_value] DESC, v.[id] DESC
      ),
      (
        SELECT TOP (1) CAST(v.[price] AS DECIMAL(18,5))
        FROM [dbo].[takt_logistics_procurement_purchase_price_scale_value] AS v
        WHERE v.[purchase_price_item_id] = p.[item_id]
          AND v.[tenant_code] = @tenant_code
          AND v.[company_code] = @company_code
          AND v.[is_deleted] = 0
          AND v.[is_obsolete] = 0
        ORDER BY v.[scale_value] ASC, v.[id] ASC
      )
    ) AS [scale_price]
  FROM #item_pick AS p
)
SELECT
  p.[bom_id],
  LEFT(p.[purchase_organization], 4) AS [purchase_organization],
  LEFT(p.[purchase_group], 3) AS [purchase_group],
  LEFT(p.[supplier_code], 10) AS [supplier_code],
  LEFT(p.[purchase_price_code], 20) AS [purchase_price_code],
  p.[valid_from_d],
  ROUND(
    CASE
      WHEN (
        UPPER(p.[scale_basis]) = N'C'
        OR (UPPER(p.[scale_basis]) <> N'B' AND ISNULL(qc.[cnt], 0) > 0)
      ) AND ISNULL(qc.[cnt], 0) > 0
      THEN ISNULL(qp.[scale_price], p.[item_price])
      WHEN ISNULL(vc.[cnt], 0) > 0
      THEN ISNULL(vp.[scale_price], p.[item_price])
      ELSE p.[item_price]
    END,
    5
  ) AS [net_purchase_price],
  p.[purchase_price_unit],
  LEFT(ISNULL(p.[purchase_currency_code], N''), 3) AS [purchase_currency_code]
INTO #fill
FROM #item_pick AS p
LEFT JOIN qty_cnt AS qc ON qc.[item_id] = p.[item_id]
LEFT JOIN val_cnt AS vc ON vc.[item_id] = p.[item_id]
LEFT JOIN qty_price AS qp ON qp.[item_id] = p.[item_id]
LEFT JOIN val_price AS vp ON vp.[item_id] = p.[item_id];

UPDATE t
SET
  t.[purchase_organization] = CASE
    WHEN LTRIM(RTRIM(ISNULL(t.[purchase_organization], N''))) = N''
      AND LTRIM(RTRIM(ISNULL(f.[purchase_organization], N''))) <> N''
    THEN f.[purchase_organization] ELSE t.[purchase_organization] END,
  t.[purchase_group] = CASE
    WHEN LTRIM(RTRIM(ISNULL(t.[purchase_group], N''))) = N''
      AND LTRIM(RTRIM(ISNULL(f.[purchase_group], N''))) <> N''
    THEN f.[purchase_group] ELSE t.[purchase_group] END,
  t.[supplier_code] = CASE
    WHEN LTRIM(RTRIM(ISNULL(t.[supplier_code], N''))) = N''
      AND LTRIM(RTRIM(ISNULL(f.[supplier_code], N''))) <> N''
    THEN f.[supplier_code] ELSE t.[supplier_code] END,
  t.[net_purchase_price] = CASE
    WHEN t.[net_purchase_price] = 0 AND f.[net_purchase_price] <> 0
    THEN f.[net_purchase_price] ELSE t.[net_purchase_price] END,
  t.[purchase_price_unit] = CASE
    WHEN t.[purchase_price_unit] <= 1 AND f.[purchase_price_unit] > 1
    THEN f.[purchase_price_unit] ELSE t.[purchase_price_unit] END,
  t.[purchase_currency_code] = CASE
    WHEN LTRIM(RTRIM(ISNULL(t.[purchase_currency_code], N''))) = N''
      AND LTRIM(RTRIM(ISNULL(f.[purchase_currency_code], N''))) <> N''
    THEN f.[purchase_currency_code] ELSE t.[purchase_currency_code] END,
  t.[ext_field] = LEFT(x.[new_ext], 4000),
  t.[updated_by] = @sync_user_id,
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] AS t
INNER JOIN #fill AS f ON f.[bom_id] = t.[id]
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
    JSON_MODIFY(
      JSON_MODIFY(
        JSON_MODIFY(
          JSON_MODIFY(
            JSON_MODIFY(
              JSON_MODIFY(
                JSON_MODIFY(
                  JSON_MODIFY(
                    JSON_MODIFY(
                      JSON_MODIFY(N'{}', N'$.at', CONVERT(VARCHAR(19), @now, 126)),
                      N'$.price_info',
                      CONCAT(
                        N'定价记录号：', ISNULL(f.[purchase_price_code], N''),
                        N'：供应商：', ISNULL(f.[supplier_code], N''),
                        N'：有效起始日：', CONVERT(VARCHAR(10), f.[valid_from_d], 23),
                        N'：价格：', CONVERT(VARCHAR(40), f.[net_purchase_price]))),
                    N'$.purchase_price_code', f.[purchase_price_code]),
                  N'$.valid_from', CONVERT(VARCHAR(10), f.[valid_from_d], 23)),
                N'$.price', f.[net_purchase_price]),
              N'$.purchase_organization',
              CASE
                WHEN LTRIM(RTRIM(ISNULL(t.[purchase_organization], N''))) = N''
                  AND LTRIM(RTRIM(ISNULL(f.[purchase_organization], N''))) <> N''
                THEN f.[purchase_organization] END),
            N'$.purchase_group',
            CASE
              WHEN LTRIM(RTRIM(ISNULL(t.[purchase_group], N''))) = N''
                AND LTRIM(RTRIM(ISNULL(f.[purchase_group], N''))) <> N''
              THEN f.[purchase_group] END),
          N'$.supplier_code',
          CASE
            WHEN LTRIM(RTRIM(ISNULL(t.[supplier_code], N''))) = N''
              AND LTRIM(RTRIM(ISNULL(f.[supplier_code], N''))) <> N''
            THEN f.[supplier_code] END),
        N'$.net_purchase_price',
        CASE
          WHEN t.[net_purchase_price] = 0 AND f.[net_purchase_price] <> 0
          THEN f.[net_purchase_price] END),
      N'$.purchase_price_unit',
      CASE
        WHEN t.[purchase_price_unit] <= 1 AND f.[purchase_price_unit] > 1
        THEN f.[purchase_price_unit] END)
    AS [frag_pre]
) AS e2a
CROSS APPLY (
  SELECT
    JSON_MODIFY(
      e2a.[frag_pre],
      N'$.purchase_currency_code',
      CASE
        WHEN LTRIM(RTRIM(ISNULL(t.[purchase_currency_code], N''))) = N''
          AND LTRIM(RTRIM(ISNULL(f.[purchase_currency_code], N''))) <> N''
        THEN f.[purchase_currency_code] END)
    AS [frag]
) AS e2
CROSS APPLY (
  SELECT JSON_MODIFY(e1.[with_bk], N'lax $._bk.bc', JSON_QUERY(e2.[frag])) AS [new_ext]
) AS x
WHERE t.[tenant_code] = @tenant_code
  AND t.[company_code] = @company_code
  AND t.[is_deleted] = 0
  AND (
    (LTRIM(RTRIM(ISNULL(t.[purchase_organization], N''))) = N'' AND LTRIM(RTRIM(ISNULL(f.[purchase_organization], N''))) <> N'')
    OR (LTRIM(RTRIM(ISNULL(t.[purchase_group], N''))) = N'' AND LTRIM(RTRIM(ISNULL(f.[purchase_group], N''))) <> N'')
    OR (LTRIM(RTRIM(ISNULL(t.[supplier_code], N''))) = N'' AND LTRIM(RTRIM(ISNULL(f.[supplier_code], N''))) <> N'')
    OR (t.[net_purchase_price] = 0 AND f.[net_purchase_price] <> 0)
    OR (t.[purchase_price_unit] <= 1 AND f.[purchase_price_unit] > 1)
    OR (LTRIM(RTRIM(ISNULL(t.[purchase_currency_code], N''))) = N'' AND LTRIM(RTRIM(ISNULL(f.[purchase_currency_code], N''))) <> N'')
  );

SET @updated = @@ROWCOUNT;

DROP TABLE #bom;
DROP TABLE #hdr_pick;
DROP TABLE #item_pick;
DROP TABLE #fill;

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'bc_bk' AS NVARCHAR(40)) AS [scope],
  @tenant_code AS [tenant_code],
  @company_code AS [company_code],
  @updated AS [purchase_fields_updated],
  @updated AS [total_updated_rows];
