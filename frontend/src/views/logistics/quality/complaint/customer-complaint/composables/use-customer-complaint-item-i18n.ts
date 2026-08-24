// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/quality/complaint/customer-complaint/composables
// 文件名称：use-customer-complaint-item-i18n.ts
// 功能描述：CustomerComplaintItem字段清单 + useCustomerComplaintItemI18n（字段名映射一次，文案由 entity.customercomplaintitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerComplaintItemQuery } from '@/types/logistics/quality/complaint/customer-complaint-item'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktCustomerComplaintItemI18nSeedData 一致的实体 slug */
export const CUSTOMERCOMPLAINTITEM_ENTITY_SLUG = 'customercomplaintitem'

/** entity.customercomplaintitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERCOMPLAINTITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERCOMPLAINTITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERCOMPLAINTITEM_LIST_FIELDS = [
  'customerComplaintCode',
  'lineNumber',
  'productCode',
  'productName',
  'batchCode',
  'itemType',
  'defectDescription',
  'defectLevel',
  'defectQuantity',
  'defectRate',
  'causeAnalysis',
  'improvementAction',
  'improvementResponsible',
  'plannedCompletionDate',
  'actualCompletionDate',
  'attachmentPaths',
  'improvementStatus',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const CUSTOMERCOMPLAINTITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'customerComplaintCode',
  'lineNumber',
  'productCode',
  'productName',
  'batchCode',
  'itemType',
  'defectDescription',
  'defectLevel',
  'defectQuantity',
  'defectRate',
  'causeAnalysis',
  'improvementAction',
  'improvementResponsible',
  'plannedCompletionDate',
  'actualCompletionDate',
  'attachmentPaths',
  'improvementStatus',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const CUSTOMERCOMPLAINTITEM_SUMMARY_SUM_FIELDS = [
  'itemType',
  'defectQuantity',
  'defectRate',
  'improvementStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERCOMPLAINTITEM_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerComplaintItemField = keyof typeof CUSTOMERCOMPLAINTITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERCOMPLAINTITEM_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof CustomerComplaintItemQuery)[]

export type CustomerComplaintItemQueryField = (typeof CUSTOMERCOMPLAINTITEM_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERCOMPLAINTITEM_QUERY_FIELDS: readonly CustomerComplaintItemQueryField[] = [...CUSTOMERCOMPLAINTITEM_QUERY_STRING_FIELDS]

/**
 * CustomerComplaintItem字段 i18n：index / customer-complaint-item-form 统一入口
 */
export function useCustomerComplaintItemI18n() {
  const ef = useEntityFieldI18n(CUSTOMERCOMPLAINTITEM_ENTITY_SLUG)

  function ph(field: CustomerComplaintItemField): string {
    return ef.placeholder(field, CUSTOMERCOMPLAINTITEM_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerComplaintItemQueryField, kind: EntityFieldPlaceholderKind): string {
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
