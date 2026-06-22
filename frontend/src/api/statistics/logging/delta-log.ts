// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：delta-log.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/logging 模块 API（自动生成，请勿手改路由常量）
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
  DeltaLog,
  DeltaLogCreate,
  DeltaLogUpdate
} from '@/types/statistics/logging/delta-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDeltaLogs
 */
const DELTA_LOG_API_BASE = 'TaktDeltaLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取差异日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<DeltaLog>>} 分页结果
 */
export function getDeltaLogList(queryDto: any): Promise<TaktPagedResult<DeltaLog>> {
  return request<TaktPagedResult<DeltaLog>>({
    url: `${DELTA_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取差异日志
 * @param {string} id 差异日志ID
 * @returns {Promise<DeltaLog>} 差异日志DTO
 */
export function getDeltaLogById(id: string): Promise<DeltaLog> {
  return request<DeltaLog>({
    url: `${DELTA_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建差异日志
 * @param {DeltaLogCreate} dto 创建DTO
 * @returns {Promise<DeltaLog>} 差异日志DTO
 */
export function createDeltaLog(dto: DeltaLogCreate): Promise<DeltaLog> {
  return request<DeltaLog>({
    url: `${DELTA_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新差异日志
 * @param {string} id 差异日志ID
 * @param {DeltaLogUpdate} dto 更新DTO
 * @returns {Promise<DeltaLog>} 差异日志DTO
 */
export function updateDeltaLog(id: string, dto: DeltaLogUpdate): Promise<DeltaLog> {
  return request<DeltaLog>({
    url: `${DELTA_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除差异日志
 * @param {string} id 差异日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDeltaLogById(id: string): Promise<void> {
  return request({
    url: `${DELTA_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除差异日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDeltaLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DELTA_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取差异日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getDeltaLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${DELTA_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出差异日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDeltaLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DELTA_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
