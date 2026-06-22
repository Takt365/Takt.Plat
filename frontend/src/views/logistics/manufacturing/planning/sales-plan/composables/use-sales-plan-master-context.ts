// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/planning/sales-plan/composables
// 文件名称：use-sales-plan-master-context.ts
// 功能描述：Takt销售计划实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { SalesPlan } from '@/types/logistics/manufacturing/planning/sales-plan'

/** 主表选中行上下文 */
export interface SalesPlanMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SalesPlan | null>
}

const salesPlanMasterContextKey: InjectionKey<SalesPlanMasterContext> = Symbol('sales-planMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesPlanMasterContext} 主表上下文
 */
export function provideSalesPlanMasterContext(): SalesPlanMasterContext {
  const selectedMasterRow = ref<SalesPlan | null>(null)
  const ctx: SalesPlanMasterContext = { selectedMasterRow }
  provide(salesPlanMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesPlanMasterContext} 主表上下文
 */
export function useSalesPlanMasterContext(): SalesPlanMasterContext {
  const ctx = inject(salesPlanMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesPlanMasterContext must be used within sales-plan index')
  }
  return ctx
}
