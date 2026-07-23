-- ========================================
-- Sap_Data 行政区划暂存表 DDL（手工执行，非 Quartz 任务）
-- 表名与租户库目标表相同（仅库不同：Sap_Data vs Takt_{Tenant}_*）
-- 对齐：sap_sync_ad.sql / TaktAdminDivision（含 id/parent_id 树关系）
-- ========================================

USE [Sap_Data];
GO

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
    [postal_code] NVARCHAR(20) NULL,
    [culture_code] VARCHAR(5) NOT NULL CONSTRAINT [df_sap_admin_division_culture] DEFAULT (''),
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
