// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/procurement/purchase-order/composables
// 文件名称：use-purchase-order-item-i18n.ts
// 功能描述：PurchaseOrderItem字段清单 + usePurchaseOrderItemI18n（字段名映射一次，文案由 entity.purchaseorderitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseOrderItemQuery } from '@/types/logistics/procurement/purchase-order-item'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktPurchaseOrderItemI18nSeedData 一致的实体 slug */
export const PURCHASEORDERITEM_ENTITY_SLUG = 'purchaseorderitem'

/** entity.purchaseorderitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEORDERITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEORDERITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEORDERITEM_LIST_FIELDS = [
  'purchaseOrderId',
  'purchaseOrderCode',
  'lineNumber',
  'requestCode',
  'requestLineNumber',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'purchaseUnit',
  'orderQuantity',
  'receivedQuantity',
  'purchasePerUnit',
  'purchaseUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'purchaseAmount',
  'deliveryStatus',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const PURCHASEORDERITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'purchaseOrderId',
  'purchaseOrderCode',
  'lineNumber',
  'requestCode',
  'requestLineNumber',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'purchaseUnit',
  'orderQuantity',
  'receivedQuantity',
  'purchasePerUnit',
  'purchaseUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'purchaseAmount',
  'deliveryStatus',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const PURCHASEORDERITEM_SUMMARY_SUM_FIELDS = [
  'requestLineNumber',
  'orderQuantity',
  'receivedQuantity',
  'purchasePerUnit',
  'purchaseUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'purchaseAmount',
  'deliveryStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEORDERITEM_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseOrderItemField = keyof typeof PURCHASEORDERITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEORDERITEM_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof PurchaseOrderItemQuery)[]

export type PurchaseOrderItemQueryField = (typeof PURCHASEORDERITEM_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEORDERITEM_QUERY_FIELDS: readonly PurchaseOrderItemQueryField[] = [...PURCHASEORDERITEM_QUERY_STRING_FIELDS]

/**
 * PurchaseOrderItem字段 i18n：index / purchase-order-item-form 统一入口
 */
export function usePurchaseOrderItemI18n() {
  const ef = useEntityFieldI18n(PURCHASEORDERITEM_ENTITY_SLUG)

  function ph(field: PurchaseOrderItemField): string {
    return ef.placeholder(field, PURCHASEORDERITEM_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseOrderItemQueryField, kind: EntityFieldPlaceholderKind): string {
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
