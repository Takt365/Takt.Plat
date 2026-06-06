// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/defect
// 文件名称：pcba-inspection.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/defect 模块 API（自动生成，请勿手改路由常量）
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
  PcbaInspection,
  PcbaInspectionCreate,
  PcbaInspectionStatus,
  PcbaInspectionUpdate
} from '@/types/logistics/manufacturing/defect/pcba-inspection';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPcbaInspections
 */
const PCBA_INSPECTION_API_BASE = 'TaktPcbaInspections';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取PCBA检查日报列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PcbaInspection>>} 分页结果
 */
export function getPcbaInspectionList(queryDto: any): Promise<TaktPagedResult<PcbaInspection>> {
  return request<TaktPagedResult<PcbaInspection>>({
    url: `${PCBA_INSPECTION_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取PCBA检查日报
 * @param {string} id PCBA检查日报ID
 * @returns {Promise<PcbaInspection>} PCBA检查日报DTO
 */
export function getPcbaInspectionById(id: string): Promise<PcbaInspection> {
  return request<PcbaInspection>({
    url: `${PCBA_INSPECTION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建PCBA检查日报
 * @param {PcbaInspectionCreate} dto 创建DTO
 * @returns {Promise<PcbaInspection>} PCBA检查日报DTO
 */
export function createPcbaInspection(dto: PcbaInspectionCreate): Promise<PcbaInspection> {
  return request<PcbaInspection>({
    url: `${PCBA_INSPECTION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新PCBA检查日报
 * @param {string} id PCBA检查日报ID
 * @param {PcbaInspectionUpdate} dto 更新DTO
 * @returns {Promise<PcbaInspection>} PCBA检查日报DTO
 */
export function updatePcbaInspection(id: string, dto: PcbaInspectionUpdate): Promise<PcbaInspection> {
  return request<PcbaInspection>({
    url: `${PCBA_INSPECTION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除PCBA检查日报
 * @param {string} id PCBA检查日报ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePcbaInspectionById(id: string): Promise<void> {
  return request({
    url: `${PCBA_INSPECTION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除PCBA检查日报
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePcbaInspectionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PCBA_INSPECTION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新PCBA检查日报状态
 * @param {PcbaInspectionStatus} dto 状态DTO
 * @returns {Promise<PcbaInspection>} PCBA检查日报DTO
 */
export function updatePcbaInspectionStatus(dto: PcbaInspectionStatus): Promise<PcbaInspection> {
  return request<PcbaInspection>({
    url: `${PCBA_INSPECTION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取PCBA检查日报选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPcbaInspectionOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PCBA_INSPECTION_API_BASE}/options`,
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
export function getPcbaInspectionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PCBA_INSPECTION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入PCBA检查日报
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPcbaInspection(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PCBA_INSPECTION_API_BASE}/import`,
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
 * 导出PCBA检查日报
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPcbaInspection(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PCBA_INSPECTION_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
