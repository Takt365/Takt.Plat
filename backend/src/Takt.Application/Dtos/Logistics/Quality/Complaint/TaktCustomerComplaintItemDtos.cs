// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintItemDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerComplaintItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCustomerComplaintItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Complaint;

// ========================================
// CustomerComplaintItem 响应 DTO
// ========================================

/// <summary>
/// 客诉明细实体
/// 对应前端 TaktCustomerComplaintItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCustomerComplaintItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CustomerComplaintItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintItemId { get; set; }

    /// <summary>
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }

    /// <summary>
    /// 客诉名称（填充字段）
    /// </summary>
    public string? ComplaintName { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    public string CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（CR=严重，MA=主要，MI=次要）
    /// </summary>
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善责任人
    /// </summary>
    public string? ImprovementResponsible { get; set; } = string.Empty;

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
    /// </summary>
    public int ImprovementStatus { get; set; } = 0;

    /// <summary>
    /// 附件路径（多个附件用逗号分隔）
    /// </summary>
    public string? AttachmentPaths { get; set; } = string.Empty;

    /// <summary>
    /// 客诉主表
    /// （主表：TaktCustomerComplaint）
    /// </summary>
    public TaktCustomerComplaintDto? Complaint { get; set; }

}

// ========================================
// CustomerComplaintItem 查询 DTO
// ========================================

/// <summary>
/// CustomerComplaintItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCustomerComplaintItemQueryDto : TaktPagedQuery
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
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    public string? CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 产品编码
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（CR=严重，MA=主要，MI=次要）
    /// </summary>
    public string? DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善责任人
    /// </summary>
    public string? ImprovementResponsible { get; set; } = string.Empty;

    /// <summary>
    /// 计划完成日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedCompletionDateStart { get; set; }

    /// <summary>
    /// 计划完成日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedCompletionDateEnd { get; set; }

    /// <summary>
    /// 实际完成日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualCompletionDateStart { get; set; }

    /// <summary>
    /// 实际完成日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualCompletionDateEnd { get; set; }

    /// <summary>
    /// 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
    /// </summary>
    public int? ImprovementStatus { get; set; }

    /// <summary>
    /// 附件路径（多个附件用逗号分隔）
    /// </summary>
    public string? AttachmentPaths { get; set; } = string.Empty;

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
// 创建CustomerComplaintItem DTO
// ========================================

/// <summary>
/// 创建CustomerComplaintItem DTO
/// </summary>
public class TaktCustomerComplaintItemCreateDto
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
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "客诉单号（冗余字段，便于查询）不能为空")]
    public string CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    [Required(ErrorMessage = "不良现象描述不能为空")]
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（CR=严重，MA=主要，MI=次要）
    /// </summary>
    [Required(ErrorMessage = "缺点等级（CR=严重，MA=主要，MI=次要）不能为空")]
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善责任人
    /// </summary>
    public string? ImprovementResponsible { get; set; } = string.Empty;

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
    /// </summary>
    public int ImprovementStatus { get; set; } = 0;

    /// <summary>
    /// 附件路径（多个附件用逗号分隔）
    /// </summary>
    public string? AttachmentPaths { get; set; } = string.Empty;

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
// 更新CustomerComplaintItem DTO
// ========================================

/// <summary>
/// 更新CustomerComplaintItem DTO
/// 继承 TaktCustomerComplaintItemCreateDto，添加 CustomerComplaintItemId 字段
/// </summary>
public class TaktCustomerComplaintItemUpdateDto : TaktCustomerComplaintItemCreateDto
{
    /// <summary>
    /// CustomerComplaintItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintItemId { get; set; }

}

// ========================================
// CustomerComplaintItem 状态 DTO
// ========================================

/// <summary>
/// CustomerComplaintItem 状态更新 DTO
/// </summary>
public class TaktCustomerComplaintItemStatusDto
{
    /// <summary>
    /// CustomerComplaintItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintItemId { get; set; }

    /// <summary>
    /// 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
    /// </summary>
    [Required(ErrorMessage = "改善状态（0=待改善，1=改善中，2=已完成，3=已验证）不能为空")]
    public int ImprovementStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// CustomerComplaintItem 导入模板行 DTO
/// </summary>
public class TaktCustomerComplaintItemTemplateDto
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
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    public string? CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 产品编码
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（CR=严重，MA=主要，MI=次要）
    /// </summary>
    public string? DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

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
/// CustomerComplaintItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCustomerComplaintItemImportDto
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
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    public string? CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 产品编码
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
    /// </summary>
    public int? ItemType { get; set; }

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（CR=严重，MA=主要，MI=次要）
    /// </summary>
    public string? DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

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
/// CustomerComplaintItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCustomerComplaintItemExportDto
{
    /// <summary>
    /// CustomerComplaintItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    public string CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
    /// </summary>
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 缺点等级（CR=严重，MA=主要，MI=次要）
    /// </summary>
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal? DefectRate { get; set; }

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策
    /// </summary>
    public string? ImprovementAction { get; set; } = string.Empty;

    /// <summary>
    /// 改善责任人
    /// </summary>
    public string? ImprovementResponsible { get; set; } = string.Empty;

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
    /// </summary>
    public int ImprovementStatus { get; set; } = 0;

    /// <summary>
    /// 附件路径（多个附件用逗号分隔）
    /// </summary>
    public string? AttachmentPaths { get; set; } = string.Empty;

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
