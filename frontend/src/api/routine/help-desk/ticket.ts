// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：ticket.ts
// 创建时间：2026-07-09
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
  HelpDeskTicketStat
} from '@/types/routine/help-desk/help-desk-ticket-stat';
import type {
  HelpDeskTicketStatQuery
} from '@/types/routine/help-desk/help-desk-ticket-stat-query';
import type {
  MyTicketReply
} from '@/types/routine/help-desk/my-ticket-reply';
import type {
  Ticket,
  TicketCreate,
  TicketStatus,
  TicketUpdate
} from '@/types/routine/help-desk/ticket';
import type {
  TicketAssign
} from '@/types/routine/help-desk/ticket-assign';
import type {
  TicketCreateFromChannel
} from '@/types/routine/help-desk/ticket-create-from-channel';
import type {
  TicketMyAsset
} from '@/types/routine/help-desk/ticket-my-asset';
import type {
  TicketReply
} from '@/types/routine/help-desk/ticket-reply';
import type {
  TicketSessionReplyCreate
} from '@/types/routine/help-desk/ticket-session-reply-create';
import type {
  TicketSubmit
} from '@/types/routine/help-desk/ticket-submit';
import type {
  TicketWorkflowAction
} from '@/types/routine/help-desk/ticket-workflow-action';

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
 * 获取服务台工单统计（数据看板）
 * @param {HelpDeskTicketStatQuery} queryDto 查询 DTO
 * @returns {Promise<HelpDeskTicketStat>} 服务台工单统计
 */
export function getHelpDeskTicketStat(queryDto: HelpDeskTicketStatQuery): Promise<HelpDeskTicketStat> {
  return request<HelpDeskTicketStat>({
    url: `${TICKET_API_BASE}/ticket-stat`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前用户提交的工单列表（分页）
 * @param {any} queryDto 查询 DTO
 * @returns {Promise<TaktPagedResult<Ticket>>} 分页结果
 */
export function getMyTicketList(queryDto: any): Promise<TaktPagedResult<Ticket>> {
  return request<TaktPagedResult<Ticket>>({
    url: `${TICKET_API_BASE}/my-tickets`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前用户提交的工单详情
 * @param {string} id 工单 ID
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function getMyTicketById(id: string): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/my-tickets/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 获取当前用户工单的回复列表（分页，不含内部备注）
 * @param {string} id 工单 ID
 * @param {any} queryDto 分页查询
 * @returns {Promise<TaktPagedResult<MyTicketReply>>} 分页结果
 */
export function getMyTicketReplyList(id: string, queryDto: any): Promise<TaktPagedResult<MyTicketReply>> {
  return request<TaktPagedResult<MyTicketReply>>({
    url: `${TICKET_API_BASE}/my-tickets/{id:long}/replies`,
    method: 'get',
    params: {
      id,
      ...queryDto
    },
  });
}

/**
 * 门户用户回复自己的工单
 * @param {string} id 工单 ID
 * @param {TicketSessionReplyCreate} dto 回复 DTO
 * @returns {Promise<TicketSessionReplyCreate>} 回复 DTO
 */
export function replyMyTicket(id: string, dto: TicketSessionReplyCreate): Promise<TicketSessionReplyCreate> {
  return request<TicketSessionReplyCreate>({
    url: `${TICKET_API_BASE}/my-tickets/{id:long}/reply`,
    method: 'post',
    data: dto,
  });
}

/**
 * 根据ID获取工单
 * @param {string} id 工单ID
 * @returns {Promise<Ticket>} 工单DTO
 */
export function getTicketById(id: string): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
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
    url: `${TICKET_API_BASE}/{id:long}`,
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
    url: `${TICKET_API_BASE}/{id:long}`,
    method: 'delete',
    params: {
      id
    },
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
 * 开始处理工单
 * @param {TicketWorkflowAction} dto 动作 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function startTicketProgress(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/start`,
    method: 'post',
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
    url: `${TICKET_API_BASE}/wait`,
    method: 'post',
    data: dto,
  });
}

/**
 * 标记工单已解决
 * @param {TicketWorkflowAction} dto 动作 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function resolveTicket(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/resolve`,
    method: 'post',
    data: dto,
  });
}

/**
 * 用户确认关闭工单
 * @param {TicketWorkflowAction} dto 动作 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function confirmCloseTicket(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/confirm-close`,
    method: 'post',
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
    method: 'post',
    data: dto,
  });
}

/**
 * 添加工单回复（会话）
 * @param {TicketSessionReplyCreate} dto 回复 DTO
 * @returns {Promise<TicketReply>} 回复 DTO
 */
export function replyTicket(dto: TicketSessionReplyCreate): Promise<TicketReply> {
  return request<TicketReply>({
    url: `${TICKET_API_BASE}/reply`,
    method: 'post',
    data: dto,
  });
}

/**
 * 获取工单回复列表（分页）
 * @param {any} queryDto 查询 DTO
 * @returns {Promise<TaktPagedResult<TicketReply>>} 分页结果
 */
export function getTicketReplyList(queryDto: any): Promise<TaktPagedResult<TicketReply>> {
  return request<TaktPagedResult<TicketReply>>({
    url: `${TICKET_API_BASE}/replies`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前用户工单关联资产汇总
 * @param {any} queryDto 分页查询
 * @returns {Promise<TaktPagedResult<TicketMyAsset>>} 分页结果
 */
export function getMyAssetList(queryDto: any): Promise<TaktPagedResult<TicketMyAsset>> {
  return request<TaktPagedResult<TicketMyAsset>>({
    url: `${TICKET_API_BASE}/my-assets`,
    method: 'get',
    params: queryDto,
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
// 关联分配
// ========================================

/**
 * 指派或领取工单
 * @param {TicketAssign} dto 指派 DTO
 * @returns {Promise<Ticket>} 工单 DTO
 */
export function assignTicket(dto: TicketAssign): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/assign`,
    method: 'post',
    data: dto,
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
