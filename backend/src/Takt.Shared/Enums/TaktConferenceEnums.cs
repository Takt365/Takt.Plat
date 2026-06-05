// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktConferenceEnums.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：会议中心相关枚举
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 会议类型枚举
/// </summary>
public enum TaktConferenceType
{
    /// <summary>
    /// 内部会议
    /// </summary>
    [Display(Name = "内部会议")]
    Internal = 0,
    /// <summary>
    /// 外部会议
    /// </summary>
    [Display(Name = "外部会议")]
    External = 1,
    /// <summary>
    /// 视频会议
    /// </summary>
    [Display(Name = "视频会议")]
    Video = 2,
    /// <summary>
    /// 混合会议
    /// </summary>
    [Display(Name = "混合会议")]
    Hybrid = 3
}

/// <summary>
/// 会议状态枚举
/// </summary>
public enum TaktConferenceStatus
{
    /// <summary>
    /// 草稿
    /// </summary>
    [Display(Name = "草稿")]
    Draft = 0,
    /// <summary>
    /// 已排期
    /// </summary>
    [Display(Name = "已排期")]
    Scheduled = 1,
    /// <summary>
    /// 进行中
    /// </summary>
    [Display(Name = "进行中")]
    InProgress = 2,
    /// <summary>
    /// 已结束
    /// </summary>
    [Display(Name = "已结束")]
    Completed = 3,
    /// <summary>
    /// 已取消
    /// </summary>
    [Display(Name = "已取消")]
    Cancelled = 4
}

/// <summary>
/// 会议参与角色枚举
/// </summary>
public enum TaktConferenceParticipantRole
{
    /// <summary>
    /// 参会人
    /// </summary>
    [Display(Name = "参会人")]
    Participant = 0,
    /// <summary>
    /// 主持人
    /// </summary>
    [Display(Name = "主持人")]
    Host = 1,
    /// <summary>
    /// 记录人
    /// </summary>
    [Display(Name = "记录人")]
    Recorder = 2,
    /// <summary>
    /// 嘉宾
    /// </summary>
    [Display(Name = "嘉宾")]
    Guest = 3
}

/// <summary>
/// 会议出席状态枚举
/// </summary>
public enum TaktConferenceAttendanceStatus
{
    /// <summary>
    /// 待确认
    /// </summary>
    [Display(Name = "待确认")]
    Pending = 0,
    /// <summary>
    /// 已接受
    /// </summary>
    [Display(Name = "已接受")]
    Accepted = 1,
    /// <summary>
    /// 已拒绝
    /// </summary>
    [Display(Name = "已拒绝")]
    Declined = 2,
    /// <summary>
    /// 已签到
    /// </summary>
    [Display(Name = "已签到")]
    CheckedIn = 3,
    /// <summary>
    /// 缺席
    /// </summary>
    [Display(Name = "缺席")]
    Absent = 4
}
