// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Planning
// 文件名称：TaktPurchasePlanItem.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购计划明细实体，MRP 计划采购行（原材料/外购件）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Planning;

/// <summary>
/// Takt采购计划明细实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_planning_purchase_plan_item", "采购计划明细表")]
[SugarIndex("ix_purchase_plan_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_plan_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_planning_purchase_plan_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePlanId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_planning_purchase_plan_item_plan_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePlanCode), OrderByType.Asc, false)]
public class TaktPurchasePlanItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_id", ColumnDescription = "采购计划ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePlanId { get; set; }

    /// <summary>
    /// 采购计划编码（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_code", ColumnDescription = "采购计划编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "production_plan_id", ColumnDescription = "来源生产计划ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 来源生产计划编码
    /// </summary>
    [SugarColumn(ColumnName = "production_plan_code", ColumnDescription = "来源生产计划编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ProductionPlanCode { get; set; }

    /// <summary>
    /// 来源生产计划行号
    /// </summary>
    [SugarColumn(ColumnName = "production_plan_line_number", ColumnDescription = "来源生产计划行号", ColumnDataType = "int", IsNullable = true)]
    public int? ProductionPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    [SugarColumn(ColumnName = "material_name", ColumnDescription = "物料名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 计划单位
    /// </summary>
    [SugarColumn(ColumnName = "plan_unit", ColumnDescription = "计划单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string PlanUnit { get; set; } = "PC";

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "plan_quantity", ColumnDescription = "计划数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PlanQuantity { get; set; } = 0;

    /// <summary>
    /// 计划到货日期
    /// </summary>
    [SugarColumn(ColumnName = "planned_arrival_date", ColumnDescription = "计划到货日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedArrivalDate { get; set; }

    /// <summary>
    /// 已转申请/订单数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转申请订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; } = 0;

    /// <summary>
    /// 预计单价
    /// </summary>
    [SugarColumn(ColumnName = "estimated_unit_price", ColumnDescription = "预计单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EstimatedUnitPrice { get; set; } = 0;

    /// <summary>
    /// 预计金额
    /// </summary>
    [SugarColumn(ColumnName = "estimated_amount", ColumnDescription = "预计金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EstimatedAmount { get; set; } = 0;

    /// <summary>
    /// 参考供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "reference_supplier_code", ColumnDescription = "参考供货商编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ReferenceSupplierCode { get; set; }

    /// <summary>
    /// 参考供货商名称
    /// </summary>
    [SugarColumn(ColumnName = "reference_supplier_name", ColumnDescription = "参考供货商名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ReferenceSupplierName { get; set; }
}
