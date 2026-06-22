// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Planning
// 文件名称：TaktPurchasePlanItemDtos.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchasePlanItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchasePlanItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Planning;

// ========================================
// PurchasePlanItem 响应 DTO
// ========================================

/// <summary>
/// Takt采购计划明细实体
/// 对应前端 TaktPurchasePlanItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchasePlanItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchasePlanItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePlanItemId { get; set; }

    /// <summary>
    /// 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePlanId { get; set; }

    /// <summary>
    /// 采购计划名称（填充字段）
    /// </summary>
    public string? PurchasePlanName { get; set; }

    /// <summary>
    /// 采购计划编码（冗余字段，便于查询）
    /// </summary>
    public string PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 来源生产计划名称（填充字段）
    /// </summary>
    public string? ProductionPlanName { get; set; }

    /// <summary>
    /// 来源生产计划编码
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源生产计划行号
    /// </summary>
    public int? ProductionPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划单位
    /// </summary>
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划到货日期
    /// </summary>
    public DateTime? PlannedArrivalDate { get; set; }

    /// <summary>
    /// 已转申请/订单数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 参考供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string? ReferenceSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考供货商名称
    /// </summary>
    public string? ReferenceSupplierName { get; set; } = string.Empty;

}

// ========================================
// PurchasePlanItem 查询 DTO
// ========================================

/// <summary>
/// PurchasePlanItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchasePlanItemQueryDto : TaktPagedQuery
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
    /// 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 采购计划编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 来源生产计划编码
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源生产计划行号
    /// </summary>
    public int? ProductionPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划单位
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划到货日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedArrivalDateStart { get; set; }

    /// <summary>
    /// 计划到货日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedArrivalDateEnd { get; set; }

    /// <summary>
    /// 已转申请/订单数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal? EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

    /// <summary>
    /// 参考供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string? ReferenceSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考供货商名称
    /// </summary>
    public string? ReferenceSupplierName { get; set; } = string.Empty;

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
// 创建PurchasePlanItem DTO
// ========================================

/// <summary>
/// 创建PurchasePlanItem DTO
/// </summary>
public class TaktPurchasePlanItemCreateDto
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
    /// 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePlanId { get; set; }

    /// <summary>
    /// 采购计划编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "采购计划编码（冗余字段，便于查询）不能为空")]
    public string PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 来源生产计划编码
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源生产计划行号
    /// </summary>
    public int? ProductionPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（关联 TaktMaterialPlant.MaterialCode）不能为空")]
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
    /// 计划单位
    /// </summary>
    [Required(ErrorMessage = "计划单位不能为空")]
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划到货日期
    /// </summary>
    public DateTime? PlannedArrivalDate { get; set; }

    /// <summary>
    /// 已转申请/订单数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 参考供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string? ReferenceSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考供货商名称
    /// </summary>
    public string? ReferenceSupplierName { get; set; } = string.Empty;

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
// 更新PurchasePlanItem DTO
// ========================================

/// <summary>
/// 更新PurchasePlanItem DTO
/// 继承 TaktPurchasePlanItemCreateDto，添加 PurchasePlanItemId 字段
/// </summary>
public class TaktPurchasePlanItemUpdateDto : TaktPurchasePlanItemCreateDto
{
    /// <summary>
    /// PurchasePlanItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePlanItemId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchasePlanItem 导入模板行 DTO
/// </summary>
public class TaktPurchasePlanItemTemplateDto
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
    /// 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 采购计划编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 来源生产计划编码
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源生产计划行号
    /// </summary>
    public int? ProductionPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划单位
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 参考供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string? ReferenceSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考供货商名称
    /// </summary>
    public string? ReferenceSupplierName { get; set; } = string.Empty;

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
/// PurchasePlanItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchasePlanItemImportDto
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
    /// 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 采购计划编码（冗余字段，便于查询）
    /// </summary>
    public string? PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 来源生产计划编码
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源生产计划行号
    /// </summary>
    public int? ProductionPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划单位
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 参考供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string? ReferenceSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考供货商名称
    /// </summary>
    public string? ReferenceSupplierName { get; set; } = string.Empty;

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
/// PurchasePlanItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchasePlanItemExportDto
{
    /// <summary>
    /// PurchasePlanItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePlanItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePlanId { get; set; }

    /// <summary>
    /// 采购计划编码（冗余字段，便于查询）
    /// </summary>
    public string PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 来源生产计划编码
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源生产计划行号
    /// </summary>
    public int? ProductionPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划单位
    /// </summary>
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划到货日期
    /// </summary>
    public DateTime? PlannedArrivalDate { get; set; }

    /// <summary>
    /// 已转申请/订单数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 参考供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string? ReferenceSupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考供货商名称
    /// </summary>
    public string? ReferenceSupplierName { get; set; } = string.Empty;

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
