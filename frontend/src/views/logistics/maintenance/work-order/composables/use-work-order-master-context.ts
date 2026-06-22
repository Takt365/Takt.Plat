// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/maintenance/work-order/composables
// 文件名称：use-work-order-master-context.ts
// 功能描述：维护工单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { MaintenanceWorkOrder } from '@/types/logistics/maintenance/work-order'

/** 主表选中行上下文 */
export interface MaintenanceWorkOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<MaintenanceWorkOrder | null>
}

const maintenanceWorkOrderMasterContextKey: InjectionKey<MaintenanceWorkOrderMasterContext> = Symbol('work-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MaintenanceWorkOrderMasterContext} 主表上下文
 */
export function provideMaintenanceWorkOrderMasterContext(): MaintenanceWorkOrderMasterContext {
  const selectedMasterRow = ref<MaintenanceWorkOrder | null>(null)
  const ctx: MaintenanceWorkOrderMasterContext = { selectedMasterRow }
  provide(maintenanceWorkOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MaintenanceWorkOrderMasterContext} 主表上下文
 */
export function useMaintenanceWorkOrderMasterContext(): MaintenanceWorkOrderMasterContext {
  const ctx = inject(maintenanceWorkOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useMaintenanceWorkOrderMasterContext must be used within work-order index')
  }
  return ctx
}
