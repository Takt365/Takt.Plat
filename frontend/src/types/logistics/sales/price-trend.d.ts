// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：price-trend.d.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格月推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common';

/**
 * 销售价格月推移查询
 * @description 对应后端 TaktSalesPriceMonthlyTrendQueryDto
 */
export interface SalesPriceMonthlyTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 物料编码（模糊） */
  materialCode?: string;
  /** 客户编码 */
  customerCode?: string;
  /** 价格类型 */
  priceType?: number;
  /** 仅启用主表 */
  onlyEnabled?: boolean;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 销售价格月推移转置行
 * @description 对应后端 TaktSalesPriceMonthlyTrendDto
 */
export interface SalesPriceMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 物料编码 */
  materialCode: string;
  /** 物料名称 */
  materialName?: string;
  /** 客户编码（空串表示通用价） */
  customerCode: string;
  /** 客户名称 */
  customerName?: string;
  /** 币种 */
  currency?: string;
  /** 销售单位 */
  unit?: string;
  /** 各期间单价 */
  periodUnitPrices?: Record<string, number>;
  /** 各期间单价来源月 */
  periodPriceSourcePeriods?: Record<string, string>;
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
 * 销售价格月推移分析结果
 * @description 对应后端 TaktSalesPriceMonthlyTrendResultDto
 */
export interface SalesPriceMonthlyTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<SalesPriceMonthlyTrend>;
  /** 期间列顺序 */
  periodOrder: string[];
  /** 行总数 */
  materialCount: number;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 涨价行数 */
  upCount: number;
  /** 跌价行数 */
  downCount: number;
  /** 持平行数 */
  flatCount: number;
  /** 无法比较行数 */
  noneCount: number;
}

/**
 * 销售机种价格推移转置行
 * @description 对应后端 TaktSalesPriceModelTrendDto
 */
export interface SalesPriceModelTrend extends SalesPriceMonthlyTrend {
  /** 机种组展示 */
  modelGroup?: string;
  /** 产品组展示 */
  productGroup?: string;
  /** 机种编码列表 */
  modelCodes?: string[];
  /** 产品编码列表 */
  productCodes?: string[];
  /** 物料描述 */
  materialText?: string;
}

/**
 * 销售机种价格推移分析结果
 * @description 对应后端 TaktSalesPriceModelTrendResultDto
 */
export interface SalesPriceModelTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<SalesPriceModelTrend>;
  /** 期间列顺序 */
  periodOrder: string[];
  /** 行总数 */
  materialCount: number;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 涨价行数 */
  upCount: number;
  /** 跌价行数 */
  downCount: number;
  /** 持平行数 */
  flatCount: number;
  /** 无法比较行数 */
  noneCount: number;
}
