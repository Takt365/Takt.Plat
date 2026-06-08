// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerSatisfactionSurvey 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCustomerSatisfactionSurvey 生成，请按需审阅）
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
// CustomerSatisfactionSurvey 响应 DTO
// ========================================

/// <summary>
/// 客户满意度调查表主表实体
/// 对应前端 TaktCustomerSatisfactionSurveyDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCustomerSatisfactionSurveyDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CustomerSatisfactionSurveyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyId { get; set; }

    /// <summary>
    /// 调查表编号（组合唯一索引）
    /// </summary>
    public string CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 调查日期
    /// </summary>
    public DateTime SurveyDate { get; set; }

    /// <summary>
    /// 调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）
    /// </summary>
    public int SurveyMethod { get; set; } = 0;

    /// <summary>
    /// 调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）
    /// </summary>
    public int SurveyType { get; set; } = 0;

    /// <summary>
    /// 调查周期（0=月度，1=季度，2=半年度，3=年度）
    /// </summary>
    public int SurveyPeriod { get; set; } = 0;

    /// <summary>
    /// 调查人（人员代码）
    /// </summary>
    public string? SurveyorBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系人
    /// </summary>
    public string? CustomerContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
    /// </summary>
    public int OverallSatisfaction { get; set; } = 0;

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 产品质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付准时率评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 服务质量评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 价格竞争力评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 技术支持评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 客户主要表扬
    /// </summary>
    public string? CustomerPraise { get; set; } = string.Empty;

    /// <summary>
    /// 客户主要意见/建议
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进计划/措施
    /// </summary>
    public string? ImprovementPlan { get; set; } = string.Empty;

    /// <summary>
    /// 调查状态（0=草稿，1=进行中，2=已完成，3=已归档）
    /// </summary>
    public int SurveyStatus { get; set; } = 0;

    /// <summary>
    /// 跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）
    /// </summary>
    public int FollowUpStatus { get; set; } = 0;

    /// <summary>
    /// 关联客诉ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RelatedComplaintId { get; set; }

    /// <summary>
    /// 关联客诉名称（填充字段）
    /// </summary>
    public string? RelatedComplaintName { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 调查项目明细列表（主子表关系）
    /// （子表：TaktCustomerSatisfactionSurveyItem）
    /// </summary>
    public List<TaktCustomerSatisfactionSurveyItemDto>? Items { get; set; }

}

// ========================================
// CustomerSatisfactionSurvey 查询 DTO
// ========================================

/// <summary>
/// CustomerSatisfactionSurvey 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCustomerSatisfactionSurveyQueryDto : TaktPagedQuery
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
    /// 调查表编号（组合唯一索引）
    /// </summary>
    public string? CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 调查日期（范围查询-开始）
    /// </summary>
    public DateTime? SurveyDateStart { get; set; }

    /// <summary>
    /// 调查日期（范围查询-结束）
    /// </summary>
    public DateTime? SurveyDateEnd { get; set; }

    /// <summary>
    /// 调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）
    /// </summary>
    public int? SurveyMethod { get; set; }

    /// <summary>
    /// 调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）
    /// </summary>
    public int? SurveyType { get; set; }

    /// <summary>
    /// 调查周期（0=月度，1=季度，2=半年度，3=年度）
    /// </summary>
    public int? SurveyPeriod { get; set; }

    /// <summary>
    /// 调查人（人员代码）
    /// </summary>
    public string? SurveyorBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系人
    /// </summary>
    public string? CustomerContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
    /// </summary>
    public int? OverallSatisfaction { get; set; }

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 产品质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付准时率评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 服务质量评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 价格竞争力评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 技术支持评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 客户主要表扬
    /// </summary>
    public string? CustomerPraise { get; set; } = string.Empty;

    /// <summary>
    /// 客户主要意见/建议
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进计划/措施
    /// </summary>
    public string? ImprovementPlan { get; set; } = string.Empty;

    /// <summary>
    /// 调查状态（0=草稿，1=进行中，2=已完成，3=已归档）
    /// </summary>
    public int? SurveyStatus { get; set; }

    /// <summary>
    /// 跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）
    /// </summary>
    public int? FollowUpStatus { get; set; }

    /// <summary>
    /// 关联客诉ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RelatedComplaintId { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建CustomerSatisfactionSurvey DTO
// ========================================

/// <summary>
/// 创建CustomerSatisfactionSurvey DTO
/// </summary>
public class TaktCustomerSatisfactionSurveyCreateDto
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
    /// 调查表编号（组合唯一索引）
    /// </summary>
    [Required(ErrorMessage = "调查表编号（组合唯一索引）不能为空")]
    public string CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    [Required(ErrorMessage = "客户名称不能为空")]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 调查日期
    /// </summary>
    public DateTime SurveyDate { get; set; }

    /// <summary>
    /// 调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）
    /// </summary>
    public int SurveyMethod { get; set; } = 0;

    /// <summary>
    /// 调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）
    /// </summary>
    public int SurveyType { get; set; } = 0;

    /// <summary>
    /// 调查周期（0=月度，1=季度，2=半年度，3=年度）
    /// </summary>
    public int SurveyPeriod { get; set; } = 0;

    /// <summary>
    /// 调查人（人员代码）
    /// </summary>
    public string? SurveyorBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系人
    /// </summary>
    public string? CustomerContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
    /// </summary>
    public int OverallSatisfaction { get; set; } = 0;

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 产品质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付准时率评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 服务质量评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 价格竞争力评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 技术支持评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 客户主要表扬
    /// </summary>
    public string? CustomerPraise { get; set; } = string.Empty;

    /// <summary>
    /// 客户主要意见/建议
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进计划/措施
    /// </summary>
    public string? ImprovementPlan { get; set; } = string.Empty;

    /// <summary>
    /// 调查状态（0=草稿，1=进行中，2=已完成，3=已归档）
    /// </summary>
    public int SurveyStatus { get; set; } = 0;

    /// <summary>
    /// 跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）
    /// </summary>
    public int FollowUpStatus { get; set; } = 0;

    /// <summary>
    /// 关联客诉ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RelatedComplaintId { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 调查项目明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktCustomerSatisfactionSurveyItemCreateDto>? Items { get; set; }

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
// 更新CustomerSatisfactionSurvey DTO
// ========================================

/// <summary>
/// 更新CustomerSatisfactionSurvey DTO
/// 继承 TaktCustomerSatisfactionSurveyCreateDto，添加 CustomerSatisfactionSurveyId 字段
/// </summary>
public class TaktCustomerSatisfactionSurveyUpdateDto : TaktCustomerSatisfactionSurveyCreateDto
{
    /// <summary>
    /// CustomerSatisfactionSurveyID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyId { get; set; }

}

// ========================================
// CustomerSatisfactionSurvey 状态 DTO
// ========================================

/// <summary>
/// CustomerSatisfactionSurvey 状态更新 DTO
/// </summary>
public class TaktCustomerSatisfactionSurveyStatusDto
{
    /// <summary>
    /// CustomerSatisfactionSurveyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyId { get; set; }

    /// <summary>
    /// 调查状态（0=草稿，1=进行中，2=已完成，3=已归档）
    /// </summary>
    [Required(ErrorMessage = "调查状态（0=草稿，1=进行中，2=已完成，3=已归档）不能为空")]
    public int SurveyStatus { get; set; } = 0;
}

// ========================================
// CustomerSatisfactionSurvey 排序 DTO
// ========================================

/// <summary>
/// CustomerSatisfactionSurvey 排序更新 DTO
/// </summary>
public class TaktCustomerSatisfactionSurveySortDto
{
    /// <summary>
    /// CustomerSatisfactionSurveyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyId { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// CustomerSatisfactionSurvey 导入模板行 DTO
/// </summary>
public class TaktCustomerSatisfactionSurveyTemplateDto
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
    /// 调查表编号（组合唯一索引）
    /// </summary>
    public string? CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）
    /// </summary>
    public int? SurveyMethod { get; set; }

    /// <summary>
    /// 调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）
    /// </summary>
    public int? SurveyType { get; set; }

    /// <summary>
    /// 调查周期（0=月度，1=季度，2=半年度，3=年度）
    /// </summary>
    public int? SurveyPeriod { get; set; }

    /// <summary>
    /// 调查人（人员代码）
    /// </summary>
    public string? SurveyorBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系人
    /// </summary>
    public string? CustomerContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
    /// </summary>
    public int? OverallSatisfaction { get; set; }

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

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
/// CustomerSatisfactionSurvey 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCustomerSatisfactionSurveyImportDto
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
    /// 调查表编号（组合唯一索引）
    /// </summary>
    public string? CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）
    /// </summary>
    public int? SurveyMethod { get; set; }

    /// <summary>
    /// 调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）
    /// </summary>
    public int? SurveyType { get; set; }

    /// <summary>
    /// 调查周期（0=月度，1=季度，2=半年度，3=年度）
    /// </summary>
    public int? SurveyPeriod { get; set; }

    /// <summary>
    /// 调查人（人员代码）
    /// </summary>
    public string? SurveyorBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系人
    /// </summary>
    public string? CustomerContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
    /// </summary>
    public int? OverallSatisfaction { get; set; }

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

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
/// CustomerSatisfactionSurvey 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCustomerSatisfactionSurveyExportDto
{
    /// <summary>
    /// CustomerSatisfactionSurveyID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 调查表编号（组合唯一索引）
    /// </summary>
    public string CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 调查日期
    /// </summary>
    public DateTime SurveyDate { get; set; }

    /// <summary>
    /// 调查方式（0=问卷，1=电话，2=邮件，3=现场，4=在线）
    /// </summary>
    public int SurveyMethod { get; set; } = 0;

    /// <summary>
    /// 调查类型（0=定期调查，1=专项调查，2=投诉后调查，3=其他）
    /// </summary>
    public int SurveyType { get; set; } = 0;

    /// <summary>
    /// 调查周期（0=月度，1=季度，2=半年度，3=年度）
    /// </summary>
    public int SurveyPeriod { get; set; } = 0;

    /// <summary>
    /// 调查人（人员代码）
    /// </summary>
    public string? SurveyorBy { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系人
    /// </summary>
    public string? CustomerContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户联系电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 整体满意度（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
    /// </summary>
    public int OverallSatisfaction { get; set; } = 0;

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 产品质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付准时率评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 服务质量评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 价格竞争力评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 技术支持评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 客户主要表扬
    /// </summary>
    public string? CustomerPraise { get; set; } = string.Empty;

    /// <summary>
    /// 客户主要意见/建议
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进计划/措施
    /// </summary>
    public string? ImprovementPlan { get; set; } = string.Empty;

    /// <summary>
    /// 调查状态（0=草稿，1=进行中，2=已完成，3=已归档）
    /// </summary>
    public int SurveyStatus { get; set; } = 0;

    /// <summary>
    /// 跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）
    /// </summary>
    public int FollowUpStatus { get; set; } = 0;

    /// <summary>
    /// 关联客诉ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RelatedComplaintId { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
