// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialTransactionItemDtos.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialTransactionItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialTransactionItem 生成，请按需审阅）
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
// MaterialTransactionItem 响应 DTO
// ========================================

/// <summary>
/// Takt物料交易明细实体
/// 对应前端 TaktMaterialTransactionItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialTransactionItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialTransactionItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionItemId { get; set; }

    /// <summary>
    /// 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionId { get; set; }

    /// <summary>
    /// 物料交易名称（填充字段）
    /// </summary>
    public string? MaterialTransactionName { get; set; }

    /// <summary>
    /// 物料交易单号（冗余字段，便于查询）
    /// </summary>
    public string MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源单号（采购订单、销售订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源单行号
    /// </summary>
    public int? SourceLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 交易单位
    /// </summary>
    public string TransactionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 交易数量（基本单位数量）
    /// </summary>
    public decimal TransactionQuantity { get; set; }

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 行金额
    /// </summary>
    public decimal LineAmount { get; set; }

    /// <summary>
    /// 物料交易主表
    /// （主表：TaktMaterialTransaction）
    /// </summary>
    public TaktMaterialTransactionDto? MaterialTransaction { get; set; }

}

// ========================================
// MaterialTransactionItem 查询 DTO
// ========================================

/// <summary>
/// MaterialTransactionItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialTransactionItemQueryDto : TaktPagedQuery
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
    /// 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialTransactionId { get; set; }

    /// <summary>
    /// 物料交易单号（冗余字段，便于查询）
    /// </summary>
    public string? MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源单号（采购订单、销售订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源单行号
    /// </summary>
    public int? SourceLineNumber { get; set; }

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
    /// 交易单位
    /// </summary>
    public string? TransactionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 交易数量（基本单位数量）
    /// </summary>
    public decimal? TransactionQuantity { get; set; }

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// 行金额
    /// </summary>
    public decimal? LineAmount { get; set; }

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
// 创建MaterialTransactionItem DTO
// ========================================

/// <summary>
/// 创建MaterialTransactionItem DTO
/// </summary>
public class TaktMaterialTransactionItemCreateDto
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
    /// 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionId { get; set; }

    /// <summary>
    /// 物料交易单号（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "物料交易单号（冗余字段，便于查询）不能为空")]
    public string MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源单号（采购订单、销售订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源单行号
    /// </summary>
    public int? SourceLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

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
    /// 交易单位
    /// </summary>
    [Required(ErrorMessage = "交易单位不能为空")]
    public string TransactionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 交易数量（基本单位数量）
    /// </summary>
    public decimal TransactionQuantity { get; set; }

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 行金额
    /// </summary>
    public decimal LineAmount { get; set; }

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
// 更新MaterialTransactionItem DTO
// ========================================

/// <summary>
/// 更新MaterialTransactionItem DTO
/// 继承 TaktMaterialTransactionItemCreateDto，添加 MaterialTransactionItemId 字段
/// </summary>
public class TaktMaterialTransactionItemUpdateDto : TaktMaterialTransactionItemCreateDto
{
    /// <summary>
    /// MaterialTransactionItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionItemId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialTransactionItem 导入模板行 DTO
/// </summary>
public class TaktMaterialTransactionItemTemplateDto
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
    /// 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialTransactionId { get; set; }

    /// <summary>
    /// 物料交易单号（冗余字段，便于查询）
    /// </summary>
    public string? MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源单号（采购订单、销售订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源单行号
    /// </summary>
    public int? SourceLineNumber { get; set; }

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
    /// 交易单位
    /// </summary>
    public string? TransactionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

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
/// MaterialTransactionItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialTransactionItemImportDto
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
    /// 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialTransactionId { get; set; }

    /// <summary>
    /// 物料交易单号（冗余字段，便于查询）
    /// </summary>
    public string? MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源单号（采购订单、销售订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源单行号
    /// </summary>
    public int? SourceLineNumber { get; set; }

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
    /// 交易单位
    /// </summary>
    public string? TransactionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

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
/// MaterialTransactionItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialTransactionItemExportDto
{
    /// <summary>
    /// MaterialTransactionItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionId { get; set; }

    /// <summary>
    /// 物料交易单号（冗余字段，便于查询）
    /// </summary>
    public string MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源单号（采购订单、销售订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源单行号
    /// </summary>
    public int? SourceLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 交易单位
    /// </summary>
    public string TransactionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 交易数量（基本单位数量）
    /// </summary>
    public decimal TransactionQuantity { get; set; }

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 行金额
    /// </summary>
    public decimal LineAmount { get; set; }

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
