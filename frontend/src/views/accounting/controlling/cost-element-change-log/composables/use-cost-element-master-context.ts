// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/accounting/controlling/cost-element-change-log/composables
// 文件名称：use-cost-element-master-context.ts
// 功能描述：成本要素实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { CostElement } from '@/types/accounting/controlling/cost-element'

/** 主表选中行上下文 */
export interface CostElementMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<CostElement | null>
}

const costElementMasterContextKey: InjectionKey<CostElementMasterContext> = Symbol('cost-elementMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {CostElementMasterContext} 主表上下文
 */
export function provideCostElementMasterContext(): CostElementMasterContext {
  const selectedMasterRow = ref<CostElement | null>(null)
  const ctx: CostElementMasterContext = { selectedMasterRow }
  provide(costElementMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {CostElementMasterContext} 主表上下文
 */
export function useCostElementMasterContext(): CostElementMasterContext {
  const ctx = inject(costElementMasterContextKey)
  if (!ctx) {
    throw new Error('useCostElementMasterContext must be used within cost-element index')
  }
  return ctx
}
