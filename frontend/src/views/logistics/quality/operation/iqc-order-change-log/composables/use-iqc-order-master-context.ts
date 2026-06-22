// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/quality/operation/iqc-order-change-log/composables
// 文件名称：use-iqc-order-master-context.ts
// 功能描述：IQC进货检验单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { IqcOrder } from '@/types/logistics/quality/operation/iqc-order'

/** 主表选中行上下文 */
export interface IqcOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<IqcOrder | null>
}

const iqcOrderMasterContextKey: InjectionKey<IqcOrderMasterContext> = Symbol('iqc-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {IqcOrderMasterContext} 主表上下文
 */
export function provideIqcOrderMasterContext(): IqcOrderMasterContext {
  const selectedMasterRow = ref<IqcOrder | null>(null)
  const ctx: IqcOrderMasterContext = { selectedMasterRow }
  provide(iqcOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {IqcOrderMasterContext} 主表上下文
 */
export function useIqcOrderMasterContext(): IqcOrderMasterContext {
  const ctx = inject(iqcOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useIqcOrderMasterContext must be used within iqc-order index')
  }
  return ctx
}
