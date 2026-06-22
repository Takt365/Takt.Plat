// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：ipqc-order-change-log.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块 API（自动生成，请勿手改路由常量）
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
  IpqcOrderChangeLog,
  IpqcOrderChangeLogCreate,
  IpqcOrderChangeLogUpdate
} from '@/types/logistics/quality/operation/ipqc-order-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktIpqcOrderChangeLogs
 */
const IPQC_ORDER_CHANGE_LOG_API_BASE = 'TaktIpqcOrderChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取制程检验单变更日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<IpqcOrderChangeLog>>} 分页结果
 */
export function getIpqcOrderChangeLogList(queryDto: any): Promise<TaktPagedResult<IpqcOrderChangeLog>> {
  return request<TaktPagedResult<IpqcOrderChangeLog>>({
    url: `${IPQC_ORDER_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取制程检验单变更日志
 * @param {string} id 制程检验单变更日志ID
 * @returns {Promise<IpqcOrderChangeLog>} 制程检验单变更日志DTO
 */
export function getIpqcOrderChangeLogById(id: string): Promise<IpqcOrderChangeLog> {
  return request<IpqcOrderChangeLog>({
    url: `${IPQC_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建制程检验单变更日志
 * @param {IpqcOrderChangeLogCreate} dto 创建DTO
 * @returns {Promise<IpqcOrderChangeLog>} 制程检验单变更日志DTO
 */
export function createIpqcOrderChangeLog(dto: IpqcOrderChangeLogCreate): Promise<IpqcOrderChangeLog> {
  return request<IpqcOrderChangeLog>({
    url: `${IPQC_ORDER_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新制程检验单变更日志
 * @param {string} id 制程检验单变更日志ID
 * @param {IpqcOrderChangeLogUpdate} dto 更新DTO
 * @returns {Promise<IpqcOrderChangeLog>} 制程检验单变更日志DTO
 */
export function updateIpqcOrderChangeLog(id: string, dto: IpqcOrderChangeLogUpdate): Promise<IpqcOrderChangeLog> {
  return request<IpqcOrderChangeLog>({
    url: `${IPQC_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除制程检验单变更日志
 * @param {string} id 制程检验单变更日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteIpqcOrderChangeLogById(id: string): Promise<void> {
  return request({
    url: `${IPQC_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除制程检验单变更日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteIpqcOrderChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${IPQC_ORDER_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取制程检验单变更日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getIpqcOrderChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${IPQC_ORDER_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出制程检验单变更日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportIpqcOrderChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${IPQC_ORDER_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
