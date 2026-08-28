// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：model-trend.d.ts
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售机种推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common';
import type { SalesPriceTrend } from '@/types/logistics/sales/price-trend';

/**
 * 销售机种推移查询
 * @description 对应后端 TaktSalesModelTrendQueryDto
 */
export interface SalesModelTrendQuery extends TaktPagedQuery {
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
  /** 价格类型（字典 logistics_procurement_price_type，如 PR00） */
  priceType?: string;
  /** 仅启用主表 */
  onlyEnabled?: boolean;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 销售机种推移转置行
 * @description 对应后端 TaktSalesModelTrendDto
 */
export interface SalesModelTrend extends SalesPriceTrend {
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
 * 销售机种推移分析结果
 * @description 对应后端 TaktSalesModelTrendResultDto
 */
export interface SalesModelTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<SalesModelTrend>;
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
