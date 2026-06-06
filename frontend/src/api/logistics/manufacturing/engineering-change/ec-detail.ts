// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec-detail.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块 API（自动生成，请勿手改路由常量）
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
  EcDetail,
  EcDetailCreate,
  EcDetailUpdate
} from '@/types/logistics/manufacturing/engineering-change/ec-detail';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcDetails
 */
const EC_DETAIL_API_BASE = 'TaktEcDetails';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcDetail>>} 分页结果
 */
export function getEcDetailList(queryDto: any): Promise<TaktPagedResult<EcDetail>> {
  return request<TaktPagedResult<EcDetail>>({
    url: `${EC_DETAIL_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取设变明细
 * @param {string} id 设变明细ID
 * @returns {Promise<EcDetail>} 设变明细DTO
 */
export function getEcDetailById(id: string): Promise<EcDetail> {
  return request<EcDetail>({
    url: `${EC_DETAIL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变明细
 * @param {EcDetailCreate} dto 创建DTO
 * @returns {Promise<EcDetail>} 设变明细DTO
 */
export function createEcDetail(dto: EcDetailCreate): Promise<EcDetail> {
  return request<EcDetail>({
    url: `${EC_DETAIL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变明细
 * @param {string} id 设变明细ID
 * @param {EcDetailUpdate} dto 更新DTO
 * @returns {Promise<EcDetail>} 设变明细DTO
 */
export function updateEcDetail(id: string, dto: EcDetailUpdate): Promise<EcDetail> {
  return request<EcDetail>({
    url: `${EC_DETAIL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变明细
 * @param {string} id 设变明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcDetailById(id: string): Promise<void> {
  return request({
    url: `${EC_DETAIL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcDetailBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_DETAIL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcDetailOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_DETAIL_API_BASE}/options`,
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
export function getEcDetailTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_DETAIL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变明细
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcDetail(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_DETAIL_API_BASE}/import`,
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
 * 导出设变明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcDetail(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_DETAIL_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
