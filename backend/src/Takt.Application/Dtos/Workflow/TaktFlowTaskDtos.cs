// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktFlowTaskDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowTask 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFlowTask 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Workflow;

// ========================================
// FlowTask 响应 DTO
// ========================================

/// <summary>
/// 流程用户任务实体
/// 对应前端 TaktFlowTaskDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFlowTaskDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FlowTaskID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 流程实例 名称（填充字段）
    /// </summary>
    public string? InstanceName { get; set; }

    /// <summary>
    /// 任务定义键（设计器节点 nodeId）
    /// </summary>
    public string TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务所有者 名称（填充字段）
    /// </summary>
    public string? OwnerUserName { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktFlowTaskStatus TaskStatus { get; set; }

    /// <summary>
    /// 会签类型
    /// </summary>
    public TaktFlowSignType SignType { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 认领时间
    /// </summary>
    public DateTime? ClaimTime { get; set; }

    /// <summary>
    /// 办结时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 是否加签任务
    /// </summary>
    public int IsAddSign { get; set; } = 0;

    /// <summary>
    /// 加签记录 ID（<see cref="TaktFlowAddSign"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 加签记录 名称（填充字段）
    /// </summary>
    public string? AddSignName { get; set; }

    /// <summary>
    /// 多实例序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 所属流程实例
    /// （主表：TaktFlowInstance）
    /// </summary>
    public TaktFlowInstanceDto? Instance { get; set; }

}

// ========================================
// FlowTask 查询 DTO
// ========================================

/// <summary>
/// FlowTask 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFlowTaskQueryDto : TaktPagedQuery
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器节点 nodeId）
    /// </summary>
    public string? TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktFlowTaskStatus? TaskStatus { get; set; }

    /// <summary>
    /// 会签类型
    /// </summary>
    public TaktFlowSignType? SignType { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 到期时间（范围查询-开始）
    /// </summary>
    public DateTime? DueDateStart { get; set; }

    /// <summary>
    /// 到期时间（范围查询-结束）
    /// </summary>
    public DateTime? DueDateEnd { get; set; }

    /// <summary>
    /// 认领时间（范围查询-开始）
    /// </summary>
    public DateTime? ClaimTimeStart { get; set; }

    /// <summary>
    /// 认领时间（范围查询-结束）
    /// </summary>
    public DateTime? ClaimTimeEnd { get; set; }

    /// <summary>
    /// 办结时间（范围查询-开始）
    /// </summary>
    public DateTime? CompletedAtStart { get; set; }

    /// <summary>
    /// 办结时间（范围查询-结束）
    /// </summary>
    public DateTime? CompletedAtEnd { get; set; }

    /// <summary>
    /// 是否加签任务
    /// </summary>
    public int? IsAddSign { get; set; }

    /// <summary>
    /// 加签记录 ID（<see cref="TaktFlowAddSign"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 多实例序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

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
// 创建FlowTask DTO
// ========================================

/// <summary>
/// 创建FlowTask DTO
/// </summary>
public class TaktFlowTaskCreateDto
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器节点 nodeId）
    /// </summary>
    [Required(ErrorMessage = "任务定义键（设计器节点 nodeId）不能为空")]
    public string TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktFlowTaskStatus TaskStatus { get; set; }

    /// <summary>
    /// 会签类型
    /// </summary>
    public TaktFlowSignType SignType { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 认领时间
    /// </summary>
    public DateTime? ClaimTime { get; set; }

    /// <summary>
    /// 办结时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 是否加签任务
    /// </summary>
    public int IsAddSign { get; set; } = 0;

    /// <summary>
    /// 加签记录 ID（<see cref="TaktFlowAddSign"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 多实例序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

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
// 更新FlowTask DTO
// ========================================

/// <summary>
/// 更新FlowTask DTO
/// 继承 TaktFlowTaskCreateDto，添加 FlowTaskId 字段
/// </summary>
public class TaktFlowTaskUpdateDto : TaktFlowTaskCreateDto
{
    /// <summary>
    /// FlowTaskID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

}

// ========================================
// FlowTask 状态 DTO
// ========================================

/// <summary>
/// FlowTask 状态更新 DTO
/// </summary>
public class TaktFlowTaskStatusDto
{
    /// <summary>
    /// FlowTaskID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    [Required(ErrorMessage = "任务状态不能为空")]
    public TaktFlowTaskStatus TaskStatus { get; set; }
}

// ========================================
// FlowTask 排序 DTO
// ========================================

/// <summary>
/// FlowTask 排序更新 DTO
/// </summary>
public class TaktFlowTaskSortDto
{
    /// <summary>
    /// FlowTaskID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

    /// <summary>
    /// 多实例序号
    /// </summary>
    [Required(ErrorMessage = "多实例序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FlowTask 导入模板行 DTO
/// </summary>
public class TaktFlowTaskTemplateDto
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器节点 nodeId）
    /// </summary>
    public string? TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktFlowTaskStatus? TaskStatus { get; set; }

    /// <summary>
    /// 会签类型
    /// </summary>
    public TaktFlowSignType? SignType { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 是否加签任务
    /// </summary>
    public int? IsAddSign { get; set; }

    /// <summary>
    /// 加签记录 ID（<see cref="TaktFlowAddSign"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 多实例序号
    /// </summary>
    public int? SortOrder { get; set; }

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
/// FlowTask 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFlowTaskImportDto
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器节点 nodeId）
    /// </summary>
    public string? TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktFlowTaskStatus? TaskStatus { get; set; }

    /// <summary>
    /// 会签类型
    /// </summary>
    public TaktFlowSignType? SignType { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 是否加签任务
    /// </summary>
    public int? IsAddSign { get; set; }

    /// <summary>
    /// 加签记录 ID（<see cref="TaktFlowAddSign"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 多实例序号
    /// </summary>
    public int? SortOrder { get; set; }

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
/// FlowTask 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFlowTaskExportDto
{
    /// <summary>
    /// FlowTaskID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器节点 nodeId）
    /// </summary>
    public string TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public TaktFlowTaskStatus TaskStatus { get; set; }

    /// <summary>
    /// 会签类型
    /// </summary>
    public TaktFlowSignType SignType { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 认领时间
    /// </summary>
    public DateTime? ClaimTime { get; set; }

    /// <summary>
    /// 办结时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 是否加签任务
    /// </summary>
    public int IsAddSign { get; set; } = 0;

    /// <summary>
    /// 加签记录 ID（<see cref="TaktFlowAddSign"/>）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 多实例序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

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
