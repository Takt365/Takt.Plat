// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktFoundationEnums.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Foundation 域在线用户与在线消息相关枚举
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 在线用户状态
/// </summary>
public enum TaktOnlineStatus
{
    /// <summary>
    /// 在线
    /// </summary>
    [Display(Name = "在线")]
    Online = 0,

    /// <summary>
    /// 离线
    /// </summary>
    [Display(Name = "离线")]
    Offline = 1,

    /// <summary>
    /// 离开（含强退）
    /// </summary>
    [Display(Name = "离开")]
    Away = 2,
}

/// <summary>
/// 客户端设备类型
/// </summary>
public enum TaktDeviceType
{
    /// <summary>
    /// 未知
    /// </summary>
    [Display(Name = "未知")]
    Unknown = 0,

    /// <summary>
    /// PC
    /// </summary>
    [Display(Name = "PC")]
    Pc = 1,

    /// <summary>
    /// 手机
    /// </summary>
    [Display(Name = "手机")]
    Mobile = 2,

    /// <summary>
    /// 平板
    /// </summary>
    [Display(Name = "平板")]
    Tablet = 3,
}

/// <summary>
/// 浏览器类型
/// </summary>
public enum TaktBrowserType
{
    /// <summary>
    /// 未知
    /// </summary>
    [Display(Name = "未知")]
    Unknown = 0,

    /// <summary>
    /// Chrome
    /// </summary>
    [Display(Name = "Chrome")]
    Chrome = 1,

    /// <summary>
    /// Firefox
    /// </summary>
    [Display(Name = "Firefox")]
    Firefox = 2,

    /// <summary>
    /// Safari
    /// </summary>
    [Display(Name = "Safari")]
    Safari = 3,

    /// <summary>
    /// Edge
    /// </summary>
    [Display(Name = "Edge")]
    Edge = 4,
}

/// <summary>
/// 操作系统类型
/// </summary>
public enum TaktOperatingSystem
{
    /// <summary>
    /// 未知
    /// </summary>
    [Display(Name = "未知")]
    Unknown = 0,

    /// <summary>
    /// Windows
    /// </summary>
    [Display(Name = "Windows")]
    Windows = 1,

    /// <summary>
    /// macOS
    /// </summary>
    [Display(Name = "macOS")]
    MacOS = 2,

    /// <summary>
    /// Linux
    /// </summary>
    [Display(Name = "Linux")]
    Linux = 3,

    /// <summary>
    /// Android
    /// </summary>
    [Display(Name = "Android")]
    Android = 4,

    /// <summary>
    /// iOS
    /// </summary>
    [Display(Name = "iOS")]
    IOS = 5,
}

/// <summary>
/// 在线消息读取状态
/// </summary>
public enum TaktMessageReadStatus
{
    /// <summary>
    /// 未读
    /// </summary>
    [Display(Name = "未读")]
    Unread = 0,

    /// <summary>
    /// 已读
    /// </summary>
    [Display(Name = "已读")]
    Read = 1,
}

/// <summary>
/// 在线消息类型
/// </summary>
public enum TaktMessageType
{
    /// <summary>
    /// 系统通知（广播）
    /// </summary>
    [Display(Name = "系统通知")]
    SystemNotice = 1,

    /// <summary>
    /// 用户私信
    /// </summary>
    [Display(Name = "用户私信")]
    UserMessage = 2,

    /// <summary>
    /// 流程审批通知
    /// </summary>
    [Display(Name = "流程审批通知")]
    ApprovalNotify = 4,

    /// <summary>
    /// 强制下线
    /// </summary>
    [Display(Name = "强制下线")]
    ForceLogout = 5,

    /// <summary>
    /// 心跳 / 在线状态
    /// </summary>
    [Display(Name = "心跳")]
    Heartbeat = 6,
}

/// <summary>
/// 在线消息分组
/// </summary>
public enum TaktMessageGroup
{
    /// <summary>
    /// 聊天
    /// </summary>
    [Display(Name = "聊天")]
    Chat = 1,

    /// <summary>
    /// 通知
    /// </summary>
    [Display(Name = "通知")]
    Notification = 2,
}
