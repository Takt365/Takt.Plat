// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：asset-change-log.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块 API（自动生成，请勿手改路由常量）
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
  AssetChangeLog,
  AssetChangeLogCreate,
  AssetChangeLogUpdate
} from '@/types/accounting/financial/asset-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAssetChangeLogs
 */
const ASSET_CHANGE_LOG_API_BASE = 'TaktAssetChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取资产变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<AssetChangeLog>>} 分页结果
 */
export function getAssetChangeLogList(queryDto: any): Promise<TaktPagedResult<AssetChangeLog>> {
  return request<TaktPagedResult<AssetChangeLog>>({
    url: `${ASSET_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取资产变更记录
 * @param {string} id 资产变更记录ID
 * @returns {Promise<AssetChangeLog>} 资产变更记录DTO
 */
export function getAssetChangeLogById(id: string): Promise<AssetChangeLog> {
  return request<AssetChangeLog>({
    url: `${ASSET_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建资产变更记录
 * @param {AssetChangeLogCreate} dto 创建DTO
 * @returns {Promise<AssetChangeLog>} 资产变更记录DTO
 */
export function createAssetChangeLog(dto: AssetChangeLogCreate): Promise<AssetChangeLog> {
  return request<AssetChangeLog>({
    url: `${ASSET_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新资产变更记录
 * @param {string} id 资产变更记录ID
 * @param {AssetChangeLogUpdate} dto 更新DTO
 * @returns {Promise<AssetChangeLog>} 资产变更记录DTO
 */
export function updateAssetChangeLog(id: string, dto: AssetChangeLogUpdate): Promise<AssetChangeLog> {
  return request<AssetChangeLog>({
    url: `${ASSET_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除资产变更记录
 * @param {string} id 资产变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssetChangeLogById(id: string): Promise<void> {
  return request({
    url: `${ASSET_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除资产变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssetChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ASSET_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取资产变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAssetChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ASSET_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出资产变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAssetChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ASSET_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
