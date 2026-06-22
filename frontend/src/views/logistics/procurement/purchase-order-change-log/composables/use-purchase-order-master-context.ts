// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/procurement/purchase-order-change-log/composables
// 文件名称：use-purchase-order-master-context.ts
// 功能描述：Takt采购订单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { PurchaseOrder } from '@/types/logistics/procurement/purchase-order'

/** 主表选中行上下文 */
export interface PurchaseOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchaseOrder | null>
}

const purchaseOrderMasterContextKey: InjectionKey<PurchaseOrderMasterContext> = Symbol('purchase-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseOrderMasterContext} 主表上下文
 */
export function providePurchaseOrderMasterContext(): PurchaseOrderMasterContext {
  const selectedMasterRow = ref<PurchaseOrder | null>(null)
  const ctx: PurchaseOrderMasterContext = { selectedMasterRow }
  provide(purchaseOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseOrderMasterContext} 主表上下文
 */
export function usePurchaseOrderMasterContext(): PurchaseOrderMasterContext {
  const ctx = inject(purchaseOrderMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseOrderMasterContext must be used within purchase-order index')
  }
  return ctx
}
