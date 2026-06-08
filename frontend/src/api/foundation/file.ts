// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：file.ts
// 创建时间：2026-06-08
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
  File as TaktFile,
  FileCreate,
  FileStatus,
  FileUpdate
} from '@/types/foundation/file';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFiles
 */
const FILE_API_BASE = 'TaktFiles';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取文件列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TaktFile>>} 分页结果
 */
export function getFileList(queryDto: any): Promise<TaktPagedResult<TaktFile>> {
  return request<TaktPagedResult<TaktFile>>({
    url: `${FILE_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取文件
 * @param {string} id 文件ID
 * @returns {Promise<TaktFile>} 文件DTO
 */
export function getFileById(id: string): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建文件
 * @param {FileCreate} dto 创建DTO
 * @returns {Promise<TaktFile>} 文件DTO
 */
export function createFile(dto: FileCreate): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新文件
 * @param {string} id 文件ID
 * @param {FileUpdate} dto 更新DTO
 * @returns {Promise<TaktFile>} 文件DTO
 */
export function updateFile(id: string, dto: FileUpdate): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除文件
 * @param {string} id 文件ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFileById(id: string): Promise<void> {
  return request({
    url: `${FILE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除文件
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFileBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FILE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新文件状态
 * @param {FileStatus} dto 状态 DTO（TaktCommonStatus 枚举）
 * @returns {Promise<TaktFile>} 文件DTO
 */
export function updateFileStatus(dto: FileStatus): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取文件选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFileOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${FILE_API_BASE}/options`,
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
export function getFileTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FILE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入文件
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFile(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FILE_API_BASE}/import`,
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
 * 导出文件
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFile(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FILE_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
