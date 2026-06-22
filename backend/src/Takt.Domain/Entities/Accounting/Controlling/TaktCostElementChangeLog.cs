// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Accounting.Controlling
// 文件名称：TaktCostElementChangeLog.cs
// 创建时间：2025-03-16
// 创建人：Takt365(Cursor AI)
// 功能描述：成本要素变更记录实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Controlling;

/// <summary>
/// 成本要素变更记录实体
/// </summary>
[SugarTable("takt_accounting_controlling_cost_element_change_log", "成本要素变更记录表")]
[SugarIndex("ix_cost_element_change_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_cost_element_change_log_cost_element", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CostElementId), OrderByType.Asc, false)]
[SugarIndex("ix_cost_element_change_log_change_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ChangeTime), OrderByType.Desc, false)]
public class TaktCostElementChangeLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 成本要素 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_id", ColumnDescription = "成本要素ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostElementId { get; set; }
    /// <summary>
    /// 成本要素编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_code", ColumnDescription = "成本要素编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string CostElementCode { get; set; } = string.Empty;
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
    /// <summary>
    /// 成本要素主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(CostElementId))]
    public TaktCostElement? CostElement { get; set; }
}
