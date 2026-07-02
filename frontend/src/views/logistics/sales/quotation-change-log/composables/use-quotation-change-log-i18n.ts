// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/sales/quotation-change-log/composables
// 文件名称：use-quotation-change-log-i18n.ts
// 功能描述：SalesQuotationChangeLog字段清单 + useSalesQuotationChangeLogI18n（字段名映射一次，文案由 entity.salesquotationchangelog.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesQuotationChangeLogQuery } from '@/types/logistics/sales/quotation-change-log'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktSalesQuotationChangeLogI18nSeedData 一致的实体 slug */
export const SALESQUOTATIONCHANGELOG_ENTITY_SLUG = 'salesquotationchangelog'

/** entity.salesquotationchangelog._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESQUOTATIONCHANGELOG_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESQUOTATIONCHANGELOG_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESQUOTATIONCHANGELOG_LIST_FIELDS = [
  'salesQuotationName',
  'salesQuotationCode',
  'changeFields',
  'changeTime',
  'changeBy',
  'changeReason',
  'salesQuotation',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESQUOTATIONCHANGELOG_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  changeFields: 'optional',
  changeTime: 'select',
  changeBy: 'optional',
  changeReason: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesQuotationChangeLogField = keyof typeof SALESQUOTATIONCHANGELOG_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESQUOTATIONCHANGELOG_QUERY_STRING_FIELDS = [
  'salesQuotationCode',
  'changeFields',
  'changeTimeStart',
  'changeTimeEnd',
  'changeBy',
  'changeReason',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesQuotationChangeLogQuery)[]

export type SalesQuotationChangeLogQueryField = (typeof SALESQUOTATIONCHANGELOG_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const SALESQUOTATIONCHANGELOG_QUERY_FIELDS: readonly SalesQuotationChangeLogQueryField[] = [...SALESQUOTATIONCHANGELOG_QUERY_STRING_FIELDS]

/**
 * SalesQuotationChangeLog字段 i18n：index / quotation-change-log-form 统一入口
 */
export function useSalesQuotationChangeLogI18n() {
  const ef = useEntityFieldI18n(SALESQUOTATIONCHANGELOG_ENTITY_SLUG)

  function ph(field: SalesQuotationChangeLogField): string {
    return ef.placeholder(field, SALESQUOTATIONCHANGELOG_PLACEHOLDER[field])
  }

  function queryPh(field: SalesQuotationChangeLogQueryField, kind: EntityFieldPlaceholderKind): string {
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
