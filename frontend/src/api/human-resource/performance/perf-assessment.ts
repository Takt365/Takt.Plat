// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：perf-assessment.ts
// 创建时间：2026-06-23
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
  PerfAssessment,
  PerfAssessmentCreate,
  PerfAssessmentStatus,
  PerfAssessmentUpdate
} from '@/types/human-resource/performance/perf-assessment';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPerfAssessments
 */
const PERF_ASSESSMENT_API_BASE = 'TaktPerfAssessments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取绩效考核列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PerfAssessment>>} 分页结果
 */
export function getPerfAssessmentList(queryDto: any): Promise<TaktPagedResult<PerfAssessment>> {
  return request<TaktPagedResult<PerfAssessment>>({
    url: `${PERF_ASSESSMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取绩效考核
 * @param {string} id 绩效考核ID
 * @returns {Promise<PerfAssessment>} 绩效考核DTO
 */
export function getPerfAssessmentById(id: string): Promise<PerfAssessment> {
  return request<PerfAssessment>({
    url: `${PERF_ASSESSMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建绩效考核
 * @param {PerfAssessmentCreate} dto 创建DTO
 * @returns {Promise<PerfAssessment>} 绩效考核DTO
 */
export function createPerfAssessment(dto: PerfAssessmentCreate): Promise<PerfAssessment> {
  return request<PerfAssessment>({
    url: `${PERF_ASSESSMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新绩效考核
 * @param {string} id 绩效考核ID
 * @param {PerfAssessmentUpdate} dto 更新DTO
 * @returns {Promise<PerfAssessment>} 绩效考核DTO
 */
export function updatePerfAssessment(id: string, dto: PerfAssessmentUpdate): Promise<PerfAssessment> {
  return request<PerfAssessment>({
    url: `${PERF_ASSESSMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除绩效考核
 * @param {string} id 绩效考核ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfAssessmentById(id: string): Promise<void> {
  return request({
    url: `${PERF_ASSESSMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除绩效考核
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfAssessmentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PERF_ASSESSMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新绩效考核状态
 * @param {PerfAssessmentStatus} dto 状态 DTO
 * @returns {Promise<PerfAssessment>} 绩效考核DTO
 */
export function updatePerfAssessmentStatus(dto: PerfAssessmentStatus): Promise<PerfAssessment> {
  return request<PerfAssessment>({
    url: `${PERF_ASSESSMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取绩效考核选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPerfAssessmentOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PERF_ASSESSMENT_API_BASE}/options`,
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
export function getPerfAssessmentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_ASSESSMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入绩效考核
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPerfAssessment(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PERF_ASSESSMENT_API_BASE}/import`,
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
 * 导出绩效考核
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPerfAssessment(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_ASSESSMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
