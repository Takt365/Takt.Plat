// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktIqcDefectHandlingDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：IqcDefectHandling 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktIqcDefectHandling 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

// ========================================
// IqcDefectHandling 响应 DTO
// ========================================

/// <summary>
/// IQC进货检验不良处理记录实体
/// 对应前端 TaktIqcDefectHandlingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktIqcDefectHandlingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// IqcDefectHandlingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcDefectHandlingId { get; set; }

    /// <summary>
    /// IQC不良处理编码
    /// </summary>
    public string IqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderItemId { get; set; }

    /// <summary>
    /// IQC检验单明细名称（填充字段）
    /// </summary>
    public string? IqcOrderItemName { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int DefectType { get; set; } = 0;

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int HandlingMethod { get; set; } = 0;

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

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
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

    /// <summary>
    /// IQC检验单明细（主表）
    /// （主表：TaktIqcOrderItem）
    /// </summary>
    public TaktIqcOrderItemDto? OrderItem { get; set; }

}

// ========================================
// IqcDefectHandling 查询 DTO
// ========================================

/// <summary>
/// IqcDefectHandling 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktIqcDefectHandlingQueryDto : TaktPagedQuery
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
    /// IQC不良处理编码
    /// </summary>
    public string? IqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? IqcOrderItemId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int? DefectType { get; set; }

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string? DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int? HandlingMethod { get; set; }

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

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
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int? HandlingStatus { get; set; }

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

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
// 创建IqcDefectHandling DTO
// ========================================

/// <summary>
/// 创建IqcDefectHandling DTO
/// </summary>
public class TaktIqcDefectHandlingCreateDto
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
    /// IQC不良处理编码
    /// </summary>
    [Required(ErrorMessage = "IQC不良处理编码不能为空")]
    public string IqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderItemId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "IQC检验单编码（冗余字段，便于查询）不能为空")]
    public string IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int DefectType { get; set; } = 0;

    /// <summary>
    /// 不良现象编码
    /// </summary>
    [Required(ErrorMessage = "不良现象编码不能为空")]
    public string DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    [Required(ErrorMessage = "不良现象描述不能为空")]
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int HandlingMethod { get; set; } = 0;

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

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
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

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
// 更新IqcDefectHandling DTO
// ========================================

/// <summary>
/// 更新IqcDefectHandling DTO
/// 继承 TaktIqcDefectHandlingCreateDto，添加 IqcDefectHandlingId 字段
/// </summary>
public class TaktIqcDefectHandlingUpdateDto : TaktIqcDefectHandlingCreateDto
{
    /// <summary>
    /// IqcDefectHandlingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcDefectHandlingId { get; set; }

}

// ========================================
// IqcDefectHandling 状态 DTO
// ========================================

/// <summary>
/// IqcDefectHandling 状态更新 DTO
/// </summary>
public class TaktIqcDefectHandlingStatusDto
{
    /// <summary>
    /// IqcDefectHandlingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcDefectHandlingId { get; set; }

    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    [Required(ErrorMessage = "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）不能为空")]
    public int HandlingStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// IqcDefectHandling 导入模板行 DTO
/// </summary>
public class TaktIqcDefectHandlingTemplateDto
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
    /// IQC不良处理编码
    /// </summary>
    public string? IqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? IqcOrderItemId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int? DefectType { get; set; }

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string? DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int? HandlingMethod { get; set; }

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

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
/// IqcDefectHandling 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktIqcDefectHandlingImportDto
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
    /// IQC不良处理编码
    /// </summary>
    public string? IqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? IqcOrderItemId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int? DefectType { get; set; }

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string? DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string? DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int? DefectQuantity { get; set; }

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int? HandlingMethod { get; set; }

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

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
/// IqcDefectHandling 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktIqcDefectHandlingExportDto
{
    /// <summary>
    /// IqcDefectHandlingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcDefectHandlingId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// IQC不良处理编码
    /// </summary>
    public string IqcDefectHandlingCode { get; set; } = string.Empty;

    /// <summary>
    /// IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IqcOrderItemId { get; set; }

    /// <summary>
    /// IQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string IqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    public int DefectType { get; set; } = 0;

    /// <summary>
    /// 不良现象编码
    /// </summary>
    public string DefectCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良现象描述
    /// </summary>
    public string DefectDescription { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public int DefectQuantity { get; set; } = 0;

    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    public int HandlingMethod { get; set; } = 0;

    /// <summary>
    /// 处理说明
    /// </summary>
    public string? HandlingDescription { get; set; } = string.Empty;

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
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    public string? CorrectiveAction { get; set; } = string.Empty;

    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    public string? DefectImages { get; set; } = string.Empty;

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
