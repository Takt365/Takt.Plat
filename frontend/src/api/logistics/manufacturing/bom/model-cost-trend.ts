// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：model-cost-trend.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  BomMaterialCostItemModelCostTrendQuery,
  BomMaterialCostItemModelCostTrendResult,
} from '@/types/logistics/manufacturing/bom/model-cost-trend';

/** API 路由前缀（对应 TaktBomModelCostTrendsController） */
const BOM_MODEL_COST_TREND_API_BASE = 'TaktBomModelCostTrends';

/**
 * 机种成本推移分析
 * @param {BomMaterialCostItemModelCostTrendQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemModelCostTrendResult>} 分析结果
 */
export function getBomMaterialCostItemModelCostTrendAnalysis(
  queryDto: BomMaterialCostItemModelCostTrendQuery
): Promise<BomMaterialCostItemModelCostTrendResult> {
  return request<BomMaterialCostItemModelCostTrendResult>({
    url: `${BOM_MODEL_COST_TREND_API_BASE}/model-cost-trend-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 机种选项 URL（分析：期间最后月本表机种去重；❌ 非 CRUD 主数据 TaktBomMaterialCosts/model-options）
 * @returns {string} 相对 API 路径
 */
export function getBomModelCostTrendModelOptionsUrl(): string {
  return `/${BOM_MODEL_COST_TREND_API_BASE}/model-options`;
}

/**
 * 物料/组件选项 URL（工厂 + 期间最后月；ProductionRelated=X 且 PurchaseType=F）
 * @returns {string} 相对 API 路径
 */
export function getBomModelCostTrendComponentOptionsUrl(): string {
  return `/${BOM_MODEL_COST_TREND_API_BASE}/component-options`;
}

/**
 * 导出机种成本推移分析
 * @param {BomMaterialCostItemModelCostTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemModelCostTrendAnalysis(
  query: BomMaterialCostItemModelCostTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MODEL_COST_TREND_API_BASE}/model-cost-trend-analysis/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    returnBinaryMeta: true,
    timeout: 300000,
  });
}
