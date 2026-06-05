// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktCustomer.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt客户信息实体，定义客户领域模型
// 
// 业务语义说明：
// Customer 适用于核心交付物为产品、货物或可明确交易的标的的场景
// （如：商品购买客户、设备采购客户、物料采购客户等）
// Client 适用于核心交付物为服务、项目交付、长期合作或专业服务的场景
// （如：咨询公司客户、SaaS订阅客户、外包服务客户等）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt客户信息实体
/// </summary>
[SugarTable("takt_logistics_sales_customer", "客户信息表")]
[SugarIndex("ix_customer_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_customer_customer_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_customer_customer_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_customer_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktCustomer : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    [SugarColumn(ColumnName = "customer_name", ColumnDescription = "客户名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    [SugarColumn(ColumnName = "customer_short_name", ColumnDescription = "客户简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? CustomerShortName { get; set; }

    /// <summary>
    /// 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
    /// </summary>
    [SugarColumn(ColumnName = "customer_type", ColumnDescription = "客户类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CustomerType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    [SugarColumn(ColumnName = "industry_sector", ColumnDescription = "行业领域", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? IndustrySector { get; set; }

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    [SugarColumn(ColumnName = "customer_tax_number", ColumnDescription = "客户标识", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CustomerTaxNumber { get; set; }

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
    /// 客户电话
    /// </summary>
    [SugarColumn(ColumnName = "customer_phone", ColumnDescription = "客户电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CustomerPhone { get; set; }

    /// <summary>
    /// 客户传真
    /// </summary>
    [SugarColumn(ColumnName = "customer_fax", ColumnDescription = "客户传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CustomerFax { get; set; }

    /// <summary>
    /// 客户邮箱
    /// </summary>
    [SugarColumn(ColumnName = "customer_email", ColumnDescription = "客户邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? CustomerEmail { get; set; }

    /// <summary>
    /// 客户网站
    /// </summary>
    [SugarColumn(ColumnName = "customer_website", ColumnDescription = "客户网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? CustomerWebsite { get; set; }

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
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种代码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";

    /// <summary>
    /// 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
    /// </summary>
    [SugarColumn(ColumnName = "credit_level", ColumnDescription = "信用等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "credit_amount", ColumnDescription = "信用额度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal CreditAmount { get; set; } = 0;

    /// <summary>
    /// 折扣率（百分比，如：5.5表示5.5%折扣）
    /// </summary>
    [SugarColumn(ColumnName = "discount_rate", ColumnDescription = "折扣率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DiscountRate { get; set; } = 0;

    /// <summary>
    /// 销售员（人员代码）
    /// </summary>
    [SugarColumn(ColumnName = "sales_by", ColumnDescription = "销售员", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SalesBy { get; set; }

    /// <summary>
    /// 客户等级（0=普通，1=重要，2=VIP，3=战略）
    /// </summary>
    [SugarColumn(ColumnName = "customer_level", ColumnDescription = "客户等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CustomerLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_score", ColumnDescription = "评价分数", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EvaluationScore { get; set; } = 0;

    /// <summary>
    /// 是否合格客户（0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_qualified", ColumnDescription = "是否合格客户", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsQualified { get; set; } = 1;

    /// <summary>
    /// 客户状态（1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "customer_status", ColumnDescription = "客户状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CustomerStatus { get; set; } = 1;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

}
