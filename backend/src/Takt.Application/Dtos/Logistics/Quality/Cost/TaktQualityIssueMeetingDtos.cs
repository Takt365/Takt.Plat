// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueMeetingDtos.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityIssueMeeting 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityIssueMeeting 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Cost;

// ========================================
// QualityIssueMeeting 响应 DTO
// ========================================

/// <summary>
/// 品质问题应对明细 - 会议/调查/试验费用
/// 对应前端 TaktQualityIssueMeetingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityIssueMeetingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityIssueMeetingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueMeetingId { get; set; }

    /// <summary>
    /// 品质问题主表 ID（关联 TaktQualityIssue.Id，选项 TaktQualityIssues/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题主表 名称（填充字段）
    /// </summary>
    public string? QualityIssueName { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 直接人员费率（元/分钟）
    /// </summary>
    public decimal DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 间接人员费率（元/分钟）
    /// </summary>
    public decimal IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 讨论调查试验内容(会议记录)
    /// </summary>
    public string? MeetingInvestigationContent { get; set; } = string.Empty;

    /// <summary>
    /// 讨论调查试验费用(元)
    /// </summary>
    public decimal MeetingInvestigationCost { get; set; }

    /// <summary>
    /// 讨论会使用时间(分钟)
    /// </summary>
    public int MeetingTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 直接人员参加人数
    /// </summary>
    public int DirectParticipantCount { get; set; } = 0;

    /// <summary>
    /// 间接人员参加人数
    /// </summary>
    public int IndirectParticipantCount { get; set; } = 0;

    /// <summary>
    /// 调查评价试验工作时间（分钟）
    /// </summary>
    public int InvestigationWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 交通费、旅费（元）
    /// </summary>
    public decimal TravelCost { get; set; }

    /// <summary>
    /// 其他费用（元）
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 其他作业時間（分钟）
    /// </summary>
    public int OtherWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 其他设备购入费、工程费、搬运费等（元）
    /// </summary>
    public decimal OtherApparatusCost { get; set; }

    /// <summary>
    /// 品质问题対応记录者（会议调查试验记录者）
    /// </summary>
    public string? MeetingRecorder { get; set; } = string.Empty;

    /// <summary>
    /// 质量问题主表（导航属性）
    /// （主表：TaktQualityIssue）
    /// </summary>
    public TaktQualityIssueDto? Issue { get; set; }

}

// ========================================
// QualityIssueMeeting 查询 DTO
// ========================================

/// <summary>
/// QualityIssueMeeting 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityIssueMeetingQueryDto : TaktPagedQuery
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
    /// 品质问题主表 ID（关联 TaktQualityIssue.Id，选项 TaktQualityIssues/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 直接人员费率（元/分钟）
    /// </summary>
    public decimal? DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 间接人员费率（元/分钟）
    /// </summary>
    public decimal? IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 讨论调查试验内容(会议记录)
    /// </summary>
    public string? MeetingInvestigationContent { get; set; } = string.Empty;

    /// <summary>
    /// 讨论调查试验费用(元)
    /// </summary>
    public decimal? MeetingInvestigationCost { get; set; }

    /// <summary>
    /// 讨论会使用时间(分钟)
    /// </summary>
    public int? MeetingTimeMinutes { get; set; }

    /// <summary>
    /// 直接人员参加人数
    /// </summary>
    public int? DirectParticipantCount { get; set; }

    /// <summary>
    /// 间接人员参加人数
    /// </summary>
    public int? IndirectParticipantCount { get; set; }

    /// <summary>
    /// 调查评价试验工作时间（分钟）
    /// </summary>
    public int? InvestigationWorkTimeMinutes { get; set; }

    /// <summary>
    /// 交通费、旅费（元）
    /// </summary>
    public decimal? TravelCost { get; set; }

    /// <summary>
    /// 其他费用（元）
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 其他作业時間（分钟）
    /// </summary>
    public int? OtherWorkTimeMinutes { get; set; }

    /// <summary>
    /// 其他设备购入费、工程费、搬运费等（元）
    /// </summary>
    public decimal? OtherApparatusCost { get; set; }

    /// <summary>
    /// 品质问题対応记录者（会议调查试验记录者）
    /// </summary>
    public string? MeetingRecorder { get; set; } = string.Empty;

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
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建QualityIssueMeeting DTO
// ========================================

/// <summary>
/// 创建QualityIssueMeeting DTO
/// </summary>
public class TaktQualityIssueMeetingCreateDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题主表 ID（关联 TaktQualityIssue.Id，选项 TaktQualityIssues/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "品质问题编码（冗余字段，便于查询）不能为空")]
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 直接人员费率（元/分钟）
    /// </summary>
    public decimal DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 间接人员费率（元/分钟）
    /// </summary>
    public decimal IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 讨论调查试验内容(会议记录)
    /// </summary>
    public string? MeetingInvestigationContent { get; set; } = string.Empty;

    /// <summary>
    /// 讨论调查试验费用(元)
    /// </summary>
    public decimal MeetingInvestigationCost { get; set; }

    /// <summary>
    /// 讨论会使用时间(分钟)
    /// </summary>
    public int MeetingTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 直接人员参加人数
    /// </summary>
    public int DirectParticipantCount { get; set; } = 0;

    /// <summary>
    /// 间接人员参加人数
    /// </summary>
    public int IndirectParticipantCount { get; set; } = 0;

    /// <summary>
    /// 调查评价试验工作时间（分钟）
    /// </summary>
    public int InvestigationWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 交通费、旅费（元）
    /// </summary>
    public decimal TravelCost { get; set; }

    /// <summary>
    /// 其他费用（元）
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 其他作业時間（分钟）
    /// </summary>
    public int OtherWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 其他设备购入费、工程费、搬运费等（元）
    /// </summary>
    public decimal OtherApparatusCost { get; set; }

    /// <summary>
    /// 品质问题対応记录者（会议调查试验记录者）
    /// </summary>
    public string? MeetingRecorder { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新QualityIssueMeeting DTO
// ========================================

/// <summary>
/// 更新QualityIssueMeeting DTO
/// 继承 TaktQualityIssueMeetingCreateDto，添加 QualityIssueMeetingId 字段
/// </summary>
public class TaktQualityIssueMeetingUpdateDto : TaktQualityIssueMeetingCreateDto
{
    /// <summary>
    /// QualityIssueMeetingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueMeetingId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityIssueMeeting 导入模板行 DTO
/// </summary>
public class TaktQualityIssueMeetingTemplateDto
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
    /// 品质问题主表 ID（关联 TaktQualityIssue.Id，选项 TaktQualityIssues/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 直接人员费率（元/分钟）
    /// </summary>
    public decimal? DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 间接人员费率（元/分钟）
    /// </summary>
    public decimal? IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 讨论调查试验内容(会议记录)
    /// </summary>
    public string? MeetingInvestigationContent { get; set; } = string.Empty;

    /// <summary>
    /// 讨论调查试验费用(元)
    /// </summary>
    public decimal? MeetingInvestigationCost { get; set; }

    /// <summary>
    /// 讨论会使用时间(分钟)
    /// </summary>
    public int? MeetingTimeMinutes { get; set; }

    /// <summary>
    /// 直接人员参加人数
    /// </summary>
    public int? DirectParticipantCount { get; set; }

    /// <summary>
    /// 间接人员参加人数
    /// </summary>
    public int? IndirectParticipantCount { get; set; }

    /// <summary>
    /// 调查评价试验工作时间（分钟）
    /// </summary>
    public int? InvestigationWorkTimeMinutes { get; set; }

    /// <summary>
    /// 交通费、旅费（元）
    /// </summary>
    public decimal? TravelCost { get; set; }

    /// <summary>
    /// 其他费用（元）
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 其他作业時間（分钟）
    /// </summary>
    public int? OtherWorkTimeMinutes { get; set; }

    /// <summary>
    /// 其他设备购入费、工程费、搬运费等（元）
    /// </summary>
    public decimal? OtherApparatusCost { get; set; }

    /// <summary>
    /// 品质问题対応记录者（会议调查试验记录者）
    /// </summary>
    public string? MeetingRecorder { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// QualityIssueMeeting 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityIssueMeetingImportDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题主表 ID（关联 TaktQualityIssue.Id，选项 TaktQualityIssues/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 直接人员费率（元/分钟）
    /// </summary>
    public decimal? DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 间接人员费率（元/分钟）
    /// </summary>
    public decimal? IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 讨论调查试验内容(会议记录)
    /// </summary>
    public string? MeetingInvestigationContent { get; set; } = string.Empty;

    /// <summary>
    /// 讨论调查试验费用(元)
    /// </summary>
    public decimal? MeetingInvestigationCost { get; set; }

    /// <summary>
    /// 讨论会使用时间(分钟)
    /// </summary>
    public int? MeetingTimeMinutes { get; set; }

    /// <summary>
    /// 直接人员参加人数
    /// </summary>
    public int? DirectParticipantCount { get; set; }

    /// <summary>
    /// 间接人员参加人数
    /// </summary>
    public int? IndirectParticipantCount { get; set; }

    /// <summary>
    /// 调查评价试验工作时间（分钟）
    /// </summary>
    public int? InvestigationWorkTimeMinutes { get; set; }

    /// <summary>
    /// 交通费、旅费（元）
    /// </summary>
    public decimal? TravelCost { get; set; }

    /// <summary>
    /// 其他费用（元）
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 其他作业時間（分钟）
    /// </summary>
    public int? OtherWorkTimeMinutes { get; set; }

    /// <summary>
    /// 其他设备购入费、工程费、搬运费等（元）
    /// </summary>
    public decimal? OtherApparatusCost { get; set; }

    /// <summary>
    /// 品质问题対応记录者（会议调查试验记录者）
    /// </summary>
    public string? MeetingRecorder { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// QualityIssueMeeting 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityIssueMeetingExportDto
{
    /// <summary>
    /// QualityIssueMeetingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueMeetingId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题主表 ID（关联 TaktQualityIssue.Id，选项 TaktQualityIssues/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 直接人员费率（元/分钟）
    /// </summary>
    public decimal DirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 间接人员费率（元/分钟）
    /// </summary>
    public decimal IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 讨论调查试验内容(会议记录)
    /// </summary>
    public string? MeetingInvestigationContent { get; set; } = string.Empty;

    /// <summary>
    /// 讨论调查试验费用(元)
    /// </summary>
    public decimal MeetingInvestigationCost { get; set; }

    /// <summary>
    /// 讨论会使用时间(分钟)
    /// </summary>
    public int MeetingTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 直接人员参加人数
    /// </summary>
    public int DirectParticipantCount { get; set; } = 0;

    /// <summary>
    /// 间接人员参加人数
    /// </summary>
    public int IndirectParticipantCount { get; set; } = 0;

    /// <summary>
    /// 调查评价试验工作时间（分钟）
    /// </summary>
    public int InvestigationWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 交通费、旅费（元）
    /// </summary>
    public decimal TravelCost { get; set; }

    /// <summary>
    /// 其他费用（元）
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 其他作业時間（分钟）
    /// </summary>
    public int OtherWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 其他设备购入费、工程费、搬运费等（元）
    /// </summary>
    public decimal OtherApparatusCost { get; set; }

    /// <summary>
    /// 品质问题対応记录者（会议调查试验记录者）
    /// </summary>
    public string? MeetingRecorder { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
