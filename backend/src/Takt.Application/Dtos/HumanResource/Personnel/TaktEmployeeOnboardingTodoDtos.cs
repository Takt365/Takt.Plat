// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeOnboardingTodoDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeOnboardingTodo 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeOnboardingTodo 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Application.Dtos.HumanResource.Talent;

namespace Takt.Application.Dtos.HumanResource.Personnel;

// ========================================
// EmployeeOnboardingTodo 响应 DTO
// ========================================

/// <summary>
/// 入职待办（办理待办单，非审批单；状态见 todo_status）
/// 对应前端 TaktEmployeeOnboardingTodoDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeOnboardingTodoDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeOnboardingTodoID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeOnboardingTodoId { get; set; }

    /// <summary>
    /// 录用信息ID（人才管理 TaktTalentOffer）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OfferId { get; set; }

    /// <summary>
    /// 录用信息名称（填充字段）
    /// </summary>
    public string? OfferName { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编号）
    /// </summary>
    public string TodoNo { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
    /// </summary>
    public int TodoStatus { get; set; } = 0;

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）
    /// </summary>
    public DateTime PlannedJoinedDate { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工ID（建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 关联员工名称（填充字段）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 入职上岗单ID（待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 入职上岗单名称（填充字段）
    /// </summary>
    public string? EmployeeJoinedName { get; set; }

    /// <summary>
    /// 待办说明
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 录用信息
    /// （主表：TaktTalentOffer）
    /// </summary>
    public TaktTalentOfferDto? Offer { get; set; }

    /// <summary>
    /// 入职上岗单
    /// （主表：TaktEmployeeJoined）
    /// </summary>
    public TaktEmployeeJoinedDto? EmployeeJoined { get; set; }

}

// ========================================
// EmployeeOnboardingTodo 查询 DTO
// ========================================

/// <summary>
/// EmployeeOnboardingTodo 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeOnboardingTodoQueryDto : TaktPagedQuery
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
    /// 录用信息ID（人才管理 TaktTalentOffer）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编号）
    /// </summary>
    public string? TodoNo { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
    /// </summary>
    public int? TodoStatus { get; set; }

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）（范围查询-开始）
    /// </summary>
    public DateTime? PlannedJoinedDateStart { get; set; }

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）（范围查询-结束）
    /// </summary>
    public DateTime? PlannedJoinedDateEnd { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string? CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工ID（建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 入职上岗单ID（待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
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
// 创建EmployeeOnboardingTodo DTO
// ========================================

/// <summary>
/// 创建EmployeeOnboardingTodo DTO
/// </summary>
public class TaktEmployeeOnboardingTodoCreateDto
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
    /// 录用信息ID（人才管理 TaktTalentOffer）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编号）
    /// </summary>
    [Required(ErrorMessage = "待办单号（租户+公司内业务编号）不能为空")]
    public string TodoNo { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
    /// </summary>
    public int TodoStatus { get; set; } = 0;

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）
    /// </summary>
    public DateTime PlannedJoinedDate { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    [Required(ErrorMessage = "候选人姓名（快照）不能为空")]
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工ID（建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 入职上岗单ID（待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
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
// 更新EmployeeOnboardingTodo DTO
// ========================================

/// <summary>
/// 更新EmployeeOnboardingTodo DTO
/// 继承 TaktEmployeeOnboardingTodoCreateDto，添加 EmployeeOnboardingTodoId 字段
/// </summary>
public class TaktEmployeeOnboardingTodoUpdateDto : TaktEmployeeOnboardingTodoCreateDto
{
    /// <summary>
    /// EmployeeOnboardingTodoID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeOnboardingTodoId { get; set; }

}

// ========================================
// EmployeeOnboardingTodo 状态 DTO
// ========================================

/// <summary>
/// EmployeeOnboardingTodo 状态更新 DTO
/// </summary>
public class TaktEmployeeOnboardingTodoStatusDto
{
    /// <summary>
    /// EmployeeOnboardingTodoID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeOnboardingTodoId { get; set; }

    /// <summary>
    /// 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
    /// </summary>
    [Required(ErrorMessage = "待办状态（0=待办理，1=办理中，2=已完成，3=已取消）不能为空")]
    public int TodoStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeOnboardingTodo 导入模板行 DTO
/// </summary>
public class TaktEmployeeOnboardingTodoTemplateDto
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
    /// 录用信息ID（人才管理 TaktTalentOffer）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编号）
    /// </summary>
    public string? TodoNo { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
    /// </summary>
    public int? TodoStatus { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string? CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工ID（建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 入职上岗单ID（待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
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
/// EmployeeOnboardingTodo 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeOnboardingTodoImportDto
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
    /// 录用信息ID（人才管理 TaktTalentOffer）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编号）
    /// </summary>
    public string? TodoNo { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
    /// </summary>
    public int? TodoStatus { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string? CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工ID（建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 入职上岗单ID（待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
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
/// EmployeeOnboardingTodo 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeOnboardingTodoExportDto
{
    /// <summary>
    /// EmployeeOnboardingTodoID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeOnboardingTodoId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 录用信息ID（人才管理 TaktTalentOffer）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OfferId { get; set; }

    /// <summary>
    /// 待办单号（租户+公司内业务编号）
    /// </summary>
    public string TodoNo { get; set; } = string.Empty;

    /// <summary>
    /// 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
    /// </summary>
    public int TodoStatus { get; set; } = 0;

    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）
    /// </summary>
    public DateTime PlannedJoinedDate { get; set; }

    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    public string CandidateName { get; set; } = string.Empty;

    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 关联员工ID（建档后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 入职上岗单ID（待办完成后回填，可空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeJoinedId { get; set; }

    /// <summary>
    /// 待办说明
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
