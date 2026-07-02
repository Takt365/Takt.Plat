// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/composables
// 文件名称：use-source-ec-master-context.ts
// 功能描述：设变来源主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { SourceEc } from '@/types/logistics/manufacturing/engineering-change/source-ec'

/** 主表选中行上下文 */
export interface SourceEcMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SourceEc | null>
}

const sourceEcMasterContextKey: InjectionKey<SourceEcMasterContext> = Symbol('source-ecMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SourceEcMasterContext} 主表上下文
 */
export function provideSourceEcMasterContext(): SourceEcMasterContext {
  const selectedMasterRow = ref<SourceEc | null>(null)
  const ctx: SourceEcMasterContext = { selectedMasterRow }
  provide(sourceEcMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SourceEcMasterContext} 主表上下文
 */
export function useSourceEcMasterContext(): SourceEcMasterContext {
  const ctx = inject(sourceEcMasterContextKey)
  if (!ctx) {
    throw new Error('useSourceEcMasterContext must be used within source-ec index')
  }
  return ctx
}
