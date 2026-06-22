// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：material-change-log.ts
// 创建时间：2026-06-21
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块 API（自动生成，请勿手改路由常量）
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
  MaterialChangeLog,
  MaterialChangeLogCreate,
  MaterialChangeLogUpdate
} from '@/types/logistics/materials/material-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialChangeLogs
 */
const MATERIAL_CHANGE_LOG_API_BASE = 'TaktMaterialChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取全局物料变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialChangeLog>>} 分页结果
 */
export function getMaterialChangeLogList(queryDto: any): Promise<TaktPagedResult<MaterialChangeLog>> {
  return request<TaktPagedResult<MaterialChangeLog>>({
    url: `${MATERIAL_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取全局物料变更记录
 * @param {string} id 全局物料变更记录ID
 * @returns {Promise<MaterialChangeLog>} 全局物料变更记录DTO
 */
export function getMaterialChangeLogById(id: string): Promise<MaterialChangeLog> {
  return request<MaterialChangeLog>({
    url: `${MATERIAL_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建全局物料变更记录
 * @param {MaterialChangeLogCreate} dto 创建DTO
 * @returns {Promise<MaterialChangeLog>} 全局物料变更记录DTO
 */
export function createMaterialChangeLog(dto: MaterialChangeLogCreate): Promise<MaterialChangeLog> {
  return request<MaterialChangeLog>({
    url: `${MATERIAL_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新全局物料变更记录
 * @param {string} id 全局物料变更记录ID
 * @param {MaterialChangeLogUpdate} dto 更新DTO
 * @returns {Promise<MaterialChangeLog>} 全局物料变更记录DTO
 */
export function updateMaterialChangeLog(id: string, dto: MaterialChangeLogUpdate): Promise<MaterialChangeLog> {
  return request<MaterialChangeLog>({
    url: `${MATERIAL_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除全局物料变更记录
 * @param {string} id 全局物料变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialChangeLogById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除全局物料变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取全局物料变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出全局物料变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
