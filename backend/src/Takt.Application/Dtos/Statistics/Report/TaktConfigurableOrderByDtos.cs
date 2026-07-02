// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Statistics.Report
// 文件名称：TaktConfigurableOrderByDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：ConfigurableOrderBy 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConfigurableOrderBy 生成，请按需审阅）
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
// ConfigurableOrderBy 响应 DTO
// ========================================

/// <summary>
/// 自定义报表排序字段定义
/// 对应前端 TaktConfigurableOrderByDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConfigurableOrderByDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConfigurableOrderByID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableOrderById { get; set; }

    /// <summary>
    /// 关联报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 关联报表主表 名称（填充字段）
    /// </summary>
    public string? ConfigurableName { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（升序/降序）
    /// </summary>
    public int SortDirection { get; set; }

    /// <summary>
    /// 排序号（ORDER BY 优先级）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 关联的报表主表
    /// （主表：TaktConfigurable）
    /// </summary>
    public TaktConfigurableDto? Configurable { get; set; }

}

// ========================================
// ConfigurableOrderBy 查询 DTO
// ========================================

/// <summary>
/// ConfigurableOrderBy 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConfigurableOrderByQueryDto : TaktPagedQuery
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
    /// 关联报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string? SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string? ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（升序/降序）
    /// </summary>
    public int? SortDirection { get; set; }

    /// <summary>
    /// 排序号（ORDER BY 优先级）
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
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建ConfigurableOrderBy DTO
// ========================================

/// <summary>
/// 创建ConfigurableOrderBy DTO
/// </summary>
public class TaktConfigurableOrderByCreateDto
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
    /// 关联报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    [Required(ErrorMessage = "数据源别名不能为空")]
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    [Required(ErrorMessage = "列名不能为空")]
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（升序/降序）
    /// </summary>
    public int SortDirection { get; set; }

    /// <summary>
    /// 排序号（ORDER BY 优先级）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
// 更新ConfigurableOrderBy DTO
// ========================================

/// <summary>
/// 更新ConfigurableOrderBy DTO
/// 继承 TaktConfigurableOrderByCreateDto，添加 ConfigurableOrderById 字段
/// </summary>
public class TaktConfigurableOrderByUpdateDto : TaktConfigurableOrderByCreateDto
{
    /// <summary>
    /// ConfigurableOrderByID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableOrderById { get; set; }

}

// ========================================
// ConfigurableOrderBy 排序 DTO
// ========================================

/// <summary>
/// ConfigurableOrderBy 排序更新 DTO
/// </summary>
public class TaktConfigurableOrderBySortDto
{
    /// <summary>
    /// ConfigurableOrderByID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableOrderById { get; set; }

    /// <summary>
    /// 排序号（ORDER BY 优先级）
    /// </summary>
    [Required(ErrorMessage = "排序号（ORDER BY 优先级）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ConfigurableOrderBy 导入模板行 DTO
/// </summary>
public class TaktConfigurableOrderByTemplateDto
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
    /// 关联报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string? SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string? ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（升序/降序）
    /// </summary>
    public int? SortDirection { get; set; }

    /// <summary>
    /// 排序号（ORDER BY 优先级）
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
/// ConfigurableOrderBy 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConfigurableOrderByImportDto
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
    /// 关联报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string? SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string? ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（升序/降序）
    /// </summary>
    public int? SortDirection { get; set; }

    /// <summary>
    /// 排序号（ORDER BY 优先级）
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
/// ConfigurableOrderBy 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConfigurableOrderByExportDto
{
    /// <summary>
    /// ConfigurableOrderByID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableOrderById { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（升序/降序）
    /// </summary>
    public int SortDirection { get; set; }

    /// <summary>
    /// 排序号（ORDER BY 优先级）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
