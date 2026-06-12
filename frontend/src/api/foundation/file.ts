// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：file.ts
// 创建时间：2026-06-09
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
  FileChunkCheck,
  FileChunkCheckResult,
  FileChunkCancel,
  FileChunkList,
  FileChunkListResult,
  FileChunkMerge,
  FileChunkUpload,
  FileCreate,
  FilePublicAccess,
  FileStatus,
  FileUpdate,
  FileUploadMeta,
  FileUploadResult,
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
// 上传 / 下载
// ========================================

/**
 * 整文件上传
 * @param {globalThis.File} file 文件
 * @param {FileUploadMeta} [meta] 可选元数据（描述、标签、公开范围、上传类型、目标文件名）
 * @returns {Promise<FileUploadResult>} 上传结果
 */
export function uploadFile(
  file: globalThis.File,
  meta?: FileUploadMeta
): Promise<FileUploadResult> {
  const formData = new FormData();
  formData.append('file', file);
  if (meta?.fileDescription != null) formData.append('fileDescription', meta.fileDescription);
  if (meta?.fileTags != null) formData.append('fileTags', meta.fileTags);
  if (meta?.isPublic != null) formData.append('isPublic', String(meta.isPublic));
  if (meta?.fileUploadType != null) formData.append('fileUploadType', String(meta.fileUploadType));
  if (meta?.targetFileName != null) formData.append('targetFileName', meta.targetFileName);
  return request<FileUploadResult>({
    url: `${FILE_API_BASE}/upload`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
}

/**
 * 检查分片是否已上传
 * @param {FileChunkCheck} dto 检查参数
 * @returns {Promise<FileChunkCheckResult>} 是否存在
 */
export function checkFileChunk(dto: FileChunkCheck): Promise<FileChunkCheckResult> {
  return request<FileChunkCheckResult>({
    url: `${FILE_API_BASE}/check`,
    method: 'post',
    data: dto,
  });
}

/**
 * 列出已上传分片（断点续传）
 * @param {FileChunkList} dto 查询参数
 * @returns {Promise<FileChunkListResult>} 已上传分片序号
 */
export function listFileChunks(dto: FileChunkList): Promise<FileChunkListResult> {
  return request<FileChunkListResult>({
    url: `${FILE_API_BASE}/chunk-list`,
    method: 'post',
    data: dto,
  });
}

/**
 * 取消分片上传并清理临时文件
 * @param {FileChunkCancel} dto 取消参数
 * @returns {Promise<void>} 操作结果
 */
export function cancelFileChunks(dto: FileChunkCancel): Promise<void> {
  return request({
    url: `${FILE_API_BASE}/chunk`,
    method: 'delete',
    data: dto,
  });
}

/**
 * 上传分片
 * @param {globalThis.File} file 分片数据
 * @param {FileChunkUpload} dto 分片元数据
 * @returns {Promise<void>} 操作结果
 */
export function uploadFileChunk(file: globalThis.File, dto: FileChunkUpload): Promise<void> {
  const formData = new FormData();
  formData.append('file', file);
  Object.entries(dto).forEach(([key, value]) => {
    if (value != null) formData.append(key, String(value));
  });
  return request({
    url: `${FILE_API_BASE}/chunk`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
}

/**
 * 合并分片
 * @param {FileChunkMerge} dto 合并参数
 * @returns {Promise<FileUploadResult>} 上传结果
 */
export function mergeFileChunks(dto: FileChunkMerge): Promise<FileUploadResult> {
  return request<FileUploadResult>({
    url: `${FILE_API_BASE}/merge`,
    method: 'post',
    data: dto,
  });
}

/**
 * 下载文件
 * @param {string} id 文件 ID
 * @returns {Promise<Blob>} 文件流
 */
export function downloadFile(id: string): Promise<Blob> {
  return request<Blob>({
    url: `${FILE_API_BASE}/${id}/download`,
    method: 'get',
    responseType: 'blob',
  });
}

/**
 * 更新文件公开范围
 * @param {string} id 文件 ID
 * @param {FilePublicAccess} dto 公开范围
 * @returns {Promise<TaktFile>} 文件 DTO
 */
export function changeFilePublicAccess(id: string, dto: FilePublicAccess): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}/${id}/is-public`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 导出
// ========================================

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
