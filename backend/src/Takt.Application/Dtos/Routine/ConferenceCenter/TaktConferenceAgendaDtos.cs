// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.ConferenceCenter
// 文件名称：TaktConferenceAgendaDtos.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：ConferenceAgenda 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConferenceAgenda 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.ConferenceCenter;

// ========================================
// ConferenceAgenda 响应 DTO
// ========================================

/// <summary>
/// 会议议程/纪要实体 RecordType=议程项时多行维护议题；RecordType=会议纪要时通常一条记录承载正文与摘要
/// 对应前端 TaktConferenceAgendaDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConferenceAgendaDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConferenceAgendaID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceAgendaId { get; set; }

    /// <summary>
    /// 会议 ID（关联 TaktConference.Id，选项 TaktConferences/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

    /// <summary>
    /// 会议 名称（填充字段）
    /// </summary>
    public string? ConferenceName { get; set; }

    /// <summary>
    /// 记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）
    /// </summary>
    public int RecordType { get; set; } = 0;

    /// <summary>
    /// 行号（议程项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 标题（议程议题或纪要标题）
    /// </summary>
    public string ConferenceAgendaTitle { get; set; } = string.Empty;

    /// <summary>
    /// 正文（议程说明或会议纪要富文本 HTML）
    /// </summary>
    public string? ConferenceAgendaContent { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? ConferenceAgendaSummary { get; set; } = string.Empty;

    /// <summary>
    /// 主讲人/汇报人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PresenterId { get; set; }

    /// <summary>
    /// 主讲人姓名（议程项）
    /// </summary>
    public string? PresenterName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间（议程项）
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划时长（分钟，议程项）
    /// </summary>
    public int DurationMinutes { get; set; } = 0;

    /// <summary>
    /// 记录人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录人姓名（会议纪要）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 会议（主表）
    /// （主表：TaktConference）
    /// </summary>
    public TaktConferenceDto? Conference { get; set; }

}

// ========================================
// ConferenceAgenda 查询 DTO
// ========================================

/// <summary>
/// ConferenceAgenda 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConferenceAgendaQueryDto : TaktPagedQuery
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
    /// 会议 ID（关联 TaktConference.Id，选项 TaktConferences/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceId { get; set; }

    /// <summary>
    /// 记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）
    /// </summary>
    public int? RecordType { get; set; }

    /// <summary>
    /// 行号（议程项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 标题（议程议题或纪要标题）
    /// </summary>
    public string? ConferenceAgendaTitle { get; set; } = string.Empty;

    /// <summary>
    /// 正文（议程说明或会议纪要富文本 HTML）
    /// </summary>
    public string? ConferenceAgendaContent { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? ConferenceAgendaSummary { get; set; } = string.Empty;

    /// <summary>
    /// 主讲人/汇报人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PresenterId { get; set; }

    /// <summary>
    /// 主讲人姓名（议程项）
    /// </summary>
    public string? PresenterName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间（议程项）（范围查询-开始）
    /// </summary>
    public DateTime? PlannedStartTimeStart { get; set; }

    /// <summary>
    /// 计划开始时间（议程项）（范围查询-结束）
    /// </summary>
    public DateTime? PlannedStartTimeEnd { get; set; }

    /// <summary>
    /// 计划时长（分钟，议程项）
    /// </summary>
    public int? DurationMinutes { get; set; }

    /// <summary>
    /// 记录人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录人姓名（会议纪要）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建ConferenceAgenda DTO
// ========================================

/// <summary>
/// 创建ConferenceAgenda DTO
/// </summary>
public class TaktConferenceAgendaCreateDto
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
    /// 会议 ID（关联 TaktConference.Id，选项 TaktConferences/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

    /// <summary>
    /// 记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）
    /// </summary>
    public int RecordType { get; set; } = 0;

    /// <summary>
    /// 行号（议程项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 标题（议程议题或纪要标题）
    /// </summary>
    [Required(ErrorMessage = "标题（议程议题或纪要标题）不能为空")]
    public string ConferenceAgendaTitle { get; set; } = string.Empty;

    /// <summary>
    /// 正文（议程说明或会议纪要富文本 HTML）
    /// </summary>
    public string? ConferenceAgendaContent { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? ConferenceAgendaSummary { get; set; } = string.Empty;

    /// <summary>
    /// 主讲人/汇报人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PresenterId { get; set; }

    /// <summary>
    /// 主讲人姓名（议程项）
    /// </summary>
    public string? PresenterName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间（议程项）
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划时长（分钟，议程项）
    /// </summary>
    public int DurationMinutes { get; set; } = 0;

    /// <summary>
    /// 记录人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录人姓名（会议纪要）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新ConferenceAgenda DTO
// ========================================

/// <summary>
/// 更新ConferenceAgenda DTO
/// 继承 TaktConferenceAgendaCreateDto，添加 ConferenceAgendaId 字段
/// </summary>
public class TaktConferenceAgendaUpdateDto : TaktConferenceAgendaCreateDto
{
    /// <summary>
    /// ConferenceAgendaID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceAgendaId { get; set; }

}

// ========================================
// ConferenceAgenda 作废 DTO
// ========================================

/// <summary>
/// ConferenceAgenda 作废/撤销作废 DTO
/// </summary>
public class TaktConferenceAgendaObsoleteDto
{
    /// <summary>
    /// ConferenceAgendaID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceAgendaId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ConferenceAgenda 导入模板行 DTO
/// </summary>
public class TaktConferenceAgendaTemplateDto
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
    /// 会议 ID（关联 TaktConference.Id，选项 TaktConferences/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceId { get; set; }

    /// <summary>
    /// 记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）
    /// </summary>
    public int? RecordType { get; set; }

    /// <summary>
    /// 行号（议程项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 标题（议程议题或纪要标题）
    /// </summary>
    public string? ConferenceAgendaTitle { get; set; } = string.Empty;

    /// <summary>
    /// 正文（议程说明或会议纪要富文本 HTML）
    /// </summary>
    public string? ConferenceAgendaContent { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? ConferenceAgendaSummary { get; set; } = string.Empty;

    /// <summary>
    /// 主讲人/汇报人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PresenterId { get; set; }

    /// <summary>
    /// 主讲人姓名（议程项）
    /// </summary>
    public string? PresenterName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间（议程项）
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划时长（分钟，议程项）
    /// </summary>
    public int? DurationMinutes { get; set; }

    /// <summary>
    /// 记录人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录人姓名（会议纪要）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// ConferenceAgenda 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConferenceAgendaImportDto
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
    /// 会议 ID（关联 TaktConference.Id，选项 TaktConferences/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceId { get; set; }

    /// <summary>
    /// 记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）
    /// </summary>
    public int? RecordType { get; set; }

    /// <summary>
    /// 行号（议程项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 标题（议程议题或纪要标题）
    /// </summary>
    public string? ConferenceAgendaTitle { get; set; } = string.Empty;

    /// <summary>
    /// 正文（议程说明或会议纪要富文本 HTML）
    /// </summary>
    public string? ConferenceAgendaContent { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? ConferenceAgendaSummary { get; set; } = string.Empty;

    /// <summary>
    /// 主讲人/汇报人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PresenterId { get; set; }

    /// <summary>
    /// 主讲人姓名（议程项）
    /// </summary>
    public string? PresenterName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间（议程项）
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划时长（分钟，议程项）
    /// </summary>
    public int? DurationMinutes { get; set; }

    /// <summary>
    /// 记录人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录人姓名（会议纪要）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// ConferenceAgenda 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConferenceAgendaExportDto
{
    /// <summary>
    /// ConferenceAgendaID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceAgendaId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议 ID（关联 TaktConference.Id，选项 TaktConferences/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

    /// <summary>
    /// 记录类型（字典 routine_conference_record_type；0=议程项 1=会议纪要）
    /// </summary>
    public int RecordType { get; set; } = 0;

    /// <summary>
    /// 行号（议程项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 标题（议程议题或纪要标题）
    /// </summary>
    public string ConferenceAgendaTitle { get; set; } = string.Empty;

    /// <summary>
    /// 正文（议程说明或会议纪要富文本 HTML）
    /// </summary>
    public string? ConferenceAgendaContent { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? ConferenceAgendaSummary { get; set; } = string.Empty;

    /// <summary>
    /// 主讲人/汇报人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PresenterId { get; set; }

    /// <summary>
    /// 主讲人姓名（议程项）
    /// </summary>
    public string? PresenterName { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间（议程项）
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划时长（分钟，议程项）
    /// </summary>
    public int DurationMinutes { get; set; } = 0;

    /// <summary>
    /// 记录人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录人姓名（会议纪要）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
