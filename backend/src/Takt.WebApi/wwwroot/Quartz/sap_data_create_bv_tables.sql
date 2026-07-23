-- ========================================
-- Sap_Data BOM 物料成本汇总暂存表 DDL（手工执行，非 Quartz 任务）
-- 表名与租户库目标表相同（仅库不同：Sap_Data vs Takt_{Tenant}_*）
-- 对齐：sap_sync_bv.sql / TaktBomMaterialCost
-- ========================================

USE [Sap_Data];
GO

IF OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_manufacturing_bom_material_cost] (
    [plant_code] NVARCHAR(4) NOT NULL,
    [model_code] NVARCHAR(40) NOT NULL,
    [model_monthly_average_cost] DECIMAL(18,5) NULL,
    [product_code] NVARCHAR(20) NOT NULL,
    [product_description] NVARCHAR(40) NULL,
    [product_monthly_cost] DECIMAL(18,5) NULL,
    [currency_code] NVARCHAR(3) NULL,
    [costing_period] NVARCHAR(7) NULL,
    [costing_date] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_bom_material_cost]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost]
    ([plant_code], [model_code], [product_code], [costing_period]);
END
GO
