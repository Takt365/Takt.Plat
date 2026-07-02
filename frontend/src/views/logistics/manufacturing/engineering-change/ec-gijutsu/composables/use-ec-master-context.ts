// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables
// 文件名称：use-ec-master-context.ts
// 功能描述：设变主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { EcGijutsu } from '@/types/logistics/manufacturing/engineering-change/ec-gijutsu'

/** 主表选中行上下文 */
export interface EcMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<Ec | null>
}

const ecMasterContextKey: InjectionKey<EcMasterContext> = Symbol('ecMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {EcMasterContext} 主表上下文
 */
export function provideEcMasterContext(): EcMasterContext {
  const selectedMasterRow = ref<EcGijutsu | null>(null)
  const ctx: EcMasterContext = { selectedMasterRow }
  provide(ecMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {EcMasterContext} 主表上下文
 */
export function useEcMasterContext(): EcMasterContext {
  const ctx = inject(ecMasterContextKey)
  if (!ctx) {
    throw new Error('useEcMasterContext must be used within ec-gijutsu index')
  }
  return ctx
}
