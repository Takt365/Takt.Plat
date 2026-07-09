// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/materials/material-document/composables
// 文件名称：use-material-document-item-i18n.ts
// 功能描述：MaterialDocumentItem字段清单 + useMaterialDocumentItemI18n（字段名映射一次，文案由 entity.materialdocumentitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaterialDocumentItemQuery } from '@/types/logistics/materials/material-document-item'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktMaterialDocumentItemI18nSeedData 一致的实体 slug */
export const MATERIALDOCUMENTITEM_ENTITY_SLUG = 'materialdocumentitem'

/** entity.materialdocumentitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const MATERIALDOCUMENTITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(MATERIALDOCUMENTITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MATERIALDOCUMENTITEM_LIST_FIELDS = [
  'materialDocumentCode',
  'lineNumber',
  'warehouseCode',
  'movementType',
  'postingDate',
  'quantity',
  'specialStock',
  'purchaseOrderCode',
  'productionOrderCode',
  'projectCode',
  'localCurrencyAmount',
  'documentDate',
  'referenceDocumentCode',
  'customerCode',
  'materialTransaction',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MATERIALDOCUMENTITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  lineNumber: 'select',
  warehouseCode: 'select',
  movementType: 'select',
  postingDate: 'select',
  quantity: 'select',
  specialStock: 'optional',
  purchaseOrderCode: 'optional',
  productionOrderCode: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaterialDocumentItemField = keyof typeof MATERIALDOCUMENTITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS = [
  'materialDocumentCode',
  'warehouseCode',
  'movementType',
  'postingDateStart',
  'postingDateEnd',
  'specialStock',
  'purchaseOrderCode',
  'productionOrderCode',
  'projectCode',
  'documentDateStart',
  'documentDateEnd',
  'referenceDocumentCode',
  'customerCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof MaterialDocumentItemQuery)[]

export type MaterialDocumentItemQueryField =
  | (typeof MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'quantity' | 'localCurrencyAmount'

/** 高级查询抽屉全部字段（含数值） */
export const MATERIALDOCUMENTITEM_QUERY_FIELDS: readonly MaterialDocumentItemQueryField[] = [
  ...MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'quantity',
  'localCurrencyAmount',
]

/**
 * MaterialDocumentItem字段 i18n：index / material-document-item-form 统一入口
 */
export function useMaterialDocumentItemI18n() {
  const ef = useEntityFieldI18n(MATERIALDOCUMENTITEM_ENTITY_SLUG)

  function ph(field: MaterialDocumentItemField): string {
    return ef.placeholder(field, MATERIALDOCUMENTITEM_PLACEHOLDER[field])
  }

  function queryPh(field: MaterialDocumentItemQueryField, kind: EntityFieldPlaceholderKind): string {
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
