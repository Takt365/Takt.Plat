// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/procurement/purchase-price-item/composables
// 文件名称：use-purchase-price-scale-quantity-i18n.ts
// 功能描述：PurchasePriceScaleQuantity字段清单 + usePurchasePriceScaleQuantityI18n（字段名映射一次，文案由 entity.purchasepricescalequantity.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchasePriceScaleQuantityQuery } from '@/types/logistics/procurement/purchase-price-scale-quantity'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktPurchasePriceScaleQuantityI18nSeedData 一致的实体 slug */
export const PURCHASEPRICESCALEQUANTITY_ENTITY_SLUG = 'purchasepricescalequantity'

/** entity.purchasepricescalequantity._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEPRICESCALEQUANTITY_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEPRICESCALEQUANTITY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEPRICESCALEQUANTITY_LIST_FIELDS = [
  'purchasePriceItemId',
  'purchasePriceCode',
  'purchasePriceSeq',
  'purchaseScaleSeq',
  'scaleQuantity',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const PURCHASEPRICESCALEQUANTITY_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'purchasePriceItemId',
  'purchasePriceCode',
  'purchasePriceSeq',
  'purchaseScaleSeq',
  'scaleQuantity',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const PURCHASEPRICESCALEQUANTITY_SUMMARY_SUM_FIELDS = [
  'purchasePriceSeq',
  'purchaseScaleSeq',
  'scaleQuantity',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEPRICESCALEQUANTITY_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  purchasePriceCode: 'required',
  purchasePriceSeq: 'select',
  purchaseScaleSeq: 'select',
  scaleQuantity: 'select',
  price: 'select',
  untaxedPrice: 'select',
  taxIncludedPrice: 'select',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchasePriceScaleQuantityField = keyof typeof PURCHASEPRICESCALEQUANTITY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEPRICESCALEQUANTITY_QUERY_STRING_FIELDS = [
  'purchasePriceCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchasePriceScaleQuantityQuery)[]

export type PurchasePriceScaleQuantityQueryField =
  | (typeof PURCHASEPRICESCALEQUANTITY_QUERY_STRING_FIELDS)[number]
  | 'purchasePriceSeq' | 'purchaseScaleSeq' | 'scaleQuantity' | 'price' | 'untaxedPrice' | 'taxIncludedPrice' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEPRICESCALEQUANTITY_QUERY_FIELDS: readonly PurchasePriceScaleQuantityQueryField[] = [
  ...PURCHASEPRICESCALEQUANTITY_QUERY_STRING_FIELDS,
  'purchasePriceSeq',
  'purchaseScaleSeq',
  'scaleQuantity',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'isObsolete',
]

/**
 * PurchasePriceScaleQuantity字段 i18n：index / purchase-price-scale-quantity-form 统一入口
 */
export function usePurchasePriceScaleQuantityI18n() {
  const ef = useEntityFieldI18n(PURCHASEPRICESCALEQUANTITY_ENTITY_SLUG)

  function ph(field: PurchasePriceScaleQuantityField): string {
    return ef.placeholder(field, PURCHASEPRICESCALEQUANTITY_PLACEHOLDER[field])
  }

  function queryPh(field: PurchasePriceScaleQuantityQueryField, kind: EntityFieldPlaceholderKind): string {
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
