// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/sales/client/composables
// 文件名称：use-client-i18n.ts
// 功能描述：Takt客户端信息实体字段清单 + useClientI18n（字段名映射一次，文案由 entity.client.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ClientQuery } from '@/types/logistics/sales/client'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktClientI18nSeedData 一致的实体 slug */
export const CLIENT_ENTITY_SLUG = 'client'

/** entity.client._self 静态属性（导入组件 entity-i18n-key 等） */
export const CLIENT_SELF_I18N_KEY = buildEntitySelfI18nKey(CLIENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CLIENT_LIST_FIELDS = [
  'plantCode',
  'clientCode',
  'clientName',
  'clientShortName',
  'clientType',
  'industrySector',
  'clientTaxNumber',
  'taxRate',
  'registrationCountry',
  'registrationAddress1',
  'registrationAddress2',
  'registrationAddress3',
  'clientPhone',
  'clientFax',
  'clientEmail',
  'clientWebsite',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'currencyCode',
  'paymentTerms',
  'salesChannel',
  'platformName',
  'storeName',
  'clientLevel',
  'evaluationScore',
  'clientStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CLIENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  clientCode: 'required',
  clientName: 'required',
  clientShortName: 'optional',
  clientType: 'select',
  industrySector: 'optional',
  clientTaxNumber: 'optional',
  taxRate: 'select',
  registrationCountry: 'optional',
  registrationAddress1: 'optional',
  registrationAddress2: 'optional',
  registrationAddress3: 'optional',
  clientPhone: 'optional',
  clientFax: 'optional',
  clientEmail: 'optional',
  clientWebsite: 'optional',
  contactPerson: 'optional',
  contactPhone: 'optional',
  contactEmail: 'optional',
  currencyCode: 'required',
  paymentTerms: 'select',
  salesChannel: 'select',
  platformName: 'optional',
  storeName: 'optional',
  clientLevel: 'select',
  evaluationScore: 'select',
  clientStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ClientField = keyof typeof CLIENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CLIENT_QUERY_STRING_FIELDS = [
  'plantCode',
  'clientCode',
  'clientName',
  'clientShortName',
  'industrySector',
  'clientTaxNumber',
  'registrationCountry',
  'registrationAddress1',
  'registrationAddress2',
  'registrationAddress3',
  'clientPhone',
  'clientFax',
  'clientEmail',
  'clientWebsite',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'currencyCode',
  'paymentTerms',
  'platformName',
  'storeName',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ClientQuery)[]

export type ClientQueryField =
  | (typeof CLIENT_QUERY_STRING_FIELDS)[number]
  | 'clientType' | 'taxRate' | 'salesChannel' | 'clientLevel' | 'evaluationScore' | 'clientStatus'

/** 高级查询抽屉全部字段（含数值） */
export const CLIENT_QUERY_FIELDS: readonly ClientQueryField[] = [
  ...CLIENT_QUERY_STRING_FIELDS,
  'clientType',
  'taxRate',
  'salesChannel',
  'clientLevel',
  'evaluationScore',
  'clientStatus',
]

/**
 * Takt客户端信息实体字段 i18n：index / client-form 统一入口
 */
export function useClientI18n() {
  const ef = useEntityFieldI18n(CLIENT_ENTITY_SLUG)

  function ph(field: ClientField): string {
    return ef.placeholder(field, CLIENT_PLACEHOLDER[field])
  }

  function queryPh(field: ClientQueryField, kind: EntityFieldPlaceholderKind): string {
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
