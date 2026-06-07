// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：account-title-change-log.ts
// 创建时间：2026-06-07
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
  AccountTitleChangeLog,
  AccountTitleChangeLogCreate,
  AccountTitleChangeLogUpdate
} from '@/types/accounting/financial/account-title-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAccountTitleChangeLogs
 */
const ACCOUNT_TITLE_CHANGE_LOG_API_BASE = 'TaktAccountTitleChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会计科目变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<AccountTitleChangeLog>>} 分页结果
 */
export function getAccountTitleChangeLogList(queryDto: any): Promise<TaktPagedResult<AccountTitleChangeLog>> {
  return request<TaktPagedResult<AccountTitleChangeLog>>({
    url: `${ACCOUNT_TITLE_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取会计科目变更记录
 * @param {string} id 会计科目变更记录ID
 * @returns {Promise<AccountTitleChangeLog>} 会计科目变更记录DTO
 */
export function getAccountTitleChangeLogById(id: string): Promise<AccountTitleChangeLog> {
  return request<AccountTitleChangeLog>({
    url: `${ACCOUNT_TITLE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建会计科目变更记录
 * @param {AccountTitleChangeLogCreate} dto 创建DTO
 * @returns {Promise<AccountTitleChangeLog>} 会计科目变更记录DTO
 */
export function createAccountTitleChangeLog(dto: AccountTitleChangeLogCreate): Promise<AccountTitleChangeLog> {
  return request<AccountTitleChangeLog>({
    url: `${ACCOUNT_TITLE_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会计科目变更记录
 * @param {string} id 会计科目变更记录ID
 * @param {AccountTitleChangeLogUpdate} dto 更新DTO
 * @returns {Promise<AccountTitleChangeLog>} 会计科目变更记录DTO
 */
export function updateAccountTitleChangeLog(id: string, dto: AccountTitleChangeLogUpdate): Promise<AccountTitleChangeLog> {
  return request<AccountTitleChangeLog>({
    url: `${ACCOUNT_TITLE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会计科目变更记录
 * @param {string} id 会计科目变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAccountTitleChangeLogById(id: string): Promise<void> {
  return request({
    url: `${ACCOUNT_TITLE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会计科目变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAccountTitleChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ACCOUNT_TITLE_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会计科目变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAccountTitleChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ACCOUNT_TITLE_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出会计科目变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAccountTitleChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ACCOUNT_TITLE_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
