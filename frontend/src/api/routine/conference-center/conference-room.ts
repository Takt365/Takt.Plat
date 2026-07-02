// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/conference-center
// 文件名称：conference-room.ts
// 创建时间：2026-06-23
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
  ConferenceRoom,
  ConferenceRoomCreate,
  ConferenceRoomSort,
  ConferenceRoomStatus,
  ConferenceRoomUpdate
} from '@/types/routine/conference-center/conference-room';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktConferenceRooms
 */
const CONFERENCE_ROOM_API_BASE = 'TaktConferenceRooms';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会议室列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ConferenceRoom>>} 分页结果
 */
export function getConferenceRoomList(queryDto: any): Promise<TaktPagedResult<ConferenceRoom>> {
  return request<TaktPagedResult<ConferenceRoom>>({
    url: `${CONFERENCE_ROOM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取会议室
 * @param {string} id 会议室ID
 * @returns {Promise<ConferenceRoom>} 会议室DTO
 */
export function getConferenceRoomById(id: string): Promise<ConferenceRoom> {
  return request<ConferenceRoom>({
    url: `${CONFERENCE_ROOM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建会议室
 * @param {ConferenceRoomCreate} dto 创建DTO
 * @returns {Promise<ConferenceRoom>} 会议室DTO
 */
export function createConferenceRoom(dto: ConferenceRoomCreate): Promise<ConferenceRoom> {
  return request<ConferenceRoom>({
    url: `${CONFERENCE_ROOM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会议室
 * @param {string} id 会议室ID
 * @param {ConferenceRoomUpdate} dto 更新DTO
 * @returns {Promise<ConferenceRoom>} 会议室DTO
 */
export function updateConferenceRoom(id: string, dto: ConferenceRoomUpdate): Promise<ConferenceRoom> {
  return request<ConferenceRoom>({
    url: `${CONFERENCE_ROOM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会议室
 * @param {string} id 会议室ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteConferenceRoomById(id: string): Promise<void> {
  return request({
    url: `${CONFERENCE_ROOM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会议室
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteConferenceRoomBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CONFERENCE_ROOM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新会议室状态
 * @param {ConferenceRoomStatus} dto 状态 DTO
 * @returns {Promise<ConferenceRoom>} 会议室DTO
 */
export function updateConferenceRoomStatus(dto: ConferenceRoomStatus): Promise<ConferenceRoom> {
  return request<ConferenceRoom>({
    url: `${CONFERENCE_ROOM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新会议室排序
 * @param {ConferenceRoomSort} dto 排序DTO
 * @returns {Promise<ConferenceRoom>} 会议室DTO
 */
export function updateConferenceRoomSort(dto: ConferenceRoomSort): Promise<ConferenceRoom> {
  return request<ConferenceRoom>({
    url: `${CONFERENCE_ROOM_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会议室选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getConferenceRoomOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CONFERENCE_ROOM_API_BASE}/options`,
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
export function getConferenceRoomTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CONFERENCE_ROOM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入会议室
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importConferenceRoom(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CONFERENCE_ROOM_API_BASE}/import`,
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
 * 导出会议室
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportConferenceRoom(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CONFERENCE_ROOM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
