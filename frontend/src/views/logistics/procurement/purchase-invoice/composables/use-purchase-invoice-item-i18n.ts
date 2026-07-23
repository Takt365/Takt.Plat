// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/procurement/purchase-invoice/composables
// 文件名称：use-purchase-invoice-item-i18n.ts
// 功能描述：PurchaseInvoiceItem字段清单 + usePurchaseInvoiceItemI18n（字段名映射一次，文案由 entity.purchaseinvoiceitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseInvoiceItemQuery } from '@/types/logistics/procurement/purchase-invoice-item'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktPurchaseInvoiceItemI18nSeedData 一致的实体 slug */
export const PURCHASEINVOICEITEM_ENTITY_SLUG = 'purchaseinvoiceitem'

/** entity.purchaseinvoiceitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEINVOICEITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEINVOICEITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEINVOICEITEM_LIST_FIELDS = [
  'purchaseInvoiceId',
  'purchaseInvoiceCode',
  'lineNumber',
  'purchaseOrderCode',
  'purchaseOrderLineNumber',
  'materialCode',
  'materialName',
  'materialSpecification',
  'purchaseUnit',
  'invoiceQuantity',
  'invoiceUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const PURCHASEINVOICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'purchaseInvoiceId',
  'purchaseInvoiceCode',
  'lineNumber',
  'purchaseOrderCode',
  'purchaseOrderLineNumber',
  'materialCode',
  'materialName',
  'materialSpecification',
  'purchaseUnit',
  'invoiceQuantity',
  'invoiceUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const PURCHASEINVOICEITEM_SUMMARY_SUM_FIELDS = [
  'purchaseOrderLineNumber',
  'invoiceQuantity',
  'invoiceUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEINVOICEITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  lineNumber: 'select',
  purchaseOrderCode: 'optional',
  purchaseOrderLineNumber: 'optional',
  materialCode: 'optional',
  materialName: 'optional',
  materialSpecification: 'optional',
  purchaseUnit: 'select',
  invoiceQuantity: 'select',
  invoiceUnitPrice: 'select',
  discountRate: 'select',
  discountAmount: 'select',
  taxIncludedAmount: 'select',
  untaxedAmount: 'select',
  taxAmount: 'select',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseInvoiceItemField = keyof typeof PURCHASEINVOICEITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEINVOICEITEM_QUERY_STRING_FIELDS = [
  'purchaseInvoiceCode',
  'purchaseOrderCode',
  'materialCode',
  'materialName',
  'materialSpecification',
  'purchaseUnit',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseInvoiceItemQuery)[]

export type PurchaseInvoiceItemQueryField =
  | (typeof PURCHASEINVOICEITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'purchaseOrderLineNumber' | 'invoiceQuantity' | 'invoiceUnitPrice' | 'discountRate' | 'discountAmount' | 'taxIncludedAmount' | 'untaxedAmount' | 'taxAmount' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEINVOICEITEM_QUERY_FIELDS: readonly PurchaseInvoiceItemQueryField[] = [
  ...PURCHASEINVOICEITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'purchaseOrderLineNumber',
  'invoiceQuantity',
  'invoiceUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'isObsolete',
]

/**
 * PurchaseInvoiceItem字段 i18n：index / purchase-invoice-item-form 统一入口
 */
export function usePurchaseInvoiceItemI18n() {
  const ef = useEntityFieldI18n(PURCHASEINVOICEITEM_ENTITY_SLUG)

  function ph(field: PurchaseInvoiceItemField): string {
    return ef.placeholder(field, PURCHASEINVOICEITEM_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseInvoiceItemQueryField, kind: EntityFieldPlaceholderKind): string {
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
