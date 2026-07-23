// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：price-trend.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格月推移 / 机种推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  SalesPriceModelTrendResult,
  SalesPriceMonthlyTrendQuery,
  SalesPriceMonthlyTrendResult,
} from '@/types/logistics/sales/price-trend';

/** API 路由前缀（对应 TaktSalesPriceTrendsController） */
const SALES_PRICE_TREND_API_BASE = 'TaktSalesPriceTrends';

/**
 * 销售价格月推移转置分析
 * @param {SalesPriceMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<SalesPriceMonthlyTrendResult>} 转置结果
 */
export function getSalesPriceMonthlyTrendAnalysis(
  queryDto: SalesPriceMonthlyTrendQuery
): Promise<SalesPriceMonthlyTrendResult> {
  return request<SalesPriceMonthlyTrendResult>({
    url: `${SALES_PRICE_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出销售价格月推移转置分析
 * @param {SalesPriceMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportSalesPriceMonthlyTrendAnalysis(
  query: SalesPriceMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_TREND_API_BASE}/monthly-trend-analysis/export`,
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
 * 销售机种价格推移分析
 * @param {SalesPriceMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<SalesPriceModelTrendResult>} 分析结果
 */
export function getSalesPriceModelTrendAnalysis(
  queryDto: SalesPriceMonthlyTrendQuery
): Promise<SalesPriceModelTrendResult> {
  return request<SalesPriceModelTrendResult>({
    url: `${SALES_PRICE_TREND_API_BASE}/model-trend-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 导出销售机种价格推移分析
 * @param {SalesPriceMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportSalesPriceModelTrendAnalysis(
  query: SalesPriceMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_TREND_API_BASE}/model-trend-analysis/export`,
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
