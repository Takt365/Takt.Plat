// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/sales/sales-invoice/composables
// 文件名称：use-invoice-master-context.ts
// 功能描述：Takt销售发票主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { SalesInvoice } from '@/types/logistics/sales/invoice'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SalesInvoiceRowRecord = SalesInvoice | Record<string, unknown>

/** 主表选中行上下文 */
export interface SalesInvoiceMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SalesInvoiceRowRecord | null>
}

const salesInvoiceMasterContextKey: InjectionKey<SalesInvoiceMasterContext> = Symbol('invoiceMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesInvoiceMasterContext} 主表上下文
 */
export function provideSalesInvoiceMasterContext(): SalesInvoiceMasterContext {
  const selectedMasterRow = ref<SalesInvoiceRowRecord | null>(null)
  const ctx: SalesInvoiceMasterContext = { selectedMasterRow }
  provide(salesInvoiceMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesInvoiceMasterContext} 主表上下文
 */
export function useSalesInvoiceMasterContext(): SalesInvoiceMasterContext {
  const ctx = inject(salesInvoiceMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesInvoiceMasterContext must be used within invoice index')
  }
  return ctx
}
