// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：material-cost-trend.d.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本转置/差异分析类型（TaktBomMaterialCostItems/transposed、variance-analysis）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common';

/** BOM 物料成本转置查询（行=产品，列=月份总成本） */
export interface BomMaterialCostItemTransposedQuery extends TaktPagedQuery {
  plantCode?: string;
  modelCode?: string;
  productCode?: string;
  /** 核算日起 yyyy-MM-dd */
  costingDateStart?: string;
  /** 核算日止 yyyy-MM-dd */
  costingDateEnd?: string;
  /** 聚焦期间 yyyy-MM（算环比涨跌） */
  focusPeriod?: string;
  /** 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌 */
  trendFilter?: string;
}

/** BOM 物料成本转置行（行=产品，列=各月总成本；数据来自 TaktBomMaterialCost） */
export interface BomMaterialCostItemTransposed {
  plantCode: string;
  /** 机种编码 */
  modelCode?: string;
  productCode: string;
  productDescription: string;
  /** 币种 */
  currencyCode?: string;
  /** 各期间产品月成本，键 yyyy-MM */
  periodCosts: Record<string, number>;
  /** 涨跌：none | up | down | flat */
  trend?: string;
  basePeriod?: string | null;
  comparePeriod?: string | null;
  varianceAmount?: number | null;
  variancePercent?: number | null;
}

/** BOM 物料成本转置分页结果 */
export interface BomMaterialCostItemTransposedResult {
  paged: TaktPagedResult<BomMaterialCostItemTransposed>;
  /** 期间列顺序 yyyy-MM */
  periodOrder: string[];
  /** 机种汇总 */
  modelSummary?: BomMaterialCostItemModelSummary | null;
  /** 全量行各期间成本合计（分页前、已应用涨跌筛选） */
  periodCostTotals?: Record<string, number>;
  /** 全量行环比差额合计（分页前、已应用涨跌筛选） */
  varianceAmountTotal?: number | null;
}

/** 机种材料成本汇总 */
export interface BomMaterialCostItemModelSummary {
  modelCode: string;
  modelName: string;
  productCount: number;
  averagePeriodCosts: Record<string, number>;
}

/** BOM 物料成本差异分析查询 */
export interface BomMaterialCostItemVarianceQuery {
  plantCode: string;
  productCode: string;
  basePeriod: string;
  comparePeriod: string;
}

/** BOM 物料成本差异分析行 */
export interface BomMaterialCostItemVarianceLine {
  bomItemNo: string;
  componentCode: string;
  componentDescription: string;
  purchaseType: string;
  currency: string;
  baseCost: number;
  compareCost: number;
  varianceAmount: number;
  variancePercent?: number | null;
  baseUnitPrice: number;
  compareUnitPrice: number;
  unitPriceVariance: number;
  baseQuantity: number;
  compareQuantity: number;
  quantityVariance: number;
  priceEffectAmount: number;
  quantityEffectAmount: number;
  changeType: string;
}

/** BOM 物料成本差异分析结果 */
export interface BomMaterialCostItemVarianceResult {
  plantCode: string;
  productCode: string;
  productDescription: string;
  basePeriod: string;
  comparePeriod: string;
  baseTotalCost: number;
  compareTotalCost: number;
  totalVariance: number;
  lines: BomMaterialCostItemVarianceLine[];
}

/** BOM 物料成本月度涨跌分析查询 */
export interface BomMaterialCostItemMonthlyTrendQuery {
  plantCode: string;
  modelCode: string;
  productCode: string;
  periodStart?: string;
  periodEnd?: string;
}

/** BOM 物料成本月度涨跌分析行 */
export interface BomMaterialCostItemMonthlyTrendLine {
  period: string;
  totalCost: number;
  basePeriod?: string | null;
  baseTotalCost?: number | null;
  varianceAmount?: number | null;
  variancePercent?: number | null;
  trend: string;
}

/** BOM 物料成本月度涨跌分析结果 */
export interface BomMaterialCostItemMonthlyTrendResult {
  plantCode: string;
  modelCode: string;
  productCode: string;
  productDescription: string;
  allMaterialsUnderModel: boolean;
  lines: BomMaterialCostItemMonthlyTrendLine[];
}

/** BOM 成本推移：单个产品 × 月材料成本查询 */
export interface BomMaterialCostItemComponentMovingPriceQuery extends TaktPagedQuery {
  plantCode: string;
  /** 可选；仅缩小产品范围 */
  modelCode?: string;
  /** 产品编码（必填；单个产品） */
  productCode: string;
  /** 核算期间起 yyyy-MM-dd（CostingDate 月初） */
  periodDateStart?: string;
  /** 核算期间止 yyyy-MM-dd（CostingDate 月初） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 评估类别（明细无此字段，服务忽略） */
  valuation?: string;
  /** 涨跌筛选：up/down/flat/none/changed */
  trendFilter?: string;
}

/**
 * BOM 成本推移明细行：单个产品下 BOM 明细行月材料成本
 * @description 按明细表业务键跨月对齐；缺月无键；含涨跌环比
 */
export interface BomMaterialCostItemComponentMovingPrice {
  plantCode: string;
  modelCode: string;
  productCode: string;
  productDescription: string;
  /** 序号（明细表） */
  sequenceNo?: string;
  /** BOM 层级 */
  bomLevel?: string;
  /** BOM 项目号 */
  bomItemNo?: string;
  /** 组件编码（明细表） */
  componentCode: string;
  /** 组件描述（明细表） */
  componentDescription: string;
  /** 组件数量 */
  componentQuantity?: number;
  productionRelated?: string | null;
  purchaseType: string;
  currency: string;
  /** 各核算月材料成本，键 yyyy-MM；缺月无键 */
  periodMaterialCosts: Record<string, number>;
  /** 兼容旧字段：同 periodMaterialCosts */
  periodUnitPrices?: Record<string, number>;
  trend?: string;
  basePeriod?: string | null;
  comparePeriod?: string | null;
  varianceAmount?: number | null;
  variancePercent?: number | null;
}

/** BOM 成本推移（产品明细组件×月材料成本）分析结果 */
export interface BomMaterialCostItemComponentMovingPriceResult {
  paged: TaktPagedResult<BomMaterialCostItemComponentMovingPrice>;
  periodOrder: string[];
  productCodes: string[];
  /** 明细组件行总数（字段名保留兼容） */
  componentCount: number;
  basePeriod?: string | null;
  comparePeriod?: string | null;
  upCount: number;
  downCount: number;
  flatCount: number;
  noneCount: number;
  /** 全量行各期间材料成本合计（分页前、已应用涨跌筛选） */
  periodCostTotals?: Record<string, number>;
  /** 全量行环比差额合计（分页前、已应用涨跌筛选） */
  varianceAmountTotal?: number | null;
}

/** 机种成本推移查询 */
export interface BomMaterialCostItemModelMovingPriceQuery extends TaktPagedQuery {
  plantCode: string;
  /** 可选；空=工厂期间全量（有 productCode 时可反查机种） */
  modelCode?: string;
  /** 可选；有值时仅汇总该产品，否则机种或工厂下全部产品 */
  productCode?: string;
  periodDateStart?: string;
  periodDateEnd?: string;
  focusPeriod?: string;
  trendFilter?: string;
}

/**
 * 机种成本推移分析行
 * @description 合并键 Plant+Component+ProductionRelated+PurchaseType；列为月材料成本
 */
export interface BomMaterialCostItemModelMovingPrice {
  plantCode: string;
  modelCode: string;
  modelName: string;
  componentCode: string;
  componentDescription: string;
  productionRelated?: string | null;
  purchaseType: string;
  productCodes: string;
  productCount: number;
  currency: string;
  periodMaterialCosts: Record<string, number>;
  /** 兼容旧字段：同 periodMaterialCosts */
  periodUnitPrices?: Record<string, number>;
  trend?: string;
  basePeriod?: string | null;
  comparePeriod?: string | null;
  varianceAmount?: number | null;
  variancePercent?: number | null;
}

/** 机种成本推移分析结果 */
export interface BomMaterialCostItemModelMovingPriceResult {
  paged: TaktPagedResult<BomMaterialCostItemModelMovingPrice>;
  periodOrder: string[];
  productCodes: string[];
  /** 机种各月材料成本（产品月成本算术平均） */
  modelPeriodMaterialCosts: Record<string, number>;
  modelTrend?: string;
  modelBasePeriod?: string | null;
  modelComparePeriod?: string | null;
  modelVarianceAmount?: number | null;
  modelVariancePercent?: number | null;
  componentCount: number;
  basePeriod?: string | null;
  comparePeriod?: string | null;
  upCount: number;
  downCount: number;
  flatCount: number;
  noneCount: number;
  /** 全量分析行各期间材料成本合计（分页前、已应用涨跌筛选） */
  periodCostTotals?: Record<string, number>;
  /** 全量分析行环比差额合计（分页前、已应用涨跌筛选） */
  varianceAmountTotal?: number | null;
}

/** 机种零价格合并查询（工厂+机种+核算月） */
export interface BomMaterialCostItemZeroMovingPriceQuery extends TaktPagedQuery {
  plantCode: string;
  modelCode: string;
  costingDateStart?: string;
  costingDateEnd?: string;
}

/** 零价格合并行：机种 + 组件 + 共用产品 */
export interface BomMaterialCostItemZeroMovingPrice {
  plantCode: string;
  modelCode: string;
  componentCode: string;
  componentDescription: string;
  /** 共用产品编码（逗号分隔） */
  productCodes: string;
  productCount: number;
  movingAveragePrice: number;
  /**
   * 建议代替组件：末字母前推且同月移动价大于 0 的首个编码
   */
  suggestedComponentCode?: string;
  /**
   * 建议代替组件的移动价格（无建议为空）
   */
  suggestedMovingPrice?: number | null;
  costingPeriod: string;
}

/** 零价格合并结果 */
export interface BomMaterialCostItemZeroMovingPriceResult {
  paged: TaktPagedResult<BomMaterialCostItemZeroMovingPrice>;
  productCodes: string[];
  componentCount: number;
  costingPeriod: string;
}
