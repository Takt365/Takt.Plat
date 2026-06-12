// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityOperation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityOperation 生成，请按需审阅）
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
// QualityOperation 响应 DTO
// ========================================

/// <summary>
/// 品质业务主表,用于记录品质业务的基础信息(年月、顾客)及汇总数据
/// 对应前端 TaktQualityOperationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityOperationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityOperationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string OperationMonth { get; set; } = string.Empty;

    /// <summary>
    /// 顾客名
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// 记录者
    /// </summary>
    public string? Recorder { get; set; } = string.Empty;

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    public string CostCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 来料检验费用明细列表
    /// （子表：TaktQualityOperationIncoming）
    /// </summary>
    public List<TaktQualityOperationIncomingDto>? IncomingItems { get; set; }

    /// <summary>
    /// 初期/定期检定费用明细列表
    /// （子表：TaktQualityOperationFirstArticle）
    /// </summary>
    public List<TaktQualityOperationFirstArticleDto>? FirstArticleItems { get; set; }

    /// <summary>
    /// 设备校正费用明细列表
    /// （子表：TaktQualityOperationCalibration）
    /// </summary>
    public List<TaktQualityOperationCalibrationDto>? CalibrationItems { get; set; }

    /// <summary>
    /// 其他通常业务费用明细列表
    /// （子表：TaktQualityOperationOther）
    /// </summary>
    public List<TaktQualityOperationOtherDto>? OtherItems { get; set; }

    /// <summary>
    /// 出货检验费用明细列表
    /// （子表：TaktQualityOperationOutgoing）
    /// </summary>
    public List<TaktQualityOperationOutgoingDto>? OutgoingItems { get; set; }

    /// <summary>
    /// 信赖性评价/ORT费用明细列表
    /// （子表：TaktQualityOperationReliability）
    /// </summary>
    public List<TaktQualityOperationReliabilityDto>? ReliabilityItems { get; set; }

    /// <summary>
    /// 顾客品质要求对应费用明细列表
    /// （子表：TaktQualityOperationCustomerResponse）
    /// </summary>
    public List<TaktQualityOperationCustomerResponseDto>? CustomerResponseItems { get; set; }

}

// ========================================
// QualityOperation 查询 DTO
// ========================================

/// <summary>
/// QualityOperation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityOperationQueryDto : TaktPagedQuery
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
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string? QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string? OperationMonth { get; set; } = string.Empty;

    /// <summary>
    /// 顾客名
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// 记录者
    /// </summary>
    public string? Recorder { get; set; } = string.Empty;

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal? TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
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
// 创建QualityOperation DTO
// ========================================

/// <summary>
/// 创建QualityOperation DTO
/// </summary>
public class TaktQualityOperationCreateDto
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
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    [Required(ErrorMessage = "品质业务编码(唯一,如:QO-2026-0001)不能为空")]
    public string QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    [Required(ErrorMessage = "业务年月(格式:2026-05)不能为空")]
    public string OperationMonth { get; set; } = string.Empty;

    /// <summary>
    /// 顾客名
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// 记录者
    /// </summary>
    public string? Recorder { get; set; } = string.Empty;

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    [Required(ErrorMessage = "成本币种(CNY/USD/JPY等)不能为空")]
    public string CostCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 来料检验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityOperationIncomingCreateDto>? IncomingItems { get; set; }

    /// <summary>
    /// 初期/定期检定费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityOperationFirstArticleCreateDto>? FirstArticleItems { get; set; }

    /// <summary>
    /// 设备校正费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityOperationCalibrationCreateDto>? CalibrationItems { get; set; }

    /// <summary>
    /// 其他通常业务费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityOperationOtherCreateDto>? OtherItems { get; set; }

    /// <summary>
    /// 出货检验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityOperationOutgoingCreateDto>? OutgoingItems { get; set; }

    /// <summary>
    /// 信赖性评价/ORT费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityOperationReliabilityCreateDto>? ReliabilityItems { get; set; }

    /// <summary>
    /// 顾客品质要求对应费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityOperationCustomerResponseCreateDto>? CustomerResponseItems { get; set; }

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
// 更新QualityOperation DTO
// ========================================

/// <summary>
/// 更新QualityOperation DTO
/// 继承 TaktQualityOperationCreateDto，添加 QualityOperationId 字段
/// </summary>
public class TaktQualityOperationUpdateDto : TaktQualityOperationCreateDto
{
    /// <summary>
    /// QualityOperationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityOperation 导入模板行 DTO
/// </summary>
public class TaktQualityOperationTemplateDto
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
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string? QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string? OperationMonth { get; set; } = string.Empty;

    /// <summary>
    /// 顾客名
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// 记录者
    /// </summary>
    public string? Recorder { get; set; } = string.Empty;

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
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
/// QualityOperation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityOperationImportDto
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
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string? QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string? OperationMonth { get; set; } = string.Empty;

    /// <summary>
    /// 顾客名
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// 记录者
    /// </summary>
    public string? Recorder { get; set; } = string.Empty;

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
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
/// QualityOperation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityOperationExportDto
{
    /// <summary>
    /// QualityOperationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityOperationId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string QualityOperationCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string OperationMonth { get; set; } = string.Empty;

    /// <summary>
    /// 顾客名
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// 记录者
    /// </summary>
    public string? Recorder { get; set; } = string.Empty;

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
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
