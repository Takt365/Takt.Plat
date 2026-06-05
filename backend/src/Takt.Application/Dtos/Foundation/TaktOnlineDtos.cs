// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktOnlineDtos.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Auto Generated)
// 功能描述：Online 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktOnline 生成，请按需审阅）
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
// Online 响应 DTO
// ========================================

/// <summary>
/// 在线用户实体 公司级实体：在线用户按租户+公司双重隔离
/// 对应前端 TaktOnlineDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktOnlineDto : TaktCompanyDtoBase
{
    /// <summary>
    /// OnlineID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OnlineId { get; set; }

    /// <summary>
    /// SignalR 连接 ID（租户+公司内唯一）
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// SignalR 连接 名称（填充字段）
    /// </summary>
    public string? ConnectionName { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 在线状态（0=在线，1=离线，2=离开）
    /// </summary>
    public TaktOnlineStatus OnlineStatus { get; set; } = TaktOnlineStatus.Online;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string? ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string? ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// User-Agent
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 设备类型
    /// </summary>
    public TaktDeviceType? DeviceType { get; set; }

    /// <summary>
    /// 浏览器类型
    /// </summary>
    public TaktBrowserType? BrowserType { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public TaktOperatingSystem? OperatingSystem { get; set; }

    /// <summary>
    /// 连接时间
    /// </summary>
    public DateTime ConnectTime { get; set; }

    /// <summary>
    /// 最后活动时间
    /// </summary>
    public DateTime? LastActiveTime { get; set; }

    /// <summary>
    /// 断开时间
    /// </summary>
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int? ConnectionDuration { get; set; }

}

// ========================================
// Online 查询 DTO
// ========================================

/// <summary>
/// Online 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktOnlineQueryDto : TaktPagedQuery
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
    /// SignalR 连接 ID（租户+公司内唯一）
    /// </summary>
    public string? ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 在线状态（0=在线，1=离线，2=离开）
    /// </summary>
    public TaktOnlineStatus? OnlineStatus { get; set; }

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string? ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string? ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// User-Agent
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 设备类型
    /// </summary>
    public TaktDeviceType? DeviceType { get; set; }

    /// <summary>
    /// 浏览器类型
    /// </summary>
    public TaktBrowserType? BrowserType { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public TaktOperatingSystem? OperatingSystem { get; set; }

    /// <summary>
    /// 连接时间（范围查询-开始）
    /// </summary>
    public DateTime? ConnectTimeStart { get; set; }

    /// <summary>
    /// 连接时间（范围查询-结束）
    /// </summary>
    public DateTime? ConnectTimeEnd { get; set; }

    /// <summary>
    /// 最后活动时间（范围查询-开始）
    /// </summary>
    public DateTime? LastActiveTimeStart { get; set; }

    /// <summary>
    /// 最后活动时间（范围查询-结束）
    /// </summary>
    public DateTime? LastActiveTimeEnd { get; set; }

    /// <summary>
    /// 断开时间（范围查询-开始）
    /// </summary>
    public DateTime? DisconnectTimeStart { get; set; }

    /// <summary>
    /// 断开时间（范围查询-结束）
    /// </summary>
    public DateTime? DisconnectTimeEnd { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int? ConnectionDuration { get; set; }

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
// 创建Online DTO
// ========================================

/// <summary>
/// 创建Online DTO
/// </summary>
public class TaktOnlineCreateDto
{
    /// <summary>
    /// SignalR 连接 ID（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "SignalR 连接 ID（租户+公司内唯一）不能为空")]
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 在线状态（0=在线，1=离线，2=离开）
    /// </summary>
    public TaktOnlineStatus OnlineStatus { get; set; } = TaktOnlineStatus.Online;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string? ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string? ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// User-Agent
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 设备类型
    /// </summary>
    public TaktDeviceType? DeviceType { get; set; }

    /// <summary>
    /// 浏览器类型
    /// </summary>
    public TaktBrowserType? BrowserType { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public TaktOperatingSystem? OperatingSystem { get; set; }

    /// <summary>
    /// 连接时间
    /// </summary>
    public DateTime ConnectTime { get; set; }

    /// <summary>
    /// 最后活动时间
    /// </summary>
    public DateTime? LastActiveTime { get; set; }

    /// <summary>
    /// 断开时间
    /// </summary>
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int? ConnectionDuration { get; set; }

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
// 更新Online DTO
// ========================================

/// <summary>
/// 更新Online DTO
/// 继承 TaktOnlineCreateDto，添加 OnlineId 字段
/// </summary>
public class TaktOnlineUpdateDto : TaktOnlineCreateDto
{
    /// <summary>
    /// OnlineID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OnlineId { get; set; }

}

// ========================================
// Online 状态 DTO
// ========================================

/// <summary>
/// Online 状态更新 DTO
/// </summary>
public class TaktOnlineStatusDto
{
    /// <summary>
    /// OnlineID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OnlineId { get; set; }

    /// <summary>
    /// 在线状态（0=在线，1=离线，2=离开）
    /// </summary>
    [Required(ErrorMessage = "在线状态（0=在线，1=离线，2=离开）不能为空")]
    public TaktOnlineStatus OnlineStatus { get; set; } = TaktOnlineStatus.Online;
}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Online 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktOnlineExportDto
{
    /// <summary>
    /// OnlineID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OnlineId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// SignalR 连接 ID（租户+公司内唯一）
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 在线状态（0=在线，1=离线，2=离开）
    /// </summary>
    public TaktOnlineStatus OnlineStatus { get; set; } = TaktOnlineStatus.Online;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string? ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string? ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// User-Agent
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 设备类型
    /// </summary>
    public TaktDeviceType? DeviceType { get; set; }

    /// <summary>
    /// 浏览器类型
    /// </summary>
    public TaktBrowserType? BrowserType { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public TaktOperatingSystem? OperatingSystem { get; set; }

    /// <summary>
    /// 连接时间
    /// </summary>
    public DateTime ConnectTime { get; set; }

    /// <summary>
    /// 最后活动时间
    /// </summary>
    public DateTime? LastActiveTime { get; set; }

    /// <summary>
    /// 断开时间
    /// </summary>
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int? ConnectionDuration { get; set; }

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

// ========================================
// 在线用户统计 DTO
// ========================================

/// <summary>
/// 当前登录用户在线统计 DTO
/// </summary>
public class TaktOnlineStatisticsDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 当前用户活跃在线连接数（多终端/多标签页分别计数）
    /// </summary>
    public int OnlineCount { get; set; }

    /// <summary>
    /// 当前在线总时长（秒）：当前用户所有活跃会话从连接至今累计
    /// </summary>
    public long CurrentDurationSeconds { get; set; }

    /// <summary>
    /// 当天累计在线时长（秒）：当前用户当日各会话有效时长之和
    /// </summary>
    public long TodayDurationSeconds { get; set; }

    /// <summary>
    /// 当月累计在线时长（秒）：当前用户当月各会话有效时长之和
    /// </summary>
    public long MonthDurationSeconds { get; set; }
}
