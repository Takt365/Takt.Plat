// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktKnowledgeChangeLogDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：KnowledgeChangeLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktKnowledgeChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.HelpDesk;

// ========================================
// KnowledgeChangeLog 响应 DTO
// ========================================

/// <summary>
/// 知识库变更日志实体
/// 对应前端 TaktKnowledgeChangeLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktKnowledgeChangeLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// KnowledgeChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeChangeLogId { get; set; }

    /// <summary>
    /// 知识 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeId { get; set; }

    /// <summary>
    /// 知识 名称（填充字段）
    /// </summary>
    public string? KnowledgeName { get; set; }

    /// <summary>
    /// 知识标题（冗余）
    /// </summary>
    public string? KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型（0=创建，1=更新，2=删除）
    /// </summary>
    public int ChangeType { get; set; } = 0;

    /// <summary>
    /// 修改内容摘要
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
    /// 变更时知识版本号
    /// </summary>
    public int? VersionAtChange { get; set; }

    /// <summary>
    /// 知识库（主表）
    /// （主表：TaktKnowledge）
    /// </summary>
    public TaktKnowledgeDto? Knowledge { get; set; }

}

// ========================================
// KnowledgeChangeLog 查询 DTO
// ========================================

/// <summary>
/// KnowledgeChangeLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktKnowledgeChangeLogQueryDto : TaktPagedQuery
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
    /// 知识 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }

    /// <summary>
    /// 知识标题（冗余）
    /// </summary>
    public string? KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型（0=创建，1=更新，2=删除）
    /// </summary>
    public int? ChangeType { get; set; }

    /// <summary>
    /// 修改内容摘要
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
    /// 变更时知识版本号
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
// 创建KnowledgeChangeLog DTO
// ========================================

/// <summary>
/// 创建KnowledgeChangeLog DTO
/// </summary>
public class TaktKnowledgeChangeLogCreateDto
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
    /// 知识 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeId { get; set; }

    /// <summary>
    /// 知识标题（冗余）
    /// </summary>
    public string? KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型（0=创建，1=更新，2=删除）
    /// </summary>
    public int ChangeType { get; set; } = 0;

    /// <summary>
    /// 修改内容摘要
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
    /// 变更时知识版本号
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
// 更新KnowledgeChangeLog DTO
// ========================================

/// <summary>
/// 更新KnowledgeChangeLog DTO
/// 继承 TaktKnowledgeChangeLogCreateDto，添加 KnowledgeChangeLogId 字段
/// </summary>
public class TaktKnowledgeChangeLogUpdateDto : TaktKnowledgeChangeLogCreateDto
{
    /// <summary>
    /// KnowledgeChangeLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeChangeLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// KnowledgeChangeLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktKnowledgeChangeLogExportDto
{
    /// <summary>
    /// KnowledgeChangeLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeChangeLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 知识 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeId { get; set; }

    /// <summary>
    /// 知识标题（冗余）
    /// </summary>
    public string? KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型（0=创建，1=更新，2=删除）
    /// </summary>
    public int ChangeType { get; set; } = 0;

    /// <summary>
    /// 修改内容摘要
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
    /// 变更时知识版本号
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
