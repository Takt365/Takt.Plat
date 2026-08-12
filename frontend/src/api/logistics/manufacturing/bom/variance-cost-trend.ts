// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：variance-cost-trend.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 差异成本推移分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type {
  BomVarianceCostTrendQuery,
  BomVarianceCostTrendResult,
} from '@/types/logistics/manufacturing/bom/variance-cost-trend'

/** API 路由前缀（对应 TaktBomVarianceCostTrendsController） */
const BOM_VARIANCE_COST_TREND_API_BASE = 'TaktBomVarianceCostTrends'

/**
 * 差异成本推移分析
 * @param {BomVarianceCostTrendQuery} queryDto 查询条件
 * @returns {Promise<BomVarianceCostTrendResult>} 分析结果
 */
export function getBomVarianceCostTrendAnalysis(
  queryDto: BomVarianceCostTrendQuery,
): Promise<BomVarianceCostTrendResult> {
  return request<BomVarianceCostTrendResult>({
    url: `${BOM_VARIANCE_COST_TREND_API_BASE}/variance-cost-trend-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  })
}

/**
 * 机种选项 URL
 * @returns {string} 相对 API 路径
 */
export function getBomVarianceCostTrendModelOptionsUrl(): string {
  return `/${BOM_VARIANCE_COST_TREND_API_BASE}/model-options`
}

/**
 * 产品选项 URL（须带已选机种，与机种联动）
 * @returns {string} 相对 API 路径
 */
export function getBomVarianceCostTrendProductOptionsUrl(): string {
  return `/${BOM_VARIANCE_COST_TREND_API_BASE}/product-options`
}

/**
 * 导出差异成本推移
 * @param {BomVarianceCostTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomVarianceCostTrendAnalysis(
  query: BomVarianceCostTrendQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_VARIANCE_COST_TREND_API_BASE}/variance-cost-trend-analysis/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    returnBinaryMeta: true,
    timeout: 300000,
  })
}
