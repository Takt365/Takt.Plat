// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/sales/price-item/composables
// 文件名称：use-price-scale-quantity-i18n.ts
// 功能描述：SalesPriceScaleQuantity字段清单 + useSalesPriceScaleQuantityI18n（字段名映射一次，文案由 entity.salespricescalequantity.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesPriceScaleQuantityQuery } from '@/types/logistics/sales/price-scale-quantity'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktSalesPriceScaleQuantityI18nSeedData 一致的实体 slug */
export const SALESPRICESCALEQUANTITY_ENTITY_SLUG = 'salespricescalequantity'

/** entity.salespricescalequantity._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESPRICESCALEQUANTITY_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESPRICESCALEQUANTITY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESPRICESCALEQUANTITY_LIST_FIELDS = [
  'salesPriceItemId',
  'salesPriceCode',
  'salesPriceSeq',
  'lineNumber',
  'scaleQuantity',
  'amount',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SALESPRICESCALEQUANTITY_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'salesPriceItemId',
  'salesPriceCode',
  'salesPriceSeq',
  'lineNumber',
  'scaleQuantity',
  'amount',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SALESPRICESCALEQUANTITY_SUMMARY_SUM_FIELDS = [
  'salesPriceSeq',
  'scaleQuantity',
  'amount',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESPRICESCALEQUANTITY_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  salesPriceCode: 'required',
  salesPriceSeq: 'select',
  lineNumber: 'select',
  scaleQuantity: 'select',
  amount: 'select',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesPriceScaleQuantityField = keyof typeof SALESPRICESCALEQUANTITY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESPRICESCALEQUANTITY_QUERY_STRING_FIELDS = [
  'salesPriceCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesPriceScaleQuantityQuery)[]

export type SalesPriceScaleQuantityQueryField =
  | (typeof SALESPRICESCALEQUANTITY_QUERY_STRING_FIELDS)[number]
  | 'salesPriceSeq' | 'lineNumber' | 'scaleQuantity' | 'amount' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const SALESPRICESCALEQUANTITY_QUERY_FIELDS: readonly SalesPriceScaleQuantityQueryField[] = [
  ...SALESPRICESCALEQUANTITY_QUERY_STRING_FIELDS,
  'salesPriceSeq',
  'lineNumber',
  'scaleQuantity',
  'amount',
  'isObsolete',
]

/**
 * SalesPriceScaleQuantity字段 i18n：index / price-scale-quantity-form 统一入口
 */
export function useSalesPriceScaleQuantityI18n() {
  const ef = useEntityFieldI18n(SALESPRICESCALEQUANTITY_ENTITY_SLUG)

  function ph(field: SalesPriceScaleQuantityField): string {
    return ef.placeholder(field, SALESPRICESCALEQUANTITY_PLACEHOLDER[field])
  }

  function queryPh(field: SalesPriceScaleQuantityQueryField, kind: EntityFieldPlaceholderKind): string {
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
