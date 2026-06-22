// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktNumberingDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Numbering 模块 DTO（实体 CRUD + 编号预览/生成）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// Numbering 响应 DTO
// ========================================

/// <summary>
/// 编号规则实体 定义系统中各类业务单据的编号生成规则，如：订单号、合同号、发票号等 支持灵活的前缀、日期格式、流水号组合 编码顺序：单据类型-公司-部门-前缀-日期-流水号 示例：order-1000-DEPT01-SO-20250120-000001
/// 对应前端 TaktNumberingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktNumberingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// NumberingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NumberingId { get; set; }

    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务领域（与一级菜单域一致，如 Foundation、Accounting、Logistics、Routine）
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
    /// </summary>
    public string DepartmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN，最多 4 位）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH） 为空表示不使用日期
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int SequenceLength { get; set; } = 0;

    /// <summary>
    /// 流水号步长（每次递增的数值，默认1）
    /// </summary>
    public int SequenceStep { get; set; } = 0;

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
    /// </summary>
    public string ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 当前流水号（用于记录下一个流水号值）
    /// </summary>
    public int CurrentSequence { get; set; } = 0;

    /// <summary>
    /// 起始编码（完整业务编号，末段为流水号；生成编号后更新为最近一次产出编码） 如：SO-20250120-000001
    /// </summary>
    public string ExampleCode { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string Separator { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,PrefixCode,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）
    /// </summary>
    public string? Description { get; set; } = string.Empty;

}

// ========================================
// Numbering 查询 DTO
// ========================================

/// <summary>
/// Numbering 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktNumberingQueryDto : TaktPagedQuery
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
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string? RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string? RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务领域（与一级菜单域一致，如 Foundation、Accounting、Logistics、Routine）
    /// </summary>
    public string? DocumentType { get; set; }

    /// <summary>
    /// 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
    /// </summary>
    public string? DepartmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN，最多 4 位）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH） 为空表示不使用日期
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int? SequenceLength { get; set; }

    /// <summary>
    /// 流水号步长（每次递增的数值，默认1）
    /// </summary>
    public int? SequenceStep { get; set; }

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
    /// </summary>
    public string? ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 当前流水号（用于记录下一个流水号值）
    /// </summary>
    public int? CurrentSequence { get; set; }

    /// <summary>
    /// 起始编码（模糊查询，可选） 如：SO-20250120-000001
    /// </summary>
    public string? ExampleCode { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string? Separator { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,PrefixCode,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）
    /// </summary>
    public string? Description { get; set; } = string.Empty;

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
// 创建Numbering DTO
// ========================================

/// <summary>
/// 创建Numbering DTO
/// </summary>
public class TaktNumberingCreateDto
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
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    [Required(ErrorMessage = "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）不能为空")]
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    [Required(ErrorMessage = "规则名称（如：销售订单号、采购订单号）不能为空")]
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务领域（与一级菜单域一致，如 Foundation、Accounting、Logistics、Routine）
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
    /// </summary>
    [Required(ErrorMessage = "部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode不能为空")]
    public string DepartmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN，最多 4 位）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH） 为空表示不使用日期
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int SequenceLength { get; set; } = 0;

    /// <summary>
    /// 流水号步长（每次递增的数值，默认1）
    /// </summary>
    public int SequenceStep { get; set; } = 0;

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
    /// </summary>
    [Required(ErrorMessage = "重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）不能为空")]
    public string ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 当前流水号（用于记录下一个流水号值）
    /// </summary>
    public int CurrentSequence { get; set; } = 0;

    /// <summary>
    /// 起始编码（保存时由服务端按规则自动生成，客户端无须提交） 如：SO-20250120-000001
    /// </summary>
    public string ExampleCode { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string Separator { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,PrefixCode,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）
    /// </summary>
    public string? Description { get; set; } = string.Empty;

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
// 更新Numbering DTO
// ========================================

/// <summary>
/// 更新Numbering DTO
/// 继承 TaktNumberingCreateDto，添加 NumberingId 字段
/// </summary>
public class TaktNumberingUpdateDto : TaktNumberingCreateDto
{
    /// <summary>
    /// NumberingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NumberingId { get; set; }

}

// ========================================
// Numbering 状态 DTO
// ========================================

/// <summary>
/// Numbering 状态更新 DTO
/// </summary>
public class TaktNumberingStatusDto
{
    /// <summary>
    /// NumberingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NumberingId { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（1=启用，0=禁用）不能为空")]
    public int Status { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Numbering 导入模板行 DTO
/// </summary>
public class TaktNumberingTemplateDto
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
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string? RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string? RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务领域（与一级菜单域一致，如 Foundation、Accounting、Logistics、Routine）
    /// </summary>
    public string? DocumentType { get; set; }

    /// <summary>
    /// 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
    /// </summary>
    public string? DepartmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN，最多 4 位）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH） 为空表示不使用日期
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int? SequenceLength { get; set; }

    /// <summary>
    /// 流水号步长（每次递增的数值，默认1）
    /// </summary>
    public int? SequenceStep { get; set; }

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
    /// </summary>
    public string? ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 当前流水号（用于记录下一个流水号值）
    /// </summary>
    public int? CurrentSequence { get; set; }

    /// <summary>
    /// 起始编码（导入可留空，服务端按规则自动生成） 如：SO-20250120-000001
    /// </summary>
    public string ExampleCode { get; set; } = string.Empty;

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
/// Numbering 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktNumberingImportDto
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
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string? RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string? RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务领域（与一级菜单域一致，如 Foundation、Accounting、Logistics、Routine）
    /// </summary>
    public string? DocumentType { get; set; }

    /// <summary>
    /// 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
    /// </summary>
    public string? DepartmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN，最多 4 位）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH） 为空表示不使用日期
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int? SequenceLength { get; set; }

    /// <summary>
    /// 流水号步长（每次递增的数值，默认1）
    /// </summary>
    public int? SequenceStep { get; set; }

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
    /// </summary>
    public string? ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 当前流水号（用于记录下一个流水号值）
    /// </summary>
    public int? CurrentSequence { get; set; }

    /// <summary>
    /// 起始编码（导入可留空，服务端按规则自动生成） 如：SO-20250120-000001
    /// </summary>
    public string ExampleCode { get; set; } = string.Empty;

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
/// Numbering 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktNumberingExportDto
{
    /// <summary>
    /// NumberingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NumberingId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务领域（与一级菜单域一致，如 Foundation、Accounting、Logistics、Routine）
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
    /// </summary>
    public string DepartmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀编码（如：PUR、SORD、ANN，最多 4 位）
    /// </summary>
    public string? PrefixCode { get; set; } = string.Empty;

    /// <summary>
    /// 日期格式（yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH） 为空表示不使用日期
    /// </summary>
    public string? DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    public int SequenceLength { get; set; } = 0;

    /// <summary>
    /// 流水号步长（每次递增的数值，默认1）
    /// </summary>
    public int SequenceStep { get; set; } = 0;

    /// <summary>
    /// 后缀编码（可选，最多 4 位）
    /// </summary>
    public string? SuffixCode { get; set; } = string.Empty;

    /// <summary>
    /// 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
    /// </summary>
    public string ResetPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 当前流水号（用于记录下一个流水号值）
    /// </summary>
    public int CurrentSequence { get; set; } = 0;

    /// <summary>
    /// 起始编码（完整业务编号，末段为流水号；生成编号后更新为最近一次产出编码） 如：SO-20250120-000001
    /// </summary>
    public string ExampleCode { get; set; } = string.Empty;

    /// <summary>
    /// 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
    /// </summary>
    public string Separator { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,PrefixCode,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）
    /// </summary>
    public string? Description { get; set; } = string.Empty;

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

// ========================================
// Numbering 预览/生成 DTO
// ========================================

/// <summary>
/// 编号预览请求 DTO（规则 Id、规则编码或草稿字段）
/// </summary>
public class TaktNumberingPreviewRequestDto
{
    /// <summary>
    /// 编号规则 Id（优先）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NumberingId { get; set; }

    /// <summary>
    /// 规则编码
    /// </summary>
    public string? RuleCode { get; set; }

    /// <summary>
    /// 规则名称（草稿预览）
    /// </summary>
    public string? RuleName { get; set; }

    /// <summary>
    /// 业务领域（草稿预览，与一级菜单域一致）
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（草稿预览必填）
    /// </summary>
    public string? DepartmentCode { get; set; }

    /// <summary>
    /// 前缀
    /// </summary>
    public string? PrefixCode { get; set; }

    /// <summary>
    /// 日期格式
    /// </summary>
    public string? DateFormat { get; set; }

    /// <summary>
    /// 流水号位数
    /// </summary>
    public int SequenceLength { get; set; }

    /// <summary>
    /// 流水号步长
    /// </summary>
    public int SequenceStep { get; set; }

    /// <summary>
    /// 后缀
    /// </summary>
    public string? SuffixCode { get; set; }

    /// <summary>
    /// 重置周期
    /// </summary>
    public string? ResetPeriod { get; set; }

    /// <summary>
    /// 当前流水号（草稿预览）
    /// </summary>
    public int CurrentSequence { get; set; }

    /// <summary>
    /// 分隔符
    /// </summary>
    public string? Separator { get; set; }

    /// <summary>
    /// 覆盖预览流水号（不传则按规则推算下一号）
    /// </summary>
    public int? SequenceOverride { get; set; }
}

/// <summary>
/// 编号预览结果 DTO
/// </summary>
public class TaktNumberingPreviewResultDto
{
    /// <summary>
    /// 预览业务编号
    /// </summary>
    public string BusinessCode { get; set; } = string.Empty;

    /// <summary>
    /// 预览所用流水号
    /// </summary>
    public int NextSequence { get; set; }

    /// <summary>
    /// 规则编码
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;
}

/// <summary>
/// 编号生成请求 DTO
/// </summary>
public class TaktNumberingGenerateRequestDto
{
    /// <summary>
    /// 规则编码
    /// </summary>
    [Required(ErrorMessage = "规则编码不能为空")]
    public string RuleCode { get; set; } = string.Empty;
}

/// <summary>
/// 编号生成结果 DTO
/// </summary>
public class TaktNumberingGenerateResultDto
{
    /// <summary>
    /// 业务编号
    /// </summary>
    public string BusinessCode { get; set; } = string.Empty;

    /// <summary>
    /// 更新后的当前流水号
    /// </summary>
    public int CurrentSequence { get; set; }

    /// <summary>
    /// 规则编码
    /// </summary>
    public string RuleCode { get; set; } = string.Empty;
}
