// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec-attachment.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块 API（自动生成，请勿手改路由常量）
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
  EcAttachment,
  EcAttachmentCreate,
  EcAttachmentUpdate
} from '@/types/logistics/manufacturing/engineering-change/ec-attachment';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcAttachments
 */
const EC_ATTACHMENT_API_BASE = 'TaktEcAttachments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变附件列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcAttachment>>} 分页结果
 */
export function getEcAttachmentList(queryDto: any): Promise<TaktPagedResult<EcAttachment>> {
  return request<TaktPagedResult<EcAttachment>>({
    url: `${EC_ATTACHMENT_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取设变附件
 * @param {string} id 设变附件ID
 * @returns {Promise<EcAttachment>} 设变附件DTO
 */
export function getEcAttachmentById(id: string): Promise<EcAttachment> {
  return request<EcAttachment>({
    url: `${EC_ATTACHMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变附件
 * @param {EcAttachmentCreate} dto 创建DTO
 * @returns {Promise<EcAttachment>} 设变附件DTO
 */
export function createEcAttachment(dto: EcAttachmentCreate): Promise<EcAttachment> {
  return request<EcAttachment>({
    url: `${EC_ATTACHMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变附件
 * @param {string} id 设变附件ID
 * @param {EcAttachmentUpdate} dto 更新DTO
 * @returns {Promise<EcAttachment>} 设变附件DTO
 */
export function updateEcAttachment(id: string, dto: EcAttachmentUpdate): Promise<EcAttachment> {
  return request<EcAttachment>({
    url: `${EC_ATTACHMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变附件
 * @param {string} id 设变附件ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcAttachmentById(id: string): Promise<void> {
  return request({
    url: `${EC_ATTACHMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变附件
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcAttachmentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_ATTACHMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变附件选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcAttachmentOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_ATTACHMENT_API_BASE}/options`,
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
export function getEcAttachmentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_ATTACHMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变附件
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcAttachment(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_ATTACHMENT_API_BASE}/import`,
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
 * 导出设变附件
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcAttachment(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_ATTACHMENT_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
