// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsScheduleDtos.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：ApsSchedule 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktApsSchedule 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;

// ========================================
// ApsSchedule 响应 DTO
// ========================================

/// <summary>
/// APS排程主表（高级计划与排程）
/// 对应前端 TaktApsScheduleDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktApsScheduleDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ApsScheduleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleId { get; set; }

    /// <summary>
    /// 工厂编码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程编码（唯一索引）
    /// </summary>
    public string ScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程名称
    /// </summary>
    public string ScheduleName { get; set; } = string.Empty;

    /// <summary>
    /// 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
    /// </summary>
    public int ScheduleType { get; set; } = 0;

    /// <summary>
    /// 计划日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime PlanEndTime { get; set; }

    /// <summary>
    /// 计划周期（0=日计划，1=周计划，2=月计划）
    /// </summary>
    public int PlanCycle { get; set; } = 0;

    /// <summary>
    /// 车间编码
    /// </summary>
    public string? WorkshopCode { get; set; } = string.Empty;

    /// <summary>
    /// 车间名称
    /// </summary>
    public string? WorkshopName { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组编码
    /// </summary>
    public string? ProductionLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? ProductionLineName { get; set; } = string.Empty;

    /// <summary>
    /// 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
    /// </summary>
    public int ScheduleStrategy { get; set; } = 0;

    /// <summary>
    /// 排程算法（0=正向排程，1=逆向排程，2=双向排程）
    /// </summary>
    public int ScheduleAlgorithm { get; set; } = 0;

    /// <summary>
    /// 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
    /// </summary>
    public int OptimizationObjective { get; set; } = 0;

    /// <summary>
    /// 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
    /// </summary>
    public int ScheduleStatus { get; set; } = 0;

    /// <summary>
    /// 计划员ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannerId { get; set; }

    /// <summary>
    /// 计划员姓名
    /// </summary>
    public string? PlannerName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublishUserId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    public string? PublishUserName { get; set; } = string.Empty;

    /// <summary>
    /// 排程说明
    /// </summary>
    public string? ScheduleDescription { get; set; } = string.Empty;

    /// <summary>
    /// APS 排程订单列表（排程批次关联的订单）
    /// （子表：TaktApsOrder）
    /// </summary>
    public List<TaktApsOrderDto>? Orders { get; set; }

    /// <summary>
    /// 排程明细列表（主子表关系）
    /// （子表：TaktApsScheduleItem）
    /// </summary>
    public List<TaktApsScheduleItemDto>? Items { get; set; }

}

// ========================================
// ApsSchedule 查询 DTO
// ========================================

/// <summary>
/// ApsSchedule 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktApsScheduleQueryDto : TaktPagedQuery
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
    /// 工厂编码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程编码（唯一索引）
    /// </summary>
    public string? ScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程名称
    /// </summary>
    public string? ScheduleName { get; set; } = string.Empty;

    /// <summary>
    /// 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
    /// </summary>
    public int? ScheduleType { get; set; }

    /// <summary>
    /// 计划日期（范围查询-开始）
    /// </summary>
    public DateTime? PlanDateStart { get; set; }

    /// <summary>
    /// 计划日期（范围查询-结束）
    /// </summary>
    public DateTime? PlanDateEnd { get; set; }

    /// <summary>
    /// 计划开始时间（范围查询-开始）
    /// </summary>
    public DateTime? PlanStartTimeStart { get; set; }

    /// <summary>
    /// 计划开始时间（范围查询-结束）
    /// </summary>
    public DateTime? PlanStartTimeEnd { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-开始）
    /// </summary>
    public DateTime? PlanEndTimeStart { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-结束）
    /// </summary>
    public DateTime? PlanEndTimeEnd { get; set; }

    /// <summary>
    /// 计划周期（0=日计划，1=周计划，2=月计划）
    /// </summary>
    public int? PlanCycle { get; set; }

    /// <summary>
    /// 车间编码
    /// </summary>
    public string? WorkshopCode { get; set; } = string.Empty;

    /// <summary>
    /// 车间名称
    /// </summary>
    public string? WorkshopName { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组编码
    /// </summary>
    public string? ProductionLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? ProductionLineName { get; set; } = string.Empty;

    /// <summary>
    /// 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
    /// </summary>
    public int? ScheduleStrategy { get; set; }

    /// <summary>
    /// 排程算法（0=正向排程，1=逆向排程，2=双向排程）
    /// </summary>
    public int? ScheduleAlgorithm { get; set; }

    /// <summary>
    /// 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
    /// </summary>
    public int? OptimizationObjective { get; set; }

    /// <summary>
    /// 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
    /// </summary>
    public int? ScheduleStatus { get; set; }

    /// <summary>
    /// 计划员ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannerId { get; set; }

    /// <summary>
    /// 计划员姓名
    /// </summary>
    public string? PlannerName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间（范围查询-开始）
    /// </summary>
    public DateTime? PublishTimeStart { get; set; }

    /// <summary>
    /// 发布时间（范围查询-结束）
    /// </summary>
    public DateTime? PublishTimeEnd { get; set; }

    /// <summary>
    /// 发布人ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublishUserId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    public string? PublishUserName { get; set; } = string.Empty;

    /// <summary>
    /// 排程说明
    /// </summary>
    public string? ScheduleDescription { get; set; } = string.Empty;

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
// 创建ApsSchedule DTO
// ========================================

/// <summary>
/// 创建ApsSchedule DTO
/// </summary>
public class TaktApsScheduleCreateDto
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
    /// 工厂编码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂编码（选项 TaktPlants/options，DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "排程编码（唯一索引）不能为空")]
    public string ScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程名称
    /// </summary>
    [Required(ErrorMessage = "排程名称不能为空")]
    public string ScheduleName { get; set; } = string.Empty;

    /// <summary>
    /// 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
    /// </summary>
    public int ScheduleType { get; set; } = 0;

    /// <summary>
    /// 计划日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime PlanEndTime { get; set; }

    /// <summary>
    /// 计划周期（0=日计划，1=周计划，2=月计划）
    /// </summary>
    public int PlanCycle { get; set; } = 0;

    /// <summary>
    /// 车间编码
    /// </summary>
    public string? WorkshopCode { get; set; } = string.Empty;

    /// <summary>
    /// 车间名称
    /// </summary>
    public string? WorkshopName { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组编码
    /// </summary>
    public string? ProductionLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? ProductionLineName { get; set; } = string.Empty;

    /// <summary>
    /// 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
    /// </summary>
    public int ScheduleStrategy { get; set; } = 0;

    /// <summary>
    /// 排程算法（0=正向排程，1=逆向排程，2=双向排程）
    /// </summary>
    public int ScheduleAlgorithm { get; set; } = 0;

    /// <summary>
    /// 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
    /// </summary>
    public int OptimizationObjective { get; set; } = 0;

    /// <summary>
    /// 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
    /// </summary>
    public int ScheduleStatus { get; set; } = 0;

    /// <summary>
    /// 计划员ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannerId { get; set; }

    /// <summary>
    /// 计划员姓名
    /// </summary>
    public string? PlannerName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublishUserId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    public string? PublishUserName { get; set; } = string.Empty;

    /// <summary>
    /// 排程说明
    /// </summary>
    public string? ScheduleDescription { get; set; } = string.Empty;

    /// <summary>
    /// APS 排程订单列表（排程批次关联的订单）（子表，级联保存）
    /// </summary>
    public List<TaktApsOrderUpdateDto>? Orders { get; set; }

    /// <summary>
    /// 排程明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktApsScheduleItemUpdateDto>? Items { get; set; }

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
// 更新ApsSchedule DTO
// ========================================

/// <summary>
/// 更新ApsSchedule DTO
/// 继承 TaktApsScheduleCreateDto，添加 ApsScheduleId 字段
/// </summary>
public class TaktApsScheduleUpdateDto : TaktApsScheduleCreateDto
{
    /// <summary>
    /// ApsScheduleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleId { get; set; }

}

// ========================================
// ApsSchedule 状态 DTO
// ========================================

/// <summary>
/// ApsSchedule 状态更新 DTO
/// </summary>
public class TaktApsScheduleStatusDto
{
    /// <summary>
    /// ApsScheduleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleId { get; set; }

    /// <summary>
    /// 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
    /// </summary>
    [Required(ErrorMessage = "排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）不能为空")]
    public int ScheduleStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ApsSchedule 导入模板行 DTO
/// </summary>
public class TaktApsScheduleTemplateDto
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
    /// 工厂编码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程编码（唯一索引）
    /// </summary>
    public string? ScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程名称
    /// </summary>
    public string? ScheduleName { get; set; } = string.Empty;

    /// <summary>
    /// 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
    /// </summary>
    public int? ScheduleType { get; set; }

    /// <summary>
    /// 计划日期
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlanEndTime { get; set; }

    /// <summary>
    /// 计划周期（0=日计划，1=周计划，2=月计划）
    /// </summary>
    public int? PlanCycle { get; set; }

    /// <summary>
    /// 车间编码
    /// </summary>
    public string? WorkshopCode { get; set; } = string.Empty;

    /// <summary>
    /// 车间名称
    /// </summary>
    public string? WorkshopName { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组编码
    /// </summary>
    public string? ProductionLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? ProductionLineName { get; set; } = string.Empty;

    /// <summary>
    /// 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
    /// </summary>
    public int? ScheduleStrategy { get; set; }

    /// <summary>
    /// 排程算法（0=正向排程，1=逆向排程，2=双向排程）
    /// </summary>
    public int? ScheduleAlgorithm { get; set; }

    /// <summary>
    /// 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
    /// </summary>
    public int? OptimizationObjective { get; set; }

    /// <summary>
    /// 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
    /// </summary>
    public int? ScheduleStatus { get; set; }

    /// <summary>
    /// 计划员ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannerId { get; set; }

    /// <summary>
    /// 计划员姓名
    /// </summary>
    public string? PlannerName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublishUserId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    public string? PublishUserName { get; set; } = string.Empty;

    /// <summary>
    /// 排程说明
    /// </summary>
    public string? ScheduleDescription { get; set; } = string.Empty;

    /// <summary>
    /// APS 排程订单列表（排程批次关联的订单）（子表，级联保存）
    /// </summary>
    public List<TaktApsOrderCreateDto>? Orders { get; set; }

    /// <summary>
    /// 排程明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktApsScheduleItemCreateDto>? Items { get; set; }

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
/// ApsSchedule 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktApsScheduleImportDto
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
    /// 工厂编码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程编码（唯一索引）
    /// </summary>
    public string? ScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程名称
    /// </summary>
    public string? ScheduleName { get; set; } = string.Empty;

    /// <summary>
    /// 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
    /// </summary>
    public int? ScheduleType { get; set; }

    /// <summary>
    /// 计划日期
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlanEndTime { get; set; }

    /// <summary>
    /// 计划周期（0=日计划，1=周计划，2=月计划）
    /// </summary>
    public int? PlanCycle { get; set; }

    /// <summary>
    /// 车间编码
    /// </summary>
    public string? WorkshopCode { get; set; } = string.Empty;

    /// <summary>
    /// 车间名称
    /// </summary>
    public string? WorkshopName { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组编码
    /// </summary>
    public string? ProductionLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? ProductionLineName { get; set; } = string.Empty;

    /// <summary>
    /// 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
    /// </summary>
    public int? ScheduleStrategy { get; set; }

    /// <summary>
    /// 排程算法（0=正向排程，1=逆向排程，2=双向排程）
    /// </summary>
    public int? ScheduleAlgorithm { get; set; }

    /// <summary>
    /// 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
    /// </summary>
    public int? OptimizationObjective { get; set; }

    /// <summary>
    /// 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
    /// </summary>
    public int? ScheduleStatus { get; set; }

    /// <summary>
    /// 计划员ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannerId { get; set; }

    /// <summary>
    /// 计划员姓名
    /// </summary>
    public string? PlannerName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublishUserId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    public string? PublishUserName { get; set; } = string.Empty;

    /// <summary>
    /// 排程说明
    /// </summary>
    public string? ScheduleDescription { get; set; } = string.Empty;

    /// <summary>
    /// APS 排程订单列表（排程批次关联的订单）（子表，级联保存）
    /// </summary>
    public List<TaktApsOrderCreateDto>? Orders { get; set; }

    /// <summary>
    /// 排程明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktApsScheduleItemCreateDto>? Items { get; set; }

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
/// ApsSchedule 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktApsScheduleExportDto
{
    /// <summary>
    /// ApsScheduleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂编码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程编码（唯一索引）
    /// </summary>
    public string ScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程名称
    /// </summary>
    public string ScheduleName { get; set; } = string.Empty;

    /// <summary>
    /// 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
    /// </summary>
    public int ScheduleType { get; set; } = 0;

    /// <summary>
    /// 计划日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime PlanEndTime { get; set; }

    /// <summary>
    /// 计划周期（0=日计划，1=周计划，2=月计划）
    /// </summary>
    public int PlanCycle { get; set; } = 0;

    /// <summary>
    /// 车间编码
    /// </summary>
    public string? WorkshopCode { get; set; } = string.Empty;

    /// <summary>
    /// 车间名称
    /// </summary>
    public string? WorkshopName { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组编码
    /// </summary>
    public string? ProductionLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    public string? ProductionLineName { get; set; } = string.Empty;

    /// <summary>
    /// 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
    /// </summary>
    public int ScheduleStrategy { get; set; } = 0;

    /// <summary>
    /// 排程算法（0=正向排程，1=逆向排程，2=双向排程）
    /// </summary>
    public int ScheduleAlgorithm { get; set; } = 0;

    /// <summary>
    /// 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
    /// </summary>
    public int OptimizationObjective { get; set; } = 0;

    /// <summary>
    /// 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
    /// </summary>
    public int ScheduleStatus { get; set; } = 0;

    /// <summary>
    /// 计划员ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannerId { get; set; }

    /// <summary>
    /// 计划员姓名
    /// </summary>
    public string? PlannerName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublishUserId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    public string? PublishUserName { get; set; } = string.Empty;

    /// <summary>
    /// 排程说明
    /// </summary>
    public string? ScheduleDescription { get; set; } = string.Empty;

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
