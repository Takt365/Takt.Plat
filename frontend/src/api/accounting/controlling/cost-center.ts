// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/accounting/controlling
// 文件名称：cost-center.ts
// 创建时间：2026-06-21
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktTreeSelectOption
} from '@/types/common';
import type {
  CostCenter,
  CostCenterCreate,
  CostCenterSort,
  CostCenterStatus,
  CostCenterTree,
  CostCenterUpdate
} from '@/types/accounting/controlling/cost-center';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCostCenters
 */
const COST_CENTER_API_BASE = 'TaktCostCenters';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取成本中心列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CostCenter>>} 分页结果
 */
export function getCostCenterList(queryDto: any): Promise<TaktPagedResult<CostCenter>> {
  return request<TaktPagedResult<CostCenter>>({
    url: `${COST_CENTER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取成本中心
 * @param {string} id 成本中心ID
 * @returns {Promise<CostCenter>} 成本中心DTO
 */
export function getCostCenterById(id: string): Promise<CostCenter> {
  return request<CostCenter>({
    url: `${COST_CENTER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取成本中心树形列表
 * @param {string} parentId parentId
 * @param {boolean} includeDisabled 为 false 时过滤禁用项（按实体 *Status 枚举字段，如 TaktCommonStatus.Enabled）
 * @returns {Promise<CostCenterTree[]>} 树形数据
 */
export function getCostCenterTree(parentId: string, includeDisabled: boolean): Promise<CostCenterTree[]> {
  return request<CostCenterTree[]>({
    url: `${COST_CENTER_API_BASE}/tree`,
    method: 'get',
    params: {
      parentId,
      includeDisabled
    },
  });
}

/**
 * 创建成本中心
 * @param {CostCenterCreate} dto 创建DTO
 * @returns {Promise<CostCenter>} 成本中心DTO
 */
export function createCostCenter(dto: CostCenterCreate): Promise<CostCenter> {
  return request<CostCenter>({
    url: `${COST_CENTER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新成本中心
 * @param {string} id 成本中心ID
 * @param {CostCenterUpdate} dto 更新DTO
 * @returns {Promise<CostCenter>} 成本中心DTO
 */
export function updateCostCenter(id: string, dto: CostCenterUpdate): Promise<CostCenter> {
  return request<CostCenter>({
    url: `${COST_CENTER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除成本中心
 * @param {string} id 成本中心ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCostCenterById(id: string): Promise<void> {
  return request({
    url: `${COST_CENTER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除成本中心
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCostCenterBatch(ids: string[]): Promise<void> {
  return request({
    url: `${COST_CENTER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新成本中心状态
 * @param {CostCenterStatus} dto 状态 DTO
 * @returns {Promise<CostCenter>} 成本中心DTO
 */
export function updateCostCenterStatus(dto: CostCenterStatus): Promise<CostCenter> {
  return request<CostCenter>({
    url: `${COST_CENTER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新成本中心排序
 * @param {CostCenterSort} dto 排序DTO
 * @returns {Promise<CostCenter>} 成本中心DTO
 */
export function updateCostCenterSort(dto: CostCenterSort): Promise<CostCenter> {
  return request<CostCenter>({
    url: `${COST_CENTER_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取成本中心树形选项列表
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
export function getCostCenterTreeOptions(): Promise<TaktTreeSelectOption[]> {
  return request<TaktTreeSelectOption[]>({
    url: `${COST_CENTER_API_BASE}/tree-options`,
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
export function getCostCenterTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${COST_CENTER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入成本中心
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCostCenter(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${COST_CENTER_API_BASE}/import`,
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
 * 导出成本中心
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCostCenter(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${COST_CENTER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
