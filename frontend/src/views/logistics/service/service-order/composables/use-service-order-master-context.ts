// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/service/service-order/composables
// 文件名称：use-service-order-master-context.ts
// 功能描述：服务订单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { ServiceOrder } from '@/types/logistics/customer-service/service-order'

/** 主表选中行上下文 */
export interface ServiceOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ServiceOrder | null>
}

const serviceOrderMasterContextKey: InjectionKey<ServiceOrderMasterContext> = Symbol('service-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ServiceOrderMasterContext} 主表上下文
 */
export function provideServiceOrderMasterContext(): ServiceOrderMasterContext {
  const selectedMasterRow = ref<ServiceOrder | null>(null)
  const ctx: ServiceOrderMasterContext = { selectedMasterRow }
  provide(serviceOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ServiceOrderMasterContext} 主表上下文
 */
export function useServiceOrderMasterContext(): ServiceOrderMasterContext {
  const ctx = inject(serviceOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useServiceOrderMasterContext must be used within service-order index')
  }
  return ctx
}
