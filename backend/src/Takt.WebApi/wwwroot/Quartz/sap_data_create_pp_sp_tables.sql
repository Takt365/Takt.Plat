-- ========================================
-- Sap_Data 采购/销售价格四级暂存表 DDL（手工执行，非 Quartz 任务）
-- 表名与租户库目标表相同（仅库不同：Sap_Data vs Takt_{Tenant}_*）
-- 对齐：sap_sync_pp.sql / sap_sync_sp.sql
-- ========================================

USE [Sap_Data];
GO

IF OBJECT_ID(N'dbo.takt_logistics_materials_purchase_price', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_materials_purchase_price] (
    [plant_code] NVARCHAR(4) NOT NULL,
    [purchase_price_code] NVARCHAR(20) NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [supplier_code] NVARCHAR(40) NULL,
    [material_code] NVARCHAR(40) NULL,
    [purchase_group] NVARCHAR(3) NULL,
    [tax_code] NVARCHAR(4) NULL,
    [gr_based_invoice_inspection] INT NULL,
    [pricing_date_control] INT NULL,
    [valid_from] DATETIME NULL,
    [valid_to] DATETIME NULL,
    [purchase_inquiry_id] BIGINT NULL,
    [purchase_inquiry_code] NVARCHAR(40) NULL,
    [variable_key] NVARCHAR(40) NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price]
    ON [dbo].[takt_logistics_materials_purchase_price] ([plant_code], [purchase_price_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_materials_purchase_price_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_materials_purchase_price_item] (
    [purchase_price_code] NVARCHAR(20) NOT NULL,
    [purchase_price_seq] INT NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [scale_type] NVARCHAR(1) NULL,
    [scale_basis] NVARCHAR(1) NULL,
    [scale_quantity] DECIMAL(18,4) NULL,
    [scale_unit] NVARCHAR(5) NULL,
    [scale_value] DECIMAL(18,5) NULL,
    [scale_currency] NVARCHAR(3) NULL,
    [calculation_type] NVARCHAR(1) NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [condition_currency] NVARCHAR(3) NULL,
    [price_unit] INT NULL,
    [unit_of_measure] NVARCHAR(5) NULL,
    [min_order_quantity] INT NULL,
    [rounding_value] INT NULL,
    [planned_delivery_time_days] INT NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price_item]
    ON [dbo].[takt_logistics_materials_purchase_price_item] ([purchase_price_code], [purchase_price_seq]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_materials_purchase_price_scale_quantity', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_materials_purchase_price_scale_quantity] (
    [purchase_price_code] NVARCHAR(20) NOT NULL,
    [purchase_price_seq] INT NOT NULL,
    [purchase_scale_seq] INT NOT NULL,
    [scale_quantity] DECIMAL(18,4) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price_scale_qty]
    ON [dbo].[takt_logistics_materials_purchase_price_scale_quantity]
    ([purchase_price_code], [purchase_price_seq], [purchase_scale_seq], [scale_quantity]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_materials_purchase_price_scale_value', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_materials_purchase_price_scale_value] (
    [purchase_price_code] NVARCHAR(20) NOT NULL,
    [purchase_price_seq] INT NOT NULL,
    [purchase_scale_seq] INT NOT NULL,
    [scale_value] DECIMAL(18,5) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price_scale_val]
    ON [dbo].[takt_logistics_materials_purchase_price_scale_value]
    ([purchase_price_code], [purchase_price_seq], [purchase_scale_seq], [scale_value]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_price', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_price] (
    [plant_code] NVARCHAR(4) NOT NULL,
    [sales_price_code] NVARCHAR(20) NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [customer_code] NVARCHAR(40) NULL,
    [material_code] NVARCHAR(40) NULL,
    [sales_group] NVARCHAR(3) NULL,
    [tax_code] NVARCHAR(4) NULL,
    [gr_based_invoice_inspection] INT NULL,
    [pricing_date_control] INT NULL,
    [valid_from] DATETIME NULL,
    [valid_to] DATETIME NULL,
    [sales_quotation_id] BIGINT NULL,
    [sales_quotation_code] NVARCHAR(40) NULL,
    [variable_key] NVARCHAR(40) NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price]
    ON [dbo].[takt_logistics_sales_price] ([plant_code], [sales_price_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_price_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_price_item] (
    [sales_price_code] NVARCHAR(20) NOT NULL,
    [sales_price_seq] INT NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [scale_type] NVARCHAR(1) NULL,
    [scale_basis] NVARCHAR(1) NULL,
    [scale_quantity] DECIMAL(18,4) NULL,
    [scale_unit] NVARCHAR(5) NULL,
    [scale_value] DECIMAL(18,5) NULL,
    [scale_currency] NVARCHAR(3) NULL,
    [calculation_type] NVARCHAR(1) NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [condition_currency] NVARCHAR(3) NULL,
    [price_unit] INT NULL,
    [unit_of_measure] NVARCHAR(5) NULL,
    [min_order_quantity] INT NULL,
    [rounding_value] INT NULL,
    [planned_delivery_time_days] INT NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price_item]
    ON [dbo].[takt_logistics_sales_price_item] ([sales_price_code], [sales_price_seq]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_price_scale_quantity', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_price_scale_quantity] (
    [sales_price_code] NVARCHAR(20) NOT NULL,
    [sales_price_seq] INT NOT NULL,
    [sales_scale_seq] INT NOT NULL,
    [scale_quantity] DECIMAL(18,4) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price_scale_qty]
    ON [dbo].[takt_logistics_sales_price_scale_quantity]
    ([sales_price_code], [sales_price_seq], [sales_scale_seq], [scale_quantity]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_price_scale_value', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_price_scale_value] (
    [sales_price_code] NVARCHAR(20) NOT NULL,
    [sales_price_seq] INT NOT NULL,
    [sales_scale_seq] INT NOT NULL,
    [scale_value] DECIMAL(18,5) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price_scale_val]
    ON [dbo].[takt_logistics_sales_price_scale_value]
    ([sales_price_code], [sales_price_seq], [sales_scale_seq], [scale_value]);
END
GO
