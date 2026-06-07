// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：quality-failure.ts
// 创建时间：2026-06-07
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
  QualityFailure,
  QualityFailureCreate,
  QualityFailureUpdate
} from '@/types/logistics/quality/cost/quality-failure';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityFailures
 */
const QUALITY_FAILURE_API_BASE = 'TaktQualityFailures';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取品质问题应对主列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityFailure>>} 分页结果
 */
export function getQualityFailureList(queryDto: any): Promise<TaktPagedResult<QualityFailure>> {
  return request<TaktPagedResult<QualityFailure>>({
    url: `${QUALITY_FAILURE_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取品质问题应对主
 * @param {string} id 品质问题应对主ID
 * @returns {Promise<QualityFailure>} 品质问题应对主DTO
 */
export function getQualityFailureById(id: string): Promise<QualityFailure> {
  return request<QualityFailure>({
    url: `${QUALITY_FAILURE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建品质问题应对主
 * @param {QualityFailureCreate} dto 创建DTO
 * @returns {Promise<QualityFailure>} 品质问题应对主DTO
 */
export function createQualityFailure(dto: QualityFailureCreate): Promise<QualityFailure> {
  return request<QualityFailure>({
    url: `${QUALITY_FAILURE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新品质问题应对主
 * @param {string} id 品质问题应对主ID
 * @param {QualityFailureUpdate} dto 更新DTO
 * @returns {Promise<QualityFailure>} 品质问题应对主DTO
 */
export function updateQualityFailure(id: string, dto: QualityFailureUpdate): Promise<QualityFailure> {
  return request<QualityFailure>({
    url: `${QUALITY_FAILURE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除品质问题应对主
 * @param {string} id 品质问题应对主ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityFailureById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_FAILURE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除品质问题应对主
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityFailureBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_FAILURE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取品质问题应对主选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityFailureOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${QUALITY_FAILURE_API_BASE}/options`,
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
export function getQualityFailureTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_FAILURE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入品质问题应对主
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityFailure(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_FAILURE_API_BASE}/import`,
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
 * 导出品质问题应对主
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityFailure(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_FAILURE_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
