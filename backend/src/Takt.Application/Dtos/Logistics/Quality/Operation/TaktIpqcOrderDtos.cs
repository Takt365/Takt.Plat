// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：IpqcOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktIpqcOrder 生成，请按需审阅）
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
// IpqcOrder 响应 DTO
// ========================================

/// <summary>
/// IPQC制程检验单实体
/// 对应前端 TaktIpqcOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktIpqcOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// IpqcOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcOrderId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源单号（生产工单编码）
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// IPQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 生产总数
    /// </summary>
    public decimal TotalProductionQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int TotalSampleQuantity { get; set; } = 0;

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int TotalQualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int TotalUnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeBy { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期
    /// </summary>
    public DateTime? JudgeDate { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单明细列表（主子表关系）
    /// （子表：TaktIpqcOrderItem）
    /// </summary>
    public List<TaktIpqcOrderItemDto>? Items { get; set; }

    /// <summary>
    /// 变更日志列表（主子表关系）
    /// （子表：TaktIpqcOrderChangeLog）
    /// </summary>
    public List<TaktIpqcOrderChangeLogDto>? ChangeLogs { get; set; }

}

// ========================================
// IpqcOrder 查询 DTO
// ========================================

/// <summary>
/// IpqcOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktIpqcOrderQueryDto : TaktPagedQuery
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
    /// 来源单号（生产工单编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期（范围查询-开始）
    /// </summary>
    public DateTime? InspectionDateStart { get; set; }

    /// <summary>
    /// 检验日期（范围查询-结束）
    /// </summary>
    public DateTime? InspectionDateEnd { get; set; }

    /// <summary>
    /// IPQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string? IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 生产总数
    /// </summary>
    public decimal? TotalProductionQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int? TotalSampleQuantity { get; set; }

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int? TotalQualifiedQuantity { get; set; }

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int? TotalUnqualifiedQuantity { get; set; }

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal? TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeBy { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期（范围查询-开始）
    /// </summary>
    public DateTime? JudgeDateStart { get; set; }

    /// <summary>
    /// 判定日期（范围查询-结束）
    /// </summary>
    public DateTime? JudgeDateEnd { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

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
// 创建IpqcOrder DTO
// ========================================

/// <summary>
/// 创建IpqcOrder DTO
/// </summary>
public class TaktIpqcOrderCreateDto
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
    /// 来源单号（生产工单编码）
    /// </summary>
    [Required(ErrorMessage = "来源单号（生产工单编码）不能为空")]
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// IPQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    [Required(ErrorMessage = "IPQC检验单编码（唯一索引，根据来源单号自动生成）不能为空")]
    public string IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    [Required(ErrorMessage = "工序编码不能为空")]
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    [Required(ErrorMessage = "工序名称不能为空")]
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 生产总数
    /// </summary>
    public decimal TotalProductionQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int TotalSampleQuantity { get; set; } = 0;

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int TotalQualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int TotalUnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeBy { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期
    /// </summary>
    public DateTime? JudgeDate { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktIpqcOrderItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 变更日志列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktIpqcOrderChangeLogCreateDto>? ChangeLogs { get; set; }

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
// 更新IpqcOrder DTO
// ========================================

/// <summary>
/// 更新IpqcOrder DTO
/// 继承 TaktIpqcOrderCreateDto，添加 IpqcOrderId 字段
/// </summary>
public class TaktIpqcOrderUpdateDto : TaktIpqcOrderCreateDto
{
    /// <summary>
    /// IpqcOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcOrderId { get; set; }

}

// ========================================
// IpqcOrder 状态 DTO
// ========================================

/// <summary>
/// IpqcOrder 状态更新 DTO
/// </summary>
public class TaktIpqcOrderStatusDto
{
    /// <summary>
    /// IpqcOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcOrderId { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
    /// </summary>
    [Required(ErrorMessage = "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）不能为空")]
    public int JudgeStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// IpqcOrder 导入模板行 DTO
/// </summary>
public class TaktIpqcOrderTemplateDto
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
    /// 来源单号（生产工单编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string? IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int? TotalSampleQuantity { get; set; }

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int? TotalQualifiedQuantity { get; set; }

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int? TotalUnqualifiedQuantity { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeBy { get; set; } = string.Empty;

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

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
/// IpqcOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktIpqcOrderImportDto
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
    /// 来源单号（生产工单编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// IPQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string? IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int? TotalSampleQuantity { get; set; }

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int? TotalQualifiedQuantity { get; set; }

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int? TotalUnqualifiedQuantity { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeBy { get; set; } = string.Empty;

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

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
/// IpqcOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktIpqcOrderExportDto
{
    /// <summary>
    /// IpqcOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcOrderId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源单号（生产工单编码）
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// IPQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    public string IpqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 生产总数
    /// </summary>
    public decimal TotalProductionQuantity { get; set; }

    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    public int TotalSampleQuantity { get; set; } = 0;

    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    public int TotalQualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    public int TotalUnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    public decimal TotalInspectionReturnQuantity { get; set; }

    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// 判定人（人员代码）
    /// </summary>
    public string? JudgeBy { get; set; } = string.Empty;

    /// <summary>
    /// 判定日期
    /// </summary>
    public DateTime? JudgeDate { get; set; }

    /// <summary>
    /// 判定说明
    /// </summary>
    public string? JudgeDescription { get; set; } = string.Empty;

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
