// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/manufacturing/planning/production-plan/composables
// 文件名称：use-production-plan-i18n.ts
// 功能描述：Takt生产计划实体字段清单 + useProductionPlanI18n（字段名映射一次，文案由 entity.productionplan.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ProductionPlanQuery } from '@/types/logistics/manufacturing/planning/production-plan'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktProductionPlanI18nSeedData 一致的实体 slug */
export const PRODUCTIONPLAN_ENTITY_SLUG = 'productionplan'

/** entity.productionplan._self 静态属性（导入组件 entity-i18n-key 等） */
export const PRODUCTIONPLAN_SELF_I18N_KEY = buildEntitySelfI18nKey(PRODUCTIONPLAN_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PRODUCTIONPLAN_LIST_FIELDS = [
  'plantCode',
  'productionPlanCode',
  'salesPlanId',
  'salesPlanCode',
  'masterProductionScheduleId',
  'mpsCode',
  'planDate',
  'planPeriodStart',
  'planPeriodEnd',
  'plannerId',
  'planBy',
  'totalQuantity',
  'totalAmount',
  'convertedQuantity',
  'convertedAmount',
  'planStatus',
  'convertedStatus',
  'planDescription',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PRODUCTIONPLAN_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  productionPlanCode: 'required',
  salesPlanId: 'optional',
  salesPlanCode: 'optional',
  masterProductionScheduleId: 'optional',
  mpsCode: 'optional',
  planDate: 'select',
  planPeriodStart: 'select',
  planPeriodEnd: 'select',
  plannerId: 'optional',
  planBy: 'select',
  totalQuantity: 'select',
  totalAmount: 'select',
  convertedQuantity: 'select',
  convertedAmount: 'select',
  planStatus: 'select',
  convertedStatus: 'select',
  planDescription: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ProductionPlanField = keyof typeof PRODUCTIONPLAN_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PRODUCTIONPLAN_QUERY_STRING_FIELDS = [
  'plantCode',
  'productionPlanCode',
  'salesPlanId',
  'salesPlanCode',
  'masterProductionScheduleId',
  'mpsCode',
  'planDateStart',
  'planDateEnd',
  'planPeriodStartStart',
  'planPeriodStartEnd',
  'planPeriodEndStart',
  'planPeriodEndEnd',
  'plannerId',
  'planBy',
  'planDescription',
  'initiatorId',
  'initiatedAtStart',
  'initiatedAtEnd',
  'approvedBy',
  'approvedAtStart',
  'approvedAtEnd',
  'flowInstanceId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ProductionPlanQuery)[]

export type ProductionPlanQueryField =
  | (typeof PRODUCTIONPLAN_QUERY_STRING_FIELDS)[number]
  | 'totalQuantity' | 'totalAmount' | 'convertedQuantity' | 'convertedAmount' | 'planStatus' | 'convertedStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PRODUCTIONPLAN_QUERY_FIELDS: readonly ProductionPlanQueryField[] = [
  ...PRODUCTIONPLAN_QUERY_STRING_FIELDS,
  'totalQuantity',
  'totalAmount',
  'convertedQuantity',
  'convertedAmount',
  'planStatus',
  'convertedStatus',
  'approvalStatus',
]

/**
 * Takt生产计划实体字段 i18n：index / production-plan-form 统一入口
 */
export function useProductionPlanI18n() {
  const ef = useEntityFieldI18n(PRODUCTIONPLAN_ENTITY_SLUG)

  function ph(field: ProductionPlanField): string {
    return ef.placeholder(field, PRODUCTIONPLAN_PLACEHOLDER[field])
  }

  function queryPh(field: ProductionPlanQueryField, kind: EntityFieldPlaceholderKind): string {
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
