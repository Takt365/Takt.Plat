// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryItem.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：采购询价明细实体（主子表从表，外键 PurchaseInquiryId）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// 采购询价明细实体
/// </summary>
[SugarTable("takt_logistics_materials_purchase_inquiry_item", "采购询价明细表")]
[SugarIndex("ix_purchase_inquiry_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_inquiry_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_inquiry_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseInquiryId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_purchase_inquiry_item_purchase_inquiry_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseInquiryCode), OrderByType.Asc, false)]
public class TaktPurchaseInquiryItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_id", ColumnDescription = "采购询价ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryId { get; set; }
    /// <summary>
    /// 采购询价编码（冗余，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_code", ColumnDescription = "采购询价编码", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string PurchaseInquiryCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; }
    /// <summary>
    /// 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
    /// </summary>
    [SugarColumn(ColumnName = "allocation_category", ColumnDescription = "分配类别", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string AllocationCategory { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? MaterialCode { get; set; }
    /// <summary>
    /// 物料名称
    /// </summary>
    [SugarColumn(ColumnName = "material_name", ColumnDescription = "物料名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string MaterialName { get; set; } = string.Empty;
    /// <summary>
    /// 物料规格
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? MaterialSpecification { get; set; }
    /// <summary>
    /// 询价单位
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_unit", ColumnDescription = "询价单位", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "PC")]
    public string InquiryUnit { get; set; } = "PC";
    /// <summary>
    /// 询价数量（基本单位数量，decimal(18,5)）
    /// </summary>
    [SugarColumn(ColumnName = "inquiry_quantity", ColumnDescription = "询价数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal InquiryQuantity { get; set; } = 0;
    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_per_unit", ColumnDescription = "价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int PurchasePerUnit { get; set; } = 1000;
    /// <summary>
    /// 报价单价
    /// </summary>
    [SugarColumn(ColumnName = "quoted_unit_price", ColumnDescription = "报价单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal QuotedUnitPrice { get; set; }
    /// <summary>
    /// 报价金额
    /// </summary>
    [SugarColumn(ColumnName = "quoted_amount", ColumnDescription = "报价金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal QuotedAmount { get; set; }
    /// <summary>
    /// 目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "target_supplier_code", ColumnDescription = "目标供应商编码", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? TargetSupplierCode { get; set; }
    /// <summary>
    /// 目标供应商名称
    /// </summary>
    [SugarColumn(ColumnName = "target_supplier_name", ColumnDescription = "目标供应商名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? TargetSupplierName { get; set; }
}
