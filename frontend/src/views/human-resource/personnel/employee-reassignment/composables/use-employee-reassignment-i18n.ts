// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/human-resource/personnel/employee-reassignment/composables
// 文件名称：use-employee-reassignment-i18n.ts
// 功能描述：员工调动记录字段清单 + useEmployeeReassignmentI18n（字段名映射一次，文案由 entity.employeereassignment.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeReassignmentQuery } from '@/types/human-resource/personnel/employee-reassignment'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktEmployeeReassignmentI18nSeedData 一致的实体 slug */
export const EMPLOYEEREASSIGNMENT_ENTITY_SLUG = 'employeereassignment'

/** entity.employeereassignment._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEEREASSIGNMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEEREASSIGNMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEEREASSIGNMENT_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'reassignmentType',
  'fromDeptId',
  'fromDeptName',
  'fromPostId',
  'fromPostName',
  'toDeptId',
  'toDeptName',
  'toPostId',
  'toPostName',
  'effectiveDate',
  'reason',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEEREASSIGNMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  employeeId: 'select',
  employeeCode: 'required',
  employeeName: 'required',
  reassignmentType: 'select',
  fromDeptId: 'select',
  fromDeptName: 'required',
  fromPostId: 'optional',
  fromPostName: 'optional',
  toDeptId: 'select',
  toDeptName: 'required',
  toPostId: 'optional',
  toPostName: 'optional',
  effectiveDate: 'optional',
  reason: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeReassignmentField = keyof typeof EMPLOYEEREASSIGNMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEEREASSIGNMENT_QUERY_STRING_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'fromDeptId',
  'fromDeptName',
  'fromPostId',
  'fromPostName',
  'toDeptId',
  'toDeptName',
  'toPostId',
  'toPostName',
  'effectiveDateStart',
  'effectiveDateEnd',
  'reason',
  'initiatorId',
  'initiatedAtStart',
  'initiatedAtEnd',
  'approvedBy',
  'approvedAtStart',
  'approvedAtEnd',
  'flowInstanceId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeReassignmentQuery)[]

export type EmployeeReassignmentQueryField =
  | (typeof EMPLOYEEREASSIGNMENT_QUERY_STRING_FIELDS)[number]
  | 'reassignmentType' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEEREASSIGNMENT_QUERY_FIELDS: readonly EmployeeReassignmentQueryField[] = [
  ...EMPLOYEEREASSIGNMENT_QUERY_STRING_FIELDS,
  'reassignmentType',
  'approvalStatus',
]

/**
 * 员工调动记录字段 i18n：index / employee-reassignment-form 统一入口
 */
export function useEmployeeReassignmentI18n() {
  const ef = useEntityFieldI18n(EMPLOYEEREASSIGNMENT_ENTITY_SLUG)

  function ph(field: EmployeeReassignmentField): string {
    return ef.placeholder(field, EMPLOYEEREASSIGNMENT_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeReassignmentQueryField, kind: EntityFieldPlaceholderKind): string {
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
