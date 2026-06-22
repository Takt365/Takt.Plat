// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/routine/help-desk/knowledge-change-log/composables
// 文件名称：use-knowledge-master-context.ts
// 功能描述：服务台知识库实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { Knowledge } from '@/types/routine/help-desk/knowledge'

/** 主表选中行上下文 */
export interface KnowledgeMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<Knowledge | null>
}

const knowledgeMasterContextKey: InjectionKey<KnowledgeMasterContext> = Symbol('knowledgeMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {KnowledgeMasterContext} 主表上下文
 */
export function provideKnowledgeMasterContext(): KnowledgeMasterContext {
  const selectedMasterRow = ref<Knowledge | null>(null)
  const ctx: KnowledgeMasterContext = { selectedMasterRow }
  provide(knowledgeMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {KnowledgeMasterContext} 主表上下文
 */
export function useKnowledgeMasterContext(): KnowledgeMasterContext {
  const ctx = inject(knowledgeMasterContextKey)
  if (!ctx) {
    throw new Error('useKnowledgeMasterContext must be used within knowledge index')
  }
  return ctx
}
