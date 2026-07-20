// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：monthly-trend.d.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月销售推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common';

/**
 * 月销售推移查询
 * @description 对应后端 TaktSalesMonthlyTrendQueryDto
 */
export interface SalesMonthlyTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 客户编码 */
  customerCode?: string;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 月销售推移转置行
 * @description 对应后端 TaktSalesMonthlyTrendDto；periodAmounts 单位元
 */
export interface SalesMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 客户编码 */
  customerCode: string;
  /** 客户名称 */
  customerName: string;
  /** 各期间销售金额（元） */
  periodAmounts?: Record<string, number>;
  /** 环比涨跌 */
  trend?: string;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 环比差额（元） */
  varianceAmount?: number | null;
  /** 环比变动率（小数比率） */
  variancePercent?: number | null;
}

/**
 * 月销售推移分析结果
 * @description 对应后端 TaktSalesMonthlyTrendResultDto
 */
export interface SalesMonthlyTrendResult {
  /** 分页行 */
  paged?: TaktPagedResult<SalesMonthlyTrend>;
  /** 期间列顺序 */
  periodOrder?: string[];
  /** 客户行总数 */
  customerCount?: number;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 上涨行数 */
  upCount?: number;
  /** 下跌行数 */
  downCount?: number;
  /** 持平行数 */
  flatCount?: number;
  /** 无法比较行数 */
  noneCount?: number;
}
