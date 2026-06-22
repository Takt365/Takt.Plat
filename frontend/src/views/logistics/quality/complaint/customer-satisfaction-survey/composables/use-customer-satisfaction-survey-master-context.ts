// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/composables
// 文件名称：use-customer-satisfaction-survey-master-context.ts
// 功能描述：客户满意度调查表主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { CustomerSatisfactionSurvey } from '@/types/logistics/quality/complaint/customer-satisfaction-survey'

/** 主表选中行上下文 */
export interface CustomerSatisfactionSurveyMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<CustomerSatisfactionSurvey | null>
}

const customerSatisfactionSurveyMasterContextKey: InjectionKey<CustomerSatisfactionSurveyMasterContext> = Symbol('customer-satisfaction-surveyMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {CustomerSatisfactionSurveyMasterContext} 主表上下文
 */
export function provideCustomerSatisfactionSurveyMasterContext(): CustomerSatisfactionSurveyMasterContext {
  const selectedMasterRow = ref<CustomerSatisfactionSurvey | null>(null)
  const ctx: CustomerSatisfactionSurveyMasterContext = { selectedMasterRow }
  provide(customerSatisfactionSurveyMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {CustomerSatisfactionSurveyMasterContext} 主表上下文
 */
export function useCustomerSatisfactionSurveyMasterContext(): CustomerSatisfactionSurveyMasterContext {
  const ctx = inject(customerSatisfactionSurveyMasterContextKey)
  if (!ctx) {
    throw new Error('useCustomerSatisfactionSurveyMasterContext must be used within customer-satisfaction-survey index')
  }
  return ctx
}
