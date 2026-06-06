// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：vocabulary-filter.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  VocabularyFilterRequest
} from '@/types/foundation/vocabulary-filter';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktVocabularyFilters
 */
const VOCABULARY_FILTER_API_BASE = 'TaktVocabularyFilters';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 检测文本是否包含敏感词
 * @param {VocabularyFilterRequest} dto 检测请求
 * @returns {Promise<unknown>} 检测结果
 */
export function detectVocabularyText(dto: VocabularyFilterRequest): Promise<unknown> {
  return request({
    url: `${VOCABULARY_FILTER_API_BASE}/detect`,
    method: 'post',
    data: dto,
  });
}

/**
 * 过滤文本中的敏感词
 * @param {VocabularyFilterRequest} dto 过滤请求
 * @returns {Promise<unknown>} 过滤结果
 */
export function filterVocabularyText(dto: VocabularyFilterRequest): Promise<unknown> {
  return request({
    url: `${VOCABULARY_FILTER_API_BASE}/filter`,
    method: 'post',
    data: dto,
  });
}
