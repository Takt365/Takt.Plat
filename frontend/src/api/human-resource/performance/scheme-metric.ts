// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：scheme-metric.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块 API（自动生成，请勿手改路由常量）
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
  SchemeMetric,
  SchemeMetricCreate,
  SchemeMetricSort,
  SchemeMetricStatus,
  SchemeMetricUpdate
} from '@/types/human-resource/performance/scheme-metric';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSchemeMetrics
 */
const SCHEME_METRIC_API_BASE = 'TaktSchemeMetrics';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取绩效方案指标列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SchemeMetric>>} 分页结果
 */
export function getSchemeMetricList(queryDto: any): Promise<TaktPagedResult<SchemeMetric>> {
  return request<TaktPagedResult<SchemeMetric>>({
    url: `${SCHEME_METRIC_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取绩效方案指标
 * @param {string} id 绩效方案指标ID
 * @returns {Promise<SchemeMetric>} 绩效方案指标DTO
 */
export function getSchemeMetricById(id: string): Promise<SchemeMetric> {
  return request<SchemeMetric>({
    url: `${SCHEME_METRIC_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建绩效方案指标
 * @param {SchemeMetricCreate} dto 创建DTO
 * @returns {Promise<SchemeMetric>} 绩效方案指标DTO
 */
export function createSchemeMetric(dto: SchemeMetricCreate): Promise<SchemeMetric> {
  return request<SchemeMetric>({
    url: `${SCHEME_METRIC_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新绩效方案指标
 * @param {string} id 绩效方案指标ID
 * @param {SchemeMetricUpdate} dto 更新DTO
 * @returns {Promise<SchemeMetric>} 绩效方案指标DTO
 */
export function updateSchemeMetric(id: string, dto: SchemeMetricUpdate): Promise<SchemeMetric> {
  return request<SchemeMetric>({
    url: `${SCHEME_METRIC_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除绩效方案指标
 * @param {string} id 绩效方案指标ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSchemeMetricById(id: string): Promise<void> {
  return request({
    url: `${SCHEME_METRIC_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除绩效方案指标
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSchemeMetricBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SCHEME_METRIC_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新绩效方案指标状态
 * @param {SchemeMetricStatus} dto 状态 DTO
 * @returns {Promise<SchemeMetric>} 绩效方案指标DTO
 */
export function updateSchemeMetricStatus(dto: SchemeMetricStatus): Promise<SchemeMetric> {
  return request<SchemeMetric>({
    url: `${SCHEME_METRIC_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新绩效方案指标排序
 * @param {SchemeMetricSort} dto 排序DTO
 * @returns {Promise<SchemeMetric>} 绩效方案指标DTO
 */
export function updateSchemeMetricSort(dto: SchemeMetricSort): Promise<SchemeMetric> {
  return request<SchemeMetric>({
    url: `${SCHEME_METRIC_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取绩效方案指标选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSchemeMetricOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SCHEME_METRIC_API_BASE}/options`,
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
export function getSchemeMetricTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SCHEME_METRIC_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入绩效方案指标
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSchemeMetric(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SCHEME_METRIC_API_BASE}/import`,
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
 * 导出绩效方案指标
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSchemeMetric(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SCHEME_METRIC_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
