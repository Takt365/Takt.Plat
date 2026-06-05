// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：routing-change-log.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块 API（自动生成，请勿手改路由常量）
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
  RoutingChangeLog,
  RoutingChangeLogCreate,
  RoutingChangeLogUpdate
} from '@/types/logistics/manufacturing/bom/routing-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktRoutingChangeLogs
 */
const ROUTING_CHANGE_LOG_API_BASE = 'TaktRoutingChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工艺路线变更日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<RoutingChangeLog>>} 分页结果
 */
export function getRoutingChangeLogList(queryDto: any): Promise<TaktPagedResult<RoutingChangeLog>> {
  return request<TaktPagedResult<RoutingChangeLog>>({
    url: `${ROUTING_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取工艺路线变更日志
 * @param {string} id 工艺路线变更日志ID
 * @returns {Promise<RoutingChangeLog>} 工艺路线变更日志DTO
 */
export function getRoutingChangeLogById(id: string): Promise<RoutingChangeLog> {
  return request<RoutingChangeLog>({
    url: `${ROUTING_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工艺路线变更日志
 * @param {RoutingChangeLogCreate} dto 创建DTO
 * @returns {Promise<RoutingChangeLog>} 工艺路线变更日志DTO
 */
export function createRoutingChangeLog(dto: RoutingChangeLogCreate): Promise<RoutingChangeLog> {
  return request<RoutingChangeLog>({
    url: `${ROUTING_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工艺路线变更日志
 * @param {string} id 工艺路线变更日志ID
 * @param {RoutingChangeLogUpdate} dto 更新DTO
 * @returns {Promise<RoutingChangeLog>} 工艺路线变更日志DTO
 */
export function updateRoutingChangeLog(id: string, dto: RoutingChangeLogUpdate): Promise<RoutingChangeLog> {
  return request<RoutingChangeLog>({
    url: `${ROUTING_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工艺路线变更日志
 * @param {string} id 工艺路线变更日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoutingChangeLogById(id: string): Promise<void> {
  return request({
    url: `${ROUTING_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工艺路线变更日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoutingChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ROUTING_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工艺路线变更日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getRoutingChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ROUTING_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出工艺路线变更日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportRoutingChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ROUTING_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
