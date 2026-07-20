// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchase-price-scale-quantity.ts
// 创建时间：2026-07-20
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
  PurchasePriceScaleQuantity,
  PurchasePriceScaleQuantityCreate,
  PurchasePriceScaleQuantityObsolete,
  PurchasePriceScaleQuantityUpdate
} from '@/types/logistics/procurement/purchase-price-scale-quantity';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchasePriceScaleQuantities
 */
const PURCHASE_PRICE_SCALE_QUANTITY_API_BASE = 'TaktPurchasePriceScaleQuantities';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购价格数量等级列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchasePriceScaleQuantity>>} 分页结果
 */
export function getPurchasePriceScaleQuantityList(queryDto: any): Promise<TaktPagedResult<PurchasePriceScaleQuantity>> {
  return request<TaktPagedResult<PurchasePriceScaleQuantity>>({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购价格数量等级
 * @param {string} id 采购价格数量等级ID
 * @returns {Promise<PurchasePriceScaleQuantity>} 采购价格数量等级DTO
 */
export function getPurchasePriceScaleQuantityById(id: string): Promise<PurchasePriceScaleQuantity> {
  return request<PurchasePriceScaleQuantity>({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购价格数量等级
 * @param {PurchasePriceScaleQuantityCreate} dto 创建DTO
 * @returns {Promise<PurchasePriceScaleQuantity>} 采购价格数量等级DTO
 */
export function createPurchasePriceScaleQuantity(dto: PurchasePriceScaleQuantityCreate): Promise<PurchasePriceScaleQuantity> {
  return request<PurchasePriceScaleQuantity>({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购价格数量等级
 * @param {string} id 采购价格数量等级ID
 * @param {PurchasePriceScaleQuantityUpdate} dto 更新DTO
 * @returns {Promise<PurchasePriceScaleQuantity>} 采购价格数量等级DTO
 */
export function updatePurchasePriceScaleQuantity(id: string, dto: PurchasePriceScaleQuantityUpdate): Promise<PurchasePriceScaleQuantity> {
  return request<PurchasePriceScaleQuantity>({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购价格数量等级
 * @param {string} id 采购价格数量等级ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceScaleQuantityById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购价格数量等级
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceScaleQuantityBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购价格数量等级作废状态
 * @param {PurchasePriceScaleQuantityObsolete} dto 作废 DTO
 * @returns {Promise<PurchasePriceScaleQuantity>} 采购价格数量等级DTO
 */
export function updatePurchasePriceScaleQuantityObsolete(dto: PurchasePriceScaleQuantityObsolete): Promise<PurchasePriceScaleQuantity> {
  return request<PurchasePriceScaleQuantity>({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购价格数量等级选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchasePriceScaleQuantityOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/options`,
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
export function getPurchasePriceScaleQuantityTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购价格数量等级
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchasePriceScaleQuantity(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/import`,
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
 * 导出采购价格数量等级
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchasePriceScaleQuantity(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_SCALE_QUANTITY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
