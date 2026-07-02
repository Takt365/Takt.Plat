// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/document-center
// 文件名称：document-change-log.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/document-center 模块 API（自动生成，请勿手改路由常量）
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
  DocumentChangeLog,
  DocumentChangeLogCreate,
  DocumentChangeLogUpdate
} from '@/types/routine/document-center/document-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDocumentChangeLogs
 */
const DOCUMENT_CHANGE_LOG_API_BASE = 'TaktDocumentChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取文管文档变更日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<DocumentChangeLog>>} 分页结果
 */
export function getDocumentChangeLogList(queryDto: any): Promise<TaktPagedResult<DocumentChangeLog>> {
  return request<TaktPagedResult<DocumentChangeLog>>({
    url: `${DOCUMENT_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取文管文档变更日志
 * @param {string} id 文管文档变更日志ID
 * @returns {Promise<DocumentChangeLog>} 文管文档变更日志DTO
 */
export function getDocumentChangeLogById(id: string): Promise<DocumentChangeLog> {
  return request<DocumentChangeLog>({
    url: `${DOCUMENT_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建文管文档变更日志
 * @param {DocumentChangeLogCreate} dto 创建DTO
 * @returns {Promise<DocumentChangeLog>} 文管文档变更日志DTO
 */
export function createDocumentChangeLog(dto: DocumentChangeLogCreate): Promise<DocumentChangeLog> {
  return request<DocumentChangeLog>({
    url: `${DOCUMENT_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新文管文档变更日志
 * @param {string} id 文管文档变更日志ID
 * @param {DocumentChangeLogUpdate} dto 更新DTO
 * @returns {Promise<DocumentChangeLog>} 文管文档变更日志DTO
 */
export function updateDocumentChangeLog(id: string, dto: DocumentChangeLogUpdate): Promise<DocumentChangeLog> {
  return request<DocumentChangeLog>({
    url: `${DOCUMENT_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除文管文档变更日志
 * @param {string} id 文管文档变更日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDocumentChangeLogById(id: string): Promise<void> {
  return request({
    url: `${DOCUMENT_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除文管文档变更日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDocumentChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DOCUMENT_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取文管文档变更日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getDocumentChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${DOCUMENT_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出文管文档变更日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDocumentChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DOCUMENT_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
