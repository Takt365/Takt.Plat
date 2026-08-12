// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotificationDeliveryDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：EcNotificationDelivery 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcNotificationDelivery 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// EcNotificationDelivery 响应 DTO
// ========================================

/// <summary>
/// 确认时间
/// 对应前端 TaktEcNotificationDeliveryDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcNotificationDeliveryDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcNotificationDeliveryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationDeliveryId { get; set; }

    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 通知单 名称（填充字段）
    /// </summary>
    public string? EcNotificationName { get; set; }

    /// <summary>
    /// 通知单号（冗余）
    /// </summary>
    public string EcNotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变 名称（填充字段）
    /// </summary>
    public string? EcName { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（TaktDept.DeptCode，如 D0710、D0810）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门名称（冗余）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（1=普通，2=高，3=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 投递状态（0=待发送，1=已发送，2=已确认）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

}

// ========================================
// EcNotificationDelivery 查询 DTO
// ========================================

/// <summary>
/// EcNotificationDelivery 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcNotificationDeliveryQueryDto : TaktPagedQuery
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
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationId { get; set; }

    /// <summary>
    /// 通知单号（冗余）
    /// </summary>
    public string? EcNotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（TaktDept.DeptCode，如 D0710、D0810）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门名称（冗余）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（1=普通，2=高，3=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 投递状态（0=待发送，1=已发送，2=已确认）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 发送时间（范围查询-开始）
    /// </summary>
    public DateTime? SentAtStart { get; set; }

    /// <summary>
    /// 发送时间（范围查询-结束）
    /// </summary>
    public DateTime? SentAtEnd { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 确认时间（范围查询-开始）
    /// </summary>
    public DateTime? ConfirmedAtStart { get; set; }

    /// <summary>
    /// 确认时间（范围查询-结束）
    /// </summary>
    public DateTime? ConfirmedAtEnd { get; set; }

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
// 创建EcNotificationDelivery DTO
// ========================================

/// <summary>
/// 创建EcNotificationDelivery DTO
/// </summary>
public class TaktEcNotificationDeliveryCreateDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 通知单号（冗余）
    /// </summary>
    [Required(ErrorMessage = "通知单号（冗余）不能为空")]
    public string EcNotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余）不能为空")]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（TaktDept.DeptCode，如 D0710、D0810）
    /// </summary>
    [Required(ErrorMessage = "目标部门编码（TaktDept.DeptCode，如 D0710、D0810）不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门名称（冗余）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（1=普通，2=高，3=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 投递状态（0=待发送，1=已发送，2=已确认）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

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
// 更新EcNotificationDelivery DTO
// ========================================

/// <summary>
/// 更新EcNotificationDelivery DTO
/// 继承 TaktEcNotificationDeliveryCreateDto，添加 EcNotificationDeliveryId 字段
/// </summary>
public class TaktEcNotificationDeliveryUpdateDto : TaktEcNotificationDeliveryCreateDto
{
    /// <summary>
    /// EcNotificationDeliveryID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationDeliveryId { get; set; }

}

// ========================================
// EcNotificationDelivery 状态 DTO
// ========================================

/// <summary>
/// EcNotificationDelivery 状态更新 DTO
/// </summary>
public class TaktEcNotificationDeliveryStatusDto
{
    /// <summary>
    /// EcNotificationDeliveryID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationDeliveryId { get; set; }

    /// <summary>
    /// 投递状态（0=待发送，1=已发送，2=已确认）
    /// </summary>
    [Required(ErrorMessage = "投递状态（0=待发送，1=已发送，2=已确认）不能为空")]
    public int DeliveryStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcNotificationDelivery 导入模板行 DTO
/// </summary>
public class TaktEcNotificationDeliveryTemplateDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationId { get; set; }

    /// <summary>
    /// 通知单号（冗余）
    /// </summary>
    public string? EcNotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（TaktDept.DeptCode，如 D0710、D0810）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门名称（冗余）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（1=普通，2=高，3=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 投递状态（0=待发送，1=已发送，2=已确认）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

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
/// EcNotificationDelivery 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcNotificationDeliveryImportDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationId { get; set; }

    /// <summary>
    /// 通知单号（冗余）
    /// </summary>
    public string? EcNotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（TaktDept.DeptCode，如 D0710、D0810）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门名称（冗余）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（1=普通，2=高，3=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 投递状态（0=待发送，1=已发送，2=已确认）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

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
/// EcNotificationDelivery 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcNotificationDeliveryExportDto
{
    /// <summary>
    /// EcNotificationDeliveryID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationDeliveryId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 通知单号（冗余）
    /// </summary>
    public string EcNotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（TaktDept.DeptCode，如 D0710、D0810）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门名称（冗余）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（1=普通，2=高，3=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 投递状态（0=待发送，1=已发送，2=已确认）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

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
