// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/routine/conference-center/conference/composables
// 文件名称：use-conference-master-context.ts
// 功能描述：会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { Conference } from '@/types/routine/conference-center/conference'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type ConferenceRowRecord = Conference | Record<string, unknown>

/** 主表选中行上下文 */
export interface ConferenceMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ConferenceRowRecord | null>
}

const conferenceMasterContextKey: InjectionKey<ConferenceMasterContext> = Symbol('conferenceMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ConferenceMasterContext} 主表上下文
 */
export function provideConferenceMasterContext(): ConferenceMasterContext {
  const selectedMasterRow = ref<ConferenceRowRecord | null>(null)
  const ctx: ConferenceMasterContext = { selectedMasterRow }
  provide(conferenceMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ConferenceMasterContext} 主表上下文
 */
export function useConferenceMasterContext(): ConferenceMasterContext {
  const ctx = inject(conferenceMasterContextKey)
  if (!ctx) {
    throw new Error('useConferenceMasterContext must be used within conference index')
  }
  return ctx
}
