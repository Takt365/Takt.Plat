// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Talent
// 文件名称：TaktTalentInterviewDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentInterview 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTalentInterview 生成，请按需审阅）
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
// TalentInterview 响应 DTO
// ========================================

/// <summary>
/// 面试安排（业务过程单，非审批单；状态见 interview_status）
/// 对应前端 TaktTalentInterviewDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTalentInterviewDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TalentInterviewID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentInterviewId { get; set; }

    /// <summary>
    /// 职位发布ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long JobPostingId { get; set; }

    /// <summary>
    /// 职位发布名称（填充字段）
    /// </summary>
    public string? JobPostingName { get; set; }

    /// <summary>
    /// 面试单号（租户+公司内业务编号）
    /// </summary>
    public string InterviewNo { get; set; } = string.Empty;

    /// <summary>
    /// 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
    /// </summary>
    public int InterviewStatus { get; set; } = 0;

    /// <summary>
    /// 面试轮次（1=初试，2=复试，3=终试）
    /// </summary>
    public int InterviewRound { get; set; } = 0;

    /// <summary>
    /// 面试时间
    /// </summary>
    public DateTime InterviewDate { get; set; }

    /// <summary>
    /// 面试官姓名
    /// </summary>
    public string? InterviewerName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人姓名
    /// </summary>
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 候选人邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 面试地点
    /// </summary>
    public string? InterviewLocation { get; set; } = string.Empty;

    /// <summary>
    /// 面试说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布
    /// （主表：TaktTalentJobPosting）
    /// </summary>
    public TaktTalentJobPostingDto? JobPosting { get; set; }

    /// <summary>
    /// 录用信息
    /// （子表：TaktTalentOffer）
    /// </summary>
    public List<TaktTalentOfferDto>? TalentOffers { get; set; }

}

// ========================================
// TalentInterview 查询 DTO
// ========================================

/// <summary>
/// TalentInterview 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTalentInterviewQueryDto : TaktPagedQuery
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
    /// 职位发布ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? JobPostingId { get; set; }

    /// <summary>
    /// 面试单号（租户+公司内业务编号）
    /// </summary>
    public string? InterviewNo { get; set; } = string.Empty;

    /// <summary>
    /// 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
    /// </summary>
    public int? InterviewStatus { get; set; }

    /// <summary>
    /// 面试轮次（1=初试，2=复试，3=终试）
    /// </summary>
    public int? InterviewRound { get; set; }

    /// <summary>
    /// 面试时间（范围查询-开始）
    /// </summary>
    public DateTime? InterviewDateStart { get; set; }

    /// <summary>
    /// 面试时间（范围查询-结束）
    /// </summary>
    public DateTime? InterviewDateEnd { get; set; }

    /// <summary>
    /// 面试官姓名
    /// </summary>
    public string? InterviewerName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人姓名
    /// </summary>
    public string? CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 候选人邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 面试地点
    /// </summary>
    public string? InterviewLocation { get; set; } = string.Empty;

    /// <summary>
    /// 面试说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
// 创建TalentInterview DTO
// ========================================

/// <summary>
/// 创建TalentInterview DTO
/// </summary>
public class TaktTalentInterviewCreateDto
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
    /// 职位发布ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long JobPostingId { get; set; }

    /// <summary>
    /// 面试单号（租户+公司内业务编号）
    /// </summary>
    [Required(ErrorMessage = "面试单号（租户+公司内业务编号）不能为空")]
    public string InterviewNo { get; set; } = string.Empty;

    /// <summary>
    /// 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
    /// </summary>
    public int InterviewStatus { get; set; } = 0;

    /// <summary>
    /// 面试轮次（1=初试，2=复试，3=终试）
    /// </summary>
    public int InterviewRound { get; set; } = 0;

    /// <summary>
    /// 面试时间
    /// </summary>
    public DateTime InterviewDate { get; set; }

    /// <summary>
    /// 面试官姓名
    /// </summary>
    public string? InterviewerName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人姓名
    /// </summary>
    [Required(ErrorMessage = "候选人姓名不能为空")]
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 候选人邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 面试地点
    /// </summary>
    public string? InterviewLocation { get; set; } = string.Empty;

    /// <summary>
    /// 面试说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 录用信息（子表，级联保存）
    /// </summary>
    public List<TaktTalentOfferCreateDto>? TalentOffers { get; set; }

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
// 更新TalentInterview DTO
// ========================================

/// <summary>
/// 更新TalentInterview DTO
/// 继承 TaktTalentInterviewCreateDto，添加 TalentInterviewId 字段
/// </summary>
public class TaktTalentInterviewUpdateDto : TaktTalentInterviewCreateDto
{
    /// <summary>
    /// TalentInterviewID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentInterviewId { get; set; }

}

// ========================================
// TalentInterview 状态 DTO
// ========================================

/// <summary>
/// TalentInterview 状态更新 DTO
/// </summary>
public class TaktTalentInterviewStatusDto
{
    /// <summary>
    /// TalentInterviewID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentInterviewId { get; set; }

    /// <summary>
    /// 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
    /// </summary>
    [Required(ErrorMessage = "面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）不能为空")]
    public int InterviewStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TalentInterview 导入模板行 DTO
/// </summary>
public class TaktTalentInterviewTemplateDto
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
    /// 职位发布ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? JobPostingId { get; set; }

    /// <summary>
    /// 面试单号（租户+公司内业务编号）
    /// </summary>
    public string? InterviewNo { get; set; } = string.Empty;

    /// <summary>
    /// 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
    /// </summary>
    public int? InterviewStatus { get; set; }

    /// <summary>
    /// 面试轮次（1=初试，2=复试，3=终试）
    /// </summary>
    public int? InterviewRound { get; set; }

    /// <summary>
    /// 面试官姓名
    /// </summary>
    public string? InterviewerName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人姓名
    /// </summary>
    public string? CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 候选人邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 面试地点
    /// </summary>
    public string? InterviewLocation { get; set; } = string.Empty;

    /// <summary>
    /// 面试说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
/// TalentInterview 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTalentInterviewImportDto
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
    /// 职位发布ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? JobPostingId { get; set; }

    /// <summary>
    /// 面试单号（租户+公司内业务编号）
    /// </summary>
    public string? InterviewNo { get; set; } = string.Empty;

    /// <summary>
    /// 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
    /// </summary>
    public int? InterviewStatus { get; set; }

    /// <summary>
    /// 面试轮次（1=初试，2=复试，3=终试）
    /// </summary>
    public int? InterviewRound { get; set; }

    /// <summary>
    /// 面试官姓名
    /// </summary>
    public string? InterviewerName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人姓名
    /// </summary>
    public string? CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 候选人邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 面试地点
    /// </summary>
    public string? InterviewLocation { get; set; } = string.Empty;

    /// <summary>
    /// 面试说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
/// TalentInterview 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTalentInterviewExportDto
{
    /// <summary>
    /// TalentInterviewID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentInterviewId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long JobPostingId { get; set; }

    /// <summary>
    /// 面试单号（租户+公司内业务编号）
    /// </summary>
    public string InterviewNo { get; set; } = string.Empty;

    /// <summary>
    /// 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
    /// </summary>
    public int InterviewStatus { get; set; } = 0;

    /// <summary>
    /// 面试轮次（1=初试，2=复试，3=终试）
    /// </summary>
    public int InterviewRound { get; set; } = 0;

    /// <summary>
    /// 面试时间
    /// </summary>
    public DateTime InterviewDate { get; set; }

    /// <summary>
    /// 面试官姓名
    /// </summary>
    public string? InterviewerName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人姓名
    /// </summary>
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 候选人邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 面试地点
    /// </summary>
    public string? InterviewLocation { get; set; } = string.Empty;

    /// <summary>
    /// 面试说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

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
