// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/planning
// 文件名称：sales-plan.ts
// 创建时间：2026-06-20
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
  SalesPlan,
  SalesPlanCreate,
  SalesPlanStatus,
  SalesPlanUpdate
} from '@/types/logistics/manufacturing/planning/sales-plan';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesPlans
 */
const SALES_PLAN_API_BASE = 'TaktSalesPlans';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售计划列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesPlan>>} 分页结果
 */
export function getSalesPlanList(queryDto: any): Promise<TaktPagedResult<SalesPlan>> {
  return request<TaktPagedResult<SalesPlan>>({
    url: `${SALES_PLAN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售计划
 * @param {string} id 销售计划ID
 * @returns {Promise<SalesPlan>} 销售计划DTO
 */
export function getSalesPlanById(id: string): Promise<SalesPlan> {
  return request<SalesPlan>({
    url: `${SALES_PLAN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售计划
 * @param {SalesPlanCreate} dto 创建DTO
 * @returns {Promise<SalesPlan>} 销售计划DTO
 */
export function createSalesPlan(dto: SalesPlanCreate): Promise<SalesPlan> {
  return request<SalesPlan>({
    url: `${SALES_PLAN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售计划
 * @param {string} id 销售计划ID
 * @param {SalesPlanUpdate} dto 更新DTO
 * @returns {Promise<SalesPlan>} 销售计划DTO
 */
export function updateSalesPlan(id: string, dto: SalesPlanUpdate): Promise<SalesPlan> {
  return request<SalesPlan>({
    url: `${SALES_PLAN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售计划
 * @param {string} id 销售计划ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPlanById(id: string): Promise<void> {
  return request({
    url: `${SALES_PLAN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售计划
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesPlanBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_PLAN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售计划状态
 * @param {SalesPlanStatus} dto 状态 DTO
 * @returns {Promise<SalesPlan>} 销售计划DTO
 */
export function updateSalesPlanStatus(dto: SalesPlanStatus): Promise<SalesPlan> {
  return request<SalesPlan>({
    url: `${SALES_PLAN_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售计划选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesPlanOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_PLAN_API_BASE}/options`,
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
export function getSalesPlanTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PLAN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售计划
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesPlan(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_PLAN_API_BASE}/import`,
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
 * 导出销售计划
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesPlan(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PLAN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
