// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：file-upload.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  File as TaktFile
} from '@/types/foundation/file';
import type {
  FileChunkCheck,
  FileChunkCheckResult,
  FileChunkMerge,
  FileChunkUpload,
  FilePublicAccess
} from '@/types/foundation/file-upload';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFileUploads
 */
const FILE_UPLOAD_API_BASE = 'TaktFileUploads';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 整文件上传
 * @param {globalThis.File} file 文件
 * @param {string} fileDescription 描述
 * @param {string} fileTags 标签
 * @param {number} isPublic 是否公开
 * @returns {Promise<TaktFile>} 文件 DTO
 */
export function uploadFile(
  file: globalThis.File,
  fileDescription?: string,
  fileTags?: string,
  isPublic?: number
): Promise<TaktFile> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FILE_UPLOAD_API_BASE}`,
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
    url: `${FILE_UPLOAD_API_BASE}/check`,
    method: 'post',
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
  
  return request({
    url: `${FILE_UPLOAD_API_BASE}/chunk`,
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
 * @returns {Promise<TaktFile>} 文件 DTO
 */
export function mergeFileChunks(dto: FileChunkMerge): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_UPLOAD_API_BASE}/merge`,
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
    url: `${FILE_UPLOAD_API_BASE}/${id}/download`,
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
    url: `${FILE_UPLOAD_API_BASE}/${id}/is-public`,
    method: 'put',
    data: dto,
  });
}
