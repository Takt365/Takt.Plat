// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：material-plant-change-log.ts
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
  MaterialPlantChangeLog,
  MaterialPlantChangeLogCreate,
  MaterialPlantChangeLogUpdate
} from '@/types/logistics/materials/material-plant-change-log';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialPlantChangeLogs
 */
const MATERIAL_PLANT_CHANGE_LOG_API_BASE = 'TaktMaterialPlantChangeLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工厂物料变更记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialPlantChangeLog>>} 分页结果
 */
export function getMaterialPlantChangeLogList(queryDto: any): Promise<TaktPagedResult<MaterialPlantChangeLog>> {
  return request<TaktPagedResult<MaterialPlantChangeLog>>({
    url: `${MATERIAL_PLANT_CHANGE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工厂物料变更记录
 * @param {string} id 工厂物料变更记录ID
 * @returns {Promise<MaterialPlantChangeLog>} 工厂物料变更记录DTO
 */
export function getMaterialPlantChangeLogById(id: string): Promise<MaterialPlantChangeLog> {
  return request<MaterialPlantChangeLog>({
    url: `${MATERIAL_PLANT_CHANGE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工厂物料变更记录
 * @param {MaterialPlantChangeLogCreate} dto 创建DTO
 * @returns {Promise<MaterialPlantChangeLog>} 工厂物料变更记录DTO
 */
export function createMaterialPlantChangeLog(dto: MaterialPlantChangeLogCreate): Promise<MaterialPlantChangeLog> {
  return request<MaterialPlantChangeLog>({
    url: `${MATERIAL_PLANT_CHANGE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工厂物料变更记录
 * @param {string} id 工厂物料变更记录ID
 * @param {MaterialPlantChangeLogUpdate} dto 更新DTO
 * @returns {Promise<MaterialPlantChangeLog>} 工厂物料变更记录DTO
 */
export function updateMaterialPlantChangeLog(id: string, dto: MaterialPlantChangeLogUpdate): Promise<MaterialPlantChangeLog> {
  return request<MaterialPlantChangeLog>({
    url: `${MATERIAL_PLANT_CHANGE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工厂物料变更记录
 * @param {string} id 工厂物料变更记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialPlantChangeLogById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_PLANT_CHANGE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工厂物料变更记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialPlantChangeLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_PLANT_CHANGE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工厂物料变更记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialPlantChangeLogOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_PLANT_CHANGE_LOG_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出工厂物料变更记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialPlantChangeLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_PLANT_CHANGE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
