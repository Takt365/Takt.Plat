// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：material-moving-trend.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料月移动价格推移 / 机种推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  MaterialMovingPriceModelTrendResult,
  MaterialMovingPriceMonthlyTrendQuery,
  MaterialMovingPriceMonthlyTrendResult,
} from '@/types/logistics/materials/material-moving-trend';

/** API 路由前缀（对应 TaktMaterialMovingPriceTrendsController） */
const MATERIAL_MOVING_TREND_API_BASE = 'TaktMaterialMovingPriceTrends';

/**
 * 推移查询栏：本表工厂去重选项 URL（供 TaktSelect api-url）
 * @returns {string} 相对 API 路径
 */
export function getMaterialMovingPriceTrendPlantOptionsUrl(): string {
  return `${MATERIAL_MOVING_TREND_API_BASE}/plant-options`;
}

/**
 * 推移查询栏：评估类别去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMaterialMovingPriceTrendValuationOptionsUrl(): string {
  return `${MATERIAL_MOVING_TREND_API_BASE}/valuation-options`;
}

/**
 * 推移查询栏：物料去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMaterialMovingPriceTrendMaterialOptionsUrl(): string {
  return `${MATERIAL_MOVING_TREND_API_BASE}/material-options`;
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
    url: `${MATERIAL_MOVING_TREND_API_BASE}/monthly-trend-analysis`,
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
    url: `${MATERIAL_MOVING_TREND_API_BASE}/monthly-trend-analysis/export`,
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
    url: `${MATERIAL_MOVING_TREND_API_BASE}/model-trend-analysis`,
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
    url: `${MATERIAL_MOVING_TREND_API_BASE}/model-trend-analysis/export`,
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
