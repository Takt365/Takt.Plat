// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityFailurePcbaReworkDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityFailurePcbaRework 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityFailurePcbaRework 生成，请按需审阅）
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
// QualityFailurePcbaRework 响应 DTO
// ========================================

/// <summary>
/// 品质问题应对明细 - PCBA不良改修应对(PCBA选别・改修费用)
/// 对应前端 TaktQualityFailurePcbaReworkDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityFailurePcbaReworkDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityFailurePcbaReworkID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityFailurePcbaReworkId { get; set; }

    /// <summary>
    /// 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityFailureId { get; set; }

    /// <summary>
    /// 品质问题主表名称（填充字段）
    /// </summary>
    public string? QualityFailureName { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int PcbaReworkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int PcbaReinspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA不良改修应对记录者
    /// </summary>
    public string? PcbaRecorder { get; set; } = string.Empty;

    /// <summary>
    /// 质量问题主表（导航属性）
    /// （主表：TaktQualityFailure）
    /// </summary>
    public TaktQualityFailureDto? Issue { get; set; }

}

// ========================================
// QualityFailurePcbaRework 查询 DTO
// ========================================

/// <summary>
/// QualityFailurePcbaRework 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityFailurePcbaReworkQueryDto : TaktPagedQuery
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
    /// 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityFailureId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal? PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int? PcbaReworkTimeMinutes { get; set; }

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int? PcbaReinspectionTimeMinutes { get; set; }

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal? PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal? PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal? PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal? PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal? PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA不良改修应对记录者
    /// </summary>
    public string? PcbaRecorder { get; set; } = string.Empty;

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
// 创建QualityFailurePcbaRework DTO
// ========================================

/// <summary>
/// 创建QualityFailurePcbaRework DTO
/// </summary>
public class TaktQualityFailurePcbaReworkCreateDto
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
    /// 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityFailureId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "品质问题编码（冗余字段，便于查询）不能为空")]
    public string QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int PcbaReworkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int PcbaReinspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA不良改修应对记录者
    /// </summary>
    public string? PcbaRecorder { get; set; } = string.Empty;

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
// 更新QualityFailurePcbaRework DTO
// ========================================

/// <summary>
/// 更新QualityFailurePcbaRework DTO
/// 继承 TaktQualityFailurePcbaReworkCreateDto，添加 QualityFailurePcbaReworkId 字段
/// </summary>
public class TaktQualityFailurePcbaReworkUpdateDto : TaktQualityFailurePcbaReworkCreateDto
{
    /// <summary>
    /// QualityFailurePcbaReworkID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityFailurePcbaReworkId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityFailurePcbaRework 导入模板行 DTO
/// </summary>
public class TaktQualityFailurePcbaReworkTemplateDto
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
    /// 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityFailureId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int? PcbaReworkTimeMinutes { get; set; }

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int? PcbaReinspectionTimeMinutes { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA不良改修应对记录者
    /// </summary>
    public string? PcbaRecorder { get; set; } = string.Empty;

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
/// QualityFailurePcbaRework 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityFailurePcbaReworkImportDto
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
    /// 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityFailureId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int? PcbaReworkTimeMinutes { get; set; }

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int? PcbaReinspectionTimeMinutes { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA不良改修应对记录者
    /// </summary>
    public string? PcbaRecorder { get; set; } = string.Empty;

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
/// QualityFailurePcbaRework 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityFailurePcbaReworkExportDto
{
    /// <summary>
    /// QualityFailurePcbaReworkID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityFailurePcbaReworkId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityFailureId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余字段，便于查询）
    /// </summary>
    public string QualityFailureCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    public string? PcbaDefectParts { get; set; } = string.Empty;

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    public decimal PcbaReworkCost { get; set; }

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    public int PcbaReworkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    public int PcbaReinspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    public decimal PcbaTravelCost { get; set; }

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    public decimal PcbaWarehouseCost { get; set; }

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses { get; set; }

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    public string? PcbaReworkNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    public decimal PcbaScrapCost { get; set; }

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    public string? PcbaCustomerName { get; set; } = string.Empty;

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    public string? PcbaDebitNoteNo { get; set; } = string.Empty;

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    public decimal PcbaOtherExpenses2 { get; set; }

    /// <summary>
    /// PCBA备注
    /// </summary>
    public string? PcbaNote { get; set; } = string.Empty;

    /// <summary>
    /// PCBA不良改修应对记录者
    /// </summary>
    public string? PcbaRecorder { get; set; } = string.Empty;

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
