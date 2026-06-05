// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegation.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：员工代理关系实体，独立记录所有代理场景
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工代理关系实体
/// 独立记录所有代理场景（部门代理、岗位代理、审批代理等）
/// 
/// 参考 SAP HR 设计：
/// - Infotype 0001 (组织分配) 中的代理字段
/// - T77UA 代理表
/// - SWAC 工作流代理模块
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_delegation", "员工代理关系表")]
[SugarIndex("ix_employee_delegation_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_delegation_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_employee_delegation_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OriginalEmployeeId), OrderByType.Asc, nameof(ProxyEmployeeId), OrderByType.Asc, nameof(DelegationType), OrderByType.Asc, nameof(StartDate), OrderByType.Asc, true)]
public class TaktEmployeeDelegation : TaktCompanyEntityBase
{
    /// <summary>
    /// 代理人ID（代替别人处理工作的人）
    /// </summary>
    [SugarColumn(ColumnName = "proxy_employee_id", ColumnDescription = "代理人ID", ColumnDataType = "bigint", IsNullable = false)]
    public long ProxyEmployeeId { get; set; }

    /// <summary>
    /// 被代理人ID（需要别人代替的人）
    /// </summary>
    [SugarColumn(ColumnName = "original_employee_id", ColumnDescription = "被代理人ID", ColumnDataType = "bigint", IsNullable = false)]
    public long OriginalEmployeeId { get; set; }

    /// <summary>
    /// 代理类型
    /// 1 = 完全代理（代理人拥有被代理人的所有权限）
    /// 2 = 部分代理（仅代理特定部门/岗位的权限）
    /// 3 = 审批代理（仅代理审批流程）
    /// </summary>
    [SugarColumn(ColumnName = "delegation_type", ColumnDescription = "代理类型", ColumnDataType = "int", IsNullable = false)]
    public int DelegationType { get; set; }

    /// <summary>
    /// 代理范围类型
    /// 1 = 部门级别（代理被代理人在特定部门的所有权限）
    /// 2 = 岗位级别（代理被代理人在特定岗位的所有权限）
    /// 3 = 全局代理（代理被代理人的所有权限）
    /// 4 = 特定业务（仅代理特定业务流程）
    /// </summary>
    [SugarColumn(ColumnName = "scope_type", ColumnDescription = "代理范围类型", ColumnDataType = "int", IsNullable = false)]
    public int ScopeType { get; set; }

    /// <summary>
    /// 代理范围ID
    /// 当 ScopeType=1 时，表示部门ID
    /// 当 ScopeType=2 时，表示岗位ID
    /// 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
    /// </summary>
    [SugarColumn(ColumnName = "scope_id", ColumnDescription = "代理范围ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? ScopeId { get; set; }

    /// <summary>
    /// 代理原因
    /// 如：休假、出差、培训、岗位空缺、病假等
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "代理原因", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 代理开始时间
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "代理开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 代理结束时间
    /// null = 长期有效，直到手动删除
    /// </summary>
    [SugarColumn(ColumnName = "end_date", ColumnDescription = "代理结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EndDate { get; set; }
}
