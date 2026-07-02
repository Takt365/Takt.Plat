// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/customer-service
// 文件名称：service-ticket.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块 API（自动生成，请勿手改路由常量）
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
  ServiceTicket,
  ServiceTicketCreate,
  ServiceTicketSort,
  ServiceTicketStatus,
  ServiceTicketUpdate
} from '@/types/logistics/customer-service/service-ticket';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktServiceTickets
 */
const SERVICE_TICKET_API_BASE = 'TaktServiceTickets';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取服务工单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ServiceTicket>>} 分页结果
 */
export function getServiceTicketList(queryDto: any): Promise<TaktPagedResult<ServiceTicket>> {
  return request<TaktPagedResult<ServiceTicket>>({
    url: `${SERVICE_TICKET_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取服务工单
 * @param {string} id 服务工单ID
 * @returns {Promise<ServiceTicket>} 服务工单DTO
 */
export function getServiceTicketById(id: string): Promise<ServiceTicket> {
  return request<ServiceTicket>({
    url: `${SERVICE_TICKET_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建服务工单
 * @param {ServiceTicketCreate} dto 创建DTO
 * @returns {Promise<ServiceTicket>} 服务工单DTO
 */
export function createServiceTicket(dto: ServiceTicketCreate): Promise<ServiceTicket> {
  return request<ServiceTicket>({
    url: `${SERVICE_TICKET_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新服务工单
 * @param {string} id 服务工单ID
 * @param {ServiceTicketUpdate} dto 更新DTO
 * @returns {Promise<ServiceTicket>} 服务工单DTO
 */
export function updateServiceTicket(id: string, dto: ServiceTicketUpdate): Promise<ServiceTicket> {
  return request<ServiceTicket>({
    url: `${SERVICE_TICKET_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除服务工单
 * @param {string} id 服务工单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteServiceTicketById(id: string): Promise<void> {
  return request({
    url: `${SERVICE_TICKET_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除服务工单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteServiceTicketBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SERVICE_TICKET_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新服务工单状态
 * @param {ServiceTicketStatus} dto 状态 DTO
 * @returns {Promise<ServiceTicket>} 服务工单DTO
 */
export function updateServiceTicketStatus(dto: ServiceTicketStatus): Promise<ServiceTicket> {
  return request<ServiceTicket>({
    url: `${SERVICE_TICKET_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新服务工单排序
 * @param {ServiceTicketSort} dto 排序DTO
 * @returns {Promise<ServiceTicket>} 服务工单DTO
 */
export function updateServiceTicketSort(dto: ServiceTicketSort): Promise<ServiceTicket> {
  return request<ServiceTicket>({
    url: `${SERVICE_TICKET_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取服务工单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getServiceTicketOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SERVICE_TICKET_API_BASE}/options`,
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
export function getServiceTicketTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SERVICE_TICKET_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入服务工单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importServiceTicket(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SERVICE_TICKET_API_BASE}/import`,
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
 * 导出服务工单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportServiceTicket(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SERVICE_TICKET_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
