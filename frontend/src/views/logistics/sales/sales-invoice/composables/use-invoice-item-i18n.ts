// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/sales/sales-invoice/composables
// 文件名称：use-invoice-item-i18n.ts
// 功能描述：SalesInvoiceItem字段清单 + useSalesInvoiceItemI18n（字段名映射一次，文案由 entity.salesinvoiceitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesInvoiceItemQuery } from '@/types/logistics/sales/invoice-item'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktSalesInvoiceItemI18nSeedData 一致的实体 slug */
export const SALESINVOICEITEM_ENTITY_SLUG = 'salesinvoiceitem'

/** entity.salesinvoiceitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESINVOICEITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESINVOICEITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESINVOICEITEM_LIST_FIELDS = [
  'salesInvoiceName',
  'accountingDocumentCode',
  'lineNumber',
  'postingDate',
  'currency',
  'modelName',
  'materialCode',
  'materialType',
  'materialName',
  'profitCenterCode',
  'accountTitle',
  'quantity',
  'unit',
  'localCurrencyAmount',
  'transactionCurrencyAmount',
  'documentType',
  'referenceDocumentCode',
  'referenceDocumentItem',
  'salesInvoice',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESINVOICEITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  accountingDocumentCode: 'required',
  lineNumber: 'select',
  postingDate: 'select',
  currency: 'select',
  modelName: 'optional',
  materialCode: 'select',
  materialType: 'select',
  materialName: 'required',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesInvoiceItemField = keyof typeof SALESINVOICEITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESINVOICEITEM_QUERY_STRING_FIELDS = [
  'accountingDocumentCode',
  'postingDateStart',
  'postingDateEnd',
  'currency',
  'modelName',
  'materialCode',
  'materialType',
  'materialName',
  'profitCenterCode',
  'accountTitle',
  'unit',
  'documentType',
  'referenceDocumentCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesInvoiceItemQuery)[]

export type SalesInvoiceItemQueryField =
  | (typeof SALESINVOICEITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'quantity' | 'localCurrencyAmount' | 'transactionCurrencyAmount' | 'referenceDocumentItem'

/** 高级查询抽屉全部字段（含数值） */
export const SALESINVOICEITEM_QUERY_FIELDS: readonly SalesInvoiceItemQueryField[] = [
  ...SALESINVOICEITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'quantity',
  'localCurrencyAmount',
  'transactionCurrencyAmount',
  'referenceDocumentItem',
]

/**
 * SalesInvoiceItem字段 i18n：index / invoice-item-form 统一入口
 */
export function useSalesInvoiceItemI18n() {
  const ef = useEntityFieldI18n(SALESINVOICEITEM_ENTITY_SLUG)

  function ph(field: SalesInvoiceItemField): string {
    return ef.placeholder(field, SALESINVOICEITEM_PLACEHOLDER[field])
  }

  function queryPh(field: SalesInvoiceItemQueryField, kind: EntityFieldPlaceholderKind): string {
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
