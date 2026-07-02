// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Planning
// 文件名称：TaktProductionPlanItemDtos.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionPlanItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionPlanItem 生成，请按需审阅）
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
// ProductionPlanItem 响应 DTO
// ========================================

/// <summary>
/// Takt生产计划明细实体
/// 对应前端 TaktProductionPlanItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionPlanItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionPlanItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanItemId { get; set; }

    /// <summary>
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划名称（填充字段）
    /// </summary>
    public string? ProductionPlanName { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPlanId { get; set; }

    /// <summary>
    /// 来源销售计划名称（填充字段）
    /// </summary>
    public string? SalesPlanName { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

}

// ========================================
// ProductionPlanItem 查询 DTO
// ========================================

/// <summary>
/// ProductionPlanItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionPlanItemQueryDto : TaktPagedQuery
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
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPlanId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划开工日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedStartDateStart { get; set; }

    /// <summary>
    /// 计划开工日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedStartDateEnd { get; set; }

    /// <summary>
    /// 计划完工日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedEndDateStart { get; set; }

    /// <summary>
    /// 计划完工日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedEndDateEnd { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal? EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

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
// 创建ProductionPlanItem DTO
// ========================================

/// <summary>
/// 创建ProductionPlanItem DTO
/// </summary>
public class TaktProductionPlanItemCreateDto
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
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "生产计划编码（冗余字段，便于查询）不能为空")]
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPlanId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）不能为空")]
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
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

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
// 更新ProductionPlanItem DTO
// ========================================

/// <summary>
/// 更新ProductionPlanItem DTO
/// 继承 TaktProductionPlanItemCreateDto，添加 ProductionPlanItemId 字段
/// </summary>
public class TaktProductionPlanItemUpdateDto : TaktProductionPlanItemCreateDto
{
    /// <summary>
    /// ProductionPlanItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanItemId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionPlanItem 导入模板行 DTO
/// </summary>
public class TaktProductionPlanItemTemplateDto
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
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPlanId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal? EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

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
/// ProductionPlanItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionPlanItemImportDto
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
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPlanId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal? EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

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
/// ProductionPlanItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionPlanItemExportDto
{
    /// <summary>
    /// ProductionPlanItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPlanId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesPlanLineNumber { get; set; }

    /// <summary>
    /// 物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）
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
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

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
