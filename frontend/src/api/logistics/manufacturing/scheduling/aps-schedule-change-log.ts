// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/scheduling
// 文件名称：aps-schedule-change-log.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/scheduling 模块 API（自动生成，请勿手改路由常量）
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
  ApsScheduleChangeLog,
  ApsScheduleChangeLogCreate,
  ApsScheduleChangeLogUpdate
} from '@/types/logistics/manufacturing/scheduling/aps-schedule-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktApsScheduleChangeLogs
 */
const APS_SCHEDULE_CHANGE_LOG_API_BASE = 'TaktApsScheduleChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取APS排程变更日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ApsScheduleChangeLog>>} 分页结果
 */
export function getApsScheduleChangeLogList(queryDto: any): Promise<TaktPagedResult<ApsScheduleChangeLog>> {
  return request<TaktPagedResult<ApsScheduleChangeLog>>({
    url: `${APS_SCHEDULE_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取APS排程变更日志
 * @param {string} id APS排程变更日志ID
 * @returns {Promise<ApsScheduleChangeLog>} APS排程变更日志DTO
 */
export function getApsScheduleChangeLogById(id: string): Promise<ApsScheduleChangeLog> {
  return request<ApsScheduleChangeLog>({
    url: `${APS_SCHEDULE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建APS排程变更日志
 * @param {ApsScheduleChangeLogCreate} dto 创建DTO
 * @returns {Promise<ApsScheduleChangeLog>} APS排程变更日志DTO
 */
export function createApsScheduleChangeLog(dto: ApsScheduleChangeLogCreate): Promise<ApsScheduleChangeLog> {
  return request<ApsScheduleChangeLog>({
    url: `${APS_SCHEDULE_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新APS排程变更日志
 * @param {string} id APS排程变更日志ID
 * @param {ApsScheduleChangeLogUpdate} dto 更新DTO
 * @returns {Promise<ApsScheduleChangeLog>} APS排程变更日志DTO
 */
export function updateApsScheduleChangeLog(id: string, dto: ApsScheduleChangeLogUpdate): Promise<ApsScheduleChangeLog> {
  return request<ApsScheduleChangeLog>({
    url: `${APS_SCHEDULE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除APS排程变更日志
 * @param {string} id APS排程变更日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteApsScheduleChangeLogById(id: string): Promise<void> {
  return request({
    url: `${APS_SCHEDULE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除APS排程变更日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteApsScheduleChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${APS_SCHEDULE_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取APS排程变更日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getApsScheduleChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${APS_SCHEDULE_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出APS排程变更日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportApsScheduleChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${APS_SCHEDULE_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
