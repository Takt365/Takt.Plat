// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Scheduling
// 文件名称：TaktChangeoverMatrixDtos.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：ChangeoverMatrix 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktChangeoverMatrix 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;

// ========================================
// ChangeoverMatrix 响应 DTO
// ========================================

/// <summary>
/// 换型矩阵（工作中心 + 前产品 → 后产品的换型时间）
/// 对应前端 TaktChangeoverMatrixDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktChangeoverMatrixDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ChangeoverMatrixID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ChangeoverMatrixId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码
    /// </summary>
    public string WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型前物料编码
    /// </summary>
    public string FromMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型后物料编码
    /// </summary>
    public string ToMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal ChangeoverMinutes { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int MatrixStatus { get; set; } = 0;

}

// ========================================
// ChangeoverMatrix 查询 DTO
// ========================================

/// <summary>
/// ChangeoverMatrix 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktChangeoverMatrixQueryDto : TaktPagedQuery
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
    /// 工作中心编码
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型前物料编码
    /// </summary>
    public string? FromMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型后物料编码
    /// </summary>
    public string? ToMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal? ChangeoverMinutes { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? MatrixStatus { get; set; }

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
// 创建ChangeoverMatrix DTO
// ========================================

/// <summary>
/// 创建ChangeoverMatrix DTO
/// </summary>
public class TaktChangeoverMatrixCreateDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码
    /// </summary>
    [Required(ErrorMessage = "工作中心编码不能为空")]
    public string WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型前物料编码
    /// </summary>
    [Required(ErrorMessage = "换型前物料编码不能为空")]
    public string FromMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型后物料编码
    /// </summary>
    [Required(ErrorMessage = "换型后物料编码不能为空")]
    public string ToMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal ChangeoverMinutes { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int MatrixStatus { get; set; } = 0;

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
// 更新ChangeoverMatrix DTO
// ========================================

/// <summary>
/// 更新ChangeoverMatrix DTO
/// 继承 TaktChangeoverMatrixCreateDto，添加 ChangeoverMatrixId 字段
/// </summary>
public class TaktChangeoverMatrixUpdateDto : TaktChangeoverMatrixCreateDto
{
    /// <summary>
    /// ChangeoverMatrixID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ChangeoverMatrixId { get; set; }

}

// ========================================
// ChangeoverMatrix 状态 DTO
// ========================================

/// <summary>
/// ChangeoverMatrix 状态更新 DTO
/// </summary>
public class TaktChangeoverMatrixStatusDto
{
    /// <summary>
    /// ChangeoverMatrixID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ChangeoverMatrixId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int MatrixStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ChangeoverMatrix 导入模板行 DTO
/// </summary>
public class TaktChangeoverMatrixTemplateDto
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
    /// 工作中心编码
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型前物料编码
    /// </summary>
    public string? FromMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型后物料编码
    /// </summary>
    public string? ToMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal? ChangeoverMinutes { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? MatrixStatus { get; set; }

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
/// ChangeoverMatrix 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktChangeoverMatrixImportDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型前物料编码
    /// </summary>
    public string? FromMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型后物料编码
    /// </summary>
    public string? ToMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal? ChangeoverMinutes { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? MatrixStatus { get; set; }

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
/// ChangeoverMatrix 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktChangeoverMatrixExportDto
{
    /// <summary>
    /// ChangeoverMatrixID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ChangeoverMatrixId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码
    /// </summary>
    public string WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型前物料编码
    /// </summary>
    public string FromMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型后物料编码
    /// </summary>
    public string ToMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal ChangeoverMinutes { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int MatrixStatus { get; set; } = 0;

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
