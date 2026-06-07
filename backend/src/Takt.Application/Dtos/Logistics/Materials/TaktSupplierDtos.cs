// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktSupplierDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：Supplier 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSupplier 生成，请按需审阅）
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
// Supplier 响应 DTO
// ========================================

/// <summary>
/// Takt供货商实体
/// 对应前端 TaktSupplierDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSupplierDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SupplierID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（唯一索引）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称
    /// </summary>
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int SupplierType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

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
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
    /// </summary>
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 供货商等级（0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int SupplierLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格供货商（0=否，1=是）
    /// </summary>
    public int IsQualified { get; set; } = 0;

    /// <summary>
    /// 供货商状态（1=启用，0=禁用）
    /// </summary>
    public int SupplierStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// Supplier 查询 DTO
// ========================================

/// <summary>
/// Supplier 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSupplierQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（唯一索引）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称
    /// </summary>
    public string? SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int? SupplierType { get; set; }

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

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
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
    /// </summary>
    public int? PaymentTerms { get; set; }

    /// <summary>
    /// 供货商等级（0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int? SupplierLevel { get; set; }

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal? EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格供货商（0=否，1=是）
    /// </summary>
    public int? IsQualified { get; set; }

    /// <summary>
    /// 供货商状态（1=启用，0=禁用）
    /// </summary>
    public int? SupplierStatus { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

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
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建Supplier DTO
// ========================================

/// <summary>
/// 创建Supplier DTO
/// </summary>
public class TaktSupplierCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "供货商编码（唯一索引）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称
    /// </summary>
    [Required(ErrorMessage = "供货商名称不能为空")]
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int SupplierType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

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
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    [Required(ErrorMessage = "结算币种代码不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
    /// </summary>
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 供货商等级（0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int SupplierLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格供货商（0=否，1=是）
    /// </summary>
    public int IsQualified { get; set; } = 0;

    /// <summary>
    /// 供货商状态（1=启用，0=禁用）
    /// </summary>
    public int SupplierStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新Supplier DTO
// ========================================

/// <summary>
/// 更新Supplier DTO
/// 继承 TaktSupplierCreateDto，添加 SupplierId 字段
/// </summary>
public class TaktSupplierUpdateDto : TaktSupplierCreateDto
{
    /// <summary>
    /// SupplierID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

}

// ========================================
// Supplier 状态 DTO
// ========================================

/// <summary>
/// Supplier 状态更新 DTO
/// </summary>
public class TaktSupplierStatusDto
{
    /// <summary>
    /// SupplierID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

    /// <summary>
    /// 供货商状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "供货商状态（1=启用，0=禁用）不能为空")]
    public int SupplierStatus { get; set; } = 0;
}

// ========================================
// Supplier 排序 DTO
// ========================================

/// <summary>
/// Supplier 排序更新 DTO
/// </summary>
public class TaktSupplierSortDto
{
    /// <summary>
    /// SupplierID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

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
/// Supplier 导入模板行 DTO
/// </summary>
public class TaktSupplierTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（唯一索引）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称
    /// </summary>
    public string? SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int? SupplierType { get; set; }

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

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
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Supplier 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSupplierImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（唯一索引）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称
    /// </summary>
    public string? SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int? SupplierType { get; set; }

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

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
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Supplier 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSupplierExportDto
{
    /// <summary>
    /// SupplierID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（唯一索引）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称
    /// </summary>
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int SupplierType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

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
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
    /// </summary>
    public int PaymentTerms { get; set; } = 0;

    /// <summary>
    /// 供货商等级（0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int SupplierLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格供货商（0=否，1=是）
    /// </summary>
    public int IsQualified { get; set; } = 0;

    /// <summary>
    /// 供货商状态（1=启用，0=禁用）
    /// </summary>
    public int SupplierStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
