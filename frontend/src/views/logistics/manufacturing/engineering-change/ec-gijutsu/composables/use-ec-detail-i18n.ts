// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables
// 文件名称：use-ec-detail-i18n.ts
// 功能描述：EcDetail字段清单 + useEcDetailI18n（字段名映射一次，文案由 entity.ecdetail.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EcDetailQuery } from '@/types/logistics/manufacturing/engineering-change/ec-detail'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktEcDetailI18nSeedData 一致的实体 slug */
export const ECDETAIL_ENTITY_SLUG = 'ecdetail'

/** entity.ecdetail._self 静态属性（导入组件 entity-i18n-key 等） */
export const ECDETAIL_SELF_I18N_KEY = buildEntitySelfI18nKey(ECDETAIL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ECDETAIL_LIST_FIELDS = [
  'ecCode',
  'lineNumber',
  'ecBomLineCode',
  'ecModel',
  'ecBomItem',
  'ecBomItemText',
  'ecBomSubItem',
  'ecBomSubItemText',
  'isEndOfLine',
  'ecOldItem',
  'ecOldText',
  'ecOldUsage',
  'ecOldPosition',
  'ecOldStock',
  'ecOldWarehouse',
  'isOldProcurement',
  'isOldCheck',
  'ecNewItem',
  'ecNewText',
  'ecNewUsage',
  'ecNewPosition',
  'ecNewStock',
  'ecNewWarehouse',
  'isNewProcurement',
  'isNewCheck',
  'ecBomDate',
  'ecIsCompatible',
  'ecSecondDistinction',
  'ecInstruction',
  'ecLegacyPartDisposition',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const ECDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'ecCode',
  'lineNumber',
  'ecBomLineCode',
  'ecModel',
  'ecBomItem',
  'ecBomItemText',
  'ecBomSubItem',
  'ecBomSubItemText',
  'isEndOfLine',
  'ecOldItem',
  'ecOldText',
  'ecOldUsage',
  'ecOldPosition',
  'ecOldStock',
  'ecOldWarehouse',
  'isOldProcurement',
  'isOldCheck',
  'ecNewItem',
  'ecNewText',
  'ecNewUsage',
  'ecNewPosition',
  'ecNewStock',
  'ecNewWarehouse',
  'isNewProcurement',
  'isNewCheck',
  'ecBomDate',
  'ecIsCompatible',
  'ecSecondDistinction',
  'ecInstruction',
  'ecLegacyPartDisposition',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const ECDETAIL_SUMMARY_SUM_FIELDS = [
  'isEndOfLine',
  'ecOldUsage',
  'ecOldStock',
  'isOldProcurement',
  'isOldCheck',
  'ecNewUsage',
  'ecNewStock',
  'isNewProcurement',
  'isNewCheck',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ECDETAIL_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EcDetailField = keyof typeof ECDETAIL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ECDETAIL_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof EcDetailQuery)[]

export type EcDetailQueryField = (typeof ECDETAIL_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const ECDETAIL_QUERY_FIELDS: readonly EcDetailQueryField[] = [...ECDETAIL_QUERY_STRING_FIELDS]

/**
 * EcDetail字段 i18n：index / ec-detail-form 统一入口
 */
export function useEcDetailI18n() {
  const ef = useEntityFieldI18n(ECDETAIL_ENTITY_SLUG)

  function ph(field: EcDetailField): string {
    return ef.placeholder(field, ECDETAIL_PLACEHOLDER[field])
  }

  function queryPh(field: EcDetailQueryField, kind: EntityFieldPlaceholderKind): string {
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
