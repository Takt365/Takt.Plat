// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：sales-price-change-log.ts
// 创建时间：2026-06-07
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
  SalesPriceChangeLog,
  SalesPriceChangeLogCreate,
  SalesPriceChangeLogUpdate
} from '@/types/logistics/sales/sales-price-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesPriceChangeLogs
 */
const SALES_PRICE_CHANGE_LOG_API_BASE = 'TaktSalesPriceChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售价格变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesPriceChangeLog>>} 分页结果
 */
export function getSalesPriceChangeLogList(queryDto: any): Promise<TaktPagedResult<SalesPriceChangeLog>> {
  return request<TaktPagedResult<SalesPriceChangeLog>>({
    url: `${SALES_PRICE_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取销售价格变更记录
 * @param {string} id 销售价格变更记录ID
 * @returns {Promise<SalesPriceChangeLog>} 销售价格变更记录DTO
 */
export function getSalesPriceChangeLogById(id: string): Promise<SalesPriceChangeLog> {
  return request<SalesPriceChangeLog>({
    url: `${SALES_PRICE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售价格变更记录
 * @param {SalesPriceChangeLogCreate} dto 创建DTO
 * @returns {Promise<SalesPriceChangeLog>} 销售价格变更记录DTO
 */
export function createSalesPriceChangeLog(dto: SalesPriceChangeLogCreate): Promise<SalesPriceChangeLog> {
  return request<SalesPriceChangeLog>({
    url: `${SALES_PRICE_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售价格变更记录
 * @param {string} id 销售价格变更记录ID
 * @param {SalesPriceChangeLogUpdate} dto 更新DTO
 * @returns {Promise<SalesPriceChangeLog>} 销售价格变更记录DTO
 */
export function updateSalesPriceChangeLog(id: string, dto: SalesPriceChangeLogUpdate): Promise<SalesPriceChangeLog> {
  return request<SalesPriceChangeLog>({
    url: `${SALES_PRICE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售价格变更记录
 * @param {string} id 销售价格变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceChangeLogById(id: string): Promise<void> {
  return request({
    url: `${SALES_PRICE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售价格变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_PRICE_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售价格变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesPriceChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_PRICE_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出销售价格变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesPriceChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
