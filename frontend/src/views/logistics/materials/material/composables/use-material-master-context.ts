// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/materials/material/composables
// 文件名称：use-material-master-context.ts
// 功能描述：Takt全局物料实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { Material } from '@/types/logistics/materials/material'

/** 主表选中行上下文 */
export interface MaterialMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<Material | null>
}

const materialMasterContextKey: InjectionKey<MaterialMasterContext> = Symbol('materialMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MaterialMasterContext} 主表上下文
 */
export function provideMaterialMasterContext(): MaterialMasterContext {
  const selectedMasterRow = ref<Material | null>(null)
  const ctx: MaterialMasterContext = { selectedMasterRow }
  provide(materialMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MaterialMasterContext} 主表上下文
 */
export function useMaterialMasterContext(): MaterialMasterContext {
  const ctx = inject(materialMasterContextKey)
  if (!ctx) {
    throw new Error('useMaterialMasterContext must be used within material index')
  }
  return ctx
}
