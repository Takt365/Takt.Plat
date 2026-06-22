// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：ticket.ts
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
  Ticket,
  TicketCreate,
  TicketStatus,
  TicketUpdate,
  TicketSubmit,
  TicketCreateFromChannel,
  TicketAssign,
  TicketWorkflowAction,
  TicketReply,
  TicketReplyCreate,
  TicketReplyQuery,
  TicketMyAsset
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
 * 获取当前用户的工单列表（我的工单）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Ticket>>} 分页结果
 */
export function getMyTicketList(queryDto: any): Promise<TaktPagedResult<Ticket>> {
  return request<TaktPagedResult<Ticket>>({
    url: `${TICKET_API_BASE}/my-list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前用户工单关联的资产汇总（我的资产，按 AssetCode 聚合）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TicketMyAsset>>} 分页结果
 */
export function getMyTicketAssetList(queryDto: any): Promise<TaktPagedResult<TicketMyAsset>> {
  return request<TaktPagedResult<TicketMyAsset>>({
    url: `${TICKET_API_BASE}/my-assets`,
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
// ITSM 工作流
// ========================================

/**
 * 门户用户提交工单
 * @param {TicketSubmit} dto 提交 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function submitTicket(dto: TicketSubmit): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/submit`,
    method: 'post',
    data: dto,
  });
}

/**
 * 邮件/API 渠道建单
 * @param {TicketCreateFromChannel} dto 渠道建单 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function createTicketFromChannel(dto: TicketCreateFromChannel): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/channel`,
    method: 'post',
    data: dto,
  });
}

/**
 * 指派或领取工单
 * @param {TicketAssign} dto 指派 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function assignTicket(dto: TicketAssign): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/assign`,
    method: 'put',
    data: dto,
  });
}

/**
 * 开始处理工单
 * @param {TicketWorkflowAction} dto 动作 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function startTicketProgress(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/start`,
    method: 'put',
    data: dto,
  });
}

/**
 * 等待用户回复
 * @param {TicketWorkflowAction} dto 动作 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function waitForRequester(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/wait-requester`,
    method: 'put',
    data: dto,
  });
}

/**
 * 标记已解决
 * @param {TicketWorkflowAction} dto 动作 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function resolveTicket(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/resolve`,
    method: 'put',
    data: dto,
  });
}

/**
 * 用户确认关闭
 * @param {TicketWorkflowAction} dto 动作 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function confirmCloseTicket(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/confirm-close`,
    method: 'put',
    data: dto,
  });
}

/**
 * 重新打开工单
 * @param {TicketWorkflowAction} dto 动作 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function reopenTicket(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/reopen`,
    method: 'put',
    data: dto,
  });
}

/**
 * 添加工单回复
 * @param {TicketReplyCreate} dto 回复 DTO
 * @returns {Promise<TicketReply>} 回复 DTO
 */
export function replyTicket(dto: TicketReplyCreate): Promise<TicketReply> {
  return request<TicketReply>({
    url: `${TICKET_API_BASE}/reply`,
    method: 'post',
    data: dto,
  });
}

/**
 * 获取工单回复列表（分页）
 * @param {TicketReplyQuery} queryDto 查询 DTO
 * @returns {Promise<TaktPagedResult<TicketReply>>} 分页结果
 */
export function getTicketReplyList(queryDto: TicketReplyQuery): Promise<TaktPagedResult<TicketReply>> {
  return request<TaktPagedResult<TicketReply>>({
    url: `${TICKET_API_BASE}/replies`,
    method: 'get',
    params: queryDto,
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
