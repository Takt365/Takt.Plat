// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/procurement/purchase-price-item/composables
// 文件名称：use-purchase-price-item-master-context.ts
// 功能描述：Takt采购价格明细实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { PurchasePriceItem } from '@/types/logistics/procurement/purchase-price-item'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchasePriceItemRowRecord = PurchasePriceItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchasePriceItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchasePriceItemRowRecord | null>
}

const purchasePriceItemMasterContextKey: InjectionKey<PurchasePriceItemMasterContext> = Symbol('purchase-price-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchasePriceItemMasterContext} 主表上下文
 */
export function providePurchasePriceItemMasterContext(): PurchasePriceItemMasterContext {
  const selectedMasterRow = ref<PurchasePriceItemRowRecord | null>(null)
  const ctx: PurchasePriceItemMasterContext = { selectedMasterRow }
  provide(purchasePriceItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchasePriceItemMasterContext} 主表上下文
 */
export function usePurchasePriceItemMasterContext(): PurchasePriceItemMasterContext {
  const ctx = inject(purchasePriceItemMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchasePriceItemMasterContext must be used within purchase-price-item index')
  }
  return ctx
}
