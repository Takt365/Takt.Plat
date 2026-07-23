// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-price-trend.d.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格月推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common';

/**
 * 采购价格月推移查询
 * @description 对应后端 TaktPurchasePriceMonthlyTrendQueryDto
 */
export interface PurchasePriceMonthlyTrendQuery extends TaktPagedQuery {
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
  /** 供应商编码 */
  supplierCode?: string;
  /** 价格类型（字典 logistics_price_type，如 PB00） */
  priceType?: string;
  /** 仅启用主表 */
  onlyEnabled?: boolean;
  /** 涨跌筛选：空/all=全部；leading=机种推移领涨领跌各 50；up/down/changed */
  trendFilter?: string;
}

/**
 * 采购价格月推移转置行
 * @description 对应后端 TaktPurchasePriceMonthlyTrendDto
 */
export interface PurchasePriceMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 物料编码 */
  materialCode: string;
  /** 物料名称 */
  materialName?: string;
  /** 供应商编码 */
  supplierCode: string;
  /** 供应商名称 */
  supplierName?: string;
  /** 币种 */
  currency?: string;
  /** 采购单位 */
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
 * 采购价格月推移分析结果
 * @description 对应后端 TaktPurchasePriceMonthlyTrendResultDto
 */
export interface PurchasePriceMonthlyTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<PurchasePriceMonthlyTrend>;
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
 * 采购机种价格推移转置行
 * @description 对应后端 TaktPurchasePriceModelTrendDto
 */
export interface PurchasePriceModelTrend extends PurchasePriceMonthlyTrend {
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
 * 采购机种价格推移分析结果
 * @description 对应后端 TaktPurchasePriceModelTrendResultDto
 */
export interface PurchasePriceModelTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<PurchasePriceModelTrend>;
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
