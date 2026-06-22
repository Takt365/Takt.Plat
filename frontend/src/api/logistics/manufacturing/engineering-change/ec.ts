// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec.ts
// 创建时间：2026-06-20
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
  Ec,
  EcCreate,
  EcStatus,
  EcUpdate
} from '@/types/logistics/manufacturing/engineering-change/ec';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcs
 */
const EC_API_BASE = 'TaktEcs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变主列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Ec>>} 分页结果
 */
export function getEcList(queryDto: any): Promise<TaktPagedResult<Ec>> {
  return request<TaktPagedResult<Ec>>({
    url: `${EC_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变主
 * @param {string} id 设变主ID
 * @returns {Promise<Ec>} 设变主DTO
 */
export function getEcById(id: string): Promise<Ec> {
  return request<Ec>({
    url: `${EC_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变主
 * @param {EcCreate} dto 创建DTO
 * @returns {Promise<Ec>} 设变主DTO
 */
export function createEc(dto: EcCreate): Promise<Ec> {
  return request<Ec>({
    url: `${EC_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变主
 * @param {string} id 设变主ID
 * @param {EcUpdate} dto 更新DTO
 * @returns {Promise<Ec>} 设变主DTO
 */
export function updateEc(id: string, dto: EcUpdate): Promise<Ec> {
  return request<Ec>({
    url: `${EC_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变主
 * @param {string} id 设变主ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcById(id: string): Promise<void> {
  return request({
    url: `${EC_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变主
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变主状态
 * @param {EcStatus} dto 状态 DTO
 * @returns {Promise<Ec>} 设变主DTO
 */
export function updateEcStatus(dto: EcStatus): Promise<Ec> {
  return request<Ec>({
    url: `${EC_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变主选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_API_BASE}/options`,
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
export function getEcTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变主
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEc(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_API_BASE}/import`,
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
 * 导出设变主
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEc(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
