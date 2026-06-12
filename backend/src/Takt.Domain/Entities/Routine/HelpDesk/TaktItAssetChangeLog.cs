// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktItAssetChangeLog.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：IT 设备保修扩展变更日志实体
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// IT 设备保修扩展变更日志实体
/// </summary>
[SugarTable("takt_routine_help_desk_it_asset_change_log", "IT设备保修变更日志表")]
[SugarIndex("ix_it_asset_change_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_it_asset_change_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_it_asset_change_log_it_asset_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ItAssetId), OrderByType.Asc, false)]
[SugarIndex("ix_it_asset_change_log_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
public class TaktItAssetChangeLog : TaktCompanyEntityBase
{
    /// <summary>
    /// IT 设备保修扩展 ID
    /// </summary>
    [SugarColumn(ColumnName = "it_asset_id", ColumnDescription = "IT设备保修扩展ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ItAssetId { get; set; }

    /// <summary>
    /// 资产号码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "asset_code", ColumnDescription = "资产号码", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? AssetCode { get; set; }

    /// <summary>
    /// 变更类型（见 TaktHelpDeskChangeType）
    /// </summary>
    [SugarColumn(ColumnName = "change_type", ColumnDescription = "变更类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ChangeType { get; set; } = 1;

    /// <summary>
    /// 修改内容摘要
    /// </summary>
    [SugarColumn(ColumnName = "change_summary", ColumnDescription = "修改内容摘要", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ChangeSummary { get; set; }

    /// <summary>
    /// 变更字段列表（JSON 数组）
    /// </summary>
    [SugarColumn(ColumnName = "change_fields", ColumnDescription = "变更字段列表", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? ChangeFields { get; set; }

    /// <summary>
    /// 变更原因或备注
    /// </summary>
    [SugarColumn(ColumnName = "change_reason", ColumnDescription = "变更原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ChangeReason { get; set; }

    /// <summary>
    /// IT 设备保修扩展（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ItAssetId))]
    public TaktItAsset? ItAsset { get; set; }
}
