// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintHandlingDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerComplaintHandling 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCustomerComplaintHandling 生成，请按需审阅）
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
// CustomerComplaintHandling 响应 DTO
// ========================================

/// <summary>
/// 客诉处理记录实体
/// 对应前端 TaktCustomerComplaintHandlingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCustomerComplaintHandlingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CustomerComplaintHandlingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintHandlingId { get; set; }

    /// <summary>
    /// 客诉处理记录编码（唯一索引）
    /// </summary>
    public string ComplaintHandlingCode { get; set; } = string.Empty;

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
    public string ComplaintNo { get; set; } = string.Empty;

    /// <summary>
    /// 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintItemId { get; set; }

    /// <summary>
    /// 客诉明细名称（填充字段）
    /// </summary>
    public string? ComplaintItemName { get; set; }

    /// <summary>
    /// 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
    /// </summary>
    public int HandlingStage { get; set; } = 0;

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
    /// </summary>
    public int HandlingMethod { get; set; } = 0;

    /// <summary>
    /// 处理说明
    /// </summary>
    public string HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 预防措施
    /// </summary>
    public string? PreventiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDept { get; set; } = string.Empty;

    /// <summary>
    /// 责任人（人员代码）
    /// </summary>
    public string? ResponsibleBy { get; set; } = string.Empty;

    /// <summary>
    /// 处理人（人员代码）
    /// </summary>
    public string? HandlerBy { get; set; } = string.Empty;

    /// <summary>
    /// 处理时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
    /// </summary>
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 处理成本/损失金额
    /// </summary>
    public decimal? HandlingCost { get; set; }

    /// <summary>
    /// 客户反馈
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
    /// </summary>
    public int? CustomerSatisfaction { get; set; }

    /// <summary>
    /// 附件路径（JSON格式，存储相关文件URL列表）
    /// </summary>
    public string? AttachmentPaths { get; set; } = string.Empty;

    /// <summary>
    /// 客诉主表
    /// （主表：TaktCustomerComplaint）
    /// </summary>
    public TaktCustomerComplaintDto? Complaint { get; set; }

}

// ========================================
// CustomerComplaintHandling 查询 DTO
// ========================================

/// <summary>
/// CustomerComplaintHandling 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCustomerComplaintHandlingQueryDto : TaktPagedQuery
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
    /// 客诉处理记录编码（唯一索引）
    /// </summary>
    public string? ComplaintHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    public string? ComplaintNo { get; set; } = string.Empty;

    /// <summary>
    /// 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintItemId { get; set; }

    /// <summary>
    /// 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
    /// </summary>
    public int? HandlingStage { get; set; }

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
    /// </summary>
    public int? HandlingMethod { get; set; }

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 预防措施
    /// </summary>
    public string? PreventiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDept { get; set; } = string.Empty;

    /// <summary>
    /// 责任人（人员代码）
    /// </summary>
    public string? ResponsibleBy { get; set; } = string.Empty;

    /// <summary>
    /// 处理人（人员代码）
    /// </summary>
    public string? HandlerBy { get; set; } = string.Empty;

    /// <summary>
    /// 处理时间（范围查询-开始）
    /// </summary>
    public DateTime? HandlingAtStart { get; set; }

    /// <summary>
    /// 处理时间（范围查询-结束）
    /// </summary>
    public DateTime? HandlingAtEnd { get; set; }

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
    /// 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
    /// </summary>
    public int? HandlingStatus { get; set; }

    /// <summary>
    /// 处理成本/损失金额
    /// </summary>
    public decimal? HandlingCost { get; set; }

    /// <summary>
    /// 客户反馈
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
    /// </summary>
    public int? CustomerSatisfaction { get; set; }

    /// <summary>
    /// 附件路径（JSON格式，存储相关文件URL列表）
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
// 创建CustomerComplaintHandling DTO
// ========================================

/// <summary>
/// 创建CustomerComplaintHandling DTO
/// </summary>
public class TaktCustomerComplaintHandlingCreateDto
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
    /// 客诉处理记录编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "客诉处理记录编码（唯一索引）不能为空")]
    public string ComplaintHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "客诉单号（冗余字段，便于查询）不能为空")]
    public string ComplaintNo { get; set; } = string.Empty;

    /// <summary>
    /// 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintItemId { get; set; }

    /// <summary>
    /// 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
    /// </summary>
    public int HandlingStage { get; set; } = 0;

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
    /// </summary>
    public int HandlingMethod { get; set; } = 0;

    /// <summary>
    /// 处理说明
    /// </summary>
    [Required(ErrorMessage = "处理说明不能为空")]
    public string HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 预防措施
    /// </summary>
    public string? PreventiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDept { get; set; } = string.Empty;

    /// <summary>
    /// 责任人（人员代码）
    /// </summary>
    public string? ResponsibleBy { get; set; } = string.Empty;

    /// <summary>
    /// 处理人（人员代码）
    /// </summary>
    public string? HandlerBy { get; set; } = string.Empty;

    /// <summary>
    /// 处理时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
    /// </summary>
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 处理成本/损失金额
    /// </summary>
    public decimal? HandlingCost { get; set; }

    /// <summary>
    /// 客户反馈
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
    /// </summary>
    public int? CustomerSatisfaction { get; set; }

    /// <summary>
    /// 附件路径（JSON格式，存储相关文件URL列表）
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
// 更新CustomerComplaintHandling DTO
// ========================================

/// <summary>
/// 更新CustomerComplaintHandling DTO
/// 继承 TaktCustomerComplaintHandlingCreateDto，添加 CustomerComplaintHandlingId 字段
/// </summary>
public class TaktCustomerComplaintHandlingUpdateDto : TaktCustomerComplaintHandlingCreateDto
{
    /// <summary>
    /// CustomerComplaintHandlingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintHandlingId { get; set; }

}

// ========================================
// CustomerComplaintHandling 状态 DTO
// ========================================

/// <summary>
/// CustomerComplaintHandling 状态更新 DTO
/// </summary>
public class TaktCustomerComplaintHandlingStatusDto
{
    /// <summary>
    /// CustomerComplaintHandlingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintHandlingId { get; set; }

    /// <summary>
    /// 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
    /// </summary>
    [Required(ErrorMessage = "处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）不能为空")]
    public int HandlingStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// CustomerComplaintHandling 导入模板行 DTO
/// </summary>
public class TaktCustomerComplaintHandlingTemplateDto
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
    /// 客诉处理记录编码（唯一索引）
    /// </summary>
    public string? ComplaintHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    public string? ComplaintNo { get; set; } = string.Empty;

    /// <summary>
    /// 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintItemId { get; set; }

    /// <summary>
    /// 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
    /// </summary>
    public int? HandlingStage { get; set; }

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
    /// </summary>
    public int? HandlingMethod { get; set; }

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 预防措施
    /// </summary>
    public string? PreventiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDept { get; set; } = string.Empty;

    /// <summary>
    /// 责任人（人员代码）
    /// </summary>
    public string? ResponsibleBy { get; set; } = string.Empty;

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
/// CustomerComplaintHandling 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCustomerComplaintHandlingImportDto
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
    /// 客诉处理记录编码（唯一索引）
    /// </summary>
    public string? ComplaintHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    public string? ComplaintNo { get; set; } = string.Empty;

    /// <summary>
    /// 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintItemId { get; set; }

    /// <summary>
    /// 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
    /// </summary>
    public int? HandlingStage { get; set; }

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
    /// </summary>
    public int? HandlingMethod { get; set; }

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 预防措施
    /// </summary>
    public string? PreventiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDept { get; set; } = string.Empty;

    /// <summary>
    /// 责任人（人员代码）
    /// </summary>
    public string? ResponsibleBy { get; set; } = string.Empty;

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
/// CustomerComplaintHandling 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCustomerComplaintHandlingExportDto
{
    /// <summary>
    /// CustomerComplaintHandlingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintHandlingId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉处理记录编码（唯一索引）
    /// </summary>
    public string ComplaintHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }

    /// <summary>
    /// 客诉单号（冗余字段，便于查询）
    /// </summary>
    public string ComplaintNo { get; set; } = string.Empty;

    /// <summary>
    /// 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintItemId { get; set; }

    /// <summary>
    /// 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
    /// </summary>
    public int HandlingStage { get; set; } = 0;

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
    /// </summary>
    public int HandlingMethod { get; set; } = 0;

    /// <summary>
    /// 处理说明
    /// </summary>
    public string HandlingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 原因分析
    /// </summary>
    public string? CauseAnalysis { get; set; } = string.Empty;

    /// <summary>
    /// 改善对策/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 预防措施
    /// </summary>
    public string? PreventiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门
    /// </summary>
    public string? ResponsibleDept { get; set; } = string.Empty;

    /// <summary>
    /// 责任人（人员代码）
    /// </summary>
    public string? ResponsibleBy { get; set; } = string.Empty;

    /// <summary>
    /// 处理人（人员代码）
    /// </summary>
    public string? HandlerBy { get; set; } = string.Empty;

    /// <summary>
    /// 处理时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 计划完成日期
    /// </summary>
    public DateTime? PlannedCompletionDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
    /// </summary>
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 处理成本/损失金额
    /// </summary>
    public decimal? HandlingCost { get; set; }

    /// <summary>
    /// 客户反馈
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
    /// </summary>
    public int? CustomerSatisfaction { get; set; }

    /// <summary>
    /// 附件路径（JSON格式，存储相关文件URL列表）
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
