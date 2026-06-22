// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Planning
// 文件名称：TaktMasterProductionScheduleLine.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：主生产计划 MPS 行，按物料与时间桶记录净需求与 ATP
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Planning;

/// <summary>
/// 主生产计划 MPS 行（物料 + 时间桶 + ATP）
/// </summary>
[SugarTable("takt_logistics_manufacturing_planning_master_production_schedule_line", "主生产计划MPS行表")]
[SugarIndex("ix_mps_line_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_mps_line_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_planning_mps_line_bucket_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MasterProductionScheduleId), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(BucketStart), OrderByType.Asc, true)]
public class TaktMasterProductionScheduleLine : TaktCompanyEntityBase
{
    /// <summary>
    /// MPS 头表 ID（主子表关系）
    /// </summary>
    [SugarColumn(ColumnName = "master_production_schedule_id", ColumnDescription = "MPS头表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "mps_code", ColumnDescription = "MPS编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 行 ID（可选）
    /// </summary>
    [SugarColumn(ColumnName = "master_demand_schedule_line_id", ColumnDescription = "来源MDS行ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    [SugarColumn(ColumnName = "bucket_start", ColumnDescription = "时间桶开始", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    [SugarColumn(ColumnName = "bucket_end", ColumnDescription = "时间桶结束", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime BucketEnd { get; set; }

    /// <summary>
    /// 毛需求数量
    /// </summary>
    [SugarColumn(ColumnName = "gross_requirement", ColumnDescription = "毛需求数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal GrossRequirement { get; set; } = 0;

    /// <summary>
    /// 预计入库（计划接收）
    /// </summary>
    [SugarColumn(ColumnName = "scheduled_receipts", ColumnDescription = "预计入库", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ScheduledReceipts { get; set; } = 0;

    /// <summary>
    /// 预计可用库存（期初预计库存）
    /// </summary>
    [SugarColumn(ColumnName = "projected_on_hand", ColumnDescription = "预计可用库存", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ProjectedOnHand { get; set; } = 0;

    /// <summary>
    /// 净需求数量
    /// </summary>
    [SugarColumn(ColumnName = "net_requirement", ColumnDescription = "净需求数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal NetRequirement { get; set; } = 0;

    /// <summary>
    /// 计划订单数量（MPS 产出）
    /// </summary>
    [SugarColumn(ColumnName = "planned_order_quantity", ColumnDescription = "计划订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PlannedOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 可承诺量 ATP
    /// </summary>
    [SugarColumn(ColumnName = "atp_quantity", ColumnDescription = "可承诺量ATP", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal AtpQuantity { get; set; } = 0;

    /// <summary>
    /// 计量单位
    /// </summary>
    [SugarColumn(ColumnName = "unit_of_measure", ColumnDescription = "计量单位", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "PC")]
    public string UnitOfMeasure { get; set; } = "PC";
}
