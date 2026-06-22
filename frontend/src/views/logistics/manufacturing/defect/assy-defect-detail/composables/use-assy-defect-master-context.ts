// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/defect/assy-defect-detail/composables
// 文件名称：use-assy-defect-master-context.ts
// 功能描述：组立不良日报实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { AssyDefect } from '@/types/logistics/manufacturing/defect/assy-defect'

/** 主表选中行上下文 */
export interface AssyDefectMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<AssyDefect | null>
}

const assyDefectMasterContextKey: InjectionKey<AssyDefectMasterContext> = Symbol('assy-defectMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {AssyDefectMasterContext} 主表上下文
 */
export function provideAssyDefectMasterContext(): AssyDefectMasterContext {
  const selectedMasterRow = ref<AssyDefect | null>(null)
  const ctx: AssyDefectMasterContext = { selectedMasterRow }
  provide(assyDefectMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {AssyDefectMasterContext} 主表上下文
 */
export function useAssyDefectMasterContext(): AssyDefectMasterContext {
  const ctx = inject(assyDefectMasterContextKey)
  if (!ctx) {
    throw new Error('useAssyDefectMasterContext must be used within assy-defect index')
  }
  return ctx
}
