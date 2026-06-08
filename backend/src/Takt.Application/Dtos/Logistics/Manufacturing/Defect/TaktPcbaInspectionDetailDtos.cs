// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDetailDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaInspectionDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPcbaInspectionDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Defect;

// ========================================
// PcbaInspectionDetail 响应 DTO
// ========================================

/// <summary>
/// PCBA检查明细实体
/// 对应前端 TaktPcbaInspectionDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPcbaInspectionDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PcbaInspectionDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionDetailId { get; set; }

    /// <summary>
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

    /// <summary>
    /// PCBA检查日报名称（填充字段）
    /// </summary>
    public string? PcbaInspectionName { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA板别
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 目视线别
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期
    /// </summary>
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T面实装日期
    /// </summary>
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 检查员
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal InspectionQty { get; set; }

    /// <summary>
    /// 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
    /// </summary>
    public int InspectionStatus { get; set; } = 0;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// PCBA检查日报（主表）
    /// （主表：TaktPcbaInspection）
    /// </summary>
    public TaktPcbaInspectionDto? PcbaInspection { get; set; }

}

// ========================================
// PcbaInspectionDetail 查询 DTO
// ========================================

/// <summary>
/// PcbaInspectionDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPcbaInspectionDetailQueryDto : TaktPagedQuery
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
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaInspectionId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA板别
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 目视线别
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期（范围查询-开始）
    /// </summary>
    public DateTime? BSideAssemblyDateStart { get; set; }

    /// <summary>
    /// B面实装日期（范围查询-结束）
    /// </summary>
    public DateTime? BSideAssemblyDateEnd { get; set; }

    /// <summary>
    /// T面实装日期（范围查询-开始）
    /// </summary>
    public DateTime? TSideAssemblyDateStart { get; set; }

    /// <summary>
    /// T面实装日期（范围查询-结束）
    /// </summary>
    public DateTime? TSideAssemblyDateEnd { get; set; }

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 检查员
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal? DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal? InspectionQty { get; set; }

    /// <summary>
    /// 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
    /// </summary>
    public int? InspectionStatus { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal? InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal? AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

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
// 创建PcbaInspectionDetail DTO
// ========================================

/// <summary>
/// 创建PcbaInspectionDetail DTO
/// </summary>
public class TaktPcbaInspectionDetailCreateDto
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
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

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
    /// PCBA板别
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 目视线别
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期
    /// </summary>
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T面实装日期
    /// </summary>
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 检查员
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal InspectionQty { get; set; }

    /// <summary>
    /// 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
    /// </summary>
    public int InspectionStatus { get; set; } = 0;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

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
// 更新PcbaInspectionDetail DTO
// ========================================

/// <summary>
/// 更新PcbaInspectionDetail DTO
/// 继承 TaktPcbaInspectionDetailCreateDto，添加 PcbaInspectionDetailId 字段
/// </summary>
public class TaktPcbaInspectionDetailUpdateDto : TaktPcbaInspectionDetailCreateDto
{
    /// <summary>
    /// PcbaInspectionDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionDetailId { get; set; }

}

// ========================================
// PcbaInspectionDetail 状态 DTO
// ========================================

/// <summary>
/// PcbaInspectionDetail 状态更新 DTO
/// </summary>
public class TaktPcbaInspectionDetailStatusDto
{
    /// <summary>
    /// PcbaInspectionDetailID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionDetailId { get; set; }

    /// <summary>
    /// 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
    /// </summary>
    [Required(ErrorMessage = "检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)不能为空")]
    public int InspectionStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PcbaInspectionDetail 导入模板行 DTO
/// </summary>
public class TaktPcbaInspectionDetailTemplateDto
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
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaInspectionId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA板别
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 目视线别
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 检查员
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
    /// </summary>
    public int? InspectionStatus { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

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
/// PcbaInspectionDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPcbaInspectionDetailImportDto
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
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PcbaInspectionId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// PCBA板别
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 目视线别
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 检查员
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
    /// </summary>
    public int? InspectionStatus { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

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
/// PcbaInspectionDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPcbaInspectionDetailExportDto
{
    /// <summary>
    /// PcbaInspectionDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionDetailId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA板别
    /// </summary>
    public string? PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 目视线别
    /// </summary>
    public string? VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI线别
    /// </summary>
    public string? AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B面实装日期
    /// </summary>
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T面实装日期
    /// </summary>
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次(1=早班 2=中班 3=晚班)
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 检查员
    /// </summary>
    public string? InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal InspectionQty { get; set; }

    /// <summary>
    /// 检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)
    /// </summary>
    public int InspectionStatus { get; set; } = 0;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProdLine { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI工数
    /// </summary>
    public decimal AoiWorkHours { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string? HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

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
