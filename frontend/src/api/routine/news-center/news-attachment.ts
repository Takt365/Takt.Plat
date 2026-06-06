// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/news-center
// 文件名称：news-attachment.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/news-center 模块 API（自动生成，请勿手改路由常量）
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
  NewsAttachment,
  NewsAttachmentCreate,
  NewsAttachmentSort,
  NewsAttachmentUpdate
} from '@/types/routine/news-center/news-attachment';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktNewsAttachments
 */
const NEWS_ATTACHMENT_API_BASE = 'TaktNewsAttachments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取新闻中心附件列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<NewsAttachment>>} 分页结果
 */
export function getNewsAttachmentList(queryDto: any): Promise<TaktPagedResult<NewsAttachment>> {
  return request<TaktPagedResult<NewsAttachment>>({
    url: `${NEWS_ATTACHMENT_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取新闻中心附件
 * @param {string} id 新闻中心附件ID
 * @returns {Promise<NewsAttachment>} 新闻中心附件DTO
 */
export function getNewsAttachmentById(id: string): Promise<NewsAttachment> {
  return request<NewsAttachment>({
    url: `${NEWS_ATTACHMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建新闻中心附件
 * @param {NewsAttachmentCreate} dto 创建DTO
 * @returns {Promise<NewsAttachment>} 新闻中心附件DTO
 */
export function createNewsAttachment(dto: NewsAttachmentCreate): Promise<NewsAttachment> {
  return request<NewsAttachment>({
    url: `${NEWS_ATTACHMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新新闻中心附件
 * @param {string} id 新闻中心附件ID
 * @param {NewsAttachmentUpdate} dto 更新DTO
 * @returns {Promise<NewsAttachment>} 新闻中心附件DTO
 */
export function updateNewsAttachment(id: string, dto: NewsAttachmentUpdate): Promise<NewsAttachment> {
  return request<NewsAttachment>({
    url: `${NEWS_ATTACHMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除新闻中心附件
 * @param {string} id 新闻中心附件ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteNewsAttachmentById(id: string): Promise<void> {
  return request({
    url: `${NEWS_ATTACHMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除新闻中心附件
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteNewsAttachmentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${NEWS_ATTACHMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新新闻中心附件排序
 * @param {NewsAttachmentSort} dto 排序DTO
 * @returns {Promise<NewsAttachment>} 新闻中心附件DTO
 */
export function updateNewsAttachmentSort(dto: NewsAttachmentSort): Promise<NewsAttachment> {
  return request<NewsAttachment>({
    url: `${NEWS_ATTACHMENT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取新闻附件选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getNewsAttachmentOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${NEWS_ATTACHMENT_API_BASE}/options`,
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
export function getNewsAttachmentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${NEWS_ATTACHMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入新闻中心附件
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importNewsAttachment(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${NEWS_ATTACHMENT_API_BASE}/import`,
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
 * 导出新闻中心附件
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportNewsAttachment(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${NEWS_ATTACHMENT_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
