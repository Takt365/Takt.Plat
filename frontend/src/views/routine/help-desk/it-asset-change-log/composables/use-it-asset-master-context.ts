// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/routine/help-desk/it-asset-change-log/composables
// 文件名称：use-it-asset-master-context.ts
// 功能描述：服务台 IT 设备保修扩展实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { ItAsset } from '@/types/routine/help-desk/it-asset'

/** 主表选中行上下文 */
export interface ItAssetMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ItAsset | null>
}

const itAssetMasterContextKey: InjectionKey<ItAssetMasterContext> = Symbol('it-assetMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ItAssetMasterContext} 主表上下文
 */
export function provideItAssetMasterContext(): ItAssetMasterContext {
  const selectedMasterRow = ref<ItAsset | null>(null)
  const ctx: ItAssetMasterContext = { selectedMasterRow }
  provide(itAssetMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ItAssetMasterContext} 主表上下文
 */
export function useItAssetMasterContext(): ItAssetMasterContext {
  const ctx = inject(itAssetMasterContextKey)
  if (!ctx) {
    throw new Error('useItAssetMasterContext must be used within it-asset index')
  }
  return ctx
}
