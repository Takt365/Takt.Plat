// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：online.ts
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
  Online,
  OnlineCreate,
  OnlineQuery,
  OnlineStatistics,
  OnlineStatus,
  OnlineUpdate,
} from '@/types/foundation/online';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktOnline
 */
const ONLINE_API_BASE = 'TaktOnlines';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取在线用户列表（分页）
 * @param {OnlineQuery} queryDto 查询参数
 * @returns {Promise<TaktPagedResult<Online>>} 分页结果
 */
export function getOnlineList(queryDto: OnlineQuery): Promise<TaktPagedResult<Online>> {
  return request<TaktPagedResult<Online>>({
    url: `${ONLINE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取在线用户
 * @param {string} id 在线用户ID
 * @returns {Promise<Online>} 在线用户
 */
export function getOnlineById(id: string): Promise<Online> {
  return request<Online>({
    url: `${ONLINE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建在线用户
 * @param {OnlineCreate} dto 创建参数
 * @returns {Promise<Online>} 在线用户
 */
export function createOnline(dto: OnlineCreate): Promise<Online> {
  return request<Online>({
    url: `${ONLINE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新在线用户
 * @param {string} id 在线用户ID
 * @param {OnlineUpdate} dto 更新参数
 * @returns {Promise<Online>} 在线用户
 */
export function updateOnline(id: string, dto: OnlineUpdate): Promise<Online> {
  return request<Online>({
    url: `${ONLINE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除在线用户
 * @param {string} id 在线用户ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteOnlineById(id: string): Promise<void> {
  return request({
    url: `${ONLINE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除在线用户
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteOnlineBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ONLINE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新在线用户状态
 * @param {OnlineStatus} dto 状态参数
 * @returns {Promise<Online>} 在线用户
 */
export function updateOnlineStatus(dto: OnlineStatus): Promise<Online> {
  return request<Online>({
    url: `${ONLINE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取在线用户选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getOnlineOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ONLINE_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 统计
// ========================================

/**
 * 获取在线用户统计
 * @returns {Promise<OnlineStatistics>} 统计结果
 */
export function getOnlineStatistics(): Promise<OnlineStatistics> {
  return request<OnlineStatistics>({
    url: `${ONLINE_API_BASE}/statistics`,
    method: 'get',
  });
}

// ========================================
// 导出
// ========================================

/**
 * 导出在线用户
 * @param {OnlineQuery} queryDto 查询参数
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportOnline(
  queryDto?: OnlineQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ONLINE_API_BASE}/export`,
    method: 'get',
    params: {
      ...queryDto,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
