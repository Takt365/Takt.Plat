// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/accounting/controlling/cost-center-change-log/composables
// 文件名称：use-cost-center-master-context.ts
// 功能描述：成本中心实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { CostCenter } from '@/types/accounting/controlling/cost-center'

/** 主表选中行上下文 */
export interface CostCenterMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<CostCenter | null>
}

const costCenterMasterContextKey: InjectionKey<CostCenterMasterContext> = Symbol('cost-centerMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {CostCenterMasterContext} 主表上下文
 */
export function provideCostCenterMasterContext(): CostCenterMasterContext {
  const selectedMasterRow = ref<CostCenter | null>(null)
  const ctx: CostCenterMasterContext = { selectedMasterRow }
  provide(costCenterMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {CostCenterMasterContext} 主表上下文
 */
export function useCostCenterMasterContext(): CostCenterMasterContext {
  const ctx = inject(costCenterMasterContextKey)
  if (!ctx) {
    throw new Error('useCostCenterMasterContext must be used within cost-center index')
  }
  return ctx
}
