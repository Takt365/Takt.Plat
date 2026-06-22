// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/service/service-request/composables
// 文件名称：use-service-request-master-context.ts
// 功能描述：服务请求实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { ServiceRequest } from '@/types/logistics/customer-service/service-request'

/** 主表选中行上下文 */
export interface ServiceRequestMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ServiceRequest | null>
}

const serviceRequestMasterContextKey: InjectionKey<ServiceRequestMasterContext> = Symbol('service-requestMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ServiceRequestMasterContext} 主表上下文
 */
export function provideServiceRequestMasterContext(): ServiceRequestMasterContext {
  const selectedMasterRow = ref<ServiceRequest | null>(null)
  const ctx: ServiceRequestMasterContext = { selectedMasterRow }
  provide(serviceRequestMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ServiceRequestMasterContext} 主表上下文
 */
export function useServiceRequestMasterContext(): ServiceRequestMasterContext {
  const ctx = inject(serviceRequestMasterContextKey)
  if (!ctx) {
    throw new Error('useServiceRequestMasterContext must be used within service-request index')
  }
  return ctx
}
