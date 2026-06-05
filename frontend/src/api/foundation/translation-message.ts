// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：translation-message.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：前端动态翻译消息 API（独立模块，非 generate-from-backend 生成）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TranslationMessages } from '@/types/foundation/translation-message';

/**
 * API 路径前缀（相对 request baseURL，对应后端 TaktTranslationMessagesController）
 */
const TRANSLATION_MESSAGE_API_BASE = 'TaktTranslationMessages';

/**
 * 获取指定区域文化的前端扁平翻译消息（登录后供 vue-i18n 动态合并）
 * @param {string} cultureCode 区域文化编码 BCP47（如 zh-CN）
 * @returns {Promise<TranslationMessages>} 扁平 i18n 键值
 */
export function getTranslationMessages(cultureCode: string): Promise<TranslationMessages> {
  return request<TranslationMessages>({
    url: TRANSLATION_MESSAGE_API_BASE,
    method: 'get',
    params: { cultureCode },
  });
}
