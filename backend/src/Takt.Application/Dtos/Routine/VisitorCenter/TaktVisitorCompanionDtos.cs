// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.VisitorCenter
// 文件名称：TaktVisitorCompanionDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：VisitorCompanion 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktVisitorCompanion 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.VisitorCenter;

// ========================================
// VisitorCompanion 响应 DTO
// ========================================

/// <summary>
/// 来访人员子实体（部门、职称、姓名）
/// 对应前端 TaktVisitorCompanionDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktVisitorCompanionDto : TaktCompanyDtoBase
{
    /// <summary>
    /// VisitorCompanionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorCompanionId { get; set; }

    /// <summary>
    /// 来访记录 ID（选项 TaktVisitors/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorId { get; set; }

    /// <summary>
    /// 来访记录 名称（填充字段）
    /// </summary>
    public string? VisitorName { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// 职称
    /// </summary>
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 来访人员姓名
    /// </summary>
    public string CompanionName { get; set; } = string.Empty;

    /// <summary>
    /// 来访记录（主表）
    /// （主表：TaktVisitor）
    /// </summary>
    public TaktVisitorDto? Visitor { get; set; }

}

// ========================================
// VisitorCompanion 查询 DTO
// ========================================

/// <summary>
/// VisitorCompanion 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktVisitorCompanionQueryDto : TaktPagedQuery
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
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 来访记录 ID（选项 TaktVisitors/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VisitorId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public string? Department { get; set; } = string.Empty;

    /// <summary>
    /// 职称
    /// </summary>
    public string? JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 来访人员姓名
    /// </summary>
    public string? CompanionName { get; set; } = string.Empty;

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
// 创建VisitorCompanion DTO
// ========================================

/// <summary>
/// 创建VisitorCompanion DTO
/// </summary>
public class TaktVisitorCompanionCreateDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 来访记录 ID（选项 TaktVisitors/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    [Required(ErrorMessage = "部门不能为空")]
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// 职称
    /// </summary>
    [Required(ErrorMessage = "职称不能为空")]
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 来访人员姓名
    /// </summary>
    [Required(ErrorMessage = "来访人员姓名不能为空")]
    public string CompanionName { get; set; } = string.Empty;

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
// 更新VisitorCompanion DTO
// ========================================

/// <summary>
/// 更新VisitorCompanion DTO
/// 继承 TaktVisitorCompanionCreateDto，添加 VisitorCompanionId 字段
/// </summary>
public class TaktVisitorCompanionUpdateDto : TaktVisitorCompanionCreateDto
{
    /// <summary>
    /// VisitorCompanionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorCompanionId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// VisitorCompanion 导入模板行 DTO
/// </summary>
public class TaktVisitorCompanionTemplateDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 来访记录 ID（选项 TaktVisitors/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VisitorId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public string? Department { get; set; } = string.Empty;

    /// <summary>
    /// 职称
    /// </summary>
    public string? JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 来访人员姓名
    /// </summary>
    public string? CompanionName { get; set; } = string.Empty;

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
/// VisitorCompanion 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktVisitorCompanionImportDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 来访记录 ID（选项 TaktVisitors/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VisitorId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public string? Department { get; set; } = string.Empty;

    /// <summary>
    /// 职称
    /// </summary>
    public string? JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 来访人员姓名
    /// </summary>
    public string? CompanionName { get; set; } = string.Empty;

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
/// VisitorCompanion 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktVisitorCompanionExportDto
{
    /// <summary>
    /// VisitorCompanionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorCompanionId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 来访记录 ID（选项 TaktVisitors/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// 职称
    /// </summary>
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 来访人员姓名
    /// </summary>
    public string CompanionName { get; set; } = string.Empty;

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
