// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：it-asset-change-log.ts
// 创建时间：2026-06-10
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
  ItAssetChangeLog,
  ItAssetChangeLogCreate,
  ItAssetChangeLogUpdate
} from '@/types/routine/help-desk/it-asset-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktItAssetChangeLogs
 */
const IT_ASSET_CHANGE_LOG_API_BASE = 'TaktItAssetChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取IT设备保修变更日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ItAssetChangeLog>>} 分页结果
 */
export function getItAssetChangeLogList(queryDto: any): Promise<TaktPagedResult<ItAssetChangeLog>> {
  return request<TaktPagedResult<ItAssetChangeLog>>({
    url: `${IT_ASSET_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取IT设备保修变更日志
 * @param {string} id IT设备保修变更日志ID
 * @returns {Promise<ItAssetChangeLog>} IT设备保修变更日志DTO
 */
export function getItAssetChangeLogById(id: string): Promise<ItAssetChangeLog> {
  return request<ItAssetChangeLog>({
    url: `${IT_ASSET_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建IT设备保修变更日志
 * @param {ItAssetChangeLogCreate} dto 创建DTO
 * @returns {Promise<ItAssetChangeLog>} IT设备保修变更日志DTO
 */
export function createItAssetChangeLog(dto: ItAssetChangeLogCreate): Promise<ItAssetChangeLog> {
  return request<ItAssetChangeLog>({
    url: `${IT_ASSET_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新IT设备保修变更日志
 * @param {string} id IT设备保修变更日志ID
 * @param {ItAssetChangeLogUpdate} dto 更新DTO
 * @returns {Promise<ItAssetChangeLog>} IT设备保修变更日志DTO
 */
export function updateItAssetChangeLog(id: string, dto: ItAssetChangeLogUpdate): Promise<ItAssetChangeLog> {
  return request<ItAssetChangeLog>({
    url: `${IT_ASSET_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除IT设备保修变更日志
 * @param {string} id IT设备保修变更日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteItAssetChangeLogById(id: string): Promise<void> {
  return request({
    url: `${IT_ASSET_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除IT设备保修变更日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteItAssetChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${IT_ASSET_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取IT设备保修变更日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getItAssetChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${IT_ASSET_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出IT设备保修变更日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportItAssetChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${IT_ASSET_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
