// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktMessageDtos.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Auto Generated)
// 功能描述：Message 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMessage 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// Message 响应 DTO
// ========================================

/// <summary>
/// 在线消息实体 公司级实体：消息按租户+公司双重隔离
/// 对应前端 TaktMessageDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMessageDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MessageID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

    /// <summary>
    /// 发送者用户名
    /// </summary>
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromUserId { get; set; }

    /// <summary>
    /// 接收者用户名
    /// </summary>
    public string ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToUserId { get; set; }

    /// <summary>
    /// 是否抄送发送者本人（自审计，默认是）
    /// </summary>
    public int IsCc { get; set; } = 1;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件列表 JSON
    /// </summary>
    public string? Attachments { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public int MessageType { get; set; } = 1;

    /// <summary>
    /// 消息分组
    /// </summary>
    public int MessageGroup { get; set; } = 1;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 读取状态（0=未读，1=已读）
    /// </summary>
    public int ReadStatus { get; set; } = 0;

}

// ========================================
// Message 查询 DTO
// ========================================

/// <summary>
/// Message 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMessageQueryDto : TaktPagedQuery
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
    /// 发送者用户名
    /// </summary>
    public string? FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromUserId { get; set; }

    /// <summary>
    /// 接收者用户名（模糊）
    /// </summary>
    public string? ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToUserId { get; set; }

    /// <summary>
    /// 是否抄送发送者本人（自审计）
    /// </summary>
    public int? IsCc { get; set; }

    /// <summary>
    /// 消息标题
    /// </summary>
    public string? MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string? MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件（模糊）
    /// </summary>
    public string? Attachments { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public int? MessageType { get; set; }

    /// <summary>
    /// 消息分组
    /// </summary>
    public int? MessageGroup { get; set; }

    /// <summary>
    /// 发送时间（范围查询-开始）
    /// </summary>
    public DateTime? SendTimeStart { get; set; }

    /// <summary>
    /// 发送时间（范围查询-结束）
    /// </summary>
    public DateTime? SendTimeEnd { get; set; }

    /// <summary>
    /// 读取时间（范围查询-开始）
    /// </summary>
    public DateTime? ReadTimeStart { get; set; }

    /// <summary>
    /// 读取时间（范围查询-结束）
    /// </summary>
    public DateTime? ReadTimeEnd { get; set; }

    /// <summary>
    /// 读取状态（0=未读，1=已读）
    /// </summary>
    public int? ReadStatus { get; set; }

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
// 收件箱已读 / 未读列表查询 DTO
// ========================================

/// <summary>
/// 当前登录用户收件箱消息分页查询 DTO（已读/未读列表共用；接收者与 ReadStatus 由服务端按路由固定）
/// </summary>
public class TaktMessageInboxListQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 发送者用户名（模糊）
    /// </summary>
    public string? FromUserName { get; set; }

    /// <summary>
    /// 消息标题（模糊）
    /// </summary>
    public string? MessageTitle { get; set; }

    /// <summary>
    /// 消息内容（模糊）
    /// </summary>
    public string? MessageContent { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public int? MessageType { get; set; }

    /// <summary>
    /// 消息分组
    /// </summary>
    public int? MessageGroup { get; set; }

    /// <summary>
    /// 发送时间（范围查询-开始）
    /// </summary>
    public DateTime? SendTimeStart { get; set; }

    /// <summary>
    /// 发送时间（范围查询-结束）
    /// </summary>
    public DateTime? SendTimeEnd { get; set; }

    /// <summary>
    /// 读取时间（范围查询-开始，已读列表可用）
    /// </summary>
    public DateTime? ReadTimeStart { get; set; }

    /// <summary>
    /// 读取时间（范围查询-结束，已读列表可用）
    /// </summary>
    public DateTime? ReadTimeEnd { get; set; }
}

// ========================================
// 创建Message DTO
// ========================================

/// <summary>
/// 创建Message DTO
/// </summary>
public class TaktMessageCreateDto
{
    /// <summary>
    /// 发送者用户名
    /// </summary>
    [Required(ErrorMessage = "发送者用户名不能为空")]
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromUserId { get; set; }

    /// <summary>
    /// 接收者用户名
    /// </summary>
    public string ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToUserId { get; set; }

    /// <summary>
    /// 是否抄送发送者本人（自审计，默认是）
    /// </summary>
    public int IsCc { get; set; } = 1;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件列表 JSON
    /// </summary>
    public string? Attachments { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    [Required(ErrorMessage = "消息类型不能为空")]
    public int MessageType { get; set; } = 1;

    /// <summary>
    /// 消息分组
    /// </summary>
    public int MessageGroup { get; set; } = 1;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 读取状态（0=未读，1=已读）
    /// </summary>
    public int ReadStatus { get; set; } = 0;

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
// 批量创建并推送 DTO
// ========================================

/// <summary>
/// 批量创建在线消息并 SignalR 推送 DTO（全员或指定用户列表）
/// </summary>
public class TaktMessageBatchCreateDto
{
    /// <summary>
    /// 发送者用户名
    /// </summary>
    [Required(ErrorMessage = "发送者用户名不能为空")]
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromUserId { get; set; }

    /// <summary>
    /// 是否向当前公司全部可访问用户落库并推送
    /// </summary>
    public bool SendToAll { get; set; }

    /// <summary>
    /// 指定接收者用户 ID 列表（SendToAll=false 时使用；元素为 string 以兼容前端雪花 ID）
    /// </summary>
    public List<string> ToUserIds { get; set; } = new();

    /// <summary>
    /// 是否抄送发送者本人（自审计，默认是）
    /// </summary>
    public int IsCc { get; set; } = 1;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件列表 JSON
    /// </summary>
    public string? Attachments { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    [Required(ErrorMessage = "消息类型不能为空")]
    public int MessageType { get; set; } = 1;

    /// <summary>
    /// 消息分组
    /// </summary>
    public int MessageGroup { get; set; } = 1;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 读取状态（0=未读，1=已读）
    /// </summary>
    public int ReadStatus { get; set; } = 0;
}

// ========================================
// 已读 / 未读 DTO
// ========================================

/// <summary>
/// 标记在线消息已读 DTO
/// </summary>
public class TaktMessageReadDto
{
    /// <summary>
    /// MessageID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

    /// <summary>
    /// 读取状态（固定为已读）
    /// </summary>
    [Required(ErrorMessage = "读取状态不能为空")]
    public int ReadStatus { get; set; } = 1;

    /// <summary>
    /// 读取时间（为空时服务端写入当前时间）
    /// </summary>
    public DateTime? ReadTime { get; set; }
}

/// <summary>
/// 标记在线消息未读 DTO
/// </summary>
public class TaktMessageUnreadDto
{
    /// <summary>
    /// MessageID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

    /// <summary>
    /// 读取状态（固定为未读）
    /// </summary>
    [Required(ErrorMessage = "读取状态不能为空")]
    public int ReadStatus { get; set; } = 0;

    /// <summary>
    /// 读取时间（标记未读时须清空）
    /// </summary>
    public DateTime? ReadTime { get; set; }
}

// ========================================
// 广播 DTO
// ========================================

/// <summary>
/// 在线消息广播 DTO（SignalR 推送映射）
/// </summary>
public class TaktMessageBroadcastDto
{
    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户名
    /// </summary>
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string? MessageTitle { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型
    /// </summary>
    public int MessageType { get; set; } = 4;

    /// <summary>
    /// 消息分组
    /// </summary>
    public int MessageGroup { get; set; } = 4;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SendTime { get; set; }
}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Message 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMessageExportDto
{
    /// <summary>
    /// MessageID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户名
    /// </summary>
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromUserId { get; set; }

    /// <summary>
    /// 接收者用户名
    /// </summary>
    public string ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToUserId { get; set; }

    /// <summary>
    /// 是否抄送发送者本人（自审计，默认是）
    /// </summary>
    public int IsCc { get; set; } = 1;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件列表 JSON
    /// </summary>
    public string? Attachments { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public int MessageType { get; set; } = 1;

    /// <summary>
    /// 消息分组
    /// </summary>
    public int MessageGroup { get; set; } = 1;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 读取状态（0=未读，1=已读）
    /// </summary>
    public int ReadStatus { get; set; } = 0;

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

// ========================================
// 在线消息统计 DTO
// ========================================

/// <summary>
/// 当前登录用户在线消息统计 DTO
/// </summary>
public class TaktMessageStatisticsDto
{
    /// <summary>
    /// 用户名（接收者，即当前登录用户）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 接收消息总数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 已读消息数
    /// </summary>
    public int ReadCount { get; set; }

    /// <summary>
    /// 未读消息数
    /// </summary>
    public int UnreadCount { get; set; }
}
