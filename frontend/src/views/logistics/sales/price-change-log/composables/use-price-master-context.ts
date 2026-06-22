// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/sales/price-change-log/composables
// 文件名称：use-price-master-context.ts
// 功能描述：Takt销售价格实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { SalesPrice } from '@/types/logistics/sales/price'

/** 主表选中行上下文 */
export interface SalesPriceMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SalesPrice | null>
}

const salesPriceMasterContextKey: InjectionKey<SalesPriceMasterContext> = Symbol('priceMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesPriceMasterContext} 主表上下文
 */
export function provideSalesPriceMasterContext(): SalesPriceMasterContext {
  const selectedMasterRow = ref<SalesPrice | null>(null)
  const ctx: SalesPriceMasterContext = { selectedMasterRow }
  provide(salesPriceMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesPriceMasterContext} 主表上下文
 */
export function useSalesPriceMasterContext(): SalesPriceMasterContext {
  const ctx = inject(salesPriceMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesPriceMasterContext must be used within price index')
  }
  return ctx
}
