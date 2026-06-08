// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：objective.ts
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
  Objective,
  ObjectiveCreate,
  ObjectiveStatus,
  ObjectiveUpdate
} from '@/types/human-resource/performance/objective';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktObjectives
 */
const OBJECTIVE_API_BASE = 'TaktObjectives';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取绩效目标列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Objective>>} 分页结果
 */
export function getObjectiveList(queryDto: any): Promise<TaktPagedResult<Objective>> {
  return request<TaktPagedResult<Objective>>({
    url: `${OBJECTIVE_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取绩效目标
 * @param {string} id 绩效目标ID
 * @returns {Promise<Objective>} 绩效目标DTO
 */
export function getObjectiveById(id: string): Promise<Objective> {
  return request<Objective>({
    url: `${OBJECTIVE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建绩效目标
 * @param {ObjectiveCreate} dto 创建DTO
 * @returns {Promise<Objective>} 绩效目标DTO
 */
export function createObjective(dto: ObjectiveCreate): Promise<Objective> {
  return request<Objective>({
    url: `${OBJECTIVE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新绩效目标
 * @param {string} id 绩效目标ID
 * @param {ObjectiveUpdate} dto 更新DTO
 * @returns {Promise<Objective>} 绩效目标DTO
 */
export function updateObjective(id: string, dto: ObjectiveUpdate): Promise<Objective> {
  return request<Objective>({
    url: `${OBJECTIVE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除绩效目标
 * @param {string} id 绩效目标ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteObjectiveById(id: string): Promise<void> {
  return request({
    url: `${OBJECTIVE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除绩效目标
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteObjectiveBatch(ids: string[]): Promise<void> {
  return request({
    url: `${OBJECTIVE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新绩效目标状态
 * @param {ObjectiveStatus} dto 状态 DTO
 * @returns {Promise<Objective>} 绩效目标DTO
 */
export function updateObjectiveStatus(dto: ObjectiveStatus): Promise<Objective> {
  return request<Objective>({
    url: `${OBJECTIVE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取绩效目标选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getObjectiveOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${OBJECTIVE_API_BASE}/options`,
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
export function getObjectiveTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${OBJECTIVE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入绩效目标
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importObjective(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${OBJECTIVE_API_BASE}/import`,
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
 * 导出绩效目标
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportObjective(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${OBJECTIVE_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
