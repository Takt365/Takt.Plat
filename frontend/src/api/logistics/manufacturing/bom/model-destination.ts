// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：model-destination.ts
// 创建时间：2026-06-05
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
  ModelDestination,
  ModelDestinationCreate,
  ModelDestinationSort,
  ModelDestinationUpdate
} from '@/types/logistics/manufacturing/bom/model-destination';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktModelDestinations
 */
const MODEL_DESTINATION_API_BASE = 'TaktModelDestinations';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取型号目的地列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ModelDestination>>} 分页结果
 */
export function getModelDestinationList(queryDto: any): Promise<TaktPagedResult<ModelDestination>> {
  return request<TaktPagedResult<ModelDestination>>({
    url: `${MODEL_DESTINATION_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取型号目的地
 * @param {string} id 型号目的地ID
 * @returns {Promise<ModelDestination>} 型号目的地DTO
 */
export function getModelDestinationById(id: string): Promise<ModelDestination> {
  return request<ModelDestination>({
    url: `${MODEL_DESTINATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建型号目的地
 * @param {ModelDestinationCreate} dto 创建DTO
 * @returns {Promise<ModelDestination>} 型号目的地DTO
 */
export function createModelDestination(dto: ModelDestinationCreate): Promise<ModelDestination> {
  return request<ModelDestination>({
    url: `${MODEL_DESTINATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新型号目的地
 * @param {string} id 型号目的地ID
 * @param {ModelDestinationUpdate} dto 更新DTO
 * @returns {Promise<ModelDestination>} 型号目的地DTO
 */
export function updateModelDestination(id: string, dto: ModelDestinationUpdate): Promise<ModelDestination> {
  return request<ModelDestination>({
    url: `${MODEL_DESTINATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除型号目的地
 * @param {string} id 型号目的地ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteModelDestinationById(id: string): Promise<void> {
  return request({
    url: `${MODEL_DESTINATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除型号目的地
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteModelDestinationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MODEL_DESTINATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新型号目的地排序
 * @param {ModelDestinationSort} dto 排序DTO
 * @returns {Promise<ModelDestination>} 型号目的地DTO
 */
export function updateModelDestinationSort(dto: ModelDestinationSort): Promise<ModelDestination> {
  return request<ModelDestination>({
    url: `${MODEL_DESTINATION_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取型号目的地选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getModelDestinationOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MODEL_DESTINATION_API_BASE}/options`,
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
export function getModelDestinationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MODEL_DESTINATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入型号目的地
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importModelDestination(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MODEL_DESTINATION_API_BASE}/import`,
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
 * 导出型号目的地
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportModelDestination(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MODEL_DESTINATION_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
