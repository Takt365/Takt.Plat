// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesQuotationDtos.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesQuotation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesQuotation 生成，请按需审阅）
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
// SalesQuotation 响应 DTO
// ========================================

/// <summary>
/// Takt销售报价实体
/// 对应前端 TaktSalesQuotationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesQuotationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesQuotationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationId { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售报价编码（唯一索引）
    /// </summary>
    public string SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 报价日期
    /// </summary>
    public DateTime QuotationDate { get; set; }

    /// <summary>
    /// 报价有效期至
    /// </summary>
    public DateTime? ValidUntilDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 报价总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 报价总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 报价实付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
    /// </summary>
    public int QuotationStatus { get; set; } = 0;

    /// <summary>
    /// 销售报价明细列表（主子表关系）
    /// （子表：TaktSalesQuotationItem）
    /// </summary>
    public List<TaktSalesQuotationItemDto>? Items { get; set; }

}

// ========================================
// SalesQuotation 查询 DTO
// ========================================

/// <summary>
/// SalesQuotation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesQuotationQueryDto : TaktPagedQuery
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
    /// 销售报价编码（唯一索引）
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 报价日期（范围查询-开始）
    /// </summary>
    public DateTime? QuotationDateStart { get; set; }

    /// <summary>
    /// 报价日期（范围查询-结束）
    /// </summary>
    public DateTime? QuotationDateEnd { get; set; }

    /// <summary>
    /// 报价有效期至（范围查询-开始）
    /// </summary>
    public DateTime? ValidUntilDateStart { get; set; }

    /// <summary>
    /// 报价有效期至（范围查询-结束）
    /// </summary>
    public DateTime? ValidUntilDateEnd { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 报价总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 报价总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 报价实付金额
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
    /// </summary>
    public int? QuotationStatus { get; set; }

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
// 创建SalesQuotation DTO
// ========================================

/// <summary>
/// 创建SalesQuotation DTO
/// </summary>
public class TaktSalesQuotationCreateDto
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
    /// 销售报价编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "销售报价编码（唯一索引）不能为空")]
    public string SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [Required(ErrorMessage = "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）不能为空")]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    [Required(ErrorMessage = "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）不能为空")]
    public string CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 报价日期
    /// </summary>
    public DateTime QuotationDate { get; set; }

    /// <summary>
    /// 报价有效期至
    /// </summary>
    public DateTime? ValidUntilDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 报价总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 报价总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [Required(ErrorMessage = "结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 报价实付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
    /// </summary>
    public int QuotationStatus { get; set; } = 0;

    /// <summary>
    /// 销售报价明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesQuotationItemCreateDto>? Items { get; set; }

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
// 更新SalesQuotation DTO
// ========================================

/// <summary>
/// 更新SalesQuotation DTO
/// 继承 TaktSalesQuotationCreateDto，添加 SalesQuotationId 字段
/// </summary>
public class TaktSalesQuotationUpdateDto : TaktSalesQuotationCreateDto
{
    /// <summary>
    /// SalesQuotationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationId { get; set; }

    /// <summary>
    /// 销售报价明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktSalesQuotationItemUpdateDto>? Items { get; set; }

}

// ========================================
// SalesQuotation 状态 DTO
// ========================================

/// <summary>
/// SalesQuotation 状态更新 DTO
/// </summary>
public class TaktSalesQuotationStatusDto
{
    /// <summary>
    /// SalesQuotationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationId { get; set; }

    /// <summary>
    /// 报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
    /// </summary>
    [Required(ErrorMessage = "报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）不能为空")]
    public int QuotationStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesQuotation 导入模板行 DTO
/// </summary>
public class TaktSalesQuotationTemplateDto
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
    /// 销售报价编码（唯一索引）
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 报价日期
    /// </summary>
    public DateTime? QuotationDate { get; set; }

    /// <summary>
    /// 报价有效期至
    /// </summary>
    public DateTime? ValidUntilDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 报价总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 报价总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 报价实付金额
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
    /// </summary>
    public int? QuotationStatus { get; set; }

    /// <summary>
    /// 销售报价明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesQuotationItemCreateDto>? Items { get; set; }

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
/// SalesQuotation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesQuotationImportDto
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
    /// 销售报价编码（唯一索引）
    /// </summary>
    public string? SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 报价日期
    /// </summary>
    public DateTime? QuotationDate { get; set; }

    /// <summary>
    /// 报价有效期至
    /// </summary>
    public DateTime? ValidUntilDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 报价总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 报价总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税费
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 报价实付金额
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
    /// </summary>
    public int? QuotationStatus { get; set; }

    /// <summary>
    /// 销售报价明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesQuotationItemCreateDto>? Items { get; set; }

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
/// SalesQuotation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesQuotationExportDto
{
    /// <summary>
    /// SalesQuotationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesQuotationId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售报价编码（唯一索引）
    /// </summary>
    public string SalesQuotationCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 报价日期
    /// </summary>
    public DateTime QuotationDate { get; set; }

    /// <summary>
    /// 报价有效期至
    /// </summary>
    public DateTime? ValidUntilDate { get; set; }

    /// <summary>
    /// 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
    /// </summary>
    public string? SalesBy { get; set; } = string.Empty;

    /// <summary>
    /// 报价总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 报价总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 折扣金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税费
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 报价实付金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
    /// </summary>
    public string? SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
    /// </summary>
    public int QuotationStatus { get; set; } = 0;

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
