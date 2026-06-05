// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktManufacturerDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：Manufacturer 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktManufacturer 生成，请按需审阅）
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
// Manufacturer 响应 DTO
// ========================================

/// <summary>
/// Takt制造商实体
/// 对应前端 TaktManufacturerDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktManufacturerDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ManufacturerID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ManufacturerId { get; set; }

    /// <summary>
    /// 制造商编码（唯一索引）
    /// </summary>
    public string ManufacturerCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商名称
    /// </summary>
    public string ManufacturerName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商简称
    /// </summary>
    public string? ManufacturerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商类型（0=原始设备制造商OEM，1=原始设计制造商ODM，2=合同制造商CM，3=品牌制造商，4=其他）
    /// </summary>
    public int ManufacturerType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 制造商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? ManufacturerTaxNumber { get; set; } = string.Empty;

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
    /// 制造商电话
    /// </summary>
    public string? ManufacturerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 制造商传真
    /// </summary>
    public string? ManufacturerFax { get; set; } = string.Empty;

    /// <summary>
    /// 制造商邮箱
    /// </summary>
    public string? ManufacturerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 制造商网站
    /// </summary>
    public string? ManufacturerWebsite { get; set; } = string.Empty;

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
    /// 制造商等级（0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int ManufacturerLevel { get; set; } = 0;

    /// <summary>
    /// 质量认证（0=无，1=ISO9001，2=ISO14001，3=IATF16949，4=其他）
    /// </summary>
    public int QualityCertification { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格制造商（0=否，1=是）
    /// </summary>
    public int IsQualified { get; set; } = 0;

    /// <summary>
    /// 制造商状态（1=启用，0=禁用）
    /// </summary>
    public int ManufacturerStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 导航属性：制造商物料明细列表
    /// （子表：TaktManufacturerMaterial）
    /// </summary>
    public List<TaktManufacturerMaterialDto>? ManufacturerMaterials { get; set; }

}

// ========================================
// Manufacturer 查询 DTO
// ========================================

/// <summary>
/// Manufacturer 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktManufacturerQueryDto : TaktPagedQuery
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
    /// 制造商编码（唯一索引）
    /// </summary>
    public string? ManufacturerCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商名称
    /// </summary>
    public string? ManufacturerName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商简称
    /// </summary>
    public string? ManufacturerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商类型（0=原始设备制造商OEM，1=原始设计制造商ODM，2=合同制造商CM，3=品牌制造商，4=其他）
    /// </summary>
    public int? ManufacturerType { get; set; }

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 制造商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? ManufacturerTaxNumber { get; set; } = string.Empty;

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
    /// 制造商电话
    /// </summary>
    public string? ManufacturerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 制造商传真
    /// </summary>
    public string? ManufacturerFax { get; set; } = string.Empty;

    /// <summary>
    /// 制造商邮箱
    /// </summary>
    public string? ManufacturerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 制造商网站
    /// </summary>
    public string? ManufacturerWebsite { get; set; } = string.Empty;

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
    /// 制造商等级（0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int? ManufacturerLevel { get; set; }

    /// <summary>
    /// 质量认证（0=无，1=ISO9001，2=ISO14001，3=IATF16949，4=其他）
    /// </summary>
    public int? QualityCertification { get; set; }

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal? EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格制造商（0=否，1=是）
    /// </summary>
    public int? IsQualified { get; set; }

    /// <summary>
    /// 制造商状态（1=启用，0=禁用）
    /// </summary>
    public int? ManufacturerStatus { get; set; }

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
// 创建Manufacturer DTO
// ========================================

/// <summary>
/// 创建Manufacturer DTO
/// </summary>
public class TaktManufacturerCreateDto
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
    /// 制造商编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "制造商编码（唯一索引）不能为空")]
    public string ManufacturerCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商名称
    /// </summary>
    [Required(ErrorMessage = "制造商名称不能为空")]
    public string ManufacturerName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商简称
    /// </summary>
    public string? ManufacturerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商类型（0=原始设备制造商OEM，1=原始设计制造商ODM，2=合同制造商CM，3=品牌制造商，4=其他）
    /// </summary>
    public int ManufacturerType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 制造商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? ManufacturerTaxNumber { get; set; } = string.Empty;

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
    /// 制造商电话
    /// </summary>
    public string? ManufacturerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 制造商传真
    /// </summary>
    public string? ManufacturerFax { get; set; } = string.Empty;

    /// <summary>
    /// 制造商邮箱
    /// </summary>
    public string? ManufacturerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 制造商网站
    /// </summary>
    public string? ManufacturerWebsite { get; set; } = string.Empty;

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
    /// 制造商等级（0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int ManufacturerLevel { get; set; } = 0;

    /// <summary>
    /// 质量认证（0=无，1=ISO9001，2=ISO14001，3=IATF16949，4=其他）
    /// </summary>
    public int QualityCertification { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格制造商（0=否，1=是）
    /// </summary>
    public int IsQualified { get; set; } = 0;

    /// <summary>
    /// 制造商状态（1=启用，0=禁用）
    /// </summary>
    public int ManufacturerStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 导航属性：制造商物料明细列表（子表，级联保存）
    /// </summary>
    public List<TaktManufacturerMaterialCreateDto>? ManufacturerMaterials { get; set; }

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
// 更新Manufacturer DTO
// ========================================

/// <summary>
/// 更新Manufacturer DTO
/// 继承 TaktManufacturerCreateDto，添加 ManufacturerId 字段
/// </summary>
public class TaktManufacturerUpdateDto : TaktManufacturerCreateDto
{
    /// <summary>
    /// ManufacturerID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ManufacturerId { get; set; }

}

// ========================================
// Manufacturer 状态 DTO
// ========================================

/// <summary>
/// Manufacturer 状态更新 DTO
/// </summary>
public class TaktManufacturerStatusDto
{
    /// <summary>
    /// ManufacturerID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ManufacturerId { get; set; }

    /// <summary>
    /// 制造商状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "制造商状态（1=启用，0=禁用）不能为空")]
    public int ManufacturerStatus { get; set; } = 0;
}

// ========================================
// Manufacturer 排序 DTO
// ========================================

/// <summary>
/// Manufacturer 排序更新 DTO
/// </summary>
public class TaktManufacturerSortDto
{
    /// <summary>
    /// ManufacturerID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ManufacturerId { get; set; }

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
/// Manufacturer 导入模板行 DTO
/// </summary>
public class TaktManufacturerTemplateDto
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
    /// 制造商编码（唯一索引）
    /// </summary>
    public string? ManufacturerCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商名称
    /// </summary>
    public string? ManufacturerName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商简称
    /// </summary>
    public string? ManufacturerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商类型（0=原始设备制造商OEM，1=原始设计制造商ODM，2=合同制造商CM，3=品牌制造商，4=其他）
    /// </summary>
    public int? ManufacturerType { get; set; }

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 制造商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? ManufacturerTaxNumber { get; set; } = string.Empty;

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
    /// 制造商电话
    /// </summary>
    public string? ManufacturerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 制造商传真
    /// </summary>
    public string? ManufacturerFax { get; set; } = string.Empty;

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
/// Manufacturer 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktManufacturerImportDto
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
    /// 制造商编码（唯一索引）
    /// </summary>
    public string? ManufacturerCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商名称
    /// </summary>
    public string? ManufacturerName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商简称
    /// </summary>
    public string? ManufacturerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商类型（0=原始设备制造商OEM，1=原始设计制造商ODM，2=合同制造商CM，3=品牌制造商，4=其他）
    /// </summary>
    public int? ManufacturerType { get; set; }

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 制造商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? ManufacturerTaxNumber { get; set; } = string.Empty;

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
    /// 制造商电话
    /// </summary>
    public string? ManufacturerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 制造商传真
    /// </summary>
    public string? ManufacturerFax { get; set; } = string.Empty;

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
/// Manufacturer 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktManufacturerExportDto
{
    /// <summary>
    /// ManufacturerID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ManufacturerId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（唯一索引）
    /// </summary>
    public string ManufacturerCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商名称
    /// </summary>
    public string ManufacturerName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商简称
    /// </summary>
    public string? ManufacturerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商类型（0=原始设备制造商OEM，1=原始设计制造商ODM，2=合同制造商CM，3=品牌制造商，4=其他）
    /// </summary>
    public int ManufacturerType { get; set; } = 0;

    /// <summary>
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 制造商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? ManufacturerTaxNumber { get; set; } = string.Empty;

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
    /// 制造商电话
    /// </summary>
    public string? ManufacturerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 制造商传真
    /// </summary>
    public string? ManufacturerFax { get; set; } = string.Empty;

    /// <summary>
    /// 制造商邮箱
    /// </summary>
    public string? ManufacturerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 制造商网站
    /// </summary>
    public string? ManufacturerWebsite { get; set; } = string.Empty;

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
    /// 制造商等级（0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int ManufacturerLevel { get; set; } = 0;

    /// <summary>
    /// 质量认证（0=无，1=ISO9001，2=ISO14001，3=IATF16949，4=其他）
    /// </summary>
    public int QualityCertification { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 是否合格制造商（0=否，1=是）
    /// </summary>
    public int IsQualified { get; set; } = 0;

    /// <summary>
    /// 制造商状态（1=启用，0=禁用）
    /// </summary>
    public int ManufacturerStatus { get; set; } = 0;

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
