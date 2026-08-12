-- ========================================
-- 迁移：takt_logistics_materials_material_moving_price.period_date → valuation_period (yyyy-MM)
-- 适用：zTakt_900_Dev、各租户库（如 zTakt_000_Dev）；年分表 *_yyyy 同结构时一并执行
-- 执行前请备份。幂等：已存在 valuation_period 且无 period_date 时跳过主体。
-- ========================================

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @table sysname = N'takt_logistics_materials_material_moving_price';
DECLARE @sql nvarchar(max);

IF OBJECT_ID(N'dbo.' + @table, N'U') IS NULL
BEGIN
  RAISERROR(N'表不存在: %s', 16, 1, @table);
  RETURN;
END;

BEGIN TRAN;

-- 1) 增加 valuation_period（若尚无）
IF COL_LENGTH(N'dbo.' + @table, N'valuation_period') IS NULL
BEGIN
  SET @sql = N'ALTER TABLE dbo.' + QUOTENAME(@table) + N' ADD [valuation_period] nvarchar(7) NULL;';
  EXEC sp_executesql @sql;
END;

-- 2) 从 period_date 回填 yyyy-MM（源列仍在时）
IF COL_LENGTH(N'dbo.' + @table, N'period_date') IS NOT NULL
BEGIN
  SET @sql = N'UPDATE dbo.' + QUOTENAME(@table)
    + N' SET [valuation_period] = CONVERT(char(7), [period_date], 126)'
    + N' WHERE [valuation_period] IS NULL AND [period_date] IS NOT NULL;';
  EXEC sp_executesql @sql;
END;

-- 3) 禁止空值
SET @sql = N'ALTER TABLE dbo.' + QUOTENAME(@table)
  + N' ALTER COLUMN [valuation_period] nvarchar(7) NOT NULL;';
EXEC sp_executesql @sql;

-- 4) 重建唯一索引：去掉 period_date，换 valuation_period
IF EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.' + @table)
    AND name = N'ix_material_moving_price_unique'
)
BEGIN
  SET @sql = N'DROP INDEX [ix_material_moving_price_unique] ON dbo.' + QUOTENAME(@table) + N';';
  EXEC sp_executesql @sql;
END;

SET @sql = N'CREATE UNIQUE NONCLUSTERED INDEX [ix_material_moving_price_unique] ON dbo.'
  + QUOTENAME(@table)
  + N' ([tenant_code] ASC, [company_code] ASC, [plant_code] ASC, [valuation_period] ASC, [material_code] ASC, [valuation] ASC);';
EXEC sp_executesql @sql;

-- 5) 期间索引
IF EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.' + @table)
    AND name = N'ix_material_moving_price_period'
)
BEGIN
  SET @sql = N'DROP INDEX [ix_material_moving_price_period] ON dbo.' + QUOTENAME(@table) + N';';
  EXEC sp_executesql @sql;
END;

IF EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.' + @table)
    AND name = N'ix_material_moving_price_costing_period'
)
BEGIN
  SET @sql = N'DROP INDEX [ix_material_moving_price_costing_period] ON dbo.' + QUOTENAME(@table) + N';';
  EXEC sp_executesql @sql;
END;

IF NOT EXISTS (
  SELECT 1 FROM sys.indexes
  WHERE object_id = OBJECT_ID(N'dbo.' + @table)
    AND name = N'ix_material_moving_price_valuation_period'
)
BEGIN
  SET @sql = N'CREATE NONCLUSTERED INDEX [ix_material_moving_price_valuation_period] ON dbo.'
    + QUOTENAME(@table)
    + N' ([tenant_code] ASC, [company_code] ASC, [valuation_period] ASC);';
  EXEC sp_executesql @sql;
END;

-- 6) 删除 period_date
IF COL_LENGTH(N'dbo.' + @table, N'period_date') IS NOT NULL
BEGIN
  SET @sql = N'ALTER TABLE dbo.' + QUOTENAME(@table) + N' DROP COLUMN [period_date];';
  EXEC sp_executesql @sql;
END;

COMMIT TRAN;

PRINT N'OK: period_date → valuation_period on ' + @table;
