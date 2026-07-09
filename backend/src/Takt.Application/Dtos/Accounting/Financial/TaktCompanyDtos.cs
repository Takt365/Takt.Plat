// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktCompanyDtos.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Auto Generated)
// 功能描述：Company 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCompany 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// Company 响应 DTO
// ========================================

/// <summary>
/// 公司实体 代表租户下的独立公司/工厂（租户级实体，只需要TenantCode） 参照 SAP Company Code (BUKRS) 设计
/// 对应前端 TaktCompanyDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktCompanyDto : TaktTenantDtoBase
{
    /// <summary>
    /// CompanyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type）
    /// </summary>
    public string EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家
    /// </summary>
    public string RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省
    /// </summary>
    public string RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市
    /// </summary>
    public string RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家
    /// </summary>
    public string BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省
    /// </summary>
    public string BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市
    /// </summary>
    public string BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址3
    /// </summary>
    public string? BusinessAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string CompanyManager { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int CompanyExistence { get; set; } = 0;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable_status）
    /// </summary>
    public int CompanyStatus { get; set; } = 0;

    /// <summary>
    /// 可访问该公司的角色关联（RBAC，表 takt_identity_role_company）
    /// （子表：TaktRoleCompany）
    /// </summary>
    public List<TaktRoleCompanyDto>? RoleCompanies { get; set; }

    /// <summary>
    /// 可访问该公司的用户关联（RBAC，表 takt_identity_user_company）
    /// （子表：TaktUserCompany）
    /// </summary>
    public List<TaktUserCompanyDto>? UserCompanies { get; set; }

}

// ========================================
// Company 查询 DTO
// ========================================

/// <summary>
/// Company 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCompanyQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称
    /// </summary>
    public string? CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string? CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type）
    /// </summary>
    public string? EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string? BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家
    /// </summary>
    public string? BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省
    /// </summary>
    public string? BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市
    /// </summary>
    public string? BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string? BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址3
    /// </summary>
    public string? BusinessAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string? CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string? CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string? CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string? CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string? UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string? TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string? LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string? CompanyManager { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal? RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期（范围查询-开始）
    /// </summary>
    public DateTime? EstablishmentDateStart { get; set; }

    /// <summary>
    /// 成立日期（范围查询-结束）
    /// </summary>
    public DateTime? EstablishmentDateEnd { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）（范围查询-开始）
    /// </summary>
    public DateTime? ClosingDateStart { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）（范围查询-结束）
    /// </summary>
    public DateTime? ClosingDateEnd { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int? CompanyExistence { get; set; }

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string? DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string? CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 公司状态（字典 sys_normal_disable_status）
    /// </summary>
    public int? CompanyStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建Company DTO
// ========================================

/// <summary>
/// 创建Company DTO
/// </summary>
public class TaktCompanyCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称
    /// </summary>
    [Required(ErrorMessage = "公司名称不能为空")]
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    [Required(ErrorMessage = "公司简称不能为空")]
    public string CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type）
    /// </summary>
    [Required(ErrorMessage = "企业性质（字典 sys_enterprise_nature_type）不能为空")]
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type）
    /// </summary>
    [Required(ErrorMessage = "行业属性（字典 sys_industry_attribute_type）不能为空")]
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type）
    /// </summary>
    [Required(ErrorMessage = "企业规模（字典 sys_enterprise_scale_type）不能为空")]
    public string EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    [Required(ErrorMessage = "经营范围不能为空")]
    public string BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    [Required(ErrorMessage = "注册地址1不能为空")]
    public string RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家
    /// </summary>
    [Required(ErrorMessage = "注册国家不能为空")]
    public string RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省
    /// </summary>
    [Required(ErrorMessage = "注册省不能为空")]
    public string RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市
    /// </summary>
    [Required(ErrorMessage = "注册市不能为空")]
    public string RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家
    /// </summary>
    [Required(ErrorMessage = "经营国家不能为空")]
    public string BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省
    /// </summary>
    [Required(ErrorMessage = "经营地区-省不能为空")]
    public string BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市
    /// </summary>
    [Required(ErrorMessage = "经营地区-市不能为空")]
    public string BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    [Required(ErrorMessage = "经营地址1不能为空")]
    public string BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址3
    /// </summary>
    public string? BusinessAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    [Required(ErrorMessage = "公司电话不能为空")]
    public string CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    [Required(ErrorMessage = "公司邮箱不能为空")]
    public string CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    [Required(ErrorMessage = "公司传真不能为空")]
    public string CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    [Required(ErrorMessage = "公司网站不能为空")]
    public string CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    [Required(ErrorMessage = "统一社会信用代码不能为空")]
    public string UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    [Required(ErrorMessage = "税务登记号不能为空")]
    public string TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    [Required(ErrorMessage = "法定代表人不能为空")]
    public string LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    [Required(ErrorMessage = "公司负责人不能为空")]
    public string CompanyManager { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int CompanyExistence { get; set; } = 0;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    [Required(ErrorMessage = "区域文化编码（字典 sys_culture_code）不能为空")]
    public string DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    [Required(ErrorMessage = "编码代号（如 TKC、TCJ、DTA；前端字典录入）不能为空")]
    public string CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    [Required(ErrorMessage = "关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）不能为空")]
    public string RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable_status）
    /// </summary>
    public int CompanyStatus { get; set; } = 0;

    /// <summary>
    /// 可访问该公司的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 可访问该公司的用户 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? UserIds { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新Company DTO
// ========================================

/// <summary>
/// 更新Company DTO
/// 继承 TaktCompanyCreateDto，添加 CompanyId 字段
/// </summary>
public class TaktCompanyUpdateDto : TaktCompanyCreateDto
{
    /// <summary>
    /// CompanyID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

}

// ========================================
// Company 状态 DTO
// ========================================

/// <summary>
/// Company 状态更新 DTO
/// </summary>
public class TaktCompanyStatusDto
{
    /// <summary>
    /// CompanyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

    /// <summary>
    /// 公司状态（字典 sys_normal_disable_status）
    /// </summary>
    [Required(ErrorMessage = "公司状态（字典 sys_normal_disable_status）不能为空")]
    public int CompanyStatus { get; set; } = 0;
}

// ========================================
// Company 排序 DTO
// ========================================

/// <summary>
/// Company 排序更新 DTO
/// </summary>
public class TaktCompanySortDto
{
    /// <summary>
    /// CompanyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Company 导入模板行 DTO
/// </summary>
public class TaktCompanyTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称
    /// </summary>
    public string? CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string? CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type）
    /// </summary>
    public string? EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string? BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家
    /// </summary>
    public string? BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省
    /// </summary>
    public string? BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市
    /// </summary>
    public string? BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string? BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址3
    /// </summary>
    public string? BusinessAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string? CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string? CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string? CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string? CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string? UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string? TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string? LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string? CompanyManager { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal? RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime? EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int? CompanyExistence { get; set; }

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string? DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string? CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable_status）
    /// </summary>
    public int? CompanyStatus { get; set; }

    /// <summary>
    /// 可访问该公司的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 可访问该公司的用户 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? UserIds { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Company 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCompanyImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称
    /// </summary>
    public string? CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string? CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type）
    /// </summary>
    public string? EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string? BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家
    /// </summary>
    public string? BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省
    /// </summary>
    public string? BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市
    /// </summary>
    public string? BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string? BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址3
    /// </summary>
    public string? BusinessAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string? CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string? CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string? CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string? CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string? UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string? TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string? LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string? CompanyManager { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal? RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime? EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int? CompanyExistence { get; set; }

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string? DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string? CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable_status）
    /// </summary>
    public int? CompanyStatus { get; set; }

    /// <summary>
    /// 可访问该公司的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 可访问该公司的用户 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? UserIds { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Company 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCompanyExportDto
{
    /// <summary>
    /// CompanyID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type）
    /// </summary>
    public string EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址3
    /// </summary>
    public string? RegistrationAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家
    /// </summary>
    public string RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省
    /// </summary>
    public string RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市
    /// </summary>
    public string RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家
    /// </summary>
    public string BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省
    /// </summary>
    public string BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市
    /// </summary>
    public string BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址3
    /// </summary>
    public string? BusinessAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string CompanyManager { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int CompanyExistence { get; set; } = 0;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable_status）
    /// </summary>
    public int CompanyStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
