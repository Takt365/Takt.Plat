// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：material-transaction-item.ts
// 创建时间：2026-06-20
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
  MaterialTransactionItem,
  MaterialTransactionItemCreate,
  MaterialTransactionItemUpdate
} from '@/types/logistics/materials/material-transaction-item';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialTransactionItems
 */
const MATERIAL_TRANSACTION_ITEM_API_BASE = 'TaktMaterialTransactionItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料交易明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialTransactionItem>>} 分页结果
 */
export function getMaterialTransactionItemList(queryDto: any): Promise<TaktPagedResult<MaterialTransactionItem>> {
  return request<TaktPagedResult<MaterialTransactionItem>>({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取物料交易明细
 * @param {string} id 物料交易明细ID
 * @returns {Promise<MaterialTransactionItem>} 物料交易明细DTO
 */
export function getMaterialTransactionItemById(id: string): Promise<MaterialTransactionItem> {
  return request<MaterialTransactionItem>({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建物料交易明细
 * @param {MaterialTransactionItemCreate} dto 创建DTO
 * @returns {Promise<MaterialTransactionItem>} 物料交易明细DTO
 */
export function createMaterialTransactionItem(dto: MaterialTransactionItemCreate): Promise<MaterialTransactionItem> {
  return request<MaterialTransactionItem>({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料交易明细
 * @param {string} id 物料交易明细ID
 * @param {MaterialTransactionItemUpdate} dto 更新DTO
 * @returns {Promise<MaterialTransactionItem>} 物料交易明细DTO
 */
export function updateMaterialTransactionItem(id: string, dto: MaterialTransactionItemUpdate): Promise<MaterialTransactionItem> {
  return request<MaterialTransactionItem>({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料交易明细
 * @param {string} id 物料交易明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialTransactionItemById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料交易明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialTransactionItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料交易明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialTransactionItemOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}/options`,
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
export function getMaterialTransactionItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料交易明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaterialTransactionItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}/import`,
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
 * 导出物料交易明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialTransactionItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_TRANSACTION_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
