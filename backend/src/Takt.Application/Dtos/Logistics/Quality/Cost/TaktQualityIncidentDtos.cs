// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityIncident 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityIncident 生成，请按需审阅）
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
// QualityIncident 响应 DTO
// ========================================

/// <summary>
/// 品质事故主表,用于记录废弃单的基础信息(年月日、机种)及汇总数据
/// 对应前端 TaktQualityIncidentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityIncidentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityIncidentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质事故编码(唯一,如:QI-2026-0001)
    /// </summary>
    public string QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 事故日期
    /// </summary>
    public DateTime IncidentDate { get; set; }

    /// <summary>
    /// 间接人员费率(元/分钟)
    /// </summary>
    public decimal IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 事故内容(废弃原因)
    /// </summary>
    public string? IncidentReason { get; set; } = string.Empty;

    /// <summary>
    /// 废弃总数(自动计算 = 各子表废弃数量合计)
    /// </summary>
    public decimal TotalScrapQuantity { get; set; }

    /// <summary>
    /// 总废弃费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalScrapCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    public string CostCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 事故明细列表
    /// （子表：TaktQualityIncidentItem）
    /// </summary>
    public List<TaktQualityIncidentItemDto>? IncidentItems { get; set; }

}

// ========================================
// QualityIncident 查询 DTO
// ========================================

/// <summary>
/// QualityIncident 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityIncidentQueryDto : TaktPagedQuery
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
    /// 品质事故编码(唯一,如:QI-2026-0001)
    /// </summary>
    public string? QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 事故日期（范围查询-开始）
    /// </summary>
    public DateTime? IncidentDateStart { get; set; }

    /// <summary>
    /// 事故日期（范围查询-结束）
    /// </summary>
    public DateTime? IncidentDateEnd { get; set; }

    /// <summary>
    /// 间接人员费率(元/分钟)
    /// </summary>
    public decimal? IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string? Model { get; set; } = string.Empty;

    /// <summary>
    /// 事故内容(废弃原因)
    /// </summary>
    public string? IncidentReason { get; set; } = string.Empty;

    /// <summary>
    /// 废弃总数(自动计算 = 各子表废弃数量合计)
    /// </summary>
    public decimal? TotalScrapQuantity { get; set; }

    /// <summary>
    /// 总废弃费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal? TotalScrapCost { get; set; }

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
// 创建QualityIncident DTO
// ========================================

/// <summary>
/// 创建QualityIncident DTO
/// </summary>
public class TaktQualityIncidentCreateDto
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
    /// 品质事故编码(唯一,如:QI-2026-0001)
    /// </summary>
    [Required(ErrorMessage = "品质事故编码(唯一,如:QI-2026-0001)不能为空")]
    public string QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 事故日期
    /// </summary>
    public DateTime IncidentDate { get; set; }

    /// <summary>
    /// 间接人员费率(元/分钟)
    /// </summary>
    public decimal IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    [Required(ErrorMessage = "机种/产品型号不能为空")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 事故内容(废弃原因)
    /// </summary>
    public string? IncidentReason { get; set; } = string.Empty;

    /// <summary>
    /// 废弃总数(自动计算 = 各子表废弃数量合计)
    /// </summary>
    public decimal TotalScrapQuantity { get; set; }

    /// <summary>
    /// 总废弃费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalScrapCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    [Required(ErrorMessage = "成本币种(CNY/USD/JPY等)不能为空")]
    public string CostCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 事故明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIncidentItemCreateDto>? IncidentItems { get; set; }

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
// 更新QualityIncident DTO
// ========================================

/// <summary>
/// 更新QualityIncident DTO
/// 继承 TaktQualityIncidentCreateDto，添加 QualityIncidentId 字段
/// </summary>
public class TaktQualityIncidentUpdateDto : TaktQualityIncidentCreateDto
{
    /// <summary>
    /// QualityIncidentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityIncident 导入模板行 DTO
/// </summary>
public class TaktQualityIncidentTemplateDto
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
    /// 品质事故编码(唯一,如:QI-2026-0001)
    /// </summary>
    public string? QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string? Model { get; set; } = string.Empty;

    /// <summary>
    /// 事故内容(废弃原因)
    /// </summary>
    public string? IncidentReason { get; set; } = string.Empty;

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
/// QualityIncident 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityIncidentImportDto
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
    /// 品质事故编码(唯一,如:QI-2026-0001)
    /// </summary>
    public string? QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string? Model { get; set; } = string.Empty;

    /// <summary>
    /// 事故内容(废弃原因)
    /// </summary>
    public string? IncidentReason { get; set; } = string.Empty;

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
/// QualityIncident 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityIncidentExportDto
{
    /// <summary>
    /// QualityIncidentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质事故编码(唯一,如:QI-2026-0001)
    /// </summary>
    public string QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 事故日期
    /// </summary>
    public DateTime IncidentDate { get; set; }

    /// <summary>
    /// 间接人员费率(元/分钟)
    /// </summary>
    public decimal IndirectManpowerCostPerMinute { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 事故内容(废弃原因)
    /// </summary>
    public string? IncidentReason { get; set; } = string.Empty;

    /// <summary>
    /// 废弃总数(自动计算 = 各子表废弃数量合计)
    /// </summary>
    public decimal TotalScrapQuantity { get; set; }

    /// <summary>
    /// 总废弃费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalScrapCost { get; set; }

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
