// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：production-order-change-log.ts
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
  ProductionOrderChangeLog,
  ProductionOrderChangeLogCreate,
  ProductionOrderChangeLogUpdate
} from '@/types/logistics/manufacturing/output/production-order-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductionOrderChangeLogs
 */
const PRODUCTION_ORDER_CHANGE_LOG_API_BASE = 'TaktProductionOrderChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取生产工单变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductionOrderChangeLog>>} 分页结果
 */
export function getProductionOrderChangeLogList(queryDto: any): Promise<TaktPagedResult<ProductionOrderChangeLog>> {
  return request<TaktPagedResult<ProductionOrderChangeLog>>({
    url: `${PRODUCTION_ORDER_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取生产工单变更记录
 * @param {string} id 生产工单变更记录ID
 * @returns {Promise<ProductionOrderChangeLog>} 生产工单变更记录DTO
 */
export function getProductionOrderChangeLogById(id: string): Promise<ProductionOrderChangeLog> {
  return request<ProductionOrderChangeLog>({
    url: `${PRODUCTION_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建生产工单变更记录
 * @param {ProductionOrderChangeLogCreate} dto 创建DTO
 * @returns {Promise<ProductionOrderChangeLog>} 生产工单变更记录DTO
 */
export function createProductionOrderChangeLog(dto: ProductionOrderChangeLogCreate): Promise<ProductionOrderChangeLog> {
  return request<ProductionOrderChangeLog>({
    url: `${PRODUCTION_ORDER_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新生产工单变更记录
 * @param {string} id 生产工单变更记录ID
 * @param {ProductionOrderChangeLogUpdate} dto 更新DTO
 * @returns {Promise<ProductionOrderChangeLog>} 生产工单变更记录DTO
 */
export function updateProductionOrderChangeLog(id: string, dto: ProductionOrderChangeLogUpdate): Promise<ProductionOrderChangeLog> {
  return request<ProductionOrderChangeLog>({
    url: `${PRODUCTION_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除生产工单变更记录
 * @param {string} id 生产工单变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionOrderChangeLogById(id: string): Promise<void> {
  return request({
    url: `${PRODUCTION_ORDER_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除生产工单变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionOrderChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCTION_ORDER_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取生产工单变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductionOrderChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PRODUCTION_ORDER_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出生产工单变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductionOrderChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_ORDER_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
