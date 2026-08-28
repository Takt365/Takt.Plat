// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchase-request-item.ts
// 创建时间：2026-08-28
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
  PurchaseRequestItem,
  PurchaseRequestItemCreate,
  PurchaseRequestItemObsolete,
  PurchaseRequestItemUpdate
} from '@/types/logistics/procurement/purchase-request-item';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseRequestItems
 */
const PURCHASE_REQUEST_ITEM_API_BASE = 'TaktPurchaseRequestItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购申请明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseRequestItem>>} 分页结果
 */
export function getPurchaseRequestItemList(queryDto: any): Promise<TaktPagedResult<PurchaseRequestItem>> {
  return request<TaktPagedResult<PurchaseRequestItem>>({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购申请明细
 * @param {string} id 采购申请明细ID
 * @returns {Promise<PurchaseRequestItem>} 采购申请明细DTO
 */
export function getPurchaseRequestItemById(id: string): Promise<PurchaseRequestItem> {
  return request<PurchaseRequestItem>({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购申请明细
 * @param {PurchaseRequestItemCreate} dto 创建DTO
 * @returns {Promise<PurchaseRequestItem>} 采购申请明细DTO
 */
export function createPurchaseRequestItem(dto: PurchaseRequestItemCreate): Promise<PurchaseRequestItem> {
  return request<PurchaseRequestItem>({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购申请明细
 * @param {string} id 采购申请明细ID
 * @param {PurchaseRequestItemUpdate} dto 更新DTO
 * @returns {Promise<PurchaseRequestItem>} 采购申请明细DTO
 */
export function updatePurchaseRequestItem(id: string, dto: PurchaseRequestItemUpdate): Promise<PurchaseRequestItem> {
  return request<PurchaseRequestItem>({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购申请明细
 * @param {string} id 采购申请明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseRequestItemById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购申请明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseRequestItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购申请明细作废状态
 * @param {PurchaseRequestItemObsolete} dto 作废 DTO
 * @returns {Promise<PurchaseRequestItem>} 采购申请明细DTO
 */
export function updatePurchaseRequestItemObsolete(dto: PurchaseRequestItemObsolete): Promise<PurchaseRequestItem> {
  return request<PurchaseRequestItem>({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购申请明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseRequestItemOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName sheetName
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getPurchaseRequestItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购申请明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseRequestItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/import`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    params: {
      sheetName
    },
  });
}

/**
 * 导出采购申请明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseRequestItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_REQUEST_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
