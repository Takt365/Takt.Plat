// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：numbering-generator.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  NumberingGenerateRequest,
  NumberingPreviewRequest
} from '@/types/foundation/numbering-generator';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktNumberingGenerators
 */
const NUMBERING_GENERATOR_API_BASE = 'TaktNumberingGenerators';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 预览业务编号（不占用流水号）
 * @param {NumberingPreviewRequest} request 预览参数
 * @returns {Promise<unknown>} 预览结果
 */
export function previewNumbering(request: NumberingPreviewRequest): Promise<unknown> {
  return request({
    url: `${NUMBERING_GENERATOR_API_BASE}/preview`,
    method: 'post',
    data: request,
  });
}

/**
 * 生成下一个业务编号（占用流水号）
 * @param {NumberingGenerateRequest} request 生成参数
 * @returns {Promise<unknown>} 生成结果
 */
export function generateNumbering(request: NumberingGenerateRequest): Promise<unknown> {
  return request({
    url: `${NUMBERING_GENERATOR_API_BASE}/generate`,
    method: 'post',
    data: request,
  });
}
