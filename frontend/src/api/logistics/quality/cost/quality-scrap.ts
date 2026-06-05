// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：quality-scrap.ts
// 创建时间：2026-06-05
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
  QualityScrap,
  QualityScrapCreate,
  QualityScrapUpdate
} from '@/types/logistics/quality/cost/quality-scrap';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityScraps
 */
const QUALITY_SCRAP_API_BASE = 'TaktQualityScraps';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取品质废弃主列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityScrap>>} 分页结果
 */
export function getQualityScrapList(queryDto: any): Promise<TaktPagedResult<QualityScrap>> {
  return request<TaktPagedResult<QualityScrap>>({
    url: `${QUALITY_SCRAP_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取品质废弃主
 * @param {string} id 品质废弃主ID
 * @returns {Promise<QualityScrap>} 品质废弃主DTO
 */
export function getQualityScrapById(id: string): Promise<QualityScrap> {
  return request<QualityScrap>({
    url: `${QUALITY_SCRAP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建品质废弃主
 * @param {QualityScrapCreate} dto 创建DTO
 * @returns {Promise<QualityScrap>} 品质废弃主DTO
 */
export function createQualityScrap(dto: QualityScrapCreate): Promise<QualityScrap> {
  return request<QualityScrap>({
    url: `${QUALITY_SCRAP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新品质废弃主
 * @param {string} id 品质废弃主ID
 * @param {QualityScrapUpdate} dto 更新DTO
 * @returns {Promise<QualityScrap>} 品质废弃主DTO
 */
export function updateQualityScrap(id: string, dto: QualityScrapUpdate): Promise<QualityScrap> {
  return request<QualityScrap>({
    url: `${QUALITY_SCRAP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除品质废弃主
 * @param {string} id 品质废弃主ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityScrapById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_SCRAP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除品质废弃主
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityScrapBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_SCRAP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取品质废弃主选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityScrapOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${QUALITY_SCRAP_API_BASE}/options`,
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
export function getQualityScrapTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_SCRAP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入品质废弃主
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityScrap(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_SCRAP_API_BASE}/import`,
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
 * 导出品质废弃主
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityScrap(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_SCRAP_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
