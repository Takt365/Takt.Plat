// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktSamplingSchemeDtos.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Auto Generated)
// 功能描述：SamplingScheme 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSamplingScheme 生成，请按需审阅）
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
// SamplingScheme 响应 DTO
// ========================================

/// <summary>
/// Takt抽样方案实体
/// 对应前端 TaktSamplingSchemeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSamplingSchemeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SamplingSchemeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SamplingSchemeId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（唯一索引）
    /// </summary>
    public string SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）
    /// </summary>
    public int SamplingSchemeType { get; set; } = 0;

    /// <summary>
    /// 抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）
    /// </summary>
    public int SamplingStandard { get; set; } = 0;

    /// <summary>
    /// 检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）
    /// </summary>
    public int InspectionLevel { get; set; } = 0;

    /// <summary>
    /// AQL值（可接受质量水平，0.010-1000，存储为小数）
    /// </summary>
    public decimal AqlValue { get; set; }

    /// <summary>
    /// 批量范围最小值
    /// </summary>
    public int LotSizeMin { get; set; } = 0;

    /// <summary>
    /// 批量范围最大值（0表示无上限）
    /// </summary>
    public int LotSizeMax { get; set; } = 0;

    /// <summary>
    /// 样本量（抽样数量）
    /// </summary>
    public int SampleSize { get; set; } = 0;

    /// <summary>
    /// 接收数（Ac，Acceptance Number）
    /// </summary>
    public int AcceptanceNumber { get; set; } = 0;

    /// <summary>
    /// 拒收数（Re，Rejection Number）
    /// </summary>
    public int RejectionNumber { get; set; } = 0;

    /// <summary>
    /// 检验严格度（0=正常检验，1=加严检验，2=放宽检验）
    /// </summary>
    public int InspectionStrictness { get; set; } = 0;

    /// <summary>
    /// 是否支持转移规则（0=否，1=是）
    /// </summary>
    public int IsTransferRuleEnabled { get; set; } = 0;

    /// <summary>
    /// 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
    /// </summary>
    public string? TransferRuleConfig { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int SamplingSchemeStatus { get; set; } = 0;

    /// <summary>
    /// 抽样方案描述
    /// </summary>
    public string? SchemeDescription { get; set; } = string.Empty;

}

// ========================================
// SamplingScheme 查询 DTO
// ========================================

/// <summary>
/// SamplingScheme 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSamplingSchemeQueryDto : TaktPagedQuery
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
    /// 抽样方案编码（唯一索引）
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string? SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）
    /// </summary>
    public int? SamplingSchemeType { get; set; }

    /// <summary>
    /// 抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）
    /// </summary>
    public int? SamplingStandard { get; set; }

    /// <summary>
    /// 检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）
    /// </summary>
    public int? InspectionLevel { get; set; }

    /// <summary>
    /// AQL值（可接受质量水平，0.010-1000，存储为小数）
    /// </summary>
    public decimal? AqlValue { get; set; }

    /// <summary>
    /// 批量范围最小值
    /// </summary>
    public int? LotSizeMin { get; set; }

    /// <summary>
    /// 批量范围最大值（0表示无上限）
    /// </summary>
    public int? LotSizeMax { get; set; }

    /// <summary>
    /// 样本量（抽样数量）
    /// </summary>
    public int? SampleSize { get; set; }

    /// <summary>
    /// 接收数（Ac，Acceptance Number）
    /// </summary>
    public int? AcceptanceNumber { get; set; }

    /// <summary>
    /// 拒收数（Re，Rejection Number）
    /// </summary>
    public int? RejectionNumber { get; set; }

    /// <summary>
    /// 检验严格度（0=正常检验，1=加严检验，2=放宽检验）
    /// </summary>
    public int? InspectionStrictness { get; set; }

    /// <summary>
    /// 是否支持转移规则（0=否，1=是）
    /// </summary>
    public int? IsTransferRuleEnabled { get; set; }

    /// <summary>
    /// 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
    /// </summary>
    public string? TransferRuleConfig { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int? SamplingSchemeStatus { get; set; }

    /// <summary>
    /// 抽样方案描述
    /// </summary>
    public string? SchemeDescription { get; set; } = string.Empty;

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
// 创建SamplingScheme DTO
// ========================================

/// <summary>
/// 创建SamplingScheme DTO
/// </summary>
public class TaktSamplingSchemeCreateDto
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
    /// 抽样方案编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "抽样方案编码（唯一索引）不能为空")]
    public string SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    [Required(ErrorMessage = "抽样方案名称不能为空")]
    public string SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）
    /// </summary>
    public int SamplingSchemeType { get; set; } = 0;

    /// <summary>
    /// 抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）
    /// </summary>
    public int SamplingStandard { get; set; } = 0;

    /// <summary>
    /// 检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）
    /// </summary>
    public int InspectionLevel { get; set; } = 0;

    /// <summary>
    /// AQL值（可接受质量水平，0.010-1000，存储为小数）
    /// </summary>
    public decimal AqlValue { get; set; }

    /// <summary>
    /// 批量范围最小值
    /// </summary>
    public int LotSizeMin { get; set; } = 0;

    /// <summary>
    /// 批量范围最大值（0表示无上限）
    /// </summary>
    public int LotSizeMax { get; set; } = 0;

    /// <summary>
    /// 样本量（抽样数量）
    /// </summary>
    public int SampleSize { get; set; } = 0;

    /// <summary>
    /// 接收数（Ac，Acceptance Number）
    /// </summary>
    public int AcceptanceNumber { get; set; } = 0;

    /// <summary>
    /// 拒收数（Re，Rejection Number）
    /// </summary>
    public int RejectionNumber { get; set; } = 0;

    /// <summary>
    /// 检验严格度（0=正常检验，1=加严检验，2=放宽检验）
    /// </summary>
    public int InspectionStrictness { get; set; } = 0;

    /// <summary>
    /// 是否支持转移规则（0=否，1=是）
    /// </summary>
    public int IsTransferRuleEnabled { get; set; } = 0;

    /// <summary>
    /// 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
    /// </summary>
    public string? TransferRuleConfig { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int SamplingSchemeStatus { get; set; } = 0;

    /// <summary>
    /// 抽样方案描述
    /// </summary>
    public string? SchemeDescription { get; set; } = string.Empty;

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
// 更新SamplingScheme DTO
// ========================================

/// <summary>
/// 更新SamplingScheme DTO
/// 继承 TaktSamplingSchemeCreateDto，添加 SamplingSchemeId 字段
/// </summary>
public class TaktSamplingSchemeUpdateDto : TaktSamplingSchemeCreateDto
{
    /// <summary>
    /// SamplingSchemeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SamplingSchemeId { get; set; }

}

// ========================================
// SamplingScheme 状态 DTO
// ========================================

/// <summary>
/// SamplingScheme 状态更新 DTO
/// </summary>
public class TaktSamplingSchemeStatusDto
{
    /// <summary>
    /// SamplingSchemeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SamplingSchemeId { get; set; }

    /// <summary>
    /// 抽样方案状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    [Required(ErrorMessage = "抽样方案状态（0=草稿，1=已发布，2=已停用）不能为空")]
    public int SamplingSchemeStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SamplingScheme 导入模板行 DTO
/// </summary>
public class TaktSamplingSchemeTemplateDto
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
    /// 抽样方案编码（唯一索引）
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string? SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）
    /// </summary>
    public int? SamplingSchemeType { get; set; }

    /// <summary>
    /// 抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）
    /// </summary>
    public int? SamplingStandard { get; set; }

    /// <summary>
    /// 检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）
    /// </summary>
    public int? InspectionLevel { get; set; }

    /// <summary>
    /// 批量范围最小值
    /// </summary>
    public int? LotSizeMin { get; set; }

    /// <summary>
    /// 批量范围最大值（0表示无上限）
    /// </summary>
    public int? LotSizeMax { get; set; }

    /// <summary>
    /// 样本量（抽样数量）
    /// </summary>
    public int? SampleSize { get; set; }

    /// <summary>
    /// 接收数（Ac，Acceptance Number）
    /// </summary>
    public int? AcceptanceNumber { get; set; }

    /// <summary>
    /// 拒收数（Re，Rejection Number）
    /// </summary>
    public int? RejectionNumber { get; set; }

    /// <summary>
    /// 检验严格度（0=正常检验，1=加严检验，2=放宽检验）
    /// </summary>
    public int? InspectionStrictness { get; set; }

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
/// SamplingScheme 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSamplingSchemeImportDto
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
    /// 抽样方案编码（唯一索引）
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string? SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）
    /// </summary>
    public int? SamplingSchemeType { get; set; }

    /// <summary>
    /// 抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）
    /// </summary>
    public int? SamplingStandard { get; set; }

    /// <summary>
    /// 检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）
    /// </summary>
    public int? InspectionLevel { get; set; }

    /// <summary>
    /// 批量范围最小值
    /// </summary>
    public int? LotSizeMin { get; set; }

    /// <summary>
    /// 批量范围最大值（0表示无上限）
    /// </summary>
    public int? LotSizeMax { get; set; }

    /// <summary>
    /// 样本量（抽样数量）
    /// </summary>
    public int? SampleSize { get; set; }

    /// <summary>
    /// 接收数（Ac，Acceptance Number）
    /// </summary>
    public int? AcceptanceNumber { get; set; }

    /// <summary>
    /// 拒收数（Re，Rejection Number）
    /// </summary>
    public int? RejectionNumber { get; set; }

    /// <summary>
    /// 检验严格度（0=正常检验，1=加严检验，2=放宽检验）
    /// </summary>
    public int? InspectionStrictness { get; set; }

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
/// SamplingScheme 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSamplingSchemeExportDto
{
    /// <summary>
    /// SamplingSchemeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SamplingSchemeId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（唯一索引）
    /// </summary>
    public string SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案名称
    /// </summary>
    public string SamplingSchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）
    /// </summary>
    public int SamplingSchemeType { get; set; } = 0;

    /// <summary>
    /// 抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）
    /// </summary>
    public int SamplingStandard { get; set; } = 0;

    /// <summary>
    /// 检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）
    /// </summary>
    public int InspectionLevel { get; set; } = 0;

    /// <summary>
    /// AQL值（可接受质量水平，0.010-1000，存储为小数）
    /// </summary>
    public decimal AqlValue { get; set; }

    /// <summary>
    /// 批量范围最小值
    /// </summary>
    public int LotSizeMin { get; set; } = 0;

    /// <summary>
    /// 批量范围最大值（0表示无上限）
    /// </summary>
    public int LotSizeMax { get; set; } = 0;

    /// <summary>
    /// 样本量（抽样数量）
    /// </summary>
    public int SampleSize { get; set; } = 0;

    /// <summary>
    /// 接收数（Ac，Acceptance Number）
    /// </summary>
    public int AcceptanceNumber { get; set; } = 0;

    /// <summary>
    /// 拒收数（Re，Rejection Number）
    /// </summary>
    public int RejectionNumber { get; set; } = 0;

    /// <summary>
    /// 检验严格度（0=正常检验，1=加严检验，2=放宽检验）
    /// </summary>
    public int InspectionStrictness { get; set; } = 0;

    /// <summary>
    /// 是否支持转移规则（0=否，1=是）
    /// </summary>
    public int IsTransferRuleEnabled { get; set; } = 0;

    /// <summary>
    /// 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
    /// </summary>
    public string? TransferRuleConfig { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int SamplingSchemeStatus { get; set; } = 0;

    /// <summary>
    /// 抽样方案描述
    /// </summary>
    public string? SchemeDescription { get; set; } = string.Empty;

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
