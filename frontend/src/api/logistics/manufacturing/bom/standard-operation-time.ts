// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：standard-operation-time.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块 API（自动生成，请勿手改路由常量）
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
  StandardOperationTime,
  StandardOperationTimeCreate,
  StandardOperationTimeUpdate
} from '@/types/logistics/manufacturing/bom/standard-operation-time';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktStandardOperationTimes
 */
const STANDARD_OPERATION_TIME_API_BASE = 'TaktStandardOperationTimes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取标准工序时间列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<StandardOperationTime>>} 分页结果
 */
export function getStandardOperationTimeList(queryDto: any): Promise<TaktPagedResult<StandardOperationTime>> {
  return request<TaktPagedResult<StandardOperationTime>>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取标准工序时间
 * @param {string} id 标准工序时间ID
 * @returns {Promise<StandardOperationTime>} 标准工序时间DTO
 */
export function getStandardOperationTimeById(id: string): Promise<StandardOperationTime> {
  return request<StandardOperationTime>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建标准工序时间
 * @param {StandardOperationTimeCreate} dto 创建DTO
 * @returns {Promise<StandardOperationTime>} 标准工序时间DTO
 */
export function createStandardOperationTime(dto: StandardOperationTimeCreate): Promise<StandardOperationTime> {
  return request<StandardOperationTime>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新标准工序时间
 * @param {string} id 标准工序时间ID
 * @param {StandardOperationTimeUpdate} dto 更新DTO
 * @returns {Promise<StandardOperationTime>} 标准工序时间DTO
 */
export function updateStandardOperationTime(id: string, dto: StandardOperationTimeUpdate): Promise<StandardOperationTime> {
  return request<StandardOperationTime>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除标准工序时间
 * @param {string} id 标准工序时间ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardOperationTimeById(id: string): Promise<void> {
  return request({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除标准工序时间
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardOperationTimeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取标准工序时间选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getStandardOperationTimeOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/options`,
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
export function getStandardOperationTimeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入标准工序时间
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importStandardOperationTime(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/import`,
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
 * 导出标准工序时间
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportStandardOperationTime(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
