// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：purchase-price-scale.ts
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
  PurchasePriceScale,
  PurchasePriceScaleCreate,
  PurchasePriceScaleSort,
  PurchasePriceScaleUpdate
} from '@/types/logistics/materials/purchase-price-scale';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchasePriceScales
 */
const PURCHASE_PRICE_SCALE_API_BASE = 'TaktPurchasePriceScales';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购价格阶梯列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchasePriceScale>>} 分页结果
 */
export function getPurchasePriceScaleList(queryDto: any): Promise<TaktPagedResult<PurchasePriceScale>> {
  return request<TaktPagedResult<PurchasePriceScale>>({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取采购价格阶梯
 * @param {string} id 采购价格阶梯ID
 * @returns {Promise<PurchasePriceScale>} 采购价格阶梯DTO
 */
export function getPurchasePriceScaleById(id: string): Promise<PurchasePriceScale> {
  return request<PurchasePriceScale>({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购价格阶梯
 * @param {PurchasePriceScaleCreate} dto 创建DTO
 * @returns {Promise<PurchasePriceScale>} 采购价格阶梯DTO
 */
export function createPurchasePriceScale(dto: PurchasePriceScaleCreate): Promise<PurchasePriceScale> {
  return request<PurchasePriceScale>({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购价格阶梯
 * @param {string} id 采购价格阶梯ID
 * @param {PurchasePriceScaleUpdate} dto 更新DTO
 * @returns {Promise<PurchasePriceScale>} 采购价格阶梯DTO
 */
export function updatePurchasePriceScale(id: string, dto: PurchasePriceScaleUpdate): Promise<PurchasePriceScale> {
  return request<PurchasePriceScale>({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购价格阶梯
 * @param {string} id 采购价格阶梯ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceScaleById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购价格阶梯
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePriceScaleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购价格阶梯排序
 * @param {PurchasePriceScaleSort} dto 排序DTO
 * @returns {Promise<PurchasePriceScale>} 采购价格阶梯DTO
 */
export function updatePurchasePriceScaleSort(dto: PurchasePriceScaleSort): Promise<PurchasePriceScale> {
  return request<PurchasePriceScale>({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购价格阶梯选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchasePriceScaleOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/options`,
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
export function getPurchasePriceScaleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购价格阶梯
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchasePriceScale(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/import`,
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
 * 导出采购价格阶梯
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchasePriceScale(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_SCALE_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
