// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：material-model-trend.ts
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料机种推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  MaterialModelTrendQuery,
  MaterialModelTrendResult,
} from '@/types/logistics/materials/material-model-trend';

/** API 路由前缀（对应 TaktMaterialModelTrendsController） */
const MATERIAL_MODEL_TREND_API_BASE = 'TaktMaterialModelTrends';

/**
 * 推移查询栏：本表工厂去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMaterialModelTrendPlantOptionsUrl(): string {
  return `${MATERIAL_MODEL_TREND_API_BASE}/plant-options`;
}

/**
 * 推移查询栏：评估类别去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMaterialModelTrendValuationOptionsUrl(): string {
  return `${MATERIAL_MODEL_TREND_API_BASE}/valuation-options`;
}

/**
 * 推移查询栏：物料去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMaterialModelTrendMaterialOptionsUrl(): string {
  return `${MATERIAL_MODEL_TREND_API_BASE}/material-options`;
}

/**
 * 物料机种推移转置分析
 * @param {MaterialModelTrendQuery} queryDto 查询条件
 * @returns {Promise<MaterialModelTrendResult>} 转置结果
 */
export function getMaterialModelTrendAnalysis(
  queryDto: MaterialModelTrendQuery
): Promise<MaterialModelTrendResult> {
  return request<MaterialModelTrendResult>({
    url: `${MATERIAL_MODEL_TREND_API_BASE}/trend-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 导出物料机种推移转置分析
 * @param {MaterialModelTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportMaterialModelTrendAnalysis(
  query: MaterialModelTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_MODEL_TREND_API_BASE}/trend-analysis/export`,
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
