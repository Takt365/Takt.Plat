// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：message.ts
// 创建时间：2026-05-25
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
  Message,
  MessageBatchCreate,
  MessageCreate,
  MessageQuery,
  MessageRead,
  MessageReadListQuery,
  MessageStatistics,
  MessageUnread,
  MessageUnreadListQuery,
} from '@/types/foundation/message';
import { TaktReadStatus } from '@/utils/common-enums';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMessage
 */
const MESSAGE_API_BASE = 'TaktMessages';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取在线消息列表（分页）
 * @param {MessageQuery} queryDto 查询参数
 * @returns {Promise<TaktPagedResult<Message>>} 分页结果
 */
export function getMessageList(queryDto: MessageQuery): Promise<TaktPagedResult<Message>> {
  return request<TaktPagedResult<Message>>({
    url: `${MESSAGE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前用户已读消息列表（分页）
 * @param {MessageReadListQuery} queryDto 查询参数
 * @returns {Promise<TaktPagedResult<Message>>} 分页结果
 */
export function getMessageReadList(queryDto: MessageReadListQuery): Promise<TaktPagedResult<Message>> {
  return request<TaktPagedResult<Message>>({
    url: `${MESSAGE_API_BASE}/read-list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前用户未读消息列表（分页）
 * @param {MessageUnreadListQuery} queryDto 查询参数
 * @returns {Promise<TaktPagedResult<Message>>} 分页结果
 */
export function getMessageUnreadList(queryDto: MessageUnreadListQuery): Promise<TaktPagedResult<Message>> {
  return request<TaktPagedResult<Message>>({
    url: `${MESSAGE_API_BASE}/unread-list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取在线消息
 * @param {string} id 在线消息ID
 * @returns {Promise<Message>} 在线消息
 */
export function getMessageById(id: string): Promise<Message> {
  return request<Message>({
    url: `${MESSAGE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建在线消息
 * @param {MessageCreate} dto 创建参数
 * @returns {Promise<Message>} 在线消息
 */
export function createMessage(dto: MessageCreate): Promise<Message> {
  return request<Message>({
    url: `${MESSAGE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 批量创建在线消息并 SignalR 推送给各接收者（全员或指定用户）
 * @param {MessageBatchCreate} dto 批量创建参数
 * @returns {Promise<Message[]>} 已落库消息列表
 */
export function createAndSendMessages(dto: MessageBatchCreate): Promise<Message[]> {
  return request<Message[]>({
    url: `${MESSAGE_API_BASE}/batch-send`,
    method: 'post',
    data: dto,
  });
}

/**
 * 按消息 ID 推送给接收者（SignalR；须已落库）
 * @param {string} messageId 在线消息 ID（须为 string，避免雪花 ID 精度丢失）
 * @returns {Promise<void>} 操作结果
 */
export function sendMessageById(messageId: string): Promise<void> {
  return request({
    url: `${MESSAGE_API_BASE}/${messageId}/send`,
    method: 'post',
  });
}

/**
 * 删除在线消息
 * @param {string} id 在线消息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMessageById(id: string): Promise<void> {
  return request({
    url: `${MESSAGE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除在线消息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMessageBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MESSAGE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取在线消息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMessageOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MESSAGE_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 统计
// ========================================

/**
 * 获取在线消息统计
 * @returns {Promise<MessageStatistics>} 统计结果
 */
export function getMessageStatistics(): Promise<MessageStatistics> {
  return request<MessageStatistics>({
    url: `${MESSAGE_API_BASE}/statistics`,
    method: 'get',
  });
}

/**
 * 标记在线消息为已读
 * @param {string} id 在线消息 ID
 * @returns {Promise<Message>} 在线消息
 */
export function markMessageReadById(id: string): Promise<Message> {
  const dto: MessageRead = {
    messageId: id,
    readStatus: TaktReadStatus.Read,
  };
  return request<Message>({
    url: `${MESSAGE_API_BASE}/${id}/read`,
    method: 'put',
    data: dto,
  });
}

/**
 * 标记在线消息为未读
 * @param {string} id 在线消息 ID
 * @returns {Promise<Message>} 在线消息
 */
export function markMessageUnreadById(id: string): Promise<Message> {
  const dto: MessageUnread = {
    messageId: id,
    readStatus: TaktReadStatus.Unread,
    readTime: null,
  };
  return request<Message>({
    url: `${MESSAGE_API_BASE}/${id}/unread`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 导出
// ========================================

/**
 * 导出在线消息
 * @param {MessageQuery} queryDto 查询参数
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMessage(
  queryDto?: MessageQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MESSAGE_API_BASE}/export`,
    method: 'get',
    params: {
      ...queryDto,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
