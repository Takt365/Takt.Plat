// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：translation-message.d.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：前端动态翻译消息类型（类型名去 Takt 前缀与末尾 Dto，如 TaktTranslationMessagesDto → TranslationMessages）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 指定区域文化下的前端扁平翻译消息（供 vue-i18n mergeLocaleMessage）
 * @description 对应后端 TaktTranslationMessagesDto
 */
export interface TranslationMessages {
  /**
   * 区域文化编码（BCP47，如 zh-CN）
   */
  cultureCode: string;

  /**
   * 扁平 i18n 键值（键为 i18nKey，值为 translationText）
   */
  messages: Record<string, string>;
}
