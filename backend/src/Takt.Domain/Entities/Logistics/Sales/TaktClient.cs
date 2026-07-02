// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktClient.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt客户端信息实体，定义客户端领域模型
// 
// 业务语义说明：
// Client 适用于核心交付物为服务、项目交付、长期合作或专业服务的场景
// （如：咨询公司客户、SaaS订阅客户、外包服务客户等）
// Customer 适用于核心交付物为产品、货物或可明确交易的标的的场景
// （如：商品购买客户、设备采购客户、物料采购客户等）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt客户端信息实体
/// </summary>
[SugarTable("takt_logistics_sales_client", "客户端信息表")]
[SugarIndex("ix_client_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_client_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_client_client_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ClientCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_client_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_client_client_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ClientStatus), OrderByType.Asc, false)]
public class TaktClient : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户端编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "client_code", ColumnDescription = "客户端编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ClientCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户端名称
    /// </summary>
    [SugarColumn(ColumnName = "client_name", ColumnDescription = "客户端名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string ClientName { get; set; } = string.Empty;
    /// <summary>
    /// 客户端简称
    /// </summary>
    [SugarColumn(ColumnName = "client_short_name", ColumnDescription = "客户端简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ClientShortName { get; set; }
    /// <summary>
    /// 客户端类型（字典 logistics_client_category；0=终端客户 1=分销商 2=零售商 3=电商平台 4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "client_type", ColumnDescription = "客户端类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ClientType { get; set; } = 0;
    /// <summary>
    /// 行业领域
    /// </summary>
    [SugarColumn(ColumnName = "industry_sector", ColumnDescription = "行业领域", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? IndustrySector { get; set; }
    /// <summary>
    /// 客户端标识（税务登记证号/统一社会信用代码）
    /// </summary>
    [SugarColumn(ColumnName = "client_tax_number", ColumnDescription = "客户端标识", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ClientTaxNumber { get; set; }
    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "int", IsNullable = false, DefaultValue = "13")]
    public int TaxRate { get; set; } = 13;
    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    [SugarColumn(ColumnName = "registration_country", ColumnDescription = "注册国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? RegistrationCountry { get; set; }
    /// <summary>
    /// 注册地址1
    /// </summary>
    [SugarColumn(ColumnName = "registration_address1", ColumnDescription = "注册地址1", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RegistrationAddress1 { get; set; }
    /// <summary>
    /// 注册地址2
    /// </summary>
    [SugarColumn(ColumnName = "registration_address2", ColumnDescription = "注册地址2", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RegistrationAddress2 { get; set; }
    /// <summary>
    /// 注册地址3
    /// </summary>
    [SugarColumn(ColumnName = "registration_address3", ColumnDescription = "注册地址3", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RegistrationAddress3 { get; set; }
    /// <summary>
    /// 客户端电话
    /// </summary>
    [SugarColumn(ColumnName = "client_phone", ColumnDescription = "客户端电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ClientPhone { get; set; }
    /// <summary>
    /// 客户端传真
    /// </summary>
    [SugarColumn(ColumnName = "client_fax", ColumnDescription = "客户端传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ClientFax { get; set; }
    /// <summary>
    /// 客户端邮箱
    /// </summary>
    [SugarColumn(ColumnName = "client_email", ColumnDescription = "客户端邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ClientEmail { get; set; }
    /// <summary>
    /// 客户端网站
    /// </summary>
    [SugarColumn(ColumnName = "client_website", ColumnDescription = "客户端网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ClientWebsite { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    [SugarColumn(ColumnName = "contact_person", ColumnDescription = "联系人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ContactPerson { get; set; }
    /// <summary>
    /// 联系人电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系人电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ContactPhone { get; set; }
    /// <summary>
    /// 联系人邮箱
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "联系人邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ContactEmail { get; set; }
    /// <summary>
    /// 结算币种代码
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种代码", ColumnDataType = "nvarchar", Length = 3, IsNullable = true, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "prepayship")]
    public string PaymentTerms { get; set; } = "prepayship";
    /// <summary>
    /// 销售渠道（字典 logistics_sales_channel_type；0=直销 1=经销 2=代销 3=电商 4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "sales_channel", ColumnDescription = "销售渠道", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SalesChannel { get; set; } = 0;
    /// <summary>
    /// 平台名称（电商平台名称）
    /// </summary>
    [SugarColumn(ColumnName = "platform_name", ColumnDescription = "平台名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? PlatformName { get; set; }
    /// <summary>
    /// 店铺名称
    /// </summary>
    [SugarColumn(ColumnName = "store_name", ColumnDescription = "店铺名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? StoreName { get; set; }
    /// <summary>
    /// 客户端等级（字典 logistics_customer_level_category；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    [SugarColumn(ColumnName = "client_level", ColumnDescription = "客户端等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ClientLevel { get; set; } = 0;
    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_score", ColumnDescription = "评价分数", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EvaluationScore { get; set; } = 0;
    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 客户端状态（字典 sys_normal_disable_status）
    /// </summary>
    [SugarColumn(ColumnName = "client_status", ColumnDescription = "客户端状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ClientStatus { get; set; } = 1;
}
