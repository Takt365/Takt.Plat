// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktTranslationMessagesDtos.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：指定区域文化下的前端扁平翻译消息 DTO（供 vue-i18n 动态合并）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Foundation;

/// <summary>
/// 指定区域文化下的前端扁平翻译键值（供 vue-i18n mergeLocaleMessage）
/// 对应前端 TaktTranslationMessagesDto
/// </summary>
public class TaktTranslationMessagesDto
{
    /// <summary>
    /// 区域文化编码（BCP47，如 zh-CN）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 扁平 i18n 键值（键为 I18nKey，值为 TranslationText）
    /// </summary>
    public Dictionary<string, string> Messages { get; set; } = new();
}
