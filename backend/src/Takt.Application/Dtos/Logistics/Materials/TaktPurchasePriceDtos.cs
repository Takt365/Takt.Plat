// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktPurchasePriceDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchasePrice 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchasePrice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// PurchasePrice 响应 DTO
// ========================================

/// <summary>
/// Takt采购价格实体（供应商价格主表，一个供应商可以有多个物料价格）
/// 对应前端 TaktPurchasePriceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchasePriceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchasePriceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceId { get; set; }

    /// <summary>
    /// 工厂代码（不可空）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格编码（唯一索引）
    /// </summary>
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
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
    public TaktCommonStatus PriceStatus { get; set; }

    /// <summary>
    /// 物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）
    /// （子表：TaktPurchasePriceItem）
    /// </summary>
    public List<TaktPurchasePriceItemDto>? Items { get; set; }

    /// <summary>
    /// 采购价格变更记录列表（外键在子表 <see cref="TaktPurchasePriceChangeLog.PriceId"/>）
    /// （子表：TaktPurchasePriceChangeLog）
    /// </summary>
    public List<TaktPurchasePriceChangeLogDto>? ChangeLogs { get; set; }

}

// ========================================
// PurchasePrice 查询 DTO
// ========================================

/// <summary>
/// PurchasePrice 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchasePriceQueryDto : TaktPagedQuery
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
    /// 工厂代码（不可空）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格编码（唯一索引）
    /// </summary>
    public string? PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
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
    public TaktCommonStatus? PriceStatus { get; set; }

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
// 创建PurchasePrice DTO
// ========================================

/// <summary>
/// 创建PurchasePrice DTO
/// </summary>
public class TaktPurchasePriceCreateDto
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
    /// 工厂代码（不可空）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（不可空）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "采购价格编码（唯一索引）不能为空")]
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    [Required(ErrorMessage = "供应商编码不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
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
    public TaktCommonStatus PriceStatus { get; set; }

    /// <summary>
    /// 物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）（子表，级联保存）
    /// </summary>
    public List<TaktPurchasePriceItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 采购价格变更记录列表（外键在子表 <see cref="TaktPurchasePriceChangeLog.PriceId"/>）（子表，级联保存）
    /// </summary>
    public List<TaktPurchasePriceChangeLogCreateDto>? ChangeLogs { get; set; }

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
// 更新PurchasePrice DTO
// ========================================

/// <summary>
/// 更新PurchasePrice DTO
/// 继承 TaktPurchasePriceCreateDto，添加 PurchasePriceId 字段
/// </summary>
public class TaktPurchasePriceUpdateDto : TaktPurchasePriceCreateDto
{
    /// <summary>
    /// PurchasePriceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceId { get; set; }

}

// ========================================
// PurchasePrice 状态 DTO
// ========================================

/// <summary>
/// PurchasePrice 状态更新 DTO
/// </summary>
public class TaktPurchasePriceStatusDto
{
    /// <summary>
    /// PurchasePriceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceId { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "价格状态（1=启用，0=禁用）不能为空")]
    public TaktCommonStatus PriceStatus { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchasePrice 导入模板行 DTO
/// </summary>
public class TaktPurchasePriceTemplateDto
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
    /// 工厂代码（不可空）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格编码（唯一索引）
    /// </summary>
    public string? PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
    /// </summary>
    public int? PriceType { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    public TaktCommonStatus? PriceStatus { get; set; }

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
/// PurchasePrice 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchasePriceImportDto
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
    /// 工厂代码（不可空）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格编码（唯一索引）
    /// </summary>
    public string? PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
    /// </summary>
    public int? PriceType { get; set; }

    /// <summary>
    /// 价格状态（1=启用，0=禁用）
    /// </summary>
    public TaktCommonStatus? PriceStatus { get; set; }

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
/// PurchasePrice 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchasePriceExportDto
{
    /// <summary>
    /// PurchasePriceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（不可空）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格编码（唯一索引）
    /// </summary>
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
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
    public TaktCommonStatus PriceStatus { get; set; }

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
