// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：bill-of-material-change-log.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块 API（自动生成，请勿手改路由常量）
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
  BillOfMaterialChangeLog,
  BillOfMaterialChangeLogCreate,
  BillOfMaterialChangeLogUpdate
} from '@/types/logistics/manufacturing/bom/bill-of-material-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBillOfMaterialChangeLogs
 */
const BILL_OF_MATERIAL_CHANGE_LOG_API_BASE = 'TaktBillOfMaterialChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取BOM变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BillOfMaterialChangeLog>>} 分页结果
 */
export function getBillOfMaterialChangeLogList(queryDto: any): Promise<TaktPagedResult<BillOfMaterialChangeLog>> {
  return request<TaktPagedResult<BillOfMaterialChangeLog>>({
    url: `${BILL_OF_MATERIAL_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取BOM变更记录
 * @param {string} id BOM变更记录ID
 * @returns {Promise<BillOfMaterialChangeLog>} BOM变更记录DTO
 */
export function getBillOfMaterialChangeLogById(id: string): Promise<BillOfMaterialChangeLog> {
  return request<BillOfMaterialChangeLog>({
    url: `${BILL_OF_MATERIAL_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建BOM变更记录
 * @param {BillOfMaterialChangeLogCreate} dto 创建DTO
 * @returns {Promise<BillOfMaterialChangeLog>} BOM变更记录DTO
 */
export function createBillOfMaterialChangeLog(dto: BillOfMaterialChangeLogCreate): Promise<BillOfMaterialChangeLog> {
  return request<BillOfMaterialChangeLog>({
    url: `${BILL_OF_MATERIAL_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新BOM变更记录
 * @param {string} id BOM变更记录ID
 * @param {BillOfMaterialChangeLogUpdate} dto 更新DTO
 * @returns {Promise<BillOfMaterialChangeLog>} BOM变更记录DTO
 */
export function updateBillOfMaterialChangeLog(id: string, dto: BillOfMaterialChangeLogUpdate): Promise<BillOfMaterialChangeLog> {
  return request<BillOfMaterialChangeLog>({
    url: `${BILL_OF_MATERIAL_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除BOM变更记录
 * @param {string} id BOM变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBillOfMaterialChangeLogById(id: string): Promise<void> {
  return request({
    url: `${BILL_OF_MATERIAL_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除BOM变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBillOfMaterialChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BILL_OF_MATERIAL_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取BOM变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBillOfMaterialChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${BILL_OF_MATERIAL_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出BOM变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBillOfMaterialChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BILL_OF_MATERIAL_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
