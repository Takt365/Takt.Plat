// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：production-order.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块 API（自动生成，请勿手改路由常量）
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
  ProductionOrder,
  ProductionOrderCreate,
  ProductionOrderStatus,
  ProductionOrderUpdate
} from '@/types/logistics/manufacturing/output/production-order';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductionOrders
 */
const PRODUCTION_ORDER_API_BASE = 'TaktProductionOrders';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取生产工单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductionOrder>>} 分页结果
 */
export function getProductionOrderList(queryDto: any): Promise<TaktPagedResult<ProductionOrder>> {
  return request<TaktPagedResult<ProductionOrder>>({
    url: `${PRODUCTION_ORDER_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取生产工单
 * @param {string} id 生产工单ID
 * @returns {Promise<ProductionOrder>} 生产工单DTO
 */
export function getProductionOrderById(id: string): Promise<ProductionOrder> {
  return request<ProductionOrder>({
    url: `${PRODUCTION_ORDER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建生产工单
 * @param {ProductionOrderCreate} dto 创建DTO
 * @returns {Promise<ProductionOrder>} 生产工单DTO
 */
export function createProductionOrder(dto: ProductionOrderCreate): Promise<ProductionOrder> {
  return request<ProductionOrder>({
    url: `${PRODUCTION_ORDER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新生产工单
 * @param {string} id 生产工单ID
 * @param {ProductionOrderUpdate} dto 更新DTO
 * @returns {Promise<ProductionOrder>} 生产工单DTO
 */
export function updateProductionOrder(id: string, dto: ProductionOrderUpdate): Promise<ProductionOrder> {
  return request<ProductionOrder>({
    url: `${PRODUCTION_ORDER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除生产工单
 * @param {string} id 生产工单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionOrderById(id: string): Promise<void> {
  return request({
    url: `${PRODUCTION_ORDER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除生产工单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionOrderBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCTION_ORDER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新生产工单状态
 * @param {ProductionOrderStatus} dto 状态DTO
 * @returns {Promise<ProductionOrder>} 生产工单DTO
 */
export function updateProductionOrderStatus(dto: ProductionOrderStatus): Promise<ProductionOrder> {
  return request<ProductionOrder>({
    url: `${PRODUCTION_ORDER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取生产工单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductionOrderOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PRODUCTION_ORDER_API_BASE}/options`,
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
export function getProductionOrderTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_ORDER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入生产工单
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductionOrder(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCTION_ORDER_API_BASE}/import`,
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
 * 导出生产工单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductionOrder(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_ORDER_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
