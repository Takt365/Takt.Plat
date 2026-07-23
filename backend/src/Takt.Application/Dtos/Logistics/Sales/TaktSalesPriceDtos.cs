// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesPriceDtos.cs
// 创建时间：2026-07-23
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
/// Takt销售价格实体（定价记录；条件类型 + 客户 + 物料 + 有效期；含子表 Items）
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesQuotationId { get; set; }

    /// <summary>
    /// 来源销售报价 名称（填充字段）
    /// </summary>
    public string? SalesQuotationName { get; set; }

    /// <summary>
    /// 来源销售报价编码（冗余）
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件行列表（主子表关系）
    /// （子表：TaktSalesPriceItem）
    /// </summary>
    public List<TaktSalesPriceItemDto>? Items { get; set; }

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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 定价日期控制（字典 logistics_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 有效起始日（范围查询-开始）
    /// </summary>
    public DateTime? ValidFromStart { get; set; }

    /// <summary>
    /// 有效起始日（范围查询-结束）
    /// </summary>
    public DateTime? ValidFromEnd { get; set; }

    /// <summary>
    /// 有效截至日（范围查询-开始）
    /// </summary>
    public DateTime? ValidToStart { get; set; }

    /// <summary>
    /// 有效截至日（范围查询-结束）
    /// </summary>
    public DateTime? ValidToEnd { get; set; }

    /// <summary>
    /// 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesQuotationId { get; set; }

    /// <summary>
    /// 来源销售报价编码（冗余）
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    [Required(ErrorMessage = "定价记录号（唯一索引；长度 20）不能为空")]
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    [Required(ErrorMessage = "条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）不能为空")]
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [Required(ErrorMessage = "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）不能为空")]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesQuotationId { get; set; }

    /// <summary>
    /// 来源销售报价编码（冗余）
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件行列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceItemCreateDto>? Items { get; set; }

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

    /// <summary>
    /// 定价条件行列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktSalesPriceItemUpdateDto>? Items { get; set; }

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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 定价日期控制（字典 logistics_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesQuotationId { get; set; }

    /// <summary>
    /// 来源销售报价编码（冗余）
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件行列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceItemCreateDto>? Items { get; set; }

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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 定价日期控制（字典 logistics_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesQuotationId { get; set; }

    /// <summary>
    /// 来源销售报价编码（冗余）
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

    /// <summary>
    /// 定价条件行列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceItemCreateDto>? Items { get; set; }

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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价记录号（唯一索引；长度 20）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
    /// </summary>
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票检验（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesQuotationId { get; set; }

    /// <summary>
    /// 来源销售报价编码（冗余）
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

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
