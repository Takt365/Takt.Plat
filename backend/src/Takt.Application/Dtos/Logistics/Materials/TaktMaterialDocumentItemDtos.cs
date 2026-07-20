// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialDocumentItemDtos.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialDocumentItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialDocumentItem 生成，请按需审阅）
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
// MaterialDocumentItem 响应 DTO
// ========================================

/// <summary>
/// Takt物料凭证行项目实体
/// 对应前端 TaktMaterialDocumentItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialDocumentItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialDocumentItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentItemId { get; set; }

    /// <summary>
    /// 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证 名称（填充字段）
    /// </summary>
    public string? MaterialDocumentName { get; set; }

    /// <summary>
    /// 物料凭证号（冗余）
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_movement_type，如 101=收货）
    /// </summary>
    public string MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 过账日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 数量（基本单位数量，出库为负由移动类型决定）
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号（WBS 元素）
    /// </summary>
    public string? ProjectCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 收货/发货单编号
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 物料凭证主表
    /// （主表：TaktMaterialDocument）
    /// </summary>
    public TaktMaterialDocumentDto? MaterialTransaction { get; set; }

}

// ========================================
// MaterialDocumentItem 查询 DTO
// ========================================

/// <summary>
/// MaterialDocumentItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialDocumentItemQueryDto : TaktPagedQuery
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
    /// 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证号（冗余）
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_movement_type，如 101=收货）
    /// </summary>
    public string? MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 过账日期（范围查询-开始）
    /// </summary>
    public DateTime? PostingDateStart { get; set; }

    /// <summary>
    /// 过账日期（范围查询-结束）
    /// </summary>
    public DateTime? PostingDateEnd { get; set; }

    /// <summary>
    /// 数量（基本单位数量，出库为负由移动类型决定）
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号（WBS 元素）
    /// </summary>
    public string? ProjectCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal? LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 凭证日期（范围查询-开始）
    /// </summary>
    public DateTime? DocumentDateStart { get; set; }

    /// <summary>
    /// 凭证日期（范围查询-结束）
    /// </summary>
    public DateTime? DocumentDateEnd { get; set; }

    /// <summary>
    /// 收货/发货单编号
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
// 创建MaterialDocumentItem DTO
// ========================================

/// <summary>
/// 创建MaterialDocumentItem DTO
/// </summary>
public class TaktMaterialDocumentItemCreateDto
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
    /// 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证号（冗余）
    /// </summary>
    [Required(ErrorMessage = "物料凭证号（冗余）不能为空")]
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    [Required(ErrorMessage = "库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）不能为空")]
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_movement_type，如 101=收货）
    /// </summary>
    [Required(ErrorMessage = "移动类型（字典 logistics_movement_type，如 101=收货）不能为空")]
    public string MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 过账日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 数量（基本单位数量，出库为负由移动类型决定）
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号（WBS 元素）
    /// </summary>
    public string? ProjectCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 收货/发货单编号
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
// 更新MaterialDocumentItem DTO
// ========================================

/// <summary>
/// 更新MaterialDocumentItem DTO
/// 继承 TaktMaterialDocumentItemCreateDto，添加 MaterialDocumentItemId 字段
/// </summary>
public class TaktMaterialDocumentItemUpdateDto : TaktMaterialDocumentItemCreateDto
{
    /// <summary>
    /// MaterialDocumentItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentItemId { get; set; }

}

// ========================================
// MaterialDocumentItem 作废 DTO
// ========================================

/// <summary>
/// MaterialDocumentItem 作废/撤销作废 DTO
/// </summary>
public class TaktMaterialDocumentItemObsoleteDto
{
    /// <summary>
    /// MaterialDocumentItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialDocumentItem 导入模板行 DTO
/// </summary>
public class TaktMaterialDocumentItemTemplateDto
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
    /// 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证号（冗余）
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_movement_type，如 101=收货）
    /// </summary>
    public string? MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 过账日期
    /// </summary>
    public DateTime? PostingDate { get; set; }

    /// <summary>
    /// 数量（基本单位数量，出库为负由移动类型决定）
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号（WBS 元素）
    /// </summary>
    public string? ProjectCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal? LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime? DocumentDate { get; set; }

    /// <summary>
    /// 收货/发货单编号
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
/// MaterialDocumentItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialDocumentItemImportDto
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
    /// 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证号（冗余）
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_movement_type，如 101=收货）
    /// </summary>
    public string? MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 过账日期
    /// </summary>
    public DateTime? PostingDate { get; set; }

    /// <summary>
    /// 数量（基本单位数量，出库为负由移动类型决定）
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号（WBS 元素）
    /// </summary>
    public string? ProjectCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal? LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime? DocumentDate { get; set; }

    /// <summary>
    /// 收货/发货单编号
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
/// MaterialDocumentItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialDocumentItemExportDto
{
    /// <summary>
    /// MaterialDocumentItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证号（冗余）
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_movement_type，如 101=收货）
    /// </summary>
    public string MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 过账日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 数量（基本单位数量，出库为负由移动类型决定）
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号（WBS 元素）
    /// </summary>
    public string? ProjectCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 收货/发货单编号
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
