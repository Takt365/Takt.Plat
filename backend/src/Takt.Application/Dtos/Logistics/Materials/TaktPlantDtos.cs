// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktPlantDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Auto Generated)
// 功能描述：Plant 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPlant 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// Plant 响应 DTO
// ========================================

/// <summary>
/// Takt工厂实体 代表租户下的独立工厂（租户级实体，只需要TenantCode） 与公司种子对称，参照 SAP Plant 设计
/// 对应前端 TaktPlantDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktPlantDto : TaktTenantDtoBase
{
    /// <summary>
    /// PlantID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PlantId { get; set; }

    /// <summary>
    /// 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂名称
    /// </summary>
    public string PlantName { get; set; } = string.Empty;

    /// <summary>
    /// 工厂简称
    /// </summary>
    public string PlantShortName { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code，选项 TaktCultures/options，DictValue=CultureCode）
    /// </summary>
    public string DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type，DictValue=150 等）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type，DictValue=C 等）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type，DictValue=M 等）
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
    /// 工厂地址1
    /// </summary>
    public string? PlantAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址2
    /// </summary>
    public string? PlantAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址3
    /// </summary>
    public string? PlantAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂电话
    /// </summary>
    public string PlantPhone { get; set; } = string.Empty;

    /// <summary>
    /// 工厂邮箱
    /// </summary>
    public string PlantEmail { get; set; } = string.Empty;

    /// <summary>
    /// 工厂传真
    /// </summary>
    public string PlantFax { get; set; } = string.Empty;

    /// <summary>
    /// 工厂网站
    /// </summary>
    public string PlantWebsite { get; set; } = string.Empty;

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
    /// 工厂负责人
    /// </summary>
    public string PlantManager { get; set; } = string.Empty;

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
    /// 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
    /// </summary>
    public int PlantExistence { get; set; } = 0;

    /// <summary>
    /// 关联公司（关联 TaktCompany.CompanyCode，选项 TaktCompanies/options）
    /// </summary>
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工厂状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int PlantStatus { get; set; } = 0;

}

// ========================================
// Plant 查询 DTO
// ========================================

/// <summary>
/// Plant 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPlantQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂名称
    /// </summary>
    public string? PlantName { get; set; } = string.Empty;

    /// <summary>
    /// 工厂简称
    /// </summary>
    public string? PlantShortName { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string? CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code，选项 TaktCultures/options，DictValue=CultureCode）
    /// </summary>
    public string? DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type，DictValue=150 等）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type，DictValue=C 等）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type，DictValue=M 等）
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
    /// 工厂地址1
    /// </summary>
    public string? PlantAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址2
    /// </summary>
    public string? PlantAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址3
    /// </summary>
    public string? PlantAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂电话
    /// </summary>
    public string? PlantPhone { get; set; } = string.Empty;

    /// <summary>
    /// 工厂邮箱
    /// </summary>
    public string? PlantEmail { get; set; } = string.Empty;

    /// <summary>
    /// 工厂传真
    /// </summary>
    public string? PlantFax { get; set; } = string.Empty;

    /// <summary>
    /// 工厂网站
    /// </summary>
    public string? PlantWebsite { get; set; } = string.Empty;

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
    /// 工厂负责人
    /// </summary>
    public string? PlantManager { get; set; } = string.Empty;

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
    /// 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
    /// </summary>
    public int? PlantExistence { get; set; }

    /// <summary>
    /// 关联公司（关联 TaktCompany.CompanyCode，选项 TaktCompanies/options）
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 工厂状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? PlantStatus { get; set; }

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
// 创建Plant DTO
// ========================================

/// <summary>
/// 创建Plant DTO
/// </summary>
public class TaktPlantCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂名称
    /// </summary>
    [Required(ErrorMessage = "工厂名称不能为空")]
    public string PlantName { get; set; } = string.Empty;

    /// <summary>
    /// 工厂简称
    /// </summary>
    [Required(ErrorMessage = "工厂简称不能为空")]
    public string PlantShortName { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    [Required(ErrorMessage = "编码代号（如 TKC、TCJ、DTA；前端字典录入）不能为空")]
    public string CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code，选项 TaktCultures/options，DictValue=CultureCode）
    /// </summary>
    [Required(ErrorMessage = "区域文化编码（字典 sys_culture_code，选项 TaktCultures/options，DictValue=CultureCode）不能为空")]
    public string DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type，DictValue=150 等）
    /// </summary>
    [Required(ErrorMessage = "企业性质（字典 sys_enterprise_nature_type，DictValue=150 等）不能为空")]
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type，DictValue=C 等）
    /// </summary>
    [Required(ErrorMessage = "行业属性（字典 sys_industry_attribute_type，DictValue=C 等）不能为空")]
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type，DictValue=M 等）
    /// </summary>
    [Required(ErrorMessage = "企业规模（字典 sys_enterprise_scale_type，DictValue=M 等）不能为空")]
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
    /// 工厂地址1
    /// </summary>
    public string? PlantAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址2
    /// </summary>
    public string? PlantAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址3
    /// </summary>
    public string? PlantAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂电话
    /// </summary>
    [Required(ErrorMessage = "工厂电话不能为空")]
    public string PlantPhone { get; set; } = string.Empty;

    /// <summary>
    /// 工厂邮箱
    /// </summary>
    [Required(ErrorMessage = "工厂邮箱不能为空")]
    public string PlantEmail { get; set; } = string.Empty;

    /// <summary>
    /// 工厂传真
    /// </summary>
    [Required(ErrorMessage = "工厂传真不能为空")]
    public string PlantFax { get; set; } = string.Empty;

    /// <summary>
    /// 工厂网站
    /// </summary>
    [Required(ErrorMessage = "工厂网站不能为空")]
    public string PlantWebsite { get; set; } = string.Empty;

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
    /// 工厂负责人
    /// </summary>
    [Required(ErrorMessage = "工厂负责人不能为空")]
    public string PlantManager { get; set; } = string.Empty;

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
    /// 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
    /// </summary>
    public int PlantExistence { get; set; } = 0;

    /// <summary>
    /// 关联公司（关联 TaktCompany.CompanyCode，选项 TaktCompanies/options）
    /// </summary>
    [Required(ErrorMessage = "关联公司（关联 TaktCompany.CompanyCode，选项 TaktCompanies/options）不能为空")]
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 工厂状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int PlantStatus { get; set; } = 0;

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
// 更新Plant DTO
// ========================================

/// <summary>
/// 更新Plant DTO
/// 继承 TaktPlantCreateDto，添加 PlantId 字段
/// </summary>
public class TaktPlantUpdateDto : TaktPlantCreateDto
{
    /// <summary>
    /// PlantID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PlantId { get; set; }

}

// ========================================
// Plant 状态 DTO
// ========================================

/// <summary>
/// Plant 状态更新 DTO
/// </summary>
public class TaktPlantStatusDto
{
    /// <summary>
    /// PlantID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PlantId { get; set; }

    /// <summary>
    /// 工厂状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    [Required(ErrorMessage = "工厂状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）不能为空")]
    public int PlantStatus { get; set; } = 0;
}

// ========================================
// Plant 排序 DTO
// ========================================

/// <summary>
/// Plant 排序更新 DTO
/// </summary>
public class TaktPlantSortDto
{
    /// <summary>
    /// PlantID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PlantId { get; set; }

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
/// Plant 导入模板行 DTO
/// </summary>
public class TaktPlantTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂名称
    /// </summary>
    public string? PlantName { get; set; } = string.Empty;

    /// <summary>
    /// 工厂简称
    /// </summary>
    public string? PlantShortName { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string? CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code，选项 TaktCultures/options，DictValue=CultureCode）
    /// </summary>
    public string? DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type，DictValue=150 等）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type，DictValue=C 等）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type，DictValue=M 等）
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
    /// 工厂地址1
    /// </summary>
    public string? PlantAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址2
    /// </summary>
    public string? PlantAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址3
    /// </summary>
    public string? PlantAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂电话
    /// </summary>
    public string? PlantPhone { get; set; } = string.Empty;

    /// <summary>
    /// 工厂邮箱
    /// </summary>
    public string? PlantEmail { get; set; } = string.Empty;

    /// <summary>
    /// 工厂传真
    /// </summary>
    public string? PlantFax { get; set; } = string.Empty;

    /// <summary>
    /// 工厂网站
    /// </summary>
    public string? PlantWebsite { get; set; } = string.Empty;

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
    /// 工厂负责人
    /// </summary>
    public string? PlantManager { get; set; } = string.Empty;

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
    /// 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
    /// </summary>
    public int? PlantExistence { get; set; }

    /// <summary>
    /// 关联公司（关联 TaktCompany.CompanyCode，选项 TaktCompanies/options）
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 工厂状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? PlantStatus { get; set; }

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
/// Plant 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPlantImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂名称
    /// </summary>
    public string? PlantName { get; set; } = string.Empty;

    /// <summary>
    /// 工厂简称
    /// </summary>
    public string? PlantShortName { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string? CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code，选项 TaktCultures/options，DictValue=CultureCode）
    /// </summary>
    public string? DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type，DictValue=150 等）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type，DictValue=C 等）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type，DictValue=M 等）
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
    /// 工厂地址1
    /// </summary>
    public string? PlantAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址2
    /// </summary>
    public string? PlantAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址3
    /// </summary>
    public string? PlantAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂电话
    /// </summary>
    public string? PlantPhone { get; set; } = string.Empty;

    /// <summary>
    /// 工厂邮箱
    /// </summary>
    public string? PlantEmail { get; set; } = string.Empty;

    /// <summary>
    /// 工厂传真
    /// </summary>
    public string? PlantFax { get; set; } = string.Empty;

    /// <summary>
    /// 工厂网站
    /// </summary>
    public string? PlantWebsite { get; set; } = string.Empty;

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
    /// 工厂负责人
    /// </summary>
    public string? PlantManager { get; set; } = string.Empty;

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
    /// 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
    /// </summary>
    public int? PlantExistence { get; set; }

    /// <summary>
    /// 关联公司（关联 TaktCompany.CompanyCode，选项 TaktCompanies/options）
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 工厂状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? PlantStatus { get; set; }

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
/// Plant 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPlantExportDto
{
    /// <summary>
    /// PlantID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PlantId { get; set; }

    /// <summary>
    /// 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂名称
    /// </summary>
    public string PlantName { get; set; } = string.Empty;

    /// <summary>
    /// 工厂简称
    /// </summary>
    public string PlantShortName { get; set; } = string.Empty;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code，选项 TaktCultures/options，DictValue=CultureCode）
    /// </summary>
    public string DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type，DictValue=150 等）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type，DictValue=C 等）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type，DictValue=M 等）
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
    /// 工厂地址1
    /// </summary>
    public string? PlantAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址2
    /// </summary>
    public string? PlantAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂地址3
    /// </summary>
    public string? PlantAddress3 { get; set; } = string.Empty;

    /// <summary>
    /// 工厂电话
    /// </summary>
    public string PlantPhone { get; set; } = string.Empty;

    /// <summary>
    /// 工厂邮箱
    /// </summary>
    public string PlantEmail { get; set; } = string.Empty;

    /// <summary>
    /// 工厂传真
    /// </summary>
    public string PlantFax { get; set; } = string.Empty;

    /// <summary>
    /// 工厂网站
    /// </summary>
    public string PlantWebsite { get; set; } = string.Empty;

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
    /// 工厂负责人
    /// </summary>
    public string PlantManager { get; set; } = string.Empty;

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
    /// 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
    /// </summary>
    public int PlantExistence { get; set; } = 0;

    /// <summary>
    /// 关联公司（关联 TaktCompany.CompanyCode，选项 TaktCompanies/options）
    /// </summary>
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工厂状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int PlantStatus { get; set; } = 0;

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
