// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：standard-operation-rate-change-log.ts
// 创建时间：2026-06-30
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
  StandardOperationRateChangeLog,
  StandardOperationRateChangeLogCreate,
  StandardOperationRateChangeLogUpdate
} from '@/types/logistics/manufacturing/output/standard-operation-rate-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktStandardOperationRateChangeLogs
 */
const STANDARD_OPERATION_RATE_CHANGE_LOG_API_BASE = 'TaktStandardOperationRateChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取标准生产稼动率变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<StandardOperationRateChangeLog>>} 分页结果
 */
export function getStandardOperationRateChangeLogList(queryDto: any): Promise<TaktPagedResult<StandardOperationRateChangeLog>> {
  return request<TaktPagedResult<StandardOperationRateChangeLog>>({
    url: `${STANDARD_OPERATION_RATE_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取标准生产稼动率变更记录
 * @param {string} id 标准生产稼动率变更记录ID
 * @returns {Promise<StandardOperationRateChangeLog>} 标准生产稼动率变更记录DTO
 */
export function getStandardOperationRateChangeLogById(id: string): Promise<StandardOperationRateChangeLog> {
  return request<StandardOperationRateChangeLog>({
    url: `${STANDARD_OPERATION_RATE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建标准生产稼动率变更记录
 * @param {StandardOperationRateChangeLogCreate} dto 创建DTO
 * @returns {Promise<StandardOperationRateChangeLog>} 标准生产稼动率变更记录DTO
 */
export function createStandardOperationRateChangeLog(dto: StandardOperationRateChangeLogCreate): Promise<StandardOperationRateChangeLog> {
  return request<StandardOperationRateChangeLog>({
    url: `${STANDARD_OPERATION_RATE_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新标准生产稼动率变更记录
 * @param {string} id 标准生产稼动率变更记录ID
 * @param {StandardOperationRateChangeLogUpdate} dto 更新DTO
 * @returns {Promise<StandardOperationRateChangeLog>} 标准生产稼动率变更记录DTO
 */
export function updateStandardOperationRateChangeLog(id: string, dto: StandardOperationRateChangeLogUpdate): Promise<StandardOperationRateChangeLog> {
  return request<StandardOperationRateChangeLog>({
    url: `${STANDARD_OPERATION_RATE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除标准生产稼动率变更记录
 * @param {string} id 标准生产稼动率变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardOperationRateChangeLogById(id: string): Promise<void> {
  return request({
    url: `${STANDARD_OPERATION_RATE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除标准生产稼动率变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardOperationRateChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${STANDARD_OPERATION_RATE_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取标准生产稼动率变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getStandardOperationRateChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${STANDARD_OPERATION_RATE_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出标准生产稼动率变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportStandardOperationRateChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${STANDARD_OPERATION_RATE_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
