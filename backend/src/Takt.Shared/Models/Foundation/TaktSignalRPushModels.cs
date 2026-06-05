// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Models.Foundation
// 文件名称：TaktSignalRPushModels.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 实时推送模型（私信、广播）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Shared.Models.Foundation;

/// <summary>
/// SignalR 私信推送模型
/// </summary>
public class TaktSignalRPrivateMessagePush
{
    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 消息 ID
    /// </summary>
    public long MessageId { get; set; }

    /// <summary>
    /// 发送者用户名
    /// </summary>
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户 ID
    /// </summary>
    public long? FromUserId { get; set; }

    /// <summary>
    /// 接收者用户名
    /// </summary>
    public string ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户 ID
    /// </summary>
    public long? ToUserId { get; set; }

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
    public TaktMessageType MessageType { get; set; } = TaktMessageType.UserMessage;

    /// <summary>
    /// 消息分组
    /// </summary>
    public TaktMessageGroup? MessageGroup { get; set; }

    /// <summary>
    /// 读取状态
    /// </summary>
    public TaktMessageReadStatus ReadStatus { get; set; }

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 扩展数据 JSON
    /// </summary>
    public string? MessageExtData { get; set; }
}

/// <summary>
/// SignalR 广播推送模型
/// </summary>
public class TaktSignalRBroadcastPush
{
    /// <summary>
    /// 公司编码（广播范围）
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
    public TaktMessageType MessageType { get; set; } = TaktMessageType.SystemNotice;

    /// <summary>
    /// 消息分组
    /// </summary>
    public TaktMessageGroup MessageGroup { get; set; } = TaktMessageGroup.Notification;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SendTime { get; set; }
}
