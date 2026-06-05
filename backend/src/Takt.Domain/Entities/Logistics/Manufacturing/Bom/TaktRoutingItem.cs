#nullable enable
// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItem.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线明细表实体，定义工艺路线的工序序列
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线明细表实体（工序序列）
/// </summary>
[SugarTable("takt_logistics_manufacturing_bom_routing_item", "工艺路线明细表")]
[SugarIndex("ix_routing_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_routing_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoutingId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
public class TaktRoutingItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "routing_id", ColumnDescription = "工艺路线ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingId { get; set; }

    /// <summary>
    /// 工艺路线编码（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "routing_code", ColumnDescription = "工艺路线编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 作业/工序计量单位（PC或EA）
    /// </summary>
    [SugarColumn(ColumnName = "base_unit", ColumnDescription = "计量单位", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "PC")]
    public string BaseUnit { get; set; } = "PC";

    /// <summary>
    /// 基本数量
    /// </summary>
    [SugarColumn(ColumnName = "base_quantity", ColumnDescription = "基本数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "1")]
    public decimal BaseQuantity { get; set; } = 1;

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "standard_minutes", ColumnDescription = "标准工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StandardMinutes { get; set; } = 0;

    /// <summary>
    /// 工时单位
    /// </summary>
    [SugarColumn(ColumnName = "time_unit", ColumnDescription = "工时单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "MIN")]
    public string TimeUnit { get; set; } = "MIN";

    /// <summary>
    /// 标准点数
    /// </summary>
    [SugarColumn(ColumnName = "standard_shorts", ColumnDescription = "标准点数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int StandardShorts { get; set; } = 0;

    /// <summary>
    /// 点数单位
    /// </summary>
    [SugarColumn(ColumnName = "points_unit", ColumnDescription = "点数单位", ColumnDataType = "nvarchar", Length = 5, IsNullable = false, DefaultValue = "SHORT")]
    public string PointsUnit { get; set; } = "SHORT";

    /// <summary>
    /// 点数转分钟汇率（1 点数 = 多少分钟）
    /// </summary>
    [SugarColumn(ColumnName = "points_to_minutes_rate", ColumnDescription = "转换汇率", ColumnDataType = "decimal", Length = 8, DecimalDigits = 4, IsNullable = false, DefaultValue = "1")]
    public decimal PointsToMinutesRate { get; set; } = 1;

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "converted_minutes", ColumnDescription = "转换工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedMinutes { get; set; } = 0;

    /// <summary>
    /// 准备时间（分钟），如换模、调试等
    /// </summary>
    [SugarColumn(ColumnName = "setup_minutes", ColumnDescription = "准备时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SetupMinutes { get; set; } = 0;

    /// <summary>
    /// 清理时间（分钟），如清洁、整理等
    /// </summary>
    [SugarColumn(ColumnName = "teardown_minutes", ColumnDescription = "清理时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TeardownMinutes { get; set; } = 0;

    /// <summary>
    /// 是否质量检验点
    /// </summary>
    [SugarColumn(ColumnName = "is_quality_check", ColumnDescription = "是否质检点", ColumnDataType = "bit", IsNullable = false, DefaultValue = "0")]
    public bool IsQualityCheck { get; set; } = false;

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工序说明
    /// </summary>
    [SugarColumn(ColumnName = "process_description", ColumnDescription = "工序说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ProcessDescription { get; set; }

    /// <summary>
    /// 工艺路线主表（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RoutingId))]
    public TaktRouting? Routing { get; set; }
}
