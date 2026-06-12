// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Statistics.Report
// 文件名称：TaktConfigurableDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Configurable 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConfigurable 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Statistics.Report;

// ========================================
// Configurable 响应 DTO
// ========================================

/// <summary>
/// 自定义报表主实体（对标 SAP QuickViewer / SQVI 查询定义）
/// 对应前端 TaktConfigurableDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConfigurableDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConfigurableID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 报表编码（租户+公司内唯一）
    /// </summary>
    public string ReportCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表名称
    /// </summary>
    public string ReportName { get; set; } = string.Empty;

    /// <summary>
    /// 报表业务域（财务/人力/后勤等）
    /// </summary>
    public int ReportDomain { get; set; }

    /// <summary>
    /// 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ReportSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int DistinctRows { get; set; }

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int MaxExportRows { get; set; } = 0;

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int MaxQueryRows { get; set; } = 0;

    /// <summary>
    /// 归属用户 ID（为空表示公司级共享报表）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 归属用户 名称（填充字段）
    /// </summary>
    public string? OwnerUserName { get; set; }

    /// <summary>
    /// 是否内置（内置报表禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 报表状态（0=禁用 1=启用）
    /// </summary>
    public int ReportStatus { get; set; }

    /// <summary>
    /// 报表描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 数据源表列表（FROM）
    /// （子表：TaktConfigurableSource）
    /// </summary>
    public List<TaktConfigurableSourceDto>? Sources { get; set; }

    /// <summary>
    /// 多表关联列表（JOIN）
    /// （子表：TaktConfigurableJoin）
    /// </summary>
    public List<TaktConfigurableJoinDto>? Joins { get; set; }

    /// <summary>
    /// 输出字段列表（SELECT）
    /// （子表：TaktConfigurableField）
    /// </summary>
    public List<TaktConfigurableFieldDto>? Fields { get; set; }

    /// <summary>
    /// 筛选条件列表（Selection Screen / WHERE）
    /// （子表：TaktConfigurableSelection）
    /// </summary>
    public List<TaktConfigurableSelectionDto>? Selections { get; set; }

    /// <summary>
    /// 分组字段列表（GROUP BY）
    /// （子表：TaktConfigurableGroupBy）
    /// </summary>
    public List<TaktConfigurableGroupByDto>? GroupBys { get; set; }

    /// <summary>
    /// 排序字段列表（ORDER BY）
    /// （子表：TaktConfigurableOrderBy）
    /// </summary>
    public List<TaktConfigurableOrderByDto>? OrderBys { get; set; }

}

// ========================================
// Configurable 查询 DTO
// ========================================

/// <summary>
/// Configurable 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConfigurableQueryDto : TaktPagedQuery
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
    /// 报表编码（租户+公司内唯一）
    /// </summary>
    public string? ReportCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表名称
    /// </summary>
    public string? ReportName { get; set; } = string.Empty;

    /// <summary>
    /// 报表业务域（财务/人力/后勤等）
    /// </summary>
    public int? ReportDomain { get; set; }

    /// <summary>
    /// 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ReportSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int? DistinctRows { get; set; }

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int? MaxExportRows { get; set; }

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int? MaxQueryRows { get; set; }

    /// <summary>
    /// 归属用户 ID（为空表示公司级共享报表）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 是否内置（内置报表禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 报表状态（0=禁用 1=启用）
    /// </summary>
    public int? ReportStatus { get; set; }

    /// <summary>
    /// 报表描述
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
// 创建Configurable DTO
// ========================================

/// <summary>
/// 创建Configurable DTO
/// </summary>
public class TaktConfigurableCreateDto
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
    /// 报表编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "报表编码（租户+公司内唯一）不能为空")]
    public string ReportCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表名称
    /// </summary>
    [Required(ErrorMessage = "报表名称不能为空")]
    public string ReportName { get; set; } = string.Empty;

    /// <summary>
    /// 报表业务域（财务/人力/后勤等）
    /// </summary>
    public int ReportDomain { get; set; }

    /// <summary>
    /// 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ReportSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int DistinctRows { get; set; }

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int MaxExportRows { get; set; } = 0;

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int MaxQueryRows { get; set; } = 0;

    /// <summary>
    /// 归属用户 ID（为空表示公司级共享报表）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 是否内置（内置报表禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 报表状态（0=禁用 1=启用）
    /// </summary>
    public int ReportStatus { get; set; }

    /// <summary>
    /// 报表描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 数据源表列表（FROM）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableSourceCreateDto>? Sources { get; set; }

    /// <summary>
    /// 多表关联列表（JOIN）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableJoinCreateDto>? Joins { get; set; }

    /// <summary>
    /// 输出字段列表（SELECT）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableFieldCreateDto>? Fields { get; set; }

    /// <summary>
    /// 筛选条件列表（Selection Screen / WHERE）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableSelectionCreateDto>? Selections { get; set; }

    /// <summary>
    /// 分组字段列表（GROUP BY）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableGroupByCreateDto>? GroupBys { get; set; }

    /// <summary>
    /// 排序字段列表（ORDER BY）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableOrderByCreateDto>? OrderBys { get; set; }

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
// 更新Configurable DTO
// ========================================

/// <summary>
/// 更新Configurable DTO
/// 继承 TaktConfigurableCreateDto，添加 ConfigurableId 字段
/// </summary>
public class TaktConfigurableUpdateDto : TaktConfigurableCreateDto
{
    /// <summary>
    /// ConfigurableID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

}

// ========================================
// Configurable 状态 DTO
// ========================================

/// <summary>
/// Configurable 状态更新 DTO
/// </summary>
public class TaktConfigurableStatusDto
{
    /// <summary>
    /// ConfigurableID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 报表状态（0=禁用 1=启用）
    /// </summary>
    [Required(ErrorMessage = "报表状态（0=禁用 1=启用）不能为空")]
    public int ReportStatus { get; set; }
}

// ========================================
// Configurable 排序 DTO
// ========================================

/// <summary>
/// Configurable 排序更新 DTO
/// </summary>
public class TaktConfigurableSortDto
{
    /// <summary>
    /// ConfigurableID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

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
/// Configurable 导入模板行 DTO
/// </summary>
public class TaktConfigurableTemplateDto
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
    /// 报表编码（租户+公司内唯一）
    /// </summary>
    public string? ReportCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表名称
    /// </summary>
    public string? ReportName { get; set; } = string.Empty;

    /// <summary>
    /// 报表业务域（财务/人力/后勤等）
    /// </summary>
    public int? ReportDomain { get; set; }

    /// <summary>
    /// 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ReportSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int? DistinctRows { get; set; }

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int? MaxExportRows { get; set; }

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int? MaxQueryRows { get; set; }

    /// <summary>
    /// 归属用户 ID（为空表示公司级共享报表）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 是否内置（内置报表禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 报表状态（0=禁用 1=启用）
    /// </summary>
    public int? ReportStatus { get; set; }

    /// <summary>
    /// 报表描述
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

}

/// <summary>
/// Configurable 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConfigurableImportDto
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
    /// 报表编码（租户+公司内唯一）
    /// </summary>
    public string? ReportCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表名称
    /// </summary>
    public string? ReportName { get; set; } = string.Empty;

    /// <summary>
    /// 报表业务域（财务/人力/后勤等）
    /// </summary>
    public int? ReportDomain { get; set; }

    /// <summary>
    /// 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ReportSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int? DistinctRows { get; set; }

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int? MaxExportRows { get; set; }

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int? MaxQueryRows { get; set; }

    /// <summary>
    /// 归属用户 ID（为空表示公司级共享报表）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 是否内置（内置报表禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 报表状态（0=禁用 1=启用）
    /// </summary>
    public int? ReportStatus { get; set; }

    /// <summary>
    /// 报表描述
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

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Configurable 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConfigurableExportDto
{
    /// <summary>
    /// ConfigurableID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表编码（租户+公司内唯一）
    /// </summary>
    public string ReportCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表名称
    /// </summary>
    public string ReportName { get; set; } = string.Empty;

    /// <summary>
    /// 报表业务域（财务/人力/后勤等）
    /// </summary>
    public int ReportDomain { get; set; }

    /// <summary>
    /// 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ReportSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int DistinctRows { get; set; }

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int MaxExportRows { get; set; } = 0;

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int MaxQueryRows { get; set; } = 0;

    /// <summary>
    /// 归属用户 ID（为空表示公司级共享报表）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 是否内置（内置报表禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 报表状态（0=禁用 1=启用）
    /// </summary>
    public int ReportStatus { get; set; }

    /// <summary>
    /// 报表描述
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
