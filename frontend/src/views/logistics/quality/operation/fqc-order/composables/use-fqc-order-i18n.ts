// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/quality/operation/fqc-order/composables
// 文件名称：use-fqc-order-i18n.ts
// 功能描述：FQC出货检验单实体字段清单 + useFqcOrderI18n（字段名映射一次，文案由 entity.fqcorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FqcOrderQuery } from '@/types/logistics/quality/operation/fqc-order'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktFqcOrderI18nSeedData 一致的实体 slug */
export const FQCORDER_ENTITY_SLUG = 'fqcorder'

/** entity.fqcorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const FQCORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(FQCORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const FQCORDER_LIST_FIELDS = [
  'plantCode',
  'sourceCode',
  'inspectionDate',
  'fqcOrderCode',
  'customerCode',
  'totalWarehouseQuantity',
  'totalSampleQuantity',
  'totalQualifiedQuantity',
  'totalUnqualifiedQuantity',
  'totalInspectionReturnQuantity',
  'judgeBy',
  'judgeDate',
  'judgeDescription',
  'judgeStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const FQCORDER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  sourceCode: 'select',
  inspectionDate: 'optional',
  fqcOrderCode: 'required',
  customerCode: 'optional',
  totalWarehouseQuantity: 'select',
  totalSampleQuantity: 'select',
  totalQualifiedQuantity: 'select',
  totalUnqualifiedQuantity: 'select',
  totalInspectionReturnQuantity: 'select',
  judgeBy: 'optional',
  judgeDate: 'optional',
  judgeDescription: 'optional',
  judgeStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type FqcOrderField = keyof typeof FQCORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const FQCORDER_QUERY_STRING_FIELDS = [
  'plantCode',
  'sourceCode',
  'inspectionDateStart',
  'inspectionDateEnd',
  'fqcOrderCode',
  'customerCode',
  'judgeBy',
  'judgeDateStart',
  'judgeDateEnd',
  'judgeDescription',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof FqcOrderQuery)[]

export type FqcOrderQueryField =
  | (typeof FQCORDER_QUERY_STRING_FIELDS)[number]
  | 'totalWarehouseQuantity' | 'totalSampleQuantity' | 'totalQualifiedQuantity' | 'totalUnqualifiedQuantity' | 'totalInspectionReturnQuantity' | 'judgeStatus'

/** 高级查询抽屉全部字段（含数值） */
export const FQCORDER_QUERY_FIELDS: readonly FqcOrderQueryField[] = [
  ...FQCORDER_QUERY_STRING_FIELDS,
  'totalWarehouseQuantity',
  'totalSampleQuantity',
  'totalQualifiedQuantity',
  'totalUnqualifiedQuantity',
  'totalInspectionReturnQuantity',
  'judgeStatus',
]

/**
 * FQC出货检验单实体字段 i18n：index / fqc-order-form 统一入口
 */
export function useFqcOrderI18n() {
  const ef = useEntityFieldI18n(FQCORDER_ENTITY_SLUG)

  function ph(field: FqcOrderField): string {
    return ef.placeholder(field, FQCORDER_PLACEHOLDER[field])
  }

  function queryPh(field: FqcOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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
