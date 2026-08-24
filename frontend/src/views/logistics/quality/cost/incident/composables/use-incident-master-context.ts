// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/quality/cost/incident/composables
// 文件名称：use-incident-master-context.ts
// 功能描述：品质事故主表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { QualityIncident } from '@/types/logistics/quality/cost/incident'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type QualityIncidentRowRecord = QualityIncident | Record<string, unknown>

/** 主表选中行上下文 */
export interface QualityIncidentMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<QualityIncidentRowRecord | null>
}

const qualityIncidentMasterContextKey: InjectionKey<QualityIncidentMasterContext> = Symbol('incidentMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {QualityIncidentMasterContext} 主表上下文
 */
export function provideQualityIncidentMasterContext(): QualityIncidentMasterContext {
  const selectedMasterRow = ref<QualityIncidentRowRecord | null>(null)
  const ctx: QualityIncidentMasterContext = { selectedMasterRow }
  provide(qualityIncidentMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {QualityIncidentMasterContext} 主表上下文
 */
export function useQualityIncidentMasterContext(): QualityIncidentMasterContext {
  const ctx = inject(qualityIncidentMasterContextKey)
  if (!ctx) {
    throw new Error('useQualityIncidentMasterContext must be used within incident index')
  }
  return ctx
}
