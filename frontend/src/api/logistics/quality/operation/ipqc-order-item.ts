// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：ipqc-order-item.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块 API（自动生成，请勿手改路由常量）
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
  IpqcOrderItem,
  IpqcOrderItemCreate,
  IpqcOrderItemStatus,
  IpqcOrderItemUpdate
} from '@/types/logistics/quality/operation/ipqc-order-item';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktIpqcOrderItems
 */
const IPQC_ORDER_ITEM_API_BASE = 'TaktIpqcOrderItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取制程检验单明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<IpqcOrderItem>>} 分页结果
 */
export function getIpqcOrderItemList(queryDto: any): Promise<TaktPagedResult<IpqcOrderItem>> {
  return request<TaktPagedResult<IpqcOrderItem>>({
    url: `${IPQC_ORDER_ITEM_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取制程检验单明细
 * @param {string} id 制程检验单明细ID
 * @returns {Promise<IpqcOrderItem>} 制程检验单明细DTO
 */
export function getIpqcOrderItemById(id: string): Promise<IpqcOrderItem> {
  return request<IpqcOrderItem>({
    url: `${IPQC_ORDER_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建制程检验单明细
 * @param {IpqcOrderItemCreate} dto 创建DTO
 * @returns {Promise<IpqcOrderItem>} 制程检验单明细DTO
 */
export function createIpqcOrderItem(dto: IpqcOrderItemCreate): Promise<IpqcOrderItem> {
  return request<IpqcOrderItem>({
    url: `${IPQC_ORDER_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新制程检验单明细
 * @param {string} id 制程检验单明细ID
 * @param {IpqcOrderItemUpdate} dto 更新DTO
 * @returns {Promise<IpqcOrderItem>} 制程检验单明细DTO
 */
export function updateIpqcOrderItem(id: string, dto: IpqcOrderItemUpdate): Promise<IpqcOrderItem> {
  return request<IpqcOrderItem>({
    url: `${IPQC_ORDER_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除制程检验单明细
 * @param {string} id 制程检验单明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteIpqcOrderItemById(id: string): Promise<void> {
  return request({
    url: `${IPQC_ORDER_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除制程检验单明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteIpqcOrderItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${IPQC_ORDER_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新制程检验单明细状态
 * @param {IpqcOrderItemStatus} dto 状态DTO
 * @returns {Promise<IpqcOrderItem>} 制程检验单明细DTO
 */
export function updateIpqcOrderItemStatus(dto: IpqcOrderItemStatus): Promise<IpqcOrderItem> {
  return request<IpqcOrderItem>({
    url: `${IPQC_ORDER_ITEM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取制程检验单明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getIpqcOrderItemOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${IPQC_ORDER_ITEM_API_BASE}/options`,
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
export function getIpqcOrderItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${IPQC_ORDER_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入制程检验单明细
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importIpqcOrderItem(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${IPQC_ORDER_ITEM_API_BASE}/import`,
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
 * 导出制程检验单明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportIpqcOrderItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${IPQC_ORDER_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
