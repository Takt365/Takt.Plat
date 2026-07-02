// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/sales/price-item/composables
// 文件名称：use-price-scale-i18n.ts
// 功能描述：SalesPriceScale字段清单 + useSalesPriceScaleI18n（字段名映射一次，文案由 entity.salespricescale.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesPriceScaleQuery } from '@/types/logistics/sales/price-scale'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktSalesPriceScaleI18nSeedData 一致的实体 slug */
export const SALESPRICESCALE_ENTITY_SLUG = 'salespricescale'

/** entity.salespricescale._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESPRICESCALE_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESPRICESCALE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESPRICESCALE_LIST_FIELDS = [
  'itemId',
  'salesPriceCode',
  'lineNumber',
  'startQuantity',
  'endQuantity',
  'scalePrice',
  'priceItem',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESPRICESCALE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  itemId: 'select',
  salesPriceCode: 'required',
  lineNumber: 'select',
  startQuantity: 'select',
  endQuantity: 'select',
  scalePrice: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesPriceScaleField = keyof typeof SALESPRICESCALE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESPRICESCALE_QUERY_STRING_FIELDS = [
  'itemId',
  'salesPriceCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesPriceScaleQuery)[]

export type SalesPriceScaleQueryField =
  | (typeof SALESPRICESCALE_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'startQuantity' | 'endQuantity' | 'scalePrice'

/** 高级查询抽屉全部字段（含数值） */
export const SALESPRICESCALE_QUERY_FIELDS: readonly SalesPriceScaleQueryField[] = [
  ...SALESPRICESCALE_QUERY_STRING_FIELDS,
  'lineNumber',
  'startQuantity',
  'endQuantity',
  'scalePrice',
]

/**
 * SalesPriceScale字段 i18n：index / price-scale-form 统一入口
 */
export function useSalesPriceScaleI18n() {
  const ef = useEntityFieldI18n(SALESPRICESCALE_ENTITY_SLUG)

  function ph(field: SalesPriceScaleField): string {
    return ef.placeholder(field, SALESPRICESCALE_PLACEHOLDER[field])
  }

  function queryPh(field: SalesPriceScaleQueryField, kind: EntityFieldPlaceholderKind): string {
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
