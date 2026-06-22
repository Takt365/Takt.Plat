// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecStepDtos.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：SopExecStep 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopExecStep 生成，请按需审阅）
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
// SopExecStep 响应 DTO
// ========================================

/// <summary>
/// SOP 工步执行明细实体
/// 对应前端 TaktSopExecStepDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopExecStepDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopExecStepID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopExecStepId { get; set; }

    /// <summary>
    /// 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecId { get; set; }

    /// <summary>
    /// 执行追溯 名称（填充字段）
    /// </summary>
    public string? ExecName { get; set; }

    /// <summary>
    /// 工步 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 工步 名称（填充字段）
    /// </summary>
    public string? StepName { get; set; }

    /// <summary>
    /// 工步序号快照
    /// </summary>
    public int StepNo { get; set; } = 0;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// 工步结果（1=合格，2=不合格，3=跳过；字典 logistics_sop_check_result_type）
    /// </summary>
    public int? StepResult { get; set; }

    /// <summary>
    /// 确认人 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedBy { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否禁止下一步（字典 sys_yes_no_type，扫码 NG 等）
    /// </summary>
    public int BlockNextStep { get; set; } = 0;

    /// <summary>
    /// 执行追溯
    /// （主表：TaktSopExec）
    /// </summary>
    public TaktSopExecDto? Exec { get; set; }

}

// ========================================
// SopExecStep 查询 DTO
// ========================================

/// <summary>
/// SopExecStep 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopExecStepQueryDto : TaktPagedQuery
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
    /// 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 工步 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StepId { get; set; }

    /// <summary>
    /// 工步序号快照
    /// </summary>
    public int? StepNo { get; set; }

    /// <summary>
    /// 开始时间（范围查询-开始）
    /// </summary>
    public DateTime? StartedAtStart { get; set; }

    /// <summary>
    /// 开始时间（范围查询-结束）
    /// </summary>
    public DateTime? StartedAtEnd { get; set; }

    /// <summary>
    /// 结束时间（范围查询-开始）
    /// </summary>
    public DateTime? EndedAtStart { get; set; }

    /// <summary>
    /// 结束时间（范围查询-结束）
    /// </summary>
    public DateTime? EndedAtEnd { get; set; }

    /// <summary>
    /// 工步结果（1=合格，2=不合格，3=跳过；字典 logistics_sop_check_result_type）
    /// </summary>
    public int? StepResult { get; set; }

    /// <summary>
    /// 确认人 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedBy { get; set; }

    /// <summary>
    /// 确认时间（范围查询-开始）
    /// </summary>
    public DateTime? ConfirmedAtStart { get; set; }

    /// <summary>
    /// 确认时间（范围查询-结束）
    /// </summary>
    public DateTime? ConfirmedAtEnd { get; set; }

    /// <summary>
    /// 是否禁止下一步（字典 sys_yes_no_type，扫码 NG 等）
    /// </summary>
    public int? BlockNextStep { get; set; }

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
// 创建SopExecStep DTO
// ========================================

/// <summary>
/// 创建SopExecStep DTO
/// </summary>
public class TaktSopExecStepCreateDto
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
    /// 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecId { get; set; }

    /// <summary>
    /// 工步 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 工步序号快照
    /// </summary>
    public int StepNo { get; set; } = 0;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// 工步结果（1=合格，2=不合格，3=跳过；字典 logistics_sop_check_result_type）
    /// </summary>
    public int? StepResult { get; set; }

    /// <summary>
    /// 确认人 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedBy { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否禁止下一步（字典 sys_yes_no_type，扫码 NG 等）
    /// </summary>
    public int BlockNextStep { get; set; } = 0;

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
// 更新SopExecStep DTO
// ========================================

/// <summary>
/// 更新SopExecStep DTO
/// 继承 TaktSopExecStepCreateDto，添加 SopExecStepId 字段
/// </summary>
public class TaktSopExecStepUpdateDto : TaktSopExecStepCreateDto
{
    /// <summary>
    /// SopExecStepID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopExecStepId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopExecStep 导入模板行 DTO
/// </summary>
public class TaktSopExecStepTemplateDto
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
    /// 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 工步 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StepId { get; set; }

    /// <summary>
    /// 工步序号快照
    /// </summary>
    public int? StepNo { get; set; }

    /// <summary>
    /// 工步结果（1=合格，2=不合格，3=跳过；字典 logistics_sop_check_result_type）
    /// </summary>
    public int? StepResult { get; set; }

    /// <summary>
    /// 确认人 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedBy { get; set; }

    /// <summary>
    /// 是否禁止下一步（字典 sys_yes_no_type，扫码 NG 等）
    /// </summary>
    public int? BlockNextStep { get; set; }

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
/// SopExecStep 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopExecStepImportDto
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
    /// 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 工步 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StepId { get; set; }

    /// <summary>
    /// 工步序号快照
    /// </summary>
    public int? StepNo { get; set; }

    /// <summary>
    /// 工步结果（1=合格，2=不合格，3=跳过；字典 logistics_sop_check_result_type）
    /// </summary>
    public int? StepResult { get; set; }

    /// <summary>
    /// 确认人 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedBy { get; set; }

    /// <summary>
    /// 是否禁止下一步（字典 sys_yes_no_type，扫码 NG 等）
    /// </summary>
    public int? BlockNextStep { get; set; }

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
/// SopExecStep 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopExecStepExportDto
{
    /// <summary>
    /// SopExecStepID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopExecStepId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecId { get; set; }

    /// <summary>
    /// 工步 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 工步序号快照
    /// </summary>
    public int StepNo { get; set; } = 0;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// 工步结果（1=合格，2=不合格，3=跳过；字典 logistics_sop_check_result_type）
    /// </summary>
    public int? StepResult { get; set; }

    /// <summary>
    /// 确认人 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedBy { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否禁止下一步（字典 sys_yes_no_type，扫码 NG 等）
    /// </summary>
    public int BlockNextStep { get; set; } = 0;

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
