// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/conference-center
// 文件名称：conference-agenda.d.ts
// 创建时间：2026-06-21
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/conference-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 会议议程/纪要实体 RecordType=议程项时多行维护议题；RecordType=会议纪要时通常一条记录承载正文与摘要
 * 对应前端 TaktConferenceAgendaDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConferenceAgenda
 * @description 对应后端 TaktConferenceAgendaDto
 */
export interface ConferenceAgenda extends CompanyDtoBase {
  /**
   * ConferenceAgendaID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  conferenceAgendaId: string;

  /**
   * 会议 ID（主子表关系）
   */
  conferenceId: string;

  /**
   * 会议 名称（填充字段）
   */
  conferenceName?: string;

  /**
   * 记录类型（议程项 / 会议纪要）
   */
  recordType: number;

  /**
   * 行号（议程项序号，固定步长=10；纪要通常为 10）
   */
  lineNumber: number;

  /**
   * 标题（议程议题或纪要标题）
   */
  title: string;

  /**
   * 正文（议程说明或会议纪要富文本 HTML）
   */
  content?: string;

  /**
   * 摘要（纪要列表展示用）
   */
  summary?: string;

  /**
   * 主讲人/汇报人 ID（议程项）
   */
  presenterId?: string;

  /**
   * 主讲人姓名（议程项）
   */
  presenterName?: string;

  /**
   * 计划开始时间（议程项）
   */
  plannedStartTime?: string;

  /**
   * 计划时长（分钟，议程项）
   */
  durationMinutes: number;

  /**
   * 记录人 ID（会议纪要）
   */
  recorderId?: string;

  /**
   * 记录人姓名（会议纪要）
   */
  recorderName?: string;

  /**
   * 会议（主表） （主表：TaktConference）
   */
  conference?: Conference;

}


/**
 * ConferenceAgenda 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConferenceAgendaQuery
 * @description 对应后端 TaktConferenceAgendaQueryDto
 */
export interface ConferenceAgendaQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 会议 ID（主子表关系）
   */
  conferenceId?: string;

  /**
   * 记录类型（议程项 / 会议纪要）
   */
  recordType?: number;

  /**
   * 行号（议程项序号，固定步长=10；纪要通常为 10）
   */
  lineNumber?: number;

  /**
   * 标题（议程议题或纪要标题）
   */
  title?: string;

  /**
   * 正文（议程说明或会议纪要富文本 HTML）
   */
  content?: string;

  /**
   * 摘要（纪要列表展示用）
   */
  summary?: string;

  /**
   * 主讲人/汇报人 ID（议程项）
   */
  presenterId?: string;

  /**
   * 主讲人姓名（议程项）
   */
  presenterName?: string;

  /**
   * 计划开始时间（议程项）（范围查询-开始）
   */
  plannedStartTimeStart?: string;

  /**
   * 计划开始时间（议程项）（范围查询-结束）
   */
  plannedStartTimeEnd?: string;

  /**
   * 计划时长（分钟，议程项）
   */
  durationMinutes?: number;

  /**
   * 记录人 ID（会议纪要）
   */
  recorderId?: string;

  /**
   * 记录人姓名（会议纪要）
   */
  recorderName?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建ConferenceAgenda DTO
 * 对应前端 ConferenceAgendaCreate
 * @description 对应后端 TaktConferenceAgendaCreateDto
 */
export interface ConferenceAgendaCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 会议 ID（主子表关系）
   */
  conferenceId: string;

  /**
   * 记录类型（议程项 / 会议纪要）
   */
  recordType: number;

  /**
   * 行号（议程项序号，固定步长=10；纪要通常为 10）
   */
  lineNumber: number;

  /**
   * 标题（议程议题或纪要标题）
   */
  title: string;

  /**
   * 正文（议程说明或会议纪要富文本 HTML）
   */
  content?: string;

  /**
   * 摘要（纪要列表展示用）
   */
  summary?: string;

  /**
   * 主讲人/汇报人 ID（议程项）
   */
  presenterId?: string;

  /**
   * 主讲人姓名（议程项）
   */
  presenterName?: string;

  /**
   * 计划开始时间（议程项）
   */
  plannedStartTime?: string;

  /**
   * 计划时长（分钟，议程项）
   */
  durationMinutes: number;

  /**
   * 记录人 ID（会议纪要）
   */
  recorderId?: string;

  /**
   * 记录人姓名（会议纪要）
   */
  recorderName?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新ConferenceAgenda DTO
 * 继承 TaktConferenceAgendaCreateDto，添加 ConferenceAgendaId 字段
 * 对应前端 ConferenceAgendaUpdate
 * @description 对应后端 TaktConferenceAgendaUpdateDto
 */
export interface ConferenceAgendaUpdate extends ConferenceAgendaCreate {
  /**
   * ConferenceAgendaID（标识要更新的实体）
   */
  conferenceAgendaId: string;

}


/**
 * ConferenceAgenda 导入模板行 DTO
 * 对应前端 ConferenceAgendaTemplate
 * @description 对应后端 TaktConferenceAgendaTemplateDto
 */
export interface ConferenceAgendaTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 会议 ID（主子表关系）
   */
  conferenceId?: string;

  /**
   * 记录类型（议程项 / 会议纪要）
   */
  recordType?: number;

  /**
   * 行号（议程项序号，固定步长=10；纪要通常为 10）
   */
  lineNumber?: number;

  /**
   * 标题（议程议题或纪要标题）
   */
  title?: string;

  /**
   * 正文（议程说明或会议纪要富文本 HTML）
   */
  content?: string;

  /**
   * 摘要（纪要列表展示用）
   */
  summary?: string;

  /**
   * 主讲人/汇报人 ID（议程项）
   */
  presenterId?: string;

  /**
   * 主讲人姓名（议程项）
   */
  presenterName?: string;

  /**
   * 计划时长（分钟，议程项）
   */
  durationMinutes?: number;

  /**
   * 记录人 ID（会议纪要）
   */
  recorderId?: string;

  /**
   * 记录人姓名（会议纪要）
   */
  recorderName?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * ConferenceAgenda 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConferenceAgendaImport
 * @description 对应后端 TaktConferenceAgendaImportDto
 */
export interface ConferenceAgendaImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 会议 ID（主子表关系）
   */
  conferenceId?: string;

  /**
   * 记录类型（议程项 / 会议纪要）
   */
  recordType?: number;

  /**
   * 行号（议程项序号，固定步长=10；纪要通常为 10）
   */
  lineNumber?: number;

  /**
   * 标题（议程议题或纪要标题）
   */
  title?: string;

  /**
   * 正文（议程说明或会议纪要富文本 HTML）
   */
  content?: string;

  /**
   * 摘要（纪要列表展示用）
   */
  summary?: string;

  /**
   * 主讲人/汇报人 ID（议程项）
   */
  presenterId?: string;

  /**
   * 主讲人姓名（议程项）
   */
  presenterName?: string;

  /**
   * 计划时长（分钟，议程项）
   */
  durationMinutes?: number;

  /**
   * 记录人 ID（会议纪要）
   */
  recorderId?: string;

  /**
   * 记录人姓名（会议纪要）
   */
  recorderName?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * ConferenceAgenda 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConferenceAgendaExport
 * @description 对应后端 TaktConferenceAgendaExportDto
 */
export interface ConferenceAgendaExport {
  /**
   * ConferenceAgendaID
   */
  conferenceAgendaId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 会议 ID（主子表关系）
   */
  conferenceId: string;

  /**
   * 记录类型（议程项 / 会议纪要）
   */
  recordType: number;

  /**
   * 行号（议程项序号，固定步长=10；纪要通常为 10）
   */
  lineNumber: number;

  /**
   * 标题（议程议题或纪要标题）
   */
  title: string;

  /**
   * 正文（议程说明或会议纪要富文本 HTML）
   */
  content?: string;

  /**
   * 摘要（纪要列表展示用）
   */
  summary?: string;

  /**
   * 主讲人/汇报人 ID（议程项）
   */
  presenterId?: string;

  /**
   * 主讲人姓名（议程项）
   */
  presenterName?: string;

  /**
   * 计划开始时间（议程项）
   */
  plannedStartTime?: string;

  /**
   * 计划时长（分钟，议程项）
   */
  durationMinutes: number;

  /**
   * 记录人 ID（会议纪要）
   */
  recorderId?: string;

  /**
   * 记录人姓名（会议纪要）
   */
  recorderName?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

