// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/conference-center
// 文件名称：conference-agenda.ts
// 创建时间：2026-06-21
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/conference-center 模块 API（自动生成，请勿手改路由常量）
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
  ConferenceAgenda,
  ConferenceAgendaCreate,
  ConferenceAgendaUpdate
} from '@/types/routine/conference-center/conference-agenda';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktConferenceAgendas
 */
const CONFERENCE_AGENDA_API_BASE = 'TaktConferenceAgendas';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会议议程纪要列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ConferenceAgenda>>} 分页结果
 */
export function getConferenceAgendaList(queryDto: any): Promise<TaktPagedResult<ConferenceAgenda>> {
  return request<TaktPagedResult<ConferenceAgenda>>({
    url: `${CONFERENCE_AGENDA_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取会议议程纪要
 * @param {string} id 会议议程纪要ID
 * @returns {Promise<ConferenceAgenda>} 会议议程纪要DTO
 */
export function getConferenceAgendaById(id: string): Promise<ConferenceAgenda> {
  return request<ConferenceAgenda>({
    url: `${CONFERENCE_AGENDA_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建会议议程纪要
 * @param {ConferenceAgendaCreate} dto 创建DTO
 * @returns {Promise<ConferenceAgenda>} 会议议程纪要DTO
 */
export function createConferenceAgenda(dto: ConferenceAgendaCreate): Promise<ConferenceAgenda> {
  return request<ConferenceAgenda>({
    url: `${CONFERENCE_AGENDA_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会议议程纪要
 * @param {string} id 会议议程纪要ID
 * @param {ConferenceAgendaUpdate} dto 更新DTO
 * @returns {Promise<ConferenceAgenda>} 会议议程纪要DTO
 */
export function updateConferenceAgenda(id: string, dto: ConferenceAgendaUpdate): Promise<ConferenceAgenda> {
  return request<ConferenceAgenda>({
    url: `${CONFERENCE_AGENDA_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会议议程纪要
 * @param {string} id 会议议程纪要ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteConferenceAgendaById(id: string): Promise<void> {
  return request({
    url: `${CONFERENCE_AGENDA_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会议议程纪要
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteConferenceAgendaBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CONFERENCE_AGENDA_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会议议程纪要选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getConferenceAgendaOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CONFERENCE_AGENDA_API_BASE}/options`,
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
export function getConferenceAgendaTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CONFERENCE_AGENDA_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入会议议程纪要
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importConferenceAgenda(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CONFERENCE_AGENDA_API_BASE}/import`,
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
 * 导出会议议程纪要
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportConferenceAgenda(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CONFERENCE_AGENDA_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
