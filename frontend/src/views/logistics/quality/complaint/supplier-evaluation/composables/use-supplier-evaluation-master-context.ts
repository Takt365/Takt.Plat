// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/composables
// 文件名称：use-supplier-evaluation-master-context.ts
// 功能描述：供应商评价考核主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { SupplierEvaluation } from '@/types/logistics/quality/complaint/supplier-evaluation'

/** 主表选中行上下文 */
export interface SupplierEvaluationMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SupplierEvaluation | null>
}

const supplierEvaluationMasterContextKey: InjectionKey<SupplierEvaluationMasterContext> = Symbol('supplier-evaluationMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SupplierEvaluationMasterContext} 主表上下文
 */
export function provideSupplierEvaluationMasterContext(): SupplierEvaluationMasterContext {
  const selectedMasterRow = ref<SupplierEvaluation | null>(null)
  const ctx: SupplierEvaluationMasterContext = { selectedMasterRow }
  provide(supplierEvaluationMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SupplierEvaluationMasterContext} 主表上下文
 */
export function useSupplierEvaluationMasterContext(): SupplierEvaluationMasterContext {
  const ctx = inject(supplierEvaluationMasterContextKey)
  if (!ctx) {
    throw new Error('useSupplierEvaluationMasterContext must be used within supplier-evaluation index')
  }
  return ctx
}
