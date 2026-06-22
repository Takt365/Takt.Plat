// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.HumanResource.Organization
// 文件名称：TaktDeptDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Dept 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDept 生成，请按需审阅）
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
// Dept 响应 DTO
// ========================================

/// <summary>
/// 部门实体 代表组织架构中的部门（树形结构） 参照 SAP Organizational Unit (ORGEH) 设计
/// 对应前端 TaktDeptDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktDeptDto : TaktCompanyDtoBase
{
    /// <summary>
    /// DeptID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 父部门ID（0表示根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 层级（1=一级部门，2=二级部门，以此类推）
    /// </summary>
    public int Level { get; set; } = 0;

    /// <summary>
    /// 部门路径（如：/1/3/5/，用于快速查询子部门）
    /// </summary>
    public string DeptPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（0=否，1=是）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// 成本中心编码（关联财务成本中心）
    /// </summary>
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（1=直接，2=间接）
    /// </summary>
    public int CostCategory { get; set; }

    /// <summary>
    /// 部门负责人ID（关联TaktUser.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HeadUserId { get; set; }

    /// <summary>
    /// 部门负责人名称（填充字段）
    /// </summary>
    public string? HeadUserName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int DeptStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子部门为内置，不允许删除
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（同级部门排序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 部门描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 角色数据权限关联该部门（RBAC，表 takt_human_resource_organization_roledept）
    /// （子表：TaktRoleDept）
    /// </summary>
    public List<TaktRoleDeptDto>? RoleDepts { get; set; }

    /// <summary>
    /// 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）
    /// （子表：TaktEmployeeDept）
    /// </summary>
    public List<TaktEmployeeDeptDto>? EmployeeDepts { get; set; }

}

// ========================================
// Dept 树形响应 DTO
// ========================================

/// <summary>
/// Dept 树形列表/树选择 DTO（含子节点）
/// 对应 GetDeptTreeAsync 等接口
/// </summary>
public class TaktDeptTreeDto : TaktDeptDto
{
    /// <summary>
    /// 子节点
    /// </summary>
    public List<TaktDeptTreeDto> Children { get; set; } = new();
}

// ========================================
// Dept 查询 DTO
// ========================================

/// <summary>
/// Dept 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDeptQueryDto : TaktPagedQuery
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
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 父部门ID（0表示根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 层级（1=一级部门，2=二级部门，以此类推）
    /// </summary>
    public int? Level { get; set; }

    /// <summary>
    /// 部门路径（如：/1/3/5/，用于快速查询子部门）
    /// </summary>
    public string? DeptPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（0=否，1=是）
    /// </summary>
    public int? IsLeaf { get; set; }

    /// <summary>
    /// 成本中心编码（关联财务成本中心）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（1=直接，2=间接）
    /// </summary>
    public int? CostCategory { get; set; }

    /// <summary>
    /// 部门负责人ID（关联TaktUser.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HeadUserId { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int? DeptStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子部门为内置，不允许删除
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（同级部门排序）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 部门描述
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
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建Dept DTO
// ========================================

/// <summary>
/// 创建Dept DTO
/// </summary>
public class TaktDeptCreateDto
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
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    [Required(ErrorMessage = "部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称
    /// </summary>
    [Required(ErrorMessage = "部门名称不能为空")]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 父部门ID（0表示根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 成本中心编码（关联财务成本中心）
    /// </summary>
    [Required(ErrorMessage = "成本中心编码（关联财务成本中心）不能为空")]
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（1=直接，2=间接）
    /// </summary>
    public int CostCategory { get; set; }

    /// <summary>
    /// 部门负责人ID（关联TaktUser.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HeadUserId { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [Required(ErrorMessage = "联系电话不能为空")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    [Required(ErrorMessage = "办公地点不能为空")]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int DeptStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子部门为内置，不允许删除
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（同级部门排序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 部门描述
    /// </summary>
    [Required(ErrorMessage = "部门描述不能为空")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 数据权限关联该部门的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 关联该部门的员工 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? EmployeeIds { get; set; }

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
// 更新Dept DTO
// ========================================

/// <summary>
/// 更新Dept DTO
/// 继承 TaktDeptCreateDto，添加 DeptId 字段
/// </summary>
public class TaktDeptUpdateDto : TaktDeptCreateDto
{
    /// <summary>
    /// DeptID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

}

// ========================================
// Dept 状态 DTO
// ========================================

/// <summary>
/// Dept 状态更新 DTO
/// </summary>
public class TaktDeptStatusDto
{
    /// <summary>
    /// DeptID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（1=启用，0=禁用）不能为空")]
    public int DeptStatus { get; set; }
}

// ========================================
// Dept 是否内置 DTO
// ========================================

/// <summary>
/// Dept 是否内置更新 DTO
/// </summary>
public class TaktDeptBuiltInDto
{
    /// <summary>
    /// DeptID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 是否内置（字典 sys_yes_no_type；1=是，0=否）
    /// </summary>
    [Required(ErrorMessage = "是否内置不能为空")]
    public int IsBuiltIn { get; set; }
}

// ========================================
// Dept 排序 DTO
// ========================================

/// <summary>
/// Dept 排序更新 DTO
/// </summary>
public class TaktDeptSortDto
{
    /// <summary>
    /// DeptID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 排序号（同级部门排序）
    /// </summary>
    [Required(ErrorMessage = "排序号（同级部门排序）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Dept 导入模板行 DTO
/// </summary>
public class TaktDeptTemplateDto
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
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 父部门ID（0表示根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 成本中心编码（关联财务成本中心）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（1=直接，2=间接）
    /// </summary>
    public int? CostCategory { get; set; }

    /// <summary>
    /// 部门负责人ID（关联TaktUser.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HeadUserId { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int? DeptStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子部门为内置，不允许删除
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（同级部门排序）
    /// </summary>
    public int? SortOrder { get; set; }

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
/// Dept 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktDeptImportDto
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
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 父部门ID（0表示根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 成本中心编码（关联财务成本中心）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（1=直接，2=间接）
    /// </summary>
    public int? CostCategory { get; set; }

    /// <summary>
    /// 部门负责人ID（关联TaktUser.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HeadUserId { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int? DeptStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子部门为内置，不允许删除
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（同级部门排序）
    /// </summary>
    public int? SortOrder { get; set; }

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
/// Dept 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDeptExportDto
{
    /// <summary>
    /// DeptID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 父部门ID（0表示根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 层级（1=一级部门，2=二级部门，以此类推）
    /// </summary>
    public int Level { get; set; } = 0;

    /// <summary>
    /// 部门路径（如：/1/3/5/，用于快速查询子部门）
    /// </summary>
    public string DeptPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（0=否，1=是）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// 成本中心编码（关联财务成本中心）
    /// </summary>
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（1=直接，2=间接）
    /// </summary>
    public int CostCategory { get; set; }

    /// <summary>
    /// 部门负责人ID（关联TaktUser.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HeadUserId { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int DeptStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子部门为内置，不允许删除
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（同级部门排序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 部门描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

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
