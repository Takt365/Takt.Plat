// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：setting.ts
// 创建时间：2026-06-09
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
  Setting,
  SettingCreate,
  SettingSort,
  SettingUpdate
} from '@/types/foundation/setting';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSettings
 */
const SETTING_API_BASE = 'TaktSettings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取系统设置列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Setting>>} 分页结果
 */
export function getSettingList(queryDto: any): Promise<TaktPagedResult<Setting>> {
  return request<TaktPagedResult<Setting>>({
    url: `${SETTING_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取系统设置
 * @param {string} id 系统设置ID
 * @returns {Promise<Setting>} 系统设置DTO
 */
export function getSettingById(id: string): Promise<Setting> {
  return request<Setting>({
    url: `${SETTING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建系统设置
 * @param {SettingCreate} dto 创建DTO
 * @returns {Promise<Setting>} 系统设置DTO
 */
export function createSetting(dto: SettingCreate): Promise<Setting> {
  return request<Setting>({
    url: `${SETTING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新系统设置
 * @param {string} id 系统设置ID
 * @param {SettingUpdate} dto 更新DTO
 * @returns {Promise<Setting>} 系统设置DTO
 */
export function updateSetting(id: string, dto: SettingUpdate): Promise<Setting> {
  return request<Setting>({
    url: `${SETTING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除系统设置
 * @param {string} id 系统设置ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSettingById(id: string): Promise<void> {
  return request({
    url: `${SETTING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除系统设置
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSettingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SETTING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新系统设置排序
 * @param {SettingSort} dto 排序DTO
 * @returns {Promise<Setting>} 系统设置DTO
 */
export function updateSettingSort(dto: SettingSort): Promise<Setting> {
  return request<Setting>({
    url: `${SETTING_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取系统设置选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSettingOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SETTING_API_BASE}/options`,
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
export function getSettingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SETTING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入系统设置
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSetting(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SETTING_API_BASE}/import`,
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
 * 导出系统设置
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSetting(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SETTING_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
