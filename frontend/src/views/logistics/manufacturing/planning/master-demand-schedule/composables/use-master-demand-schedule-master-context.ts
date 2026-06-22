// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/planning/master-demand-schedule/composables
// 文件名称：use-master-demand-schedule-master-context.ts
// 功能描述：主需求计划 MDS 头表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { MasterDemandSchedule } from '@/types/logistics/manufacturing/planning/master-demand-schedule'

/** 主表选中行上下文 */
export interface MasterDemandScheduleMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<MasterDemandSchedule | null>
}

const masterDemandScheduleMasterContextKey: InjectionKey<MasterDemandScheduleMasterContext> = Symbol('master-demand-scheduleMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MasterDemandScheduleMasterContext} 主表上下文
 */
export function provideMasterDemandScheduleMasterContext(): MasterDemandScheduleMasterContext {
  const selectedMasterRow = ref<MasterDemandSchedule | null>(null)
  const ctx: MasterDemandScheduleMasterContext = { selectedMasterRow }
  provide(masterDemandScheduleMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MasterDemandScheduleMasterContext} 主表上下文
 */
export function useMasterDemandScheduleMasterContext(): MasterDemandScheduleMasterContext {
  const ctx = inject(masterDemandScheduleMasterContextKey)
  if (!ctx) {
    throw new Error('useMasterDemandScheduleMasterContext must be used within master-demand-schedule index')
  }
  return ctx
}
