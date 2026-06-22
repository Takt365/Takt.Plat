// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/materials/manufacturer/composables
// 文件名称：use-manufacturer-master-context.ts
// 功能描述：Takt制造商实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { Manufacturer } from '@/types/logistics/materials/manufacturer'

/** 主表选中行上下文 */
export interface ManufacturerMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<Manufacturer | null>
}

const manufacturerMasterContextKey: InjectionKey<ManufacturerMasterContext> = Symbol('manufacturerMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ManufacturerMasterContext} 主表上下文
 */
export function provideManufacturerMasterContext(): ManufacturerMasterContext {
  const selectedMasterRow = ref<Manufacturer | null>(null)
  const ctx: ManufacturerMasterContext = { selectedMasterRow }
  provide(manufacturerMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ManufacturerMasterContext} 主表上下文
 */
export function useManufacturerMasterContext(): ManufacturerMasterContext {
  const ctx = inject(manufacturerMasterContextKey)
  if (!ctx) {
    throw new Error('useManufacturerMasterContext must be used within manufacturer index')
  }
  return ctx
}
