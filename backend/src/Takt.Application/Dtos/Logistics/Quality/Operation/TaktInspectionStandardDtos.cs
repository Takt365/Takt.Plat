// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardDtos.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Auto Generated)
// 功能描述：InspectionStandard 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktInspectionStandard 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

// ========================================
// InspectionStandard 响应 DTO
// ========================================

/// <summary>
/// 检验标准实体（IQC/IPQC/FQC通用）
/// 对应前端 TaktInspectionStandardDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktInspectionStandardDto : TaktCompanyDtoBase
{
    /// <summary>
    /// InspectionStandardID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准编码（唯一索引）
    /// </summary>
    public string StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准名称
    /// </summary>
    public string StandardName { get; set; } = string.Empty;

    /// <summary>
    /// 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
    /// </summary>
    public int InspectionType { get; set; } = 0;

    /// <summary>
    /// 物料类别编码
    /// </summary>
    public string MaterialCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别名称
    /// </summary>
    public string MaterialCategoryName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string? SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int IsEnabled { get; set; } = 0;

    /// <summary>
    /// 检验标准状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int StandardStatus { get; set; } = 0;

    /// <summary>
    /// 检验标准描述
    /// </summary>
    public string? StandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准明细列表（主子表关系）
    /// （子表：TaktInspectionStandardItem）
    /// </summary>
    public List<TaktInspectionStandardItemDto>? Items { get; set; }

}

// ========================================
// InspectionStandard 查询 DTO
// ========================================

/// <summary>
/// InspectionStandard 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktInspectionStandardQueryDto : TaktPagedQuery
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
    /// 检验标准编码（唯一索引）
    /// </summary>
    public string? StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准名称
    /// </summary>
    public string? StandardName { get; set; } = string.Empty;

    /// <summary>
    /// 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
    /// </summary>
    public int? InspectionType { get; set; }

    /// <summary>
    /// 物料类别编码
    /// </summary>
    public string? MaterialCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别名称
    /// </summary>
    public string? MaterialCategoryName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string? SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int? IsEnabled { get; set; }

    /// <summary>
    /// 检验标准状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int? StandardStatus { get; set; }

    /// <summary>
    /// 检验标准描述
    /// </summary>
    public string? StandardDescription { get; set; } = string.Empty;

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
// 创建InspectionStandard DTO
// ========================================

/// <summary>
/// 创建InspectionStandard DTO
/// </summary>
public class TaktInspectionStandardCreateDto
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
    /// 检验标准编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "检验标准编码（唯一索引）不能为空")]
    public string StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准名称
    /// </summary>
    [Required(ErrorMessage = "检验标准名称不能为空")]
    public string StandardName { get; set; } = string.Empty;

    /// <summary>
    /// 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
    /// </summary>
    public int InspectionType { get; set; } = 0;

    /// <summary>
    /// 物料类别编码
    /// </summary>
    [Required(ErrorMessage = "物料类别编码不能为空")]
    public string MaterialCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别名称
    /// </summary>
    [Required(ErrorMessage = "物料类别名称不能为空")]
    public string MaterialCategoryName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string? SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int IsEnabled { get; set; } = 0;

    /// <summary>
    /// 检验标准状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int StandardStatus { get; set; } = 0;

    /// <summary>
    /// 检验标准描述
    /// </summary>
    public string? StandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktInspectionStandardItemCreateDto>? Items { get; set; }

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
// 更新InspectionStandard DTO
// ========================================

/// <summary>
/// 更新InspectionStandard DTO
/// 继承 TaktInspectionStandardCreateDto，添加 InspectionStandardId 字段
/// </summary>
public class TaktInspectionStandardUpdateDto : TaktInspectionStandardCreateDto
{
    /// <summary>
    /// InspectionStandardID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardId { get; set; }

}

// ========================================
// InspectionStandard 状态 DTO
// ========================================

/// <summary>
/// InspectionStandard 状态更新 DTO
/// </summary>
public class TaktInspectionStandardStatusDto
{
    /// <summary>
    /// InspectionStandardID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardId { get; set; }

    /// <summary>
    /// 检验标准状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    [Required(ErrorMessage = "检验标准状态（0=草稿，1=已发布，2=已停用）不能为空")]
    public int StandardStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// InspectionStandard 导入模板行 DTO
/// </summary>
public class TaktInspectionStandardTemplateDto
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
    /// 检验标准编码（唯一索引）
    /// </summary>
    public string? StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准名称
    /// </summary>
    public string? StandardName { get; set; } = string.Empty;

    /// <summary>
    /// 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
    /// </summary>
    public int? InspectionType { get; set; }

    /// <summary>
    /// 物料类别编码
    /// </summary>
    public string? MaterialCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别名称
    /// </summary>
    public string? MaterialCategoryName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string? SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int? IsEnabled { get; set; }

    /// <summary>
    /// 检验标准状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int? StandardStatus { get; set; }

    /// <summary>
    /// 检验标准描述
    /// </summary>
    public string? StandardDescription { get; set; } = string.Empty;

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
/// InspectionStandard 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktInspectionStandardImportDto
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
    /// 检验标准编码（唯一索引）
    /// </summary>
    public string? StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准名称
    /// </summary>
    public string? StandardName { get; set; } = string.Empty;

    /// <summary>
    /// 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
    /// </summary>
    public int? InspectionType { get; set; }

    /// <summary>
    /// 物料类别编码
    /// </summary>
    public string? MaterialCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别名称
    /// </summary>
    public string? MaterialCategoryName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string? SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int? IsEnabled { get; set; }

    /// <summary>
    /// 检验标准状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int? StandardStatus { get; set; }

    /// <summary>
    /// 检验标准描述
    /// </summary>
    public string? StandardDescription { get; set; } = string.Empty;

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
/// InspectionStandard 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktInspectionStandardExportDto
{
    /// <summary>
    /// InspectionStandardID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准编码（唯一索引）
    /// </summary>
    public string StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验标准名称
    /// </summary>
    public string StandardName { get; set; } = string.Empty;

    /// <summary>
    /// 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
    /// </summary>
    public int InspectionType { get; set; } = 0;

    /// <summary>
    /// 物料类别编码
    /// </summary>
    public string MaterialCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别名称
    /// </summary>
    public string MaterialCategoryName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string? SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int IsEnabled { get; set; } = 0;

    /// <summary>
    /// 检验标准状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int StandardStatus { get; set; } = 0;

    /// <summary>
    /// 检验标准描述
    /// </summary>
    public string? StandardDescription { get; set; } = string.Empty;

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
