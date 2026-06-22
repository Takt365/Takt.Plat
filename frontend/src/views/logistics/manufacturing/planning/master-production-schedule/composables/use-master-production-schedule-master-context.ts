// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/planning/master-production-schedule/composables
// 文件名称：use-master-production-schedule-master-context.ts
// 功能描述：主生产计划 MPS 头表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { MasterProductionSchedule } from '@/types/logistics/manufacturing/planning/master-production-schedule'

/** 主表选中行上下文 */
export interface MasterProductionScheduleMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<MasterProductionSchedule | null>
}

const masterProductionScheduleMasterContextKey: InjectionKey<MasterProductionScheduleMasterContext> = Symbol('master-production-scheduleMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MasterProductionScheduleMasterContext} 主表上下文
 */
export function provideMasterProductionScheduleMasterContext(): MasterProductionScheduleMasterContext {
  const selectedMasterRow = ref<MasterProductionSchedule | null>(null)
  const ctx: MasterProductionScheduleMasterContext = { selectedMasterRow }
  provide(masterProductionScheduleMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MasterProductionScheduleMasterContext} 主表上下文
 */
export function useMasterProductionScheduleMasterContext(): MasterProductionScheduleMasterContext {
  const ctx = inject(masterProductionScheduleMasterContextKey)
  if (!ctx) {
    throw new Error('useMasterProductionScheduleMasterContext must be used within master-production-schedule index')
  }
  return ctx
}
