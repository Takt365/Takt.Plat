// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/sales/order/composables
// 文件名称：use-order-i18n.ts
// 功能描述：APS 排程订单字段清单 + useApsOrderI18n（字段名映射一次，文案由 entity.apsorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ApsOrderQuery } from '@/types/logistics/manufacturing/aps/order'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktApsOrderI18nSeedData 一致的实体 slug */
export const APSORDER_ENTITY_SLUG = 'apsorder'

/** entity.apsorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const APSORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(APSORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const APSORDER_LIST_FIELDS = [
  'plantCode',
  'apsOrderCode',
  'plannedOrderId',
  'plannedOrderCode',
  'materialCode',
  'orderQuantity',
  'unitOfMeasure',
  'routingCode',
  'plannedStartTime',
  'plannedEndTime',
  'orderStatus',
  'apsScheduleId',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const APSORDER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  apsOrderCode: 'required',
  plannedOrderId: 'optional',
  plannedOrderCode: 'optional',
  materialCode: 'select',
  orderQuantity: 'select',
  unitOfMeasure: 'select',
  routingCode: 'optional',
  plannedStartTime: 'optional',
  plannedEndTime: 'optional',
  orderStatus: 'select',
  apsScheduleId: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ApsOrderField = keyof typeof APSORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const APSORDER_QUERY_STRING_FIELDS = [
  'plantCode',
  'apsOrderCode',
  'plannedOrderId',
  'plannedOrderCode',
  'materialCode',
  'unitOfMeasure',
  'routingCode',
  'plannedStartTimeStart',
  'plannedStartTimeEnd',
  'plannedEndTimeStart',
  'plannedEndTimeEnd',
  'apsScheduleId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ApsOrderQuery)[]

export type ApsOrderQueryField =
  | (typeof APSORDER_QUERY_STRING_FIELDS)[number]
  | 'orderQuantity' | 'orderStatus'

/** 高级查询抽屉全部字段（含数值） */
export const APSORDER_QUERY_FIELDS: readonly ApsOrderQueryField[] = [
  ...APSORDER_QUERY_STRING_FIELDS,
  'orderQuantity',
  'orderStatus',
]

/**
 * APS 排程订单字段 i18n：index / order-form 统一入口
 */
export function useApsOrderI18n() {
  const ef = useEntityFieldI18n(APSORDER_ENTITY_SLUG)

  function ph(field: ApsOrderField): string {
    return ef.placeholder(field, APSORDER_PLACEHOLDER[field])
  }

  function queryPh(field: ApsOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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
