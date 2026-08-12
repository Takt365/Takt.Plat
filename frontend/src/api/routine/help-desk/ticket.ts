// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：ticket.ts
// 创建时间：2026-08-11
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
  Ticket,
  TicketCreate,
  TicketStatus,
  TicketUpdate
} from '@/types/routine/help-desk/ticket';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTickets
 */
const TICKET_API_BASE = 'TaktTickets';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Ticket>>} 分页结果
 */
export function getTicketList(queryDto: any): Promise<TaktPagedResult<Ticket>> {
  return request<TaktPagedResult<Ticket>>({
    url: `${TICKET_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工单
 * @param {string} id 工单ID
 * @returns {Promise<Ticket>} 工单DTO
 */
export function getTicketById(id: string): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工单
 * @param {TicketCreate} dto 创建DTO
 * @returns {Promise<Ticket>} 工单DTO
 */
export function createTicket(dto: TicketCreate): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工单
 * @param {string} id 工单ID
 * @param {TicketUpdate} dto 更新DTO
 * @returns {Promise<Ticket>} 工单DTO
 */
export function updateTicket(id: string, dto: TicketUpdate): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工单
 * @param {string} id 工单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketById(id: string): Promise<void> {
  return request({
    url: `${TICKET_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TICKET_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工单状态
 * @param {TicketStatus} dto 状态 DTO
 * @returns {Promise<Ticket>} 工单DTO
 */
export function updateTicketStatus(dto: TicketStatus): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTicketOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${TICKET_API_BASE}/options`,
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
export function getTicketTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTicket(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TICKET_API_BASE}/import`,
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
 * 导出工单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTicket(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
