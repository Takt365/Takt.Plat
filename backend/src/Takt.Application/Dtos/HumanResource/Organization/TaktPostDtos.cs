// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Organization
// 文件名称：TaktPostDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：Post 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPost 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.HumanResource.Organization;

// ========================================
// Post 响应 DTO
// ========================================

/// <summary>
/// 岗位实体 代表组织架构中的岗位/职位 参照 SAP Position (STELL) 设计
/// 对应前端 TaktPostDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPostDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PostID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（填充字段）
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）
    /// </summary>
    public TaktPostType PostType { get; set; }

    /// <summary>
    /// 岗位职级（0=一线/基层，1=技术/骨干层，2=管理/决策层）
    /// </summary>
    public TaktPostLevel? PostLevel { get; set; }

    /// <summary>
    /// 编制人数
    /// </summary>
    public int Headcount { get; set; } = 0;

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int CurrentCount { get; set; } = 0;

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string? Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string? Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）
    /// </summary>
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public TaktCommonStatus PostStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子岗位为内置，不允许删除
    /// </summary>
    public TaktYesNo IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）
    /// （子表：TaktEmployeePost）
    /// </summary>
    public List<TaktEmployeePostDto>? EmployeePosts { get; set; }

}

// ========================================
// Post 查询 DTO
// ========================================

/// <summary>
/// Post 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPostQueryDto : TaktPagedQuery
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
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string? PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）
    /// </summary>
    public TaktPostType? PostType { get; set; }

    /// <summary>
    /// 岗位职级（0=一线/基层，1=技术/骨干层，2=管理/决策层）
    /// </summary>
    public TaktPostLevel? PostLevel { get; set; }

    /// <summary>
    /// 编制人数
    /// </summary>
    public int? Headcount { get; set; }

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int? CurrentCount { get; set; }

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string? Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string? Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）
    /// </summary>
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public TaktCommonStatus? PostStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子岗位为内置，不允许删除
    /// </summary>
    public TaktYesNo? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

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
// 创建Post DTO
// ========================================

/// <summary>
/// 创建Post DTO
/// </summary>
public class TaktPostCreateDto
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
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    [Required(ErrorMessage = "岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）不能为空")]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    [Required(ErrorMessage = "岗位名称不能为空")]
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）
    /// </summary>
    public TaktPostType PostType { get; set; }

    /// <summary>
    /// 岗位职级（0=一线/基层，1=技术/骨干层，2=管理/决策层）
    /// </summary>
    public TaktPostLevel? PostLevel { get; set; }

    /// <summary>
    /// 编制人数
    /// </summary>
    public int Headcount { get; set; } = 0;

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int CurrentCount { get; set; } = 0;

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string? Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string? Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）
    /// </summary>
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public TaktCommonStatus PostStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子岗位为内置，不允许删除
    /// </summary>
    public TaktYesNo IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 关联该岗位的员工 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? EmployeeIds { get; set; }

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
// 更新Post DTO
// ========================================

/// <summary>
/// 更新Post DTO
/// 继承 TaktPostCreateDto，添加 PostId 字段
/// </summary>
public class TaktPostUpdateDto : TaktPostCreateDto
{
    /// <summary>
    /// PostID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

}

// ========================================
// Post 状态 DTO
// ========================================

/// <summary>
/// Post 状态更新 DTO
/// </summary>
public class TaktPostStatusDto
{
    /// <summary>
    /// PostID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（1=启用，0=禁用）不能为空")]
    public TaktCommonStatus PostStatus { get; set; }
}

// ========================================
// Post 排序 DTO
// ========================================

/// <summary>
/// Post 排序更新 DTO
/// </summary>
public class TaktPostSortDto
{
    /// <summary>
    /// PostID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Post 导入模板行 DTO
/// </summary>
public class TaktPostTemplateDto
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
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string? PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）
    /// </summary>
    public TaktPostType? PostType { get; set; }

    /// <summary>
    /// 岗位职级（0=一线/基层，1=技术/骨干层，2=管理/决策层）
    /// </summary>
    public TaktPostLevel? PostLevel { get; set; }

    /// <summary>
    /// 编制人数
    /// </summary>
    public int? Headcount { get; set; }

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int? CurrentCount { get; set; }

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string? Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string? Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）
    /// </summary>
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public TaktCommonStatus? PostStatus { get; set; }

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
/// Post 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPostImportDto
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
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string? PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）
    /// </summary>
    public TaktPostType? PostType { get; set; }

    /// <summary>
    /// 岗位职级（0=一线/基层，1=技术/骨干层，2=管理/决策层）
    /// </summary>
    public TaktPostLevel? PostLevel { get; set; }

    /// <summary>
    /// 编制人数
    /// </summary>
    public int? Headcount { get; set; }

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int? CurrentCount { get; set; }

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string? Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string? Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）
    /// </summary>
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public TaktCommonStatus? PostStatus { get; set; }

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
/// Post 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPostExportDto
{
    /// <summary>
    /// PostID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）
    /// </summary>
    public TaktPostType PostType { get; set; }

    /// <summary>
    /// 岗位职级（0=一线/基层，1=技术/骨干层，2=管理/决策层）
    /// </summary>
    public TaktPostLevel? PostLevel { get; set; }

    /// <summary>
    /// 编制人数
    /// </summary>
    public int Headcount { get; set; } = 0;

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int CurrentCount { get; set; } = 0;

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string? Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string? Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）
    /// </summary>
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public TaktCommonStatus PostStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子岗位为内置，不允许删除
    /// </summary>
    public TaktYesNo IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

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
