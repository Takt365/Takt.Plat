// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection/composables
// 文件名称：use-pcba-inspection-master-context.ts
// 功能描述：PCBA检查日报实体 不良率主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { PcbaInspection } from '@/types/logistics/manufacturing/defect/pcba-inspection'

/** 主表选中行上下文 */
export interface PcbaInspectionMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PcbaInspection | null>
}

const pcbaInspectionMasterContextKey: InjectionKey<PcbaInspectionMasterContext> = Symbol('pcba-inspectionMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PcbaInspectionMasterContext} 主表上下文
 */
export function providePcbaInspectionMasterContext(): PcbaInspectionMasterContext {
  const selectedMasterRow = ref<PcbaInspection | null>(null)
  const ctx: PcbaInspectionMasterContext = { selectedMasterRow }
  provide(pcbaInspectionMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PcbaInspectionMasterContext} 主表上下文
 */
export function usePcbaInspectionMasterContext(): PcbaInspectionMasterContext {
  const ctx = inject(pcbaInspectionMasterContextKey)
  if (!ctx) {
    throw new Error('usePcbaInspectionMasterContext must be used within pcba-inspection index')
  }
  return ctx
}
