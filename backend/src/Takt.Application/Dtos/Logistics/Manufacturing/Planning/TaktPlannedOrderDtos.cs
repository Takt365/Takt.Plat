// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Planning
// 文件名称：TaktPlannedOrderDtos.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：PlannedOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPlannedOrder 生成，请按需审阅）
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
// PlannedOrder 响应 DTO
// ========================================

/// <summary>
/// 计划订单（MPS 净需求固化为可排程计划订单，下推 APS_Order）
/// 对应前端 TaktPlannedOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPlannedOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PlannedOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PlannedOrderId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单编码
    /// </summary>
    public string PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MPS 头表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// 来源 MPS 头表 名称（填充字段）
    /// </summary>
    public string? MasterProductionScheduleName { get; set; }

    /// <summary>
    /// 来源 MPS 行 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleLineId { get; set; }

    /// <summary>
    /// 来源 MPS 行 名称（填充字段）
    /// </summary>
    public string? MasterProductionScheduleLineName { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal PlannedQuantity { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 工艺路线编码（关联 TaktRouting.RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

}

// ========================================
// PlannedOrder 查询 DTO
// ========================================

/// <summary>
/// PlannedOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPlannedOrderQueryDto : TaktPagedQuery
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单编码
    /// </summary>
    public string? PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MPS 头表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// 来源 MPS 行 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal? PlannedQuantity { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedStartTimeStart { get; set; }

    /// <summary>
    /// 计划开始时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedStartTimeEnd { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedEndTimeStart { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedEndTimeEnd { get; set; }

    /// <summary>
    /// 工艺路线编码（关联 TaktRouting.RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
    /// </summary>
    public int? OrderStatus { get; set; }

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
// 创建PlannedOrder DTO
// ========================================

/// <summary>
/// 创建PlannedOrder DTO
/// </summary>
public class TaktPlannedOrderCreateDto
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
    /// 工厂代码
    /// </summary>
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单编码
    /// </summary>
    [Required(ErrorMessage = "计划订单编码不能为空")]
    public string PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MPS 头表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// 来源 MPS 行 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal PlannedQuantity { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    [Required(ErrorMessage = "计量单位不能为空")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 工艺路线编码（关联 TaktRouting.RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

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
// 更新PlannedOrder DTO
// ========================================

/// <summary>
/// 更新PlannedOrder DTO
/// 继承 TaktPlannedOrderCreateDto，添加 PlannedOrderId 字段
/// </summary>
public class TaktPlannedOrderUpdateDto : TaktPlannedOrderCreateDto
{
    /// <summary>
    /// PlannedOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PlannedOrderId { get; set; }

}

// ========================================
// PlannedOrder 状态 DTO
// ========================================

/// <summary>
/// PlannedOrder 状态更新 DTO
/// </summary>
public class TaktPlannedOrderStatusDto
{
    /// <summary>
    /// PlannedOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PlannedOrderId { get; set; }

    /// <summary>
    /// 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
    /// </summary>
    [Required(ErrorMessage = "计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）不能为空")]
    public int OrderStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PlannedOrder 导入模板行 DTO
/// </summary>
public class TaktPlannedOrderTemplateDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单编码
    /// </summary>
    public string? PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MPS 头表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// 来源 MPS 行 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码（关联 TaktRouting.RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
    /// </summary>
    public int? OrderStatus { get; set; }

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
/// PlannedOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPlannedOrderImportDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单编码
    /// </summary>
    public string? PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MPS 头表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// 来源 MPS 行 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码（关联 TaktRouting.RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
    /// </summary>
    public int? OrderStatus { get; set; }

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
/// PlannedOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPlannedOrderExportDto
{
    /// <summary>
    /// PlannedOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PlannedOrderId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单编码
    /// </summary>
    public string PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MPS 头表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// 来源 MPS 行 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleLineId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal PlannedQuantity { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 工艺路线编码（关联 TaktRouting.RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

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
