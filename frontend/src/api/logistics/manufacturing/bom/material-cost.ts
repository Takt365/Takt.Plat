// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：material-cost.ts
// 创建时间：2026-07-14
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
  BomMaterialCost,
  BomMaterialCostCreate,
  BomMaterialCostModelGroup,
  BomMaterialCostUpdate
} from '@/types/logistics/manufacturing/bom/material-cost';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBomMaterialCosts
 */
const BOM_MATERIAL_COST_API_BASE = 'TaktBomMaterialCosts';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取BOM物料成本列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BomMaterialCost>>} 分页结果
 */
export function getBomMaterialCostList(queryDto: any): Promise<TaktPagedResult<BomMaterialCost>> {
  return request<TaktPagedResult<BomMaterialCost>>({
    url: `${BOM_MATERIAL_COST_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取机种维度汇总列表（分页；同表按工厂+机种+核算期间聚合）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BomMaterialCostModelGroup>>} 分页结果
 */
export function getBomMaterialCostModelGroupList(queryDto: any): Promise<TaktPagedResult<BomMaterialCostModelGroup>> {
  return request<TaktPagedResult<BomMaterialCostModelGroup>>({
    url: `${BOM_MATERIAL_COST_API_BASE}/model-group-list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取机种下拉选项（汇总表 ModelCode 去重；可按工厂过滤）
 * @param {string} [plantCode] 工厂代码
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBomMaterialCostModelOptions(plantCode?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${BOM_MATERIAL_COST_API_BASE}/model-options`,
    method: 'get',
    params: plantCode ? { plantCode } : undefined,
  });
}

/**
 * 根据ID获取BOM物料成本
 * @param {string} id BOM物料成本ID
 * @returns {Promise<BomMaterialCost>} BOM物料成本DTO
 */
export function getBomMaterialCostById(id: string): Promise<BomMaterialCost> {
  return request<BomMaterialCost>({
    url: `${BOM_MATERIAL_COST_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建BOM物料成本
 * @param {BomMaterialCostCreate} dto 创建DTO
 * @returns {Promise<BomMaterialCost>} BOM物料成本DTO
 */
export function createBomMaterialCost(dto: BomMaterialCostCreate): Promise<BomMaterialCost> {
  return request<BomMaterialCost>({
    url: `${BOM_MATERIAL_COST_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新BOM物料成本
 * @param {string} id BOM物料成本ID
 * @param {BomMaterialCostUpdate} dto 更新DTO
 * @returns {Promise<BomMaterialCost>} BOM物料成本DTO
 */
export function updateBomMaterialCost(id: string, dto: BomMaterialCostUpdate): Promise<BomMaterialCost> {
  return request<BomMaterialCost>({
    url: `${BOM_MATERIAL_COST_API_BASE}/{id:long}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除BOM物料成本
 * @param {string} id BOM物料成本ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBomMaterialCostById(id: string): Promise<void> {
  return request({
    url: `${BOM_MATERIAL_COST_API_BASE}/{id:long}`,
    method: 'delete',
    params: {
      id
    },
  });
}

/**
 * 批量删除BOM物料成本
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBomMaterialCostBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BOM_MATERIAL_COST_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取BOM物料成本选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBomMaterialCostOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${BOM_MATERIAL_COST_API_BASE}/options`,
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
export function getBomMaterialCostTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入BOM物料成本
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBomMaterialCost(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BOM_MATERIAL_COST_API_BASE}/import`,
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
 * 导出BOM物料成本
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBomMaterialCost(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
