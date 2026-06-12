// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaOutputDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPcbaOutputDetail 生成，请按需审阅）
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
// PcbaOutputDetail 响应 DTO
// ========================================

/// <summary>
/// PCBA明细实体
/// 对应前端 TaktPcbaOutputDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPcbaOutputDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PcbaOutputDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputDetailId { get; set; }

    /// <summary>
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

    /// <summary>
    /// PCBA日报名称（填充字段）
    /// </summary>
    public string? PcbaOutputName { get; set; }

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
    /// 班组
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 板别（PCB板别）
    /// </summary>
    public string PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别
    /// </summary>
    public string PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数
    /// </summary>
    public decimal TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（0=未完成 1=部分完成 2=已完成）
    /// </summary>
    public int CompletedStatus { get; set; } = 0;

    /// <summary>
    /// 序列号
    /// </summary>
    public string SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int DefectCount { get; set; } = 0;

    /// <summary>
    /// 投入工数(分钟)
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int SwitchCount { get; set; } = 0;

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal TotalMinutes { get; set; }

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

    /// <summary>
    /// PCBA日报（主表）
    /// （主表：TaktPcbaOutput）
    /// </summary>
    public TaktPcbaOutputDto? PcbaOutput { get; set; }

}

// ========================================
// PcbaOutputDetail 查询 DTO
// ========================================

/// <summary>
/// PcbaOutputDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPcbaOutputDetailQueryDto : TaktPagedQuery
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
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaOutputId { get; set; }

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
    /// 班组
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 板别（PCB板别）
    /// </summary>
    public string? PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别
    /// </summary>
    public string? PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal? BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal? DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数
    /// </summary>
    public decimal? TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（0=未完成 1=部分完成 2=已完成）
    /// </summary>
    public int? CompletedStatus { get; set; }

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int? DefectCount { get; set; }

    /// <summary>
    /// 投入工数(分钟)
    /// </summary>
    public decimal? InputMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal? RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? SwitchCount { get; set; }

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal? SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal? StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal? TotalMinutes { get; set; }

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

    /// <summary>
    /// 未达成说明
    /// </summary>
    public string? UnachievedDescription { get; set; } = string.Empty;

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
// 创建PcbaOutputDetail DTO
// ========================================

/// <summary>
/// 创建PcbaOutputDetail DTO
/// </summary>
public class TaktPcbaOutputDetailCreateDto
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
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

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
    /// 班组
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 板别（PCB板别）
    /// </summary>
    [Required(ErrorMessage = "板别（PCB板别）不能为空")]
    public string PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别
    /// </summary>
    [Required(ErrorMessage = "面板别不能为空")]
    public string PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数
    /// </summary>
    public decimal TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（0=未完成 1=部分完成 2=已完成）
    /// </summary>
    public int CompletedStatus { get; set; } = 0;

    /// <summary>
    /// 序列号
    /// </summary>
    [Required(ErrorMessage = "序列号不能为空")]
    public string SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int DefectCount { get; set; } = 0;

    /// <summary>
    /// 投入工数(分钟)
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int SwitchCount { get; set; } = 0;

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal TotalMinutes { get; set; }

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
// 更新PcbaOutputDetail DTO
// ========================================

/// <summary>
/// 更新PcbaOutputDetail DTO
/// 继承 TaktPcbaOutputDetailCreateDto，添加 PcbaOutputDetailId 字段
/// </summary>
public class TaktPcbaOutputDetailUpdateDto : TaktPcbaOutputDetailCreateDto
{
    /// <summary>
    /// PcbaOutputDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputDetailId { get; set; }

}

// ========================================
// PcbaOutputDetail 状态 DTO
// ========================================

/// <summary>
/// PcbaOutputDetail 状态更新 DTO
/// </summary>
public class TaktPcbaOutputDetailStatusDto
{
    /// <summary>
    /// PcbaOutputDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputDetailId { get; set; }

    /// <summary>
    /// 完成状态（0=未完成 1=部分完成 2=已完成）
    /// </summary>
    [Required(ErrorMessage = "完成状态（0=未完成 1=部分完成 2=已完成）不能为空")]
    public int CompletedStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PcbaOutputDetail 导入模板行 DTO
/// </summary>
public class TaktPcbaOutputDetailTemplateDto
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
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaOutputId { get; set; }

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
    /// 班组
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 板别（PCB板别）
    /// </summary>
    public string? PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别
    /// </summary>
    public string? PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 完成状态（0=未完成 1=部分完成 2=已完成）
    /// </summary>
    public int? CompletedStatus { get; set; }

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int? DefectCount { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? SwitchCount { get; set; }

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

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
/// PcbaOutputDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPcbaOutputDetailImportDto
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
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaOutputId { get; set; }

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
    /// 班组
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 板别（PCB板别）
    /// </summary>
    public string? PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别
    /// </summary>
    public string? PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 完成状态（0=未完成 1=部分完成 2=已完成）
    /// </summary>
    public int? CompletedStatus { get; set; }

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int? DefectCount { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? SwitchCount { get; set; }

    /// <summary>
    /// 未达成原因
    /// </summary>
    public string? UnachievedReason { get; set; } = string.Empty;

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
/// PcbaOutputDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPcbaOutputDetailExportDto
{
    /// <summary>
    /// PcbaOutputDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputDetailId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

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
    /// 班组
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 板别（PCB板别）
    /// </summary>
    public string PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别
    /// </summary>
    public string PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    public decimal BatchQty { get; set; }

    /// <summary>
    /// 当日完成数
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 累计完成数
    /// </summary>
    public decimal TotalCompletedQty { get; set; }

    /// <summary>
    /// 完成状态（0=未完成 1=部分完成 2=已完成）
    /// </summary>
    public int CompletedStatus { get; set; } = 0;

    /// <summary>
    /// 序列号
    /// </summary>
    public string SerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    public int DefectCount { get; set; } = 0;

    /// <summary>
    /// 投入工数(分钟)
    /// </summary>
    public decimal InputMinutes { get; set; }

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    public decimal RepairMinutes { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int SwitchCount { get; set; } = 0;

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    public decimal SwitchTime { get; set; }

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    public decimal StopTime { get; set; }

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    public decimal TotalMinutes { get; set; }

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

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
