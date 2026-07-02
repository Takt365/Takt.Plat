// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Talent
// 文件名称：TaktTalentJobPostingDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentJobPosting 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTalentJobPosting 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Talent;

// ========================================
// TalentJobPosting 响应 DTO
// ========================================

/// <summary>
/// 职位发布（业务发布单，非审批单；状态见 posting_status）
/// 对应前端 TaktTalentJobPostingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTalentJobPostingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TalentJobPostingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentJobPostingId { get; set; }

    /// <summary>
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StaffingRequirementId { get; set; }

    /// <summary>
    /// 用人需求名称（填充字段）
    /// </summary>
    public string? StaffingRequirementName { get; set; }

    /// <summary>
    /// 发布编号（租户+公司内唯一）
    /// </summary>
    public string PostingCode { get; set; } = string.Empty;

    /// <summary>
    /// 职位标题
    /// </summary>
    public string TalentJobPostingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布日期
    /// </summary>
    public DateTime PublishDate { get; set; }

    /// <summary>
    /// 招聘开放日期
    /// </summary>
    public DateTime OpenDate { get; set; }

    /// <summary>
    /// 招聘关闭日期
    /// </summary>
    public DateTime? CloseDate { get; set; }

    /// <summary>
    /// 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
    /// </summary>
    public int PublishChannel { get; set; } = 0;

    /// <summary>
    /// 发布说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
    /// </summary>
    public int PostingStatus { get; set; } = 0;

    /// <summary>
    /// 用人需求
    /// （主表：TaktTalentStaffingRequirement）
    /// </summary>
    public TaktTalentStaffingRequirementDto? StaffingRequirement { get; set; }

    /// <summary>
    /// 录用信息
    /// （子表：TaktTalentOffer）
    /// </summary>
    public List<TaktTalentOfferDto>? TalentOffers { get; set; }

}

// ========================================
// TalentJobPosting 查询 DTO
// ========================================

/// <summary>
/// TalentJobPosting 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTalentJobPostingQueryDto : TaktPagedQuery
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
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StaffingRequirementId { get; set; }

    /// <summary>
    /// 发布编号（租户+公司内唯一）
    /// </summary>
    public string? PostingCode { get; set; } = string.Empty;

    /// <summary>
    /// 职位标题
    /// </summary>
    public string? TalentJobPostingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布日期（范围查询-开始）
    /// </summary>
    public DateTime? PublishDateStart { get; set; }

    /// <summary>
    /// 职位发布日期（范围查询-结束）
    /// </summary>
    public DateTime? PublishDateEnd { get; set; }

    /// <summary>
    /// 招聘开放日期（范围查询-开始）
    /// </summary>
    public DateTime? OpenDateStart { get; set; }

    /// <summary>
    /// 招聘开放日期（范围查询-结束）
    /// </summary>
    public DateTime? OpenDateEnd { get; set; }

    /// <summary>
    /// 招聘关闭日期（范围查询-开始）
    /// </summary>
    public DateTime? CloseDateStart { get; set; }

    /// <summary>
    /// 招聘关闭日期（范围查询-结束）
    /// </summary>
    public DateTime? CloseDateEnd { get; set; }

    /// <summary>
    /// 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
    /// </summary>
    public int? PublishChannel { get; set; }

    /// <summary>
    /// 发布说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
    /// </summary>
    public int? PostingStatus { get; set; }

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
// 创建TalentJobPosting DTO
// ========================================

/// <summary>
/// 创建TalentJobPosting DTO
/// </summary>
public class TaktTalentJobPostingCreateDto
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
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StaffingRequirementId { get; set; }

    /// <summary>
    /// 发布编号（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "发布编号（租户+公司内唯一）不能为空")]
    public string PostingCode { get; set; } = string.Empty;

    /// <summary>
    /// 职位标题
    /// </summary>
    [Required(ErrorMessage = "职位标题不能为空")]
    public string TalentJobPostingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布日期
    /// </summary>
    public DateTime PublishDate { get; set; }

    /// <summary>
    /// 招聘开放日期
    /// </summary>
    public DateTime OpenDate { get; set; }

    /// <summary>
    /// 招聘关闭日期
    /// </summary>
    public DateTime? CloseDate { get; set; }

    /// <summary>
    /// 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
    /// </summary>
    public int PublishChannel { get; set; } = 0;

    /// <summary>
    /// 发布说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
    /// </summary>
    public int PostingStatus { get; set; } = 0;

    /// <summary>
    /// 录用信息（子表，级联保存）
    /// </summary>
    public List<TaktTalentOfferCreateDto>? TalentOffers { get; set; }

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
// 更新TalentJobPosting DTO
// ========================================

/// <summary>
/// 更新TalentJobPosting DTO
/// 继承 TaktTalentJobPostingCreateDto，添加 TalentJobPostingId 字段
/// </summary>
public class TaktTalentJobPostingUpdateDto : TaktTalentJobPostingCreateDto
{
    /// <summary>
    /// TalentJobPostingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentJobPostingId { get; set; }

}

// ========================================
// TalentJobPosting 状态 DTO
// ========================================

/// <summary>
/// TalentJobPosting 状态更新 DTO
/// </summary>
public class TaktTalentJobPostingStatusDto
{
    /// <summary>
    /// TalentJobPostingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentJobPostingId { get; set; }

    /// <summary>
    /// 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
    /// </summary>
    [Required(ErrorMessage = "发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）不能为空")]
    public int PostingStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TalentJobPosting 导入模板行 DTO
/// </summary>
public class TaktTalentJobPostingTemplateDto
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
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StaffingRequirementId { get; set; }

    /// <summary>
    /// 发布编号（租户+公司内唯一）
    /// </summary>
    public string? PostingCode { get; set; } = string.Empty;

    /// <summary>
    /// 职位标题
    /// </summary>
    public string? TalentJobPostingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布日期
    /// </summary>
    public DateTime? PublishDate { get; set; }

    /// <summary>
    /// 招聘开放日期
    /// </summary>
    public DateTime? OpenDate { get; set; }

    /// <summary>
    /// 招聘关闭日期
    /// </summary>
    public DateTime? CloseDate { get; set; }

    /// <summary>
    /// 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
    /// </summary>
    public int? PublishChannel { get; set; }

    /// <summary>
    /// 发布说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
    /// </summary>
    public int? PostingStatus { get; set; }

    /// <summary>
    /// 录用信息（子表，级联保存）
    /// </summary>
    public List<TaktTalentOfferCreateDto>? TalentOffers { get; set; }

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
/// TalentJobPosting 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTalentJobPostingImportDto
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
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StaffingRequirementId { get; set; }

    /// <summary>
    /// 发布编号（租户+公司内唯一）
    /// </summary>
    public string? PostingCode { get; set; } = string.Empty;

    /// <summary>
    /// 职位标题
    /// </summary>
    public string? TalentJobPostingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布日期
    /// </summary>
    public DateTime? PublishDate { get; set; }

    /// <summary>
    /// 招聘开放日期
    /// </summary>
    public DateTime? OpenDate { get; set; }

    /// <summary>
    /// 招聘关闭日期
    /// </summary>
    public DateTime? CloseDate { get; set; }

    /// <summary>
    /// 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
    /// </summary>
    public int? PublishChannel { get; set; }

    /// <summary>
    /// 发布说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
    /// </summary>
    public int? PostingStatus { get; set; }

    /// <summary>
    /// 录用信息（子表，级联保存）
    /// </summary>
    public List<TaktTalentOfferCreateDto>? TalentOffers { get; set; }

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
/// TalentJobPosting 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTalentJobPostingExportDto
{
    /// <summary>
    /// TalentJobPostingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentJobPostingId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 用人需求ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StaffingRequirementId { get; set; }

    /// <summary>
    /// 发布编号（租户+公司内唯一）
    /// </summary>
    public string PostingCode { get; set; } = string.Empty;

    /// <summary>
    /// 职位标题
    /// </summary>
    public string TalentJobPostingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布日期
    /// </summary>
    public DateTime PublishDate { get; set; }

    /// <summary>
    /// 招聘开放日期
    /// </summary>
    public DateTime OpenDate { get; set; }

    /// <summary>
    /// 招聘关闭日期
    /// </summary>
    public DateTime? CloseDate { get; set; }

    /// <summary>
    /// 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
    /// </summary>
    public int PublishChannel { get; set; } = 0;

    /// <summary>
    /// 发布说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
    /// </summary>
    public int PostingStatus { get; set; } = 0;

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
