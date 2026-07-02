// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/sales/price-item/composables
// 文件名称：use-price-item-master-context.ts
// 功能描述：Takt销售价格明细实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { SalesPriceItem } from '@/types/logistics/sales/price-item'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SalesPriceItemRowRecord = SalesPriceItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface SalesPriceItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SalesPriceItemRowRecord | null>
}

const salesPriceItemMasterContextKey: InjectionKey<SalesPriceItemMasterContext> = Symbol('price-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesPriceItemMasterContext} 主表上下文
 */
export function provideSalesPriceItemMasterContext(): SalesPriceItemMasterContext {
  const selectedMasterRow = ref<SalesPriceItemRowRecord | null>(null)
  const ctx: SalesPriceItemMasterContext = { selectedMasterRow }
  provide(salesPriceItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesPriceItemMasterContext} 主表上下文
 */
export function useSalesPriceItemMasterContext(): SalesPriceItemMasterContext {
  const ctx = inject(salesPriceItemMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesPriceItemMasterContext must be used within price-item index')
  }
  return ctx
}
