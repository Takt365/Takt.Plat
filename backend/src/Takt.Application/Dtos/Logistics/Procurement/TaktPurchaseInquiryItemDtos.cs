// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryItemDtos.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInquiryItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseInquiryItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Procurement;

// ========================================
// PurchaseInquiryItem 响应 DTO
// ========================================

/// <summary>
/// 采购询价明细实体
/// 对应前端 TaktPurchaseInquiryItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseInquiryItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseInquiryItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryItemId { get; set; }

    /// <summary>
    /// 采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryId { get; set; }

    /// <summary>
    /// 采购询价 名称（填充字段）
    /// </summary>
    public string? PurchaseInquiryName { get; set; }

    /// <summary>
    /// 采购询价编码（冗余，便于查询）
    /// </summary>
    public string PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
    /// </summary>
    public string AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 询价单位
    /// </summary>
    public string InquiryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 询价数量（基本单位数量，decimal(18,5)）
    /// </summary>
    public decimal InquiryQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    public int PurchasePerUnit { get; set; } = 0;

    /// <summary>
    /// 报价单价
    /// </summary>
    public decimal QuotedUnitPrice { get; set; }

    /// <summary>
    /// 报价金额
    /// </summary>
    public decimal QuotedAmount { get; set; }

    /// <summary>
    /// 目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string? TargetSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标供应商名称
    /// </summary>
    public string? TargetSupplierName { get; set; } = string.Empty;

}

// ========================================
// PurchaseInquiryItem 查询 DTO
// ========================================

/// <summary>
/// PurchaseInquiryItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseInquiryItemQueryDto : TaktPagedQuery
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
    /// 采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 采购询价编码（冗余，便于查询）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
    /// </summary>
    public string? AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
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
    /// 询价单位
    /// </summary>
    public string? InquiryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 询价数量（基本单位数量，decimal(18,5)）
    /// </summary>
    public decimal? InquiryQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    public int? PurchasePerUnit { get; set; }

    /// <summary>
    /// 报价单价
    /// </summary>
    public decimal? QuotedUnitPrice { get; set; }

    /// <summary>
    /// 报价金额
    /// </summary>
    public decimal? QuotedAmount { get; set; }

    /// <summary>
    /// 目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string? TargetSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标供应商名称
    /// </summary>
    public string? TargetSupplierName { get; set; } = string.Empty;

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
// 创建PurchaseInquiryItem DTO
// ========================================

/// <summary>
/// 创建PurchaseInquiryItem DTO
/// </summary>
public class TaktPurchaseInquiryItemCreateDto
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
    /// 采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryId { get; set; }

    /// <summary>
    /// 采购询价编码（冗余，便于查询）
    /// </summary>
    [Required(ErrorMessage = "采购询价编码（冗余，便于查询）不能为空")]
    public string PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
    /// </summary>
    [Required(ErrorMessage = "分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）不能为空")]
    public string AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 询价单位
    /// </summary>
    [Required(ErrorMessage = "询价单位不能为空")]
    public string InquiryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 询价数量（基本单位数量，decimal(18,5)）
    /// </summary>
    public decimal InquiryQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    public int PurchasePerUnit { get; set; } = 0;

    /// <summary>
    /// 报价单价
    /// </summary>
    public decimal QuotedUnitPrice { get; set; }

    /// <summary>
    /// 报价金额
    /// </summary>
    public decimal QuotedAmount { get; set; }

    /// <summary>
    /// 目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string? TargetSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标供应商名称
    /// </summary>
    public string? TargetSupplierName { get; set; } = string.Empty;

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
// 更新PurchaseInquiryItem DTO
// ========================================

/// <summary>
/// 更新PurchaseInquiryItem DTO
/// 继承 TaktPurchaseInquiryItemCreateDto，添加 PurchaseInquiryItemId 字段
/// </summary>
public class TaktPurchaseInquiryItemUpdateDto : TaktPurchaseInquiryItemCreateDto
{
    /// <summary>
    /// PurchaseInquiryItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryItemId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseInquiryItem 导入模板行 DTO
/// </summary>
public class TaktPurchaseInquiryItemTemplateDto
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
    /// 采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 采购询价编码（冗余，便于查询）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
    /// </summary>
    public string? AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
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
    /// 询价单位
    /// </summary>
    public string? InquiryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 询价数量（基本单位数量，decimal(18,5)）
    /// </summary>
    public decimal? InquiryQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    public int? PurchasePerUnit { get; set; }

    /// <summary>
    /// 报价单价
    /// </summary>
    public decimal? QuotedUnitPrice { get; set; }

    /// <summary>
    /// 报价金额
    /// </summary>
    public decimal? QuotedAmount { get; set; }

    /// <summary>
    /// 目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string? TargetSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标供应商名称
    /// </summary>
    public string? TargetSupplierName { get; set; } = string.Empty;

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
/// PurchaseInquiryItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseInquiryItemImportDto
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
    /// 采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 采购询价编码（冗余，便于查询）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
    /// </summary>
    public string? AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
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
    /// 询价单位
    /// </summary>
    public string? InquiryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 询价数量（基本单位数量，decimal(18,5)）
    /// </summary>
    public decimal? InquiryQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    public int? PurchasePerUnit { get; set; }

    /// <summary>
    /// 报价单价
    /// </summary>
    public decimal? QuotedUnitPrice { get; set; }

    /// <summary>
    /// 报价金额
    /// </summary>
    public decimal? QuotedAmount { get; set; }

    /// <summary>
    /// 目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string? TargetSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标供应商名称
    /// </summary>
    public string? TargetSupplierName { get; set; } = string.Empty;

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
/// PurchaseInquiryItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseInquiryItemExportDto
{
    /// <summary>
    /// PurchaseInquiryItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购询价 ID（关联 TaktPurchaseInquiry.Id，选项 TaktPurchaseInquirys/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInquiryId { get; set; }

    /// <summary>
    /// 采购询价编码（冗余，便于查询）
    /// </summary>
    public string PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
    /// </summary>
    public string AllocationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 询价单位
    /// </summary>
    public string InquiryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 询价数量（基本单位数量，decimal(18,5)）
    /// </summary>
    public decimal InquiryQuantity { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
    /// </summary>
    public int PurchasePerUnit { get; set; } = 0;

    /// <summary>
    /// 报价单价
    /// </summary>
    public decimal QuotedUnitPrice { get; set; }

    /// <summary>
    /// 报价金额
    /// </summary>
    public decimal QuotedAmount { get; set; }

    /// <summary>
    /// 目标供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string? TargetSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标供应商名称
    /// </summary>
    public string? TargetSupplierName { get; set; } = string.Empty;

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
