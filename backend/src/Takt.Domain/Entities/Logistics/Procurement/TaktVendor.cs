// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktVendor.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt经销商实体，定义经销商领域模型
// 
// 业务语义说明：
// Vendor（卖方/供应商）：含义更广，常作为"交易对方"的统称，既可指供货方，
// 也可涵盖服务提供者、资产出租方等；在ERP/财务系统里，"Vendor Master"用
// 来管理所有对外付款对象（包含Supplier、服务公司、租赁公司等）
// 
// Supplier（供应商）：更常用于"供货方"，强调向你提供物料、产品、零部件的对象
// 典型出现在采购（Procurement）、库存、生产、MM（物料管理）等场景
// 例如：制造商的原材料供应商、贸易公司的货品供应商
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt经销商实体
/// </summary>
[SugarTable("takt_logistics_materials_vendor", "经销商信息表")]
[SugarIndex("ix_vendor_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_vendor_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_vendor_vendor_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(VendorCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_vendor_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktVendor : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "vendor_code", ColumnDescription = "经销商编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商名称
    /// </summary>
    [SugarColumn(ColumnName = "vendor_name", ColumnDescription = "经销商名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string VendorName { get; set; } = string.Empty;

    /// <summary>
    /// 经销商简称
    /// </summary>
    [SugarColumn(ColumnName = "vendor_short_name", ColumnDescription = "经销商简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? VendorShortName { get; set; }

    /// <summary>
    /// 经销商类型（0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "vendor_type", ColumnDescription = "经销商类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int VendorType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    [SugarColumn(ColumnName = "industry_sector", ColumnDescription = "行业领域", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? IndustrySector { get; set; }

    /// <summary>
    /// 经销商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    [SugarColumn(ColumnName = "vendor_tax_number", ColumnDescription = "经销商标识", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? VendorTaxNumber { get; set; }

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
    /// 经销商电话
    /// </summary>
    [SugarColumn(ColumnName = "vendor_phone", ColumnDescription = "经销商电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? VendorPhone { get; set; }

    /// <summary>
    /// 经销商传真
    /// </summary>
    [SugarColumn(ColumnName = "vendor_fax", ColumnDescription = "经销商传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? VendorFax { get; set; }

    /// <summary>
    /// 经销商邮箱
    /// </summary>
    [SugarColumn(ColumnName = "vendor_email", ColumnDescription = "经销商邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? VendorEmail { get; set; }

    /// <summary>
    /// 经销商网站
    /// </summary>
    [SugarColumn(ColumnName = "vendor_website", ColumnDescription = "经销商网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? VendorWebsite { get; set; }

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
    /// 授权品牌
    /// </summary>
    [SugarColumn(ColumnName = "authorized_brand", ColumnDescription = "授权品牌", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? AuthorizedBrand { get; set; }

    /// <summary>
    /// 代理区域
    /// </summary>
    [SugarColumn(ColumnName = "agent_region", ColumnDescription = "代理区域", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? AgentRegion { get; set; }

    /// <summary>
    /// 经销商等级（0=普通，1=核心，2=战略，3=临时）
    /// </summary>
    [SugarColumn(ColumnName = "vendor_level", ColumnDescription = "经销商等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int VendorLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_score", ColumnDescription = "评价分数", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EvaluationScore { get; set; } = 0;

    /// <summary>
    /// 是否合格经销商（0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_qualified", ColumnDescription = "是否合格经销商", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsQualified { get; set; } = 1;

    /// <summary>
    /// 经销商状态（1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "vendor_status", ColumnDescription = "经销商状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int VendorStatus { get; set; } = 1;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

}
