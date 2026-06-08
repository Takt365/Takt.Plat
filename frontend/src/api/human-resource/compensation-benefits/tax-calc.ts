// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/compensation-benefits
// 文件名称：tax-calc.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation-benefits 模块 API（自动生成，请勿手改路由常量）
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
  TaxCalc,
  TaxCalcCreate,
  TaxCalcStatus,
  TaxCalcUpdate
} from '@/types/human-resource/compensation-benefits/tax-calc';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTaxCalcs
 */
const TAX_CALC_API_BASE = 'TaktTaxCalcs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取个税计算规则列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TaxCalc>>} 分页结果
 */
export function getTaxCalcList(queryDto: any): Promise<TaktPagedResult<TaxCalc>> {
  return request<TaktPagedResult<TaxCalc>>({
    url: `${TAX_CALC_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取个税计算规则
 * @param {string} id 个税计算规则ID
 * @returns {Promise<TaxCalc>} 个税计算规则DTO
 */
export function getTaxCalcById(id: string): Promise<TaxCalc> {
  return request<TaxCalc>({
    url: `${TAX_CALC_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建个税计算规则
 * @param {TaxCalcCreate} dto 创建DTO
 * @returns {Promise<TaxCalc>} 个税计算规则DTO
 */
export function createTaxCalc(dto: TaxCalcCreate): Promise<TaxCalc> {
  return request<TaxCalc>({
    url: `${TAX_CALC_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新个税计算规则
 * @param {string} id 个税计算规则ID
 * @param {TaxCalcUpdate} dto 更新DTO
 * @returns {Promise<TaxCalc>} 个税计算规则DTO
 */
export function updateTaxCalc(id: string, dto: TaxCalcUpdate): Promise<TaxCalc> {
  return request<TaxCalc>({
    url: `${TAX_CALC_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除个税计算规则
 * @param {string} id 个税计算规则ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTaxCalcById(id: string): Promise<void> {
  return request({
    url: `${TAX_CALC_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除个税计算规则
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTaxCalcBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TAX_CALC_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新个税计算规则状态
 * @param {TaxCalcStatus} dto 状态 DTO
 * @returns {Promise<TaxCalc>} 个税计算规则DTO
 */
export function updateTaxCalcStatus(dto: TaxCalcStatus): Promise<TaxCalc> {
  return request<TaxCalc>({
    url: `${TAX_CALC_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取个税计算规则选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTaxCalcOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${TAX_CALC_API_BASE}/options`,
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
export function getTaxCalcTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TAX_CALC_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入个税计算规则
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTaxCalc(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TAX_CALC_API_BASE}/import`,
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
 * 导出个税计算规则
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTaxCalc(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TAX_CALC_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
