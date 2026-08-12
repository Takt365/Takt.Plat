// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/manufacturing/defect/pcba-repair/composables
// 文件名称：use-pcba-repair-i18n.ts
// 功能描述：PCBA改修日报实体 不良率字段清单 + usePcbaRepairI18n（字段名映射一次，文案由 entity.pcbarepair.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PcbaRepairQuery } from '@/types/logistics/manufacturing/defect/pcba-repair'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktPcbaRepairI18nSeedData 一致的实体 slug */
export const PCBAREPAIR_ENTITY_SLUG = 'pcbarepair'

/** entity.pcbarepair._self 静态属性（导入组件 entity-i18n-key 等） */
export const PCBAREPAIR_SELF_I18N_KEY = buildEntitySelfI18nKey(PCBAREPAIR_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PCBAREPAIR_LIST_FIELDS = [
  'plantCode',
  'prodCategory',
  'prodDate',
  'TeamCode',
  'shiftNo',
  'prodOrderType',
  'prodOrderCode',
  'prodOrderQty',
  'modelCode',
  'batchCode',
  'materialCode',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PCBAREPAIR_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'optional',
  prodCategory: 'select',
  prodDate: 'select',
  TeamCode: 'select',
  shiftNo: 'select',
  prodOrderType: 'optional',
  prodOrderCode: 'select',
  prodOrderQty: 'select',
  modelCode: 'required',
  batchCode: 'optional',
  materialCode: 'required',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PcbaRepairField = keyof typeof PCBAREPAIR_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PCBAREPAIR_QUERY_STRING_FIELDS = [
  'plantCode',
  'prodCategory',
  'prodDateStart',
  'prodDateEnd',
  'TeamCode',
  'prodOrderType',
  'prodOrderCode',
  'modelCode',
  'batchCode',
  'materialCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PcbaRepairQuery)[]

export type PcbaRepairQueryField =
  | (typeof PCBAREPAIR_QUERY_STRING_FIELDS)[number]
  | 'shiftNo' | 'prodOrderQty'

/** 高级查询抽屉全部字段（含数值） */
export const PCBAREPAIR_QUERY_FIELDS: readonly PcbaRepairQueryField[] = [
  ...PCBAREPAIR_QUERY_STRING_FIELDS,
  'shiftNo',
  'prodOrderQty',
]

/**
 * PCBA改修日报实体 不良率字段 i18n：index / pcba-repair-form 统一入口
 */
export function usePcbaRepairI18n() {
  const ef = useEntityFieldI18n(PCBAREPAIR_ENTITY_SLUG)

  function ph(field: PcbaRepairField): string {
    return ef.placeholder(field, PCBAREPAIR_PLACEHOLDER[field])
  }

  function queryPh(field: PcbaRepairQueryField, kind: EntityFieldPlaceholderKind): string {
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
