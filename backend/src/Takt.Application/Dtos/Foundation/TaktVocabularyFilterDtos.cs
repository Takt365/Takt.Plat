// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktVocabularyFilterDtos.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：敏感词过滤/检测 API DTO
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Application.Dtos.Foundation;

/// <summary>
/// 敏感词过滤/检测请求 DTO
/// </summary>
public class TaktVocabularyFilterRequestDto
{
    /// <summary>
    /// 待检测或过滤的文本
    /// </summary>
    [Required(ErrorMessage = "待过滤文本不能为空")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// 最低过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）；为空时匹配全部启用词条
    /// </summary>
    public int? MinFilterLevel { get; set; }
}

/// <summary>
/// 敏感词过滤结果 DTO
/// </summary>
public class TaktVocabularyFilterResultDto
{
    /// <summary>
    /// 原始文本
    /// </summary>
    public string OriginalText { get; set; } = string.Empty;

    /// <summary>
    /// 过滤后的文本
    /// </summary>
    public string FilteredText { get; set; } = string.Empty;

    /// <summary>
    /// 是否命中敏感词
    /// </summary>
    public bool HasSensitiveWord { get; set; }

    /// <summary>
    /// 命中的敏感词列表（去重）
    /// </summary>
    public IReadOnlyList<string> MatchedWords { get; set; } = Array.Empty<string>();
}

/// <summary>
/// 敏感词检测结果 DTO（不返回替换后文本）
/// </summary>
public class TaktVocabularyDetectResultDto
{
    /// <summary>
    /// 是否命中敏感词
    /// </summary>
    public bool HasSensitiveWord { get; set; }

    /// <summary>
    /// 命中的敏感词列表（去重）
    /// </summary>
    public IReadOnlyList<string> MatchedWords { get; set; } = Array.Empty<string>();
}
