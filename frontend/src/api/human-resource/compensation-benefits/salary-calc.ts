// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/compensation-benefits
// 文件名称：salary-calc.ts
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
  SalaryCalc,
  SalaryCalcCreate,
  SalaryCalcStatus,
  SalaryCalcUpdate
} from '@/types/human-resource/compensation-benefits/salary-calc';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalaryCalcs
 */
const SALARY_CALC_API_BASE = 'TaktSalaryCalcs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取薪资核算列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalaryCalc>>} 分页结果
 */
export function getSalaryCalcList(queryDto: any): Promise<TaktPagedResult<SalaryCalc>> {
  return request<TaktPagedResult<SalaryCalc>>({
    url: `${SALARY_CALC_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取薪资核算
 * @param {string} id 薪资核算ID
 * @returns {Promise<SalaryCalc>} 薪资核算DTO
 */
export function getSalaryCalcById(id: string): Promise<SalaryCalc> {
  return request<SalaryCalc>({
    url: `${SALARY_CALC_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建薪资核算
 * @param {SalaryCalcCreate} dto 创建DTO
 * @returns {Promise<SalaryCalc>} 薪资核算DTO
 */
export function createSalaryCalc(dto: SalaryCalcCreate): Promise<SalaryCalc> {
  return request<SalaryCalc>({
    url: `${SALARY_CALC_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新薪资核算
 * @param {string} id 薪资核算ID
 * @param {SalaryCalcUpdate} dto 更新DTO
 * @returns {Promise<SalaryCalc>} 薪资核算DTO
 */
export function updateSalaryCalc(id: string, dto: SalaryCalcUpdate): Promise<SalaryCalc> {
  return request<SalaryCalc>({
    url: `${SALARY_CALC_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除薪资核算
 * @param {string} id 薪资核算ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalaryCalcById(id: string): Promise<void> {
  return request({
    url: `${SALARY_CALC_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除薪资核算
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalaryCalcBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALARY_CALC_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新薪资核算状态
 * @param {SalaryCalcStatus} dto 状态 DTO
 * @returns {Promise<SalaryCalc>} 薪资核算DTO
 */
export function updateSalaryCalcStatus(dto: SalaryCalcStatus): Promise<SalaryCalc> {
  return request<SalaryCalc>({
    url: `${SALARY_CALC_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取薪资核算选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalaryCalcOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALARY_CALC_API_BASE}/options`,
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
export function getSalaryCalcTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALARY_CALC_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入薪资核算
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalaryCalc(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALARY_CALC_API_BASE}/import`,
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
 * 导出薪资核算
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalaryCalc(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALARY_CALC_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
