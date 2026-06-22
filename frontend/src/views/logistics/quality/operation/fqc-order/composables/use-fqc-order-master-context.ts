// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/quality/operation/fqc-order/composables
// 文件名称：use-fqc-order-master-context.ts
// 功能描述：FQC出货检验单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { FqcOrder } from '@/types/logistics/quality/operation/fqc-order'

/** 主表选中行上下文 */
export interface FqcOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<FqcOrder | null>
}

const fqcOrderMasterContextKey: InjectionKey<FqcOrderMasterContext> = Symbol('fqc-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {FqcOrderMasterContext} 主表上下文
 */
export function provideFqcOrderMasterContext(): FqcOrderMasterContext {
  const selectedMasterRow = ref<FqcOrder | null>(null)
  const ctx: FqcOrderMasterContext = { selectedMasterRow }
  provide(fqcOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {FqcOrderMasterContext} 主表上下文
 */
export function useFqcOrderMasterContext(): FqcOrderMasterContext {
  const ctx = inject(fqcOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useFqcOrderMasterContext must be used within fqc-order index')
  }
  return ctx
}
