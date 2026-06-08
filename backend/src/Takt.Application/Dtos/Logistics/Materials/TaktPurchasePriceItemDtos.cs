// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktPurchasePriceItemDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchasePriceItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchasePriceItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// PurchasePriceItem 响应 DTO
// ========================================

/// <summary>
/// Takt采购价格明细实体（供应商物料价格明细表）
/// 对应前端 TaktPurchasePriceItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchasePriceItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchasePriceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceItemId { get; set; }

    /// <summary>
    /// 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceId { get; set; }

    /// <summary>
    /// 采购价格名称（填充字段）
    /// </summary>
    public string? PurchasePriceName { get; set; }

    /// <summary>
    /// 采购价格编码（冗余字段，便于查询）
    /// </summary>
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// 最小采购量（基本单位数量）
    /// </summary>
    public decimal MinPurchaseQuantity { get; set; }

    /// <summary>
    /// 最大采购量（基本单位数量，0表示无限制）
    /// </summary>
    public decimal MaxPurchaseQuantity { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）
    /// （子表：TaktPurchasePriceScale）
    /// </summary>
    public List<TaktPurchasePriceScaleDto>? Scales { get; set; }

}

// ========================================
// PurchasePriceItem 查询 DTO
// ========================================

/// <summary>
/// PurchasePriceItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchasePriceItemQueryDto : TaktPagedQuery
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
    /// 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePriceId { get; set; }

    /// <summary>
    /// 采购价格编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string? PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? PurchasePrice { get; set; }

    /// <summary>
    /// 最小采购量（基本单位数量）
    /// </summary>
    public decimal? MinPurchaseQuantity { get; set; }

    /// <summary>
    /// 最大采购量（基本单位数量，0表示无限制）
    /// </summary>
    public decimal? MaxPurchaseQuantity { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
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
// 创建PurchasePriceItem DTO
// ========================================

/// <summary>
/// 创建PurchasePriceItem DTO
/// </summary>
public class TaktPurchasePriceItemCreateDto
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
    /// 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceId { get; set; }

    /// <summary>
    /// 采购价格编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "采购价格编码（冗余字段，便于查询）不能为空")]
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    [Required(ErrorMessage = "采购单位不能为空")]
    public string PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// 最小采购量（基本单位数量）
    /// </summary>
    public decimal MinPurchaseQuantity { get; set; }

    /// <summary>
    /// 最大采购量（基本单位数量，0表示无限制）
    /// </summary>
    public decimal MaxPurchaseQuantity { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）（子表，级联保存）
    /// </summary>
    public List<TaktPurchasePriceScaleCreateDto>? Scales { get; set; }

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
// 更新PurchasePriceItem DTO
// ========================================

/// <summary>
/// 更新PurchasePriceItem DTO
/// 继承 TaktPurchasePriceItemCreateDto，添加 PurchasePriceItemId 字段
/// </summary>
public class TaktPurchasePriceItemUpdateDto : TaktPurchasePriceItemCreateDto
{
    /// <summary>
    /// PurchasePriceItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceItemId { get; set; }

}

// ========================================
// PurchasePriceItem 排序 DTO
// ========================================

/// <summary>
/// PurchasePriceItem 排序更新 DTO
/// </summary>
public class TaktPurchasePriceItemSortDto
{
    /// <summary>
    /// PurchasePriceItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceItemId { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchasePriceItem 导入模板行 DTO
/// </summary>
public class TaktPurchasePriceItemTemplateDto
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
    /// 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePriceId { get; set; }

    /// <summary>
    /// 采购价格编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string? PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
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
/// PurchasePriceItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchasePriceItemImportDto
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
    /// 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePriceId { get; set; }

    /// <summary>
    /// 采购价格编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string? PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
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
/// PurchasePriceItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchasePriceItemExportDto
{
    /// <summary>
    /// PurchasePriceItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceId { get; set; }

    /// <summary>
    /// 采购价格编码（冗余字段，便于查询）
    /// </summary>
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// 最小采购量（基本单位数量）
    /// </summary>
    public decimal MinPurchaseQuantity { get; set; }

    /// <summary>
    /// 最大采购量（基本单位数量，0表示无限制）
    /// </summary>
    public decimal MaxPurchaseQuantity { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
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
