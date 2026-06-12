// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.ConferenceCenter
// 文件名称：TaktConferenceAgendaDtos.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：ConferenceAgenda 模块 DTO（会议主子表级联议程/纪要）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.ConferenceCenter;

// ========================================
// ConferenceAgenda 响应 DTO
// ========================================

/// <summary>
/// 会议议程/纪要响应 DTO
/// 对应前端 TaktConferenceAgendaDto
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
    /// 会议 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

    /// <summary>
    /// 记录类型（议程项 / 会议纪要）
    /// </summary>
    public int RecordType { get; set; }

    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 正文
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 摘要
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// 主讲人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PresenterId { get; set; }

    /// <summary>
    /// 主讲人姓名
    /// </summary>
    public string? PresenterName { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划时长（分钟）
    /// </summary>
    public int DurationMinutes { get; set; } = 0;

    /// <summary>
    /// 记录人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录人姓名
    /// </summary>
    public string? RecorderName { get; set; }
}

// ========================================
// 创建 ConferenceAgenda DTO
// ========================================

/// <summary>
/// 创建会议议程/纪要 DTO（主子表级联提交）
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
    /// 当前公司默认区域文化 BCP47
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 会议 ID（级联保存时由主表 Id 覆盖）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

    /// <summary>
    /// 记录类型
    /// </summary>
    public int RecordType { get; set; }

    /// <summary>
    /// 行号（≤0 时由服务自动生成）
    /// </summary>
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 标题
    /// </summary>
    [Required(ErrorMessage = "标题不能为空")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 正文
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 摘要
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// 主讲人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PresenterId { get; set; }

    /// <summary>
    /// 主讲人姓名
    /// </summary>
    public string? PresenterName { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划时长（分钟）
    /// </summary>
    public int DurationMinutes { get; set; } = 0;

    /// <summary>
    /// 记录人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录人姓名
    /// </summary>
    public string? RecorderName { get; set; }

    /// <summary>
    /// 扩展字段 JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}
