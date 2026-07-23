// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchase-price-scale-value.ts
// 创建时间：2026-07-21
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
  PurchasePriceScaleValue,
  PurchasePriceScaleValueCreate,
  PurchasePriceScaleValueObsolete,
  PurchasePriceScaleValueUpdate
} from '@/types/logistics/procurement/purchase-price-scale-value';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchasePriceScaleValues
 */
const PURCHASE_PRICE_SCALE_VALUE_API_BASE = 'TaktPurchasePriceScaleValues';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购价格价值等级列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchasePriceScaleValue>>} 分页结果
 */
export function getPurchasePriceScaleValueList(queryDto: any): Promise<TaktPagedResult<PurchasePriceScaleValue>> {
  return request<TaktPagedResult<PurchasePriceScaleValue>>({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购价格价值等级
 * @param {string} id 采购价格价值等级ID
 * @returns {Promise<PurchasePriceScaleValue>} 采购价格价值等级DTO
 */
export function getPurchasePriceScaleValueById(id: string): Promise<PurchasePriceScaleValue> {
  return request<PurchasePriceScaleValue>({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购价格价值等级
 * @param {PurchasePriceScaleValueCreate} dto 创建DTO
 * @returns {Promise<PurchasePriceScaleValue>} 采购价格价值等级DTO
 */
export function createPurchasePriceScaleValue(dto: PurchasePriceScaleValueCreate): Promise<PurchasePriceScaleValue> {
  return request<PurchasePriceScaleValue>({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购价格价值等级
 * @param {string} id 采购价格价值等级ID
 * @param {PurchasePriceScaleValueUpdate} dto 更新DTO
 * @returns {Promise<PurchasePriceScaleValue>} 采购价格价值等级DTO
 */
export function updatePurchasePriceScaleValue(id: string, dto: PurchasePriceScaleValueUpdate): Promise<PurchasePriceScaleValue> {
  return request<PurchasePriceScaleValue>({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购价格价值等级
 * @param {string} id 采购价格价值等级ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceScaleValueById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购价格价值等级
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceScaleValueBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购价格价值等级作废状态
 * @param {PurchasePriceScaleValueObsolete} dto 作废 DTO
 * @returns {Promise<PurchasePriceScaleValue>} 采购价格价值等级DTO
 */
export function updatePurchasePriceScaleValueObsolete(dto: PurchasePriceScaleValueObsolete): Promise<PurchasePriceScaleValue> {
  return request<PurchasePriceScaleValue>({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购价格价值等级选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchasePriceScaleValueOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/options`,
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
export function getPurchasePriceScaleValueTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购价格价值等级
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchasePriceScaleValue(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/import`,
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
 * 导出采购价格价值等级
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchasePriceScaleValue(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_SCALE_VALUE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
