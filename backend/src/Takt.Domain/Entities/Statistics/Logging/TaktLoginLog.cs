// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktLoginLog.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：登录日志实体，记录用户登录行为和结果（统计日志域）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// 登录日志实体
/// </summary>
/// <remarks>
/// 记录用户登录行为和结果（统计日志域）。
/// 数据隔离：租户 + 公司（<see cref="TaktCompanyEntityBase"/>）。
/// </remarks>
[SugarTable("takt_statistics_logging_login_log", "登录日志表")]
[SugarIndex("ix_login_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_login_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_login_log_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_login_log_login_ip", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(LoginIp), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_login_log_login_result", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(LoginResult), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_login_log_username", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(Username), OrderByType.Asc, false)]
public class TaktLoginLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    [SugarColumn(ColumnName = "username", ColumnDescription = "用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 登录方式
    /// </summary>
    [SugarColumn(ColumnName = "login_type", ColumnDescription = "登录方式", ColumnDataType = "int", IsNullable = true)]
    public TaktLoginType? LoginType { get; set; }

    /// <summary>
    /// 浏览器类型
    /// </summary>
    [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器类型", ColumnDataType = "int", IsNullable = true)]
    public TaktBrowserType? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", ColumnDataType = "int", IsNullable = true)]
    public TaktOperatingSystem? Os { get; set; }

    /// <summary>
    /// 用户代理字符串（User-Agent）
    /// </summary>
    [SugarColumn(ColumnName = "user_agent", ColumnDescription = "用户代理字符串", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// 登录结果
    /// </summary>
    [SugarColumn(ColumnName = "login_result", ColumnDescription = "登录结果", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public TaktLoginResult LoginResult { get; set; } = TaktLoginResult.Success;

    /// <summary>
    /// 登录结果消息
    /// </summary>
    [SugarColumn(ColumnName = "login_message", ColumnDescription = "登录结果消息", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? LoginMessage { get; set; }

    /// <summary>
    /// 登录IP地址
    /// </summary>
    [SugarColumn(ColumnName = "login_ip", ColumnDescription = "登录IP地址", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? LoginIp { get; set; }

    /// <summary>
    /// 登录地点（IP解析，如：中国-广东省-深圳市）
    /// </summary>
    [SugarColumn(ColumnName = "login_location", ColumnDescription = "登录地点", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? LoginLocation { get; set; }
    
    /// <summary>
    /// 登出时间
    /// </summary>
    [SugarColumn(ColumnName = "logout_at", ColumnDescription = "登出时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LogoutAt { get; set; }

    /// <summary>
    /// 会话时长（秒，从登录到登出的时长）
    /// </summary>
    [SugarColumn(ColumnName = "session_duration", ColumnDescription = "会话时长", ColumnDataType = "int", IsNullable = true)]
    public int? SessionDuration { get; set; }
}
