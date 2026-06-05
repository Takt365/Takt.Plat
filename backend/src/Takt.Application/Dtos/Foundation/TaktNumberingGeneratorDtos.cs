// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktNumberingGeneratorDtos.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：编号生成器 DTO（预览/生成，对应 ITaktNumberingGenerator / TaktNumberingGeneratorsController）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// 编号预览
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
    /// 单据类型（草稿预览）
    /// </summary>
    public TaktDocumentType DocumentType { get; set; }

    /// <summary>
    /// 部门编码（草稿预览必填）
    /// </summary>
    public string? DepartmentCode { get; set; }

    /// <summary>
    /// 前缀
    /// </summary>
    public string? Prefix { get; set; }

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
    public string? Suffix { get; set; }

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

// ========================================
// 编号生成
// ========================================

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
