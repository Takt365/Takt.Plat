// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetailDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyOutputDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssyOutputDetail 生成，请按需审阅）
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
// AssyOutputDetail 响应 DTO
// ========================================

/// <summary>
/// 组立日报明细（产出子表）实体
/// 对应前端 TaktAssyOutputDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssyOutputDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssyOutputDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputDetailId { get; set; }

    /// <summary>
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 组立日报名称（填充字段）
    /// </summary>
    public string? AssyOutputName { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产时段
    /// </summary>
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int DowntimeMinutes { get; set; } = 0;

    /// <summary>
    /// 停线原因
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 生产工时(分钟)
    /// </summary>
    public decimal ProdMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)
    /// </summary>
    public decimal ActualMinutes { get; set; }

    /// <summary>
    /// 达成率(%)
    /// </summary>
    public decimal AchievementRate { get; set; }

    /// <summary>
    /// 组立日报（主表）
    /// （主表：TaktAssyOutput）
    /// </summary>
    public TaktAssyOutputDto? AssyOutput { get; set; }

}

// ========================================
// AssyOutputDetail 查询 DTO
// ========================================

/// <summary>
/// AssyOutputDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssyOutputDetailQueryDto : TaktPagedQuery
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
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyOutputId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产时段
    /// </summary>
    public string? TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal? ProdActualQty { get; set; }

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int? DowntimeMinutes { get; set; }

    /// <summary>
    /// 停线原因
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 生产工时(分钟)
    /// </summary>
    public decimal? ProdMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)
    /// </summary>
    public decimal? ActualMinutes { get; set; }

    /// <summary>
    /// 达成率(%)
    /// </summary>
    public decimal? AchievementRate { get; set; }

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
// 创建AssyOutputDetail DTO
// ========================================

/// <summary>
/// 创建AssyOutputDetail DTO
/// </summary>
public class TaktAssyOutputDetailCreateDto
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
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "生产工单号（冗余字段,便于查询）不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产时段
    /// </summary>
    [Required(ErrorMessage = "生产时段不能为空")]
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int DowntimeMinutes { get; set; } = 0;

    /// <summary>
    /// 停线原因
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 生产工时(分钟)
    /// </summary>
    public decimal ProdMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)
    /// </summary>
    public decimal ActualMinutes { get; set; }

    /// <summary>
    /// 达成率(%)
    /// </summary>
    public decimal AchievementRate { get; set; }

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
// 更新AssyOutputDetail DTO
// ========================================

/// <summary>
/// 更新AssyOutputDetail DTO
/// 继承 TaktAssyOutputDetailCreateDto，添加 AssyOutputDetailId 字段
/// </summary>
public class TaktAssyOutputDetailUpdateDto : TaktAssyOutputDetailCreateDto
{
    /// <summary>
    /// AssyOutputDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputDetailId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AssyOutputDetail 导入模板行 DTO
/// </summary>
public class TaktAssyOutputDetailTemplateDto
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
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyOutputId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产时段
    /// </summary>
    public string? TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int? DowntimeMinutes { get; set; }

    /// <summary>
    /// 停线原因
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

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
/// AssyOutputDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssyOutputDetailImportDto
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
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyOutputId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产时段
    /// </summary>
    public string? TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int? DowntimeMinutes { get; set; }

    /// <summary>
    /// 停线原因
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

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
/// AssyOutputDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssyOutputDetailExportDto
{
    /// <summary>
    /// AssyOutputDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputDetailId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产时段
    /// </summary>
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 实际生产数量
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    public int DowntimeMinutes { get; set; } = 0;

    /// <summary>
    /// 停线原因
    /// </summary>
    public string? DowntimeReason { get; set; } = string.Empty;

    /// <summary>
    /// 停线说明
    /// </summary>
    public string? DowntimeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// 投入工时(分钟)
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 生产工时(分钟)
    /// </summary>
    public decimal ProdMinutes { get; set; }

    /// <summary>
    /// 实际工时(分钟)
    /// </summary>
    public decimal ActualMinutes { get; set; }

    /// <summary>
    /// 达成率(%)
    /// </summary>
    public decimal AchievementRate { get; set; }

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
