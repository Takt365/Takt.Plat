// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：knowledge-change-log.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块 API（自动生成，请勿手改路由常量）
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
  KnowledgeChangeLog,
  KnowledgeChangeLogCreate,
  KnowledgeChangeLogUpdate
} from '@/types/routine/help-desk/knowledge-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktKnowledgeChangeLogs
 */
const KNOWLEDGE_CHANGE_LOG_API_BASE = 'TaktKnowledgeChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取知识库变更日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<KnowledgeChangeLog>>} 分页结果
 */
export function getKnowledgeChangeLogList(queryDto: any): Promise<TaktPagedResult<KnowledgeChangeLog>> {
  return request<TaktPagedResult<KnowledgeChangeLog>>({
    url: `${KNOWLEDGE_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取知识库变更日志
 * @param {string} id 知识库变更日志ID
 * @returns {Promise<KnowledgeChangeLog>} 知识库变更日志DTO
 */
export function getKnowledgeChangeLogById(id: string): Promise<KnowledgeChangeLog> {
  return request<KnowledgeChangeLog>({
    url: `${KNOWLEDGE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建知识库变更日志
 * @param {KnowledgeChangeLogCreate} dto 创建DTO
 * @returns {Promise<KnowledgeChangeLog>} 知识库变更日志DTO
 */
export function createKnowledgeChangeLog(dto: KnowledgeChangeLogCreate): Promise<KnowledgeChangeLog> {
  return request<KnowledgeChangeLog>({
    url: `${KNOWLEDGE_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新知识库变更日志
 * @param {string} id 知识库变更日志ID
 * @param {KnowledgeChangeLogUpdate} dto 更新DTO
 * @returns {Promise<KnowledgeChangeLog>} 知识库变更日志DTO
 */
export function updateKnowledgeChangeLog(id: string, dto: KnowledgeChangeLogUpdate): Promise<KnowledgeChangeLog> {
  return request<KnowledgeChangeLog>({
    url: `${KNOWLEDGE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除知识库变更日志
 * @param {string} id 知识库变更日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteKnowledgeChangeLogById(id: string): Promise<void> {
  return request({
    url: `${KNOWLEDGE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除知识库变更日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteKnowledgeChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${KNOWLEDGE_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取知识库变更日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getKnowledgeChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${KNOWLEDGE_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出知识库变更日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportKnowledgeChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${KNOWLEDGE_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
