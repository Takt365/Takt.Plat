// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesInvoiceItemDtos.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesInvoiceItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesInvoiceItem 生成，请按需审阅）
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
// SalesInvoiceItem 响应 DTO
// ========================================

/// <summary>
/// Takt销售发票明细实体
/// 对应前端 TaktSalesInvoiceItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesInvoiceItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesInvoiceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceItemId { get; set; }

    /// <summary>
    /// 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    public string? SalesInvoiceName { get; set; }

    /// <summary>
    /// 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
    /// </summary>
    public string AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项目/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 机种名称
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 业务货币计价的金额
    /// </summary>
    public decimal TransactionCurrencyAmount { get; set; }

    /// <summary>
    /// 含税价格（打印用；如 100.00）
    /// </summary>
    public decimal TaxIncludedPrice { get; set; }

    /// <summary>
    /// 未税价格（打印用；如 88.50）
    /// </summary>
    public decimal UntaxedPrice { get; set; }

    /// <summary>
    /// 税费（打印用；行税额，如 11.50）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目（行号）
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 销售发票主表
    /// （主表：TaktSalesInvoice）
    /// </summary>
    public TaktSalesInvoiceDto? SalesInvoice { get; set; }

}

// ========================================
// SalesInvoiceItem 查询 DTO
// ========================================

/// <summary>
/// SalesInvoiceItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesInvoiceItemQueryDto : TaktPagedQuery
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
    /// 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesInvoiceId { get; set; }

    /// <summary>
    /// 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
    /// </summary>
    public string? AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项目/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 过帐日期（范围查询-开始）
    /// </summary>
    public DateTime? PostingDateStart { get; set; }

    /// <summary>
    /// 过帐日期（范围查询-结束）
    /// </summary>
    public DateTime? PostingDateEnd { get; set; }

    /// <summary>
    /// 机种名称
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? Unit { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal? LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 业务货币计价的金额
    /// </summary>
    public decimal? TransactionCurrencyAmount { get; set; }

    /// <summary>
    /// 含税价格（打印用；如 100.00）
    /// </summary>
    public decimal? TaxIncludedPrice { get; set; }

    /// <summary>
    /// 未税价格（打印用；如 88.50）
    /// </summary>
    public decimal? UntaxedPrice { get; set; }

    /// <summary>
    /// 税费（打印用；行税额，如 11.50）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目（行号）
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建SalesInvoiceItem DTO
// ========================================

/// <summary>
/// 创建SalesInvoiceItem DTO
/// </summary>
public class TaktSalesInvoiceItemCreateDto
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
    /// 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
    /// </summary>
    [Required(ErrorMessage = "会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）不能为空")]
    public string AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项目/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 机种名称
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    [Required(ErrorMessage = "物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）不能为空")]
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    [Required(ErrorMessage = "物料名称（回填：随物料）不能为空")]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）不能为空")]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 业务货币计价的金额
    /// </summary>
    public decimal TransactionCurrencyAmount { get; set; }

    /// <summary>
    /// 含税价格（打印用；如 100.00）
    /// </summary>
    public decimal TaxIncludedPrice { get; set; }

    /// <summary>
    /// 未税价格（打印用；如 88.50）
    /// </summary>
    public decimal UntaxedPrice { get; set; }

    /// <summary>
    /// 税费（打印用；行税额，如 11.50）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
    /// </summary>
    [Required(ErrorMessage = "凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）不能为空")]
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目（行号）
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新SalesInvoiceItem DTO
// ========================================

/// <summary>
/// 更新SalesInvoiceItem DTO
/// 继承 TaktSalesInvoiceItemCreateDto，添加 SalesInvoiceItemId 字段
/// </summary>
public class TaktSalesInvoiceItemUpdateDto : TaktSalesInvoiceItemCreateDto
{
    /// <summary>
    /// SalesInvoiceItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceItemId { get; set; }

}

// ========================================
// SalesInvoiceItem 作废 DTO
// ========================================

/// <summary>
/// SalesInvoiceItem 作废/撤销作废 DTO
/// </summary>
public class TaktSalesInvoiceItemObsoleteDto
{
    /// <summary>
    /// SalesInvoiceItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesInvoiceItem 导入模板行 DTO
/// </summary>
public class TaktSalesInvoiceItemTemplateDto
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
    /// 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesInvoiceId { get; set; }

    /// <summary>
    /// 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
    /// </summary>
    public string? AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项目/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime? PostingDate { get; set; }

    /// <summary>
    /// 机种名称
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? Unit { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal? LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 业务货币计价的金额
    /// </summary>
    public decimal? TransactionCurrencyAmount { get; set; }

    /// <summary>
    /// 含税价格（打印用；如 100.00）
    /// </summary>
    public decimal? TaxIncludedPrice { get; set; }

    /// <summary>
    /// 未税价格（打印用；如 88.50）
    /// </summary>
    public decimal? UntaxedPrice { get; set; }

    /// <summary>
    /// 税费（打印用；行税额，如 11.50）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目（行号）
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// SalesInvoiceItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesInvoiceItemImportDto
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
    /// 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesInvoiceId { get; set; }

    /// <summary>
    /// 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
    /// </summary>
    public string? AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项目/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime? PostingDate { get; set; }

    /// <summary>
    /// 机种名称
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? Unit { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal? LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 业务货币计价的金额
    /// </summary>
    public decimal? TransactionCurrencyAmount { get; set; }

    /// <summary>
    /// 含税价格（打印用；如 100.00）
    /// </summary>
    public decimal? TaxIncludedPrice { get; set; }

    /// <summary>
    /// 未税价格（打印用；如 88.50）
    /// </summary>
    public decimal? UntaxedPrice { get; set; }

    /// <summary>
    /// 税费（打印用；行税额，如 11.50）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目（行号）
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// SalesInvoiceItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesInvoiceItemExportDto
{
    /// <summary>
    /// SalesInvoiceItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
    /// </summary>
    public string AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项目/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 机种名称
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
    /// </summary>
    public string? AccountTitle { get; set; } = string.Empty;

    /// <summary>
    /// 数量
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 业务货币计价的金额
    /// </summary>
    public decimal TransactionCurrencyAmount { get; set; }

    /// <summary>
    /// 含税价格（打印用；如 100.00）
    /// </summary>
    public decimal TaxIncludedPrice { get; set; }

    /// <summary>
    /// 未税价格（打印用；如 88.50）
    /// </summary>
    public decimal UntaxedPrice { get; set; }

    /// <summary>
    /// 税费（打印用；行税额，如 11.50）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目（行号）
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
