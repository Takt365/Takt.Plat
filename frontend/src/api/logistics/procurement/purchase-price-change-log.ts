// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchase-price-change-log.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块 API（自动生成，请勿手改路由常量）
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
  PurchasePriceChangeLog,
  PurchasePriceChangeLogCreate,
  PurchasePriceChangeLogUpdate
} from '@/types/logistics/procurement/purchase-price-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchasePriceChangeLogs
 */
const PURCHASE_PRICE_CHANGE_LOG_API_BASE = 'TaktPurchasePriceChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购价格变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchasePriceChangeLog>>} 分页结果
 */
export function getPurchasePriceChangeLogList(queryDto: any): Promise<TaktPagedResult<PurchasePriceChangeLog>> {
  return request<TaktPagedResult<PurchasePriceChangeLog>>({
    url: `${PURCHASE_PRICE_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购价格变更记录
 * @param {string} id 采购价格变更记录ID
 * @returns {Promise<PurchasePriceChangeLog>} 采购价格变更记录DTO
 */
export function getPurchasePriceChangeLogById(id: string): Promise<PurchasePriceChangeLog> {
  return request<PurchasePriceChangeLog>({
    url: `${PURCHASE_PRICE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购价格变更记录
 * @param {PurchasePriceChangeLogCreate} dto 创建DTO
 * @returns {Promise<PurchasePriceChangeLog>} 采购价格变更记录DTO
 */
export function createPurchasePriceChangeLog(dto: PurchasePriceChangeLogCreate): Promise<PurchasePriceChangeLog> {
  return request<PurchasePriceChangeLog>({
    url: `${PURCHASE_PRICE_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购价格变更记录
 * @param {string} id 采购价格变更记录ID
 * @param {PurchasePriceChangeLogUpdate} dto 更新DTO
 * @returns {Promise<PurchasePriceChangeLog>} 采购价格变更记录DTO
 */
export function updatePurchasePriceChangeLog(id: string, dto: PurchasePriceChangeLogUpdate): Promise<PurchasePriceChangeLog> {
  return request<PurchasePriceChangeLog>({
    url: `${PURCHASE_PRICE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购价格变更记录
 * @param {string} id 采购价格变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceChangeLogById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购价格变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购价格变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchasePriceChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_PRICE_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出采购价格变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchasePriceChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
