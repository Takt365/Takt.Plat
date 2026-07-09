// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/materials/material-document-item/composables
// 文件名称：use-material-document-master-context.ts
// 功能描述：Takt物料凭证主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { MaterialTransaction } from '@/types/logistics/materials/material-document'

/** 主表选中行上下文 */
export interface MaterialTransactionMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<MaterialTransaction | null>
}

const materialDocumentMasterContextKey: InjectionKey<MaterialTransactionMasterContext> = Symbol('material-documentMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MaterialTransactionMasterContext} 主表上下文
 */
export function provideMaterialTransactionMasterContext(): MaterialTransactionMasterContext {
  const selectedMasterRow = ref<MaterialTransaction | null>(null)
  const ctx: MaterialTransactionMasterContext = { selectedMasterRow }
  provide(materialDocumentMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MaterialTransactionMasterContext} 主表上下文
 */
export function useMaterialTransactionMasterContext(): MaterialTransactionMasterContext {
  const ctx = inject(materialDocumentMasterContextKey)
  if (!ctx) {
    throw new Error('useMaterialTransactionMasterContext must be used within material-document index')
  }
  return ctx
}
