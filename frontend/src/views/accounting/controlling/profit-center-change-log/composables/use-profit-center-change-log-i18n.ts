// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/accounting/controlling/profit-center-change-log/composables
// 文件名称：use-profit-center-change-log-i18n.ts
// 功能描述：ProfitCenterChangeLog字段清单 + useProfitCenterChangeLogI18n（字段名映射一次，文案由 entity.profitcenterchangelog.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ProfitCenterChangeLogQuery } from '@/types/accounting/controlling/profit-center-change-log'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktProfitCenterChangeLogI18nSeedData 一致的实体 slug */
export const PROFITCENTERCHANGELOG_ENTITY_SLUG = 'profitcenterchangelog'

/** entity.profitcenterchangelog._self 静态属性（导入组件 entity-i18n-key 等） */
export const PROFITCENTERCHANGELOG_SELF_I18N_KEY = buildEntitySelfI18nKey(PROFITCENTERCHANGELOG_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PROFITCENTERCHANGELOG_LIST_FIELDS = [
  'profitCenterCode',
  'changeFields',
  'changeTime',
  'changeBy',
  'changeReason',
  'profitCenter',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PROFITCENTERCHANGELOG_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  changeFields: 'optional',
  changeTime: 'select',
  changeBy: 'optional',
  changeReason: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ProfitCenterChangeLogField = keyof typeof PROFITCENTERCHANGELOG_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PROFITCENTERCHANGELOG_QUERY_STRING_FIELDS = [
  'profitCenterCode',
  'changeFields',
  'changeTimeStart',
  'changeTimeEnd',
  'changeBy',
  'changeReason',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ProfitCenterChangeLogQuery)[]

export type ProfitCenterChangeLogQueryField = (typeof PROFITCENTERCHANGELOG_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const PROFITCENTERCHANGELOG_QUERY_FIELDS: readonly ProfitCenterChangeLogQueryField[] = [...PROFITCENTERCHANGELOG_QUERY_STRING_FIELDS]

/**
 * ProfitCenterChangeLog字段 i18n：index / profit-center-change-log-form 统一入口
 */
export function useProfitCenterChangeLogI18n() {
  const ef = useEntityFieldI18n(PROFITCENTERCHANGELOG_ENTITY_SLUG)

  function ph(field: ProfitCenterChangeLogField): string {
    return ef.placeholder(field, PROFITCENTERCHANGELOG_PLACEHOLDER[field])
  }

  function queryPh(field: ProfitCenterChangeLogQueryField, kind: EntityFieldPlaceholderKind): string {
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
