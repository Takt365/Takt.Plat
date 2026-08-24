// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/quality/complaint/customer-complaint/composables
// 文件名称：use-customer-complaint-i18n.ts
// 功能描述：客诉主表实体字段清单 + useCustomerComplaintI18n（字段名映射一次，文案由 entity.customercomplaint.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerComplaintQuery } from '@/types/logistics/quality/complaint/customer-complaint'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktCustomerComplaintI18nSeedData 一致的实体 slug */
export const CUSTOMERCOMPLAINT_ENTITY_SLUG = 'customercomplaint'

/** entity.customercomplaint._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERCOMPLAINT_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERCOMPLAINT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERCOMPLAINT_LIST_FIELDS = [
  'complaintStatus',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERCOMPLAINT_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerComplaintField = keyof typeof CUSTOMERCOMPLAINT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERCOMPLAINT_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof CustomerComplaintQuery)[]

export type CustomerComplaintQueryField = (typeof CUSTOMERCOMPLAINT_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERCOMPLAINT_QUERY_FIELDS: readonly CustomerComplaintQueryField[] = [...CUSTOMERCOMPLAINT_QUERY_STRING_FIELDS]

/**
 * 客诉主表实体字段 i18n：index / customer-complaint-form 统一入口
 */
export function useCustomerComplaintI18n() {
  const ef = useEntityFieldI18n(CUSTOMERCOMPLAINT_ENTITY_SLUG)

  function ph(field: CustomerComplaintField): string {
    return ef.placeholder(field, CUSTOMERCOMPLAINT_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerComplaintQueryField, kind: EntityFieldPlaceholderKind): string {
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
