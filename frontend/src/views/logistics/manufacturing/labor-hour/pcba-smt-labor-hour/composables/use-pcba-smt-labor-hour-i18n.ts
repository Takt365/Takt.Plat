// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/manufacturing/labor-hour/pcba-smt-labor-hour/composables
// 文件名称：use-pcba-smt-labor-hour-i18n.ts
// 功能描述：PCBA SMT工数统计实体字段清单 + usePcbaSmtLaborHourI18n（字段名映射一次，文案由 entity.pcbasmtlaborhour.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PcbaSmtLaborHourQuery } from '@/types/logistics/manufacturing/labor-hour/pcba-smt-labor-hour'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktPcbaSmtLaborHourI18nSeedData 一致的实体 slug */
export const PCBASMTLABORHOUR_ENTITY_SLUG = 'pcbasmtlaborhour'

/** entity.pcbasmtlaborhour._self 静态属性（导入组件 entity-i18n-key 等） */
export const PCBASMTLABORHOUR_SELF_I18N_KEY = buildEntitySelfI18nKey(PCBASMTLABORHOUR_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PCBASMTLABORHOUR_LIST_FIELDS = [
  'prodDate',
  'prodTeam',
  'shiftNo',
  'stdCapacity',
  'prodActualQty',
  'inputMinutes',
  'downtimeMinutes',
  'confirmMinutes',
  'actualMinutes',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PCBASMTLABORHOUR_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  prodDate: 'select',
  prodTeam: 'select',
  shiftNo: 'select',
  stdCapacity: 'select',
  prodActualQty: 'select',
  inputMinutes: 'select',
  downtimeMinutes: 'select',
  confirmMinutes: 'select',
  actualMinutes: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PcbaSmtLaborHourField = keyof typeof PCBASMTLABORHOUR_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PCBASMTLABORHOUR_QUERY_STRING_FIELDS = [
  'prodDateStart',
  'prodDateEnd',
  'prodTeam',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PcbaSmtLaborHourQuery)[]

export type PcbaSmtLaborHourQueryField =
  | (typeof PCBASMTLABORHOUR_QUERY_STRING_FIELDS)[number]
  | 'shiftNo' | 'stdCapacity' | 'prodActualQty' | 'inputMinutes' | 'downtimeMinutes' | 'confirmMinutes' | 'actualMinutes'

/** 高级查询抽屉全部字段（含数值） */
export const PCBASMTLABORHOUR_QUERY_FIELDS: readonly PcbaSmtLaborHourQueryField[] = [
  ...PCBASMTLABORHOUR_QUERY_STRING_FIELDS,
  'shiftNo',
  'stdCapacity',
  'prodActualQty',
  'inputMinutes',
  'downtimeMinutes',
  'confirmMinutes',
  'actualMinutes',
]

/**
 * PCBA SMT工数统计实体字段 i18n：index / pcba-smt-labor-hour-form 统一入口
 */
export function usePcbaSmtLaborHourI18n() {
  const ef = useEntityFieldI18n(PCBASMTLABORHOUR_ENTITY_SLUG)

  function ph(field: PcbaSmtLaborHourField): string {
    return ef.placeholder(field, PCBASMTLABORHOUR_PLACEHOLDER[field])
  }

  function queryPh(field: PcbaSmtLaborHourQueryField, kind: EntityFieldPlaceholderKind): string {
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
