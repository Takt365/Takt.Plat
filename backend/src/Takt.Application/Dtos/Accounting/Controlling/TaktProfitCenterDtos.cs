// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Accounting.Controlling
// 文件名称：TaktProfitCenterDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：ProfitCenter 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProfitCenter 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Controlling;

// ========================================
// ProfitCenter 响应 DTO
// ========================================

/// <summary>
/// 利润中心实体
/// 对应前端 TaktProfitCenterDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProfitCenterDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProfitCenterID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitCenterId { get; set; }

    /// <summary>
    /// 利润中心编码
    /// </summary>
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心名称
    /// </summary>
    public string ProfitCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 负责人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心层级
    /// </summary>
    public int ProfitCenterLevel { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心状态（1=启用，0=禁用）
    /// </summary>
    public int ProfitCenterStatus { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// ProfitCenter 树形响应 DTO
// ========================================

/// <summary>
/// ProfitCenter 树形列表/树选择 DTO（含子节点）
/// 对应 GetProfitCenterTreeAsync 等接口
/// </summary>
public class TaktProfitCenterTreeDto : TaktProfitCenterDto
{
    /// <summary>
    /// 子节点
    /// </summary>
    public List<TaktProfitCenterTreeDto> Children { get; set; } = new();
}

// ========================================
// ProfitCenter 查询 DTO
// ========================================

/// <summary>
/// ProfitCenter 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProfitCenterQueryDto : TaktPagedQuery
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
    /// 利润中心编码
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心名称
    /// </summary>
    public string? ProfitCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 负责人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心层级
    /// </summary>
    public int? ProfitCenterLevel { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心状态（1=启用，0=禁用）
    /// </summary>
    public int? ProfitCenterStatus { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidFromStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidFromEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidToStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidToEnd { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建ProfitCenter DTO
// ========================================

/// <summary>
/// 创建ProfitCenter DTO
/// </summary>
public class TaktProfitCenterCreateDto
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
    /// 利润中心编码
    /// </summary>
    [Required(ErrorMessage = "利润中心编码不能为空")]
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心名称
    /// </summary>
    [Required(ErrorMessage = "利润中心名称不能为空")]
    public string ProfitCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 负责人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心层级
    /// </summary>
    public int ProfitCenterLevel { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心状态（1=启用，0=禁用）
    /// </summary>
    public int ProfitCenterStatus { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
// 更新ProfitCenter DTO
// ========================================

/// <summary>
/// 更新ProfitCenter DTO
/// 继承 TaktProfitCenterCreateDto，添加 ProfitCenterId 字段
/// </summary>
public class TaktProfitCenterUpdateDto : TaktProfitCenterCreateDto
{
    /// <summary>
    /// ProfitCenterID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitCenterId { get; set; }

}

// ========================================
// ProfitCenter 状态 DTO
// ========================================

/// <summary>
/// ProfitCenter 状态更新 DTO
/// </summary>
public class TaktProfitCenterStatusDto
{
    /// <summary>
    /// ProfitCenterID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitCenterId { get; set; }

    /// <summary>
    /// 利润中心状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "利润中心状态（1=启用，0=禁用）不能为空")]
    public int ProfitCenterStatus { get; set; } = 0;
}

// ========================================
// ProfitCenter 排序 DTO
// ========================================

/// <summary>
/// ProfitCenter 排序更新 DTO
/// </summary>
public class TaktProfitCenterSortDto
{
    /// <summary>
    /// ProfitCenterID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitCenterId { get; set; }

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
/// ProfitCenter 导入模板行 DTO
/// </summary>
public class TaktProfitCenterTemplateDto
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
    /// 利润中心编码
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心名称
    /// </summary>
    public string? ProfitCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 负责人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心层级
    /// </summary>
    public int? ProfitCenterLevel { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心状态（1=启用，0=禁用）
    /// </summary>
    public int? ProfitCenterStatus { get; set; }

    /// <summary>
    /// 排序号
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
/// ProfitCenter 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProfitCenterImportDto
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
    /// 利润中心编码
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心名称
    /// </summary>
    public string? ProfitCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 负责人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心层级
    /// </summary>
    public int? ProfitCenterLevel { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心状态（1=启用，0=禁用）
    /// </summary>
    public int? ProfitCenterStatus { get; set; }

    /// <summary>
    /// 排序号
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
/// ProfitCenter 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProfitCenterExportDto
{
    /// <summary>
    /// ProfitCenterID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitCenterId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心编码
    /// </summary>
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心名称
    /// </summary>
    public string ProfitCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 负责人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerId { get; set; }

    /// <summary>
    /// 负责人姓名
    /// </summary>
    public string? ManagerName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心层级
    /// </summary>
    public int ProfitCenterLevel { get; set; } = 0;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心状态（1=启用，0=禁用）
    /// </summary>
    public int ProfitCenterStatus { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
