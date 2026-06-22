// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：iso-code.ts
// 创建时间：2026-06-18
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
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
  IsoCode,
  IsoCodeCreate,
  IsoCodeSort,
  IsoCodeStatus,
  IsoCodeUpdate
} from '@/types/foundation/iso-code';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktIsoCodes
 */
const ISO_CODE_API_BASE = 'TaktIsoCodes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取ISO编码列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<IsoCode>>} 分页结果
 */
export function getIsoCodeList(queryDto: any): Promise<TaktPagedResult<IsoCode>> {
  return request<TaktPagedResult<IsoCode>>({
    url: `${ISO_CODE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取ISO编码
 * @param {string} id ISO编码ID
 * @returns {Promise<IsoCode>} ISO编码DTO
 */
export function getIsoCodeById(id: string): Promise<IsoCode> {
  return request<IsoCode>({
    url: `${ISO_CODE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建ISO编码
 * @param {IsoCodeCreate} dto 创建DTO
 * @returns {Promise<IsoCode>} ISO编码DTO
 */
export function createIsoCode(dto: IsoCodeCreate): Promise<IsoCode> {
  return request<IsoCode>({
    url: `${ISO_CODE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新ISO编码
 * @param {string} id ISO编码ID
 * @param {IsoCodeUpdate} dto 更新DTO
 * @returns {Promise<IsoCode>} ISO编码DTO
 */
export function updateIsoCode(id: string, dto: IsoCodeUpdate): Promise<IsoCode> {
  return request<IsoCode>({
    url: `${ISO_CODE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除ISO编码
 * @param {string} id ISO编码ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteIsoCodeById(id: string): Promise<void> {
  return request({
    url: `${ISO_CODE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除ISO编码
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteIsoCodeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ISO_CODE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新ISO编码状态
 * @param {IsoCodeStatus} dto 状态 DTO
 * @returns {Promise<IsoCode>} ISO编码DTO
 */
export function updateIsoCodeStatus(dto: IsoCodeStatus): Promise<IsoCode> {
  return request<IsoCode>({
    url: `${ISO_CODE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新ISO编码排序
 * @param {IsoCodeSort} dto 排序DTO
 * @returns {Promise<IsoCode>} ISO编码DTO
 */
export function updateIsoCodeSort(dto: IsoCodeSort): Promise<IsoCode> {
  return request<IsoCode>({
    url: `${ISO_CODE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取ISO编码选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getIsoCodeOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ISO_CODE_API_BASE}/options`,
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
export function getIsoCodeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ISO_CODE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入ISO编码
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importIsoCode(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ISO_CODE_API_BASE}/import`,
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
 * 导出ISO编码
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportIsoCode(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ISO_CODE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
