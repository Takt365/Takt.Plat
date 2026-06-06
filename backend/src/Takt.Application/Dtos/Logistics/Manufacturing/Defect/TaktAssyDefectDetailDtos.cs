// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectDetailDtos.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyDefectDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssyDefectDetail 生成，请按需审阅）
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
// AssyDefectDetail 响应 DTO
// ========================================

/// <summary>
/// 组立不良明细实体
/// 对应前端 TaktAssyDefectDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssyDefectDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssyDefectDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectDetailId { get; set; }

    /// <summary>
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectId { get; set; }

    /// <summary>
    /// 组立不良日报名称（填充字段）
    /// </summary>
    public string? AssyDefectName { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良区分
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员
    /// </summary>
    public string? RepairOperator { get; set; } = string.Empty;

    /// <summary>
    /// 组立不良日报（主表）
    /// （主表：TaktAssyDefect）
    /// </summary>
    public TaktAssyDefectDto? AssyDefect { get; set; }

}

// ========================================
// AssyDefectDetail 查询 DTO
// ========================================

/// <summary>
/// AssyDefectDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssyDefectDetailQueryDto : TaktPagedQuery
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
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyDefectId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良区分
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal? DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal? CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员
    /// </summary>
    public string? RepairOperator { get; set; } = string.Empty;

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
// 创建AssyDefectDetail DTO
// ========================================

/// <summary>
/// 创建AssyDefectDetail DTO
/// </summary>
public class TaktAssyDefectDetailCreateDto
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
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectId { get; set; }

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
    /// 不良区分
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员
    /// </summary>
    public string? RepairOperator { get; set; } = string.Empty;

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
// 更新AssyDefectDetail DTO
// ========================================

/// <summary>
/// 更新AssyDefectDetail DTO
/// 继承 TaktAssyDefectDetailCreateDto，添加 AssyDefectDetailId 字段
/// </summary>
public class TaktAssyDefectDetailUpdateDto : TaktAssyDefectDetailCreateDto
{
    /// <summary>
    /// AssyDefectDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectDetailId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// AssyDefectDetail 导入模板行 DTO
/// </summary>
public class TaktAssyDefectDetailTemplateDto
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
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyDefectId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良区分
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员
    /// </summary>
    public string? RepairOperator { get; set; } = string.Empty;

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
/// AssyDefectDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAssyDefectDetailImportDto
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
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssyDefectId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 不良区分
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员
    /// </summary>
    public string? RepairOperator { get; set; } = string.Empty;

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
/// AssyDefectDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssyDefectDetailExportDto
{
    /// <summary>
    /// AssyDefectDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectDetailId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectId { get; set; }

    /// <summary>
    /// 生产工单号（冗余字段,便于查询）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良区分
    /// </summary>
    public string? DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 累计不良
    /// </summary>
    public decimal CumulativeDefectQty { get; set; }

    /// <summary>
    /// 随机卡号
    /// </summary>
    public string? RandomCardNo { get; set; } = string.Empty;

    /// <summary>
    /// 发生工程
    /// </summary>
    public string? OccurrenceEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 测试步骤
    /// </summary>
    public string? TestStep { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string? DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所
    /// </summary>
    public string? DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string? DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 修理员
    /// </summary>
    public string? RepairOperator { get; set; } = string.Empty;

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
