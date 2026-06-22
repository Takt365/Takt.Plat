// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/procurement/purchase-price-change-log/composables
// 文件名称：use-purchase-price-master-context.ts
// 功能描述：Takt采购价格实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { PurchasePrice } from '@/types/logistics/procurement/purchase-price'

/** 主表选中行上下文 */
export interface PurchasePriceMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchasePrice | null>
}

const purchasePriceMasterContextKey: InjectionKey<PurchasePriceMasterContext> = Symbol('purchase-priceMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchasePriceMasterContext} 主表上下文
 */
export function providePurchasePriceMasterContext(): PurchasePriceMasterContext {
  const selectedMasterRow = ref<PurchasePrice | null>(null)
  const ctx: PurchasePriceMasterContext = { selectedMasterRow }
  provide(purchasePriceMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchasePriceMasterContext} 主表上下文
 */
export function usePurchasePriceMasterContext(): PurchasePriceMasterContext {
  const ctx = inject(purchasePriceMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchasePriceMasterContext must be used within purchase-price index')
  }
  return ctx
}
