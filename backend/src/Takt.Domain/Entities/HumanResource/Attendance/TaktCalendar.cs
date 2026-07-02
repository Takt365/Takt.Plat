// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Attendance
// 文件名称：TaktCalendar.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂日历实体，按公司/工厂维度定义工作日、休息日与调休
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Attendance;

/// <summary>
/// 工厂日历（公司级；按 RelatedPlant 区分工厂维度）
/// </summary>
[SugarTable("takt_human_resource_attendance_calendar", "工厂日历表")]
[SugarIndex("ix_calendar_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_calendar_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_calendar_plant_date_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RelatedPlant), OrderByType.Asc, nameof(CalendarDate), OrderByType.Asc, true)]
[SugarIndex("ix_calendar_calendar_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CalendarDate), OrderByType.Asc, false)]
[SugarIndex("ix_calendar_is_working_day", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsWorkingDay), OrderByType.Asc, false)]
public class TaktCalendar : TaktCompanyEntityBase
{
    /// <summary>
    /// 日历日期
    /// </summary>
    [SugarColumn(ColumnName = "calendar_date", ColumnDescription = "日历日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime CalendarDate { get; set; }
    /// <summary>
    /// 是否工作日（字典 hr_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
    /// </summary>
    [SugarColumn(ColumnName = "is_working_day", ColumnDescription = "是否工作日", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsWorkingDay { get; set; }
    /// <summary>
    /// 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
    /// </summary>
    [SugarColumn(ColumnName = "holiday_id", ColumnDescription = "关联假日ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }
    /// <summary>
    /// 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [SugarColumn(ColumnName = "shift_id", ColumnDescription = "关联班次ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }
    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string RelatedPlant { get; set; } = string.Empty;
}
