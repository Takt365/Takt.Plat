// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：fqc-order-change-log.ts
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
  FqcOrderChangeLog,
  FqcOrderChangeLogCreate,
  FqcOrderChangeLogUpdate
} from '@/types/logistics/quality/operation/fqc-order-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFqcOrderChangeLogs
 */
const FQC_ORDER_CHANGE_LOG_API_BASE = 'TaktFqcOrderChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取出货检验单变更日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FqcOrderChangeLog>>} 分页结果
 */
export function getFqcOrderChangeLogList(queryDto: any): Promise<TaktPagedResult<FqcOrderChangeLog>> {
  return request<TaktPagedResult<FqcOrderChangeLog>>({
    url: `${FQC_ORDER_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取出货检验单变更日志
 * @param {string} id 出货检验单变更日志ID
 * @returns {Promise<FqcOrderChangeLog>} 出货检验单变更日志DTO
 */
export function getFqcOrderChangeLogById(id: string): Promise<FqcOrderChangeLog> {
  return request<FqcOrderChangeLog>({
    url: `${FQC_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建出货检验单变更日志
 * @param {FqcOrderChangeLogCreate} dto 创建DTO
 * @returns {Promise<FqcOrderChangeLog>} 出货检验单变更日志DTO
 */
export function createFqcOrderChangeLog(dto: FqcOrderChangeLogCreate): Promise<FqcOrderChangeLog> {
  return request<FqcOrderChangeLog>({
    url: `${FQC_ORDER_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新出货检验单变更日志
 * @param {string} id 出货检验单变更日志ID
 * @param {FqcOrderChangeLogUpdate} dto 更新DTO
 * @returns {Promise<FqcOrderChangeLog>} 出货检验单变更日志DTO
 */
export function updateFqcOrderChangeLog(id: string, dto: FqcOrderChangeLogUpdate): Promise<FqcOrderChangeLog> {
  return request<FqcOrderChangeLog>({
    url: `${FQC_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除出货检验单变更日志
 * @param {string} id 出货检验单变更日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFqcOrderChangeLogById(id: string): Promise<void> {
  return request({
    url: `${FQC_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除出货检验单变更日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFqcOrderChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FQC_ORDER_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取出货检验单变更日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFqcOrderChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${FQC_ORDER_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出出货检验单变更日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFqcOrderChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FQC_ORDER_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
