// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：material-moving-price.ts
// 创建时间：2026-07-16
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
  MaterialMovingPrice,
  MaterialMovingPriceCreate,
  MaterialMovingPriceUpdate
} from '@/types/logistics/materials/material-moving-price';
import type {
  MaterialMovingPriceMonthlyTrendQuery,
  MaterialMovingPriceMonthlyTrendResult,
  MaterialMovingPriceModelTrendResult
} from '@/types/logistics/materials/material-moving-trend';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialMovingPrices
 */
const MATERIAL_MOVING_PRICE_API_BASE = 'TaktMaterialMovingPrices';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取移动价格列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialMovingPrice>>} 分页结果
 */
export function getMaterialMovingPriceList(queryDto: any): Promise<TaktPagedResult<MaterialMovingPrice>> {
  return request<TaktPagedResult<MaterialMovingPrice>>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取移动价格
 * @param {string} id 移动价格ID
 * @returns {Promise<MaterialMovingPrice>} 移动价格DTO
 */
export function getMaterialMovingPriceById(id: string): Promise<MaterialMovingPrice> {
  return request<MaterialMovingPrice>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建移动价格
 * @param {MaterialMovingPriceCreate} dto 创建DTO
 * @returns {Promise<MaterialMovingPrice>} 移动价格DTO
 */
export function createMaterialMovingPrice(dto: MaterialMovingPriceCreate): Promise<MaterialMovingPrice> {
  return request<MaterialMovingPrice>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新移动价格
 * @param {string} id 移动价格ID
 * @param {MaterialMovingPriceUpdate} dto 更新DTO
 * @returns {Promise<MaterialMovingPrice>} 移动价格DTO
 */
export function updateMaterialMovingPrice(id: string, dto: MaterialMovingPriceUpdate): Promise<MaterialMovingPrice> {
  return request<MaterialMovingPrice>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除移动价格
 * @param {string} id 移动价格ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialMovingPriceById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除移动价格
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialMovingPriceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料移动价格选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialMovingPriceOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/options`,
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
export function getMaterialMovingPriceTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入移动价格
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaterialMovingPrice(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/import`,
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
 * 导出移动价格
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialMovingPrice(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

/**
 * 物料月移动价格推移分析
 * @param {MaterialMovingPriceMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<MaterialMovingPriceMonthlyTrendResult>} 转置结果
 */
export function getMaterialMovingPriceMonthlyTrendAnalysis(
  queryDto: MaterialMovingPriceMonthlyTrendQuery
): Promise<MaterialMovingPriceMonthlyTrendResult> {
  return request<MaterialMovingPriceMonthlyTrendResult>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出物料月移动价格推移分析
 * @param {MaterialMovingPriceMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportMaterialMovingPriceMonthlyTrendAnalysis(
  query: MaterialMovingPriceMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/monthly-trend-analysis/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}

/**
 * 物料-机种-价格推移分析
 * @param {MaterialMovingPriceMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<MaterialMovingPriceModelTrendResult>} 分析结果
 */
export function getMaterialMovingPriceModelTrendAnalysis(
  queryDto: MaterialMovingPriceMonthlyTrendQuery
): Promise<MaterialMovingPriceModelTrendResult> {
  return request<MaterialMovingPriceModelTrendResult>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/model-trend-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 导出物料-机种-价格推移分析
 * @param {MaterialMovingPriceMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportMaterialMovingPriceModelTrendAnalysis(
  query: MaterialMovingPriceMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_MOVING_PRICE_API_BASE}/model-trend-analysis/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
    timeout: 300000,
  });
}
