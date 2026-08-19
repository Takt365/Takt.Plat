// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegationDtos.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeDelegation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployeeDelegation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Personnel;

// ========================================
// EmployeeDelegation 响应 DTO
// ========================================

/// <summary>
/// 员工代理关系实体 独立记录所有代理场景（部门代理、岗位代理、审批代理等） 参考 SAP HR 设计： - Infotype 0001 (组织分配) 中的代理字段 - T77UA 代理表 - SWAC 工作流代理模块
/// 对应前端 TaktEmployeeDelegationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeDelegationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeDelegationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeDelegationId { get; set; }

    /// <summary>
    /// 代理人ID（代替别人处理工作的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProxyEmployeeId { get; set; }

    /// <summary>
    /// 代理人名称（填充字段）
    /// </summary>
    public string? ProxyEmployeeName { get; set; }

    /// <summary>
    /// 被代理人ID（需要别人代替的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OriginalEmployeeId { get; set; }

    /// <summary>
    /// 被代理人名称（填充字段）
    /// </summary>
    public string? OriginalEmployeeName { get; set; }

    /// <summary>
    /// 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
    /// </summary>
    public int DelegationType { get; set; } = 0;

    /// <summary>
    /// 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
    /// </summary>
    public int ScopeType { get; set; } = 0;

    /// <summary>
    /// 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理范围名称（填充字段）
    /// </summary>
    public string? ScopeName { get; set; }

    /// <summary>
    /// 代理原因 如：休假、出差、培训、岗位空缺、病假等
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 代理结束时间 null = 长期有效，直到手动删除
    /// </summary>
    public DateTime? EndDate { get; set; }

}

// ========================================
// EmployeeDelegation 查询 DTO
// ========================================

/// <summary>
/// EmployeeDelegation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeDelegationQueryDto : TaktPagedQuery
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
    /// 代理人ID（代替别人处理工作的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProxyEmployeeId { get; set; }

    /// <summary>
    /// 被代理人ID（需要别人代替的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OriginalEmployeeId { get; set; }

    /// <summary>
    /// 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
    /// </summary>
    public int? DelegationType { get; set; }

    /// <summary>
    /// 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
    /// </summary>
    public int? ScopeType { get; set; }

    /// <summary>
    /// 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因 如：休假、出差、培训、岗位空缺、病假等
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 代理开始时间（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 代理结束时间 null = 长期有效，直到手动删除（范围查询-开始）
    /// </summary>
    public DateTime? EndDateStart { get; set; }

    /// <summary>
    /// 代理结束时间 null = 长期有效，直到手动删除（范围查询-结束）
    /// </summary>
    public DateTime? EndDateEnd { get; set; }

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
// 创建EmployeeDelegation DTO
// ========================================

/// <summary>
/// 创建EmployeeDelegation DTO
/// </summary>
public class TaktEmployeeDelegationCreateDto
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
    /// 代理人ID（代替别人处理工作的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProxyEmployeeId { get; set; }

    /// <summary>
    /// 被代理人ID（需要别人代替的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OriginalEmployeeId { get; set; }

    /// <summary>
    /// 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
    /// </summary>
    public int DelegationType { get; set; } = 0;

    /// <summary>
    /// 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
    /// </summary>
    public int ScopeType { get; set; } = 0;

    /// <summary>
    /// 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因 如：休假、出差、培训、岗位空缺、病假等
    /// </summary>
    [Required(ErrorMessage = "代理原因 如：休假、出差、培训、岗位空缺、病假等不能为空")]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 代理结束时间 null = 长期有效，直到手动删除
    /// </summary>
    public DateTime? EndDate { get; set; }

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
// 更新EmployeeDelegation DTO
// ========================================

/// <summary>
/// 更新EmployeeDelegation DTO
/// 继承 TaktEmployeeDelegationCreateDto，添加 EmployeeDelegationId 字段
/// </summary>
public class TaktEmployeeDelegationUpdateDto : TaktEmployeeDelegationCreateDto
{
    /// <summary>
    /// EmployeeDelegationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeDelegationId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmployeeDelegation 导入模板行 DTO
/// </summary>
public class TaktEmployeeDelegationTemplateDto
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
    /// 代理人ID（代替别人处理工作的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProxyEmployeeId { get; set; }

    /// <summary>
    /// 被代理人ID（需要别人代替的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OriginalEmployeeId { get; set; }

    /// <summary>
    /// 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
    /// </summary>
    public int? DelegationType { get; set; }

    /// <summary>
    /// 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
    /// </summary>
    public int? ScopeType { get; set; }

    /// <summary>
    /// 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因 如：休假、出差、培训、岗位空缺、病假等
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 代理结束时间 null = 长期有效，直到手动删除
    /// </summary>
    public DateTime? EndDate { get; set; }

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
/// EmployeeDelegation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeDelegationImportDto
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
    /// 代理人ID（代替别人处理工作的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProxyEmployeeId { get; set; }

    /// <summary>
    /// 被代理人ID（需要别人代替的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OriginalEmployeeId { get; set; }

    /// <summary>
    /// 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
    /// </summary>
    public int? DelegationType { get; set; }

    /// <summary>
    /// 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
    /// </summary>
    public int? ScopeType { get; set; }

    /// <summary>
    /// 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因 如：休假、出差、培训、岗位空缺、病假等
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 代理结束时间 null = 长期有效，直到手动删除
    /// </summary>
    public DateTime? EndDate { get; set; }

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
/// EmployeeDelegation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeDelegationExportDto
{
    /// <summary>
    /// EmployeeDelegationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeDelegationId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 代理人ID（代替别人处理工作的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProxyEmployeeId { get; set; }

    /// <summary>
    /// 被代理人ID（需要别人代替的人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OriginalEmployeeId { get; set; }

    /// <summary>
    /// 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
    /// </summary>
    public int DelegationType { get; set; } = 0;

    /// <summary>
    /// 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
    /// </summary>
    public int ScopeType { get; set; } = 0;

    /// <summary>
    /// 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因 如：休假、出差、培训、岗位空缺、病假等
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 代理结束时间 null = 长期有效，直到手动删除
    /// </summary>
    public DateTime? EndDate { get; set; }

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
