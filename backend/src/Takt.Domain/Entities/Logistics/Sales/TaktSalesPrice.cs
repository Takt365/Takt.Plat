// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesPrice.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售价格实体，定义销售价格领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售价格实体（客户价格主表，一个客户可以有多个物料价格）
/// </summary>
[SugarTable("takt_logistics_sales_price", "销售价格表")]
[SugarIndex("ix_sales_price_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_price_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_price_sp_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SalesPriceCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, nameof(PriceType), OrderByType.Asc, nameof(EffectiveStartDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_price_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_price_effective_end_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EffectiveEndDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_price_effective_start_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EffectiveStartDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_price_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_price_price_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PriceStatus), OrderByType.Asc, false)]
public class TaktSalesPrice : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "sales_price_code", ColumnDescription = "销售价格编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode；为空表示通用价格）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 价格类型（字典 logistics_sales_price_type；SAP 定价条件类型 KSCHL，如 PR00/PB00；默认 PR00）
    /// </summary>
    [SugarColumn(ColumnName = "price_type", ColumnDescription = "价格类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "PR00")]
    public string PriceType { get; set; } = "PR00";

    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_start_date", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EffectiveStartDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 失效日期（空表示长期有效）
    /// </summary>
    [SugarColumn(ColumnName = "effective_end_date", ColumnDescription = "失效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EffectiveEndDate { get; set; }

    /// <summary>
    /// 价格状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "price_status", ColumnDescription = "价格状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PriceStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 物料价格明细列表（主子表关系，一个客户价格可以有多个物料价格）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesPriceItem.SalesPriceId))]
    public List<TaktSalesPriceItem>? Items { get; set; }
}
