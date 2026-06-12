// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：changeover.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块 API（自动生成，请勿手改路由常量）
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
  Changeover,
  ChangeoverCreate,
  ChangeoverUpdate
} from '@/types/logistics/manufacturing/output/changeover';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktChangeovers
 */
const CHANGEOVER_API_BASE = 'TaktChangeovers';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取切换记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Changeover>>} 分页结果
 */
export function getChangeoverList(queryDto: any): Promise<TaktPagedResult<Changeover>> {
  return request<TaktPagedResult<Changeover>>({
    url: `${CHANGEOVER_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取切换记录
 * @param {string} id 切换记录ID
 * @returns {Promise<Changeover>} 切换记录DTO
 */
export function getChangeoverById(id: string): Promise<Changeover> {
  return request<Changeover>({
    url: `${CHANGEOVER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建切换记录
 * @param {ChangeoverCreate} dto 创建DTO
 * @returns {Promise<Changeover>} 切换记录DTO
 */
export function createChangeover(dto: ChangeoverCreate): Promise<Changeover> {
  return request<Changeover>({
    url: `${CHANGEOVER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新切换记录
 * @param {string} id 切换记录ID
 * @param {ChangeoverUpdate} dto 更新DTO
 * @returns {Promise<Changeover>} 切换记录DTO
 */
export function updateChangeover(id: string, dto: ChangeoverUpdate): Promise<Changeover> {
  return request<Changeover>({
    url: `${CHANGEOVER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除切换记录
 * @param {string} id 切换记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteChangeoverById(id: string): Promise<void> {
  return request({
    url: `${CHANGEOVER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除切换记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteChangeoverBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CHANGEOVER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取切换记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getChangeoverOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CHANGEOVER_API_BASE}/options`,
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
export function getChangeoverTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CHANGEOVER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入切换记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importChangeover(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CHANGEOVER_API_BASE}/import`,
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
 * 导出切换记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportChangeover(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CHANGEOVER_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
