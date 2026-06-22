// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/routine/help-desk/ticket-change-log/composables
// 文件名称：use-ticket-master-context.ts
// 功能描述：Takt工单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { Ticket } from '@/types/routine/help-desk/ticket'

/** 主表选中行上下文 */
export interface TicketMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<Ticket | null>
}

const ticketMasterContextKey: InjectionKey<TicketMasterContext> = Symbol('ticketMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {TicketMasterContext} 主表上下文
 */
export function provideTicketMasterContext(): TicketMasterContext {
  const selectedMasterRow = ref<Ticket | null>(null)
  const ctx: TicketMasterContext = { selectedMasterRow }
  provide(ticketMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {TicketMasterContext} 主表上下文
 */
export function useTicketMasterContext(): TicketMasterContext {
  const ctx = inject(ticketMasterContextKey)
  if (!ctx) {
    throw new Error('useTicketMasterContext must be used within ticket index')
  }
  return ctx
}
