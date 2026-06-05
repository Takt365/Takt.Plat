// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：purchase-price-item.ts
// 创建时间：2026-06-05
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
  PurchasePriceItem,
  PurchasePriceItemCreate,
  PurchasePriceItemSort,
  PurchasePriceItemUpdate
} from '@/types/logistics/materials/purchase-price-item';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchasePriceItems
 */
const PURCHASE_PRICE_ITEM_API_BASE = 'TaktPurchasePriceItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购价格明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchasePriceItem>>} 分页结果
 */
export function getPurchasePriceItemList(queryDto: any): Promise<TaktPagedResult<PurchasePriceItem>> {
  return request<TaktPagedResult<PurchasePriceItem>>({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取采购价格明细
 * @param {string} id 采购价格明细ID
 * @returns {Promise<PurchasePriceItem>} 采购价格明细DTO
 */
export function getPurchasePriceItemById(id: string): Promise<PurchasePriceItem> {
  return request<PurchasePriceItem>({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购价格明细
 * @param {PurchasePriceItemCreate} dto 创建DTO
 * @returns {Promise<PurchasePriceItem>} 采购价格明细DTO
 */
export function createPurchasePriceItem(dto: PurchasePriceItemCreate): Promise<PurchasePriceItem> {
  return request<PurchasePriceItem>({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购价格明细
 * @param {string} id 采购价格明细ID
 * @param {PurchasePriceItemUpdate} dto 更新DTO
 * @returns {Promise<PurchasePriceItem>} 采购价格明细DTO
 */
export function updatePurchasePriceItem(id: string, dto: PurchasePriceItemUpdate): Promise<PurchasePriceItem> {
  return request<PurchasePriceItem>({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购价格明细
 * @param {string} id 采购价格明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceItemById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购价格明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购价格明细排序
 * @param {PurchasePriceItemSort} dto 排序DTO
 * @returns {Promise<PurchasePriceItem>} 采购价格明细DTO
 */
export function updatePurchasePriceItemSort(dto: PurchasePriceItemSort): Promise<PurchasePriceItem> {
  return request<PurchasePriceItem>({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购价格明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchasePriceItemOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/options`,
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
export function getPurchasePriceItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购价格明细
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchasePriceItem(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/import`,
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
 * 导出采购价格明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchasePriceItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
