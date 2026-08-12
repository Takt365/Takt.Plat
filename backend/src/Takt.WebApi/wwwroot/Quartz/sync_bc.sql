SET NOCOUNT ON;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体 TaktBomMaterialCostItem 一致
-- {{SourceDatabase}}.dbo.takt_logistics_manufacturing_bom_material_cost_item → 当前租户库同名表
-- 业务唯一键：Plant+Product+SequenceCode+BomLevel+BomItemCode+Component
--   +ComponentQuantity+BatchIndicator+ProductionRelated+PurchaseType+SpecialProcurementType+CostingDate

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [bom_level] NVARCHAR(20),
  [sequence_code] NVARCHAR(4),
  [product_code] NVARCHAR(20),
  [product_description] NVARCHAR(40),
  [bom_item_code] NVARCHAR(4),
  [component_code] NVARCHAR(20),
  [component_description] NVARCHAR(40),
  [component_quantity] DECIMAL(18,2),
  [batch_indicator] NVARCHAR(1),
  [production_related] NVARCHAR(1),
  [purchase_type] NVARCHAR(1),
  [special_procurement_type] NVARCHAR(50),
  [profit_center_code] NVARCHAR(4),
  [moving_average_price] DECIMAL(18,5),
  [moving_price_unit] INT,
  [moving_price_currency_code] NVARCHAR(3),
  [purchase_organization] NVARCHAR(4),
  [purchase_group] NVARCHAR(3),
  [supplier_code] NVARCHAR(10),
  [net_purchase_price] DECIMAL(18,5),
  [purchase_price_unit] INT,
  [purchase_currency_code] NVARCHAR(3),
  [costing_date] DATETIME,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [is_deleted] INT,
  [updated_by] BIGINT
);

-- 源表装入：
-- 1) 18 位纯数字产品/组件截末 10 位
-- 2) 按完整唯一键去重：
--    Plant+Product+SequenceCode+BomLevel+BomItemCode+Component
--    +ComponentQuantity+BatchIndicator+ProductionRelated+PurchaseType
--    +SpecialProcurementType+CostingDate
--    （移动平均价 → 组件数量 → 采购净价 较大者优先）
INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[bom_level],
  S.[sequence_code],
  S.[product_code],
  S.[product_description],
  S.[bom_item_code],
  S.[component_code],
  S.[component_description],
  S.[component_quantity],
  S.[batch_indicator],
  S.[production_related],
  S.[purchase_type],
  S.[special_procurement_type],
  S.[profit_center_code],
  S.[moving_average_price],
  S.[moving_price_unit],
  S.[moving_price_currency_code],
  S.[purchase_organization],
  S.[purchase_group],
  S.[supplier_code],
  S.[net_purchase_price],
  S.[purchase_price_unit],
  S.[purchase_currency_code],
  S.[costing_date],
  @tenant_code,
  @company_code,
  @culture_code,
  '{}',
  '',
  S.[is_deleted],
  @sync_user_id
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY
        N.[plant_code], N.[product_code], N.[sequence_code], N.[bom_level],
        N.[bom_item_code], N.[component_code], N.[component_quantity],
        N.[batch_indicator], N.[production_related], N.[purchase_type],
        N.[special_procurement_type], N.[costing_date]
    ) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
          AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
        ELSE LTRIM(RTRIM(R.[product_code]))
      END AS [product_code],
      LTRIM(RTRIM(R.[sequence_code])) AS [sequence_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[product_description])), ''), '') AS [product_description],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[bom_level])), ''), '') AS [bom_level],
      LTRIM(RTRIM(R.[bom_item_code])) AS [bom_item_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[component_code]))) = 18
          AND LTRIM(RTRIM(R.[component_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[component_code])), 10)
        ELSE LTRIM(RTRIM(R.[component_code]))
      END AS [component_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[component_description])), ''), '') AS [component_description],
      COALESCE(TRY_CAST(R.[component_quantity] AS DECIMAL(18,2)), 0) AS [component_quantity],
      NULLIF(LTRIM(RTRIM(R.[batch_indicator])), '') AS [batch_indicator],
      NULLIF(LTRIM(RTRIM(R.[production_related])), '') AS [production_related],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_type])), ''), 'F') AS [purchase_type],
      NULLIF(LTRIM(RTRIM(R.[special_procurement_type])), '') AS [special_procurement_type],
      ISNULL(NULLIF(LTRIM(RTRIM(LEFT(R.[profit_center_code], 4))), ''), '') AS [profit_center_code],
      ROUND(COALESCE(TRY_CAST(R.[moving_average_price] AS DECIMAL(18,8)), 0), 5) AS [moving_average_price],
      COALESCE(TRY_CAST(R.[moving_price_unit] AS INT), 1) AS [moving_price_unit],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[moving_price_currency_code])), ''), '') AS [moving_price_currency_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_organization])), ''), '') AS [purchase_organization],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_group])), ''), '') AS [purchase_group],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[supplier_code])), ''), '') AS [supplier_code],
      ROUND(COALESCE(TRY_CAST(R.[net_purchase_price] AS DECIMAL(18,8)), 0), 5) AS [net_purchase_price],
      COALESCE(TRY_CAST(R.[purchase_price_unit] AS INT), 1) AS [purchase_price_unit],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_currency_code])), ''), '') AS [purchase_currency_code],
      R.[costing_date] AS [costing_date],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(R.[plant_code])),
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
              AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
            ELSE LTRIM(RTRIM(R.[product_code]))
          END,
          LTRIM(RTRIM(R.[sequence_code])),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[bom_level])), ''), ''),
          LTRIM(RTRIM(R.[bom_item_code])),
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[component_code]))) = 18
              AND LTRIM(RTRIM(R.[component_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[component_code])), 10)
            ELSE LTRIM(RTRIM(R.[component_code]))
          END,
          COALESCE(TRY_CAST(R.[component_quantity] AS DECIMAL(18,2)), 0),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[batch_indicator])), ''), ''),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[production_related])), ''), ''),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_type])), ''), 'F'),
          ISNULL(NULLIF(LTRIM(RTRIM(R.[special_procurement_type])), ''), ''),
          R.[costing_date]
        ORDER BY
          ROUND(COALESCE(TRY_CAST(R.[moving_average_price] AS DECIMAL(18,8)), 0), 5) DESC,
          COALESCE(TRY_CAST(R.[component_quantity] AS DECIMAL(18,2)), 0) DESC,
          ROUND(COALESCE(TRY_CAST(R.[net_purchase_price] AS DECIMAL(18,8)), 0), 5) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_bom_material_cost_item] R
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #st_source);
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_bom_material_cost_item]);
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
          AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
        ELSE LTRIM(RTRIM(R.[product_code]))
      END AS [product_code],
      LTRIM(RTRIM(R.[sequence_code])) AS [sequence_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[bom_level])), ''), '') AS [bom_level],
      LTRIM(RTRIM(R.[bom_item_code])) AS [bom_item_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[component_code]))) = 18
          AND LTRIM(RTRIM(R.[component_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[component_code])), 10)
        ELSE LTRIM(RTRIM(R.[component_code]))
      END AS [component_code],
      COALESCE(TRY_CAST(R.[component_quantity] AS DECIMAL(18,2)), 0) AS [component_quantity],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[batch_indicator])), ''), '') AS [batch_indicator],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[production_related])), ''), '') AS [production_related],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_type])), ''), 'F') AS [purchase_type],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[special_procurement_type])), ''), '') AS [special_procurement_type],
      R.[costing_date] AS [costing_date]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_bom_material_cost_item] R
    GROUP BY
      LTRIM(RTRIM(R.[plant_code])),
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
          AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
        ELSE LTRIM(RTRIM(R.[product_code]))
      END,
      LTRIM(RTRIM(R.[sequence_code])),
      ISNULL(NULLIF(LTRIM(RTRIM(R.[bom_level])), ''), ''),
      LTRIM(RTRIM(R.[bom_item_code])),
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[component_code]))) = 18
          AND LTRIM(RTRIM(R.[component_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[component_code])), 10)
        ELSE LTRIM(RTRIM(R.[component_code]))
      END,
      COALESCE(TRY_CAST(R.[component_quantity] AS DECIMAL(18,2)), 0),
      ISNULL(NULLIF(LTRIM(RTRIM(R.[batch_indicator])), ''), ''),
      ISNULL(NULLIF(LTRIM(RTRIM(R.[production_related])), ''), ''),
      ISNULL(NULLIF(LTRIM(RTRIM(R.[purchase_type])), ''), 'F'),
      ISNULL(NULLIF(LTRIM(RTRIM(R.[special_procurement_type])), ''), ''),
      R.[costing_date]
  ) K
);
DECLARE @dedupe_dropped INT = @sap_raw_count - @sap_key_count;

-- 装入行数须等于业务键去重后行数
IF @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', sap_raw=', @sap_raw_count, N', dedupe_dropped=', @dedupe_dropped);
  THROW 50003, @src_msg, 1;
END;

-- 完整唯一键去重后仍重复则失败
IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY
    [plant_code], [product_code], [sequence_code], [bom_level], [bom_item_code],
    [component_code], [component_quantity],
    ISNULL([batch_indicator], N''), ISNULL([production_related], N''),
    [purchase_type], ISNULL([special_procurement_type], N''), [costing_date]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(800);
  SELECT TOP 1
    @dup_key = CONCAT(
      [plant_code], N' / ', [product_code], N' / ', [sequence_code], N' / ',
      [bom_level], N' / ', [bom_item_code], N' / ', [component_code], N' / ',
      CAST([component_quantity] AS NVARCHAR(40)), N' / ',
      ISNULL([batch_indicator], N''), N' / ', ISNULL([production_related], N''), N' / ',
      [purchase_type], N' / ', ISNULL([special_procurement_type], N''), N' / ',
      CONVERT(NVARCHAR(19), [costing_date], 120), N' x', COUNT(*))
  FROM #st_source
  GROUP BY
    [plant_code], [product_code], [sequence_code], [bom_level], [bom_item_code],
    [component_code], [component_quantity],
    ISNULL([batch_indicator], N''), ISNULL([production_related], N''),
    [purchase_type], ISNULL([special_procurement_type], N''), [costing_date]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(4),
  product_code NVARCHAR(20),
  component_code NVARCHAR(20),
  tenant_code NVARCHAR(3),
  company_code NVARCHAR(4),
  change_by BIGINT,
  component_quantity_old DECIMAL(18,2),
  component_quantity_new DECIMAL(18,2),
  moving_average_price_old DECIMAL(18,5),
  moving_average_price_new DECIMAL(18,5),
  net_purchase_price_old DECIMAL(18,5),
  net_purchase_price_new DECIMAL(18,5),
  purchase_type_old NVARCHAR(1),
  purchase_type_new NVARCHAR(1),
  costing_date_old DATETIME,
  costing_date_new DATETIME
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost_item]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);

MERGE INTO [takt_logistics_manufacturing_bom_material_cost_item] AS T
USING #st_source AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
AND LTRIM(RTRIM(T.[product_code])) = S.[product_code]
AND LTRIM(RTRIM(T.[sequence_code])) = S.[sequence_code]
AND ISNULL(LTRIM(RTRIM(T.[bom_level])), N'') = ISNULL(LTRIM(RTRIM(S.[bom_level])), N'')
AND LTRIM(RTRIM(T.[bom_item_code])) = S.[bom_item_code]
AND LTRIM(RTRIM(T.[component_code])) = S.[component_code]
AND ROUND(T.[component_quantity], 2) = ROUND(S.[component_quantity], 2)
AND ISNULL(LTRIM(RTRIM(T.[batch_indicator])), N'') = ISNULL(LTRIM(RTRIM(S.[batch_indicator])), N'')
AND ISNULL(LTRIM(RTRIM(T.[production_related])), N'') = ISNULL(LTRIM(RTRIM(S.[production_related])), N'')
AND LTRIM(RTRIM(ISNULL(T.[purchase_type], N''))) = LTRIM(RTRIM(ISNULL(S.[purchase_type], N'')))
AND ISNULL(LTRIM(RTRIM(T.[special_procurement_type])), N'') = ISNULL(LTRIM(RTRIM(S.[special_procurement_type])), N'')
AND T.[costing_date] = S.[costing_date]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[culture_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[culture_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[product_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[product_description], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[component_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[component_description], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[profit_center_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[profit_center_code], N'')))
  OR ROUND(T.[moving_average_price], 5) <> ROUND(S.[moving_average_price], 5)
  OR T.[moving_price_unit] <> S.[moving_price_unit]
  OR LTRIM(RTRIM(ISNULL(T.[moving_price_currency_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[moving_price_currency_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[purchase_organization], N''))) <> LTRIM(RTRIM(ISNULL(S.[purchase_organization], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[purchase_group], N''))) <> LTRIM(RTRIM(ISNULL(S.[purchase_group], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[supplier_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[supplier_code], N'')))
  OR ROUND(T.[net_purchase_price], 5) <> ROUND(S.[net_purchase_price], 5)
  OR T.[purchase_price_unit] <> S.[purchase_price_unit]
  OR LTRIM(RTRIM(ISNULL(T.[purchase_currency_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[purchase_currency_code], N'')))
) THEN
  UPDATE SET
    T.[product_description] = S.[product_description],
    T.[component_description] = S.[component_description],
    T.[profit_center_code] = S.[profit_center_code],
    T.[moving_average_price] = S.[moving_average_price],
    T.[moving_price_unit] = S.[moving_price_unit],
    T.[moving_price_currency_code] = S.[moving_price_currency_code],
    T.[purchase_organization] = S.[purchase_organization],
    T.[purchase_group] = S.[purchase_group],
    T.[supplier_code] = S.[supplier_code],
    T.[net_purchase_price] = S.[net_purchase_price],
    T.[purchase_price_unit] = S.[purchase_price_unit],
    T.[purchase_currency_code] = S.[purchase_currency_code],
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
    [id],[plant_code],[bom_level],[sequence_code],[product_code],[product_description],
    [bom_item_code],[component_code],[component_description],[component_quantity],
    [batch_indicator],[production_related],[purchase_type],[special_procurement_type],
    [profit_center_code],[moving_average_price],[moving_price_unit],[moving_price_currency_code],
    [purchase_organization],[purchase_group],[supplier_code],[net_purchase_price],
    [purchase_price_unit],[purchase_currency_code],[costing_date],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[bom_level],S.[sequence_code],S.[product_code],S.[product_description],
    S.[bom_item_code],S.[component_code],S.[component_description],S.[component_quantity],
    S.[batch_indicator],S.[production_related],S.[purchase_type],S.[special_procurement_type],
    S.[profit_center_code],S.[moving_average_price],S.[moving_price_unit],S.[moving_price_currency_code],
    S.[purchase_organization],S.[purchase_group],S.[supplier_code],S.[net_purchase_price],
    S.[purchase_price_unit],S.[purchase_currency_code],S.[costing_date],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
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
  INSERTED.[product_code],
  INSERTED.[component_code],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  INSERTED.[updated_by],
  DELETED.[component_quantity], INSERTED.[component_quantity],
  DELETED.[moving_average_price], INSERTED.[moving_average_price],
  DELETED.[net_purchase_price], INSERTED.[net_purchase_price],
  DELETED.[purchase_type], INSERTED.[purchase_type],
  DELETED.[costing_date], INSERTED.[costing_date]
INTO #delta(
  rn, oper_type, id, plant_code, product_code, component_code,
  tenant_code, company_code, change_by,
  component_quantity_old, component_quantity_new,
  moving_average_price_old, moving_average_price_new,
  net_purchase_price_old, net_purchase_price_new,
  purchase_type_old, purchase_type_new,
  costing_date_old, costing_date_new
);

IF OBJECT_ID('tempdb..#soft_deleted_rows') IS NOT NULL DROP TABLE #soft_deleted_rows;
CREATE TABLE #soft_deleted_rows (
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [product_code] NVARCHAR(20),
  [component_code] NVARCHAR(20)
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
  INSERTED.[product_code],
  INSERTED.[component_code]
INTO #soft_deleted_rows ([id], [plant_code], [product_code], [component_code])
FROM [takt_logistics_manufacturing_bom_material_cost_item] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[product_code] = LTRIM(RTRIM(T.[product_code]))
      AND S.[sequence_code] = LTRIM(RTRIM(T.[sequence_code]))
      AND ISNULL(LTRIM(RTRIM(S.[bom_level])), N'') = ISNULL(LTRIM(RTRIM(T.[bom_level])), N'')
      AND S.[bom_item_code] = LTRIM(RTRIM(T.[bom_item_code]))
      AND S.[component_code] = LTRIM(RTRIM(T.[component_code]))
      AND ROUND(S.[component_quantity], 2) = ROUND(T.[component_quantity], 2)
      AND ISNULL(LTRIM(RTRIM(S.[batch_indicator])), N'') = ISNULL(LTRIM(RTRIM(T.[batch_indicator])), N'')
      AND ISNULL(LTRIM(RTRIM(S.[production_related])), N'') = ISNULL(LTRIM(RTRIM(T.[production_related])), N'')
      AND LTRIM(RTRIM(ISNULL(S.[purchase_type], N''))) = LTRIM(RTRIM(ISNULL(T.[purchase_type], N'')))
      AND ISNULL(LTRIM(RTRIM(S.[special_procurement_type])), N'') = ISNULL(LTRIM(RTRIM(T.[special_procurement_type])), N'')
      AND S.[costing_date] = T.[costing_date]
  );

DECLARE @delete_count INT = @@ROWCOUNT;
DECLARE @soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    ISNULL([plant_code], N''), N'/',
    ISNULL([product_code], N''), N'/',
    ISNULL([component_code], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost_item]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost_item]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost_item]
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
  N'takt_logistics_manufacturing_bom_material_cost_item',
  d.id,
  ISNULL((
    SELECT
      d.component_quantity_old AS [component_quantity],
      d.moving_average_price_old AS [moving_average_price],
      d.net_purchase_price_old AS [net_purchase_price],
      d.purchase_type_old AS [purchase_type]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  (
    SELECT
      d.component_quantity_new AS [component_quantity],
      d.moving_average_price_new AS [moving_average_price],
      d.net_purchase_price_new AS [net_purchase_price],
      d.purchase_type_new AS [purchase_type]
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  ISNULL((
    SELECT
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.component_quantity_old AS NVARCHAR), 'null') END AS [component_quantity.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.component_quantity_new AS NVARCHAR), 'null') END AS [component_quantity.new],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.moving_average_price_old AS NVARCHAR), 'null') END AS [moving_average_price.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.moving_average_price_new AS NVARCHAR), 'null') END AS [moving_average_price.new]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  N'MERGE BomMaterialCostItem Sync',
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,0,
  d.tenant_code,d.company_code,'{}',N'SYNC',d.change_by,@now
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
  N'SYSTEM_SYNC',
  N'SYNC',
  N'BOM物料成本明细',
  N'exec_sql_merge',
  'SQL',
  N'/sync/bom-material-cost-item',
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
