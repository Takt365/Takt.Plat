// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/procurement/purchase-price-item/composables
// 文件名称：use-purchase-price-item-i18n.ts
// 功能描述：Takt采购价格明细实体字段清单 + usePurchasePriceItemI18n（字段名映射一次，文案由 entity.purchasepriceitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchasePriceItemQuery } from '@/types/logistics/procurement/purchase-price-item'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktPurchasePriceItemI18nSeedData 一致的实体 slug */
export const PURCHASEPRICEITEM_ENTITY_SLUG = 'purchasepriceitem'

/** entity.purchasepriceitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEPRICEITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEPRICEITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEPRICEITEM_LIST_FIELDS = [
  'purchasePriceId',
  'purchasePriceCode',
  'purchasePriceSeq',
  'priceType',
  'scaleType',
  'scaleBasis',
  'scaleQuantity',
  'scaleUnit',
  'scaleValue',
  'scaleCurrency',
  'calculationType',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'conditionCurrency',
  'priceUnit',
  'unitOfMeasure',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEPRICEITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  purchasePriceId: 'select',
  purchasePriceCode: 'required',
  purchasePriceSeq: 'select',
  priceType: 'select',
  scaleType: 'optional',
  scaleBasis: 'optional',
  scaleQuantity: 'select',
  scaleUnit: 'optional',
  scaleValue: 'select',
  scaleCurrency: 'optional',
  calculationType: 'select',
  price: 'select',
  untaxedPrice: 'select',
  taxIncludedPrice: 'select',
  conditionCurrency: 'select',
  priceUnit: 'select',
  unitOfMeasure: 'select',
  minOrderQuantity: 'select',
  roundingValue: 'select',
  plannedDeliveryTimeDays: 'select',
  isObsolete: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchasePriceItemField = keyof typeof PURCHASEPRICEITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEPRICEITEM_QUERY_STRING_FIELDS = [
  'purchasePriceId',
  'purchasePriceCode',
  'priceType',
  'scaleType',
  'scaleBasis',
  'scaleUnit',
  'scaleCurrency',
  'calculationType',
  'conditionCurrency',
  'unitOfMeasure',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchasePriceItemQuery)[]

export type PurchasePriceItemQueryField =
  | (typeof PURCHASEPRICEITEM_QUERY_STRING_FIELDS)[number]
  | 'purchasePriceSeq' | 'scaleQuantity' | 'scaleValue' | 'price' | 'untaxedPrice' | 'taxIncludedPrice' | 'priceUnit' | 'minOrderQuantity' | 'roundingValue' | 'plannedDeliveryTimeDays' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEPRICEITEM_QUERY_FIELDS: readonly PurchasePriceItemQueryField[] = [
  ...PURCHASEPRICEITEM_QUERY_STRING_FIELDS,
  'purchasePriceSeq',
  'scaleQuantity',
  'scaleValue',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'priceUnit',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'isObsolete',
]

/**
 * Takt采购价格明细实体字段 i18n：index / purchase-price-item-form 统一入口
 */
export function usePurchasePriceItemI18n() {
  const ef = useEntityFieldI18n(PURCHASEPRICEITEM_ENTITY_SLUG)

  function ph(field: PurchasePriceItemField): string {
    return ef.placeholder(field, PURCHASEPRICEITEM_PLACEHOLDER[field])
  }

  function queryPh(field: PurchasePriceItemQueryField, kind: EntityFieldPlaceholderKind): string {
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
