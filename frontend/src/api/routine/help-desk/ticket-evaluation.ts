// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：ticket-evaluation.ts
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
  TicketEvaluation,
  TicketEvaluationCreate,
  TicketEvaluationUpdate
} from '@/types/routine/help-desk/ticket-evaluation';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTicketEvaluations
 */
const TICKET_EVALUATION_API_BASE = 'TaktTicketEvaluations';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工单服务评价列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TicketEvaluation>>} 分页结果
 */
export function getTicketEvaluationList(queryDto: any): Promise<TaktPagedResult<TicketEvaluation>> {
  return request<TaktPagedResult<TicketEvaluation>>({
    url: `${TICKET_EVALUATION_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取工单服务评价
 * @param {string} id 工单服务评价ID
 * @returns {Promise<TicketEvaluation>} 工单服务评价DTO
 */
export function getTicketEvaluationById(id: string): Promise<TicketEvaluation> {
  return request<TicketEvaluation>({
    url: `${TICKET_EVALUATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工单服务评价
 * @param {TicketEvaluationCreate} dto 创建DTO
 * @returns {Promise<TicketEvaluation>} 工单服务评价DTO
 */
export function createTicketEvaluation(dto: TicketEvaluationCreate): Promise<TicketEvaluation> {
  return request<TicketEvaluation>({
    url: `${TICKET_EVALUATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工单服务评价
 * @param {string} id 工单服务评价ID
 * @param {TicketEvaluationUpdate} dto 更新DTO
 * @returns {Promise<TicketEvaluation>} 工单服务评价DTO
 */
export function updateTicketEvaluation(id: string, dto: TicketEvaluationUpdate): Promise<TicketEvaluation> {
  return request<TicketEvaluation>({
    url: `${TICKET_EVALUATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工单服务评价
 * @param {string} id 工单服务评价ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketEvaluationById(id: string): Promise<void> {
  return request({
    url: `${TICKET_EVALUATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工单服务评价
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketEvaluationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TICKET_EVALUATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工单服务评价选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTicketEvaluationOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${TICKET_EVALUATION_API_BASE}/options`,
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
export function getTicketEvaluationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_EVALUATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工单服务评价
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTicketEvaluation(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TICKET_EVALUATION_API_BASE}/import`,
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
 * 导出工单服务评价
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTicketEvaluation(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_EVALUATION_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
