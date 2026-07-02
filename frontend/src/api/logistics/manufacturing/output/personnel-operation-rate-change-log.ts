// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：personnel-operation-rate-change-log.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块 API（自动生成，请勿手改路由常量）
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
  PersonnelOperationRateChangeLog,
  PersonnelOperationRateChangeLogCreate,
  PersonnelOperationRateChangeLogUpdate
} from '@/types/logistics/manufacturing/output/personnel-operation-rate-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPersonnelOperationRateChangeLogs
 */
const PERSONNEL_OPERATION_RATE_CHANGE_LOG_API_BASE = 'TaktPersonnelOperationRateChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取人员稼动率变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PersonnelOperationRateChangeLog>>} 分页结果
 */
export function getPersonnelOperationRateChangeLogList(queryDto: any): Promise<TaktPagedResult<PersonnelOperationRateChangeLog>> {
  return request<TaktPagedResult<PersonnelOperationRateChangeLog>>({
    url: `${PERSONNEL_OPERATION_RATE_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取人员稼动率变更记录
 * @param {string} id 人员稼动率变更记录ID
 * @returns {Promise<PersonnelOperationRateChangeLog>} 人员稼动率变更记录DTO
 */
export function getPersonnelOperationRateChangeLogById(id: string): Promise<PersonnelOperationRateChangeLog> {
  return request<PersonnelOperationRateChangeLog>({
    url: `${PERSONNEL_OPERATION_RATE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建人员稼动率变更记录
 * @param {PersonnelOperationRateChangeLogCreate} dto 创建DTO
 * @returns {Promise<PersonnelOperationRateChangeLog>} 人员稼动率变更记录DTO
 */
export function createPersonnelOperationRateChangeLog(dto: PersonnelOperationRateChangeLogCreate): Promise<PersonnelOperationRateChangeLog> {
  return request<PersonnelOperationRateChangeLog>({
    url: `${PERSONNEL_OPERATION_RATE_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新人员稼动率变更记录
 * @param {string} id 人员稼动率变更记录ID
 * @param {PersonnelOperationRateChangeLogUpdate} dto 更新DTO
 * @returns {Promise<PersonnelOperationRateChangeLog>} 人员稼动率变更记录DTO
 */
export function updatePersonnelOperationRateChangeLog(id: string, dto: PersonnelOperationRateChangeLogUpdate): Promise<PersonnelOperationRateChangeLog> {
  return request<PersonnelOperationRateChangeLog>({
    url: `${PERSONNEL_OPERATION_RATE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除人员稼动率变更记录
 * @param {string} id 人员稼动率变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePersonnelOperationRateChangeLogById(id: string): Promise<void> {
  return request({
    url: `${PERSONNEL_OPERATION_RATE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除人员稼动率变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePersonnelOperationRateChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PERSONNEL_OPERATION_RATE_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取人员稼动率变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPersonnelOperationRateChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PERSONNEL_OPERATION_RATE_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出人员稼动率变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPersonnelOperationRateChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PERSONNEL_OPERATION_RATE_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
