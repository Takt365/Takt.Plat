// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/planning/purchase-plan/composables
// 文件名称：use-purchase-plan-master-context.ts
// 功能描述：Takt采购计划实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { PurchasePlan } from '@/types/logistics/manufacturing/planning/purchase-plan'

/** 主表选中行上下文 */
export interface PurchasePlanMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchasePlan | null>
}

const purchasePlanMasterContextKey: InjectionKey<PurchasePlanMasterContext> = Symbol('purchase-planMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchasePlanMasterContext} 主表上下文
 */
export function providePurchasePlanMasterContext(): PurchasePlanMasterContext {
  const selectedMasterRow = ref<PurchasePlan | null>(null)
  const ctx: PurchasePlanMasterContext = { selectedMasterRow }
  provide(purchasePlanMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchasePlanMasterContext} 主表上下文
 */
export function usePurchasePlanMasterContext(): PurchasePlanMasterContext {
  const ctx = inject(purchasePlanMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchasePlanMasterContext must be used within purchase-plan index')
  }
  return ctx
}
