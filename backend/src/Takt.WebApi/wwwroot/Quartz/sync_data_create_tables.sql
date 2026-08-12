-- ========================================
-- 暂存表 DDL（手工执行，非 Quartz 任务）
-- 表名与租户库目标表相同（仅库不同：暂存库 vs 租户业务库）
-- 对齐：
--   sync_ad.sql  / TaktAdminDivision
--   sync_bc.sql  / TaktBomMaterialCostItem；sync_bv.sql / TaktBomMaterialCost
--   sync_pup.sql / sync_sp.sql（采购/销售价格四级）
--   sync_sup.sql / TaktSupplier；sync_cus.sql / TaktCustomer
--   sync_matdoc.sql / TaktMaterialDocument(+Item)
--   sync_sdinv.sql / TaktSalesInvoice(+Item)
-- ========================================
-- 手工执行前请将 USE 改为实际暂存库名（如 zTakt_900_Dev / zTakt_900_Prod）
USE [zTakt_900_Dev];
GO

-- ----------------------------------------
-- 行政区划（sync_ad）
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_foundation_admin_division', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_foundation_admin_division] (
    [id] BIGINT NOT NULL,
    [country_code] NVARCHAR(2) NOT NULL,
    [division_code] NVARCHAR(40) NOT NULL,
    [division_name] NVARCHAR(200) NOT NULL,
    [parent_id] BIGINT NOT NULL CONSTRAINT [df_sap_admin_division_parent_id] DEFAULT (0),
    [level] INT NOT NULL CONSTRAINT [df_sap_admin_division_level] DEFAULT (1),
    [division_path] NVARCHAR(500) NOT NULL CONSTRAINT [df_sap_admin_division_path] DEFAULT (N''),
    [is_leaf] INT NOT NULL CONSTRAINT [df_sap_admin_division_is_leaf] DEFAULT (0),
    [postal_code] NVARCHAR(20) NULLVARCHAR(5) NOT NULL CONSTRAINT [df_sap_admin_division_culture] DEFAULT (''),
    [currency_code] VARCHAR(3) NOT NULL CONSTRAINT [df_sap_admin_division_currency] DEFAULT (''),
    [phone_code] VARCHAR(16) NOT NULL CONSTRAINT [df_sap_admin_division_phone] DEFAULT (''),
    [is_built_in] INT NOT NULL CONSTRAINT [df_sap_admin_division_built_in] DEFAULT (0),
    [sort_order] INT NOT NULL CONSTRAINT [df_sap_admin_division_sort] DEFAULT (0),
    [division_status] INT NOT NULL CONSTRAINT [df_sap_admin_division_status] DEFAULT (1),
    CONSTRAINT [pk_sap_admin_division] PRIMARY KEY CLUSTERED ([id])
  );
  CREATE UNIQUE INDEX [ux_sap_admin_division_code]
    ON [dbo].[takt_foundation_admin_division] ([division_code]);
  CREATE INDEX [ix_sap_admin_division_parent]
    ON [dbo].[takt_foundation_admin_division] ([parent_id]);
  CREATE INDEX [ix_sap_admin_division_country_level]
    ON [dbo].[takt_foundation_admin_division] ([country_code], [level]);
END
GO

-- ----------------------------------------
-- BOM 物料成本明细 / 汇总（sync_bc / sync_bv）
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_manufacturing_bom_material_cost_item] (
    [plant_code] NVARCHAR(4) NOT NULL,
    [product_code] NVARCHAR(20) NOT NULL,
    [sequence_code] NVARCHAR(4) NOT NULL,
    [product_description] NVARCHAR(40) NULL,
    [bom_level] NVARCHAR(20) NULL,
    [bom_item_code] NVARCHAR(4) NOT NULL,
    [component_code] NVARCHAR(20) NOT NULL,
    [component_description] NVARCHAR(40) NULL,
    [component_quantity] DECIMAL(18,2) NULL,
    [batch_indicator] NVARCHAR(1) NULL,
    [production_related] NVARCHAR(1) NULL,
    [purchase_type] NVARCHAR(1) NULL,
    [special_procurement_type] NVARCHAR(50) NULL,
    [profit_center_code] NVARCHAR(4) NULL,
    [moving_average_price] DECIMAL(18,5) NULL,
    [moving_price_unit] INT NULL,
    [moving_price_currency_code] NVARCHAR(3) NULL,
    [purchase_organization] NVARCHAR(4) NULL,
    [purchase_group] NVARCHAR(3) NULL,
    [supplier_code] NVARCHAR(10) NULL,
    [net_purchase_price] DECIMAL(18,5) NULL,
    [purchase_price_unit] INT NULL,
    [purchase_currency_code] NVARCHAR(3) NULL,
    [costing_date] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_bom_material_cost_item]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost_item]
    ([plant_code], [product_code], [sequence_code], [bom_level], [bom_item_code], [component_code],
     [component_quantity], [batch_indicator], [production_related], [purchase_type],
     [special_procurement_type], [costing_date]);
END
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

-- ----------------------------------------
-- 采购价格四级（sync_pup）
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_price', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_price] (
    [company_code] NVARCHAR(4) NULL,
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
    ON [dbo].[takt_logistics_procurement_purchase_price] ([company_code], [plant_code], [purchase_price_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_price_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_price_item] (
    [company_code] NVARCHAR(4) NULL,
    [purchase_price_code] NVARCHAR(20) NOT NULL,
    [purchase_price_seq] INT NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [scale_type] NVARCHAR(1) NULL,
    [scale_basis] NVARCHAR(1) NULL,
    [scale_quantity] DECIMAL(18,4) NULL,
    [scale_unit] NVARCHAR(5) NULL,
    [scale_value] DECIMAL(18,5) NULL,
    [scale_currency_code] NVARCHAR(3) NULL,
    [calculation_type] NVARCHAR(1) NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [condition_currency_code] NVARCHAR(3) NULL,
    [price_unit] INT NULL,
    [unit_of_measure] NVARCHAR(5) NULL,
    [min_order_quantity] INT NULL,
    [rounding_value] INT NULL,
    [planned_delivery_time_days] INT NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price_item]
    ON [dbo].[takt_logistics_procurement_purchase_price_item] ([company_code], [purchase_price_code], [purchase_price_seq]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_price_scale_quantity', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_price_scale_quantity] (
    [company_code] NVARCHAR(4) NULL,
    [purchase_price_code] NVARCHAR(20) NOT NULL,
    [purchase_price_seq] INT NOT NULL,
    [purchase_scale_seq] INT NOT NULL,
    [scale_quantity] DECIMAL(18,4) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price_scale_qty]
    ON [dbo].[takt_logistics_procurement_purchase_price_scale_quantity]
    ([company_code], [purchase_price_code], [purchase_price_seq], [purchase_scale_seq], [scale_quantity]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_price_scale_value', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_price_scale_value] (
    [company_code] NVARCHAR(4) NULL,
    [purchase_price_code] NVARCHAR(20) NOT NULL,
    [purchase_price_seq] INT NOT NULL,
    [purchase_scale_seq] INT NOT NULL,
    [scale_value] DECIMAL(18,5) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price_scale_val]
    ON [dbo].[takt_logistics_procurement_purchase_price_scale_value]
    ([company_code], [purchase_price_code], [purchase_price_seq], [purchase_scale_seq], [scale_value]);
END
GO

-- ----------------------------------------
-- 销售价格四级（sync_sp）
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_sales_price', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_price] (
    [company_code] NVARCHAR(4) NULL,
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
    ON [dbo].[takt_logistics_sales_price] ([company_code], [plant_code], [sales_price_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_price_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_price_item] (
    [company_code] NVARCHAR(4) NULL,
    [sales_price_code] NVARCHAR(20) NOT NULL,
    [sales_price_seq] INT NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [scale_type] NVARCHAR(1) NULL,
    [scale_basis] NVARCHAR(1) NULL,
    [scale_quantity] DECIMAL(18,4) NULL,
    [scale_unit] NVARCHAR(5) NULL,
    [scale_value] DECIMAL(18,5) NULL,
    [scale_currency_code] NVARCHAR(3) NULL,
    [calculation_type] NVARCHAR(1) NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [condition_currency_code] NVARCHAR(3) NULL,
    [price_unit] INT NULL,
    [unit_of_measure] NVARCHAR(5) NULL,
    [min_order_quantity] INT NULL,
    [rounding_value] INT NULL,
    [planned_delivery_time_days] INT NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price_item]
    ON [dbo].[takt_logistics_sales_price_item] ([company_code], [sales_price_code], [sales_price_seq]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_price_scale_quantity', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_price_scale_quantity] (
    [company_code] NVARCHAR(4) NULL,
    [sales_price_code] NVARCHAR(20) NOT NULL,
    [sales_price_seq] INT NOT NULL,
    [sales_scale_seq] INT NOT NULL,
    [scale_quantity] DECIMAL(18,4) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price_scale_qty]
    ON [dbo].[takt_logistics_sales_price_scale_quantity]
    ([company_code], [sales_price_code], [sales_price_seq], [sales_scale_seq], [scale_quantity]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_price_scale_value', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_price_scale_value] (
    [company_code] NVARCHAR(4) NULL,
    [sales_price_code] NVARCHAR(20) NOT NULL,
    [sales_price_seq] INT NOT NULL,
    [sales_scale_seq] INT NOT NULL,
    [scale_value] DECIMAL(18,5) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price_scale_val]
    ON [dbo].[takt_logistics_sales_price_scale_value]
    ([company_code], [sales_price_code], [sales_price_seq], [sales_scale_seq], [scale_value]);
END
GO

-- 已有价格主表补列 company_code 并重建唯一索引（幂等）
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_price', N'company_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_price] ADD [company_code] NVARCHAR(4) NULL;
GO
IF EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_price')
    AND name = N'ux_sap_purchase_price'
)
  DROP INDEX [ux_sap_purchase_price] ON [dbo].[takt_logistics_procurement_purchase_price];
GO
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_price')
    AND name = N'ux_sap_purchase_price'
)
  CREATE UNIQUE INDEX [ux_sap_purchase_price]
    ON [dbo].[takt_logistics_procurement_purchase_price] ([company_code], [plant_code], [purchase_price_code]);
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_price', N'company_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_price] ADD [company_code] NVARCHAR(4) NULL;
GO
IF EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_sales_price')
    AND name = N'ux_sap_sales_price'
)
  DROP INDEX [ux_sap_sales_price] ON [dbo].[takt_logistics_sales_price];
GO
IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_sales_price')
    AND name = N'ux_sap_sales_price'
)
  CREATE UNIQUE INDEX [ux_sap_sales_price]
    ON [dbo].[takt_logistics_sales_price] ([company_code], [plant_code], [sales_price_code]);
GO

-- 已有价格子表补列 company_code（幂等）
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_price_item', N'company_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_price_item] ADD [company_code] NVARCHAR(4) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_price_scale_quantity', N'company_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_price_scale_quantity] ADD [company_code] NVARCHAR(4) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_price_scale_value', N'company_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_price_scale_value] ADD [company_code] NVARCHAR(4) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_price_item', N'company_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_price_item] ADD [company_code] NVARCHAR(4) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_price_scale_quantity', N'company_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_price_scale_quantity] ADD [company_code] NVARCHAR(4) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_price_scale_value', N'company_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_price_scale_value] ADD [company_code] NVARCHAR(4) NULL;
GO

-- 已有价格表补列 tax_amount（幂等）
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_price_item', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_price_item] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_price_scale_quantity', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_price_scale_quantity] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_price_scale_value', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_price_scale_value] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_price_item', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_price_item] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_price_scale_quantity', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_price_scale_quantity] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_price_scale_value', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_price_scale_value] ADD [tax_amount] DECIMAL(18,5) NULL;
GO

-- ----------------------------------------
-- 供货商 / 客户信息（sync_sup / sync_cus）
-- 与实体 TaktSupplier 同名：takt_logistics_procurement_supplier
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_materials_supplier', N'U') IS NOT NULL
  AND OBJECT_ID(N'dbo.takt_logistics_procurement_supplier', N'U') IS NULL
BEGIN
  EXEC sp_rename N'dbo.takt_logistics_materials_supplier', N'takt_logistics_procurement_supplier';
  IF EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_procurement_supplier')
      AND name = N'ux_sap_materials_supplier'
  )
    EXEC sp_rename N'dbo.takt_logistics_procurement_supplier.ux_sap_materials_supplier', N'ux_sap_procurement_supplier', N'INDEX';
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_procurement_supplier', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_supplier] (
    [plant_code] NVARCHAR(4) NOT NULL,
    [supplier_code] NVARCHAR(10) NOT NULL,
    [supplier_name1] NVARCHAR(140) NOT NULL,
    [supplier_name2] NVARCHAR(140) NULL,
    [supplier_short_name] NVARCHAR(40) NULL,
    [supplier_type] INT NULL,
    [enterprise_nature] VARCHAR(4) NULL,
    [industry_attribute] VARCHAR(4) NULL,
    [default_culture] VARCHAR(5) NULL,
    [supplier_tax_number] NVARCHAR(50) NULL,
    [tax_rate] INT NULL,
    [registration_country] NVARCHAR(2) NULL,
    [registration_province] NVARCHAR(70) NULL,
    [registration_city] NVARCHAR(70) NULL,
    [registration_address1] NVARCHAR(140) NULL,
    [registration_address2] NVARCHAR(140) NULL,
    [supplier_phone] NVARCHAR(50) NULL,
    [supplier_fax] NVARCHAR(50) NULL,
    [supplier_email] NVARCHAR(100) NULL,
    [supplier_website] NVARCHAR(200) NULL,
    [contact_person] NVARCHAR(50) NULL,
    [contact_phone] NVARCHAR(50) NULL,
    [contact_email] NVARCHAR(100) NULL,
    [currency_code] NVARCHAR(3) NULL,
    [reconciliation_account] VARCHAR(40) NULL,
    [customer_code] NVARCHAR(10) NULL,
    [clearing_with_customer] INT NULL,
    [payment_method] INT NULL,
    [payment_terms] NVARCHAR(40) NULL,
    [bank_code] NVARCHAR(15) NULL,
    [bank_account] NVARCHAR(40) NULL,
    [account_holder] NVARCHAR(100) NULL,
    [gr_based_invoice_inspection] INT NULL,
    [incoterms1] VARCHAR(3) NULL,
    [incoterms2] NVARCHAR(40) NULL,
    [automatic_purchase_order] INT NULL,
    [pricing_date_control] INT NULL,
    [purchase_group] NVARCHAR(3) NULL,
    [planned_delivery_time_days] INT NULL,
    [evaluated_receipt_settlement] INT NULL,
    [purchasing_organization] VARCHAR(4) NULL,
    [supplier_level] INT NULL,
    [evaluation_score] DECIMAL(5,2) NULL,
    [sort_order] INT NULL,
    [supplier_status] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_procurement_supplier]
    ON [dbo].[takt_logistics_procurement_supplier] ([supplier_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_customer', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_customer] (
    [plant_code] NVARCHAR(4) NOT NULL,
    [customer_code] NVARCHAR(10) NOT NULL,
    [customer_name1] NVARCHAR(140) NOT NULL,
    [customer_name2] NVARCHAR(140) NULL,
    [customer_short_name] NVARCHAR(40) NULL,
    [customer_type] INT NULL,
    [enterprise_nature] VARCHAR(4) NULL,
    [industry_attribute] VARCHAR(4) NULL,
    [default_culture] VARCHAR(5) NULL,
    [customer_tax_number] NVARCHAR(50) NULL,
    [tax_rate] INT NULL,
    [registration_country] NVARCHAR(2) NULL,
    [registration_province] NVARCHAR(70) NULL,
    [registration_city] NVARCHAR(70) NULL,
    [registration_address1] NVARCHAR(140) NULL,
    [registration_address2] NVARCHAR(140) NULL,
    [customer_phone] NVARCHAR(50) NULL,
    [customer_fax] NVARCHAR(50) NULL,
    [customer_email] NVARCHAR(100) NULL,
    [customer_website] NVARCHAR(200) NULL,
    [contact_person] NVARCHAR(50) NULL,
    [contact_phone] NVARCHAR(50) NULL,
    [contact_email] NVARCHAR(100) NULL,
    [currency_code] NVARCHAR(3) NULL,
    [sales_organization] VARCHAR(4) NULL,
    [distribution_channel] VARCHAR(2) NULL,
    [product_group] VARCHAR(2) NULL,
    [customer_group] VARCHAR(2) NULL,
    [trading_partner] VARCHAR(4) NULL,
    [account_assignment_group] VARCHAR(2) NULL,
    [supplier_code] NVARCHAR(10) NULL,
    [nielsen_indicator] VARCHAR(2) NULL,
    [central_posting_block] INT NULL,
    [reconciliation_account] VARCHAR(40) NULL,
    [headquarters] NVARCHAR(20) NULL,
    [clearing_with_vendor] INT NULL,
    [payment_terms] NVARCHAR(40) NULL,
    [payment_method] INT NULL,
    [delivering_plant] VARCHAR(4) NULL,
    [incoterms1] VARCHAR(3) NULL,
    [incoterms2] NVARCHAR(40) NULL,
    [shipping_conditions] VARCHAR(2) NULL,
    [customer_pricing_procedure] VARCHAR(2) NULL,
    [credit_level] INT NULL,
    [credit_amount] DECIMAL(18,2) NULL,
    [discount_rate] DECIMAL(5,2) NULL,
    [sales_by] NVARCHAR(50) NULL,
    [customer_level] INT NULL,
    [evaluation_score] DECIMAL(5,2) NULL,
    [sort_order] INT NULL,
    [customer_status] INT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_customer]
    ON [dbo].[takt_logistics_sales_customer] ([plant_code], [customer_code]);
END
GO

-- ----------------------------------------
-- 物料凭证 / 采购发票 / 销售发票暂存表
-- 规则：表名与租户库相同；业务列与实体一致（源=目标业务列）
-- 不含 id/tenant/company/审计列（由 sync 脚本写入目标）
-- ----------------------------------------
-- 物料凭证主（sync_matdoc；业务列=TaktMaterialDocument）
IF OBJECT_ID(N'dbo.takt_logistics_materials_material_document', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_materials_material_document] (
    [material_document_code] NVARCHAR(10) NOT NULL,
    [material_document_year] NVARCHAR(4) NOT NULL,
    [transaction_event_type] NVARCHAR(2) NULL,
    [document_type] NVARCHAR(2) NULL,
    [revaluation_type] NVARCHAR(2) NULL,
    [document_date] DATETIME NOT NULL,
    [posting_date] DATETIME NOT NULL,
    [reference_code] NVARCHAR(16) NULL,
    [header_text] NVARCHAR(25) NULL,
    [transaction_code] NVARCHAR(4) NULL,
    [delivery_code] NVARCHAR(10) NULL,
    [posted_by] NVARCHAR(12) NULL
  );
  CREATE UNIQUE INDEX [ux_sap_material_document]
    ON [dbo].[takt_logistics_materials_material_document]
    ([material_document_year], [material_document_code]);
END
GO

-- 物料凭证明细（sync_matdoc；业务列=TaktMaterialDocumentItem，无 year；关联靠 material_document_id）
IF OBJECT_ID(N'dbo.takt_logistics_materials_material_document_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_materials_material_document_item] (
    [material_document_id] BIGINT NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [material_document_code] NVARCHAR(10) NOT NULL,
    [line_number] INT NOT NULL,
    [line_id] NVARCHAR(6) NULL,
    [parent_line_id] NVARCHAR(6) NULL,
    [line_depth] NVARCHAR(2) NULL,
    [account_assignment_original_line] INT NULL,
    [movement_type] NVARCHAR(3) NOT NULL,
    [auto_created_flag] NVARCHAR(1) NULL,
    [material_code] NVARCHAR(20) NOT NULL,
    [warehouse_code] NVARCHAR(4) NULL,
    [batch_code] NVARCHAR(10) NULL,
    [stock_type] NVARCHAR(1) NULL,
    [batch_status_key] NVARCHAR(1) NULL,
    [restricted_stock_flag] NVARCHAR(1) NULL,
    [special_stock] NVARCHAR(1) NULL,
    [supplier_code] NVARCHAR(10) NULL,
    [customer_code] NVARCHAR(10) NULL,
    [sales_order_code] NVARCHAR(20) NULL,
    [sales_order_item] INT NULL,
    [sales_order_schedule] INT NULL,
    [distribution_code] NVARCHAR(10) NULL,
    [debit_credit_indicator] NVARCHAR(1) NULL,
    [currency_code] NVARCHAR(3) NULL,
    [local_currency_amount] DECIMAL(13,2) NOT NULL,
    [delivery_cost_amount] DECIMAL(13,2) NULL,
    [alternative_amount] DECIMAL(13,2) NULL,
    [revaluation_debit_credit] NVARCHAR(1) NULL,
    [revaluation_amount] DECIMAL(13,2) NULL,
    [valuation_type] NVARCHAR(10) NULL,
    [quantity] DECIMAL(13,3) NOT NULL,
    [base_unit] NVARCHAR(3) NULL,
    [entry_quantity] DECIMAL(13,3) NULL,
    [entry_unit] NVARCHAR(3) NULL,
    [po_price_quantity] DECIMAL(13,3) NULL,
    [po_price_unit] NVARCHAR(3) NULL,
    [purchase_order_code] NVARCHAR(20) NULL,
    [purchase_order_item] INT NULL,
    [reference_document_year] NVARCHAR(4) NULL,
    [reference_document_code] NVARCHAR(10) NULL,
    [reference_document_item] INT NULL,
    [original_material_document_year] NVARCHAR(4) NULL,
    [original_material_document_code] NVARCHAR(10) NULL,
    [original_line_number] INT NULL,
    [delivery_completed_flag] NVARCHAR(1) NULL,
    [item_text] NVARCHAR(50) NULL,
    [equipment_code] NVARCHAR(18) NULL,
    [goods_recipient] NVARCHAR(12) NULL,
    [unloading_point] NVARCHAR(25) NULL,
    [business_area_code] NVARCHAR(4) NULL,
    [controlling_area_code] NVARCHAR(4) NULL,
    [trading_partner_business_area] NVARCHAR(4) NULL,
    [clearing_company_code] NVARCHAR(4) NULL,
    [cost_center_code] NVARCHAR(10) NULL,
    [legacy_project_code] NVARCHAR(16) NULL,
    [production_order_code] NVARCHAR(12) NULL,
    [asset_code] NVARCHAR(12) NULL,
    [asset_sub_code] NVARCHAR(4) NULL,
    [cost_center_stat_flag] NVARCHAR(1) NULL,
    [order_stat_flag] NVARCHAR(1) NULL,
    [project_stat_flag] NVARCHAR(1) NULL,
    [profitability_stat_flag] NVARCHAR(1) NULL,
    [fiscal_year] NVARCHAR(4) NULL,
    [post_to_previous_period_flag] NVARCHAR(1) NULL,
    [post_to_previous_year_flag] NVARCHAR(1) NULL,
    [accounting_document_code] NVARCHAR(10) NULL,
    [accounting_document_item] INT NULL,
    [revaluation_document_code] NVARCHAR(10) NULL,
    [revaluation_document_item] NVARCHAR(3) NULL,
    [reservation_code] NVARCHAR(10) NULL,
    [reservation_item] INT NULL,
    [final_issue_flag] NVARCHAR(1) NULL,
    [reservation_quantity] DECIMAL(13,3) NULL,
    [statistics_relevant_flag] NVARCHAR(1) NULL,
    [receiving_material_code] NVARCHAR(20) NULL,
    [receiving_plant_code] NVARCHAR(4) NULL,
    [receiving_warehouse_code] NVARCHAR(4) NULL,
    [goods_receipt_slip_count] INT NULL,
    [profit_center_code] NVARCHAR(10) NULL,
    [network_code] NVARCHAR(12) NULL,
    [routing_number] NVARCHAR(10) NULL,
    [routing_counter] NVARCHAR(8) NULL,
    [order_item_number] INT NULL,
    [gl_account_code] NVARCHAR(10) NULL,
    [order_unit_quantity] DECIMAL(13,3) NULL,
    [order_unit] NVARCHAR(3) NULL,
    [supplying_vendor_code] NVARCHAR(10) NULL,
    [partner_profit_center_code] NVARCHAR(10) NULL,
    [stock_managed_material_code] NVARCHAR(20) NULL,
    [receiving_stock_material_code] NVARCHAR(20) NULL,
    [quantity_string] NVARCHAR(4) NULL,
    [value_string] NVARCHAR(4) NULL,
    [quantity_update_flag] NVARCHAR(1) NULL,
    [value_update_flag] NVARCHAR(1) NULL,
    [valuated_stock_quantity] DECIMAL(13,3) NULL,
    [total_valuated_stock_value] DECIMAL(13,2) NULL,
    [price_control] NVARCHAR(1) NULL,
    [original_item_line] INT NULL,
    [manufacturer_part_material_code] NVARCHAR(40) NULL,
    [stock_type_modification] NVARCHAR(1) NULL,
    [transaction_event_type] NVARCHAR(2) NULL,
    [mkpf_reference_code] NVARCHAR(32) NULL,
    [mkpf_transaction_code2] NVARCHAR(40) NULL,
    [im_delivery_code] NVARCHAR(20) NULL,
    [im_delivery_item] INT NULL,
    [is_obsolete] INT NOT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_material_document_item]
    ON [dbo].[takt_logistics_materials_material_document_item]
    ([material_document_id], [line_number]);
END
GO

-- 采购发票主（sync_puinv；业务列=TaktPurchaseInvoice）
IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_invoice', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_invoice] (
    [purchase_invoice_code] NVARCHAR(10) NOT NULL,
    [fiscal_year] NVARCHAR(4) NOT NULL,
    [document_type] NVARCHAR(2) NULL,
    [document_date] DATETIME NOT NULL,
    [posting_date] DATETIME NOT NULL,
    [posted_by] NVARCHAR(12) NULL,
    [transaction_code] NVARCHAR(20) NULL,
    [transaction_event_type] NVARCHAR(2) NULL,
    [reference_code] NVARCHAR(16) NULL,
    [supplier_code] NVARCHAR(10) NOT NULL,
    [currency_code] NVARCHAR(3) NOT NULL,
    [exchange_rate] DECIMAL(9,5) NULL,
    [gross_amount] DECIMAL(13,2) NOT NULL,
    [vat_amount] DECIMAL(13,2) NULL,
    [tax_code] NVARCHAR(2) NULL,
    [payment_terms] NVARCHAR(4) NULL,
    [invoice_flag] NVARCHAR(1) NULL,
    [header_text] NVARCHAR(25) NULL,
    [calculate_tax_flag] NVARCHAR(1) NULL,
    [reversal_document_code] NVARCHAR(10) NULL,
    [reversal_fiscal_year] NVARCHAR(4) NULL,
    [invoice_verification_category] NVARCHAR(1) NULL,
    [invoice_verification_type] NVARCHAR(1) NULL,
    [invoice_status] NVARCHAR(1) NULL,
    [supplying_country] NVARCHAR(3) NULL,
    [scb_indicator] NVARCHAR(3) NULL,
    [tax_exchange_rate] DECIMAL(9,5) NULL,
    [payment_method] NVARCHAR(1) NULL,
    [baseline_date] DATETIME NULL,
    [entered_by] NVARCHAR(12) NULL,
    [branch_account] NVARCHAR(10) NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_invoice]
    ON [dbo].[takt_logistics_procurement_purchase_invoice]
    ([fiscal_year], [purchase_invoice_code]);
END
GO

-- 采购发票明细（sync_puinv；业务列=TaktPurchaseInvoiceItem + fiscal_year 关联键）
IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_invoice_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_invoice_item] (
    [fiscal_year] NVARCHAR(4) NOT NULL,
    [plant_code] NVARCHAR(4) NULL,
    [purchase_invoice_code] NVARCHAR(10) NOT NULL,
    [line_number] INT NOT NULL,
    [purchase_order_code] NVARCHAR(20) NULL,
    [purchase_order_item] INT NULL,
    [account_assignment_seq] NVARCHAR(2) NULL,
    [material_code] NVARCHAR(20) NULL,
    [valuation_area] NVARCHAR(4) NULL,
    [amount] DECIMAL(13,2) NULL,
    [debit_credit_indicator] NVARCHAR(1) NULL,
    [tax_code] NVARCHAR(2) NULL,
    [quantity] DECIMAL(13,3) NULL,
    [order_unit] NVARCHAR(3) NULL,
    [po_price_quantity] DECIMAL(13,3) NULL,
    [po_price_unit] NVARCHAR(3) NULL,
    [valuated_stock_quantity] DECIMAL(13,3) NULL,
    [previous_period_stock] DECIMAL(13,3) NULL,
    [base_unit] NVARCHAR(3) NULL,
    [item_category] NVARCHAR(1) NULL,
    [account_assignment_category] NVARCHAR(1) NULL,
    [valuation_class] NVARCHAR(4) NULL,
    [final_invoice_flag] NVARCHAR(1) NULL,
    [update_po_history_flag] NVARCHAR(1) NULL,
    [subsequent_debit_credit] NVARCHAR(1) NULL,
    [block_reason_quantity] NVARCHAR(1) NULL,
    [value_string] NVARCHAR(4) NULL,
    [reference_code] NVARCHAR(16) NULL,
    [return_posting_flag] NVARCHAR(1) NULL,
    [delivery_cost_share] DECIMAL(13,2) NULL,
    [total_valuated_stock_value] DECIMAL(13,2) NULL,
    [previous_period_value] DECIMAL(13,2) NULL,
    [reference_document_code] NVARCHAR(10) NULL,
    [reference_document_year] NVARCHAR(4) NULL,
    [reference_document_item] INT NULL,
    [stock_managed_material_code] NVARCHAR(20) NULL,
    [material_document_item] INT NULL,
    [is_obsolete] INT NOT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_invoice_item]
    ON [dbo].[takt_logistics_procurement_purchase_invoice_item]
    ([fiscal_year], [purchase_invoice_code], [line_number]);
END
GO

-- 销售发票主（sync_sdinv；业务列=TaktSalesInvoice）
IF OBJECT_ID(N'dbo.takt_logistics_sales_invoice', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_invoice] (
    [billing_document_code] NVARCHAR(10) NOT NULL,
    [billing_type] NVARCHAR(4) NULL,
    [billing_category] NVARCHAR(1) NULL,
    [document_category] NVARCHAR(1) NULL,
    [currency_code] NVARCHAR(3) NOT NULL,
    [sales_organization] NVARCHAR(4) NULL,
    [distribution_channel] NVARCHAR(2) NULL,
    [pricing_procedure] NVARCHAR(6) NULL,
    [condition_code] NVARCHAR(10) NULL,
    [shipping_conditions] NVARCHAR(2) NULL,
    [billing_date] DATETIME NOT NULL,
    [customer_group] NVARCHAR(2) NULL,
    [price_list_type] NVARCHAR(2) NULL,
    [incoterms1] NVARCHAR(3) NULL,
    [export_flag] NVARCHAR(1) NULL,
    [posting_status] NVARCHAR(1) NULL,
    [accounting_exchange_rate] DECIMAL(9,5) NULL,
    [fixed_exchange_rate_flag] NVARCHAR(1) NULL,
    [payment_terms] NVARCHAR(4) NULL,
    [payment_method] NVARCHAR(1) NULL,
    [account_assignment_group] NVARCHAR(2) NULL,
    [country_code] NVARCHAR(3) NULL,
    [region] NVARCHAR(3) NULL,
    [customer_tax_class1] NVARCHAR(1) NULL,
    [net_amount] DECIMAL(15,2) NOT NULL,
    [combination_criteria] NVARCHAR(40) NULL,
    [posted_by] NVARCHAR(12) NULL,
    [payer_code] NVARCHAR(10) NULL,
    [customer_code] NVARCHAR(10) NOT NULL,
    [dunning_area] NVARCHAR(2) NULL,
    [statistics_currency_code] NVARCHAR(3) NULL,
    [foreign_trade_code] NVARCHAR(10) NULL,
    [cancelled_billing_document] NVARCHAR(10) NULL,
    [agreement_code] NVARCHAR(10) NULL,
    [invoice_list_type] NVARCHAR(4) NULL,
    [invoice_list_date] DATETIME NULL,
    [exchange_rate_type] NVARCHAR(4) NULL,
    [dunning_key] NVARCHAR(1) NULL,
    [dunning_block] NVARCHAR(1) NULL,
    [division] NVARCHAR(2) NULL,
    [credit_control_area] NVARCHAR(4) NULL,
    [credit_account] NVARCHAR(10) NULL,
    [credit_currency_code] NVARCHAR(3) NULL,
    [credit_exchange_rate] DECIMAL(9,5) NULL,
    [hierarchy_type_pricing] NVARCHAR(1) NULL,
    [customer_purchase_order] NVARCHAR(35) NULL,
    [trading_partner] NVARCHAR(6) NULL,
    [tax_departure_country] NVARCHAR(3) NULL,
    [organization_sales_tax_number] NVARCHAR(20) NULL,
    [country_sales_tax_number] NVARCHAR(20) NULL,
    [reference_code] NVARCHAR(16) NULL,
    [assignment] NVARCHAR(18) NULL,
    [tax_amount] DECIMAL(13,2) NULL,
    [logical_system] NVARCHAR(10) NULL,
    [cancelled_flag] NVARCHAR(1) NULL,
    [exchange_rate_date] DATETIME NULL,
    [payment_reference] NVARCHAR(30) NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_invoice]
    ON [dbo].[takt_logistics_sales_invoice]
    ([billing_document_code]);
END
GO

-- 销售发票明细（sync_sdinv；业务列=TaktSalesInvoiceItem）
IF OBJECT_ID(N'dbo.takt_logistics_sales_invoice_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_invoice_item] (
    [plant_code] NVARCHAR(4) NULL,
    [billing_document_code] NVARCHAR(10) NOT NULL,
    [line_number] INT NOT NULL,
    [billing_quantity] DECIMAL(13,3) NULL,
    [sales_unit] NVARCHAR(3) NULL,
    [numerator] DECIMAL(5,0) NULL,
    [denominator] DECIMAL(5,0) NULL,
    [base_unit] NVARCHAR(3) NULL,
    [scale_quantity] DECIMAL(13,3) NULL,
    [billing_quantity_sku] DECIMAL(13,3) NULL,
    [required_quantity] DECIMAL(13,3) NULL,
    [net_weight] DECIMAL(15,3) NULL,
    [gross_weight] DECIMAL(15,3) NULL,
    [weight_unit] NVARCHAR(3) NULL,
    [volume] DECIMAL(15,3) NULL,
    [volume_unit] NVARCHAR(3) NULL,
    [business_area_code] NVARCHAR(4) NULL,
    [pricing_date] DATETIME NULL,
    [service_rendered_date] DATETIME NULL,
    [pricing_exchange_rate] DECIMAL(9,5) NULL,
    [net_amount] DECIMAL(15,2) NULL,
    [origin_document_code] NVARCHAR(10) NULL,
    [origin_document_item] INT NULL,
    [reference_document_code] NVARCHAR(10) NULL,
    [reference_document_item] INT NULL,
    [reference_document_category] NVARCHAR(1) NULL,
    [sales_document_code] NVARCHAR(20) NULL,
    [sales_document_item] INT NULL,
    [sales_document_reference_flag] NVARCHAR(1) NULL,
    [material_code] NVARCHAR(20) NOT NULL,
    [material_description] NVARCHAR(40) NULL,
    [material_group] NVARCHAR(9) NULL,
    [sales_item_category] NVARCHAR(4) NULL,
    [item_type] NVARCHAR(1) NULL,
    [product_hierarchy] NVARCHAR(18) NULL,
    [shipping_point] NVARCHAR(4) NULL,
    [replacement_part_flag] NVARCHAR(1) NULL,
    [division] NVARCHAR(2) NULL,
    [partner_item] INT NULL,
    [departure_country] NVARCHAR(3) NULL,
    [plant_region] NVARCHAR(3) NULL,
    [statistical_value_flag] NVARCHAR(1) NULL,
    [pricing_flag] NVARCHAR(1) NULL,
    [cash_discount_flag] NVARCHAR(1) NULL,
    [cash_discount_base] DECIMAL(13,2) NULL,
    [cost_center_code] NVARCHAR(10) NULL,
    [sales_office] NVARCHAR(4) NULL,
    [division_for_order] NVARCHAR(2) NULL,
    [debit_credit_indicator] NVARCHAR(1) NULL,
    [posted_by] NVARCHAR(12) NULL,
    [valuation_type] NVARCHAR(10) NULL,
    [warehouse_code] NVARCHAR(4) NULL,
    [cost_amount] DECIMAL(13,2) NULL,
    [subtotal1] DECIMAL(13,2) NULL,
    [subtotal2] DECIMAL(13,2) NULL,
    [subtotal3] DECIMAL(13,2) NULL,
    [subtotal4] DECIMAL(13,2) NULL,
    [subtotal5] DECIMAL(13,2) NULL,
    [subtotal6] DECIMAL(13,2) NULL,
    [statistics_exchange_rate] DECIMAL(9,5) NULL,
    [international_article_number] NVARCHAR(18) NULL,
    [profit_center_code] NVARCHAR(10) NULL,
    [material_group4] NVARCHAR(3) NULL,
    [entered_material_code] NVARCHAR(20) NULL,
    [controlling_area_code] NVARCHAR(4) NULL,
    [profitability_segment] NVARCHAR(10) NULL,
    [credit_price] DECIMAL(11,2) NULL,
    [credit_active_flag] NVARCHAR(1) NULL,
    [customer_group_sales_order] NVARCHAR(2) NULL,
    [destination_country_order] NVARCHAR(3) NULL,
    [manual_pricing_flag] NVARCHAR(1) NULL,
    [price_list_order] NVARCHAR(2) NULL,
    [region_order] NVARCHAR(3) NULL,
    [sales_organization_order] NVARCHAR(4) NULL,
    [distribution_channel_order] NVARCHAR(2) NULL,
    [document_category] NVARCHAR(1) NULL,
    [tax_amount] DECIMAL(13,2) NULL,
    [order_reason] NVARCHAR(3) NULL,
    [payment_guarantee_form] NVARCHAR(2) NULL,
    [gross_amount] DECIMAL(15,2) NULL,
    [exchange_rate_date] DATETIME NULL,
    [is_obsolete] INT NOT NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_invoice_item]
    ON [dbo].[takt_logistics_sales_invoice_item]
    ([billing_document_code], [line_number]);
END
GO

