// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Talent
// 文件名称：TaktTalentInterview.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：面试安排（人才链路第4步：基于职位发布安排候选人面试）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Talent;

/// <summary>
/// 面试安排（业务过程单，非审批单；状态见 interview_status）
/// </summary>
[SugarTable("takt_human_resource_talent_interview", "面试安排表")]
[SugarIndex("ix_talent_interview_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_talent_interview_job_posting", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(JobPostingId), OrderByType.Asc, false)]
public class TaktTalentInterview : TaktCompanyEntityBase
{
    /// <summary>
    /// 职位发布ID
    /// </summary>
    [SugarColumn(ColumnName = "job_posting_id", ColumnDescription = "职位发布ID", ColumnDataType = "bigint", IsNullable = false)]
    public long JobPostingId { get; set; }

    /// <summary>
    /// 面试单号（租户+公司内业务编号）
    /// </summary>
    [SugarColumn(ColumnName = "interview_no", ColumnDescription = "面试单号", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string InterviewNo { get; set; } = string.Empty;

    /// <summary>
    /// 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "interview_status", ColumnDescription = "面试办理状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InterviewStatus { get; set; }

    /// <summary>
    /// 面试轮次（1=初试，2=复试，3=终试）
    /// </summary>
    [SugarColumn(ColumnName = "interview_round", ColumnDescription = "面试轮次", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int InterviewRound { get; set; } = 1;

    /// <summary>
    /// 面试时间
    /// </summary>
    [SugarColumn(ColumnName = "interview_date", ColumnDescription = "面试时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime InterviewDate { get; set; }

    /// <summary>
    /// 面试官姓名
    /// </summary>
    [SugarColumn(ColumnName = "interviewer_name", ColumnDescription = "面试官姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? InterviewerName { get; set; }

    /// <summary>
    /// 候选人姓名
    /// </summary>
    [SugarColumn(ColumnName = "candidate_name", ColumnDescription = "候选人姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机
    /// </summary>
    [SugarColumn(ColumnName = "mobile", ColumnDescription = "候选人手机", ColumnDataType = "varchar", Length = 11, IsNullable = true)]
    public string? Mobile { get; set; }

    /// <summary>
    /// 候选人邮箱
    /// </summary>
    [SugarColumn(ColumnName = "email", ColumnDescription = "候选人邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? Email { get; set; }

    /// <summary>
    /// 面试地点
    /// </summary>
    [SugarColumn(ColumnName = "interview_location", ColumnDescription = "面试地点", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? InterviewLocation { get; set; }

    /// <summary>
    /// 面试说明
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "面试说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Reason { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 职位发布
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(JobPostingId))]
    public TaktTalentJobPosting? JobPosting { get; set; }

    /// <summary>
    /// 录用信息
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktTalentOffer.InterviewId))]
    public List<TaktTalentOffer>? TalentOffers { get; set; }
}
