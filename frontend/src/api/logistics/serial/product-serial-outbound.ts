// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/serial
// 文件名称：product-serial-outbound.ts
// 创建时间：2026-06-06
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
  ProductSerialOutbound,
  ProductSerialOutboundCreate,
  ProductSerialOutboundUpdate
} from '@/types/logistics/serial/product-serial-outbound';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductSerialOutbounds
 */
const PRODUCT_SERIAL_OUTBOUND_API_BASE = 'TaktProductSerialOutbounds';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取产品序列号出库列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductSerialOutbound>>} 分页结果
 */
export function getProductSerialOutboundList(queryDto: any): Promise<TaktPagedResult<ProductSerialOutbound>> {
  return request<TaktPagedResult<ProductSerialOutbound>>({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取产品序列号出库
 * @param {string} id 产品序列号出库ID
 * @returns {Promise<ProductSerialOutbound>} 产品序列号出库DTO
 */
export function getProductSerialOutboundById(id: string): Promise<ProductSerialOutbound> {
  return request<ProductSerialOutbound>({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建产品序列号出库
 * @param {ProductSerialOutboundCreate} dto 创建DTO
 * @returns {Promise<ProductSerialOutbound>} 产品序列号出库DTO
 */
export function createProductSerialOutbound(dto: ProductSerialOutboundCreate): Promise<ProductSerialOutbound> {
  return request<ProductSerialOutbound>({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新产品序列号出库
 * @param {string} id 产品序列号出库ID
 * @param {ProductSerialOutboundUpdate} dto 更新DTO
 * @returns {Promise<ProductSerialOutbound>} 产品序列号出库DTO
 */
export function updateProductSerialOutbound(id: string, dto: ProductSerialOutboundUpdate): Promise<ProductSerialOutbound> {
  return request<ProductSerialOutbound>({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除产品序列号出库
 * @param {string} id 产品序列号出库ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductSerialOutboundById(id: string): Promise<void> {
  return request({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除产品序列号出库
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductSerialOutboundBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取产品序列号出库选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductSerialOutboundOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}/options`,
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
export function getProductSerialOutboundTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入产品序列号出库
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductSerialOutbound(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}/import`,
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
 * 导出产品序列号出库
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductSerialOutbound(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCT_SERIAL_OUTBOUND_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
