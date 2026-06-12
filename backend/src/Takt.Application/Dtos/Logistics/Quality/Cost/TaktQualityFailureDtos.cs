// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityFailureDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityFailure 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityFailure 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Cost;

// ========================================
// QualityFailure 响应 DTO
// ========================================

/// <summary>
/// 品质问题应对主表,用于记录质量问题的基础信息(年月日、机种、批次)及汇总数据
/// 对应前端 TaktQualityFailureDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityFailureDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityFailureID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityFailureId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期
    /// </summary>
    public DateTime FailureDate { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int TotalTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string CostCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 会议/调查/试验费用明细列表
    /// （子表：TaktQualityFailureMeeting）
    /// </summary>
    public List<TaktQualityFailureMeetingDto>? MeetingItems { get; set; }

    /// <summary>
    /// 组装不良改修应对明细列表
    /// （子表：TaktQualityFailureAssyRework）
    /// </summary>
    public List<TaktQualityFailureAssyReworkDto>? AssyReworkItems { get; set; }

    /// <summary>
    /// PCBA不良改修应对明细列表
    /// （子表：TaktQualityFailurePcbaRework）
    /// </summary>
    public List<TaktQualityFailurePcbaReworkDto>? PcbaReworkItems { get; set; }

}

// ========================================
// QualityFailure 查询 DTO
// ========================================

/// <summary>
/// QualityFailure 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityFailureQueryDto : TaktPagedQuery
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
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string? QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期（范围查询-开始）
    /// </summary>
    public DateTime? FailureDateStart { get; set; }

    /// <summary>
    /// 问题日期（范围查询-结束）
    /// </summary>
    public DateTime? FailureDateEnd { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string? Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string? Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int? TotalTimeMinutes { get; set; }

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string? CostCurrency { get; set; } = string.Empty;

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
// 创建QualityFailure DTO
// ========================================

/// <summary>
/// 创建QualityFailure DTO
/// </summary>
public class TaktQualityFailureCreateDto
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
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    [Required(ErrorMessage = "品质问题编码（唯一，如：QF-2026-0001）不能为空")]
    public string QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期
    /// </summary>
    public DateTime FailureDate { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    [Required(ErrorMessage = "机种/产品型号不能为空")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    [Required(ErrorMessage = "批次号/Lot No不能为空")]
    public string Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int TotalTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    [Required(ErrorMessage = "成本币种（CNY/USD/JPY等）不能为空")]
    public string CostCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 会议/调查/试验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityFailureMeetingCreateDto>? MeetingItems { get; set; }

    /// <summary>
    /// 组装不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityFailureAssyReworkCreateDto>? AssyReworkItems { get; set; }

    /// <summary>
    /// PCBA不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityFailurePcbaReworkCreateDto>? PcbaReworkItems { get; set; }

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
// 更新QualityFailure DTO
// ========================================

/// <summary>
/// 更新QualityFailure DTO
/// 继承 TaktQualityFailureCreateDto，添加 QualityFailureId 字段
/// </summary>
public class TaktQualityFailureUpdateDto : TaktQualityFailureCreateDto
{
    /// <summary>
    /// QualityFailureID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityFailureId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityFailure 导入模板行 DTO
/// </summary>
public class TaktQualityFailureTemplateDto
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
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string? QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string? Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string? Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int? TotalTimeMinutes { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string? CostCurrency { get; set; } = string.Empty;

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
/// QualityFailure 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityFailureImportDto
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
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string? QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string? Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string? Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int? TotalTimeMinutes { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string? CostCurrency { get; set; } = string.Empty;

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
/// QualityFailure 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityFailureExportDto
{
    /// <summary>
    /// QualityFailureID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityFailureId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期
    /// </summary>
    public DateTime FailureDate { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int TotalTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string CostCurrency { get; set; } = string.Empty;

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
