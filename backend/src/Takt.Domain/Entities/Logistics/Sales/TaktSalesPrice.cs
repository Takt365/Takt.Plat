// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesPrice.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售价格实体（定价记录：条件类型+客户+物料+有效期）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售价格实体（定价记录；条件类型 + 客户 + 物料 + 有效期；含子表 Items）
/// </summary>
[SugarTable("takt_logistics_sales_price", "销售价格表")]
[SugarIndex("ix_sales_price_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_price_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_price_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesPriceCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_price_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PriceType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_price_customer_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktSalesPrice : TaktCompanyEntityBase
{
    /// <summary>
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    [SugarColumn(ColumnName = "sales_price_code", ColumnDescription = "定价记录号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    [SugarColumn(ColumnName = "price_type", ColumnDescription = "条件类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "PB00")]
    public string PriceType { get; set; } = "PB00";

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日
    /// </summary>
    [SugarColumn(ColumnName = "valid_from", ColumnDescription = "有效起始日", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// 有效截至日
    /// </summary>
    [SugarColumn(ColumnName = "valid_to", ColumnDescription = "有效截至日", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidTo { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59);

    /// <summary>
    /// 可变关键字
    /// </summary>
    [SugarColumn(ColumnName = "variable_key", ColumnDescription = "可变关键字", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? VariableKey { get; set; }

    /// <summary>
    /// 来源销售报价 ID（选项 TaktSalesQuotations/options，DictValue=Id；对应采购侧来源询价）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_id", ColumnDescription = "来源销售报价ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesQuotationId { get; set; }

    /// <summary>
    /// 来源销售报价编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "sales_quotation_code", ColumnDescription = "来源销售报价编码", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? SalesQuotationCode { get; set; }

    /// <summary>
    /// 定价条件行列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesPriceItem.SalesPriceId))]
    public List<TaktSalesPriceItem>? Items { get; set; }
}
