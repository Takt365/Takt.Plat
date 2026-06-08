// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/accounting/controlling
// 文件名称：cost-element-change-log.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块 API（自动生成，请勿手改路由常量）
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
  CostElementChangeLog,
  CostElementChangeLogCreate,
  CostElementChangeLogUpdate
} from '@/types/accounting/controlling/cost-element-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCostElementChangeLogs
 */
const COST_ELEMENT_CHANGE_LOG_API_BASE = 'TaktCostElementChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取成本要素变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CostElementChangeLog>>} 分页结果
 */
export function getCostElementChangeLogList(queryDto: any): Promise<TaktPagedResult<CostElementChangeLog>> {
  return request<TaktPagedResult<CostElementChangeLog>>({
    url: `${COST_ELEMENT_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取成本要素变更记录
 * @param {string} id 成本要素变更记录ID
 * @returns {Promise<CostElementChangeLog>} 成本要素变更记录DTO
 */
export function getCostElementChangeLogById(id: string): Promise<CostElementChangeLog> {
  return request<CostElementChangeLog>({
    url: `${COST_ELEMENT_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建成本要素变更记录
 * @param {CostElementChangeLogCreate} dto 创建DTO
 * @returns {Promise<CostElementChangeLog>} 成本要素变更记录DTO
 */
export function createCostElementChangeLog(dto: CostElementChangeLogCreate): Promise<CostElementChangeLog> {
  return request<CostElementChangeLog>({
    url: `${COST_ELEMENT_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新成本要素变更记录
 * @param {string} id 成本要素变更记录ID
 * @param {CostElementChangeLogUpdate} dto 更新DTO
 * @returns {Promise<CostElementChangeLog>} 成本要素变更记录DTO
 */
export function updateCostElementChangeLog(id: string, dto: CostElementChangeLogUpdate): Promise<CostElementChangeLog> {
  return request<CostElementChangeLog>({
    url: `${COST_ELEMENT_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除成本要素变更记录
 * @param {string} id 成本要素变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCostElementChangeLogById(id: string): Promise<void> {
  return request({
    url: `${COST_ELEMENT_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除成本要素变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCostElementChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${COST_ELEMENT_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取成本要素变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCostElementChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${COST_ELEMENT_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出成本要素变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCostElementChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${COST_ELEMENT_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
