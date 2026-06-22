// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/serial/outbound/composables
// 文件名称：use-outbound-master-context.ts
// 功能描述：序列号出库主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { SerialOutbound } from '@/types/logistics/serial/outbound'

/** 主表选中行上下文 */
export interface SerialOutboundMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SerialOutbound | null>
}

const serialOutboundMasterContextKey: InjectionKey<SerialOutboundMasterContext> = Symbol('outboundMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SerialOutboundMasterContext} 主表上下文
 */
export function provideSerialOutboundMasterContext(): SerialOutboundMasterContext {
  const selectedMasterRow = ref<SerialOutbound | null>(null)
  const ctx: SerialOutboundMasterContext = { selectedMasterRow }
  provide(serialOutboundMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SerialOutboundMasterContext} 主表上下文
 */
export function useSerialOutboundMasterContext(): SerialOutboundMasterContext {
  const ctx = inject(serialOutboundMasterContextKey)
  if (!ctx) {
    throw new Error('useSerialOutboundMasterContext must be used within outbound index')
  }
  return ctx
}
