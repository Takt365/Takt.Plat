// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：quality-operation-first-article.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktSelectOption
} from '@/types/common';
import type {
  QualityOperationFirstArticle,
  QualityOperationFirstArticleCreate,
  QualityOperationFirstArticleUpdate
} from '@/types/logistics/quality/cost/quality-operation-first-article';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityOperationFirstArticles
 */
const QUALITY_OPERATION_FIRST_ARTICLE_API_BASE = 'TaktQualityOperationFirstArticles';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取品质业务初期定期检定费用明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityOperationFirstArticle>>} 分页结果
 */
export function getQualityOperationFirstArticleList(queryDto: any): Promise<TaktPagedResult<QualityOperationFirstArticle>> {
  return request<TaktPagedResult<QualityOperationFirstArticle>>({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取品质业务初期定期检定费用明细
 * @param {string} id 品质业务初期定期检定费用明细ID
 * @returns {Promise<QualityOperationFirstArticle>} 品质业务初期定期检定费用明细DTO
 */
export function getQualityOperationFirstArticleById(id: string): Promise<QualityOperationFirstArticle> {
  return request<QualityOperationFirstArticle>({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建品质业务初期定期检定费用明细
 * @param {QualityOperationFirstArticleCreate} dto 创建DTO
 * @returns {Promise<QualityOperationFirstArticle>} 品质业务初期定期检定费用明细DTO
 */
export function createQualityOperationFirstArticle(dto: QualityOperationFirstArticleCreate): Promise<QualityOperationFirstArticle> {
  return request<QualityOperationFirstArticle>({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新品质业务初期定期检定费用明细
 * @param {string} id 品质业务初期定期检定费用明细ID
 * @param {QualityOperationFirstArticleUpdate} dto 更新DTO
 * @returns {Promise<QualityOperationFirstArticle>} 品质业务初期定期检定费用明细DTO
 */
export function updateQualityOperationFirstArticle(id: string, dto: QualityOperationFirstArticleUpdate): Promise<QualityOperationFirstArticle> {
  return request<QualityOperationFirstArticle>({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除品质业务初期定期检定费用明细
 * @param {string} id 品质业务初期定期检定费用明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityOperationFirstArticleById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除品质业务初期定期检定费用明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityOperationFirstArticleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取品质业务初期定期检定费用明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityOperationFirstArticleOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName sheetName
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getQualityOperationFirstArticleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入品质业务初期定期检定费用明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityOperationFirstArticle(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}/import`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    params: {
      sheetName
    },
  });
}

/**
 * 导出品质业务初期定期检定费用明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityOperationFirstArticle(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_OPERATION_FIRST_ARTICLE_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
