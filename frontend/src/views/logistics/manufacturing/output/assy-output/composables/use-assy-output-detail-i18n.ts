// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：use-assy-output-detail-i18n.ts
// 功能描述：AssyOutputDetail字段清单 + useAssyOutputDetailI18n（字段名映射一次，文案由 entity.assyoutputdetail.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { useI18n } from 'vue-i18n'
import type { AssyOutputDetailQuery } from '@/types/logistics/manufacturing/output/assy-output-detail'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktAssyOutputDetailI18nSeedData 一致的实体 slug */
export const ASSYOUTPUTDETAIL_ENTITY_SLUG = 'assyoutputdetail'

/** 报工工时字段说明（静态 locales，避免 entity 种子被脚本覆盖） */
export const ASSYOUTPUTDETAIL_CONFIRM_MINUTES_HINT_I18N_KEY = 'logistics.manufacturing.output.assy-output.page.confirmminuteshint'

/** 标准产能计算说明（静态 locales，避免 entity 种子被脚本覆盖） */
export const ASSYOUTPUTDETAIL_STD_CAPACITY_HINT_I18N_KEY = 'logistics.manufacturing.output.assy-output.page.detailstdcapacityhint'

/** entity.assyoutputdetail._self 静态属性（导入组件 entity-i18n-key 等） */
export const ASSYOUTPUTDETAIL_SELF_I18N_KEY = buildEntitySelfI18nKey(ASSYOUTPUTDETAIL_ENTITY_SLUG)

/** 明细抽屉默认展示列（含未达成说明，避免 masterDetailDetail 仅 id+4 列时看不到） */
export const ASSYOUTPUTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'timePeriod',
  'stdCapacity',
  'prodActualQty',
  'downtimeMinutes',
  'downtimeReason',
  'downtimeDescription',
  'unachievedReason',
  'unachievedDescription',
  'confirmMinutes',
  'action',
] as const

/** 明细列表/面板子表合计列（当前页 dataSource 求和） */
export const ASSYOUTPUTDETAIL_SUMMARY_SUM_FIELDS = [
  'prodActualQty',
  'downtimeMinutes',
  'inputMinutes',
  'actualMinutes',
  'indirectMinutes',
  'confirmMinutes',
  'stdCapacity',
] as const

/** 主表弹窗内嵌子表可编辑列（与 assy-output-form TaktEditableTable 对齐） */
export const ASSYOUTPUTDETAIL_EMBEDDED_TABLE_FIELDS = [
  'timePeriod',
  'stdCapacity',
  'prodActualQty',
  'downtimeMinutes',
  'downtimeReason',
  'downtimeDescription',
  'unachievedReason',
  'unachievedDescription',
  'confirmMinutes',
  'lineNumber',
] as const

/** 列表业务列（不含主键） */
export const ASSYOUTPUTDETAIL_LIST_FIELDS = [
  'prodOrderCode',
  'lineNumber',
  'timePeriod',
  'stdCapacity',
  'prodActualQty',
  'downtimeMinutes',
  'downtimeReason',
  'downtimeDescription',
  'unachievedReason',
  'unachievedDescription',
  'inputMinutes',
  'actualMinutes',
  'indirectMinutes',
  'confirmMinutes',
  'mixedProd',
  'achievementRate',
  'assyOutputId',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ASSYOUTPUTDETAIL_PLACEHOLDER = {
  tenantCode: 'required',
  companyCode: 'required',
  companyDefaultCulture: 'required',
  prodOrderCode: 'required',
  lineNumber: 'select',
  timePeriod: 'required',
  prodActualQty: 'select',
  downtimeMinutes: 'select',
  downtimeReason: 'select',
  downtimeDescription: 'optional',
  unachievedReason: 'select',
  unachievedDescription: 'optional',
  confirmMinutes: 'select',
  inputMinutes: 'select',
  actualMinutes: 'select',
  indirectMinutes: 'select',
  mixedProd: 'select',
  stdCapacity: 'select',
  achievementRate: 'select',
  plantCode: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AssyOutputDetailField = keyof typeof ASSYOUTPUTDETAIL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS = [
  'prodOrderCode',
  'timePeriod',
  'downtimeReason',
  'downtimeDescription',
  'unachievedReason',
  'unachievedDescription',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AssyOutputDetailQuery)[]

export type AssyOutputDetailQueryField =
  | (typeof ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'prodActualQty' | 'downtimeMinutes' | 'inputMinutes' | 'actualMinutes' | 'indirectMinutes' | 'confirmMinutes' | 'mixedProd' | 'stdCapacity' | 'achievementRate'

/** 高级查询抽屉全部字段（含数值） */
export const ASSYOUTPUTDETAIL_QUERY_FIELDS: readonly AssyOutputDetailQueryField[] = [
  ...ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS,
  'lineNumber',
  'stdCapacity',
  'prodActualQty',
  'downtimeMinutes',
  'inputMinutes',
  'actualMinutes',
  'indirectMinutes',
  'confirmMinutes',
  'mixedProd',
  'stdCapacity',
  'achievementRate',
]

/**
 * AssyOutputDetail字段 i18n：index / assy-output-detail-form 统一入口
 */
export function useAssyOutputDetailI18n() {
  const { t: localeT } = useI18n()
  const ef = useEntityFieldI18n(ASSYOUTPUTDETAIL_ENTITY_SLUG)

  function ph(field: AssyOutputDetailField): string {
    return ef.placeholder(field, ASSYOUTPUTDETAIL_PLACEHOLDER[field])
  }

  function queryPh(field: AssyOutputDetailQueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  /** 报工工时填写场景说明 */
  function confirmMinutesHint(): string {
    return localeT(ASSYOUTPUTDETAIL_CONFIRM_MINUTES_HINT_I18N_KEY)
  }

  /** 标准产能计算说明 */
  function stdCapacityHint(): string {
    return localeT(ASSYOUTPUTDETAIL_STD_CAPACITY_HINT_I18N_KEY)
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
    confirmMinutesHint,
    stdCapacityHint,
  }
}
