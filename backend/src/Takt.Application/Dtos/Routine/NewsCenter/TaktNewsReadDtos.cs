// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.NewsCenter
// 文件名称：TaktNewsReadDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：NewsRead 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktNewsRead 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.NewsCenter;

// ========================================
// NewsRead 响应 DTO
// ========================================

/// <summary>
/// 新闻中心阅读记录实体
/// 对应前端 TaktNewsReadDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktNewsReadDto : TaktCompanyDtoBase
{
    /// <summary>
    /// NewsReadID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsReadId { get; set; }

    /// <summary>
    /// 新闻 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

    /// <summary>
    /// 新闻 名称（填充字段）
    /// </summary>
    public string? NewsName { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 阅读时间
    /// </summary>
    public DateTime ReadTime { get; set; }

    /// <summary>
    /// 新闻（主表）
    /// （主表：TaktNews）
    /// </summary>
    public TaktNewsDto? News { get; set; }

}

// ========================================
// NewsRead 查询 DTO
// ========================================

/// <summary>
/// NewsRead 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktNewsReadQueryDto : TaktPagedQuery
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
    /// 新闻 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? NewsId { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 阅读时间（范围查询-开始）
    /// </summary>
    public DateTime? ReadTimeStart { get; set; }

    /// <summary>
    /// 阅读时间（范围查询-结束）
    /// </summary>
    public DateTime? ReadTimeEnd { get; set; }

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
// 创建NewsRead DTO
// ========================================

/// <summary>
/// 创建NewsRead DTO
/// </summary>
public class TaktNewsReadCreateDto
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
    /// 新闻 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    [Required(ErrorMessage = "用户姓名不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 阅读时间
    /// </summary>
    public DateTime ReadTime { get; set; }

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
// 更新NewsRead DTO
// ========================================

/// <summary>
/// 更新NewsRead DTO
/// 继承 TaktNewsReadCreateDto，添加 NewsReadId 字段
/// </summary>
public class TaktNewsReadUpdateDto : TaktNewsReadCreateDto
{
    /// <summary>
    /// NewsReadID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsReadId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// NewsRead 导入模板行 DTO
/// </summary>
public class TaktNewsReadTemplateDto
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
    /// 新闻 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? NewsId { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

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
/// NewsRead 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktNewsReadImportDto
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
    /// 新闻 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? NewsId { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

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
/// NewsRead 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktNewsReadExportDto
{
    /// <summary>
    /// NewsReadID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsReadId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 新闻 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 阅读时间
    /// </summary>
    public DateTime ReadTime { get; set; }

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
