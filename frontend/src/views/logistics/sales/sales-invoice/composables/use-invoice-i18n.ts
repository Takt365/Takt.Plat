// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/sales/sales-invoice/composables
// 文件名称：use-invoice-i18n.ts
// 功能描述：Takt销售发票实体字段清单 + useSalesInvoiceI18n（字段名映射一次，文案由 entity.salesinvoice.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesInvoiceQuery } from '@/types/logistics/sales/invoice'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktSalesInvoiceI18nSeedData 一致的实体 slug */
export const SALESINVOICE_ENTITY_SLUG = 'salesinvoice'

/** entity.salesinvoice._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESINVOICE_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESINVOICE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESINVOICE_LIST_FIELDS = [
  'plantCode',
  'yearMonth',
  'customerCode',
  'customerName1',
  'currencyCode',
  'taxRate',
  'taxAmount',
  'accountingDocumentCode',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESINVOICE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  yearMonth: 'required',
  customerCode: 'select',
  customerName1: 'required',
  currencyCode: 'select',
  taxRate: 'select',
  taxAmount: 'select',
  accountingDocumentCode: 'required',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesInvoiceField = keyof typeof SALESINVOICE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESINVOICE_QUERY_STRING_FIELDS = [
  'plantCode',
  'yearMonth',
  'customerCode',
  'customerName1',
  'currencyCode',
  'accountingDocumentCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesInvoiceQuery)[]

export type SalesInvoiceQueryField =
  | (typeof SALESINVOICE_QUERY_STRING_FIELDS)[number]
  | 'taxRate' | 'taxAmount'

/** 高级查询抽屉全部字段（含数值） */
export const SALESINVOICE_QUERY_FIELDS: readonly SalesInvoiceQueryField[] = [
  ...SALESINVOICE_QUERY_STRING_FIELDS,
  'taxRate',
  'taxAmount',
]

/**
 * Takt销售发票实体字段 i18n：index / invoice-form 统一入口
 */
export function useSalesInvoiceI18n() {
  const ef = useEntityFieldI18n(SALESINVOICE_ENTITY_SLUG)

  function ph(field: SalesInvoiceField): string {
    return ef.placeholder(field, SALESINVOICE_PLACEHOLDER[field])
  }

  function queryPh(field: SalesInvoiceQueryField, kind: EntityFieldPlaceholderKind): string {
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
