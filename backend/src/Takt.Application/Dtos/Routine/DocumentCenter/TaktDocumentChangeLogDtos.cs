// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.DocumentCenter
// 文件名称：TaktDocumentChangeLogDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：DocumentChangeLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDocumentChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Routine.DocumentCenter;

// ========================================
// DocumentChangeLog 响应 DTO
// ========================================

/// <summary>
/// 文管文档变更日志实体 完整记录文档的创建、修订、发布、归档、删除等历史
/// 对应前端 TaktDocumentChangeLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktDocumentChangeLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// DocumentChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentChangeLogId { get; set; }

    /// <summary>
    /// 文档 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 文档 名称（填充字段）
    /// </summary>
    public string? DocumentName { get; set; }

    /// <summary>
    /// 文档编码（冗余，便于日志列表展示）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题（冗余，便于日志列表展示）
    /// </summary>
    public string? DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型
    /// </summary>
    public TaktDocumentChangeType ChangeType { get; set; }

    /// <summary>
    /// 变更内容摘要
    /// </summary>
    public string? ChangeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON 数组）
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因或备注
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 变更时文档版本号
    /// </summary>
    public int? VersionAtChange { get; set; }

    /// <summary>
    /// 文档（主表）
    /// （主表：TaktDocument）
    /// </summary>
    public TaktDocumentDto? Document { get; set; }

}

// ========================================
// DocumentChangeLog 查询 DTO
// ========================================

/// <summary>
/// DocumentChangeLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDocumentChangeLogQueryDto : TaktPagedQuery
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
    /// 文档 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DocumentId { get; set; }

    /// <summary>
    /// 文档编码（冗余，便于日志列表展示）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题（冗余，便于日志列表展示）
    /// </summary>
    public string? DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型
    /// </summary>
    public TaktDocumentChangeType? ChangeType { get; set; }

    /// <summary>
    /// 变更内容摘要
    /// </summary>
    public string? ChangeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON 数组）
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因或备注
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 变更时文档版本号
    /// </summary>
    public int? VersionAtChange { get; set; }

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
// 创建DocumentChangeLog DTO
// ========================================

/// <summary>
/// 创建DocumentChangeLog DTO
/// </summary>
public class TaktDocumentChangeLogCreateDto
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
    /// 文档 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 文档编码（冗余，便于日志列表展示）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题（冗余，便于日志列表展示）
    /// </summary>
    public string? DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型
    /// </summary>
    public TaktDocumentChangeType ChangeType { get; set; }

    /// <summary>
    /// 变更内容摘要
    /// </summary>
    public string? ChangeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON 数组）
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因或备注
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 变更时文档版本号
    /// </summary>
    public int? VersionAtChange { get; set; }

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
// 更新DocumentChangeLog DTO
// ========================================

/// <summary>
/// 更新DocumentChangeLog DTO
/// 继承 TaktDocumentChangeLogCreateDto，添加 DocumentChangeLogId 字段
/// </summary>
public class TaktDocumentChangeLogUpdateDto : TaktDocumentChangeLogCreateDto
{
    /// <summary>
    /// DocumentChangeLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentChangeLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// DocumentChangeLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDocumentChangeLogExportDto
{
    /// <summary>
    /// DocumentChangeLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentChangeLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 文档编码（冗余，便于日志列表展示）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题（冗余，便于日志列表展示）
    /// </summary>
    public string? DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型
    /// </summary>
    public TaktDocumentChangeType ChangeType { get; set; }

    /// <summary>
    /// 变更内容摘要
    /// </summary>
    public string? ChangeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON 数组）
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因或备注
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 变更时文档版本号
    /// </summary>
    public int? VersionAtChange { get; set; }

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
