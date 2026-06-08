// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：quality-failure-pcba-rework.ts
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
  QualityFailurePcbaRework,
  QualityFailurePcbaReworkCreate,
  QualityFailurePcbaReworkUpdate
} from '@/types/logistics/quality/cost/quality-failure-pcba-rework';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityFailurePcbaReworks
 */
const QUALITY_FAILURE_PCBA_REWORK_API_BASE = 'TaktQualityFailurePcbaReworks';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取质量问题PCBA不良改修费用明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityFailurePcbaRework>>} 分页结果
 */
export function getQualityFailurePcbaReworkList(queryDto: any): Promise<TaktPagedResult<QualityFailurePcbaRework>> {
  return request<TaktPagedResult<QualityFailurePcbaRework>>({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取质量问题PCBA不良改修费用明细
 * @param {string} id 质量问题PCBA不良改修费用明细ID
 * @returns {Promise<QualityFailurePcbaRework>} 质量问题PCBA不良改修费用明细DTO
 */
export function getQualityFailurePcbaReworkById(id: string): Promise<QualityFailurePcbaRework> {
  return request<QualityFailurePcbaRework>({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建质量问题PCBA不良改修费用明细
 * @param {QualityFailurePcbaReworkCreate} dto 创建DTO
 * @returns {Promise<QualityFailurePcbaRework>} 质量问题PCBA不良改修费用明细DTO
 */
export function createQualityFailurePcbaRework(dto: QualityFailurePcbaReworkCreate): Promise<QualityFailurePcbaRework> {
  return request<QualityFailurePcbaRework>({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新质量问题PCBA不良改修费用明细
 * @param {string} id 质量问题PCBA不良改修费用明细ID
 * @param {QualityFailurePcbaReworkUpdate} dto 更新DTO
 * @returns {Promise<QualityFailurePcbaRework>} 质量问题PCBA不良改修费用明细DTO
 */
export function updateQualityFailurePcbaRework(id: string, dto: QualityFailurePcbaReworkUpdate): Promise<QualityFailurePcbaRework> {
  return request<QualityFailurePcbaRework>({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除质量问题PCBA不良改修费用明细
 * @param {string} id 质量问题PCBA不良改修费用明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityFailurePcbaReworkById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除质量问题PCBA不良改修费用明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityFailurePcbaReworkBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取质量问题PCBA不良改修费用明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityFailurePcbaReworkOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}/options`,
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
export function getQualityFailurePcbaReworkTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入质量问题PCBA不良改修费用明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityFailurePcbaRework(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}/import`,
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
 * 导出质量问题PCBA不良改修费用明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityFailurePcbaRework(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_FAILURE_PCBA_REWORK_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
