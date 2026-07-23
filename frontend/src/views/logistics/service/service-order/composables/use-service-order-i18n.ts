// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/service/service-order/composables
// 文件名称：use-service-order-i18n.ts
// 功能描述：服务订单实体字段清单 + useServiceOrderI18n（字段名映射一次，文案由 entity.serviceorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ServiceOrderQuery } from '@/types/logistics/customer-service/service-order'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktServiceOrderI18nSeedData 一致的实体 slug */
export const SERVICEORDER_ENTITY_SLUG = 'serviceorder'

/** entity.serviceorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const SERVICEORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(SERVICEORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SERVICEORDER_LIST_FIELDS = [
  'plantCode',
  'serviceOrderCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceContractId',
  'serviceContractCode',
  'serviceRequestId',
  'serviceRequestCode',
  'orderDate',
  'orderType',
  'orderStatus',
  'totalAmount',
  'discountAmount',
  'taxAmount',
  'actualAmount',
  'currencyCode',
  'plannedStartDate',
  'plannedEndDate',
  'actualStartDate',
  'actualEndDate',
  'serviceBy',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SERVICEORDER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'required',
  serviceOrderCode: 'required',
  clientId: 'required',
  clientCode: 'required',
  clientName1: 'required',
  serviceContractId: 'optional',
  serviceContractCode: 'optional',
  serviceRequestId: 'optional',
  serviceRequestCode: 'optional',
  orderDate: 'select',
  orderType: 'select',
  orderStatus: 'select',
  totalAmount: 'select',
  discountAmount: 'select',
  taxAmount: 'select',
  actualAmount: 'select',
  currencyCode: 'required',
  plannedStartDate: 'optional',
  plannedEndDate: 'optional',
  actualStartDate: 'optional',
  actualEndDate: 'optional',
  serviceBy: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ServiceOrderField = keyof typeof SERVICEORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SERVICEORDER_QUERY_STRING_FIELDS = [
  'plantCode',
  'serviceOrderCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceContractId',
  'serviceContractCode',
  'serviceRequestId',
  'serviceRequestCode',
  'orderDateStart',
  'orderDateEnd',
  'currencyCode',
  'plannedStartDateStart',
  'plannedStartDateEnd',
  'plannedEndDateStart',
  'plannedEndDateEnd',
  'actualStartDateStart',
  'actualStartDateEnd',
  'actualEndDateStart',
  'actualEndDateEnd',
  'serviceBy',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ServiceOrderQuery)[]

export type ServiceOrderQueryField =
  | (typeof SERVICEORDER_QUERY_STRING_FIELDS)[number]
  | 'orderType' | 'orderStatus' | 'totalAmount' | 'discountAmount' | 'taxAmount' | 'actualAmount'

/** 高级查询抽屉全部字段（含数值） */
export const SERVICEORDER_QUERY_FIELDS: readonly ServiceOrderQueryField[] = [
  ...SERVICEORDER_QUERY_STRING_FIELDS,
  'orderType',
  'orderStatus',
  'totalAmount',
  'discountAmount',
  'taxAmount',
  'actualAmount',
]

/**
 * 服务订单实体字段 i18n：index / service-order-form 统一入口
 */
export function useServiceOrderI18n() {
  const ef = useEntityFieldI18n(SERVICEORDER_ENTITY_SLUG)

  function ph(field: ServiceOrderField): string {
    return ef.placeholder(field, SERVICEORDER_PLACEHOLDER[field])
  }

  function queryPh(field: ServiceOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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
