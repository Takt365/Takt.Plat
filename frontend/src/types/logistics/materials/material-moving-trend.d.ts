// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material-moving-trend.d.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：物料月移动价格推移 / 物料-机种-价格推移分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common';

/**
 * 物料月移动价格推移查询
 * @description 对应后端 TaktMaterialMovingPriceMonthlyTrendQueryDto
 */
export interface MaterialMovingPriceMonthlyTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 评估类别 */
  valuation?: string;
  /** 物料编码（模糊） */
  materialCode?: string;
  /** 涨跌筛选：空/all=全部；leading=机种推移领涨领跌各 50；up/down/changed */
  trendFilter?: string;
}

/**
 * 物料月移动价格转置行
 * @description 对应后端 TaktMaterialMovingPriceMonthlyTrendDto
 */
export interface MaterialMovingPriceMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 物料编码 */
  materialCode: string;
  /** 物料名称 */
  materialName?: string;
  /** 评估类别 */
  valuation: string;
  /** 币种 */
  currency?: string;
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
  /** 环比变动率（小数比率，如 0.2978 = 29.78%；清单导出 4 位小数） */
  variancePercent?: number | null;
}

/**
 * 物料月移动价格推移分析结果
 * @description 对应后端 TaktMaterialMovingPriceMonthlyTrendResultDto
 */
export interface MaterialMovingPriceMonthlyTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<MaterialMovingPriceMonthlyTrend>;
  /** 期间列顺序 */
  periodOrder: string[];
  /** 物料行总数 */
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
 * 物料-机种-价格推移行
 * @description 对应后端 TaktMaterialMovingPriceModelTrendDto
 */
export interface MaterialMovingPriceModelTrend extends MaterialMovingPriceMonthlyTrend {
  /** 机种组（逗号分隔） */
  modelGroup?: string;
  /** 产品组（逗号分隔） */
  productGroup?: string;
  /** 机种编码列表 */
  modelCodes?: string[];
  /** 产品编码列表 */
  productCodes?: string[];
  /** 物料描述 */
  materialText?: string;
}

/**
 * 物料-机种-价格推移分析结果
 * @description 对应后端 TaktMaterialMovingPriceModelTrendResultDto
 */
export interface MaterialMovingPriceModelTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<MaterialMovingPriceModelTrend>;
  /** 期间列顺序 */
  periodOrder: string[];
  /** 物料行总数 */
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
