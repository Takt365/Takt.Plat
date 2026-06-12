// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/composables
// 文件名称：use-bill-of-material-master-context.ts
// 功能描述：制造 BOM 主表选中行上下文（供底部明细面板读取，对齐 Vue.NetCore getBomSelectRow）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { BillOfMaterial } from '@/types/logistics/manufacturing/bom/bill-of-material'

/** 主表选中行上下文 */
export interface BillOfMaterialMasterContext {
  /** 当前选中的 BOM 主表行（底部明细依赖） */
  selectedMasterRow: Ref<BillOfMaterial | null>
}

const billOfMaterialMasterContextKey: InjectionKey<BillOfMaterialMasterContext> = Symbol(
  'billOfMaterialMasterContext'
)

/**
 * 在 BOM 主表页 provide 选中行上下文
 * @returns {BillOfMaterialMasterContext} 主表上下文
 */
export function provideBillOfMaterialMasterContext(): BillOfMaterialMasterContext {
  const selectedMasterRow = ref<BillOfMaterial | null>(null)
  const ctx: BillOfMaterialMasterContext = { selectedMasterRow }
  provide(billOfMaterialMasterContextKey, ctx)
  return ctx
}

/**
 * 在 BOM 明细面板 inject 主表选中行
 * @returns {BillOfMaterialMasterContext} 主表上下文
 */
export function useBillOfMaterialMasterContext(): BillOfMaterialMasterContext {
  const ctx = inject(billOfMaterialMasterContextKey)
  if (!ctx) {
    throw new Error('useBillOfMaterialMasterContext must be used within bill-of-material index')
  }
  return ctx
}
