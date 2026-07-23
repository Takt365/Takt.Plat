// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/service/service-request/composables
// 文件名称：use-service-request-i18n.ts
// 功能描述：服务请求实体字段清单 + useServiceRequestI18n（字段名映射一次，文案由 entity.servicerequest.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ServiceRequestQuery } from '@/types/logistics/customer-service/service-request'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktServiceRequestI18nSeedData 一致的实体 slug */
export const SERVICEREQUEST_ENTITY_SLUG = 'servicerequest'

/** entity.servicerequest._self 静态属性（导入组件 entity-i18n-key 等） */
export const SERVICEREQUEST_SELF_I18N_KEY = buildEntitySelfI18nKey(SERVICEREQUEST_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SERVICEREQUEST_LIST_FIELDS = [
  'plantCode',
  'serviceRequestCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceContractId',
  'serviceContractCode',
  'requestDate',
  'expectedServiceDate',
  'requestType',
  'sourceChannel',
  'priority',
  'requestStatus',
  'requestSubject',
  'requestDescription',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'serviceAddress',
  'assignedEmployeeId',
  'assignedEmployeeName',
  'assignedAt',
  'closedAt',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SERVICEREQUEST_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'required',
  serviceRequestCode: 'required',
  clientId: 'required',
  clientCode: 'required',
  clientName1: 'required',
  serviceContractId: 'optional',
  serviceContractCode: 'optional',
  requestDate: 'select',
  expectedServiceDate: 'optional',
  requestType: 'select',
  sourceChannel: 'select',
  priority: 'select',
  requestStatus: 'select',
  requestSubject: 'required',
  requestDescription: 'optional',
  contactPerson: 'optional',
  contactPhone: 'optional',
  contactEmail: 'optional',
  serviceAddress: 'optional',
  assignedEmployeeId: 'optional',
  assignedEmployeeName: 'optional',
  assignedAt: 'optional',
  closedAt: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ServiceRequestField = keyof typeof SERVICEREQUEST_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SERVICEREQUEST_QUERY_STRING_FIELDS = [
  'plantCode',
  'serviceRequestCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceContractId',
  'serviceContractCode',
  'requestDateStart',
  'requestDateEnd',
  'expectedServiceDateStart',
  'expectedServiceDateEnd',
  'requestSubject',
  'requestDescription',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'serviceAddress',
  'assignedEmployeeId',
  'assignedEmployeeName',
  'assignedAtStart',
  'assignedAtEnd',
  'closedAtStart',
  'closedAtEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ServiceRequestQuery)[]

export type ServiceRequestQueryField =
  | (typeof SERVICEREQUEST_QUERY_STRING_FIELDS)[number]
  | 'requestType' | 'sourceChannel' | 'priority' | 'requestStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SERVICEREQUEST_QUERY_FIELDS: readonly ServiceRequestQueryField[] = [
  ...SERVICEREQUEST_QUERY_STRING_FIELDS,
  'requestType',
  'sourceChannel',
  'priority',
  'requestStatus',
]

/**
 * 服务请求实体字段 i18n：index / service-request-form 统一入口
 */
export function useServiceRequestI18n() {
  const ef = useEntityFieldI18n(SERVICEREQUEST_ENTITY_SLUG)

  function ph(field: ServiceRequestField): string {
    return ef.placeholder(field, SERVICEREQUEST_PLACEHOLDER[field])
  }

  function queryPh(field: ServiceRequestQueryField, kind: EntityFieldPlaceholderKind): string {
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
