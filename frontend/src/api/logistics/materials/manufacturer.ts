// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：manufacturer.ts
// 创建时间：2026-06-08
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
  Manufacturer,
  ManufacturerCreate,
  ManufacturerSort,
  ManufacturerStatus,
  ManufacturerUpdate
} from '@/types/logistics/materials/manufacturer';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktManufacturers
 */
const MANUFACTURER_API_BASE = 'TaktManufacturers';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取制造商信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Manufacturer>>} 分页结果
 */
export function getManufacturerList(queryDto: any): Promise<TaktPagedResult<Manufacturer>> {
  return request<TaktPagedResult<Manufacturer>>({
    url: `${MANUFACTURER_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取制造商信息
 * @param {string} id 制造商信息ID
 * @returns {Promise<Manufacturer>} 制造商信息DTO
 */
export function getManufacturerById(id: string): Promise<Manufacturer> {
  return request<Manufacturer>({
    url: `${MANUFACTURER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建制造商信息
 * @param {ManufacturerCreate} dto 创建DTO
 * @returns {Promise<Manufacturer>} 制造商信息DTO
 */
export function createManufacturer(dto: ManufacturerCreate): Promise<Manufacturer> {
  return request<Manufacturer>({
    url: `${MANUFACTURER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新制造商信息
 * @param {string} id 制造商信息ID
 * @param {ManufacturerUpdate} dto 更新DTO
 * @returns {Promise<Manufacturer>} 制造商信息DTO
 */
export function updateManufacturer(id: string, dto: ManufacturerUpdate): Promise<Manufacturer> {
  return request<Manufacturer>({
    url: `${MANUFACTURER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除制造商信息
 * @param {string} id 制造商信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteManufacturerById(id: string): Promise<void> {
  return request({
    url: `${MANUFACTURER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除制造商信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteManufacturerBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MANUFACTURER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新制造商信息状态
 * @param {ManufacturerStatus} dto 状态 DTO（TaktCommonStatus 枚举）
 * @returns {Promise<Manufacturer>} 制造商信息DTO
 */
export function updateManufacturerStatus(dto: ManufacturerStatus): Promise<Manufacturer> {
  return request<Manufacturer>({
    url: `${MANUFACTURER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新制造商信息排序
 * @param {ManufacturerSort} dto 排序DTO
 * @returns {Promise<Manufacturer>} 制造商信息DTO
 */
export function updateManufacturerSort(dto: ManufacturerSort): Promise<Manufacturer> {
  return request<Manufacturer>({
    url: `${MANUFACTURER_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取制造商信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getManufacturerOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MANUFACTURER_API_BASE}/options`,
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
export function getManufacturerTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MANUFACTURER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入制造商信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importManufacturer(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MANUFACTURER_API_BASE}/import`,
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
 * 导出制造商信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportManufacturer(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MANUFACTURER_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
