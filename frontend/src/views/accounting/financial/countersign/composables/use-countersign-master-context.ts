// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/accounting/financial/countersign/composables
// 文件名称：use-countersign-master-context.ts
// 功能描述：会签单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { Countersign } from '@/types/accounting/financial/countersign'

/** 主表选中行上下文 */
export interface CountersignMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<Countersign | null>
}

const countersignMasterContextKey: InjectionKey<CountersignMasterContext> = Symbol('countersignMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {CountersignMasterContext} 主表上下文
 */
export function provideCountersignMasterContext(): CountersignMasterContext {
  const selectedMasterRow = ref<Countersign | null>(null)
  const ctx: CountersignMasterContext = { selectedMasterRow }
  provide(countersignMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {CountersignMasterContext} 主表上下文
 */
export function useCountersignMasterContext(): CountersignMasterContext {
  const ctx = inject(countersignMasterContextKey)
  if (!ctx) {
    throw new Error('useCountersignMasterContext must be used within countersign index')
  }
  return ctx
}
