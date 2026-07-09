// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/manufacturing/planning/production-plan/composables
// 文件名称：use-production-plan-item-i18n.ts
// 功能描述：ProductionPlanItem字段清单 + useProductionPlanItemI18n（字段名映射一次，文案由 entity.productionplanitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ProductionPlanItemQuery } from '@/types/logistics/manufacturing/planning/production-plan-item'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktProductionPlanItemI18nSeedData 一致的实体 slug */
export const PRODUCTIONPLANITEM_ENTITY_SLUG = 'productionplanitem'

/** entity.productionplanitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PRODUCTIONPLANITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PRODUCTIONPLANITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PRODUCTIONPLANITEM_LIST_FIELDS = [
  'productionPlanCode',
  'lineNumber',
  'salesPlanId',
  'salesPlanCode',
  'salesPlanLineNumber',
  'materialCode',
  'materialName',
  'materialSpecification',
  'planUnit',
  'planQuantity',
  'plannedStartDate',
  'plannedEndDate',
  'convertedQuantity',
  'estimatedUnitCost',
  'estimatedAmount',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PRODUCTIONPLANITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  lineNumber: 'select',
  salesPlanId: 'optional',
  salesPlanCode: 'optional',
  salesPlanLineNumber: 'optional',
  materialCode: 'select',
  materialName: 'required',
  materialSpecification: 'optional',
  planUnit: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ProductionPlanItemField = keyof typeof PRODUCTIONPLANITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PRODUCTIONPLANITEM_QUERY_STRING_FIELDS = [
  'productionPlanCode',
  'salesPlanId',
  'salesPlanCode',
  'materialCode',
  'materialName',
  'materialSpecification',
  'planUnit',
  'plannedStartDateStart',
  'plannedStartDateEnd',
  'plannedEndDateStart',
  'plannedEndDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ProductionPlanItemQuery)[]

export type ProductionPlanItemQueryField =
  | (typeof PRODUCTIONPLANITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'salesPlanLineNumber' | 'planQuantity' | 'convertedQuantity' | 'estimatedUnitCost' | 'estimatedAmount'

/** 高级查询抽屉全部字段（含数值） */
export const PRODUCTIONPLANITEM_QUERY_FIELDS: readonly ProductionPlanItemQueryField[] = [
  ...PRODUCTIONPLANITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'salesPlanLineNumber',
  'planQuantity',
  'convertedQuantity',
  'estimatedUnitCost',
  'estimatedAmount',
]

/**
 * ProductionPlanItem字段 i18n：index / production-plan-item-form 统一入口
 */
export function useProductionPlanItemI18n() {
  const ef = useEntityFieldI18n(PRODUCTIONPLANITEM_ENTITY_SLUG)

  function ph(field: ProductionPlanItemField): string {
    return ef.placeholder(field, PRODUCTIONPLANITEM_PLACEHOLDER[field])
  }

  function queryPh(field: ProductionPlanItemQueryField, kind: EntityFieldPlaceholderKind): string {
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
