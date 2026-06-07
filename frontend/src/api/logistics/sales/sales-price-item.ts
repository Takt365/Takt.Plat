// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：sales-price-item.ts
// 创建时间：2026-06-07
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
  SalesPriceItem,
  SalesPriceItemCreate,
  SalesPriceItemUpdate
} from '@/types/logistics/sales/sales-price-item';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesPriceItems
 */
const SALES_PRICE_ITEM_API_BASE = 'TaktSalesPriceItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售价格明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesPriceItem>>} 分页结果
 */
export function getSalesPriceItemList(queryDto: any): Promise<TaktPagedResult<SalesPriceItem>> {
  return request<TaktPagedResult<SalesPriceItem>>({
    url: `${SALES_PRICE_ITEM_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取销售价格明细
 * @param {string} id 销售价格明细ID
 * @returns {Promise<SalesPriceItem>} 销售价格明细DTO
 */
export function getSalesPriceItemById(id: string): Promise<SalesPriceItem> {
  return request<SalesPriceItem>({
    url: `${SALES_PRICE_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售价格明细
 * @param {SalesPriceItemCreate} dto 创建DTO
 * @returns {Promise<SalesPriceItem>} 销售价格明细DTO
 */
export function createSalesPriceItem(dto: SalesPriceItemCreate): Promise<SalesPriceItem> {
  return request<SalesPriceItem>({
    url: `${SALES_PRICE_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售价格明细
 * @param {string} id 销售价格明细ID
 * @param {SalesPriceItemUpdate} dto 更新DTO
 * @returns {Promise<SalesPriceItem>} 销售价格明细DTO
 */
export function updateSalesPriceItem(id: string, dto: SalesPriceItemUpdate): Promise<SalesPriceItem> {
  return request<SalesPriceItem>({
    url: `${SALES_PRICE_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售价格明细
 * @param {string} id 销售价格明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceItemById(id: string): Promise<void> {
  return request({
    url: `${SALES_PRICE_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售价格明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPriceItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_PRICE_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售价格明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesPriceItemOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_PRICE_ITEM_API_BASE}/options`,
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
export function getSalesPriceItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售价格明细
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesPriceItem(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_PRICE_ITEM_API_BASE}/import`,
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
 * 导出销售价格明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesPriceItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
