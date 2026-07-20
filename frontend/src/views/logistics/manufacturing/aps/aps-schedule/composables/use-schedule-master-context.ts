// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/aps/aps-schedule/composables
// 文件名称：use-schedule-master-context.ts
// 功能描述：APS排程主表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { ApsSchedule } from '@/types/logistics/manufacturing/aps/schedule'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type ApsScheduleRowRecord = ApsSchedule | Record<string, unknown>

/** 主表选中行上下文 */
export interface ApsScheduleMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ApsScheduleRowRecord | null>
}

const apsScheduleMasterContextKey: InjectionKey<ApsScheduleMasterContext> = Symbol('scheduleMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ApsScheduleMasterContext} 主表上下文
 */
export function provideApsScheduleMasterContext(): ApsScheduleMasterContext {
  const selectedMasterRow = ref<ApsScheduleRowRecord | null>(null)
  const ctx: ApsScheduleMasterContext = { selectedMasterRow }
  provide(apsScheduleMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ApsScheduleMasterContext} 主表上下文
 */
export function useApsScheduleMasterContext(): ApsScheduleMasterContext {
  const ctx = inject(apsScheduleMasterContextKey)
  if (!ctx) {
    throw new Error('useApsScheduleMasterContext must be used within schedule index')
  }
  return ctx
}
