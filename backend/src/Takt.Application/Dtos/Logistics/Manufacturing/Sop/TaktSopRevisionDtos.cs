// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopRevisionDtos.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：SopRevision 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopRevision 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Sop;

// ========================================
// SopRevision 响应 DTO
// ========================================

/// <summary>
/// SOP 版本实体
/// 对应前端 TaktSopRevisionDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopRevisionDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopRevisionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopRevisionId { get; set; }

    /// <summary>
    /// SOP 文档头 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 文档头 名称（填充字段）
    /// </summary>
    public string? SopName { get; set; }

    /// <summary>
    /// 版本号（主版本.次版本，如 1.0、A.01）
    /// </summary>
    public string Revision { get; set; } = string.Empty;

    /// <summary>
    /// 受控 PDF URL
    /// </summary>
    public string? FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 变更说明
    /// </summary>
    public string? ChangeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 关联 ECN 主表 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnId { get; set; }

    /// <summary>
    /// 关联 ECN 主表 名称（填充字段）
    /// </summary>
    public string? EcnName { get; set; }

    /// <summary>
    /// 是否锁定（ECN 后旧版锁定；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int IsLocked { get; set; } = 0;

    /// <summary>
    /// 是否强制班组长确认（新版本弹窗；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int ForceLeaderAck { get; set; } = 0;

    /// <summary>
    /// 版本状态（字典 sys_lifecycle_status）
    /// </summary>
    public int RevisionStatus { get; set; } = 0;

    /// <summary>
    /// 生效规则（1=立即生效，2=按工单生效；字典 logistics_sop_effective_rule）
    /// </summary>
    public int EffectiveRule { get; set; } = 0;

    /// <summary>
    /// SOP 文档头
    /// （主表：TaktSopDoc）
    /// </summary>
    public TaktSopDocDto? SopDoc { get; set; }

    /// <summary>
    /// 多语言正文
    /// （子表：TaktSopContent）
    /// </summary>
    public List<TaktSopContentDto>? Contents { get; set; }

}

// ========================================
// SopRevision 查询 DTO
// ========================================

/// <summary>
/// SopRevision 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopRevisionQueryDto : TaktPagedQuery
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
    /// SOP 文档头 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// 版本号（主版本.次版本，如 1.0、A.01）
    /// </summary>
    public string? Revision { get; set; } = string.Empty;

    /// <summary>
    /// 受控 PDF URL
    /// </summary>
    public string? FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 变更说明
    /// </summary>
    public string? ChangeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 关联 ECN 主表 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnId { get; set; }

    /// <summary>
    /// 是否锁定（ECN 后旧版锁定；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int? IsLocked { get; set; }

    /// <summary>
    /// 是否强制班组长确认（新版本弹窗；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int? ForceLeaderAck { get; set; }

    /// <summary>
    /// 版本状态（字典 sys_lifecycle_status）
    /// </summary>
    public int? RevisionStatus { get; set; }

    /// <summary>
    /// 生效规则（1=立即生效，2=按工单生效；字典 logistics_sop_effective_rule）
    /// </summary>
    public int? EffectiveRule { get; set; }

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
// 创建SopRevision DTO
// ========================================

/// <summary>
/// 创建SopRevision DTO
/// </summary>
public class TaktSopRevisionCreateDto
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
    /// SOP 文档头 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// 版本号（主版本.次版本，如 1.0、A.01）
    /// </summary>
    [Required(ErrorMessage = "版本号（主版本.次版本，如 1.0、A.01）不能为空")]
    public string Revision { get; set; } = string.Empty;

    /// <summary>
    /// 受控 PDF URL
    /// </summary>
    public string? FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 变更说明
    /// </summary>
    public string? ChangeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 关联 ECN 主表 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnId { get; set; }

    /// <summary>
    /// 是否锁定（ECN 后旧版锁定；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int IsLocked { get; set; } = 0;

    /// <summary>
    /// 是否强制班组长确认（新版本弹窗；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int ForceLeaderAck { get; set; } = 0;

    /// <summary>
    /// 版本状态（字典 sys_lifecycle_status）
    /// </summary>
    public int RevisionStatus { get; set; } = 0;

    /// <summary>
    /// 生效规则（1=立即生效，2=按工单生效；字典 logistics_sop_effective_rule）
    /// </summary>
    public int EffectiveRule { get; set; } = 0;

    /// <summary>
    /// 多语言正文（子表，级联保存）
    /// </summary>
    public List<TaktSopContentCreateDto>? Contents { get; set; }

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
// 更新SopRevision DTO
// ========================================

/// <summary>
/// 更新SopRevision DTO
/// 继承 TaktSopRevisionCreateDto，添加 SopRevisionId 字段
/// </summary>
public class TaktSopRevisionUpdateDto : TaktSopRevisionCreateDto
{
    /// <summary>
    /// SopRevisionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopRevisionId { get; set; }

}

// ========================================
// SopRevision 状态 DTO
// ========================================

/// <summary>
/// SopRevision 状态更新 DTO
/// </summary>
public class TaktSopRevisionStatusDto
{
    /// <summary>
    /// SopRevisionID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopRevisionId { get; set; }

    /// <summary>
    /// 版本状态（字典 sys_lifecycle_status）
    /// </summary>
    [Required(ErrorMessage = "版本状态（字典 sys_lifecycle_status）不能为空")]
    public int RevisionStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopRevision 导入模板行 DTO
/// </summary>
public class TaktSopRevisionTemplateDto
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
    /// SOP 文档头 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// 版本号（主版本.次版本，如 1.0、A.01）
    /// </summary>
    public string? Revision { get; set; } = string.Empty;

    /// <summary>
    /// 受控 PDF URL
    /// </summary>
    public string? FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 变更说明
    /// </summary>
    public string? ChangeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 关联 ECN 主表 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnId { get; set; }

    /// <summary>
    /// 是否锁定（ECN 后旧版锁定；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int? IsLocked { get; set; }

    /// <summary>
    /// 是否强制班组长确认（新版本弹窗；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int? ForceLeaderAck { get; set; }

    /// <summary>
    /// 版本状态（字典 sys_lifecycle_status）
    /// </summary>
    public int? RevisionStatus { get; set; }

    /// <summary>
    /// 生效规则（1=立即生效，2=按工单生效；字典 logistics_sop_effective_rule）
    /// </summary>
    public int? EffectiveRule { get; set; }

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
/// SopRevision 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopRevisionImportDto
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
    /// SOP 文档头 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// 版本号（主版本.次版本，如 1.0、A.01）
    /// </summary>
    public string? Revision { get; set; } = string.Empty;

    /// <summary>
    /// 受控 PDF URL
    /// </summary>
    public string? FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 变更说明
    /// </summary>
    public string? ChangeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 关联 ECN 主表 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnId { get; set; }

    /// <summary>
    /// 是否锁定（ECN 后旧版锁定；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int? IsLocked { get; set; }

    /// <summary>
    /// 是否强制班组长确认（新版本弹窗；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int? ForceLeaderAck { get; set; }

    /// <summary>
    /// 版本状态（字典 sys_lifecycle_status）
    /// </summary>
    public int? RevisionStatus { get; set; }

    /// <summary>
    /// 生效规则（1=立即生效，2=按工单生效；字典 logistics_sop_effective_rule）
    /// </summary>
    public int? EffectiveRule { get; set; }

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
/// SopRevision 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopRevisionExportDto
{
    /// <summary>
    /// SopRevisionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopRevisionId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 文档头 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// 版本号（主版本.次版本，如 1.0、A.01）
    /// </summary>
    public string Revision { get; set; } = string.Empty;

    /// <summary>
    /// 受控 PDF URL
    /// </summary>
    public string? FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 变更说明
    /// </summary>
    public string? ChangeDesc { get; set; } = string.Empty;

    /// <summary>
    /// 关联 ECN 主表 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnId { get; set; }

    /// <summary>
    /// 是否锁定（ECN 后旧版锁定；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int IsLocked { get; set; } = 0;

    /// <summary>
    /// 是否强制班组长确认（新版本弹窗；字典 sys_yes_no_type，0=否，1=是）
    /// </summary>
    public int ForceLeaderAck { get; set; } = 0;

    /// <summary>
    /// 版本状态（字典 sys_lifecycle_status）
    /// </summary>
    public int RevisionStatus { get; set; } = 0;

    /// <summary>
    /// 生效规则（1=立即生效，2=按工单生效；字典 logistics_sop_effective_rule）
    /// </summary>
    public int EffectiveRule { get; set; } = 0;

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
