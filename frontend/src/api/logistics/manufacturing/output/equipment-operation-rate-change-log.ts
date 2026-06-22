// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：equipment-operation-rate-change-log.ts
// 创建时间：2026-06-21
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
  EquipmentOperationRateChangeLog,
  EquipmentOperationRateChangeLogCreate,
  EquipmentOperationRateChangeLogUpdate
} from '@/types/logistics/manufacturing/output/equipment-operation-rate-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEquipmentOperationRateChangeLogs
 */
const EQUIPMENT_OPERATION_RATE_CHANGE_LOG_API_BASE = 'TaktEquipmentOperationRateChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取机器稼动率变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EquipmentOperationRateChangeLog>>} 分页结果
 */
export function getEquipmentOperationRateChangeLogList(queryDto: any): Promise<TaktPagedResult<EquipmentOperationRateChangeLog>> {
  return request<TaktPagedResult<EquipmentOperationRateChangeLog>>({
    url: `${EQUIPMENT_OPERATION_RATE_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取机器稼动率变更记录
 * @param {string} id 机器稼动率变更记录ID
 * @returns {Promise<EquipmentOperationRateChangeLog>} 机器稼动率变更记录DTO
 */
export function getEquipmentOperationRateChangeLogById(id: string): Promise<EquipmentOperationRateChangeLog> {
  return request<EquipmentOperationRateChangeLog>({
    url: `${EQUIPMENT_OPERATION_RATE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建机器稼动率变更记录
 * @param {EquipmentOperationRateChangeLogCreate} dto 创建DTO
 * @returns {Promise<EquipmentOperationRateChangeLog>} 机器稼动率变更记录DTO
 */
export function createEquipmentOperationRateChangeLog(dto: EquipmentOperationRateChangeLogCreate): Promise<EquipmentOperationRateChangeLog> {
  return request<EquipmentOperationRateChangeLog>({
    url: `${EQUIPMENT_OPERATION_RATE_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新机器稼动率变更记录
 * @param {string} id 机器稼动率变更记录ID
 * @param {EquipmentOperationRateChangeLogUpdate} dto 更新DTO
 * @returns {Promise<EquipmentOperationRateChangeLog>} 机器稼动率变更记录DTO
 */
export function updateEquipmentOperationRateChangeLog(id: string, dto: EquipmentOperationRateChangeLogUpdate): Promise<EquipmentOperationRateChangeLog> {
  return request<EquipmentOperationRateChangeLog>({
    url: `${EQUIPMENT_OPERATION_RATE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除机器稼动率变更记录
 * @param {string} id 机器稼动率变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEquipmentOperationRateChangeLogById(id: string): Promise<void> {
  return request({
    url: `${EQUIPMENT_OPERATION_RATE_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除机器稼动率变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEquipmentOperationRateChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EQUIPMENT_OPERATION_RATE_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取机器稼动率变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEquipmentOperationRateChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EQUIPMENT_OPERATION_RATE_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出机器稼动率变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEquipmentOperationRateChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EQUIPMENT_OPERATION_RATE_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
