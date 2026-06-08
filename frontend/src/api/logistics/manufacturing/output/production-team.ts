// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：production-team.ts
// 创建时间：2026-06-08
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
  ProductionTeam,
  ProductionTeamCreate,
  ProductionTeamStatus,
  ProductionTeamUpdate
} from '@/types/logistics/manufacturing/output/production-team';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductionTeams
 */
const PRODUCTION_TEAM_API_BASE = 'TaktProductionTeams';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取生产班组列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductionTeam>>} 分页结果
 */
export function getProductionTeamList(queryDto: any): Promise<TaktPagedResult<ProductionTeam>> {
  return request<TaktPagedResult<ProductionTeam>>({
    url: `${PRODUCTION_TEAM_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取生产班组
 * @param {string} id 生产班组ID
 * @returns {Promise<ProductionTeam>} 生产班组DTO
 */
export function getProductionTeamById(id: string): Promise<ProductionTeam> {
  return request<ProductionTeam>({
    url: `${PRODUCTION_TEAM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建生产班组
 * @param {ProductionTeamCreate} dto 创建DTO
 * @returns {Promise<ProductionTeam>} 生产班组DTO
 */
export function createProductionTeam(dto: ProductionTeamCreate): Promise<ProductionTeam> {
  return request<ProductionTeam>({
    url: `${PRODUCTION_TEAM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新生产班组
 * @param {string} id 生产班组ID
 * @param {ProductionTeamUpdate} dto 更新DTO
 * @returns {Promise<ProductionTeam>} 生产班组DTO
 */
export function updateProductionTeam(id: string, dto: ProductionTeamUpdate): Promise<ProductionTeam> {
  return request<ProductionTeam>({
    url: `${PRODUCTION_TEAM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除生产班组
 * @param {string} id 生产班组ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionTeamById(id: string): Promise<void> {
  return request({
    url: `${PRODUCTION_TEAM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除生产班组
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionTeamBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCTION_TEAM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新生产班组状态
 * @param {ProductionTeamStatus} dto 状态 DTO（TaktCommonStatus 枚举）
 * @returns {Promise<ProductionTeam>} 生产班组DTO
 */
export function updateProductionTeamStatus(dto: ProductionTeamStatus): Promise<ProductionTeam> {
  return request<ProductionTeam>({
    url: `${PRODUCTION_TEAM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取生产班组选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductionTeamOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PRODUCTION_TEAM_API_BASE}/options`,
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
export function getProductionTeamTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_TEAM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入生产班组
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductionTeam(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCTION_TEAM_API_BASE}/import`,
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
 * 导出生产班组
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductionTeam(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_TEAM_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
