// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：variance-cost-trend.d.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 差异成本推移类型（有无/版本差异×移动单价推移）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common'

/** 差异成本推移查询（工厂、期间、机种必填可多选） */
export interface BomVarianceCostTrendQuery extends TaktPagedQuery {
  plantCode: string
  /** 兼容单值 */
  modelCode?: string
  /** 机种多选，逗号分隔 */
  modelCodes?: string
  materialType?: string
  /** 兼容单值 */
  productCode?: string
  /** 产品多选，逗号分隔；空=机种下全部产品 */
  productCodes?: string
  periodDateStart?: string
  periodDateEnd?: string
  focusPeriod?: string
  /** 空=全部；new / removed / version */
  trendFilter?: string
  /** 全量排序（分页前）：trend / varianceDesc / componentCode */
  sortBy?: string
}

/** 差异成本推移分析行（期间列为移动单价） */
export interface BomVarianceCostTrend {
  plantCode: string
  modelCode: string
  modelName: string
  /** 对比槽位：产品编码 */
  productCode: string
  /** 对比槽位：序号 */
  sequenceCode: string
  /** 对比槽位：层级 */
  bomLevel: string
  /** 对比槽位：BOM 项目号 */
  bomItemCode: string
  componentCode: string
  /** 基准月组件（版本变更） */
  previousComponentCode?: string | null
  componentDescription: string
  componentQuantity?: number | null
  /** 关注月移动单价 */
  movingPrice?: number | null
  currencyCode: string
  productionRelated?: string | null
  purchaseType: string
  /** 兼容：单产品时等于 productCode */
  productCodes: string
  productCount: number
  /** 各月移动单价 yyyy-MM */
  periodMovingPrices: Record<string, number>
  periodChangeTypes?: Record<string, string>
  /** new / removed / version */
  trend?: string
  basePeriod?: string | null
  comparePeriod?: string | null
  varianceAmount?: number | null
  variancePercent?: number | null
}

/** 差异成本推移分析结果 */
export interface BomVarianceCostTrendResult {
  paged: TaktPagedResult<BomVarianceCostTrend>
  periodOrder: string[]
  productCodes: string[]
  componentCount: number
  basePeriod?: string | null
  comparePeriod?: string | null
  upCount: number
  downCount: number
  flatCount: number
  newCount: number
  removedCount: number
  versionCount: number
  noneCount: number
}
