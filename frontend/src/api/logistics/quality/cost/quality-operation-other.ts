// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：quality-operation-other.ts
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
  QualityOperationOther,
  QualityOperationOtherCreate,
  QualityOperationOtherUpdate
} from '@/types/logistics/quality/cost/quality-operation-other';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityOperationOthers
 */
const QUALITY_OPERATION_OTHER_API_BASE = 'TaktQualityOperationOthers';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取品质业务其他通常业务费用明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityOperationOther>>} 分页结果
 */
export function getQualityOperationOtherList(queryDto: any): Promise<TaktPagedResult<QualityOperationOther>> {
  return request<TaktPagedResult<QualityOperationOther>>({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取品质业务其他通常业务费用明细
 * @param {string} id 品质业务其他通常业务费用明细ID
 * @returns {Promise<QualityOperationOther>} 品质业务其他通常业务费用明细DTO
 */
export function getQualityOperationOtherById(id: string): Promise<QualityOperationOther> {
  return request<QualityOperationOther>({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建品质业务其他通常业务费用明细
 * @param {QualityOperationOtherCreate} dto 创建DTO
 * @returns {Promise<QualityOperationOther>} 品质业务其他通常业务费用明细DTO
 */
export function createQualityOperationOther(dto: QualityOperationOtherCreate): Promise<QualityOperationOther> {
  return request<QualityOperationOther>({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新品质业务其他通常业务费用明细
 * @param {string} id 品质业务其他通常业务费用明细ID
 * @param {QualityOperationOtherUpdate} dto 更新DTO
 * @returns {Promise<QualityOperationOther>} 品质业务其他通常业务费用明细DTO
 */
export function updateQualityOperationOther(id: string, dto: QualityOperationOtherUpdate): Promise<QualityOperationOther> {
  return request<QualityOperationOther>({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除品质业务其他通常业务费用明细
 * @param {string} id 品质业务其他通常业务费用明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityOperationOtherById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除品质业务其他通常业务费用明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityOperationOtherBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取品质业务其他通常业务费用明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityOperationOtherOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}/options`,
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
export function getQualityOperationOtherTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入品质业务其他通常业务费用明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityOperationOther(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}/import`,
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
 * 导出品质业务其他通常业务费用明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityOperationOther(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_OPERATION_OTHER_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
