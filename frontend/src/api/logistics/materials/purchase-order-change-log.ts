// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：purchase-order-change-log.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块 API（自动生成，请勿手改路由常量）
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
  PurchaseOrderChangeLog,
  PurchaseOrderChangeLogCreate,
  PurchaseOrderChangeLogUpdate
} from '@/types/logistics/materials/purchase-order-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseOrderChangeLogs
 */
const PURCHASE_ORDER_CHANGE_LOG_API_BASE = 'TaktPurchaseOrderChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购订单变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseOrderChangeLog>>} 分页结果
 */
export function getPurchaseOrderChangeLogList(queryDto: any): Promise<TaktPagedResult<PurchaseOrderChangeLog>> {
  return request<TaktPagedResult<PurchaseOrderChangeLog>>({
    url: `${PURCHASE_ORDER_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取采购订单变更记录
 * @param {string} id 采购订单变更记录ID
 * @returns {Promise<PurchaseOrderChangeLog>} 采购订单变更记录DTO
 */
export function getPurchaseOrderChangeLogById(id: string): Promise<PurchaseOrderChangeLog> {
  return request<PurchaseOrderChangeLog>({
    url: `${PURCHASE_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购订单变更记录
 * @param {PurchaseOrderChangeLogCreate} dto 创建DTO
 * @returns {Promise<PurchaseOrderChangeLog>} 采购订单变更记录DTO
 */
export function createPurchaseOrderChangeLog(dto: PurchaseOrderChangeLogCreate): Promise<PurchaseOrderChangeLog> {
  return request<PurchaseOrderChangeLog>({
    url: `${PURCHASE_ORDER_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购订单变更记录
 * @param {string} id 采购订单变更记录ID
 * @param {PurchaseOrderChangeLogUpdate} dto 更新DTO
 * @returns {Promise<PurchaseOrderChangeLog>} 采购订单变更记录DTO
 */
export function updatePurchaseOrderChangeLog(id: string, dto: PurchaseOrderChangeLogUpdate): Promise<PurchaseOrderChangeLog> {
  return request<PurchaseOrderChangeLog>({
    url: `${PURCHASE_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购订单变更记录
 * @param {string} id 采购订单变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseOrderChangeLogById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购订单变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseOrderChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_ORDER_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购订单变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseOrderChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_ORDER_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出采购订单变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseOrderChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_ORDER_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
