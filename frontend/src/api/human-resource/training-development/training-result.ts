// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/training-development
// 文件名称：training-result.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/training-development 模块 API（自动生成，请勿手改路由常量）
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
  TrainingResult,
  TrainingResultCreate,
  TrainingResultStatus,
  TrainingResultUpdate
} from '@/types/human-resource/training-development/training-result';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTrainingResults
 */
const TRAINING_RESULT_API_BASE = 'TaktTrainingResults';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取培训结果列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TrainingResult>>} 分页结果
 */
export function getTrainingResultList(queryDto: any): Promise<TaktPagedResult<TrainingResult>> {
  return request<TaktPagedResult<TrainingResult>>({
    url: `${TRAINING_RESULT_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取培训结果
 * @param {string} id 培训结果ID
 * @returns {Promise<TrainingResult>} 培训结果DTO
 */
export function getTrainingResultById(id: string): Promise<TrainingResult> {
  return request<TrainingResult>({
    url: `${TRAINING_RESULT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建培训结果
 * @param {TrainingResultCreate} dto 创建DTO
 * @returns {Promise<TrainingResult>} 培训结果DTO
 */
export function createTrainingResult(dto: TrainingResultCreate): Promise<TrainingResult> {
  return request<TrainingResult>({
    url: `${TRAINING_RESULT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新培训结果
 * @param {string} id 培训结果ID
 * @param {TrainingResultUpdate} dto 更新DTO
 * @returns {Promise<TrainingResult>} 培训结果DTO
 */
export function updateTrainingResult(id: string, dto: TrainingResultUpdate): Promise<TrainingResult> {
  return request<TrainingResult>({
    url: `${TRAINING_RESULT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除培训结果
 * @param {string} id 培训结果ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTrainingResultById(id: string): Promise<void> {
  return request({
    url: `${TRAINING_RESULT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除培训结果
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTrainingResultBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TRAINING_RESULT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新培训结果状态
 * @param {TrainingResultStatus} dto 状态DTO
 * @returns {Promise<TrainingResult>} 培训结果DTO
 */
export function updateTrainingResultStatus(dto: TrainingResultStatus): Promise<TrainingResult> {
  return request<TrainingResult>({
    url: `${TRAINING_RESULT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取培训结果选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTrainingResultOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${TRAINING_RESULT_API_BASE}/options`,
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
export function getTrainingResultTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TRAINING_RESULT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入培训结果
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTrainingResult(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TRAINING_RESULT_API_BASE}/import`,
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
 * 导出培训结果
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTrainingResult(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TRAINING_RESULT_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
