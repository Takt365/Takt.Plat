// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/customer-service/order/composables
// 文件名称：use-order-master-context.ts
// 功能描述：APS 排程订单主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { ApsOrder } from '@/types/logistics/manufacturing/aps/order'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type ApsOrderRowRecord = ApsOrder | Record<string, unknown>

/** 主表选中行上下文 */
export interface ApsOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ApsOrderRowRecord | null>
}

const apsOrderMasterContextKey: InjectionKey<ApsOrderMasterContext> = Symbol('orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ApsOrderMasterContext} 主表上下文
 */
export function provideApsOrderMasterContext(): ApsOrderMasterContext {
  const selectedMasterRow = ref<ApsOrderRowRecord | null>(null)
  const ctx: ApsOrderMasterContext = { selectedMasterRow }
  provide(apsOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ApsOrderMasterContext} 主表上下文
 */
export function useApsOrderMasterContext(): ApsOrderMasterContext {
  const ctx = inject(apsOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useApsOrderMasterContext must be used within order index')
  }
  return ctx
}
