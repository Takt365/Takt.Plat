// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Maintenance
// 文件名称：TaktMaintenanceDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：Maintenance 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaintenance 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Maintenance;

// ========================================
// Maintenance 响应 DTO
// ========================================

/// <summary>
/// Takt设备维护记录实体（TaktEquipment的子表）
/// 对应前端 TaktMaintenanceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaintenanceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaintenanceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceId { get; set; }

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备名称（填充字段）
    /// </summary>
    public string? EquipmentName { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期
    /// </summary>
    public DateTime MaintenanceDate { get; set; }

    /// <summary>
    /// 维护开始时间
    /// </summary>
    public DateTime? MaintenanceStartTime { get; set; }

    /// <summary>
    /// 维护结束时间
    /// </summary>
    public DateTime? MaintenanceEndTime { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON格式，存储使用的配件列表）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int MaintenanceStatus { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 设备（主表）
    /// （主表：TaktEquipment）
    /// </summary>
    public TaktEquipmentDto? Equipment { get; set; }

}

// ========================================
// Maintenance 查询 DTO
// ========================================

/// <summary>
/// Maintenance 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaintenanceQueryDto : TaktPagedQuery
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
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int? MaintenanceType { get; set; }

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期（范围查询-开始）
    /// </summary>
    public DateTime? MaintenanceDateStart { get; set; }

    /// <summary>
    /// 维护日期（范围查询-结束）
    /// </summary>
    public DateTime? MaintenanceDateEnd { get; set; }

    /// <summary>
    /// 维护开始时间（范围查询-开始）
    /// </summary>
    public DateTime? MaintenanceStartTimeStart { get; set; }

    /// <summary>
    /// 维护开始时间（范围查询-结束）
    /// </summary>
    public DateTime? MaintenanceStartTimeEnd { get; set; }

    /// <summary>
    /// 维护结束时间（范围查询-开始）
    /// </summary>
    public DateTime? MaintenanceEndTimeStart { get; set; }

    /// <summary>
    /// 维护结束时间（范围查询-结束）
    /// </summary>
    public DateTime? MaintenanceEndTimeEnd { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON格式，存储使用的配件列表）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int? MaintenanceResult { get; set; }

    /// <summary>
    /// 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int? MaintenanceStatus { get; set; }

    /// <summary>
    /// 下次维护日期（范围查询-开始）
    /// </summary>
    public DateTime? NextMaintenanceDateStart { get; set; }

    /// <summary>
    /// 下次维护日期（范围查询-结束）
    /// </summary>
    public DateTime? NextMaintenanceDateEnd { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int? MaintenanceCycleDays { get; set; }

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间（范围查询-开始）
    /// </summary>
    public DateTime? AcceptedAtStart { get; set; }

    /// <summary>
    /// 验收时间（范围查询-结束）
    /// </summary>
    public DateTime? AcceptedAtEnd { get; set; }

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
// 创建Maintenance DTO
// ========================================

/// <summary>
/// 创建Maintenance DTO
/// </summary>
public class TaktMaintenanceCreateDto
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
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "设备编码（冗余字段,便于查询）不能为空")]
    public string EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期
    /// </summary>
    public DateTime MaintenanceDate { get; set; }

    /// <summary>
    /// 维护开始时间
    /// </summary>
    public DateTime? MaintenanceStartTime { get; set; }

    /// <summary>
    /// 维护结束时间
    /// </summary>
    public DateTime? MaintenanceEndTime { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON格式，存储使用的配件列表）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int MaintenanceStatus { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

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
// 更新Maintenance DTO
// ========================================

/// <summary>
/// 更新Maintenance DTO
/// 继承 TaktMaintenanceCreateDto，添加 MaintenanceId 字段
/// </summary>
public class TaktMaintenanceUpdateDto : TaktMaintenanceCreateDto
{
    /// <summary>
    /// MaintenanceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceId { get; set; }

}

// ========================================
// Maintenance 状态 DTO
// ========================================

/// <summary>
/// Maintenance 状态更新 DTO
/// </summary>
public class TaktMaintenanceStatusDto
{
    /// <summary>
    /// MaintenanceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceId { get; set; }

    /// <summary>
    /// 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    [Required(ErrorMessage = "维护状态（0=待执行，1=执行中，2=已完成，3=已取消）不能为空")]
    public int MaintenanceStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Maintenance 导入模板行 DTO
/// </summary>
public class TaktMaintenanceTemplateDto
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
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int? MaintenanceType { get; set; }

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON格式，存储使用的配件列表）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int? MaintenanceResult { get; set; }

    /// <summary>
    /// 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int? MaintenanceStatus { get; set; }

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
/// Maintenance 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaintenanceImportDto
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
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int? MaintenanceType { get; set; }

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON格式，存储使用的配件列表）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int? MaintenanceResult { get; set; }

    /// <summary>
    /// 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int? MaintenanceStatus { get; set; }

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
/// Maintenance 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaintenanceExportDto
{
    /// <summary>
    /// MaintenanceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期
    /// </summary>
    public DateTime MaintenanceDate { get; set; }

    /// <summary>
    /// 维护开始时间
    /// </summary>
    public DateTime? MaintenanceStartTime { get; set; }

    /// <summary>
    /// 维护结束时间
    /// </summary>
    public DateTime? MaintenanceEndTime { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON格式，存储使用的配件列表）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int MaintenanceStatus { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 验收人
    /// </summary>
    public string? AcceptedBy { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

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
