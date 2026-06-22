// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Planning
// 文件名称：TaktPlannedOrder.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：计划订单，MPS 行下推至 APS 的中间层（Planned Order）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Planning;

/// <summary>
/// 计划订单（MPS 净需求固化为可排程计划订单，下推 APS_Order）
/// </summary>
[SugarTable("takt_logistics_manufacturing_planning_planned_order", "计划订单表")]
[SugarIndex("ix_planned_order_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_planned_order_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_planning_planned_order_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PlannedOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_planning_planned_order_mps_line", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MasterProductionScheduleLineId), OrderByType.Asc, false)]
public class TaktPlannedOrder : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单编码
    /// </summary>
    [SugarColumn(ColumnName = "planned_order_code", ColumnDescription = "计划订单编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MPS 头表 ID
    /// </summary>
    [SugarColumn(ColumnName = "master_production_schedule_id", ColumnDescription = "来源MPS头表ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// 来源 MPS 行 ID
    /// </summary>
    [SugarColumn(ColumnName = "master_production_schedule_line_id", ColumnDescription = "来源MPS行ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量
    /// </summary>
    [SugarColumn(ColumnName = "planned_quantity", ColumnDescription = "计划数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PlannedQuantity { get; set; } = 0;

    /// <summary>
    /// 计量单位
    /// </summary>
    [SugarColumn(ColumnName = "unit_of_measure", ColumnDescription = "计量单位", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "PC")]
    public string UnitOfMeasure { get; set; } = "PC";

    /// <summary>
    /// 计划开始时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_start_time", ColumnDescription = "计划开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_end_time", ColumnDescription = "计划结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 工艺路线编码（关联 TaktRouting.RoutingCode）
    /// </summary>
    [SugarColumn(ColumnName = "routing_code", ColumnDescription = "工艺路线编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? RoutingCode { get; set; }

    /// <summary>
    /// 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
    /// </summary>
    [SugarColumn(ColumnName = "order_status", ColumnDescription = "计划订单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OrderStatus { get; set; } = 0;
}
