// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Planning
// 文件名称：TaktMasterProductionScheduleLineDtos.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MasterProductionScheduleLine 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMasterProductionScheduleLine 生成，请按需审阅）
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
// MasterProductionScheduleLine 响应 DTO
// ========================================

/// <summary>
/// 主生产计划 MPS 行（物料 + 时间桶 + ATP）
/// 对应前端 TaktMasterProductionScheduleLineDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMasterProductionScheduleLineDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MasterProductionScheduleLineID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleLineId { get; set; }

    /// <summary>
    /// MPS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 头表 名称（填充字段）
    /// </summary>
    public string? MasterProductionScheduleName { get; set; }

    /// <summary>
    /// MPS 编码（冗余）
    /// </summary>
    public string MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 行 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleLineId { get; set; }

    /// <summary>
    /// 来源 MDS 行 名称（填充字段）
    /// </summary>
    public string? MasterDemandScheduleLineName { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    public DateTime BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    public DateTime BucketEnd { get; set; }

    /// <summary>
    /// 毛需求数量
    /// </summary>
    public decimal GrossRequirement { get; set; }

    /// <summary>
    /// 预计入库（计划接收）
    /// </summary>
    public decimal ScheduledReceipts { get; set; }

    /// <summary>
    /// 预计可用库存（期初预计库存）
    /// </summary>
    public decimal ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量
    /// </summary>
    public decimal NetRequirement { get; set; }

    /// <summary>
    /// 计划订单数量（MPS 产出）
    /// </summary>
    public decimal PlannedOrderQuantity { get; set; }

    /// <summary>
    /// 可承诺量 ATP
    /// </summary>
    public decimal AtpQuantity { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

}

// ========================================
// MasterProductionScheduleLine 查询 DTO
// ========================================

/// <summary>
/// MasterProductionScheduleLine 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMasterProductionScheduleLineQueryDto : TaktPagedQuery
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
    /// MPS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 编码（冗余）
    /// </summary>
    public string? MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 行 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始（范围查询-开始）
    /// </summary>
    public DateTime? BucketStartStart { get; set; }

    /// <summary>
    /// 时间桶开始（范围查询-结束）
    /// </summary>
    public DateTime? BucketStartEnd { get; set; }

    /// <summary>
    /// 时间桶结束（范围查询-开始）
    /// </summary>
    public DateTime? BucketEndStart { get; set; }

    /// <summary>
    /// 时间桶结束（范围查询-结束）
    /// </summary>
    public DateTime? BucketEndEnd { get; set; }

    /// <summary>
    /// 毛需求数量
    /// </summary>
    public decimal? GrossRequirement { get; set; }

    /// <summary>
    /// 预计入库（计划接收）
    /// </summary>
    public decimal? ScheduledReceipts { get; set; }

    /// <summary>
    /// 预计可用库存（期初预计库存）
    /// </summary>
    public decimal? ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量
    /// </summary>
    public decimal? NetRequirement { get; set; }

    /// <summary>
    /// 计划订单数量（MPS 产出）
    /// </summary>
    public decimal? PlannedOrderQuantity { get; set; }

    /// <summary>
    /// 可承诺量 ATP
    /// </summary>
    public decimal? AtpQuantity { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

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
// 创建MasterProductionScheduleLine DTO
// ========================================

/// <summary>
/// 创建MasterProductionScheduleLine DTO
/// </summary>
public class TaktMasterProductionScheduleLineCreateDto
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
    /// MPS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 编码（冗余）
    /// </summary>
    [Required(ErrorMessage = "MPS 编码（冗余）不能为空")]
    public string MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 行 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    public DateTime BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    public DateTime BucketEnd { get; set; }

    /// <summary>
    /// 毛需求数量
    /// </summary>
    public decimal GrossRequirement { get; set; }

    /// <summary>
    /// 预计入库（计划接收）
    /// </summary>
    public decimal ScheduledReceipts { get; set; }

    /// <summary>
    /// 预计可用库存（期初预计库存）
    /// </summary>
    public decimal ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量
    /// </summary>
    public decimal NetRequirement { get; set; }

    /// <summary>
    /// 计划订单数量（MPS 产出）
    /// </summary>
    public decimal PlannedOrderQuantity { get; set; }

    /// <summary>
    /// 可承诺量 ATP
    /// </summary>
    public decimal AtpQuantity { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    [Required(ErrorMessage = "计量单位不能为空")]
    public string UnitOfMeasure { get; set; } = string.Empty;

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
// 更新MasterProductionScheduleLine DTO
// ========================================

/// <summary>
/// 更新MasterProductionScheduleLine DTO
/// 继承 TaktMasterProductionScheduleLineCreateDto，添加 MasterProductionScheduleLineId 字段
/// </summary>
public class TaktMasterProductionScheduleLineUpdateDto : TaktMasterProductionScheduleLineCreateDto
{
    /// <summary>
    /// MasterProductionScheduleLineID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleLineId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MasterProductionScheduleLine 导入模板行 DTO
/// </summary>
public class TaktMasterProductionScheduleLineTemplateDto
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
    /// MPS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 编码（冗余）
    /// </summary>
    public string? MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 行 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

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
/// MasterProductionScheduleLine 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMasterProductionScheduleLineImportDto
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
    /// MPS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 编码（冗余）
    /// </summary>
    public string? MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 行 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

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
/// MasterProductionScheduleLine 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMasterProductionScheduleLineExportDto
{
    /// <summary>
    /// MasterProductionScheduleLineID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleLineId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// MPS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 编码（冗余）
    /// </summary>
    public string MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 行 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    public DateTime BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    public DateTime BucketEnd { get; set; }

    /// <summary>
    /// 毛需求数量
    /// </summary>
    public decimal GrossRequirement { get; set; }

    /// <summary>
    /// 预计入库（计划接收）
    /// </summary>
    public decimal ScheduledReceipts { get; set; }

    /// <summary>
    /// 预计可用库存（期初预计库存）
    /// </summary>
    public decimal ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量
    /// </summary>
    public decimal NetRequirement { get; set; }

    /// <summary>
    /// 计划订单数量（MPS 产出）
    /// </summary>
    public decimal PlannedOrderQuantity { get; set; }

    /// <summary>
    /// 可承诺量 ATP
    /// </summary>
    public decimal AtpQuantity { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

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
