// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Attendance
// 文件名称：TaktCalendarDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：Calendar 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCalendar 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Attendance;

// ========================================
// Calendar 响应 DTO
// ========================================

/// <summary>
/// 工厂日历（公司级；RelatedPlant 为空表示公司通用，有值表示工厂专属）
/// 对应前端 TaktCalendarDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCalendarDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CalendarID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CalendarId { get; set; }

    /// <summary>
    /// 日历日期
    /// </summary>
    public DateTime CalendarDate { get; set; }

    /// <summary>
    /// 是否工作日（0=非工作日 1=工作日 2=调休工作日等）
    /// </summary>
    public int IsWorkingDay { get; set; } = 0;

    /// <summary>
    /// 关联假日 ID（<see cref="TaktHoliday"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联假日 名称（填充字段）
    /// </summary>
    public string? HolidayName { get; set; }

    /// <summary>
    /// 关联班次 ID（<see cref="TaktWorkShift"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联班次 名称（填充字段）
    /// </summary>
    public string? ShiftName { get; set; }

    /// <summary>
    /// 关联工厂（为空表示公司级通用日历）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

}

// ========================================
// Calendar 查询 DTO
// ========================================

/// <summary>
/// Calendar 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCalendarQueryDto : TaktPagedQuery
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
    /// 日历日期（范围查询-开始）
    /// </summary>
    public DateTime? CalendarDateStart { get; set; }

    /// <summary>
    /// 日历日期（范围查询-结束）
    /// </summary>
    public DateTime? CalendarDateEnd { get; set; }

    /// <summary>
    /// 是否工作日（0=非工作日 1=工作日 2=调休工作日等）
    /// </summary>
    public int? IsWorkingDay { get; set; }

    /// <summary>
    /// 关联假日 ID（<see cref="TaktHoliday"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次 ID（<see cref="TaktWorkShift"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（为空表示公司级通用日历）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
// 创建Calendar DTO
// ========================================

/// <summary>
/// 创建Calendar DTO
/// </summary>
public class TaktCalendarCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 日历日期
    /// </summary>
    public DateTime CalendarDate { get; set; }

    /// <summary>
    /// 是否工作日（0=非工作日 1=工作日 2=调休工作日等）
    /// </summary>
    public int IsWorkingDay { get; set; } = 0;

    /// <summary>
    /// 关联假日 ID（<see cref="TaktHoliday"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次 ID（<see cref="TaktWorkShift"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（为空表示公司级通用日历）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
// 更新Calendar DTO
// ========================================

/// <summary>
/// 更新Calendar DTO
/// 继承 TaktCalendarCreateDto，添加 CalendarId 字段
/// </summary>
public class TaktCalendarUpdateDto : TaktCalendarCreateDto
{
    /// <summary>
    /// CalendarID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CalendarId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Calendar 导入模板行 DTO
/// </summary>
public class TaktCalendarTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否工作日（0=非工作日 1=工作日 2=调休工作日等）
    /// </summary>
    public int? IsWorkingDay { get; set; }

    /// <summary>
    /// 关联假日 ID（<see cref="TaktHoliday"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次 ID（<see cref="TaktWorkShift"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（为空表示公司级通用日历）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Calendar 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCalendarImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 是否工作日（0=非工作日 1=工作日 2=调休工作日等）
    /// </summary>
    public int? IsWorkingDay { get; set; }

    /// <summary>
    /// 关联假日 ID（<see cref="TaktHoliday"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次 ID（<see cref="TaktWorkShift"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（为空表示公司级通用日历）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
// 导出 DTO
// ========================================

/// <summary>
/// Calendar 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCalendarExportDto
{
    /// <summary>
    /// CalendarID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CalendarId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 日历日期
    /// </summary>
    public DateTime CalendarDate { get; set; }

    /// <summary>
    /// 是否工作日（0=非工作日 1=工作日 2=调休工作日等）
    /// </summary>
    public int IsWorkingDay { get; set; } = 0;

    /// <summary>
    /// 关联假日 ID（<see cref="TaktHoliday"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次 ID（<see cref="TaktWorkShift"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（为空表示公司级通用日历）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

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
