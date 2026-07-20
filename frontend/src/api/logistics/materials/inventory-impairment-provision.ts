// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：inventory-impairment-provision.ts
// 创建时间：2026-07-18
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
  InventoryImpairmentProvision,
  InventoryImpairmentProvisionCreate,
  InventoryImpairmentProvisionSort,
  InventoryImpairmentProvisionStatus,
  InventoryImpairmentProvisionUpdate
} from '@/types/logistics/materials/inventory-impairment-provision';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktInventoryImpairmentProvisions
 */
const INVENTORY_IMPAIRMENT_PROVISION_API_BASE = 'TaktInventoryImpairmentProvisions';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取存货跌价准备列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<InventoryImpairmentProvision>>} 分页结果
 */
export function getInventoryImpairmentProvisionList(queryDto: any): Promise<TaktPagedResult<InventoryImpairmentProvision>> {
  return request<TaktPagedResult<InventoryImpairmentProvision>>({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取存货跌价准备
 * @param {string} id 存货跌价准备ID
 * @returns {Promise<InventoryImpairmentProvision>} 存货跌价准备DTO
 */
export function getInventoryImpairmentProvisionById(id: string): Promise<InventoryImpairmentProvision> {
  return request<InventoryImpairmentProvision>({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建存货跌价准备
 * @param {InventoryImpairmentProvisionCreate} dto 创建DTO
 * @returns {Promise<InventoryImpairmentProvision>} 存货跌价准备DTO
 */
export function createInventoryImpairmentProvision(dto: InventoryImpairmentProvisionCreate): Promise<InventoryImpairmentProvision> {
  return request<InventoryImpairmentProvision>({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新存货跌价准备
 * @param {string} id 存货跌价准备ID
 * @param {InventoryImpairmentProvisionUpdate} dto 更新DTO
 * @returns {Promise<InventoryImpairmentProvision>} 存货跌价准备DTO
 */
export function updateInventoryImpairmentProvision(id: string, dto: InventoryImpairmentProvisionUpdate): Promise<InventoryImpairmentProvision> {
  return request<InventoryImpairmentProvision>({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除存货跌价准备
 * @param {string} id 存货跌价准备ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteInventoryImpairmentProvisionById(id: string): Promise<void> {
  return request({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除存货跌价准备
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteInventoryImpairmentProvisionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新存货跌价准备状态
 * @param {InventoryImpairmentProvisionStatus} dto 状态 DTO
 * @returns {Promise<InventoryImpairmentProvision>} 存货跌价准备DTO
 */
export function updateInventoryImpairmentProvisionStatus(dto: InventoryImpairmentProvisionStatus): Promise<InventoryImpairmentProvision> {
  return request<InventoryImpairmentProvision>({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新存货跌价准备排序
 * @param {InventoryImpairmentProvisionSort} dto 排序DTO
 * @returns {Promise<InventoryImpairmentProvision>} 存货跌价准备DTO
 */
export function updateInventoryImpairmentProvisionSort(dto: InventoryImpairmentProvisionSort): Promise<InventoryImpairmentProvision> {
  return request<InventoryImpairmentProvision>({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取存货跌价准备选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getInventoryImpairmentProvisionOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/options`,
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
export function getInventoryImpairmentProvisionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入存货跌价准备
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importInventoryImpairmentProvision(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/import`,
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
 * 导出存货跌价准备
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportInventoryImpairmentProvision(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${INVENTORY_IMPAIRMENT_PROVISION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
