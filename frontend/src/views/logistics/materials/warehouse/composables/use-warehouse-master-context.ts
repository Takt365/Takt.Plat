// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/materials/warehouse/composables
// 文件名称：use-warehouse-master-context.ts
// 功能描述：Takt仓库主数据实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { Warehouse } from '@/types/logistics/materials/warehouse'

/** 主表选中行上下文 */
export interface WarehouseMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<Warehouse | null>
}

const warehouseMasterContextKey: InjectionKey<WarehouseMasterContext> = Symbol('warehouseMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {WarehouseMasterContext} 主表上下文
 */
export function provideWarehouseMasterContext(): WarehouseMasterContext {
  const selectedMasterRow = ref<Warehouse | null>(null)
  const ctx: WarehouseMasterContext = { selectedMasterRow }
  provide(warehouseMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {WarehouseMasterContext} 主表上下文
 */
export function useWarehouseMasterContext(): WarehouseMasterContext {
  const ctx = inject(warehouseMasterContextKey)
  if (!ctx) {
    throw new Error('useWarehouseMasterContext must be used within warehouse index')
  }
  return ctx
}
