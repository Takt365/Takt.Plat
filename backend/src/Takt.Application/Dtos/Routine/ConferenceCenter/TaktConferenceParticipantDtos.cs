// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.ConferenceCenter
// 文件名称：TaktConferenceParticipantDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：ConferenceParticipant 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConferenceParticipant 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Routine.ConferenceCenter;

// ========================================
// ConferenceParticipant 响应 DTO
// ========================================

/// <summary>
/// 会议参与人子实体
/// 对应前端 TaktConferenceParticipantDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConferenceParticipantDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConferenceParticipantID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceParticipantId { get; set; }

    /// <summary>
    /// 会议 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

    /// <summary>
    /// 会议 名称（填充字段）
    /// </summary>
    public string? ConferenceName { get; set; }

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
    /// 参与角色
    /// </summary>
    public TaktConferenceParticipantRole ParticipantRole { get; set; }

    /// <summary>
    /// 出席状态
    /// </summary>
    public TaktConferenceAttendanceStatus AttendanceStatus { get; set; }

    /// <summary>
    /// 签到时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 签退时间
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

    /// <summary>
    /// 会议（主表）
    /// （主表：TaktConference）
    /// </summary>
    public TaktConferenceDto? Conference { get; set; }

}

// ========================================
// ConferenceParticipant 查询 DTO
// ========================================

/// <summary>
/// ConferenceParticipant 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConferenceParticipantQueryDto : TaktPagedQuery
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
    /// 会议 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceId { get; set; }

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
    /// 参与角色
    /// </summary>
    public TaktConferenceParticipantRole? ParticipantRole { get; set; }

    /// <summary>
    /// 出席状态
    /// </summary>
    public TaktConferenceAttendanceStatus? AttendanceStatus { get; set; }

    /// <summary>
    /// 签到时间（范围查询-开始）
    /// </summary>
    public DateTime? CheckInTimeStart { get; set; }

    /// <summary>
    /// 签到时间（范围查询-结束）
    /// </summary>
    public DateTime? CheckInTimeEnd { get; set; }

    /// <summary>
    /// 签退时间（范围查询-开始）
    /// </summary>
    public DateTime? CheckOutTimeStart { get; set; }

    /// <summary>
    /// 签退时间（范围查询-结束）
    /// </summary>
    public DateTime? CheckOutTimeEnd { get; set; }

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
// 创建ConferenceParticipant DTO
// ========================================

/// <summary>
/// 创建ConferenceParticipant DTO
/// </summary>
public class TaktConferenceParticipantCreateDto
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
    /// 会议 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

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
    /// 参与角色
    /// </summary>
    public TaktConferenceParticipantRole ParticipantRole { get; set; }

    /// <summary>
    /// 出席状态
    /// </summary>
    public TaktConferenceAttendanceStatus AttendanceStatus { get; set; }

    /// <summary>
    /// 签到时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 签退时间
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

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
// 更新ConferenceParticipant DTO
// ========================================

/// <summary>
/// 更新ConferenceParticipant DTO
/// 继承 TaktConferenceParticipantCreateDto，添加 ConferenceParticipantId 字段
/// </summary>
public class TaktConferenceParticipantUpdateDto : TaktConferenceParticipantCreateDto
{
    /// <summary>
    /// ConferenceParticipantID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceParticipantId { get; set; }

}

// ========================================
// ConferenceParticipant 状态 DTO
// ========================================

/// <summary>
/// ConferenceParticipant 状态更新 DTO
/// </summary>
public class TaktConferenceParticipantStatusDto
{
    /// <summary>
    /// ConferenceParticipantID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceParticipantId { get; set; }

    /// <summary>
    /// 出席状态
    /// </summary>
    [Required(ErrorMessage = "出席状态不能为空")]
    public TaktConferenceAttendanceStatus AttendanceStatus { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ConferenceParticipant 导入模板行 DTO
/// </summary>
public class TaktConferenceParticipantTemplateDto
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
    /// 会议 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceId { get; set; }

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
    /// 参与角色
    /// </summary>
    public TaktConferenceParticipantRole? ParticipantRole { get; set; }

    /// <summary>
    /// 出席状态
    /// </summary>
    public TaktConferenceAttendanceStatus? AttendanceStatus { get; set; }

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
/// ConferenceParticipant 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConferenceParticipantImportDto
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
    /// 会议 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConferenceId { get; set; }

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
    /// 参与角色
    /// </summary>
    public TaktConferenceParticipantRole? ParticipantRole { get; set; }

    /// <summary>
    /// 出席状态
    /// </summary>
    public TaktConferenceAttendanceStatus? AttendanceStatus { get; set; }

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
/// ConferenceParticipant 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConferenceParticipantExportDto
{
    /// <summary>
    /// ConferenceParticipantID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceParticipantId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConferenceId { get; set; }

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
    /// 参与角色
    /// </summary>
    public TaktConferenceParticipantRole ParticipantRole { get; set; }

    /// <summary>
    /// 出席状态
    /// </summary>
    public TaktConferenceAttendanceStatus AttendanceStatus { get; set; }

    /// <summary>
    /// 签到时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 签退时间
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

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
