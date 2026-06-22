// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/accounting/financial/asset-change-log/composables
// 文件名称：use-asset-master-context.ts
// 功能描述：资产实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { Asset } from '@/types/accounting/financial/asset'

/** 主表选中行上下文 */
export interface AssetMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<Asset | null>
}

const assetMasterContextKey: InjectionKey<AssetMasterContext> = Symbol('assetMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {AssetMasterContext} 主表上下文
 */
export function provideAssetMasterContext(): AssetMasterContext {
  const selectedMasterRow = ref<Asset | null>(null)
  const ctx: AssetMasterContext = { selectedMasterRow }
  provide(assetMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {AssetMasterContext} 主表上下文
 */
export function useAssetMasterContext(): AssetMasterContext {
  const ctx = inject(assetMasterContextKey)
  if (!ctx) {
    throw new Error('useAssetMasterContext must be used within asset index')
  }
  return ctx
}
