// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：packaging.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块 API（自动生成，请勿手改路由常量）
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
  Packaging,
  PackagingCreate,
  PackagingSort,
  PackagingUpdate
} from '@/types/logistics/manufacturing/bom/packaging';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPackagings
 */
const PACKAGING_API_BASE = 'TaktPackagings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料包装信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Packaging>>} 分页结果
 */
export function getPackagingList(queryDto: any): Promise<TaktPagedResult<Packaging>> {
  return request<TaktPagedResult<Packaging>>({
    url: `${PACKAGING_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取物料包装信息
 * @param {string} id 物料包装信息ID
 * @returns {Promise<Packaging>} 物料包装信息DTO
 */
export function getPackagingById(id: string): Promise<Packaging> {
  return request<Packaging>({
    url: `${PACKAGING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建物料包装信息
 * @param {PackagingCreate} dto 创建DTO
 * @returns {Promise<Packaging>} 物料包装信息DTO
 */
export function createPackaging(dto: PackagingCreate): Promise<Packaging> {
  return request<Packaging>({
    url: `${PACKAGING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料包装信息
 * @param {string} id 物料包装信息ID
 * @param {PackagingUpdate} dto 更新DTO
 * @returns {Promise<Packaging>} 物料包装信息DTO
 */
export function updatePackaging(id: string, dto: PackagingUpdate): Promise<Packaging> {
  return request<Packaging>({
    url: `${PACKAGING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料包装信息
 * @param {string} id 物料包装信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePackagingById(id: string): Promise<void> {
  return request({
    url: `${PACKAGING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料包装信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePackagingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PACKAGING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新物料包装信息排序
 * @param {PackagingSort} dto 排序DTO
 * @returns {Promise<Packaging>} 物料包装信息DTO
 */
export function updatePackagingSort(dto: PackagingSort): Promise<Packaging> {
  return request<Packaging>({
    url: `${PACKAGING_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料包装信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPackagingOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PACKAGING_API_BASE}/options`,
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
export function getPackagingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PACKAGING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料包装信息
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPackaging(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PACKAGING_API_BASE}/import`,
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
 * 导出物料包装信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPackaging(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PACKAGING_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
