// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/sales/customer/composables
// 文件名称：use-customer-i18n.ts
// 功能描述：Takt客户信息实体字段清单 + useCustomerI18n（字段名映射一次，文案由 entity.customer.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerQuery } from '@/types/logistics/sales/customer'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktCustomerI18nSeedData 一致的实体 slug */
export const CUSTOMER_ENTITY_SLUG = 'customer'

/** entity.customer._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMER_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMER_LIST_FIELDS = [
  'plantCode',
  'customerCode',
  'customerName',
  'customerShortName',
  'customerType',
  'industrySector',
  'customerTaxNumber',
  'taxRate',
  'registrationCountry',
  'registrationAddress1',
  'registrationAddress2',
  'registrationAddress3',
  'customerPhone',
  'customerFax',
  'customerEmail',
  'customerWebsite',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'currencyCode',
  'paymentTerms',
  'creditLevel',
  'creditAmount',
  'discountRate',
  'salesBy',
  'customerLevel',
  'evaluationScore',
  'customerStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  customerCode: 'required',
  customerName: 'required',
  customerShortName: 'optional',
  customerType: 'select',
  industrySector: 'optional',
  customerTaxNumber: 'optional',
  taxRate: 'select',
  registrationCountry: 'optional',
  registrationAddress1: 'optional',
  registrationAddress2: 'optional',
  registrationAddress3: 'optional',
  customerPhone: 'optional',
  customerFax: 'optional',
  customerEmail: 'optional',
  customerWebsite: 'optional',
  contactPerson: 'optional',
  contactPhone: 'optional',
  contactEmail: 'optional',
  currencyCode: 'required',
  paymentTerms: 'select',
  creditLevel: 'select',
  creditAmount: 'select',
  discountRate: 'select',
  salesBy: 'optional',
  customerLevel: 'select',
  evaluationScore: 'select',
  customerStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerField = keyof typeof CUSTOMER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMER_QUERY_STRING_FIELDS = [
  'plantCode',
  'customerCode',
  'customerName',
  'customerShortName',
  'industrySector',
  'customerTaxNumber',
  'registrationCountry',
  'registrationAddress1',
  'registrationAddress2',
  'registrationAddress3',
  'customerPhone',
  'customerFax',
  'customerEmail',
  'customerWebsite',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'currencyCode',
  'paymentTerms',
  'salesBy',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CustomerQuery)[]

export type CustomerQueryField =
  | (typeof CUSTOMER_QUERY_STRING_FIELDS)[number]
  | 'customerType' | 'taxRate' | 'creditLevel' | 'creditAmount' | 'discountRate' | 'customerLevel' | 'evaluationScore' | 'customerStatus'

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMER_QUERY_FIELDS: readonly CustomerQueryField[] = [
  ...CUSTOMER_QUERY_STRING_FIELDS,
  'customerType',
  'taxRate',
  'creditLevel',
  'creditAmount',
  'discountRate',
  'customerLevel',
  'evaluationScore',
  'customerStatus',
]

/**
 * Takt客户信息实体字段 i18n：index / customer-form 统一入口
 */
export function useCustomerI18n() {
  const ef = useEntityFieldI18n(CUSTOMER_ENTITY_SLUG)

  function ph(field: CustomerField): string {
    return ef.placeholder(field, CUSTOMER_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerQueryField, kind: EntityFieldPlaceholderKind): string {
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
