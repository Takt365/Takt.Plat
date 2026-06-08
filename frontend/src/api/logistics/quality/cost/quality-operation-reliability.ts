// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：quality-operation-reliability.ts
// 创建时间：2026-06-08
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
  QualityOperationReliability,
  QualityOperationReliabilityCreate,
  QualityOperationReliabilityUpdate
} from '@/types/logistics/quality/cost/quality-operation-reliability';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityOperationReliabilities
 */
const QUALITY_OPERATION_RELIABILITY_API_BASE = 'TaktQualityOperationReliabilities';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取品质业务信赖性评价ORT费用明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityOperationReliability>>} 分页结果
 */
export function getQualityOperationReliabilityList(queryDto: any): Promise<TaktPagedResult<QualityOperationReliability>> {
  return request<TaktPagedResult<QualityOperationReliability>>({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取品质业务信赖性评价ORT费用明细
 * @param {string} id 品质业务信赖性评价ORT费用明细ID
 * @returns {Promise<QualityOperationReliability>} 品质业务信赖性评价ORT费用明细DTO
 */
export function getQualityOperationReliabilityById(id: string): Promise<QualityOperationReliability> {
  return request<QualityOperationReliability>({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建品质业务信赖性评价ORT费用明细
 * @param {QualityOperationReliabilityCreate} dto 创建DTO
 * @returns {Promise<QualityOperationReliability>} 品质业务信赖性评价ORT费用明细DTO
 */
export function createQualityOperationReliability(dto: QualityOperationReliabilityCreate): Promise<QualityOperationReliability> {
  return request<QualityOperationReliability>({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新品质业务信赖性评价ORT费用明细
 * @param {string} id 品质业务信赖性评价ORT费用明细ID
 * @param {QualityOperationReliabilityUpdate} dto 更新DTO
 * @returns {Promise<QualityOperationReliability>} 品质业务信赖性评价ORT费用明细DTO
 */
export function updateQualityOperationReliability(id: string, dto: QualityOperationReliabilityUpdate): Promise<QualityOperationReliability> {
  return request<QualityOperationReliability>({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除品质业务信赖性评价ORT费用明细
 * @param {string} id 品质业务信赖性评价ORT费用明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityOperationReliabilityById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除品质业务信赖性评价ORT费用明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityOperationReliabilityBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取品质业务信赖性评价ORT费用明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityOperationReliabilityOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}/options`,
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
export function getQualityOperationReliabilityTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入品质业务信赖性评价ORT费用明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityOperationReliability(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}/import`,
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
 * 导出品质业务信赖性评价ORT费用明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityOperationReliability(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_OPERATION_RELIABILITY_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
