// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/accounting/controlling
// 文件名称：profit-center-change-log.ts
// 创建时间：2026-06-07
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
  ProfitCenterChangeLog,
  ProfitCenterChangeLogCreate,
  ProfitCenterChangeLogUpdate
} from '@/types/accounting/controlling/profit-center-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProfitCenterChangeLogs
 */
const PROFIT_CENTER_CHANGE_LOG_API_BASE = 'TaktProfitCenterChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取利润中心变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProfitCenterChangeLog>>} 分页结果
 */
export function getProfitCenterChangeLogList(queryDto: any): Promise<TaktPagedResult<ProfitCenterChangeLog>> {
  return request<TaktPagedResult<ProfitCenterChangeLog>>({
    url: `${PROFIT_CENTER_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取利润中心变更记录
 * @param {string} id 利润中心变更记录ID
 * @returns {Promise<ProfitCenterChangeLog>} 利润中心变更记录DTO
 */
export function getProfitCenterChangeLogById(id: string): Promise<ProfitCenterChangeLog> {
  return request<ProfitCenterChangeLog>({
    url: `${PROFIT_CENTER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建利润中心变更记录
 * @param {ProfitCenterChangeLogCreate} dto 创建DTO
 * @returns {Promise<ProfitCenterChangeLog>} 利润中心变更记录DTO
 */
export function createProfitCenterChangeLog(dto: ProfitCenterChangeLogCreate): Promise<ProfitCenterChangeLog> {
  return request<ProfitCenterChangeLog>({
    url: `${PROFIT_CENTER_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新利润中心变更记录
 * @param {string} id 利润中心变更记录ID
 * @param {ProfitCenterChangeLogUpdate} dto 更新DTO
 * @returns {Promise<ProfitCenterChangeLog>} 利润中心变更记录DTO
 */
export function updateProfitCenterChangeLog(id: string, dto: ProfitCenterChangeLogUpdate): Promise<ProfitCenterChangeLog> {
  return request<ProfitCenterChangeLog>({
    url: `${PROFIT_CENTER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除利润中心变更记录
 * @param {string} id 利润中心变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProfitCenterChangeLogById(id: string): Promise<void> {
  return request({
    url: `${PROFIT_CENTER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除利润中心变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProfitCenterChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PROFIT_CENTER_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取利润中心变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProfitCenterChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PROFIT_CENTER_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出利润中心变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProfitCenterChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PROFIT_CENTER_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
