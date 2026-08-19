-- =============================================================================
-- 补齐 zTakt_000_Dev.dbo.takt_logistics_manufacturing_bom_material_cost_item 二级索引
-- 对齐实体 TaktBomMaterialCostItem 的 [SugarIndex]（表已有 PK(id)，缺业务唯一/过滤索引）
-- 用法：在目标库执行；已存在同名索引则跳过
-- 注意：唯一索引含可空列；建前建议先把 NULL 键列规范为空串，避免与 sync_bc 裸等值不一致
-- =============================================================================
USE [zTakt_000_Dev];
GO

SET NOCOUNT ON;

-- 与 sync_bc 一致：可空业务键 NULL→''，数量 5 位（避免唯一索引与 MERGE 对不齐）
UPDATE [dbo].[takt_logistics_manufacturing_bom_material_cost_item]
SET
  [bom_level] = ISNULL([bom_level], N''),
  [batch_indicator] = ISNULL([batch_indicator], N''),
  [production_related] = ISNULL([production_related], N''),
  [special_procurement_type] = ISNULL([special_procurement_type], N''),
  [component_quantity] = ROUND([component_quantity], 5)
WHERE [bom_level] IS NULL
   OR [batch_indicator] IS NULL
   OR [production_related] IS NULL
   OR [special_procurement_type] IS NULL
   OR [component_quantity] <> ROUND([component_quantity], 5);
GO

IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item')
    AND name = N'ix_bom_material_cost_item_tenant'
)
BEGIN
  CREATE NONCLUSTERED INDEX [ix_bom_material_cost_item_tenant]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost_item] ([tenant_code], [company_code]);
END
GO

IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item')
    AND name = N'ix_bom_material_cost_item_is_deleted'
)
BEGIN
  CREATE NONCLUSTERED INDEX [ix_bom_material_cost_item_is_deleted]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost_item] ([tenant_code], [company_code], [is_deleted]);
END
GO

IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item')
    AND name = N'ix_takt_logistics_manufacturing_bom_material_cost_item_line_unique'
)
BEGIN
  -- 业务唯一键（MERGE / sync_bc 依赖）
  CREATE UNIQUE NONCLUSTERED INDEX [ix_takt_logistics_manufacturing_bom_material_cost_item_line_unique]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost_item] (
      [tenant_code], [company_code], [plant_code], [bom_level],
      [bom_item_code], [product_code], [line_number], [component_code],
      [component_quantity], [batch_indicator], [production_related],
      [purchase_type], [special_procurement_type], [costing_date]
    );
END
GO

IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item')
    AND name = N'ix_takt_logistics_manufacturing_bom_material_cost_item_plant_code'
)
BEGIN
  CREATE NONCLUSTERED INDEX [ix_takt_logistics_manufacturing_bom_material_cost_item_plant_code]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost_item] ([tenant_code], [company_code], [plant_code]);
END
GO

IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item')
    AND name = N'ix_takt_logistics_manufacturing_bom_material_cost_item_product_code'
)
BEGIN
  CREATE NONCLUSTERED INDEX [ix_takt_logistics_manufacturing_bom_material_cost_item_product_code]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost_item] ([tenant_code], [company_code], [product_code]);
END
GO

IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item')
    AND name = N'ix_takt_logistics_manufacturing_bom_material_cost_item_component_code'
)
BEGIN
  CREATE NONCLUSTERED INDEX [ix_takt_logistics_manufacturing_bom_material_cost_item_component_code]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost_item] ([tenant_code], [company_code], [component_code]);
END
GO

IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item')
    AND name = N'ix_takt_logistics_manufacturing_bom_material_cost_item_costing_date'
)
BEGIN
  CREATE NONCLUSTERED INDEX [ix_takt_logistics_manufacturing_bom_material_cost_item_costing_date]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost_item] ([tenant_code], [company_code], [costing_date]);
END
GO

-- 校验
SELECT i.name, i.is_unique, i.is_primary_key, i.type_desc
FROM sys.indexes i
WHERE i.object_id = OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item')
  AND i.name IS NOT NULL
ORDER BY i.is_primary_key DESC, i.name;
GO
