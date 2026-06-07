// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：RoutingItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktRoutingItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// RoutingItem 响应 DTO
// ========================================

/// <summary>
/// 工艺路线明细表实体（工序序列）
/// 对应前端 TaktRoutingItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktRoutingItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// RoutingItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingId { get; set; }

    /// <summary>
    /// 工艺路线主表名称（填充字段）
    /// </summary>
    public string? RoutingName { get; set; }

    /// <summary>
    /// 工艺路线编码（冗余字段，便于查询）
    /// </summary>
    public string RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 作业/工序计量单位（PC或EA）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本数量
    /// </summary>
    public decimal BaseQuantity { get; set; }

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位
    /// </summary>
    public string TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int StandardShorts { get; set; } = 0;

    /// <summary>
    /// 点数单位
    /// </summary>
    public string PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（1 点数 = 多少分钟）
    /// </summary>
    public decimal PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal ConvertedMinutes { get; set; }

    /// <summary>
    /// 准备时间（分钟），如换模、调试等
    /// </summary>
    public decimal SetupMinutes { get; set; }

    /// <summary>
    /// 清理时间（分钟），如清洁、整理等
    /// </summary>
    public decimal TeardownMinutes { get; set; }

    /// <summary>
    /// 是否质量检验点
    /// </summary>
    public bool IsQualityCheck { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工序说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线主表（主表）
    /// （主表：TaktRouting）
    /// </summary>
    public TaktRoutingDto? Routing { get; set; }

}

// ========================================
// RoutingItem 查询 DTO
// ========================================

/// <summary>
/// RoutingItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktRoutingItemQueryDto : TaktPagedQuery
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
    /// 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingId { get; set; }

    /// <summary>
    /// 工艺路线编码（冗余字段，便于查询）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 作业/工序计量单位（PC或EA）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本数量
    /// </summary>
    public decimal? BaseQuantity { get; set; }

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal? StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位
    /// </summary>
    public string? TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int? StandardShorts { get; set; }

    /// <summary>
    /// 点数单位
    /// </summary>
    public string? PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（1 点数 = 多少分钟）
    /// </summary>
    public decimal? PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal? ConvertedMinutes { get; set; }

    /// <summary>
    /// 准备时间（分钟），如换模、调试等
    /// </summary>
    public decimal? SetupMinutes { get; set; }

    /// <summary>
    /// 清理时间（分钟），如清洁、整理等
    /// </summary>
    public decimal? TeardownMinutes { get; set; }

    /// <summary>
    /// 是否质量检验点
    /// </summary>
    public bool? IsQualityCheck { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 工序说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

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
// 创建RoutingItem DTO
// ========================================

/// <summary>
/// 创建RoutingItem DTO
/// </summary>
public class TaktRoutingItemCreateDto
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
    /// 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingId { get; set; }

    /// <summary>
    /// 工艺路线编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "工艺路线编码（冗余字段，便于查询）不能为空")]
    public string RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 作业/工序计量单位（PC或EA）
    /// </summary>
    [Required(ErrorMessage = "作业/工序计量单位（PC或EA）不能为空")]
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本数量
    /// </summary>
    public decimal BaseQuantity { get; set; }

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位
    /// </summary>
    [Required(ErrorMessage = "工时单位不能为空")]
    public string TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int StandardShorts { get; set; } = 0;

    /// <summary>
    /// 点数单位
    /// </summary>
    [Required(ErrorMessage = "点数单位不能为空")]
    public string PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（1 点数 = 多少分钟）
    /// </summary>
    public decimal PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal ConvertedMinutes { get; set; }

    /// <summary>
    /// 准备时间（分钟），如换模、调试等
    /// </summary>
    public decimal SetupMinutes { get; set; }

    /// <summary>
    /// 清理时间（分钟），如清洁、整理等
    /// </summary>
    public decimal TeardownMinutes { get; set; }

    /// <summary>
    /// 是否质量检验点
    /// </summary>
    public bool IsQualityCheck { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工序说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

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
// 更新RoutingItem DTO
// ========================================

/// <summary>
/// 更新RoutingItem DTO
/// 继承 TaktRoutingItemCreateDto，添加 RoutingItemId 字段
/// </summary>
public class TaktRoutingItemUpdateDto : TaktRoutingItemCreateDto
{
    /// <summary>
    /// RoutingItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

}

// ========================================
// RoutingItem 排序 DTO
// ========================================

/// <summary>
/// RoutingItem 排序更新 DTO
/// </summary>
public class TaktRoutingItemSortDto
{
    /// <summary>
    /// RoutingItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

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
/// RoutingItem 导入模板行 DTO
/// </summary>
public class TaktRoutingItemTemplateDto
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
    /// 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingId { get; set; }

    /// <summary>
    /// 工艺路线编码（冗余字段，便于查询）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 作业/工序计量单位（PC或EA）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 工时单位
    /// </summary>
    public string? TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int? StandardShorts { get; set; }

    /// <summary>
    /// 点数单位
    /// </summary>
    public string? PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 工序说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

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
/// RoutingItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktRoutingItemImportDto
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
    /// 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingId { get; set; }

    /// <summary>
    /// 工艺路线编码（冗余字段，便于查询）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 作业/工序计量单位（PC或EA）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 工时单位
    /// </summary>
    public string? TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int? StandardShorts { get; set; }

    /// <summary>
    /// 点数单位
    /// </summary>
    public string? PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 工序说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

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
/// RoutingItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktRoutingItemExportDto
{
    /// <summary>
    /// RoutingItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingId { get; set; }

    /// <summary>
    /// 工艺路线编码（冗余字段，便于查询）
    /// </summary>
    public string RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 作业/工序计量单位（PC或EA）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本数量
    /// </summary>
    public decimal BaseQuantity { get; set; }

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位
    /// </summary>
    public string TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int StandardShorts { get; set; } = 0;

    /// <summary>
    /// 点数单位
    /// </summary>
    public string PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（1 点数 = 多少分钟）
    /// </summary>
    public decimal PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal ConvertedMinutes { get; set; }

    /// <summary>
    /// 准备时间（分钟），如换模、调试等
    /// </summary>
    public decimal SetupMinutes { get; set; }

    /// <summary>
    /// 清理时间（分钟），如清洁、整理等
    /// </summary>
    public decimal TeardownMinutes { get; set; }

    /// <summary>
    /// 是否质量检验点
    /// </summary>
    public bool IsQualityCheck { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工序说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

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
