// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customer-complaint-trend.d.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：顾客投诉月度推移分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common';

/**
 * 顾客投诉月度推移查询
 * @description 对应后端 TaktCustomerComplaintMonthlyTrendQueryDto
 */
export interface CustomerComplaintMonthlyTrendQuery extends TaktPagedQuery {
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
  /** 投诉类型 */
  complaintType?: number;
  /** 投诉等级 */
  complaintLevel?: number;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 顾客投诉月度推移转置行
 * @description 对应后端 TaktCustomerComplaintMonthlyTrendDto
 */
export interface CustomerComplaintMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 客户编码 */
  customerCode: string;
  /** 客户名称 */
  customerName?: string;
  /** 各期间投诉件数 */
  periodValues?: Record<string, number>;
  /** 环比涨跌 */
  trend?: string;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 环比差额 */
  varianceAmount?: number | null;
  /** 环比变动率（小数比率） */
  variancePercent?: number | null;
}

/**
 * 顾客投诉月度推移分析结果
 * @description 对应后端 TaktCustomerComplaintMonthlyTrendResultDto
 */
export interface CustomerComplaintMonthlyTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<CustomerComplaintMonthlyTrend>;
  /** 期间列顺序 */
  periodOrder: string[];
  /** 客户行总数 */
  customerCount: number;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 上涨行数 */
  upCount: number;
  /** 下跌行数 */
  downCount: number;
  /** 持平行数 */
  flatCount: number;
  /** 无法比较行数 */
  noneCount: number;
}
