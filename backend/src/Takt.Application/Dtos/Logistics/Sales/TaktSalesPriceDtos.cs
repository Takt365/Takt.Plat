// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesPriceDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPrice 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesPrice 生成，请按需审阅）
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
// SalesPrice 响应 DTO
// ========================================

/// <summary>
/// Takt销售价格实体（客户价格主表，一个客户可以有多个物料价格）
/// 对应前端 TaktSalesPriceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesPriceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesPriceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格编码（唯一索引）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（如果为空则表示通用价格）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
    /// </summary>
    public int PriceType { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveStartDate { get; set; }

    /// <summary>
    /// 失效日期（空表示长期有效）
    /// </summary>
    public DateTime? EffectiveEndDate { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    public int PriceStatus { get; set; } = 0;

    /// <summary>
    /// 物料价格明细列表（主子表关系，一个客户价格可以有多个物料价格）
    /// （子表：TaktSalesPriceItem）
    /// </summary>
    public List<TaktSalesPriceItemDto>? Items { get; set; }

    /// <summary>
    /// 销售价格变更记录列表（外键在子表 <see cref="TaktSalesPriceChangeLog.PriceId"/>）
    /// （子表：TaktSalesPriceChangeLog）
    /// </summary>
    public List<TaktSalesPriceChangeLogDto>? ChangeLogs { get; set; }

}

// ========================================
// SalesPrice 查询 DTO
// ========================================

/// <summary>
/// SalesPrice 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesPriceQueryDto : TaktPagedQuery
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格编码（唯一索引）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（如果为空则表示通用价格）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
    /// </summary>
    public int? PriceType { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveStartDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveStartDateEnd { get; set; }

    /// <summary>
    /// 失效日期（空表示长期有效）（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveEndDateStart { get; set; }

    /// <summary>
    /// 失效日期（空表示长期有效）（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveEndDateEnd { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    public int? PriceStatus { get; set; }

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
// 创建SalesPrice DTO
// ========================================

/// <summary>
/// 创建SalesPrice DTO
/// </summary>
public class TaktSalesPriceCreateDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "销售价格编码（唯一索引）不能为空")]
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（如果为空则表示通用价格）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
    /// </summary>
    public int PriceType { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveStartDate { get; set; }

    /// <summary>
    /// 失效日期（空表示长期有效）
    /// </summary>
    public DateTime? EffectiveEndDate { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    public int PriceStatus { get; set; } = 0;

    /// <summary>
    /// 物料价格明细列表（主子表关系，一个客户价格可以有多个物料价格）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 销售价格变更记录列表（外键在子表 <see cref="TaktSalesPriceChangeLog.PriceId"/>）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceChangeLogCreateDto>? ChangeLogs { get; set; }

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
// 更新SalesPrice DTO
// ========================================

/// <summary>
/// 更新SalesPrice DTO
/// 继承 TaktSalesPriceCreateDto，添加 SalesPriceId 字段
/// </summary>
public class TaktSalesPriceUpdateDto : TaktSalesPriceCreateDto
{
    /// <summary>
    /// SalesPriceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

}

// ========================================
// SalesPrice 状态 DTO
// ========================================

/// <summary>
/// SalesPrice 状态更新 DTO
/// </summary>
public class TaktSalesPriceStatusDto
{
    /// <summary>
    /// SalesPriceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "价格状态（1=启用，0=禁用）不能为空")]
    public int PriceStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesPrice 导入模板行 DTO
/// </summary>
public class TaktSalesPriceTemplateDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格编码（唯一索引）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（如果为空则表示通用价格）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
    /// </summary>
    public int? PriceType { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    public int? PriceStatus { get; set; }

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
/// SalesPrice 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesPriceImportDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格编码（唯一索引）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（如果为空则表示通用价格）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
    /// </summary>
    public int? PriceType { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    public int? PriceStatus { get; set; }

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
/// SalesPrice 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesPriceExportDto
{
    /// <summary>
    /// SalesPriceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格编码（唯一索引）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（如果为空则表示通用价格）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
    /// </summary>
    public int PriceType { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveStartDate { get; set; }

    /// <summary>
    /// 失效日期（空表示长期有效）
    /// </summary>
    public DateTime? EffectiveEndDate { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    public int PriceStatus { get; set; } = 0;

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
