// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/customer-service
// 文件名称：service-order.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块 API（自动生成，请勿手改路由常量）
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
  ServiceOrder,
  ServiceOrderCreate,
  ServiceOrderSort,
  ServiceOrderStatus,
  ServiceOrderUpdate
} from '@/types/logistics/customer-service/service-order';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktServiceOrders
 */
const SERVICE_ORDER_API_BASE = 'TaktServiceOrders';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取服务订单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ServiceOrder>>} 分页结果
 */
export function getServiceOrderList(queryDto: any): Promise<TaktPagedResult<ServiceOrder>> {
  return request<TaktPagedResult<ServiceOrder>>({
    url: `${SERVICE_ORDER_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取服务订单
 * @param {string} id 服务订单ID
 * @returns {Promise<ServiceOrder>} 服务订单DTO
 */
export function getServiceOrderById(id: string): Promise<ServiceOrder> {
  return request<ServiceOrder>({
    url: `${SERVICE_ORDER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建服务订单
 * @param {ServiceOrderCreate} dto 创建DTO
 * @returns {Promise<ServiceOrder>} 服务订单DTO
 */
export function createServiceOrder(dto: ServiceOrderCreate): Promise<ServiceOrder> {
  return request<ServiceOrder>({
    url: `${SERVICE_ORDER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新服务订单
 * @param {string} id 服务订单ID
 * @param {ServiceOrderUpdate} dto 更新DTO
 * @returns {Promise<ServiceOrder>} 服务订单DTO
 */
export function updateServiceOrder(id: string, dto: ServiceOrderUpdate): Promise<ServiceOrder> {
  return request<ServiceOrder>({
    url: `${SERVICE_ORDER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除服务订单
 * @param {string} id 服务订单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteServiceOrderById(id: string): Promise<void> {
  return request({
    url: `${SERVICE_ORDER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除服务订单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteServiceOrderBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SERVICE_ORDER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新服务订单状态
 * @param {ServiceOrderStatus} dto 状态DTO
 * @returns {Promise<ServiceOrder>} 服务订单DTO
 */
export function updateServiceOrderStatus(dto: ServiceOrderStatus): Promise<ServiceOrder> {
  return request<ServiceOrder>({
    url: `${SERVICE_ORDER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新服务订单排序
 * @param {ServiceOrderSort} dto 排序DTO
 * @returns {Promise<ServiceOrder>} 服务订单DTO
 */
export function updateServiceOrderSort(dto: ServiceOrderSort): Promise<ServiceOrder> {
  return request<ServiceOrder>({
    url: `${SERVICE_ORDER_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取服务订单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getServiceOrderOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SERVICE_ORDER_API_BASE}/options`,
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
export function getServiceOrderTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SERVICE_ORDER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入服务订单
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importServiceOrder(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SERVICE_ORDER_API_BASE}/import`,
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
 * 导出服务订单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportServiceOrder(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SERVICE_ORDER_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
