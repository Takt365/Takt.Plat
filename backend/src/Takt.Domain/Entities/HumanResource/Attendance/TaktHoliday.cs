// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Attendance
// 文件名称：TaktHoliday.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：假日实体，支撑考勤、排班、薪资等业务（法定/调休/公司假日条目）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Attendance;

/// <summary>
/// 假日实体
/// 假日条目，用于考勤日历、排班与薪资计算；字典 hr_holiday_category、hr_holiday_working_day_type 与字段取值一致
/// 公司级实体：按 TenantCode + CompanyCode 隔离；同一公司内以开始日期+结束日期+假日类型唯一
/// </summary>
[SugarTable("takt_human_resource_attendance_holiday", "假日信息表")]
[SugarIndex("ix_holiday_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_holiday_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_holiday_start_end_type_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StartDate), OrderByType.Asc, nameof(EndDate), OrderByType.Asc, nameof(HolidayType), OrderByType.Asc, true)]
[SugarIndex("ix_holiday_end_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EndDate), OrderByType.Asc, false)]
[SugarIndex("ix_holiday_is_working_day", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsWorkingDay), OrderByType.Asc, false)]
[SugarIndex("ix_holiday_start_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StartDate), OrderByType.Asc, false)]
public class TaktHoliday : TaktCompanyEntityBase
{
    /// <summary>
    /// 假日名称
    /// </summary>
    [SugarColumn(ColumnName = "holiday_name", ColumnDescription = "假日名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string HolidayName { get; set; } = string.Empty;

    /// <summary>
    /// 假日类型（字典 hr_holiday_category）
    /// </summary>
    [SugarColumn(ColumnName = "holiday_type", ColumnDescription = "假日类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int HolidayType { get; set; } = 0;

    /// <summary>
    /// 假日开始日期
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "假日开始日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 假日结束日期
    /// </summary>
    [SugarColumn(ColumnName = "end_date", ColumnDescription = "假日结束日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 是否工作日（字典 hr_holiday_working_day_type）
    /// </summary>
    [SugarColumn(ColumnName = "is_working_day", ColumnDescription = "是否工作日", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsWorkingDay { get; set; } = 0;

    /// <summary>
    /// 假日问候语（简短，用于界面问候展示）
    /// </summary>
    [SugarColumn(ColumnName = "holiday_greeting", ColumnDescription = "假日问候语", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string HolidayGreeting { get; set; } = string.Empty;

    /// <summary>
    /// 假日引用/诗句（用于引用区展示）
    /// </summary>
    [SugarColumn(ColumnName = "holiday_quote", ColumnDescription = "假日引用", ColumnDataType = "nvarchar", Length = 500, IsNullable = false, DefaultValue = "")]
    public string HolidayQuote { get; set; } = string.Empty;

    /// <summary>
    /// 假日主题（对应前端主题色 key，用于日历等非工作日展示）
    /// </summary>
    [SugarColumn(ColumnName = "holiday_theme", ColumnDescription = "假日主题", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = "")]
    public string HolidayTheme { get; set; } = string.Empty;
}
