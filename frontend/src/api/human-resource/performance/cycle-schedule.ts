// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：cycle-schedule.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块 API（自动生成，请勿手改路由常量）
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
  CycleSchedule,
  CycleScheduleCreate,
  CycleScheduleStatus,
  CycleScheduleUpdate
} from '@/types/human-resource/performance/cycle-schedule';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCycleSchedules
 */
const CYCLE_SCHEDULE_API_BASE = 'TaktCycleSchedules';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取绩效周期日程列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CycleSchedule>>} 分页结果
 */
export function getCycleScheduleList(queryDto: any): Promise<TaktPagedResult<CycleSchedule>> {
  return request<TaktPagedResult<CycleSchedule>>({
    url: `${CYCLE_SCHEDULE_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取绩效周期日程
 * @param {string} id 绩效周期日程ID
 * @returns {Promise<CycleSchedule>} 绩效周期日程DTO
 */
export function getCycleScheduleById(id: string): Promise<CycleSchedule> {
  return request<CycleSchedule>({
    url: `${CYCLE_SCHEDULE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建绩效周期日程
 * @param {CycleScheduleCreate} dto 创建DTO
 * @returns {Promise<CycleSchedule>} 绩效周期日程DTO
 */
export function createCycleSchedule(dto: CycleScheduleCreate): Promise<CycleSchedule> {
  return request<CycleSchedule>({
    url: `${CYCLE_SCHEDULE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新绩效周期日程
 * @param {string} id 绩效周期日程ID
 * @param {CycleScheduleUpdate} dto 更新DTO
 * @returns {Promise<CycleSchedule>} 绩效周期日程DTO
 */
export function updateCycleSchedule(id: string, dto: CycleScheduleUpdate): Promise<CycleSchedule> {
  return request<CycleSchedule>({
    url: `${CYCLE_SCHEDULE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除绩效周期日程
 * @param {string} id 绩效周期日程ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCycleScheduleById(id: string): Promise<void> {
  return request({
    url: `${CYCLE_SCHEDULE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除绩效周期日程
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCycleScheduleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CYCLE_SCHEDULE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新绩效周期日程状态
 * @param {CycleScheduleStatus} dto 状态DTO
 * @returns {Promise<CycleSchedule>} 绩效周期日程DTO
 */
export function updateCycleScheduleStatus(dto: CycleScheduleStatus): Promise<CycleSchedule> {
  return request<CycleSchedule>({
    url: `${CYCLE_SCHEDULE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取绩效周期日程选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCycleScheduleOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CYCLE_SCHEDULE_API_BASE}/options`,
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
export function getCycleScheduleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CYCLE_SCHEDULE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入绩效周期日程
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCycleSchedule(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CYCLE_SCHEDULE_API_BASE}/import`,
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
 * 导出绩效周期日程
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCycleSchedule(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CYCLE_SCHEDULE_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
