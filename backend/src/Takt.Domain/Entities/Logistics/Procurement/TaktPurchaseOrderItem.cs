// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseOrderItem.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购订单明细实体，定义采购订单明细领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购订单明细实体
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_order_item", "采购订单明细表")]
[SugarIndex("ix_purchase_order_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_order_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_order_item_order_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseOrderId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_order_item_purchase_order_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseOrderCode), OrderByType.Asc, false)]
public class TaktPurchaseOrderItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_id", ColumnDescription = "采购订单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseOrderId { get; set; }

    /// <summary>
    /// 采购订单编码（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "采购订单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源请购编码
    /// </summary>
    [SugarColumn(ColumnName = "request_code", ColumnDescription = "来源请购编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? RequestCode { get; set; }

    /// <summary>
    /// 来源请购行号
    /// </summary>
    [SugarColumn(ColumnName = "request_line_number", ColumnDescription = "来源请购行号", ColumnDataType = "int", IsNullable = true)]
    public int? RequestLineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_unit", ColumnDescription = "采购单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string PurchaseUnit { get; set; } = "PC";

    /// <summary>
    /// 订购数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "order_quantity", ColumnDescription = "订购数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal OrderQuantity { get; set; } = 0;

    /// <summary>
    /// 已入库数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "received_quantity", ColumnDescription = "已入库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal ReceivedQuantity { get; set; } = 0;

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_per_unit", ColumnDescription = "价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int PurchasePerUnit { get; set; } = 1000;

    /// <summary>
    /// 采购单价
    /// </summary>
    [SugarColumn(ColumnName = "purchase_unit_price", ColumnDescription = "采购单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal PurchaseUnitPrice { get; set; } = 0;

    /// <summary>
    /// 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
    /// </summary>
    [SugarColumn(ColumnName = "discount_rate", ColumnDescription = "折扣率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountRate { get; set; } = 0;

    /// <summary>
    /// 折扣金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "discount_amount", ColumnDescription = "折扣金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountAmount { get; set; } = 0;

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
    /// 采购金额
    /// </summary>
    [SugarColumn(ColumnName = "purchase_amount", ColumnDescription = "采购金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal PurchaseAmount { get; set; } = 0;

    /// <summary>
    /// 行交货状态（字典 logistics_delivery_status；0=未交货，1=部分交货，2=全部交货）
    /// </summary>
    [SugarColumn(ColumnName = "delivery_status", ColumnDescription = "行交货状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

}
