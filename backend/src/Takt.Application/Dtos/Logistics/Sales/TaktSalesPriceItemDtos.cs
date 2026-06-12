// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesPriceItemDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPriceItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesPriceItem 生成，请按需审阅）
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
// SalesPriceItem 响应 DTO
// ========================================

/// <summary>
/// Takt销售价格明细实体（客户物料价格明细表）
/// 对应前端 TaktSalesPriceItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesPriceItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesPriceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 销售价格名称（填充字段）
    /// </summary>
    public string? SalesPriceName { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位
    /// </summary>
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal SalesPrice { get; set; }

    /// <summary>
    /// 最小订购量（基本单位数量）
    /// </summary>
    public decimal MinOrderQuantity { get; set; }

    /// <summary>
    /// 最大订购量（基本单位数量，0表示无限制）
    /// </summary>
    public decimal MaxOrderQuantity { get; set; }

    /// <summary>
    /// 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）
    /// （子表：TaktSalesPriceScale）
    /// </summary>
    public List<TaktSalesPriceScaleDto>? Scales { get; set; }

    /// <summary>
    /// 销售价格（主表）
    /// （主表：TaktSalesPrice）
    /// </summary>
    public TaktSalesPriceDto? Price { get; set; }

}

// ========================================
// SalesPriceItem 查询 DTO
// ========================================

/// <summary>
/// SalesPriceItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesPriceItemQueryDto : TaktPagedQuery
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
    /// 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPriceId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? SalesPrice { get; set; }

    /// <summary>
    /// 最小订购量（基本单位数量）
    /// </summary>
    public decimal? MinOrderQuantity { get; set; }

    /// <summary>
    /// 最大订购量（基本单位数量，0表示无限制）
    /// </summary>
    public decimal? MaxOrderQuantity { get; set; }

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
// 创建SalesPriceItem DTO
// ========================================

/// <summary>
/// 创建SalesPriceItem DTO
/// </summary>
public class TaktSalesPriceItemCreateDto
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
    /// 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "销售价格编码（冗余字段，便于查询）不能为空")]
    public string SalesPriceCode { get; set; } = string.Empty;

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
    /// 销售单位
    /// </summary>
    [Required(ErrorMessage = "销售单位不能为空")]
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal SalesPrice { get; set; }

    /// <summary>
    /// 最小订购量（基本单位数量）
    /// </summary>
    public decimal MinOrderQuantity { get; set; }

    /// <summary>
    /// 最大订购量（基本单位数量，0表示无限制）
    /// </summary>
    public decimal MaxOrderQuantity { get; set; }

    /// <summary>
    /// 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceScaleCreateDto>? Scales { get; set; }

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
// 更新SalesPriceItem DTO
// ========================================

/// <summary>
/// 更新SalesPriceItem DTO
/// 继承 TaktSalesPriceItemCreateDto，添加 SalesPriceItemId 字段
/// </summary>
public class TaktSalesPriceItemUpdateDto : TaktSalesPriceItemCreateDto
{
    /// <summary>
    /// SalesPriceItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesPriceItem 导入模板行 DTO
/// </summary>
public class TaktSalesPriceItemTemplateDto
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
    /// 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPriceId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

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
/// SalesPriceItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesPriceItemImportDto
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
    /// 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPriceId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位
    /// </summary>
    public string? SalesUnit { get; set; } = string.Empty;

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
/// SalesPriceItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesPriceItemExportDto
{
    /// <summary>
    /// SalesPriceItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位
    /// </summary>
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal SalesPrice { get; set; }

    /// <summary>
    /// 最小订购量（基本单位数量）
    /// </summary>
    public decimal MinOrderQuantity { get; set; }

    /// <summary>
    /// 最大订购量（基本单位数量，0表示无限制）
    /// </summary>
    public decimal MaxOrderQuantity { get; set; }

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
