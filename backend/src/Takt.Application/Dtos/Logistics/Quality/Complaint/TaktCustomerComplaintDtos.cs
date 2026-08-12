// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerComplaint 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCustomerComplaint 生成，请按需审阅）
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
// CustomerComplaint 响应 DTO
// ========================================

/// <summary>
/// 客诉主表实体
/// 对应前端 TaktCustomerComplaintDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCustomerComplaintDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CustomerComplaintID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintId { get; set; }


    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 客诉状态（字典 logistics_quality_complaint_status）
    /// </summary>
    public int ComplaintStatus { get; set; } = 0;

    /// <summary>
    /// 客诉明细列表（主子表关系）
    /// （子表：TaktCustomerComplaintItem）
    /// </summary>
    public List<TaktCustomerComplaintItemDto>? Items { get; set; }

}

// ========================================
// CustomerComplaint 查询 DTO
// ========================================

/// <summary>
/// CustomerComplaint 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCustomerComplaintQueryDto : TaktPagedQuery
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
    /// 客诉单号（组合唯一索引）
    /// </summary>
    public string? CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户 ID（选项 TaktCustomers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CustomerId { get; set; }

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 投诉日期（范围查询-开始）
    /// </summary>
    public DateTime? ComplaintDateStart { get; set; }

    /// <summary>
    /// 投诉日期（范围查询-结束）
    /// </summary>
    public DateTime? ComplaintDateEnd { get; set; }

    /// <summary>
    /// 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
    /// </summary>
    public int? ComplaintMethod { get; set; }

    /// <summary>
    /// 投诉类型（字典 logistics_quality_complaint_type）
    /// </summary>
    public int? ComplaintType { get; set; }

    /// <summary>
    /// 投诉等级（字典 logistics_quality_complaint_level）
    /// </summary>
    public int? ComplaintLevel { get; set; }

    /// <summary>
    /// 责任部门 ID（选项 TaktDepts/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsibleDeptId { get; set; }

    /// <summary>
    /// 责任部门名称
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsiblePersonId { get; set; }

    /// <summary>
    /// 责任人姓名
    /// </summary>
    public string? ResponsiblePersonName { get; set; } = string.Empty;

    /// <summary>
    /// 要求回复日期（范围查询-开始）
    /// </summary>
    public DateTime? RequiredReplyDateStart { get; set; }

    /// <summary>
    /// 要求回复日期（范围查询-结束）
    /// </summary>
    public DateTime? RequiredReplyDateEnd { get; set; }

    /// <summary>
    /// 实际回复日期（范围查询-开始）
    /// </summary>
    public DateTime? ActualReplyDateStart { get; set; }

    /// <summary>
    /// 实际回复日期（范围查询-结束）
    /// </summary>
    public DateTime? ActualReplyDateEnd { get; set; }

    /// <summary>
    /// 客诉描述
    /// </summary>
    public string? ComplaintDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果/回复内容
    /// </summary>
    public string? HandlingResult { get; set; } = string.Empty;

    /// <summary>
    /// 客户满意度（字典 logistics_quality_customer_satisfaction）
    /// </summary>
    public int? CustomerSatisfaction { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 客诉状态（字典 logistics_quality_complaint_status）
    /// </summary>
    public int? ComplaintStatus { get; set; }

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
// 创建CustomerComplaint DTO
// ========================================

/// <summary>
/// 创建CustomerComplaint DTO
/// </summary>
public class TaktCustomerComplaintCreateDto
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
    /// 客诉单号（组合唯一索引）
    /// </summary>
    [Required(ErrorMessage = "客诉单号（组合唯一索引）不能为空")]
    public string CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户 ID（选项 TaktCustomers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    [Required(ErrorMessage = "客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）不能为空")]
    public string CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 投诉日期
    /// </summary>
    public DateTime ComplaintDate { get; set; }

    /// <summary>
    /// 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
    /// </summary>
    public int ComplaintMethod { get; set; } = 0;

    /// <summary>
    /// 投诉类型（字典 logistics_quality_complaint_type）
    /// </summary>
    public int ComplaintType { get; set; } = 0;

    /// <summary>
    /// 投诉等级（字典 logistics_quality_complaint_level）
    /// </summary>
    public int ComplaintLevel { get; set; } = 0;

    /// <summary>
    /// 责任部门 ID（选项 TaktDepts/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsibleDeptId { get; set; }

    /// <summary>
    /// 责任部门名称
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsiblePersonId { get; set; }

    /// <summary>
    /// 责任人姓名
    /// </summary>
    public string? ResponsiblePersonName { get; set; } = string.Empty;

    /// <summary>
    /// 要求回复日期
    /// </summary>
    public DateTime? RequiredReplyDate { get; set; }

    /// <summary>
    /// 实际回复日期
    /// </summary>
    public DateTime? ActualReplyDate { get; set; }

    /// <summary>
    /// 客诉描述
    /// </summary>
    [Required(ErrorMessage = "客诉描述不能为空")]
    public string ComplaintDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果/回复内容
    /// </summary>
    public string? HandlingResult { get; set; } = string.Empty;

    /// <summary>
    /// 客户满意度（字典 logistics_quality_customer_satisfaction）
    /// </summary>
    public int? CustomerSatisfaction { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "关联工厂（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉状态（字典 logistics_quality_complaint_status）
    /// </summary>
    public int ComplaintStatus { get; set; } = 0;

    /// <summary>
    /// 客诉明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktCustomerComplaintItemCreateDto>? Items { get; set; }

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
// 更新CustomerComplaint DTO
// ========================================

/// <summary>
/// 更新CustomerComplaint DTO
/// 继承 TaktCustomerComplaintCreateDto，添加 CustomerComplaintId 字段
/// </summary>
public class TaktCustomerComplaintUpdateDto : TaktCustomerComplaintCreateDto
{
    /// <summary>
    /// CustomerComplaintID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintId { get; set; }

    /// <summary>
    /// 客诉明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktCustomerComplaintItemUpdateDto>? Items { get; set; }

}

// ========================================
// CustomerComplaint 状态 DTO
// ========================================

/// <summary>
/// CustomerComplaint 状态更新 DTO
/// </summary>
public class TaktCustomerComplaintStatusDto
{
    /// <summary>
    /// CustomerComplaintID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintId { get; set; }

    /// <summary>
    /// 客诉状态（字典 logistics_quality_complaint_status）
    /// </summary>
    [Required(ErrorMessage = "客诉状态（字典 logistics_quality_complaint_status）不能为空")]
    public int ComplaintStatus { get; set; } = 0;
}

// ========================================
// CustomerComplaint 排序 DTO
// ========================================

/// <summary>
/// CustomerComplaint 排序更新 DTO
/// </summary>
public class TaktCustomerComplaintSortDto
{
    /// <summary>
    /// CustomerComplaintID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintId { get; set; }

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
/// CustomerComplaint 导入模板行 DTO
/// </summary>
public class TaktCustomerComplaintTemplateDto
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
    /// 客诉单号（组合唯一索引）
    /// </summary>
    public string? CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户 ID（选项 TaktCustomers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CustomerId { get; set; }

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 投诉日期
    /// </summary>
    public DateTime? ComplaintDate { get; set; }

    /// <summary>
    /// 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
    /// </summary>
    public int? ComplaintMethod { get; set; }

    /// <summary>
    /// 投诉类型（字典 logistics_quality_complaint_type）
    /// </summary>
    public int? ComplaintType { get; set; }

    /// <summary>
    /// 投诉等级（字典 logistics_quality_complaint_level）
    /// </summary>
    public int? ComplaintLevel { get; set; }

    /// <summary>
    /// 责任部门 ID（选项 TaktDepts/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsibleDeptId { get; set; }

    /// <summary>
    /// 责任部门名称
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsiblePersonId { get; set; }

    /// <summary>
    /// 责任人姓名
    /// </summary>
    public string? ResponsiblePersonName { get; set; } = string.Empty;

    /// <summary>
    /// 要求回复日期
    /// </summary>
    public DateTime? RequiredReplyDate { get; set; }

    /// <summary>
    /// 实际回复日期
    /// </summary>
    public DateTime? ActualReplyDate { get; set; }

    /// <summary>
    /// 客诉描述
    /// </summary>
    public string? ComplaintDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果/回复内容
    /// </summary>
    public string? HandlingResult { get; set; } = string.Empty;

    /// <summary>
    /// 客户满意度（字典 logistics_quality_customer_satisfaction）
    /// </summary>
    public int? CustomerSatisfaction { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉状态（字典 logistics_quality_complaint_status）
    /// </summary>
    public int? ComplaintStatus { get; set; }

    /// <summary>
    /// 客诉明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktCustomerComplaintItemCreateDto>? Items { get; set; }

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
/// CustomerComplaint 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCustomerComplaintImportDto
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
    /// 客诉单号（组合唯一索引）
    /// </summary>
    public string? CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户 ID（选项 TaktCustomers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CustomerId { get; set; }

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 投诉日期
    /// </summary>
    public DateTime? ComplaintDate { get; set; }

    /// <summary>
    /// 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
    /// </summary>
    public int? ComplaintMethod { get; set; }

    /// <summary>
    /// 投诉类型（字典 logistics_quality_complaint_type）
    /// </summary>
    public int? ComplaintType { get; set; }

    /// <summary>
    /// 投诉等级（字典 logistics_quality_complaint_level）
    /// </summary>
    public int? ComplaintLevel { get; set; }

    /// <summary>
    /// 责任部门 ID（选项 TaktDepts/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsibleDeptId { get; set; }

    /// <summary>
    /// 责任部门名称
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsiblePersonId { get; set; }

    /// <summary>
    /// 责任人姓名
    /// </summary>
    public string? ResponsiblePersonName { get; set; } = string.Empty;

    /// <summary>
    /// 要求回复日期
    /// </summary>
    public DateTime? RequiredReplyDate { get; set; }

    /// <summary>
    /// 实际回复日期
    /// </summary>
    public DateTime? ActualReplyDate { get; set; }

    /// <summary>
    /// 客诉描述
    /// </summary>
    public string? ComplaintDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果/回复内容
    /// </summary>
    public string? HandlingResult { get; set; } = string.Empty;

    /// <summary>
    /// 客户满意度（字典 logistics_quality_customer_satisfaction）
    /// </summary>
    public int? CustomerSatisfaction { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉状态（字典 logistics_quality_complaint_status）
    /// </summary>
    public int? ComplaintStatus { get; set; }

    /// <summary>
    /// 客诉明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktCustomerComplaintItemCreateDto>? Items { get; set; }

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
/// CustomerComplaint 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCustomerComplaintExportDto
{
    /// <summary>
    /// CustomerComplaintID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerComplaintId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉单号（组合唯一索引）
    /// </summary>
    public string CustomerComplaintCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户 ID（选项 TaktCustomers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 投诉日期
    /// </summary>
    public DateTime ComplaintDate { get; set; }

    /// <summary>
    /// 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
    /// </summary>
    public int ComplaintMethod { get; set; } = 0;

    /// <summary>
    /// 投诉类型（字典 logistics_quality_complaint_type）
    /// </summary>
    public int ComplaintType { get; set; } = 0;

    /// <summary>
    /// 投诉等级（字典 logistics_quality_complaint_level）
    /// </summary>
    public int ComplaintLevel { get; set; } = 0;

    /// <summary>
    /// 责任部门 ID（选项 TaktDepts/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsibleDeptId { get; set; }

    /// <summary>
    /// 责任部门名称
    /// </summary>
    public string? ResponsibleDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 责任人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsiblePersonId { get; set; }

    /// <summary>
    /// 责任人姓名
    /// </summary>
    public string? ResponsiblePersonName { get; set; } = string.Empty;

    /// <summary>
    /// 要求回复日期
    /// </summary>
    public DateTime? RequiredReplyDate { get; set; }

    /// <summary>
    /// 实际回复日期
    /// </summary>
    public DateTime? ActualReplyDate { get; set; }

    /// <summary>
    /// 客诉描述
    /// </summary>
    public string ComplaintDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理结果/回复内容
    /// </summary>
    public string? HandlingResult { get; set; } = string.Empty;

    /// <summary>
    /// 客户满意度（字典 logistics_quality_customer_satisfaction）
    /// </summary>
    public int? CustomerSatisfaction { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 客诉状态（字典 logistics_quality_complaint_status）
    /// </summary>
    public int ComplaintStatus { get; set; } = 0;

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
