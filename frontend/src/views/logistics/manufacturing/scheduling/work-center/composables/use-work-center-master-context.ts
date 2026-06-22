// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/scheduling/work-center/composables
// 文件名称：use-work-center-master-context.ts
// 功能描述：工作中心主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { WorkCenter } from '@/types/logistics/manufacturing/scheduling/work-center'

/** 主表选中行上下文 */
export interface WorkCenterMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<WorkCenter | null>
}

const workCenterMasterContextKey: InjectionKey<WorkCenterMasterContext> = Symbol('work-centerMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {WorkCenterMasterContext} 主表上下文
 */
export function provideWorkCenterMasterContext(): WorkCenterMasterContext {
  const selectedMasterRow = ref<WorkCenter | null>(null)
  const ctx: WorkCenterMasterContext = { selectedMasterRow }
  provide(workCenterMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {WorkCenterMasterContext} 主表上下文
 */
export function useWorkCenterMasterContext(): WorkCenterMasterContext {
  const ctx = inject(workCenterMasterContextKey)
  if (!ctx) {
    throw new Error('useWorkCenterMasterContext must be used within work-center index')
  }
  return ctx
}
