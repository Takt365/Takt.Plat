// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseRequestItem.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购申请明细实体，定义采购申请明细领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购申请明细实体
/// </summary>
[SugarTable("takt_logistics_materials_purchase_request_item", "采购申请明细表")]
[SugarIndex("ix_purchase_request_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_request_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_request_item_request_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseRequestId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_purchase_request_item_purchase_request_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseRequestCode), OrderByType.Asc, false)]
public class TaktPurchaseRequestItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_id", ColumnDescription = "采购申请ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseRequestId { get; set; }

    /// <summary>
    /// 采购申请编码（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_code", ColumnDescription = "采购申请编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_item_id", ColumnDescription = "来源采购计划明细ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanItemId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
    /// </summary>
    [SugarColumn(ColumnName = "allocation_category", ColumnDescription = "分配类别", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string AllocationCategory { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    [SugarColumn(ColumnName = "material_name", ColumnDescription = "物料名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 申请单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "request_unit", ColumnDescription = "申请单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string RequestUnit { get; set; } = "PC";

    /// <summary>
    /// 申请数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "request_quantity", ColumnDescription = "申请数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal RequestQuantity { get; set; } = 0;

    /// <summary>
    /// 已转订单数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; } = 0;

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_per_unit", ColumnDescription = "价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int PurchasePerUnit { get; set; } = 1000;

    /// <summary>
    /// 请购单价
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_unit_price", ColumnDescription = "请购单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal PurchaseRequestUnitPrice { get; set; } = 0;

    /// <summary>
    /// 含税金额
    /// </summary>
    [SugarColumn(ColumnName = "tax_included_amount", ColumnDescription = "含税金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxIncludedAmount { get; set; } = 0;
    /// <summary>
    /// 未税金额
    /// </summary>
    [SugarColumn(ColumnName = "untaxed_amount", ColumnDescription = "未税金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal UntaxedAmount { get; set; } = 0;
    /// <summary>
    /// 税费
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

}
