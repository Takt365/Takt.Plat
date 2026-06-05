// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktHolidayEnums.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：假日相关枚举（与字典 hr_holiday_type、hr_holiday_is_working_day 取值一致）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 假日类型（对应 TaktHoliday.HolidayType，字典 hr_holiday_type）
/// </summary>
public enum TaktHolidayType
{
    /// <summary>
    /// 法定假日
    /// </summary>
    [Display(Name = "法定")]
    Statutory = 0,

    /// <summary>
    /// 调休
    /// </summary>
    [Display(Name = "调休")]
    Compensatory = 1,

    /// <summary>
    /// 公司假日
    /// </summary>
    [Display(Name = "公司")]
    Company = 2
}

/// <summary>
/// 假日是否工作日（对应 TaktHoliday.IsWorkingDay，字典 hr_holiday_is_working_day）
/// </summary>
public enum TaktHolidayWorkingDay
{
    /// <summary>
    /// 非工作日
    /// </summary>
    [Display(Name = "非工作日")]
    NonWorkingDay = 0,

    /// <summary>
    /// 工作日（如调休补班）
    /// </summary>
    [Display(Name = "工作日")]
    WorkingDay = 1,

    /// <summary>
    /// 半天等
    /// </summary>
    [Display(Name = "半天等")]
    HalfDay = 2
}
