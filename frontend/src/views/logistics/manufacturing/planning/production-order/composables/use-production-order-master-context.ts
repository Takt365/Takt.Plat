// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/output/production-order/composables
// 文件名称：use-production-order-master-context.ts
// 功能描述：生产工单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { ProductionOrder } from '@/types/logistics/manufacturing/planning/production-order'

/** 主表选中行上下文 */
export interface ProductionOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ProductionOrder | null>
}

const productionOrderMasterContextKey: InjectionKey<ProductionOrderMasterContext> = Symbol('production-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ProductionOrderMasterContext} 主表上下文
 */
export function provideProductionOrderMasterContext(): ProductionOrderMasterContext {
  const selectedMasterRow = ref<ProductionOrder | null>(null)
  const ctx: ProductionOrderMasterContext = { selectedMasterRow }
  provide(productionOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ProductionOrderMasterContext} 主表上下文
 */
export function useProductionOrderMasterContext(): ProductionOrderMasterContext {
  const ctx = inject(productionOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useProductionOrderMasterContext must be used within production-order index')
  }
  return ctx
}
