// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：monthly-trend.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月销售推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  SalesMonthlyTrendQuery,
  SalesMonthlyTrendResult,
} from '@/types/logistics/sales/monthly-trend';

/** API 路由前缀 */
const SALES_MONTHLY_TREND_API_BASE = 'TaktSalesMonthlyTrends';

/**
 * 月销售推移分析
 * @param {SalesMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<SalesMonthlyTrendResult>} 转置结果
 */
export function getSalesMonthlyTrendAnalysis(
  queryDto: SalesMonthlyTrendQuery
): Promise<SalesMonthlyTrendResult> {
  return request<SalesMonthlyTrendResult>({
    url: `${SALES_MONTHLY_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出月销售推移分析
 * @param {SalesMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportSalesMonthlyTrendAnalysis(
  query: SalesMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_MONTHLY_TREND_API_BASE}/monthly-trend-analysis/export`,
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
