// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：price-scale-quantity.ts
// 创建时间：2026-07-23
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
  SalesPriceScaleQuantity,
  SalesPriceScaleQuantityCreate,
  SalesPriceScaleQuantityObsolete,
  SalesPriceScaleQuantityUpdate
} from '@/types/logistics/sales/price-scale-quantity';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesPriceScaleQuantities
 */
const SALES_PRICE_SCALE_QUANTITY_API_BASE = 'TaktSalesPriceScaleQuantities';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售价格数量等级列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesPriceScaleQuantity>>} 分页结果
 */
export function getSalesPriceScaleQuantityList(queryDto: any): Promise<TaktPagedResult<SalesPriceScaleQuantity>> {
  return request<TaktPagedResult<SalesPriceScaleQuantity>>({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售价格数量等级
 * @param {string} id 销售价格数量等级ID
 * @returns {Promise<SalesPriceScaleQuantity>} 销售价格数量等级DTO
 */
export function getSalesPriceScaleQuantityById(id: string): Promise<SalesPriceScaleQuantity> {
  return request<SalesPriceScaleQuantity>({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售价格数量等级
 * @param {SalesPriceScaleQuantityCreate} dto 创建DTO
 * @returns {Promise<SalesPriceScaleQuantity>} 销售价格数量等级DTO
 */
export function createSalesPriceScaleQuantity(dto: SalesPriceScaleQuantityCreate): Promise<SalesPriceScaleQuantity> {
  return request<SalesPriceScaleQuantity>({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售价格数量等级
 * @param {string} id 销售价格数量等级ID
 * @param {SalesPriceScaleQuantityUpdate} dto 更新DTO
 * @returns {Promise<SalesPriceScaleQuantity>} 销售价格数量等级DTO
 */
export function updateSalesPriceScaleQuantity(id: string, dto: SalesPriceScaleQuantityUpdate): Promise<SalesPriceScaleQuantity> {
  return request<SalesPriceScaleQuantity>({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售价格数量等级
 * @param {string} id 销售价格数量等级ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceScaleQuantityById(id: string): Promise<void> {
  return request({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售价格数量等级
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceScaleQuantityBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售价格数量等级作废状态
 * @param {SalesPriceScaleQuantityObsolete} dto 作废 DTO
 * @returns {Promise<SalesPriceScaleQuantity>} 销售价格数量等级DTO
 */
export function updateSalesPriceScaleQuantityObsolete(dto: SalesPriceScaleQuantityObsolete): Promise<SalesPriceScaleQuantity> {
  return request<SalesPriceScaleQuantity>({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售价格数量等级选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesPriceScaleQuantityOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/options`,
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
export function getSalesPriceScaleQuantityTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售价格数量等级
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesPriceScaleQuantity(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/import`,
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
 * 导出销售价格数量等级
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesPriceScaleQuantity(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_SCALE_QUANTITY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
