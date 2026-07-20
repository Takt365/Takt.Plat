// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/composables
// 文件名称：use-material-cost-analysis-master-context.ts
// 功能描述：成本推移页查询条件上下文（工厂/机种/产品/期间）
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'

/** 成本推移查询上下文 */
export interface BomMaterialCostAnalysisMasterContext {
  /** 查询工厂 */
  queryPlantCode: Ref<string | undefined>
  /** 查询机种 */
  queryModelCode: Ref<string | undefined>
  /** 查询产品（可选） */
  queryProductCode: Ref<string | undefined>
  /** 查询年月区间 [yyyy-MM, yyyy-MM] */
  periodRange: Ref<[string, string] | null>
  /** 兼容旧子组件：选中行（单表模式下可为空） */
  selectedMasterRow: Ref<Record<string, unknown> | null>
}

const key: InjectionKey<BomMaterialCostAnalysisMasterContext> = Symbol('material-cost-trend-query')

/**
 * 在推移页 provide 查询上下文
 * @returns {BomMaterialCostAnalysisMasterContext} 上下文
 */
export function provideBomMaterialCostAnalysisMasterContext(): BomMaterialCostAnalysisMasterContext {
  const queryPlantCode = ref<string | undefined>(undefined)
  const queryModelCode = ref<string | undefined>(undefined)
  const queryProductCode = ref<string | undefined>(undefined)
  const periodRange = ref<[string, string] | null>(null)
  const selectedMasterRow = ref<Record<string, unknown> | null>(null)
  const ctx: BomMaterialCostAnalysisMasterContext = {
    queryPlantCode,
    queryModelCode,
    queryProductCode,
    periodRange,
    selectedMasterRow,
  }
  provide(key, ctx)
  return ctx
}

/**
 * inject 查询上下文
 * @returns {BomMaterialCostAnalysisMasterContext} 上下文
 */
export function useBomMaterialCostAnalysisMasterContext(): BomMaterialCostAnalysisMasterContext {
  const ctx = inject(key)
  if (!ctx) {
    throw new Error('useBomMaterialCostAnalysisMasterContext must be used within trend index')
  }
  return ctx
}
