// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/procurement/purchase-inquiry/composables
// 文件名称：use-purchase-inquiry-master-context.ts
// 功能描述：采购询价实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { PurchaseInquiry } from '@/types/logistics/procurement/purchase-inquiry'

/** 主表选中行上下文 */
export interface PurchaseInquiryMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchaseInquiry | null>
}

const purchaseInquiryMasterContextKey: InjectionKey<PurchaseInquiryMasterContext> = Symbol('purchase-inquiryMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseInquiryMasterContext} 主表上下文
 */
export function providePurchaseInquiryMasterContext(): PurchaseInquiryMasterContext {
  const selectedMasterRow = ref<PurchaseInquiry | null>(null)
  const ctx: PurchaseInquiryMasterContext = { selectedMasterRow }
  provide(purchaseInquiryMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseInquiryMasterContext} 主表上下文
 */
export function usePurchaseInquiryMasterContext(): PurchaseInquiryMasterContext {
  const ctx = inject(purchaseInquiryMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseInquiryMasterContext must be used within purchase-inquiry index')
  }
  return ctx
}
