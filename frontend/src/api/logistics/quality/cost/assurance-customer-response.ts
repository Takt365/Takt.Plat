// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：assurance-customer-response.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块 API（自动生成，请勿手改路由常量）
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
  QualityAssuranceCustomerResponse,
  QualityAssuranceCustomerResponseCreate,
  QualityAssuranceCustomerResponseUpdate
} from '@/types/logistics/quality/cost/assurance-customer-response';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityAssuranceCustomerResponses
 */
const QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE = 'TaktQualityAssuranceCustomerResponses';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取品质业务顾客品质要求对应费用明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityAssuranceCustomerResponse>>} 分页结果
 */
export function getQualityAssuranceCustomerResponseList(queryDto: any): Promise<TaktPagedResult<QualityAssuranceCustomerResponse>> {
  return request<TaktPagedResult<QualityAssuranceCustomerResponse>>({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取品质业务顾客品质要求对应费用明细
 * @param {string} id 品质业务顾客品质要求对应费用明细ID
 * @returns {Promise<QualityAssuranceCustomerResponse>} 品质业务顾客品质要求对应费用明细DTO
 */
export function getQualityAssuranceCustomerResponseById(id: string): Promise<QualityAssuranceCustomerResponse> {
  return request<QualityAssuranceCustomerResponse>({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建品质业务顾客品质要求对应费用明细
 * @param {QualityAssuranceCustomerResponseCreate} dto 创建DTO
 * @returns {Promise<QualityAssuranceCustomerResponse>} 品质业务顾客品质要求对应费用明细DTO
 */
export function createQualityAssuranceCustomerResponse(dto: QualityAssuranceCustomerResponseCreate): Promise<QualityAssuranceCustomerResponse> {
  return request<QualityAssuranceCustomerResponse>({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新品质业务顾客品质要求对应费用明细
 * @param {string} id 品质业务顾客品质要求对应费用明细ID
 * @param {QualityAssuranceCustomerResponseUpdate} dto 更新DTO
 * @returns {Promise<QualityAssuranceCustomerResponse>} 品质业务顾客品质要求对应费用明细DTO
 */
export function updateQualityAssuranceCustomerResponse(id: string, dto: QualityAssuranceCustomerResponseUpdate): Promise<QualityAssuranceCustomerResponse> {
  return request<QualityAssuranceCustomerResponse>({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除品质业务顾客品质要求对应费用明细
 * @param {string} id 品质业务顾客品质要求对应费用明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityAssuranceCustomerResponseById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除品质业务顾客品质要求对应费用明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityAssuranceCustomerResponseBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取品质业务顾客品质要求对应费用明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityAssuranceCustomerResponseOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}/options`,
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
export function getQualityAssuranceCustomerResponseTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入品质业务顾客品质要求对应费用明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityAssuranceCustomerResponse(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}/import`,
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
 * 导出品质业务顾客品质要求对应费用明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityAssuranceCustomerResponse(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_ASSURANCE_CUSTOMER_RESPONSE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
