// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/statistics/report
// 文件名称：configurable-order-by.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/report 模块 API（自动生成，请勿手改路由常量）
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
  ConfigurableOrderBy,
  ConfigurableOrderByCreate,
  ConfigurableOrderBySort,
  ConfigurableOrderByUpdate
} from '@/types/statistics/report/configurable-order-by';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktConfigurableOrderBies
 */
const CONFIGURABLE_ORDER_BY_API_BASE = 'TaktConfigurableOrderBies';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取自定义报表排序列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ConfigurableOrderBy>>} 分页结果
 */
export function getConfigurableOrderByList(queryDto: any): Promise<TaktPagedResult<ConfigurableOrderBy>> {
  return request<TaktPagedResult<ConfigurableOrderBy>>({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取自定义报表排序
 * @param {string} id 自定义报表排序ID
 * @returns {Promise<ConfigurableOrderBy>} 自定义报表排序DTO
 */
export function getConfigurableOrderByById(id: string): Promise<ConfigurableOrderBy> {
  return request<ConfigurableOrderBy>({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建自定义报表排序
 * @param {ConfigurableOrderByCreate} dto 创建DTO
 * @returns {Promise<ConfigurableOrderBy>} 自定义报表排序DTO
 */
export function createConfigurableOrderBy(dto: ConfigurableOrderByCreate): Promise<ConfigurableOrderBy> {
  return request<ConfigurableOrderBy>({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新自定义报表排序
 * @param {string} id 自定义报表排序ID
 * @param {ConfigurableOrderByUpdate} dto 更新DTO
 * @returns {Promise<ConfigurableOrderBy>} 自定义报表排序DTO
 */
export function updateConfigurableOrderBy(id: string, dto: ConfigurableOrderByUpdate): Promise<ConfigurableOrderBy> {
  return request<ConfigurableOrderBy>({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除自定义报表排序
 * @param {string} id 自定义报表排序ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteConfigurableOrderByById(id: string): Promise<void> {
  return request({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除自定义报表排序
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteConfigurableOrderByBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新自定义报表排序排序
 * @param {ConfigurableOrderBySort} dto 排序DTO
 * @returns {Promise<ConfigurableOrderBy>} 自定义报表排序DTO
 */
export function updateConfigurableOrderBySort(dto: ConfigurableOrderBySort): Promise<ConfigurableOrderBy> {
  return request<ConfigurableOrderBy>({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取自定义报表排序选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getConfigurableOrderByOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/options`,
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
export function getConfigurableOrderByTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入自定义报表排序
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importConfigurableOrderBy(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/import`,
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
 * 导出自定义报表排序
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportConfigurableOrderBy(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CONFIGURABLE_ORDER_BY_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
