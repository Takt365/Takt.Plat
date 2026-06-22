// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/service/service-contract/composables
// 文件名称：use-service-contract-master-context.ts
// 功能描述：服务合同实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { ServiceContract } from '@/types/logistics/customer-service/service-contract'

/** 主表选中行上下文 */
export interface ServiceContractMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ServiceContract | null>
}

const serviceContractMasterContextKey: InjectionKey<ServiceContractMasterContext> = Symbol('service-contractMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ServiceContractMasterContext} 主表上下文
 */
export function provideServiceContractMasterContext(): ServiceContractMasterContext {
  const selectedMasterRow = ref<ServiceContract | null>(null)
  const ctx: ServiceContractMasterContext = { selectedMasterRow }
  provide(serviceContractMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ServiceContractMasterContext} 主表上下文
 */
export function useServiceContractMasterContext(): ServiceContractMasterContext {
  const ctx = inject(serviceContractMasterContextKey)
  if (!ctx) {
    throw new Error('useServiceContractMasterContext must be used within service-contract index')
  }
  return ctx
}
