// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployee.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：员工主档实体（人事档案核心表）；招聘/入职上岗/离职/合同/调动等明细见同域子表导航属性
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工实体（人事主档，公司级档案非审批单）
/// 员工与系统用户分离；子表承载合同、调动、任职、教育、家庭、技能、外部履历、附件等全场景明细
/// 参照 SAP Personnel Number (PERNR) 设计
/// </summary>
[SugarTable("takt_human_resource_personnel_employee", "员工表")]
[SugarIndex("ix_employee_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_employee_no_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeNo), OrderByType.Asc, true)]
[SugarIndex("ix_employee_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeStatus), OrderByType.Asc, false)]
public class TaktEmployee : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工编号（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "employee_no", ColumnDescription = "员工编号", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string EmployeeNo { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    [SugarColumn(ColumnName = "name", ColumnDescription = "姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 性别（0=未知，1=男，2=女）
    /// </summary>
    [SugarColumn(ColumnName = "gender", ColumnDescription = "性别", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Gender { get; set; } = 0;

    /// <summary>
    /// 出生日期
    /// </summary>
    [SugarColumn(ColumnName = "birth_date", ColumnDescription = "出生日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    [SugarColumn(ColumnName = "id_card_no", ColumnDescription = "身份证号", ColumnDataType = "varchar", Length = 18, IsNullable = true)]
    public string? IdCardNo { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [SugarColumn(ColumnName = "mobile", ColumnDescription = "手机号码", ColumnDataType = "varchar", Length = 11, IsNullable = true)]
    public string? Mobile { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    [SugarColumn(ColumnName = "email", ColumnDescription = "电子邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? Email { get; set; }

    /// <summary>
    /// 籍贯（字典 hr_native_place 编码或文本）
    /// </summary>
    [SugarColumn(ColumnName = "native_place", ColumnDescription = "籍贯", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? NativePlace { get; set; }

    /// <summary>
    /// 民族（字典 hr_ethnic_group 编码或文本）
    /// </summary>
    [SugarColumn(ColumnName = "ethnicity", ColumnDescription = "民族", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? Ethnicity { get; set; }

    /// <summary>
    /// 政治面貌（字典 hr_political_status 编码或文本）
    /// </summary>
    [SugarColumn(ColumnName = "political_status", ColumnDescription = "政治面貌", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PoliticalStatus { get; set; }

    /// <summary>
    /// 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）
    /// </summary>
    [SugarColumn(ColumnName = "marital_status", ColumnDescription = "婚姻状况", ColumnDataType = "int", IsNullable = true)]
    public int? MaritalStatus { get; set; }

    /// <summary>
    /// 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
    /// </summary>
    [SugarColumn(ColumnName = "education", ColumnDescription = "最高学历", ColumnDataType = "int", IsNullable = true)]
    public int? Education { get; set; }

    /// <summary>
    /// 毕业院校（最高学历摘要）
    /// </summary>
    [SugarColumn(ColumnName = "graduate_school", ColumnDescription = "毕业院校", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? GraduateSchool { get; set; }

    /// <summary>
    /// 专业（最高学历摘要）
    /// </summary>
    [SugarColumn(ColumnName = "major", ColumnDescription = "专业", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? Major { get; set; }

    /// <summary>
    /// 实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）
    /// </summary>
    [SugarColumn(ColumnName = "joined_date", ColumnDescription = "实际上岗日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? JoinedDate { get; set; }

    /// <summary>
    /// 试用期结束日期
    /// </summary>
    [SugarColumn(ColumnName = "probation_end_date", ColumnDescription = "试用期结束日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 转正日期
    /// </summary>
    [SugarColumn(ColumnName = "regular_date", ColumnDescription = "转正日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RegularDate { get; set; }

    /// <summary>
    /// 离职日期
    /// </summary>
    [SugarColumn(ColumnName = "termination_date", ColumnDescription = "离职日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 最后工作日
    /// </summary>
    [SugarColumn(ColumnName = "last_work_date", ColumnDescription = "最后工作日", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
    /// </summary>
    [SugarColumn(ColumnName = "resignation_type", ColumnDescription = "离职类型", ColumnDataType = "int", IsNullable = true)]
    public int? ResignationType { get; set; }

    /// <summary>
    /// 离职原因
    /// </summary>
    [SugarColumn(ColumnName = "resignation_reason", ColumnDescription = "离职原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ResignationReason { get; set; }

    /// <summary>
    /// 员工状态（1=试用期，2=正式，3=离职，4=退休）
    /// </summary>
    [SugarColumn(ColumnName = "employee_status", ColumnDescription = "员工状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EmployeeStatus { get; set; } = 1;

    /// <summary>
    /// 当前主部门ID（任职快照，与最新已生效上岗单同步）
    /// </summary>
    [SugarColumn(ColumnName = "primary_dept_id", ColumnDescription = "当前主部门ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? PrimaryDeptId { get; set; }

    /// <summary>
    /// 当前主岗位ID（任职快照）
    /// </summary>
    [SugarColumn(ColumnName = "primary_post_id", ColumnDescription = "当前主岗位ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? PrimaryPostId { get; set; }

    /// <summary>
    /// 是否内置（种子员工不可删）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "是否内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo IsBuiltIn { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 紧急联系人姓名
    /// </summary>
    [SugarColumn(ColumnName = "emergency_contact_name", ColumnDescription = "紧急联系人姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? EmergencyContactName { get; set; }

    /// <summary>
    /// 紧急联系人电话
    /// </summary>
    [SugarColumn(ColumnName = "emergency_contact_phone", ColumnDescription = "紧急联系人电话", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? EmergencyContactPhone { get; set; }

    /// <summary>
    /// 家庭住址
    /// </summary>
    [SugarColumn(ColumnName = "home_address", ColumnDescription = "家庭住址", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? HomeAddress { get; set; }

    /// <summary>
    /// 照片URL
    /// </summary>
    [SugarColumn(ColumnName = "photo_url", ColumnDescription = "照片URL", ColumnDataType = "varchar", Length = 500, IsNullable = true)]
    public string? PhotoUrl { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeDept.EmployeeId))]
    public List<TaktEmployeeDept>? EmployeeDepts { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeePost.EmployeeId))]
    public List<TaktEmployeePost>? EmployeePosts { get; set; }

}
