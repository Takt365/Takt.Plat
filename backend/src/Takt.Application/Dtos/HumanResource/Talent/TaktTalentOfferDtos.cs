// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Talent
// 文件名称：TaktTalentOfferDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentOffer 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTalentOffer 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Application.Dtos.HumanResource.Personnel;

namespace Takt.Application.Dtos.HumanResource.Talent;

// ========================================
// TalentOffer 响应 DTO
// ========================================

/// <summary>
/// 录用信息（审批单，状态见 <see cref="TaktApprovalEntityBase.ApprovalStatus"/>）
/// 对应前端 TaktTalentOfferDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktTalentOfferDto : TaktApprovalDtoBase
{
    /// <summary>
    /// TalentOfferID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentOfferId { get; set; }

    /// <summary>
    /// 面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InterviewId { get; set; }

    /// <summary>
    /// 面试安排名称（填充字段）
    /// </summary>
    public string? InterviewName { get; set; }

    /// <summary>
    /// 录用编号（租户+公司内业务编号）
    /// </summary>
    public string OfferNo { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（HireDate：确认录用/发 offer）
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// 关联员工ID（录用通过并建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 关联员工名称（填充字段）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 拟录用部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 面试安排
    /// （主表：TaktTalentInterview）
    /// </summary>
    public TaktTalentInterviewDto? Interview { get; set; }

    /// <summary>
    /// 入职待办
    /// （子表：TaktEmployeeOnboardingTodo）
    /// </summary>
    public List<TaktEmployeeOnboardingTodoDto>? EmployeeOnboardingTodos { get; set; }

}

// ========================================
// TalentOffer 查询 DTO
// ========================================

/// <summary>
/// TalentOffer 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTalentOfferQueryDto : TaktPagedQuery
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
    /// 面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InterviewId { get; set; }

    /// <summary>
    /// 录用编号（租户+公司内业务编号）
    /// </summary>
    public string? OfferNo { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（HireDate：确认录用/发 offer）（范围查询-开始）
    /// </summary>
    public DateTime? HireDateStart { get; set; }

    /// <summary>
    /// 录用日期（HireDate：确认录用/发 offer）（范围查询-结束）
    /// </summary>
    public DateTime? HireDateEnd { get; set; }

    /// <summary>
    /// 关联员工ID（录用通过并建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 审批状态（TaktApprovalStatus）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

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
// 创建TalentOffer DTO
// ========================================

/// <summary>
/// 创建TalentOffer DTO
/// </summary>
public class TaktTalentOfferCreateDto
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
    /// 面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InterviewId { get; set; }

    /// <summary>
    /// 录用编号（租户+公司内业务编号）
    /// </summary>
    [Required(ErrorMessage = "录用编号（租户+公司内业务编号）不能为空")]
    public string OfferNo { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（HireDate：确认录用/发 offer）
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// 关联员工ID（录用通过并建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    [Required(ErrorMessage = "拟录用部门名称不能为空")]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 入职待办（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeOnboardingTodoCreateDto>? EmployeeOnboardingTodos { get; set; }

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
// 更新TalentOffer DTO
// ========================================

/// <summary>
/// 更新TalentOffer DTO
/// 继承 TaktTalentOfferCreateDto，添加 TalentOfferId 字段
/// </summary>
public class TaktTalentOfferUpdateDto : TaktTalentOfferCreateDto
{
    /// <summary>
    /// TalentOfferID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentOfferId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TalentOffer 导入模板行 DTO
/// </summary>
public class TaktTalentOfferTemplateDto
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
    /// 面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InterviewId { get; set; }

    /// <summary>
    /// 录用编号（租户+公司内业务编号）
    /// </summary>
    public string? OfferNo { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工ID（录用通过并建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
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
/// TalentOffer 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTalentOfferImportDto
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
    /// 面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InterviewId { get; set; }

    /// <summary>
    /// 录用编号（租户+公司内业务编号）
    /// </summary>
    public string? OfferNo { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工ID（录用通过并建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
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
/// TalentOffer 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTalentOfferExportDto
{
    /// <summary>
    /// TalentOfferID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentOfferId { get; set; }

    /// <summary>
    /// 面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InterviewId { get; set; }

    /// <summary>
    /// 录用编号（租户+公司内业务编号）
    /// </summary>
    public string OfferNo { get; set; } = string.Empty;

    /// <summary>
    /// 录用日期（HireDate：确认录用/发 offer）
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// 关联员工ID（录用通过并建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 拟录用部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 拟录用岗位ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 录用说明
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
