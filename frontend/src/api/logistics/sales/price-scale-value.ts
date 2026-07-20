// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：price-scale-value.ts
// 创建时间：2026-07-20
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
  SalesPriceScaleValue,
  SalesPriceScaleValueCreate,
  SalesPriceScaleValueObsolete,
  SalesPriceScaleValueUpdate
} from '@/types/logistics/sales/price-scale-value';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesPriceScaleValues
 */
const SALES_PRICE_SCALE_VALUE_API_BASE = 'TaktSalesPriceScaleValues';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售价格价值等级列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesPriceScaleValue>>} 分页结果
 */
export function getSalesPriceScaleValueList(queryDto: any): Promise<TaktPagedResult<SalesPriceScaleValue>> {
  return request<TaktPagedResult<SalesPriceScaleValue>>({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售价格价值等级
 * @param {string} id 销售价格价值等级ID
 * @returns {Promise<SalesPriceScaleValue>} 销售价格价值等级DTO
 */
export function getSalesPriceScaleValueById(id: string): Promise<SalesPriceScaleValue> {
  return request<SalesPriceScaleValue>({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售价格价值等级
 * @param {SalesPriceScaleValueCreate} dto 创建DTO
 * @returns {Promise<SalesPriceScaleValue>} 销售价格价值等级DTO
 */
export function createSalesPriceScaleValue(dto: SalesPriceScaleValueCreate): Promise<SalesPriceScaleValue> {
  return request<SalesPriceScaleValue>({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售价格价值等级
 * @param {string} id 销售价格价值等级ID
 * @param {SalesPriceScaleValueUpdate} dto 更新DTO
 * @returns {Promise<SalesPriceScaleValue>} 销售价格价值等级DTO
 */
export function updateSalesPriceScaleValue(id: string, dto: SalesPriceScaleValueUpdate): Promise<SalesPriceScaleValue> {
  return request<SalesPriceScaleValue>({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售价格价值等级
 * @param {string} id 销售价格价值等级ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceScaleValueById(id: string): Promise<void> {
  return request({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售价格价值等级
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceScaleValueBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售价格价值等级作废状态
 * @param {SalesPriceScaleValueObsolete} dto 作废 DTO
 * @returns {Promise<SalesPriceScaleValue>} 销售价格价值等级DTO
 */
export function updateSalesPriceScaleValueObsolete(dto: SalesPriceScaleValueObsolete): Promise<SalesPriceScaleValue> {
  return request<SalesPriceScaleValue>({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售价格价值等级选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesPriceScaleValueOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/options`,
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
export function getSalesPriceScaleValueTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售价格价值等级
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesPriceScaleValue(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/import`,
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
 * 导出销售价格价值等级
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesPriceScaleValue(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_SCALE_VALUE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
