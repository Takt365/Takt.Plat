// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktCompany.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：公司实体，代表租户下的独立公司/工厂（第二层数据隔离）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.Identity;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 公司实体
/// 代表租户下的独立公司/工厂（租户级实体，只需要TenantCode）
/// 参照 SAP Company Code (BUKRS) 设计
/// </summary>
[SugarTable("takt_accounting_financial_company", "公司表")]
[SugarIndex("ix_company_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_company_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_company_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, true)]
public class TaktCompany : TaktTenantEntityBase
{
    /// <summary>
    /// 公司代码（唯一索引：租户内唯一，见 ix_company_code_unique）
    /// </summary>
    [SugarColumn(ColumnName = "company_code", ColumnDescription = "公司代码", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称
    /// </summary>
    [SugarColumn(ColumnName = "company_name", ColumnDescription = "公司名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    [SugarColumn(ColumnName = "company_short_name", ColumnDescription = "公司简称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 公司类型
    /// </summary>
    [SugarColumn(ColumnName = "company_type", ColumnDescription = "公司类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public TaktCompanyType CompanyType { get; set; } = TaktCompanyType.Manufacturing;

    /// <summary>
    /// 企业性质（统计用登记注册类型代码，国统字〔1998〕200号）
    /// </summary>
    [SugarColumn(ColumnName = "enterprise_nature", ColumnDescription = "企业性质（登记注册类型代码）", ColumnDataType = "int", IsNullable = false, DefaultValue = "150")]
    public TaktEnterpriseNature EnterpriseNature { get; set; } = TaktEnterpriseNature.DomesticLimitedLiability;

    /// <summary>
    /// 行业属性（GB/T 4754-2017 国民经济行业分类门类）
    /// </summary>
    [SugarColumn(ColumnName = "industry_attribute", ColumnDescription = "行业属性（国民经济行业门类）", ColumnDataType = "int", IsNullable = false, DefaultValue = "3")]
    public TaktIndustryAttribute IndustryAttribute { get; set; } = TaktIndustryAttribute.C;

    /// <summary>
    /// 企业规模（统计上大中小微型划分代码 1–4）
    /// </summary>
    [SugarColumn(ColumnName = "enterprise_scale", ColumnDescription = "企业规模（大中小微型代码）", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public TaktEnterpriseScale EnterpriseScale { get; set; } = TaktEnterpriseScale.Medium;

    /// <summary>
    /// 经营范围
    /// </summary>
    [SugarColumn(ColumnName = "business_scope", ColumnDescription = "经营范围", ColumnDataType = "nvarchar", Length = -1, IsNullable = false, DefaultValue = "")]
    public string BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    [SugarColumn(ColumnName = "registration_address1", ColumnDescription = "注册地址1", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    [SugarColumn(ColumnName = "registration_address2", ColumnDescription = "注册地址2", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? RegistrationAddress2 { get; set; }

    /// <summary>
    /// 注册地址3
    /// </summary>
    [SugarColumn(ColumnName = "registration_address3", ColumnDescription = "注册地址3", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? RegistrationAddress3 { get; set; }

    /// <summary>
    /// 注册国家
    /// </summary>
    [SugarColumn(ColumnName = "registration_region", ColumnDescription = "注册国家", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省
    /// </summary>
    [SugarColumn(ColumnName = "registration_province", ColumnDescription = "注册省", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市
    /// </summary>
    [SugarColumn(ColumnName = "registration_city", ColumnDescription = "注册市", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家
    /// </summary>
    [SugarColumn(ColumnName = "business_region", ColumnDescription = "经营国家", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省
    /// </summary>
    [SugarColumn(ColumnName = "business_province", ColumnDescription = "经营地区-省", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市
    /// </summary>
    [SugarColumn(ColumnName = "business_city", ColumnDescription = "经营地区-市", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    [SugarColumn(ColumnName = "business_address1", ColumnDescription = "经营地址1", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    [SugarColumn(ColumnName = "business_address2", ColumnDescription = "经营地址2", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? BusinessAddress2 { get; set; }

    /// <summary>
    /// 经营地址3
    /// </summary>
    [SugarColumn(ColumnName = "business_address3", ColumnDescription = "经营地址3", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? BusinessAddress3 { get; set; }

    /// <summary>
    /// 公司电话
    /// </summary>
    [SugarColumn(ColumnName = "company_phone", ColumnDescription = "公司电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    [SugarColumn(ColumnName = "company_email", ColumnDescription = "公司邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    [SugarColumn(ColumnName = "company_fax", ColumnDescription = "公司传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    [SugarColumn(ColumnName = "company_website", ColumnDescription = "公司网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    [SugarColumn(ColumnName = "unified_social_credit_code", ColumnDescription = "统一社会信用代码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    [SugarColumn(ColumnName = "tax_registration_number", ColumnDescription = "税务登记号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    [SugarColumn(ColumnName = "legal_representative", ColumnDescription = "法定代表人", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    [SugarColumn(ColumnName = "company_manager", ColumnDescription = "公司负责人", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string CompanyManager { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    [SugarColumn(ColumnName = "registered_capital", ColumnDescription = "注册资本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal RegisteredCapital { get; set; } = 0;

    /// <summary>
    /// 成立日期
    /// </summary>
    [SugarColumn(ColumnName = "establishment_date", ColumnDescription = "成立日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    [SugarColumn(ColumnName = "closing_date", ColumnDescription = "关闭日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（市场主体登记状态）
    /// </summary>
    [SugarColumn(ColumnName = "company_existence", ColumnDescription = "存续状态（登记状态代码）", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public TaktCompanyExistenceStatus CompanyExistence { get; set; } = TaktCompanyExistenceStatus.Subsisting;

    /// <summary>
    /// 关联工厂编码（如 0001、C100）
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂编码", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "0001")]
    public string RelatedPlant { get; set; } = "0001";

    /// <summary>
    /// 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
    /// </summary>
    [SugarColumn(ColumnName = "default_culture", ColumnDescription = "默认区域文化编码", ColumnDataType = "varchar", Length = 5, IsNullable = false, DefaultValue = "en-US")]
    public string DefaultCulture { get; set; } = "en-US";

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    [SugarColumn(ColumnName = "code_alias", ColumnDescription = "编码代号", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "TKC")]
    public string CodeAlias { get; set; } = "TKC";

    /// <summary>
    /// 公司状态
    /// </summary>
    [SugarColumn(ColumnName = "company_status", ColumnDescription = "公司状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public TaktCommonStatus CompanyStatus { get; set; } = TaktCommonStatus.Enabled;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 可访问该公司的角色关联（RBAC，表 takt_identity_role_company）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktRoleCompany.CompanyCode))]
    public List<TaktRoleCompany>? RoleCompanies { get; set; }

    /// <summary>
    /// 可访问该公司的用户关联（RBAC，表 takt_identity_user_company）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktUserCompany.CompanyCode))]
    public List<TaktUserCompany>? UserCompanies { get; set; }

}
