// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesQuotationChangeLogDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesQuotationChangeLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesQuotationChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Sales;

// ========================================
// SalesQuotationChangeLog 响应 DTO
// ========================================

/// <summary>
/// 销售报价变更记录实体
/// 对应前端 TaktSalesQuotationChangeLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesQuotationChangeLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesQuotationChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationChangeLogId { get; set; }

    /// <summary>
    /// 销售报价（关联 TaktSalesQuotation.Id，选项 TaktSalesQuotations/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationId { get; set; }

    /// <summary>
    /// 销售报价（关联 TaktSalesQuotation.Id，选项 TaktSalesQuotations/options）
    /// </summary>
    public string? SalesQuotationName { get; set; }

    /// <summary>
    /// 销售报价编码
    /// </summary>
    public string SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 销售报价主表
    /// （主表：TaktSalesQuotation）
    /// </summary>
    public TaktSalesQuotationDto? SalesQuotation { get; set; }

}

// ========================================
// SalesQuotationChangeLog 查询 DTO
// ========================================

/// <summary>
/// SalesQuotationChangeLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesQuotationChangeLogQueryDto : TaktPagedQuery
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
    /// 销售报价（关联 TaktSalesQuotation.Id，选项 TaktSalesQuotations/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesQuotationId { get; set; }

    /// <summary>
    /// 销售报价编码
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间（范围查询-开始）
    /// </summary>
    public DateTime? ChangeTimeStart { get; set; }

    /// <summary>
    /// 变更时间（范围查询-结束）
    /// </summary>
    public DateTime? ChangeTimeEnd { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

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
// 创建SalesQuotationChangeLog DTO
// ========================================

/// <summary>
/// 创建SalesQuotationChangeLog DTO
/// </summary>
public class TaktSalesQuotationChangeLogCreateDto
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
    /// 销售报价（关联 TaktSalesQuotation.Id，选项 TaktSalesQuotations/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationId { get; set; }

    /// <summary>
    /// 销售报价编码
    /// </summary>
    [Required(ErrorMessage = "销售报价编码不能为空")]
    public string SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

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
// 更新SalesQuotationChangeLog DTO
// ========================================

/// <summary>
/// 更新SalesQuotationChangeLog DTO
/// 继承 TaktSalesQuotationChangeLogCreateDto，添加 SalesQuotationChangeLogId 字段
/// </summary>
public class TaktSalesQuotationChangeLogUpdateDto : TaktSalesQuotationChangeLogCreateDto
{
    /// <summary>
    /// SalesQuotationChangeLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationChangeLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// SalesQuotationChangeLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesQuotationChangeLogExportDto
{
    /// <summary>
    /// SalesQuotationChangeLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationChangeLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售报价（关联 TaktSalesQuotation.Id，选项 TaktSalesQuotations/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationId { get; set; }

    /// <summary>
    /// 销售报价编码
    /// </summary>
    public string SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

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
