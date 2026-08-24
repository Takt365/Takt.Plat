// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/foundation/setting/composables
// 文件名称：use-setting-i18n.ts
// 功能描述：系统设置实体 存储系统的各种配置参数字段清单 + useSettingI18n（字段名映射一次，文案由 entity.setting.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SettingQuery } from '@/types/foundation/setting'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktSettingI18nSeedData 一致的实体 slug */
export const SETTING_ENTITY_SLUG = 'setting'

/** entity.setting._self 静态属性（导入组件 entity-i18n-key 等） */
export const SETTING_SELF_I18N_KEY = buildEntitySelfI18nKey(SETTING_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SETTING_LIST_FIELDS = [
  'settingKey',
  'settingValue',
  'settingName',
  'settingDescription',
  'settingGroup',
  'valueType',
  'isBuiltIn',
  'isReadonly',
  'isEncrypted',
  'settingStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SETTING_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  settingKey: 'required',
  settingValue: 'optional',
  settingName: 'required',
  settingDescription: 'optional',
  settingGroup: 'select',
  valueType: 'select',
  isBuiltIn: 'select',
  isReadonly: 'select',
  isEncrypted: 'select',
  settingStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SettingField = keyof typeof SETTING_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SETTING_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'settingKey',
  'settingValue',
  'settingName',
  'settingDescription',
  'settingGroup',
  'valueType',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SettingQuery)[]

export type SettingQueryField =
  | (typeof SETTING_QUERY_STRING_FIELDS)[number]
  | 'isBuiltIn' | 'isReadonly' | 'isEncrypted' | 'settingStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SETTING_QUERY_FIELDS: readonly SettingQueryField[] = [
  ...SETTING_QUERY_STRING_FIELDS,
  'isBuiltIn',
  'isReadonly',
  'isEncrypted',
  'settingStatus',
]

/**
 * 系统设置实体 存储系统的各种配置参数字段 i18n：index / setting-form 统一入口
 */
export function useSettingI18n() {
  const ef = useEntityFieldI18n(SETTING_ENTITY_SLUG)

  function ph(field: SettingField): string {
    return ef.placeholder(field, SETTING_PLACEHOLDER[field])
  }

  function queryPh(field: SettingQueryField, kind: EntityFieldPlaceholderKind): string {
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
