// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：Employee 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployee 生成，请按需审阅）
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
// Employee 响应 DTO
// ========================================

/// <summary>
/// 员工实体（人事主档，公司级档案非审批单） 员工与系统用户分离；子表承载合同、调动、任职、教育、家庭、技能、外部履历、附件等全场景明细 参照 SAP Personnel Number (PERNR) 设计
/// 对应前端 TaktEmployeeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工编号（租户+公司内唯一）
    /// </summary>
    public string EmployeeNo { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（0=未知，1=男，2=女）
    /// </summary>
    public int Gender { get; set; } = 0;

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string IdCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 hr_native_place_code 的 6 位 GB 行政区划代码，人事档案必填）
    /// </summary>
    public string NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 hr_ethnic_code，1～56）
    /// </summary>
    public int Ethnicity { get; set; } = 0;

    /// <summary>
    /// 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
    /// </summary>
    public int? Education { get; set; }

    /// <summary>
    /// 毕业院校（最高学历摘要）
    /// </summary>
    public string? GraduateSchool { get; set; } = string.Empty;

    /// <summary>
    /// 专业（最高学历摘要）
    /// </summary>
    public string? Major { get; set; } = string.Empty;

    /// <summary>
    /// 实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）
    /// </summary>
    public DateTime? JoinedDate { get; set; }

    /// <summary>
    /// 试用期结束日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 转正日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? RegularDate { get; set; }

    /// <summary>
    /// 离职日期（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 最后工作日（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 离职类型（投影字段，由离职审批通过后回写；0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
    /// </summary>
    public int? ResignationType { get; set; }

    /// <summary>
    /// 离职原因（投影字段，由离职审批通过后回写）
    /// </summary>
    public string? ResignationReason { get; set; } = string.Empty;

    /// <summary>
    /// 员工状态（1=试用期，2=正式，3=离职，4=退休）
    /// </summary>
    public int EmployeeStatus { get; set; } = 0;

    /// <summary>
    /// 当前主部门ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryDeptId { get; set; }

    /// <summary>
    /// 当前主部门名称（填充字段）
    /// </summary>
    public string? PrimaryDeptName { get; set; }

    /// <summary>
    /// 当前主岗位ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryPostId { get; set; }

    /// <summary>
    /// 当前主岗位名称（填充字段）
    /// </summary>
    public string? PrimaryPostName { get; set; }

    /// <summary>
    /// 内置（种子员工不可删）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 紧急联系人姓名（人事档案必填）
    /// </summary>
    public string EmergencyContactName { get; set; } = string.Empty;

    /// <summary>
    /// 紧急联系人电话（人事档案必填）
    /// </summary>
    public string EmergencyContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 家庭住址（人事档案必填）
    /// </summary>
    public string HomeAddress { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 政治面貌（字典 hr_political_status，0～12；人事档案必填）
    /// </summary>
    public int PoliticalStatus { get; set; } = 0;

    /// <summary>
    /// 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶；人事档案必填）
    /// </summary>
    public int MaritalStatus { get; set; } = 0;

    /// <summary>
    /// 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）
    /// （子表：TaktEmployeeDept）
    /// </summary>
    public List<TaktEmployeeDeptDto>? EmployeeDepts { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）
    /// （子表：TaktEmployeePost）
    /// </summary>
    public List<TaktEmployeePostDto>? EmployeePosts { get; set; }

}

// ========================================
// Employee 查询 DTO
// ========================================

/// <summary>
/// Employee 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeQueryDto : TaktPagedQuery
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
    /// 员工编号（租户+公司内唯一）
    /// </summary>
    public string? EmployeeNo { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（0=未知，1=男，2=女）
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// 出生日期（人事档案必填）（范围查询-开始）
    /// </summary>
    public DateTime? BirthDateStart { get; set; }

    /// <summary>
    /// 出生日期（人事档案必填）（范围查询-结束）
    /// </summary>
    public DateTime? BirthDateEnd { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string? IdCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 hr_native_place_code 的 6 位 GB 行政区划代码，人事档案必填）
    /// </summary>
    public string? NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 hr_ethnic_code，1～56）
    /// </summary>
    public int? Ethnicity { get; set; }

    /// <summary>
    /// 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
    /// </summary>
    public int? Education { get; set; }

    /// <summary>
    /// 毕业院校（最高学历摘要）
    /// </summary>
    public string? GraduateSchool { get; set; } = string.Empty;

    /// <summary>
    /// 专业（最高学历摘要）
    /// </summary>
    public string? Major { get; set; } = string.Empty;

    /// <summary>
    /// 实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）（范围查询-开始）
    /// </summary>
    public DateTime? JoinedDateStart { get; set; }

    /// <summary>
    /// 实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）（范围查询-结束）
    /// </summary>
    public DateTime? JoinedDateEnd { get; set; }

    /// <summary>
    /// 试用期结束日期（投影字段，由上岗审批通过后回写）（范围查询-开始）
    /// </summary>
    public DateTime? ProbationEndDateStart { get; set; }

    /// <summary>
    /// 试用期结束日期（投影字段，由上岗审批通过后回写）（范围查询-结束）
    /// </summary>
    public DateTime? ProbationEndDateEnd { get; set; }

    /// <summary>
    /// 转正日期（投影字段，由上岗审批通过后回写）（范围查询-开始）
    /// </summary>
    public DateTime? RegularDateStart { get; set; }

    /// <summary>
    /// 转正日期（投影字段，由上岗审批通过后回写）（范围查询-结束）
    /// </summary>
    public DateTime? RegularDateEnd { get; set; }

    /// <summary>
    /// 离职日期（投影字段，由离职审批通过后回写）（范围查询-开始）
    /// </summary>
    public DateTime? TerminationDateStart { get; set; }

    /// <summary>
    /// 离职日期（投影字段，由离职审批通过后回写）（范围查询-结束）
    /// </summary>
    public DateTime? TerminationDateEnd { get; set; }

    /// <summary>
    /// 最后工作日（投影字段，由离职审批通过后回写）（范围查询-开始）
    /// </summary>
    public DateTime? LastWorkDateStart { get; set; }

    /// <summary>
    /// 最后工作日（投影字段，由离职审批通过后回写）（范围查询-结束）
    /// </summary>
    public DateTime? LastWorkDateEnd { get; set; }

    /// <summary>
    /// 离职类型（投影字段，由离职审批通过后回写；0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
    /// </summary>
    public int? ResignationType { get; set; }

    /// <summary>
    /// 离职原因（投影字段，由离职审批通过后回写）
    /// </summary>
    public string? ResignationReason { get; set; } = string.Empty;

    /// <summary>
    /// 员工状态（1=试用期，2=正式，3=离职，4=退休）
    /// </summary>
    public int? EmployeeStatus { get; set; }

    /// <summary>
    /// 当前主部门ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryDeptId { get; set; }

    /// <summary>
    /// 当前主岗位ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryPostId { get; set; }

    /// <summary>
    /// 内置（种子员工不可删）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 紧急联系人姓名（人事档案必填）
    /// </summary>
    public string? EmergencyContactName { get; set; } = string.Empty;

    /// <summary>
    /// 紧急联系人电话（人事档案必填）
    /// </summary>
    public string? EmergencyContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 家庭住址（人事档案必填）
    /// </summary>
    public string? HomeAddress { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 政治面貌（字典 hr_political_status，0～12；人事档案必填）
    /// </summary>
    public int? PoliticalStatus { get; set; }

    /// <summary>
    /// 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶；人事档案必填）
    /// </summary>
    public int? MaritalStatus { get; set; }

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
// 创建Employee DTO
// ========================================

/// <summary>
/// 创建Employee DTO
/// </summary>
public class TaktEmployeeCreateDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 员工编号（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "员工编号（租户+公司内唯一）不能为空")]
    public string EmployeeNo { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    [Required(ErrorMessage = "姓名不能为空")]
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（0=未知，1=男，2=女）
    /// </summary>
    public int Gender { get; set; } = 0;

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    [Required(ErrorMessage = "身份证号（人事档案必填）不能为空")]
    public string IdCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    [Required(ErrorMessage = "手机号码（人事档案必填）不能为空")]
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 hr_native_place_code 的 6 位 GB 行政区划代码，人事档案必填）
    /// </summary>
    [Required(ErrorMessage = "籍贯（字典 hr_native_place_code 的 6 位 GB 行政区划代码，人事档案必填）不能为空")]
    public string NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 hr_ethnic_code，1～56）
    /// </summary>
    public int Ethnicity { get; set; } = 0;

    /// <summary>
    /// 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
    /// </summary>
    public int? Education { get; set; }

    /// <summary>
    /// 毕业院校（最高学历摘要）
    /// </summary>
    public string? GraduateSchool { get; set; } = string.Empty;

    /// <summary>
    /// 专业（最高学历摘要）
    /// </summary>
    public string? Major { get; set; } = string.Empty;

    /// <summary>
    /// 实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）
    /// </summary>
    public DateTime? JoinedDate { get; set; }

    /// <summary>
    /// 试用期结束日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 转正日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? RegularDate { get; set; }

    /// <summary>
    /// 离职日期（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 最后工作日（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 离职类型（投影字段，由离职审批通过后回写；0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
    /// </summary>
    public int? ResignationType { get; set; }

    /// <summary>
    /// 离职原因（投影字段，由离职审批通过后回写）
    /// </summary>
    public string? ResignationReason { get; set; } = string.Empty;

    /// <summary>
    /// 员工状态（1=试用期，2=正式，3=离职，4=退休）
    /// </summary>
    public int EmployeeStatus { get; set; } = 0;

    /// <summary>
    /// 当前主部门ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryDeptId { get; set; }

    /// <summary>
    /// 当前主岗位ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryPostId { get; set; }

    /// <summary>
    /// 内置（种子员工不可删）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 紧急联系人姓名（人事档案必填）
    /// </summary>
    [Required(ErrorMessage = "紧急联系人姓名（人事档案必填）不能为空")]
    public string EmergencyContactName { get; set; } = string.Empty;

    /// <summary>
    /// 紧急联系人电话（人事档案必填）
    /// </summary>
    [Required(ErrorMessage = "紧急联系人电话（人事档案必填）不能为空")]
    public string EmergencyContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 家庭住址（人事档案必填）
    /// </summary>
    [Required(ErrorMessage = "家庭住址（人事档案必填）不能为空")]
    public string HomeAddress { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 政治面貌（字典 hr_political_status，0～12；人事档案必填）
    /// </summary>
    public int PoliticalStatus { get; set; } = 0;

    /// <summary>
    /// 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶；人事档案必填）
    /// </summary>
    public int MaritalStatus { get; set; } = 0;

    /// <summary>
    /// 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeeDeptIds { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeePostIds { get; set; }

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
// 更新Employee DTO
// ========================================

/// <summary>
/// 更新Employee DTO
/// 继承 TaktEmployeeCreateDto，添加 EmployeeId 字段
/// </summary>
public class TaktEmployeeUpdateDto : TaktEmployeeCreateDto
{
    /// <summary>
    /// EmployeeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

}

// ========================================
// Employee 状态 DTO
// ========================================

/// <summary>
/// Employee 状态更新 DTO
/// </summary>
public class TaktEmployeeStatusDto
{
    /// <summary>
    /// EmployeeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工状态（1=试用期，2=正式，3=离职，4=退休）
    /// </summary>
    [Required(ErrorMessage = "员工状态（1=试用期，2=正式，3=离职，4=退休）不能为空")]
    public int EmployeeStatus { get; set; } = 0;

    /// <summary>
    /// 政治面貌（字典 hr_political_status）
    /// </summary>
    public int PoliticalStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Employee 导入模板行 DTO
/// </summary>
public class TaktEmployeeTemplateDto
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
    /// 员工编号（租户+公司内唯一）
    /// </summary>
    public string? EmployeeNo { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（0=未知，1=男，2=女）
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string? IdCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 hr_native_place_code 的 6 位 GB 行政区划代码，人事档案必填）
    /// </summary>
    public string? NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 hr_ethnic_code，1～56）
    /// </summary>
    public int? Ethnicity { get; set; }

    /// <summary>
    /// 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
    /// </summary>
    public int? Education { get; set; }

    /// <summary>
    /// 毕业院校（最高学历摘要）
    /// </summary>
    public string? GraduateSchool { get; set; } = string.Empty;

    /// <summary>
    /// 专业（最高学历摘要）
    /// </summary>
    public string? Major { get; set; } = string.Empty;

    /// <summary>
    /// 实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）
    /// </summary>
    public DateTime? JoinedDate { get; set; }

    /// <summary>
    /// 试用期结束日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 转正日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? RegularDate { get; set; }

    /// <summary>
    /// 离职日期（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 最后工作日（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 离职类型（投影字段，由离职审批通过后回写；0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
    /// </summary>
    public int? ResignationType { get; set; }

    /// <summary>
    /// 离职原因（投影字段，由离职审批通过后回写）
    /// </summary>
    public string? ResignationReason { get; set; } = string.Empty;

    /// <summary>
    /// 员工状态（1=试用期，2=正式，3=离职，4=退休）
    /// </summary>
    public int? EmployeeStatus { get; set; }

    /// <summary>
    /// 当前主部门ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryDeptId { get; set; }

    /// <summary>
    /// 当前主岗位ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryPostId { get; set; }

    /// <summary>
    /// 内置（种子员工不可删）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 紧急联系人姓名（人事档案必填）
    /// </summary>
    public string? EmergencyContactName { get; set; } = string.Empty;

    /// <summary>
    /// 紧急联系人电话（人事档案必填）
    /// </summary>
    public string? EmergencyContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 家庭住址（人事档案必填）
    /// </summary>
    public string? HomeAddress { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 政治面貌（字典 hr_political_status，0～12；人事档案必填）
    /// </summary>
    public int? PoliticalStatus { get; set; }

    /// <summary>
    /// 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶；人事档案必填）
    /// </summary>
    public int? MaritalStatus { get; set; }

    /// <summary>
    /// 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeeDeptIds { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeePostIds { get; set; }

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
/// Employee 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeImportDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 员工编号（租户+公司内唯一）
    /// </summary>
    public string? EmployeeNo { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（0=未知，1=男，2=女）
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string? IdCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 hr_native_place_code 的 6 位 GB 行政区划代码，人事档案必填）
    /// </summary>
    public string? NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 hr_ethnic_code，1～56）
    /// </summary>
    public int? Ethnicity { get; set; }

    /// <summary>
    /// 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
    /// </summary>
    public int? Education { get; set; }

    /// <summary>
    /// 毕业院校（最高学历摘要）
    /// </summary>
    public string? GraduateSchool { get; set; } = string.Empty;

    /// <summary>
    /// 专业（最高学历摘要）
    /// </summary>
    public string? Major { get; set; } = string.Empty;

    /// <summary>
    /// 实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）
    /// </summary>
    public DateTime? JoinedDate { get; set; }

    /// <summary>
    /// 试用期结束日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 转正日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? RegularDate { get; set; }

    /// <summary>
    /// 离职日期（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 最后工作日（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 离职类型（投影字段，由离职审批通过后回写；0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
    /// </summary>
    public int? ResignationType { get; set; }

    /// <summary>
    /// 离职原因（投影字段，由离职审批通过后回写）
    /// </summary>
    public string? ResignationReason { get; set; } = string.Empty;

    /// <summary>
    /// 员工状态（1=试用期，2=正式，3=离职，4=退休）
    /// </summary>
    public int? EmployeeStatus { get; set; }

    /// <summary>
    /// 当前主部门ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryDeptId { get; set; }

    /// <summary>
    /// 当前主岗位ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryPostId { get; set; }

    /// <summary>
    /// 内置（种子员工不可删）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 紧急联系人姓名（人事档案必填）
    /// </summary>
    public string? EmergencyContactName { get; set; } = string.Empty;

    /// <summary>
    /// 紧急联系人电话（人事档案必填）
    /// </summary>
    public string? EmergencyContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 家庭住址（人事档案必填）
    /// </summary>
    public string? HomeAddress { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 政治面貌（字典 hr_political_status，0～12；人事档案必填）
    /// </summary>
    public int? PoliticalStatus { get; set; }

    /// <summary>
    /// 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶；人事档案必填）
    /// </summary>
    public int? MaritalStatus { get; set; }

    /// <summary>
    /// 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeeDeptIds { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeePostIds { get; set; }

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
/// Employee 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeExportDto
{
    /// <summary>
    /// EmployeeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工编号（租户+公司内唯一）
    /// </summary>
    public string EmployeeNo { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（0=未知，1=男，2=女）
    /// </summary>
    public int Gender { get; set; } = 0;

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string IdCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 hr_native_place_code 的 6 位 GB 行政区划代码，人事档案必填）
    /// </summary>
    public string NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 hr_ethnic_code，1～56）
    /// </summary>
    public int Ethnicity { get; set; } = 0;

    /// <summary>
    /// 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
    /// </summary>
    public int? Education { get; set; }

    /// <summary>
    /// 毕业院校（最高学历摘要）
    /// </summary>
    public string? GraduateSchool { get; set; } = string.Empty;

    /// <summary>
    /// 专业（最高学历摘要）
    /// </summary>
    public string? Major { get; set; } = string.Empty;

    /// <summary>
    /// 实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）
    /// </summary>
    public DateTime? JoinedDate { get; set; }

    /// <summary>
    /// 试用期结束日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? ProbationEndDate { get; set; }

    /// <summary>
    /// 转正日期（投影字段，由上岗审批通过后回写）
    /// </summary>
    public DateTime? RegularDate { get; set; }

    /// <summary>
    /// 离职日期（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 最后工作日（投影字段，由离职审批通过后回写）
    /// </summary>
    public DateTime? LastWorkDate { get; set; }

    /// <summary>
    /// 离职类型（投影字段，由离职审批通过后回写；0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
    /// </summary>
    public int? ResignationType { get; set; }

    /// <summary>
    /// 离职原因（投影字段，由离职审批通过后回写）
    /// </summary>
    public string? ResignationReason { get; set; } = string.Empty;

    /// <summary>
    /// 员工状态（1=试用期，2=正式，3=离职，4=退休）
    /// </summary>
    public int EmployeeStatus { get; set; } = 0;

    /// <summary>
    /// 当前主部门ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryDeptId { get; set; }

    /// <summary>
    /// 当前主岗位ID（任职投影快照；未上岗可空，在岗员工由投影服务保证有值）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryPostId { get; set; }

    /// <summary>
    /// 内置（种子员工不可删）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 紧急联系人姓名（人事档案必填）
    /// </summary>
    public string EmergencyContactName { get; set; } = string.Empty;

    /// <summary>
    /// 紧急联系人电话（人事档案必填）
    /// </summary>
    public string EmergencyContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 家庭住址（人事档案必填）
    /// </summary>
    public string HomeAddress { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 政治面貌（字典 hr_political_status，0～12；人事档案必填）
    /// </summary>
    public int PoliticalStatus { get; set; } = 0;

    /// <summary>
    /// 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶；人事档案必填）
    /// </summary>
    public int MaritalStatus { get; set; } = 0;

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
