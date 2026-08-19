// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：model-cost-trend.d.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移类型（对应 TaktBomModelCostTrends）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common';

/** 机种成本推移查询 */
export interface BomMaterialCostItemModelCostTrendQuery extends TaktPagedQuery {
  plantCode: string;
  /** 物料类型（字典 logistics_material_type；空=默认 FERT） */
  materialType?: string;
  /** 兼容单机种 */
  modelCode?: string;
  /** 机种多选（逗号分隔；空=全部） */
  modelCodes?: string;
  /** 可选产品 */
  productCode?: string;
  /** 兼容单物料 */
  componentCode?: string;
  /** 物料多选（逗号分隔；空=全部） */
  componentCodes?: string;
  periodDateStart?: string;
  periodDateEnd?: string;
  focusPeriod?: string;
  trendFilter?: string;
  /**
   * 全量列表排序（分页前）：productCountDesc（默认）/ productCountAsc / trend
   */
  sortBy?: string;
  /**
   * 合并模式：summary=材料成本推移；detail=差异组件（月度有无）
   */
  mergeMode?: 'summary' | 'detail' | string;
}

/**
 * 机种成本推移分析行
 * @description summary/detail；列为月材料成本（CostingDate→yyyy-MM）
 */
export interface BomMaterialCostItemModelCostTrend {
  plantCode: string;
  modelCode: string;
  modelName: string;
  componentCode: string;
  componentDescription: string;
  /** detail 模式 */
  componentQuantity?: number | null;
  /** detail 模式 */
  batchIndicator?: string | null;
  productionRelated?: string | null;
  /** PCB SECT 标识（空参与；X 不参与） */
  pcbSectIndicator?: string | null;
  purchaseType: string;
  /** detail 模式 */
  specialProcurementType?: string | null;
  /** detail 模式 */
  profitCenterCode?: string | null;
  /** 产品组：产品编码:组件数量，英文逗号分隔（如 8Y00000154:1,09VRS7TS04:1） */
  productCodes: string;
  productCount: number;
  currencyCode: string;
  periodMaterialCosts: Record<string, number>;
  /** detail：各月存在/变动码 */
  periodChangeTypes?: Record<string, string>;
  /** 兼容旧字段：同 periodMaterialCosts */
  periodUnitPrices?: Record<string, number>;
  trend?: string;
  basePeriod?: string | null;
  comparePeriod?: string | null;
  varianceAmount?: number | null;
  variancePercent?: number | null;
}

/** 机种成本推移分析结果 */
export interface BomMaterialCostItemModelCostTrendResult {
  paged: TaktPagedResult<BomMaterialCostItemModelCostTrend>;
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
