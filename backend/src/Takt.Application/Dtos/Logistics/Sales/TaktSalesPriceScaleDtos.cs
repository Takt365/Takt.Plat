// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesPriceScaleDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPriceScale 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesPriceScale 生成，请按需审阅）
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
// SalesPriceScale 响应 DTO
// ========================================

/// <summary>
/// Takt销售价格阶梯实体
/// 对应前端 TaktSalesPriceScaleDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesPriceScaleDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesPriceScaleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleId { get; set; }

    /// <summary>
    /// 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ItemId { get; set; }

    /// <summary>
    /// 价格明细名称（填充字段）
    /// </summary>
    public string? ItemName { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 起始数量（基本单位数量，包含此数量）
    /// </summary>
    public decimal StartQuantity { get; set; }

    /// <summary>
    /// 结束数量（基本单位数量，包含此数量，0表示无上限）
    /// </summary>
    public decimal EndQuantity { get; set; }

    /// <summary>
    /// 阶梯价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ScalePrice { get; set; }

    /// <summary>
    /// 销售价格明细（主表）
    /// （主表：TaktSalesPriceItem）
    /// </summary>
    public TaktSalesPriceItemDto? PriceItem { get; set; }

}

// ========================================
// SalesPriceScale 查询 DTO
// ========================================

/// <summary>
/// SalesPriceScale 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesPriceScaleQueryDto : TaktPagedQuery
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
    /// 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItemId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 起始数量（基本单位数量，包含此数量）
    /// </summary>
    public decimal? StartQuantity { get; set; }

    /// <summary>
    /// 结束数量（基本单位数量，包含此数量，0表示无上限）
    /// </summary>
    public decimal? EndQuantity { get; set; }

    /// <summary>
    /// 阶梯价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? ScalePrice { get; set; }

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
// 创建SalesPriceScale DTO
// ========================================

/// <summary>
/// 创建SalesPriceScale DTO
/// </summary>
public class TaktSalesPriceScaleCreateDto
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
    /// 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ItemId { get; set; }

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
    /// 起始数量（基本单位数量，包含此数量）
    /// </summary>
    public decimal StartQuantity { get; set; }

    /// <summary>
    /// 结束数量（基本单位数量，包含此数量，0表示无上限）
    /// </summary>
    public decimal EndQuantity { get; set; }

    /// <summary>
    /// 阶梯价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ScalePrice { get; set; }

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
// 更新SalesPriceScale DTO
// ========================================

/// <summary>
/// 更新SalesPriceScale DTO
/// 继承 TaktSalesPriceScaleCreateDto，添加 SalesPriceScaleId 字段
/// </summary>
public class TaktSalesPriceScaleUpdateDto : TaktSalesPriceScaleCreateDto
{
    /// <summary>
    /// SalesPriceScaleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesPriceScale 导入模板行 DTO
/// </summary>
public class TaktSalesPriceScaleTemplateDto
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
    /// 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItemId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

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
/// SalesPriceScale 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesPriceScaleImportDto
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
    /// 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItemId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

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
/// SalesPriceScale 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesPriceScaleExportDto
{
    /// <summary>
    /// SalesPriceScaleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ItemId { get; set; }

    /// <summary>
    /// 销售价格编码（冗余字段，便于查询）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 起始数量（基本单位数量，包含此数量）
    /// </summary>
    public decimal StartQuantity { get; set; }

    /// <summary>
    /// 结束数量（基本单位数量，包含此数量，0表示无上限）
    /// </summary>
    public decimal EndQuantity { get; set; }

    /// <summary>
    /// 阶梯价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal ScalePrice { get; set; }

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
