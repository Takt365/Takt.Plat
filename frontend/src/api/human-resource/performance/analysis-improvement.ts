// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：analysis-improvement.ts
// 创建时间：2026-06-07
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
  AnalysisImprovement,
  AnalysisImprovementCreate,
  AnalysisImprovementStatus,
  AnalysisImprovementUpdate
} from '@/types/human-resource/performance/analysis-improvement';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAnalysisImprovements
 */
const ANALYSIS_IMPROVEMENT_API_BASE = 'TaktAnalysisImprovements';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取绩效分析改进列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<AnalysisImprovement>>} 分页结果
 */
export function getAnalysisImprovementList(queryDto: any): Promise<TaktPagedResult<AnalysisImprovement>> {
  return request<TaktPagedResult<AnalysisImprovement>>({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取绩效分析改进
 * @param {string} id 绩效分析改进ID
 * @returns {Promise<AnalysisImprovement>} 绩效分析改进DTO
 */
export function getAnalysisImprovementById(id: string): Promise<AnalysisImprovement> {
  return request<AnalysisImprovement>({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建绩效分析改进
 * @param {AnalysisImprovementCreate} dto 创建DTO
 * @returns {Promise<AnalysisImprovement>} 绩效分析改进DTO
 */
export function createAnalysisImprovement(dto: AnalysisImprovementCreate): Promise<AnalysisImprovement> {
  return request<AnalysisImprovement>({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新绩效分析改进
 * @param {string} id 绩效分析改进ID
 * @param {AnalysisImprovementUpdate} dto 更新DTO
 * @returns {Promise<AnalysisImprovement>} 绩效分析改进DTO
 */
export function updateAnalysisImprovement(id: string, dto: AnalysisImprovementUpdate): Promise<AnalysisImprovement> {
  return request<AnalysisImprovement>({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除绩效分析改进
 * @param {string} id 绩效分析改进ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAnalysisImprovementById(id: string): Promise<void> {
  return request({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除绩效分析改进
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAnalysisImprovementBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新绩效分析改进状态
 * @param {AnalysisImprovementStatus} dto 状态DTO
 * @returns {Promise<AnalysisImprovement>} 绩效分析改进DTO
 */
export function updateAnalysisImprovementStatus(dto: AnalysisImprovementStatus): Promise<AnalysisImprovement> {
  return request<AnalysisImprovement>({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取绩效分析改进选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAnalysisImprovementOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/options`,
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
export function getAnalysisImprovementTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入绩效分析改进
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAnalysisImprovement(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/import`,
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
 * 导出绩效分析改进
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAnalysisImprovement(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ANALYSIS_IMPROVEMENT_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
