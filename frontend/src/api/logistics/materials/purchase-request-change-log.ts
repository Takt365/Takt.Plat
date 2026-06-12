// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：purchase-request-change-log.ts
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
  PurchaseRequestChangeLog,
  PurchaseRequestChangeLogCreate,
  PurchaseRequestChangeLogUpdate
} from '@/types/logistics/materials/purchase-request-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseRequestChangeLogs
 */
const PURCHASE_REQUEST_CHANGE_LOG_API_BASE = 'TaktPurchaseRequestChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购申请变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseRequestChangeLog>>} 分页结果
 */
export function getPurchaseRequestChangeLogList(queryDto: any): Promise<TaktPagedResult<PurchaseRequestChangeLog>> {
  return request<TaktPagedResult<PurchaseRequestChangeLog>>({
    url: `${PURCHASE_REQUEST_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取采购申请变更记录
 * @param {string} id 采购申请变更记录ID
 * @returns {Promise<PurchaseRequestChangeLog>} 采购申请变更记录DTO
 */
export function getPurchaseRequestChangeLogById(id: string): Promise<PurchaseRequestChangeLog> {
  return request<PurchaseRequestChangeLog>({
    url: `${PURCHASE_REQUEST_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购申请变更记录
 * @param {PurchaseRequestChangeLogCreate} dto 创建DTO
 * @returns {Promise<PurchaseRequestChangeLog>} 采购申请变更记录DTO
 */
export function createPurchaseRequestChangeLog(dto: PurchaseRequestChangeLogCreate): Promise<PurchaseRequestChangeLog> {
  return request<PurchaseRequestChangeLog>({
    url: `${PURCHASE_REQUEST_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购申请变更记录
 * @param {string} id 采购申请变更记录ID
 * @param {PurchaseRequestChangeLogUpdate} dto 更新DTO
 * @returns {Promise<PurchaseRequestChangeLog>} 采购申请变更记录DTO
 */
export function updatePurchaseRequestChangeLog(id: string, dto: PurchaseRequestChangeLogUpdate): Promise<PurchaseRequestChangeLog> {
  return request<PurchaseRequestChangeLog>({
    url: `${PURCHASE_REQUEST_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购申请变更记录
 * @param {string} id 采购申请变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseRequestChangeLogById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_REQUEST_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购申请变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseRequestChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_REQUEST_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购申请变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseRequestChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_REQUEST_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出采购申请变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseRequestChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_REQUEST_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
