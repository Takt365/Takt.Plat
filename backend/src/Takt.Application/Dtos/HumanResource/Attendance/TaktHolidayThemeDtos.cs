// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Attendance
// 文件名称：TaktHolidayThemeDtos.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：服务器当日、指定区域文化下的假日主题响应（公开 API）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Application.Dtos.HumanResource.Attendance;

/// <summary>
/// 服务器当日、用户默认登录公司下的假日主题响应 DTO
/// 对应前端 TaktHolidayThemeDto；业务字段与 TaktHoliday 实体一致，并追加 IsHolidayToday
/// </summary>
public class TaktHolidayThemeDto
{
    /// <summary>
    /// 公司代码（来自 TaktHoliday.CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 假日名称
    /// </summary>
    public string HolidayName { get; set; } = string.Empty;

    /// <summary>
    /// 假日类型（字典 hr_holiday_type）
    /// </summary>
    public TaktHolidayType HolidayType { get; set; } = TaktHolidayType.Statutory;

    /// <summary>
    /// 假日开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 假日结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 是否工作日（字典 hr_holiday_is_working_day）
    /// </summary>
    public TaktHolidayWorkingDay IsWorkingDay { get; set; } = TaktHolidayWorkingDay.NonWorkingDay;

    /// <summary>
    /// 假日问候语（简短，用于界面问候展示）
    /// </summary>
    public string HolidayGreeting { get; set; } = string.Empty;

    /// <summary>
    /// 假日引用/诗句（用于引用区展示）
    /// </summary>
    public string HolidayQuote { get; set; } = string.Empty;

    /// <summary>
    /// 假日主题（对应前端主题色 key，用于日历等非工作日展示）
    /// </summary>
    public string HolidayTheme { get; set; } = string.Empty;

    /// <summary>
    /// 服务器当日是否处于假日区间且为非工作日（用于问候/引用区；无匹配记录时为 false）
    /// </summary>
    public bool IsHolidayToday { get; set; }
}
