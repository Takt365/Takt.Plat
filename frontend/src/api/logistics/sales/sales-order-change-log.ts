// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：sales-order-change-log.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块 API（自动生成，请勿手改路由常量）
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
  SalesOrderChangeLog,
  SalesOrderChangeLogCreate,
  SalesOrderChangeLogUpdate
} from '@/types/logistics/sales/sales-order-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesOrderChangeLogs
 */
const SALES_ORDER_CHANGE_LOG_API_BASE = 'TaktSalesOrderChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售订单变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesOrderChangeLog>>} 分页结果
 */
export function getSalesOrderChangeLogList(queryDto: any): Promise<TaktPagedResult<SalesOrderChangeLog>> {
  return request<TaktPagedResult<SalesOrderChangeLog>>({
    url: `${SALES_ORDER_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取销售订单变更记录
 * @param {string} id 销售订单变更记录ID
 * @returns {Promise<SalesOrderChangeLog>} 销售订单变更记录DTO
 */
export function getSalesOrderChangeLogById(id: string): Promise<SalesOrderChangeLog> {
  return request<SalesOrderChangeLog>({
    url: `${SALES_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售订单变更记录
 * @param {SalesOrderChangeLogCreate} dto 创建DTO
 * @returns {Promise<SalesOrderChangeLog>} 销售订单变更记录DTO
 */
export function createSalesOrderChangeLog(dto: SalesOrderChangeLogCreate): Promise<SalesOrderChangeLog> {
  return request<SalesOrderChangeLog>({
    url: `${SALES_ORDER_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售订单变更记录
 * @param {string} id 销售订单变更记录ID
 * @param {SalesOrderChangeLogUpdate} dto 更新DTO
 * @returns {Promise<SalesOrderChangeLog>} 销售订单变更记录DTO
 */
export function updateSalesOrderChangeLog(id: string, dto: SalesOrderChangeLogUpdate): Promise<SalesOrderChangeLog> {
  return request<SalesOrderChangeLog>({
    url: `${SALES_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售订单变更记录
 * @param {string} id 销售订单变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesOrderChangeLogById(id: string): Promise<void> {
  return request({
    url: `${SALES_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售订单变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesOrderChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_ORDER_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售订单变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesOrderChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_ORDER_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出销售订单变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesOrderChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_ORDER_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
