// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/statistics/report
// 文件名称：configurable.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/report 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktSelectOption,
  TaktBinaryDownload
} from '@/types/common';
import type {
  Configurable,
  ConfigurableCreate,
  ConfigurableSort,
  ConfigurableStatus,
  ConfigurableUpdate,
  ConfigurableRuntimeScreen,
  ConfigurableExecuteQuery,
  ConfigurableQueryResult,
  ConfigurableExportData
} from '@/types/statistics/report/configurable';
import type {
  DatabaseInfo,
  DatabaseTableColumnInfo,
  DatabaseTableInfo
} from '@/types/code/database/database-info';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktConfigurables
 */
const CONFIGURABLE_API_BASE = 'TaktConfigurables';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取自定义报表主列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Configurable>>} 分页结果
 */
export function getConfigurableList(queryDto: any): Promise<TaktPagedResult<Configurable>> {
  return request<TaktPagedResult<Configurable>>({
    url: `${CONFIGURABLE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取自定义报表主
 * @param {string} id 自定义报表主ID
 * @returns {Promise<Configurable>} 自定义报表主DTO
 */
export function getConfigurableById(id: string): Promise<Configurable> {
  return request<Configurable>({
    url: `${CONFIGURABLE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建自定义报表主
 * @param {ConfigurableCreate} dto 创建DTO
 * @returns {Promise<Configurable>} 自定义报表主DTO
 */
export function createConfigurable(dto: ConfigurableCreate): Promise<Configurable> {
  return request<Configurable>({
    url: `${CONFIGURABLE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新自定义报表主
 * @param {string} id 自定义报表主ID
 * @param {ConfigurableUpdate} dto 更新DTO
 * @returns {Promise<Configurable>} 自定义报表主DTO
 */
export function updateConfigurable(id: string, dto: ConfigurableUpdate): Promise<Configurable> {
  return request<Configurable>({
    url: `${CONFIGURABLE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除自定义报表主
 * @param {string} id 自定义报表主ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteConfigurableById(id: string): Promise<void> {
  return request({
    url: `${CONFIGURABLE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除自定义报表主
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteConfigurableBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CONFIGURABLE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新自定义报表主状态
 * @param {ConfigurableStatus} dto 状态 DTO（TaktCommonStatus 枚举）
 * @returns {Promise<Configurable>} 自定义报表主DTO
 */
export function updateConfigurableStatus(dto: ConfigurableStatus): Promise<Configurable> {
  return request<Configurable>({
    url: `${CONFIGURABLE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新自定义报表主排序
 * @param {ConfigurableSort} dto 排序DTO
 * @returns {Promise<Configurable>} 自定义报表主DTO
 */
export function updateConfigurableSort(dto: ConfigurableSort): Promise<Configurable> {
  return request<Configurable>({
    url: `${CONFIGURABLE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取报表下拉选项
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getConfigurableOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CONFIGURABLE_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName sheetName
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getConfigurableTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CONFIGURABLE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入自定义报表主
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importConfigurable(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CONFIGURABLE_API_BASE}/import`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    params: {
      sheetName
    },
  });
}

/**
 * 导出自定义报表主
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportConfigurable(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CONFIGURABLE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// Schema 选库选表（报表设计）
// ========================================

/**
 * 获取可选租户业务库列表
 * @returns {Promise<DatabaseInfo[]>} 数据库摘要列表
 */
export function getConfigurableSchemaDatabases(): Promise<DatabaseInfo[]> {
  return request<DatabaseInfo[]>({
    url: `${CONFIGURABLE_API_BASE}/schema/databases`,
    method: 'get',
  });
}

/**
 * 获取指定租户库物理表列表
 * @param {string} tenantCode 租户编码
 * @returns {Promise<DatabaseTableInfo[]>} 表摘要列表
 */
export function getConfigurableSchemaTables(tenantCode: string): Promise<DatabaseTableInfo[]> {
  return request<DatabaseTableInfo[]>({
    url: `${CONFIGURABLE_API_BASE}/schema/tables`,
    method: 'get',
    params: {
      tenantCode,
    },
  });
}

/**
 * 获取指定物理表列列表
 * @param {string} tenantCode 租户编码
 * @param {string} tableName 表名
 * @returns {Promise<DatabaseTableColumnInfo[]>} 列摘要列表
 */
export function getConfigurableSchemaColumns(
  tenantCode: string,
  tableName: string
): Promise<DatabaseTableColumnInfo[]> {
  return request<DatabaseTableColumnInfo[]>({
    url: `${CONFIGURABLE_API_BASE}/schema/columns`,
    method: 'get',
    params: {
      tenantCode,
      tableName,
    },
  });
}

// ========================================
// SQVI 运行时
// ========================================

/**
 * 获取 SQVI 运行时筛选条件
 * @param {string} id 报表主键
 * @returns {Promise<ConfigurableRuntimeScreen>} 运行时屏幕定义
 */
export function getConfigurableRuntimeScreen(id: string): Promise<ConfigurableRuntimeScreen> {
  return request<ConfigurableRuntimeScreen>({
    url: `${CONFIGURABLE_API_BASE}/${id}/runtime-screen`,
    method: 'get',
  });
}

/**
 * 执行报表查询（分页）
 * @param {string} id 报表主键
 * @param {ConfigurableExecuteQuery} dto 查询参数
 * @returns {Promise<ConfigurableQueryResult>} 查询结果
 */
export function executeConfigurableQuery(
  id: string,
  dto: ConfigurableExecuteQuery
): Promise<ConfigurableQueryResult> {
  return request<ConfigurableQueryResult>({
    url: `${CONFIGURABLE_API_BASE}/${id}/query`,
    method: 'post',
    data: dto,
  });
}

/**
 * 导出报表数据（Excel）
 * @param {string} id 报表主键
 * @param {ConfigurableExportData} dto 筛选值
 * @param {string} sheetName 工作表名
 * @param {string} exportName 文件名
 * @returns {Promise<TaktBinaryDownload>} Excel 文件
 */
export function exportConfigurableData(
  id: string,
  dto: ConfigurableExportData,
  sheetName?: string,
  exportName?: string
): Promise<TaktBinaryDownload> {
  return request<TaktBinaryDownload>({
    url: `${CONFIGURABLE_API_BASE}/${id}/export-data`,
    method: 'post',
    data: dto,
    params: {
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}
