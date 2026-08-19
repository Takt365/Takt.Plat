// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Talent
// 文件名称：TaktTalentStaffingRequirementDtos.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentStaffingRequirement 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTalentStaffingRequirement 生成，请按需审阅）
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
// TalentStaffingRequirement 响应 DTO
// ========================================

/// <summary>
/// 用人需求（审批单；审批状态见 TaktApprovalDtoBase.ApprovalStatus）
/// 对应前端 TaktTalentStaffingRequirementDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktTalentStaffingRequirementDto : TaktApprovalDtoBase
{
    /// <summary>
    /// TalentStaffingRequirementID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentStaffingRequirementId { get; set; }

    /// <summary>
    /// 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
    /// </summary>
    public string ReqNo { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门ID（DeptID，FK→TaktDept）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 申请部门名称（填充字段）
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 申请岗位ID（PositionID，FK→TaktPost）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 申请岗位名称（填充字段）
    /// </summary>
    public string? PostName { get; set; }

    /// <summary>
    /// 职级（JobGrade/Rank，如专员/主任/工程师）
    /// </summary>
    public string? JobGrade { get; set; } = string.Empty;

    /// <summary>
    /// 需求人数（RequestQty，默认 1）
    /// </summary>
    public int RequestQty { get; set; } = 0;

    /// <summary>
    /// 编制类型（HeadcountType：正式/派遣/实习生/临时）
    /// </summary>
    public string HeadcountType { get; set; } = string.Empty;

    /// <summary>
    /// 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
    /// </summary>
    public string ReasonCode { get; set; } = string.Empty;

    /// <summary>
    /// 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplaceEmployeeId { get; set; }

    /// <summary>
    /// 替补员工名称（填充字段）
    /// </summary>
    public string? ReplaceEmployeeName { get; set; }

    /// <summary>
    /// 期望入职日（ExpectedOnboardDate）
    /// </summary>
    public DateTime? ExpectedOnboardDate { get; set; }

    /// <summary>
    /// 合同类型（ContractType：固定期/无固定/实习协议）
    /// </summary>
    public string? ContractType { get; set; } = string.Empty;

    /// <summary>
    /// 工作地点（WorkLocation，如工厂/分公司）
    /// </summary>
    public string? WorkLocation { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职责（JobDesc）
    /// </summary>
    public string? JobDesc { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求（Qualification，学历/经验/技能）
    /// </summary>
    public string? Qualification { get; set; } = string.Empty;

    /// <summary>
    /// 预算年度（BudgetYear，用于 headcount 控制）
    /// </summary>
    public string? BudgetYear { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门
    /// （主表：TaktDept）
    /// </summary>
    public TaktDeptDto? Dept { get; set; }

    /// <summary>
    /// 申请岗位
    /// （主表：TaktPost）
    /// </summary>
    public TaktPostDto? Post { get; set; }

    /// <summary>
    /// 替补员工
    /// （主表：TaktEmployee）
    /// </summary>
    public TaktEmployeeDto? ReplaceEmployee { get; set; }

    /// <summary>
    /// 职位发布
    /// （子表：TaktTalentJobPosting）
    /// </summary>
    public List<TaktTalentJobPostingDto>? TalentJobPostings { get; set; }

}

// ========================================
// TalentStaffingRequirement 查询 DTO
// ========================================

/// <summary>
/// TalentStaffingRequirement 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTalentStaffingRequirementQueryDto : TaktPagedQuery
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
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
    /// </summary>
    public string? ReqNo { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门ID（DeptID，FK→TaktDept）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 申请岗位ID（PositionID，FK→TaktPost）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 职级（JobGrade/Rank，如专员/主任/工程师）
    /// </summary>
    public string? JobGrade { get; set; } = string.Empty;

    /// <summary>
    /// 需求人数（RequestQty，默认 1）
    /// </summary>
    public int? RequestQty { get; set; }

    /// <summary>
    /// 编制类型（HeadcountType：正式/派遣/实习生/临时）
    /// </summary>
    public string? HeadcountType { get; set; } = string.Empty;

    /// <summary>
    /// 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
    /// </summary>
    public string? ReasonCode { get; set; } = string.Empty;

    /// <summary>
    /// 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplaceEmployeeId { get; set; }

    /// <summary>
    /// 期望入职日（ExpectedOnboardDate）（范围查询-开始）
    /// </summary>
    public DateTime? ExpectedOnboardDateStart { get; set; }

    /// <summary>
    /// 期望入职日（ExpectedOnboardDate）（范围查询-结束）
    /// </summary>
    public DateTime? ExpectedOnboardDateEnd { get; set; }

    /// <summary>
    /// 合同类型（ContractType：固定期/无固定/实习协议）
    /// </summary>
    public string? ContractType { get; set; } = string.Empty;

    /// <summary>
    /// 工作地点（WorkLocation，如工厂/分公司）
    /// </summary>
    public string? WorkLocation { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职责（JobDesc）
    /// </summary>
    public string? JobDesc { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求（Qualification，学历/经验/技能）
    /// </summary>
    public string? Qualification { get; set; } = string.Empty;

    /// <summary>
    /// 预算年度（BudgetYear，用于 headcount 控制）
    /// </summary>
    public string? BudgetYear { get; set; } = string.Empty;

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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

    /// <summary>
    /// 需求单号（ReqCode，租户+公司内唯一；自动生成，如 PR-2026-00123）
    /// </summary>
    public string ReqCode { get; set; } = string.Empty;
}

// ========================================
// 创建TalentStaffingRequirement DTO
// ========================================

/// <summary>
/// 创建TalentStaffingRequirement DTO
/// </summary>
public class TaktTalentStaffingRequirementCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;



    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
    /// </summary>
    [Required(ErrorMessage = "需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）不能为空")]
    public string ReqNo { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门ID（DeptID，FK→TaktDept）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 申请岗位ID（PositionID，FK→TaktPost）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 职级（JobGrade/Rank，如专员/主任/工程师）
    /// </summary>
    public string? JobGrade { get; set; } = string.Empty;

    /// <summary>
    /// 需求人数（RequestQty，默认 1）
    /// </summary>
    public int RequestQty { get; set; } = 0;

    /// <summary>
    /// 编制类型（HeadcountType：正式/派遣/实习生/临时）
    /// </summary>
    [Required(ErrorMessage = "编制类型（HeadcountType：正式/派遣/实习生/临时）不能为空")]
    public string HeadcountType { get; set; } = string.Empty;

    /// <summary>
    /// 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
    /// </summary>
    [Required(ErrorMessage = "需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）不能为空")]
    public string ReasonCode { get; set; } = string.Empty;

    /// <summary>
    /// 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplaceEmployeeId { get; set; }

    /// <summary>
    /// 期望入职日（ExpectedOnboardDate）
    /// </summary>
    public DateTime? ExpectedOnboardDate { get; set; }

    /// <summary>
    /// 合同类型（ContractType：固定期/无固定/实习协议）
    /// </summary>
    public string? ContractType { get; set; } = string.Empty;

    /// <summary>
    /// 工作地点（WorkLocation，如工厂/分公司）
    /// </summary>
    public string? WorkLocation { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职责（JobDesc）
    /// </summary>
    public string? JobDesc { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求（Qualification，学历/经验/技能）
    /// </summary>
    public string? Qualification { get; set; } = string.Empty;

    /// <summary>
    /// 预算年度（BudgetYear，用于 headcount 控制）
    /// </summary>
    public string? BudgetYear { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布（子表，级联保存）
    /// </summary>
    public List<TaktTalentJobPostingCreateDto>? TalentJobPostings { get; set; }

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
// 更新TalentStaffingRequirement DTO
// ========================================

/// <summary>
/// 更新TalentStaffingRequirement DTO
/// 继承 TaktTalentStaffingRequirementCreateDto，添加 TalentStaffingRequirementId 字段
/// </summary>
public class TaktTalentStaffingRequirementUpdateDto : TaktTalentStaffingRequirementCreateDto
{
    /// <summary>
    /// TalentStaffingRequirementID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentStaffingRequirementId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TalentStaffingRequirement 导入模板行 DTO
/// </summary>
public class TaktTalentStaffingRequirementTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
    /// </summary>
    public string? ReqNo { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门ID（DeptID，FK→TaktDept）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 申请岗位ID（PositionID，FK→TaktPost）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 职级（JobGrade/Rank，如专员/主任/工程师）
    /// </summary>
    public string? JobGrade { get; set; } = string.Empty;

    /// <summary>
    /// 需求人数（RequestQty，默认 1）
    /// </summary>
    public int? RequestQty { get; set; }

    /// <summary>
    /// 编制类型（HeadcountType：正式/派遣/实习生/临时）
    /// </summary>
    public string? HeadcountType { get; set; } = string.Empty;

    /// <summary>
    /// 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
    /// </summary>
    public string? ReasonCode { get; set; } = string.Empty;

    /// <summary>
    /// 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplaceEmployeeId { get; set; }

    /// <summary>
    /// 期望入职日（ExpectedOnboardDate）
    /// </summary>
    public DateTime? ExpectedOnboardDate { get; set; }

    /// <summary>
    /// 合同类型（ContractType：固定期/无固定/实习协议）
    /// </summary>
    public string? ContractType { get; set; } = string.Empty;

    /// <summary>
    /// 工作地点（WorkLocation，如工厂/分公司）
    /// </summary>
    public string? WorkLocation { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职责（JobDesc）
    /// </summary>
    public string? JobDesc { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求（Qualification，学历/经验/技能）
    /// </summary>
    public string? Qualification { get; set; } = string.Empty;

    /// <summary>
    /// 预算年度（BudgetYear，用于 headcount 控制）
    /// </summary>
    public string? BudgetYear { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布（子表，级联保存）
    /// </summary>
    public List<TaktTalentJobPostingCreateDto>? TalentJobPostings { get; set; }

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
/// TalentStaffingRequirement 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTalentStaffingRequirementImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;



    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
    /// </summary>
    public string? ReqNo { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门ID（DeptID，FK→TaktDept）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 申请岗位ID（PositionID，FK→TaktPost）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PostId { get; set; }

    /// <summary>
    /// 职级（JobGrade/Rank，如专员/主任/工程师）
    /// </summary>
    public string? JobGrade { get; set; } = string.Empty;

    /// <summary>
    /// 需求人数（RequestQty，默认 1）
    /// </summary>
    public int? RequestQty { get; set; }

    /// <summary>
    /// 编制类型（HeadcountType：正式/派遣/实习生/临时）
    /// </summary>
    public string? HeadcountType { get; set; } = string.Empty;

    /// <summary>
    /// 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
    /// </summary>
    public string? ReasonCode { get; set; } = string.Empty;

    /// <summary>
    /// 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplaceEmployeeId { get; set; }

    /// <summary>
    /// 期望入职日（ExpectedOnboardDate）
    /// </summary>
    public DateTime? ExpectedOnboardDate { get; set; }

    /// <summary>
    /// 合同类型（ContractType：固定期/无固定/实习协议）
    /// </summary>
    public string? ContractType { get; set; } = string.Empty;

    /// <summary>
    /// 工作地点（WorkLocation，如工厂/分公司）
    /// </summary>
    public string? WorkLocation { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职责（JobDesc）
    /// </summary>
    public string? JobDesc { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求（Qualification，学历/经验/技能）
    /// </summary>
    public string? Qualification { get; set; } = string.Empty;

    /// <summary>
    /// 预算年度（BudgetYear，用于 headcount 控制）
    /// </summary>
    public string? BudgetYear { get; set; } = string.Empty;

    /// <summary>
    /// 职位发布（子表，级联保存）
    /// </summary>
    public List<TaktTalentJobPostingCreateDto>? TalentJobPostings { get; set; }

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
/// TalentStaffingRequirement 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTalentStaffingRequirementExportDto
{
    /// <summary>
    /// TalentStaffingRequirementID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TalentStaffingRequirementId { get; set; }

    /// <summary>
    /// 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
    /// </summary>
    public string ReqNo { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门ID（DeptID，FK→TaktDept）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 申请岗位ID（PositionID，FK→TaktPost）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 职级（JobGrade/Rank，如专员/主任/工程师）
    /// </summary>
    public string? JobGrade { get; set; } = string.Empty;

    /// <summary>
    /// 需求人数（RequestQty，默认 1）
    /// </summary>
    public int RequestQty { get; set; } = 0;

    /// <summary>
    /// 编制类型（HeadcountType：正式/派遣/实习生/临时）
    /// </summary>
    public string HeadcountType { get; set; } = string.Empty;

    /// <summary>
    /// 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
    /// </summary>
    public string ReasonCode { get; set; } = string.Empty;

    /// <summary>
    /// 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplaceEmployeeId { get; set; }

    /// <summary>
    /// 期望入职日（ExpectedOnboardDate）
    /// </summary>
    public DateTime? ExpectedOnboardDate { get; set; }

    /// <summary>
    /// 合同类型（ContractType：固定期/无固定/实习协议）
    /// </summary>
    public string? ContractType { get; set; } = string.Empty;

    /// <summary>
    /// 工作地点（WorkLocation，如工厂/分公司）
    /// </summary>
    public string? WorkLocation { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职责（JobDesc）
    /// </summary>
    public string? JobDesc { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求（Qualification，学历/经验/技能）
    /// </summary>
    public string? Qualification { get; set; } = string.Empty;

    /// <summary>
    /// 预算年度（BudgetYear，用于 headcount 控制）
    /// </summary>
    public string? BudgetYear { get; set; } = string.Empty;

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
