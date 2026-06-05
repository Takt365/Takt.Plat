// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktAssetChangeLog.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：资产变更记录实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 资产变更记录实体
/// </summary>
[SugarTable("takt_accounting_financial_asset_change_log", "资产变更记录表")]
[SugarIndex("ix_asset_change_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_asset_change_log_asset", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AssetId), OrderByType.Asc, false)]
[SugarIndex("ix_asset_change_log_change_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ChangeTime), OrderByType.Desc, false)]
public class TaktAssetChangeLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 资产 ID
    /// </summary>
    [SugarColumn(ColumnName = "asset_id", ColumnDescription = "资产ID", ColumnDataType = "bigint", IsNullable = false)]
    public long AssetId { get; set; }
    /// <summary>
    /// 资产编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "asset_code", ColumnDescription = "资产编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string AssetCode { get; set; } = string.Empty;
    /// <summary>
    /// 变更字段列表 JSON
    /// </summary>
    [SugarColumn(ColumnName = "change_fields", ColumnDescription = "变更字段列表", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? ChangeFields { get; set; }
    /// <summary>
    /// 变更时间
    /// </summary>
    [SugarColumn(ColumnName = "change_time", ColumnDescription = "变更时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ChangeTime { get; set; } = DateTime.Now;
    /// <summary>
    /// 变更人
    /// </summary>
    [SugarColumn(ColumnName = "change_by", ColumnDescription = "变更人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ChangeBy { get; set; }
    /// <summary>
    /// 变更原因
    /// </summary>
    [SugarColumn(ColumnName = "change_reason", ColumnDescription = "变更原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ChangeReason { get; set; }
}
