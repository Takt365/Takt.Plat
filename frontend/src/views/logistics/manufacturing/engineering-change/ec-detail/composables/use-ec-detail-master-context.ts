// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-detail/composables
// 文件名称：use-ec-detail-master-context.ts
// 功能描述：设变主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ec-detail'

/** 主表选中行上下文 */
export interface EcDetailMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<EcDetail | null>
}

const ecDetailMasterContextKey: InjectionKey<EcDetailMasterContext> = Symbol('ec-detailMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {EcDetailMasterContext} 主表上下文
 */
export function provideEcDetailMasterContext(): EcDetailMasterContext {
  const selectedMasterRow = ref<EcDetail | null>(null)
  const ctx: EcDetailMasterContext = { selectedMasterRow }
  provide(ecDetailMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {EcDetailMasterContext} 主表上下文
 */
export function useEcDetailMasterContext(): EcDetailMasterContext {
  const ctx = inject(ecDetailMasterContextKey)
  if (!ctx) {
    throw new Error('useEcDetailMasterContext must be used within ec-detail index')
  }
  return ctx
}
