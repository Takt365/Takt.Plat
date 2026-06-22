// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/conference-center
// 文件名称：conference-participant.ts
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
  ConferenceParticipant,
  ConferenceParticipantCreate,
  ConferenceParticipantStatus,
  ConferenceParticipantUpdate
} from '@/types/routine/conference-center/conference-participant';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktConferenceParticipants
 */
const CONFERENCE_PARTICIPANT_API_BASE = 'TaktConferenceParticipants';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会议参与人列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ConferenceParticipant>>} 分页结果
 */
export function getConferenceParticipantList(queryDto: any): Promise<TaktPagedResult<ConferenceParticipant>> {
  return request<TaktPagedResult<ConferenceParticipant>>({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取会议参与人
 * @param {string} id 会议参与人ID
 * @returns {Promise<ConferenceParticipant>} 会议参与人DTO
 */
export function getConferenceParticipantById(id: string): Promise<ConferenceParticipant> {
  return request<ConferenceParticipant>({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建会议参与人
 * @param {ConferenceParticipantCreate} dto 创建DTO
 * @returns {Promise<ConferenceParticipant>} 会议参与人DTO
 */
export function createConferenceParticipant(dto: ConferenceParticipantCreate): Promise<ConferenceParticipant> {
  return request<ConferenceParticipant>({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会议参与人
 * @param {string} id 会议参与人ID
 * @param {ConferenceParticipantUpdate} dto 更新DTO
 * @returns {Promise<ConferenceParticipant>} 会议参与人DTO
 */
export function updateConferenceParticipant(id: string, dto: ConferenceParticipantUpdate): Promise<ConferenceParticipant> {
  return request<ConferenceParticipant>({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会议参与人
 * @param {string} id 会议参与人ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteConferenceParticipantById(id: string): Promise<void> {
  return request({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会议参与人
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteConferenceParticipantBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新会议参与人状态
 * @param {ConferenceParticipantStatus} dto 状态 DTO
 * @returns {Promise<ConferenceParticipant>} 会议参与人DTO
 */
export function updateConferenceParticipantStatus(dto: ConferenceParticipantStatus): Promise<ConferenceParticipant> {
  return request<ConferenceParticipant>({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会议参与人选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getConferenceParticipantOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/options`,
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
export function getConferenceParticipantTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入会议参与人
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importConferenceParticipant(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/import`,
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
 * 导出会议参与人
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportConferenceParticipant(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CONFERENCE_PARTICIPANT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
