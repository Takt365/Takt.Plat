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

using Newtonsoft.Json;

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
    /// 消息 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
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
    public long? FromUserId { get; set; }

    /// <summary>
    /// 发送者昵称（由用户表 Nickname 解析）
    /// </summary>
    public string? FromUserNickname { get; set; }

    /// <summary>
    /// 接收者用户名
    /// </summary>
    public string ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
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
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; }

    /// <summary>
    /// 消息类型（字典 sys_message_type DictValue）
    /// </summary>
    public string MessageType { get; set; } = "text";

    /// <summary>
    /// 消息分组（字典 sys_message_group_category DictValue）
    /// </summary>
    public string MessageGroup { get; set; } = "message";

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 读取状态
    /// </summary>
    public int ReadStatus { get; set; }
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
    /// 消息类型（字典 sys_message_type DictValue）
    /// </summary>
    public string MessageType { get; set; } = "system";

    /// <summary>
    /// 消息分组（字典 sys_message_group_category DictValue）
    /// </summary>
    public string MessageGroup { get; set; } = "message";

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SendTime { get; set; }
}
