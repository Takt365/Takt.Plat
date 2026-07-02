// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：event-tracking-log.ts
// 创建时间：2026-06-25
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
  EventTrackingLog,
  EventTrackingLogBatchTrack,
  EventTrackingLogCreate,
  EventTrackingLogTrackResult,
  EventTrackingLogUpdate
} from '@/types/statistics/logging/event-tracking-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEventTrackingLogs
 */
const EVENT_TRACKING_LOG_API_BASE = 'TaktEventTrackingLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取交互日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EventTrackingLog>>} 分页结果
 */
export function getEventTrackingLogList(queryDto: any): Promise<TaktPagedResult<EventTrackingLog>> {
  return request<TaktPagedResult<EventTrackingLog>>({
    url: `${EVENT_TRACKING_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取交互日志
 * @param {string} id 交互日志ID
 * @returns {Promise<EventTrackingLog>} 交互日志DTO
 */
export function getEventTrackingLogById(id: string): Promise<EventTrackingLog> {
  return request<EventTrackingLog>({
    url: `${EVENT_TRACKING_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建交互日志
 * @param {EventTrackingLogCreate} dto 创建DTO
 * @returns {Promise<EventTrackingLog>} 交互日志DTO
 */
export function createEventTrackingLog(dto: EventTrackingLogCreate): Promise<EventTrackingLog> {
  return request<EventTrackingLog>({
    url: `${EVENT_TRACKING_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新交互日志
 * @param {string} id 交互日志ID
 * @param {EventTrackingLogUpdate} dto 更新DTO
 * @returns {Promise<EventTrackingLog>} 交互日志DTO
 */
export function updateEventTrackingLog(id: string, dto: EventTrackingLogUpdate): Promise<EventTrackingLog> {
  return request<EventTrackingLog>({
    url: `${EVENT_TRACKING_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除交互日志
 * @param {string} id 交互日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEventTrackingLogById(id: string): Promise<void> {
  return request({
    url: `${EVENT_TRACKING_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除交互日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEventTrackingLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EVENT_TRACKING_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取交互日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEventTrackingLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EVENT_TRACKING_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// Long Task 客户端上报
// ========================================

/**
 * 批量上报 Long Task 等客户端性能事件
 * @param {EventTrackingLogBatchTrack} dto 批量上报 DTO
 * @returns {Promise<EventTrackingLogTrackResult>} 写入条数
 */
export function trackEventTrackingLogBatch(dto: EventTrackingLogBatchTrack): Promise<EventTrackingLogTrackResult> {
  return request<EventTrackingLogTrackResult>({
    url: `${EVENT_TRACKING_LOG_API_BASE}/track-batch`,
    method: 'post',
    data: dto,
    skipApiPerformanceTrack: true,
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出交互日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEventTrackingLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EVENT_TRACKING_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
