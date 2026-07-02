// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/accounting/controlling/profit-center-change-log/composables
// 文件名称：use-profit-center-master-context.ts
// 功能描述：利润中心实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { ProfitCenter } from '@/types/accounting/controlling/profit-center'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type ProfitCenterRowRecord = ProfitCenter | Record<string, unknown>

/** 主表选中行上下文 */
export interface ProfitCenterMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ProfitCenterRowRecord | null>
}

const profitCenterMasterContextKey: InjectionKey<ProfitCenterMasterContext> = Symbol('profit-centerMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ProfitCenterMasterContext} 主表上下文
 */
export function provideProfitCenterMasterContext(): ProfitCenterMasterContext {
  const selectedMasterRow = ref<ProfitCenterRowRecord | null>(null)
  const ctx: ProfitCenterMasterContext = { selectedMasterRow }
  provide(profitCenterMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ProfitCenterMasterContext} 主表上下文
 */
export function useProfitCenterMasterContext(): ProfitCenterMasterContext {
  const ctx = inject(profitCenterMasterContextKey)
  if (!ctx) {
    throw new Error('useProfitCenterMasterContext must be used within profit-center index')
  }
  return ctx
}
