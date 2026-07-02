// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesPriceItem.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售价格明细实体，定义客户物料价格领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售价格明细实体（客户物料价格明细表）
/// </summary>
[SugarTable("takt_logistics_sales_price_item", "销售价格明细表")]
[SugarIndex("ix_sales_price_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_price_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_price_item_price_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesPriceId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
public class TaktSalesPriceItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 销售价格（关联 TaktSalesPrice.Id，选项 TaktSalesPrices/options）
    /// </summary>
    [SugarColumn(ColumnName = "sales_price_id", ColumnDescription = "销售价格ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "sales_price_code", ColumnDescription = "销售价格编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "sales_unit", ColumnDescription = "销售单位", ColumnDataType = "nvarchar", Length = 5, IsNullable = false, DefaultValue = "PC")]
    public string SalesUnit { get; set; } = "PC";

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "sales_per_unit", ColumnDescription = "价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int SalesPerUnit { get; set; } = 1000;

    /// <summary>
    /// 销售价格（decimal(18,5)）
    /// </summary>
    [SugarColumn(ColumnName = "sales_price", ColumnDescription = "销售价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal SalesPrice { get; set; } = 0;

    /// <summary>
    /// 最小订购量（基本单位数量，整数）
    /// </summary>
    [SugarColumn(ColumnName = "min_order_quantity", ColumnDescription = "最小订购量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 最大订购量（基本单位数量，0表示无限制，整数）
    /// </summary>
    [SugarColumn(ColumnName = "max_order_quantity", ColumnDescription = "最大订购量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MaxOrderQuantity { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesPriceScale.ItemId))]
    public List<TaktSalesPriceScale>? Scales { get; set; }

    /// <summary>
    /// 销售价格（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(SalesPriceId))]
    public TaktSalesPrice? Price { get; set; }
}
