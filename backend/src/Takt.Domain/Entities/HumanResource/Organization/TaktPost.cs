// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Organization
// 文件名称：TaktPost.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：岗位实体，代表组织架构中的岗位/职位
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.HumanResource.Organization;

/// <summary>
/// 岗位实体
/// 代表组织架构中的岗位/职位
/// 参照 SAP Position (STELL) 设计
/// </summary>
[SugarTable("takt_human_resource_organization_post", "岗位表")]
[SugarIndex("ix_post_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_post_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_post_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PostCode), OrderByType.Asc, true)]
public class TaktPost : TaktCompanyEntityBase
{
    /// <summary>
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    [SugarColumn(ColumnName = "post_code", ColumnDescription = "岗位编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    [SugarColumn(ColumnName = "post_name", ColumnDescription = "岗位名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门ID
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "所属部门ID", ColumnDataType = "bigint", IsNullable = false)]
    public long DeptId { get; set; }

    /// <summary>
    /// 岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）
    /// </summary>
    [SugarColumn(ColumnName = "post_type", ColumnDescription = "岗位类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public TaktPostType PostType { get; set; } = TaktPostType.Technical;

    /// <summary>
    /// 岗位职级（0=一线/基层，1=技术/骨干层，2=管理/决策层）
    /// </summary>
    [SugarColumn(ColumnName = "post_level", ColumnDescription = "岗位职级", ColumnDataType = "int", IsNullable = true)]
    public TaktPostLevel? PostLevel { get; set; }

    /// <summary>
    /// 编制人数
    /// </summary>
    [SugarColumn(ColumnName = "headcount", ColumnDescription = "编制人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Headcount { get; set; } = 1;

    /// <summary>
    /// 当前在职人数
    /// </summary>
    [SugarColumn(ColumnName = "current_count", ColumnDescription = "当前在职人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CurrentCount { get; set; } = 0;

    /// <summary>
    /// 岗位职责
    /// </summary>
    [SugarColumn(ColumnName = "responsibilities", ColumnDescription = "岗位职责", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? Responsibilities { get; set; }

    /// <summary>
    /// 任职要求
    /// </summary>
    [SugarColumn(ColumnName = "requirements", ColumnDescription = "任职要求", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? Requirements { get; set; }

    /// <summary>
    /// 学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）
    /// </summary>
    [SugarColumn(ColumnName = "education_required", ColumnDescription = "学历要求", ColumnDataType = "int", IsNullable = true)]
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    [SugarColumn(ColumnName = "experience_years", ColumnDescription = "工作经验要求（年）", ColumnDataType = "int", IsNullable = true)]
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    [SugarColumn(ColumnName = "salary_min", ColumnDescription = "薪资范围（最低）", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = true)]
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    [SugarColumn(ColumnName = "salary_max", ColumnDescription = "薪资范围（最高）", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = true)]
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "post_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public TaktCommonStatus PostStatus { get; set; } = TaktCommonStatus.Enabled;

    /// <summary>
    /// 是否内置（1=是，0=否）
    /// 种子岗位为内置，不允许删除
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "是否内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo IsBuiltIn { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 岗位描述
    /// </summary>
    [SugarColumn(ColumnName = "description", ColumnDescription = "岗位描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Description { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeePost.PostId))]
    public List<TaktEmployeePost>? EmployeePosts { get; set; }

}
