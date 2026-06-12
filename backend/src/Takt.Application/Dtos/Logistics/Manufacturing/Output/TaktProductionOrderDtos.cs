// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktProductionOrderDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

// ========================================
// ProductionOrder 响应 DTO
// ========================================

/// <summary>
/// 生产工单实体
/// 对应前端 TaktProductionOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA
    /// </summary>
    public string ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal ProducedQty { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=生产中，2=已完成）
    /// </summary>
    public int Status { get; set; } = 0;

}

// ========================================
// ProductionOrder 查询 DTO
// ========================================

/// <summary>
/// ProductionOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionOrderQueryDto : TaktPagedQuery
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
    /// 生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单数量
    /// </summary>
    public decimal? ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal? ProducedQty { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualStartDateStart { get; set; }

    /// <summary>
    /// 实际开始日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualStartDateEnd { get; set; }

    /// <summary>
    /// 实际完成日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualEndDateStart { get; set; }

    /// <summary>
    /// 实际完成日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualEndDateEnd { get; set; }

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=生产中，2=已完成）
    /// </summary>
    public int? Status { get; set; }

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
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建ProductionOrder DTO
// ========================================

/// <summary>
/// 创建ProductionOrder DTO
/// </summary>
public class TaktProductionOrderCreateDto
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
    /// 生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA
    /// </summary>
    [Required(ErrorMessage = "生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA不能为空")]
    public string ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    [Required(ErrorMessage = "生产工单号不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    [Required(ErrorMessage = "物料编码不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal ProducedQty { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    [Required(ErrorMessage = "计量单位不能为空")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=生产中，2=已完成）
    /// </summary>
    public int Status { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新ProductionOrder DTO
// ========================================

/// <summary>
/// 更新ProductionOrder DTO
/// 继承 TaktProductionOrderCreateDto，添加 ProductionOrderId 字段
/// </summary>
public class TaktProductionOrderUpdateDto : TaktProductionOrderCreateDto
{
    /// <summary>
    /// ProductionOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

}

// ========================================
// ProductionOrder 状态 DTO
// ========================================

/// <summary>
/// ProductionOrder 状态更新 DTO
/// </summary>
public class TaktProductionOrderStatusDto
{
    /// <summary>
    /// ProductionOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

    /// <summary>
    /// 状态（0=正常，1=生产中，2=已完成）
    /// </summary>
    [Required(ErrorMessage = "状态（0=正常，1=生产中，2=已完成）不能为空")]
    public int Status { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionOrder 导入模板行 DTO
/// </summary>
public class TaktProductionOrderTemplateDto
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
    /// 生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=生产中，2=已完成）
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// ProductionOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionOrderImportDto
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
    /// 生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA
    /// </summary>
    public string? ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=生产中，2=已完成）
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// ProductionOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionOrderExportDto
{
    /// <summary>
    /// ProductionOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA
    /// </summary>
    public string ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单号
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 已生产数量
    /// </summary>
    public decimal ProducedQty { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProdBatch { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=正常，1=生产中，2=已完成）
    /// </summary>
    public int Status { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
