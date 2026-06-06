// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：ticket-change-log.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块 API（自动生成，请勿手改路由常量）
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
  TicketChangeLog,
  TicketChangeLogCreate,
  TicketChangeLogUpdate
} from '@/types/routine/help-desk/ticket-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTicketChangeLogs
 */
const TICKET_CHANGE_LOG_API_BASE = 'TaktTicketChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工单变更日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TicketChangeLog>>} 分页结果
 */
export function getTicketChangeLogList(queryDto: any): Promise<TaktPagedResult<TicketChangeLog>> {
  return request<TaktPagedResult<TicketChangeLog>>({
    url: `${TICKET_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取工单变更日志
 * @param {string} id 工单变更日志ID
 * @returns {Promise<TicketChangeLog>} 工单变更日志DTO
 */
export function getTicketChangeLogById(id: string): Promise<TicketChangeLog> {
  return request<TicketChangeLog>({
    url: `${TICKET_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工单变更日志
 * @param {TicketChangeLogCreate} dto 创建DTO
 * @returns {Promise<TicketChangeLog>} 工单变更日志DTO
 */
export function createTicketChangeLog(dto: TicketChangeLogCreate): Promise<TicketChangeLog> {
  return request<TicketChangeLog>({
    url: `${TICKET_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工单变更日志
 * @param {string} id 工单变更日志ID
 * @param {TicketChangeLogUpdate} dto 更新DTO
 * @returns {Promise<TicketChangeLog>} 工单变更日志DTO
 */
export function updateTicketChangeLog(id: string, dto: TicketChangeLogUpdate): Promise<TicketChangeLog> {
  return request<TicketChangeLog>({
    url: `${TICKET_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工单变更日志
 * @param {string} id 工单变更日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketChangeLogById(id: string): Promise<void> {
  return request({
    url: `${TICKET_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工单变更日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TICKET_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工单变更日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTicketChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${TICKET_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出工单变更日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTicketChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
