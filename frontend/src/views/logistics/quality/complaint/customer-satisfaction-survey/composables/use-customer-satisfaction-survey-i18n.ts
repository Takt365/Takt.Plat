// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/composables
// 文件名称：use-customer-satisfaction-survey-i18n.ts
// 功能描述：客户满意度调查表主表实体字段清单 + useCustomerSatisfactionSurveyI18n（字段名映射一次，文案由 entity.customersatisfactionsurvey.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerSatisfactionSurveyQuery } from '@/types/logistics/quality/complaint/customer-satisfaction-survey'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktCustomerSatisfactionSurveyI18nSeedData 一致的实体 slug */
export const CUSTOMERSATISFACTIONSURVEY_ENTITY_SLUG = 'customersatisfactionsurvey'

/** entity.customersatisfactionsurvey._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERSATISFACTIONSURVEY_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERSATISFACTIONSURVEY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERSATISFACTIONSURVEY_LIST_FIELDS = [
  'customerSatisfactionSurveyCode',
  'customerId',
  'customerName1',
  'customerCode',
  'surveyDate',
  'surveyMethod',
  'surveyType',
  'surveyPeriod',
  'surveyorBy',
  'customerContact',
  'customerPhone',
  'overallSatisfaction',
  'totalScore',
  'qualityScore',
  'deliveryScore',
  'serviceScore',
  'priceScore',
  'technicalScore',
  'customerPraise',
  'customerFeedback',
  'improvementPlan',
  'relatedComplaintId',
  'attachments',
  'surveyStatus',
  'relatedPlant',
  'followUpStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERSATISFACTIONSURVEY_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  customerSatisfactionSurveyCode: 'required',
  customerId: 'select',
  customerName1: 'required',
  customerCode: 'optional',
  surveyDate: 'select',
  surveyMethod: 'select',
  surveyType: 'select',
  surveyPeriod: 'select',
  surveyorBy: 'optional',
  customerContact: 'optional',
  customerPhone: 'optional',
  overallSatisfaction: 'select',
  totalScore: 'optional',
  qualityScore: 'optional',
  deliveryScore: 'optional',
  serviceScore: 'optional',
  priceScore: 'optional',
  technicalScore: 'optional',
  customerPraise: 'optional',
  customerFeedback: 'optional',
  improvementPlan: 'optional',
  relatedComplaintId: 'optional',
  attachments: 'optional',
  surveyStatus: 'select',
  relatedPlant: 'select',
  followUpStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerSatisfactionSurveyField = keyof typeof CUSTOMERSATISFACTIONSURVEY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS = [
  'customerSatisfactionSurveyCode',
  'customerId',
  'customerName1',
  'customerCode',
  'surveyDateStart',
  'surveyDateEnd',
  'surveyorBy',
  'customerContact',
  'customerPhone',
  'customerPraise',
  'customerFeedback',
  'improvementPlan',
  'relatedComplaintId',
  'attachments',
  'relatedPlant',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CustomerSatisfactionSurveyQuery)[]

export type CustomerSatisfactionSurveyQueryField =
  | (typeof CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS)[number]
  | 'surveyMethod' | 'surveyType' | 'surveyPeriod' | 'overallSatisfaction' | 'totalScore' | 'qualityScore' | 'deliveryScore' | 'serviceScore' | 'priceScore' | 'technicalScore' | 'surveyStatus' | 'followUpStatus'

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERSATISFACTIONSURVEY_QUERY_FIELDS: readonly CustomerSatisfactionSurveyQueryField[] = [
  ...CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS,
  'surveyMethod',
  'surveyType',
  'surveyPeriod',
  'overallSatisfaction',
  'totalScore',
  'qualityScore',
  'deliveryScore',
  'serviceScore',
  'priceScore',
  'technicalScore',
  'surveyStatus',
  'followUpStatus',
]

/**
 * 客户满意度调查表主表实体字段 i18n：index / customer-satisfaction-survey-form 统一入口
 */
export function useCustomerSatisfactionSurveyI18n() {
  const ef = useEntityFieldI18n(CUSTOMERSATISFACTIONSURVEY_ENTITY_SLUG)

  function ph(field: CustomerSatisfactionSurveyField): string {
    return ef.placeholder(field, CUSTOMERSATISFACTIONSURVEY_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerSatisfactionSurveyQueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
  }
}
