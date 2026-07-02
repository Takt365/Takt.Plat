// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktEquipmentOperationRateChangeLogDtos.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：EquipmentOperationRateChangeLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEquipmentOperationRateChangeLog 生成，请按需审阅）
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
// EquipmentOperationRateChangeLog 响应 DTO
// ========================================

/// <summary>
/// 机器稼动率变更记录实体
/// 对应前端 TaktEquipmentOperationRateChangeLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEquipmentOperationRateChangeLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EquipmentOperationRateChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateChangeLogId { get; set; }

    /// <summary>
    /// 机器稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateId { get; set; }

    /// <summary>
    /// 机器稼动率名称（填充字段）
    /// </summary>
    public string? EquipmentOperationRateName { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    public string EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 机器稼动率主表
    /// （主表：TaktEquipmentOperationRate）
    /// </summary>
    public TaktEquipmentOperationRateDto? EquipmentOperationRate { get; set; }

}

// ========================================
// EquipmentOperationRateChangeLog 查询 DTO
// ========================================

/// <summary>
/// EquipmentOperationRateChangeLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEquipmentOperationRateChangeLogQueryDto : TaktPagedQuery
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
    /// 机器稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentOperationRateId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间（范围查询-开始）
    /// </summary>
    public DateTime? ChangeTimeStart { get; set; }

    /// <summary>
    /// 变更时间（范围查询-结束）
    /// </summary>
    public DateTime? ChangeTimeEnd { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

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
// 创建EquipmentOperationRateChangeLog DTO
// ========================================

/// <summary>
/// 创建EquipmentOperationRateChangeLog DTO
/// </summary>
public class TaktEquipmentOperationRateChangeLogCreateDto
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
    /// 机器稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    [Required(ErrorMessage = "设备编码（冗余）不能为空")]
    public string EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

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
// 更新EquipmentOperationRateChangeLog DTO
// ========================================

/// <summary>
/// 更新EquipmentOperationRateChangeLog DTO
/// 继承 TaktEquipmentOperationRateChangeLogCreateDto，添加 EquipmentOperationRateChangeLogId 字段
/// </summary>
public class TaktEquipmentOperationRateChangeLogUpdateDto : TaktEquipmentOperationRateChangeLogCreateDto
{
    /// <summary>
    /// EquipmentOperationRateChangeLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateChangeLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// EquipmentOperationRateChangeLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEquipmentOperationRateChangeLogExportDto
{
    /// <summary>
    /// EquipmentOperationRateChangeLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateChangeLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 机器稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentOperationRateId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    public string EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

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
