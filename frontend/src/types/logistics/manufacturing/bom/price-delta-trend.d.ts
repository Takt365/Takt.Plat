// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：price-delta-trend.d.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：成本差异推移类型（独立模块）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common'

/** 成本差异推移查询 */
export interface BomPriceDeltaTrendQuery extends TaktPagedQuery {
  plantCode: string
  /** 物料类型（空则后端默认 FERT） */
  materialType?: string
  modelCode?: string
  productCode?: string
  costingDateStart?: string
  costingDateEnd?: string
  focusPeriod?: string
}

/** 成本差异推移行 */
export interface BomPriceDeltaTrend {
  plantCode: string
  modelCode: string
  productCode: string
  productDescription: string
  periodCosts: Record<string, number>
  /** 差异：价格差异组 Summary Var + 组件差异 Summary Var */
  priceDelta?: number | null
  /** 0价格组 (物料:用量:可替代:替代价, …)；末字母逆推仅查移动价格表最近期间，不查 cost_item */
  zeroPriceGroup: string
  /** 价格差异组 (组件:用量:基期价→关注价,Diff:单价差, …),Summary Var:行成本差合计 */
  priceDeltaTrend: string
  /** 组件差异 (…→remove/new/version, …),Summary Var:结构变动行成本合计；与价格组 Summary 之和=差异 */
  componentDeltaGroup: string
  basePeriod?: string | null
  comparePeriod?: string | null
}

/** 成本差异推移结果 */
export interface BomPriceDeltaTrendResult {
  paged: TaktPagedResult<BomPriceDeltaTrend>
  periodOrder: string[]
  basePeriod?: string | null
  comparePeriod?: string | null
}
