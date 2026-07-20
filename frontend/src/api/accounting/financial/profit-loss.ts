// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：profit-loss.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块 API（自动生成，请勿手改路由常量）
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
  ProfitLoss,
  ProfitLossCreate,
  ProfitLossSort,
  ProfitLossStatus,
  ProfitLossUpdate
} from '@/types/accounting/financial/profit-loss';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProfitLosses
 */
const PROFIT_LOSS_API_BASE = 'TaktProfitLosses';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取利润列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProfitLoss>>} 分页结果
 */
export function getProfitLossList(queryDto: any): Promise<TaktPagedResult<ProfitLoss>> {
  return request<TaktPagedResult<ProfitLoss>>({
    url: `${PROFIT_LOSS_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取利润
 * @param {string} id 利润ID
 * @returns {Promise<ProfitLoss>} 利润DTO
 */
export function getProfitLossById(id: string): Promise<ProfitLoss> {
  return request<ProfitLoss>({
    url: `${PROFIT_LOSS_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建利润
 * @param {ProfitLossCreate} dto 创建DTO
 * @returns {Promise<ProfitLoss>} 利润DTO
 */
export function createProfitLoss(dto: ProfitLossCreate): Promise<ProfitLoss> {
  return request<ProfitLoss>({
    url: `${PROFIT_LOSS_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新利润
 * @param {string} id 利润ID
 * @param {ProfitLossUpdate} dto 更新DTO
 * @returns {Promise<ProfitLoss>} 利润DTO
 */
export function updateProfitLoss(id: string, dto: ProfitLossUpdate): Promise<ProfitLoss> {
  return request<ProfitLoss>({
    url: `${PROFIT_LOSS_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除利润
 * @param {string} id 利润ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProfitLossById(id: string): Promise<void> {
  return request({
    url: `${PROFIT_LOSS_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除利润
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProfitLossBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PROFIT_LOSS_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新利润状态
 * @param {ProfitLossStatus} dto 状态 DTO
 * @returns {Promise<ProfitLoss>} 利润DTO
 */
export function updateProfitLossStatus(dto: ProfitLossStatus): Promise<ProfitLoss> {
  return request<ProfitLoss>({
    url: `${PROFIT_LOSS_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新利润排序
 * @param {ProfitLossSort} dto 排序DTO
 * @returns {Promise<ProfitLoss>} 利润DTO
 */
export function updateProfitLossSort(dto: ProfitLossSort): Promise<ProfitLoss> {
  return request<ProfitLoss>({
    url: `${PROFIT_LOSS_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取利润选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProfitLossOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PROFIT_LOSS_API_BASE}/options`,
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
export function getProfitLossTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PROFIT_LOSS_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入利润
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProfitLoss(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PROFIT_LOSS_API_BASE}/import`,
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
 * 导出利润
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProfitLoss(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PROFIT_LOSS_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
