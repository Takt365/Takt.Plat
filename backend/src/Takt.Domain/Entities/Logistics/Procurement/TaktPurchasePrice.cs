// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchasePrice.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购价格实体，定义采购价格领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购价格实体（供应商价格主表，一个供应商可以有多个物料价格）
/// </summary>
[SugarTable("takt_logistics_materials_purchase_price", "采购价格表")]
[SugarIndex("ix_purchase_price_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_price_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_price_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PurchasePriceCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_effective_end_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EffectiveEndDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_effective_start_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EffectiveStartDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_price_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PriceStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_price_price_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PriceType), OrderByType.Asc, false)]
public class TaktPurchasePrice : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_price_code", ColumnDescription = "采购价格编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 来源采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_id", ColumnDescription = "来源采购询价ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }
    /// <summary>
    /// 来源采购询价编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_code", ColumnDescription = "来源采购询价编码", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? PurchaseInquiryCode { get; set; }

    /// <summary>
    /// 价格类型（字典 logistics_price_type；0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
    /// </summary>
    [SugarColumn(ColumnName = "price_type", ColumnDescription = "价格类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PriceType { get; set; } = 0;

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
    /// 价格状态（字典 sys_normal_disable_status；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "price_status", ColumnDescription = "价格状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PriceStatus { get; set; } = 1;

    /// <summary>
    /// 物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchasePriceItem.PurchasePriceId))]
    public List<TaktPurchasePriceItem>? Items { get; set; }

    /// <summary>
    /// 采购价格变更记录列表（外键在子表 TaktPurchasePriceChangeLog.PurchasePriceId）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchasePriceChangeLog.PurchasePriceId))]
    public List<TaktPurchasePriceChangeLog>? ChangeLogs { get; set; }
}