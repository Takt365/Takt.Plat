// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/sales/price/composables
// 文件名称：use-price-i18n.ts
// 功能描述：Takt销售价格实体字段清单 + useSalesPriceI18n（字段名映射一次，文案由 entity.salesprice.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesPriceQuery } from '@/types/logistics/sales/price'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktSalesPriceI18nSeedData 一致的实体 slug */
export const SALESPRICE_ENTITY_SLUG = 'salesprice'

/** entity.salesprice._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESPRICE_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESPRICE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESPRICE_LIST_FIELDS = [
  'plantCode',
  'salesPriceCode',
  'priceType',
  'customerCode',
  'materialCode',
  'salesGroup',
  'taxCode',
  'grBasedInvoiceInspection',
  'pricingDateControl',
  'validFrom',
  'validTo',
  'salesQuotationId',
  'salesQuotationCode',
  'variableKey',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESPRICE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  salesPriceCode: 'required',
  priceType: 'select',
  customerCode: 'select',
  materialCode: 'select',
  salesGroup: 'optional',
  taxCode: 'optional',
  grBasedInvoiceInspection: 'select',
  pricingDateControl: 'select',
  validFrom: 'select',
  validTo: 'select',
  salesQuotationId: 'optional',
  salesQuotationCode: 'optional',
  variableKey: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesPriceField = keyof typeof SALESPRICE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESPRICE_QUERY_STRING_FIELDS = [
  'plantCode',
  'salesPriceCode',
  'priceType',
  'customerCode',
  'materialCode',
  'salesGroup',
  'taxCode',
  'validFromStart',
  'validFromEnd',
  'validToStart',
  'validToEnd',
  'salesQuotationId',
  'salesQuotationCode',
  'variableKey',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesPriceQuery)[]

export type SalesPriceQueryField =
  | (typeof SALESPRICE_QUERY_STRING_FIELDS)[number]
  | 'grBasedInvoiceInspection' | 'pricingDateControl'

/** 高级查询抽屉全部字段（含数值） */
export const SALESPRICE_QUERY_FIELDS: readonly SalesPriceQueryField[] = [
  ...SALESPRICE_QUERY_STRING_FIELDS,
  'grBasedInvoiceInspection',
  'pricingDateControl',
]

/**
 * Takt销售价格实体字段 i18n：index / price-form 统一入口
 */
export function useSalesPriceI18n() {
  const ef = useEntityFieldI18n(SALESPRICE_ENTITY_SLUG)

  function ph(field: SalesPriceField): string {
    return ef.placeholder(field, SALESPRICE_PLACEHOLDER[field])
  }

  function queryPh(field: SalesPriceQueryField, kind: EntityFieldPlaceholderKind): string {
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
