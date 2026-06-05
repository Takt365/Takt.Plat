// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktOnline.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线用户实体，用于通过 SignalR 管理在线用户
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 在线用户实体
/// 公司级实体：在线用户按租户+公司双重隔离
/// </summary>
[SugarTable("takt_foundation_online", "在线用户表")]
[SugarIndex("ix_online_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_online_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_online_connection_id_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConnectionId), OrderByType.Asc, true)]
[SugarIndex("ix_online_connect_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConnectTime), OrderByType.Desc, false)]
[SugarIndex("ix_online_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OnlineStatus), OrderByType.Asc, false)]
[SugarIndex("ix_online_user_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, false)]
[SugarIndex("ix_online_user_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserName), OrderByType.Asc, false)]
public class TaktOnline : TaktCompanyEntityBase
{
    /// <summary>
    /// SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）
    /// </summary>
    [SugarColumn(ColumnName = "connection_id", ColumnDescription = "SignalR连接ID", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户名
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 在线状态
    /// </summary>
    [SugarColumn(ColumnName = "online_status", ColumnDescription = "在线状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktOnlineStatus OnlineStatus { get; set; } = TaktOnlineStatus.Online;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    [SugarColumn(ColumnName = "connect_ip", ColumnDescription = "连接IP地址", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ConnectIp { get; set; }

    /// <summary>
    /// 连接地点
    /// </summary>
    [SugarColumn(ColumnName = "connect_location", ColumnDescription = "连接地点", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ConnectLocation { get; set; }

    /// <summary>
    /// User-Agent
    /// </summary>
    [SugarColumn(ColumnName = "user_agent", ColumnDescription = "User-Agent", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// 设备类型
    /// </summary>
    [SugarColumn(ColumnName = "device_type", ColumnDescription = "设备类型", ColumnDataType = "int", IsNullable = true)]
    public TaktDeviceType? DeviceType { get; set; }

    /// <summary>
    /// 浏览器类型
    /// </summary>
    [SugarColumn(ColumnName = "browser_type", ColumnDescription = "浏览器类型", ColumnDataType = "int", IsNullable = true)]
    public TaktBrowserType? BrowserType { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [SugarColumn(ColumnName = "operating_system", ColumnDescription = "操作系统", ColumnDataType = "int", IsNullable = true)]
    public TaktOperatingSystem? OperatingSystem { get; set; }

    /// <summary>
    /// 连接时间
    /// </summary>
    [SugarColumn(ColumnName = "connect_time", ColumnDescription = "连接时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ConnectTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 最后活动时间
    /// </summary>
    [SugarColumn(ColumnName = "last_active_time", ColumnDescription = "最后活动时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LastActiveTime { get; set; }

    /// <summary>
    /// 断开时间
    /// </summary>
    [SugarColumn(ColumnName = "disconnect_time", ColumnDescription = "断开时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    [SugarColumn(ColumnName = "connection_duration", ColumnDescription = "连接时长", ColumnDataType = "int", IsNullable = true)]
    public int? ConnectionDuration { get; set; }
}
