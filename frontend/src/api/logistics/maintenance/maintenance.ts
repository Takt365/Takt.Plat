// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/maintenance
// 文件名称：maintenance.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块 API（自动生成，请勿手改路由常量）
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
  Maintenance,
  MaintenanceCreate,
  MaintenanceStatus,
  MaintenanceUpdate
} from '@/types/logistics/maintenance/maintenance';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaintenances
 */
const MAINTENANCE_API_BASE = 'TaktMaintenances';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设备维护记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Maintenance>>} 分页结果
 */
export function getMaintenanceList(queryDto: any): Promise<TaktPagedResult<Maintenance>> {
  return request<TaktPagedResult<Maintenance>>({
    url: `${MAINTENANCE_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取设备维护记录
 * @param {string} id 设备维护记录ID
 * @returns {Promise<Maintenance>} 设备维护记录DTO
 */
export function getMaintenanceById(id: string): Promise<Maintenance> {
  return request<Maintenance>({
    url: `${MAINTENANCE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设备维护记录
 * @param {MaintenanceCreate} dto 创建DTO
 * @returns {Promise<Maintenance>} 设备维护记录DTO
 */
export function createMaintenance(dto: MaintenanceCreate): Promise<Maintenance> {
  return request<Maintenance>({
    url: `${MAINTENANCE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设备维护记录
 * @param {string} id 设备维护记录ID
 * @param {MaintenanceUpdate} dto 更新DTO
 * @returns {Promise<Maintenance>} 设备维护记录DTO
 */
export function updateMaintenance(id: string, dto: MaintenanceUpdate): Promise<Maintenance> {
  return request<Maintenance>({
    url: `${MAINTENANCE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设备维护记录
 * @param {string} id 设备维护记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaintenanceById(id: string): Promise<void> {
  return request({
    url: `${MAINTENANCE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设备维护记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaintenanceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MAINTENANCE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设备维护记录状态
 * @param {MaintenanceStatus} dto 状态DTO
 * @returns {Promise<Maintenance>} 设备维护记录DTO
 */
export function updateMaintenanceStatus(dto: MaintenanceStatus): Promise<Maintenance> {
  return request<Maintenance>({
    url: `${MAINTENANCE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设备维护记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaintenanceOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MAINTENANCE_API_BASE}/options`,
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
export function getMaintenanceTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MAINTENANCE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设备维护记录
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaintenance(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MAINTENANCE_API_BASE}/import`,
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
 * 导出设备维护记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaintenance(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MAINTENANCE_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
