// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/planning
// 文件名称：sales-plan-item.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/planning 模块 API（自动生成，请勿手改路由常量）
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
  SalesPlanItem,
  SalesPlanItemCreate,
  SalesPlanItemUpdate
} from '@/types/logistics/manufacturing/planning/sales-plan-item';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesPlanItems
 */
const SALES_PLAN_ITEM_API_BASE = 'TaktSalesPlanItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售计划明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesPlanItem>>} 分页结果
 */
export function getSalesPlanItemList(queryDto: any): Promise<TaktPagedResult<SalesPlanItem>> {
  return request<TaktPagedResult<SalesPlanItem>>({
    url: `${SALES_PLAN_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售计划明细
 * @param {string} id 销售计划明细ID
 * @returns {Promise<SalesPlanItem>} 销售计划明细DTO
 */
export function getSalesPlanItemById(id: string): Promise<SalesPlanItem> {
  return request<SalesPlanItem>({
    url: `${SALES_PLAN_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售计划明细
 * @param {SalesPlanItemCreate} dto 创建DTO
 * @returns {Promise<SalesPlanItem>} 销售计划明细DTO
 */
export function createSalesPlanItem(dto: SalesPlanItemCreate): Promise<SalesPlanItem> {
  return request<SalesPlanItem>({
    url: `${SALES_PLAN_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售计划明细
 * @param {string} id 销售计划明细ID
 * @param {SalesPlanItemUpdate} dto 更新DTO
 * @returns {Promise<SalesPlanItem>} 销售计划明细DTO
 */
export function updateSalesPlanItem(id: string, dto: SalesPlanItemUpdate): Promise<SalesPlanItem> {
  return request<SalesPlanItem>({
    url: `${SALES_PLAN_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售计划明细
 * @param {string} id 销售计划明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPlanItemById(id: string): Promise<void> {
  return request({
    url: `${SALES_PLAN_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售计划明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPlanItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_PLAN_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售计划明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesPlanItemOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_PLAN_ITEM_API_BASE}/options`,
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
export function getSalesPlanItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PLAN_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售计划明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesPlanItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_PLAN_ITEM_API_BASE}/import`,
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
 * 导出销售计划明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesPlanItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PLAN_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
