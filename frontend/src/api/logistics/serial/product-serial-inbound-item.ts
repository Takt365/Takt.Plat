// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/serial
// 文件名称：product-serial-inbound-item.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/serial 模块 API（自动生成，请勿手改路由常量）
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
  ProductSerialInboundItem,
  ProductSerialInboundItemCreate,
  ProductSerialInboundItemUpdate
} from '@/types/logistics/serial/product-serial-inbound-item';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductSerialInboundItems
 */
const PRODUCT_SERIAL_INBOUND_ITEM_API_BASE = 'TaktProductSerialInboundItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取产品序列号入库明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductSerialInboundItem>>} 分页结果
 */
export function getProductSerialInboundItemList(queryDto: any): Promise<TaktPagedResult<ProductSerialInboundItem>> {
  return request<TaktPagedResult<ProductSerialInboundItem>>({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取产品序列号入库明细
 * @param {string} id 产品序列号入库明细ID
 * @returns {Promise<ProductSerialInboundItem>} 产品序列号入库明细DTO
 */
export function getProductSerialInboundItemById(id: string): Promise<ProductSerialInboundItem> {
  return request<ProductSerialInboundItem>({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建产品序列号入库明细
 * @param {ProductSerialInboundItemCreate} dto 创建DTO
 * @returns {Promise<ProductSerialInboundItem>} 产品序列号入库明细DTO
 */
export function createProductSerialInboundItem(dto: ProductSerialInboundItemCreate): Promise<ProductSerialInboundItem> {
  return request<ProductSerialInboundItem>({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新产品序列号入库明细
 * @param {string} id 产品序列号入库明细ID
 * @param {ProductSerialInboundItemUpdate} dto 更新DTO
 * @returns {Promise<ProductSerialInboundItem>} 产品序列号入库明细DTO
 */
export function updateProductSerialInboundItem(id: string, dto: ProductSerialInboundItemUpdate): Promise<ProductSerialInboundItem> {
  return request<ProductSerialInboundItem>({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除产品序列号入库明细
 * @param {string} id 产品序列号入库明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductSerialInboundItemById(id: string): Promise<void> {
  return request({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除产品序列号入库明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductSerialInboundItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取产品序列号入库明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductSerialInboundItemOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}/options`,
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
export function getProductSerialInboundItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入产品序列号入库明细
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductSerialInboundItem(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}/import`,
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
 * 导出产品序列号入库明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductSerialInboundItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCT_SERIAL_INBOUND_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
