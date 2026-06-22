// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：price-scale.ts
// 创建时间：2026-06-09
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
  SalesPriceScale,
  SalesPriceScaleCreate,
  SalesPriceScaleUpdate
} from '@/types/logistics/sales/price-scale';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesPriceScales
 */
const SALES_PRICE_SCALE_API_BASE = 'TaktSalesPriceScales';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售价格阶梯列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesPriceScale>>} 分页结果
 */
export function getSalesPriceScaleList(queryDto: any): Promise<TaktPagedResult<SalesPriceScale>> {
  return request<TaktPagedResult<SalesPriceScale>>({
    url: `${SALES_PRICE_SCALE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售价格阶梯
 * @param {string} id 销售价格阶梯ID
 * @returns {Promise<SalesPriceScale>} 销售价格阶梯DTO
 */
export function getSalesPriceScaleById(id: string): Promise<SalesPriceScale> {
  return request<SalesPriceScale>({
    url: `${SALES_PRICE_SCALE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售价格阶梯
 * @param {SalesPriceScaleCreate} dto 创建DTO
 * @returns {Promise<SalesPriceScale>} 销售价格阶梯DTO
 */
export function createSalesPriceScale(dto: SalesPriceScaleCreate): Promise<SalesPriceScale> {
  return request<SalesPriceScale>({
    url: `${SALES_PRICE_SCALE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售价格阶梯
 * @param {string} id 销售价格阶梯ID
 * @param {SalesPriceScaleUpdate} dto 更新DTO
 * @returns {Promise<SalesPriceScale>} 销售价格阶梯DTO
 */
export function updateSalesPriceScale(id: string, dto: SalesPriceScaleUpdate): Promise<SalesPriceScale> {
  return request<SalesPriceScale>({
    url: `${SALES_PRICE_SCALE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售价格阶梯
 * @param {string} id 销售价格阶梯ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceScaleById(id: string): Promise<void> {
  return request({
    url: `${SALES_PRICE_SCALE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售价格阶梯
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceScaleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_PRICE_SCALE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售价格阶梯选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesPriceScaleOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_PRICE_SCALE_API_BASE}/options`,
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
export function getSalesPriceScaleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_SCALE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售价格阶梯
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesPriceScale(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_PRICE_SCALE_API_BASE}/import`,
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
 * 导出销售价格阶梯
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesPriceScale(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_SCALE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
