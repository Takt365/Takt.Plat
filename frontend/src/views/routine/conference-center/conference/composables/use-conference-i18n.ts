// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/routine/conference-center/conference/composables
// 文件名称：use-conference-i18n.ts
// 功能描述：会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理字段清单 + useConferenceI18n（字段名映射一次，文案由 entity.conference.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ConferenceQuery } from '@/types/routine/conference-center/conference'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktConferenceI18nSeedData 一致的实体 slug */
export const CONFERENCE_ENTITY_SLUG = 'conference'

/** entity.conference._self 静态属性（导入组件 entity-i18n-key 等） */
export const CONFERENCE_SELF_I18N_KEY = buildEntitySelfI18nKey(CONFERENCE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CONFERENCE_LIST_FIELDS = [
  'conferenceTitle',
  'conferenceType',
  'conferenceStatus',
  'startTime',
  'endTime',
  'location',
  'meetingLink',
  'agenda',
  'conferenceContent',
  'conferenceSummary',
  'conferenceTags',
  'organizerId',
  'organizerName',
  'deptId',
  'deptName',
  'maxParticipants',
  'reminderMinutes',
  'conferenceRoomId',
  'conferenceRoomName',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CONFERENCE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  conferenceCode: 'required',
  conferenceTitle: 'required',
  conferenceType: 'select',
  startTime: 'required',
  endTime: 'required',
  location: 'optional',
  meetingLink: 'optional',
  agenda: 'optional',
  conferenceContent: 'optional',
  conferenceSummary: 'optional',
  conferenceTags: 'optional',
  organizerId: 'select',
  organizerName: 'optional',
  deptId: 'optional',
  deptName: 'optional',
  maxParticipants: 'select',
  reminderMinutes: 'select',
  conferenceRoomId: 'select',
  conferenceRoomName: 'optional',
  conferenceStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ConferenceField = keyof typeof CONFERENCE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CONFERENCE_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof ConferenceQuery)[]

export type ConferenceQueryField = (typeof CONFERENCE_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const CONFERENCE_QUERY_FIELDS: readonly ConferenceQueryField[] = [...CONFERENCE_QUERY_STRING_FIELDS]

/**
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理字段 i18n：index / conference-form 统一入口
 */
export function useConferenceI18n() {
  const ef = useEntityFieldI18n(CONFERENCE_ENTITY_SLUG)

  function ph(field: ConferenceField): string {
    return ef.placeholder(field, CONFERENCE_PLACEHOLDER[field])
  }

  function queryPh(field: ConferenceQueryField, kind: EntityFieldPlaceholderKind): string {
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
