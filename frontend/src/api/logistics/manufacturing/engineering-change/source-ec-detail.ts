// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：source-ec-detail.ts
// 创建时间：2026-06-27
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
  SourceEcDetail,
  SourceEcDetailCreate,
  SourceEcDetailUpdate
} from '@/types/logistics/manufacturing/engineering-change/source-ec-detail';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSourceEcDetails
 */
const SOURCE_EC_DETAIL_API_BASE = 'TaktSourceEcDetails';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变来源子列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SourceEcDetail>>} 分页结果
 */
export function getSourceEcDetailList(queryDto: any): Promise<TaktPagedResult<SourceEcDetail>> {
  return request<TaktPagedResult<SourceEcDetail>>({
    url: `${SOURCE_EC_DETAIL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变来源子
 * @param {string} id 设变来源子ID
 * @returns {Promise<SourceEcDetail>} 设变来源子DTO
 */
export function getSourceEcDetailById(id: string): Promise<SourceEcDetail> {
  return request<SourceEcDetail>({
    url: `${SOURCE_EC_DETAIL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变来源子
 * @param {SourceEcDetailCreate} dto 创建DTO
 * @returns {Promise<SourceEcDetail>} 设变来源子DTO
 */
export function createSourceEcDetail(dto: SourceEcDetailCreate): Promise<SourceEcDetail> {
  return request<SourceEcDetail>({
    url: `${SOURCE_EC_DETAIL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变来源子
 * @param {string} id 设变来源子ID
 * @param {SourceEcDetailUpdate} dto 更新DTO
 * @returns {Promise<SourceEcDetail>} 设变来源子DTO
 */
export function updateSourceEcDetail(id: string, dto: SourceEcDetailUpdate): Promise<SourceEcDetail> {
  return request<SourceEcDetail>({
    url: `${SOURCE_EC_DETAIL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变来源子
 * @param {string} id 设变来源子ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSourceEcDetailById(id: string): Promise<void> {
  return request({
    url: `${SOURCE_EC_DETAIL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变来源子
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSourceEcDetailBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOURCE_EC_DETAIL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变来源子选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSourceEcDetailOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SOURCE_EC_DETAIL_API_BASE}/options`,
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
export function getSourceEcDetailTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOURCE_EC_DETAIL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变来源子
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSourceEcDetail(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOURCE_EC_DETAIL_API_BASE}/import`,
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
 * 导出设变来源子
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSourceEcDetail(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOURCE_EC_DETAIL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
