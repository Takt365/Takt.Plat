// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/conference-center
// 文件名称：conference-participant.d.ts
// 创建时间：2026-06-11
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
 * 会议参与人子实体 合并邀请、角色、出席确认与签到/签退于同一行
 * 对应前端 TaktConferenceParticipantDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConferenceParticipant
 * @description 对应后端 TaktConferenceParticipantDto
 */
export interface ConferenceParticipant extends CompanyDtoBase {
  /**
   * ConferenceParticipantID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  conferenceParticipantId: string;

  /**
   * 会议 ID（主子表关系）
   */
  conferenceId: string;

  /**
   * 会议 名称（填充字段）
   */
  conferenceName?: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 用户姓名
   */
  userName: string;

  /**
   * 参与角色
   */
  participantRole: number;

  /**
   * 出席状态（待确认/已接受/已拒绝/已签到/缺席）
   */
  attendanceStatus: number;

  /**
   * 签到时间
   */
  checkInTime?: string;

  /**
   * 签退时间
   */
  checkOutTime?: string;

  /**
   * 签到方式
   */
  checkInMethod: number;

  /**
   * 会议（主表） （主表：TaktConference）
   */
  conference?: Conference;

}


/**
 * ConferenceParticipant 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConferenceParticipantQuery
 * @description 对应后端 TaktConferenceParticipantQueryDto
 */
export interface ConferenceParticipantQuery extends TaktPagedQuery {
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
   * 用户 ID
   */
  userId?: string;

  /**
   * 用户姓名
   */
  userName?: string;

  /**
   * 参与角色
   */
  participantRole?: number;

  /**
   * 出席状态（待确认/已接受/已拒绝/已签到/缺席）
   */
  attendanceStatus?: number;

  /**
   * 签到时间（范围查询-开始）
   */
  checkInTimeStart?: string;

  /**
   * 签到时间（范围查询-结束）
   */
  checkInTimeEnd?: string;

  /**
   * 签退时间（范围查询-开始）
   */
  checkOutTimeStart?: string;

  /**
   * 签退时间（范围查询-结束）
   */
  checkOutTimeEnd?: string;

  /**
   * 签到方式
   */
  checkInMethod?: number;

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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建ConferenceParticipant DTO
 * 对应前端 ConferenceParticipantCreate
 * @description 对应后端 TaktConferenceParticipantCreateDto
 */
export interface ConferenceParticipantCreate {
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
   * 用户 ID
   */
  userId: string;

  /**
   * 用户姓名
   */
  userName: string;

  /**
   * 参与角色
   */
  participantRole: number;

  /**
   * 出席状态（待确认/已接受/已拒绝/已签到/缺席）
   */
  attendanceStatus: number;

  /**
   * 签到时间
   */
  checkInTime?: string;

  /**
   * 签退时间
   */
  checkOutTime?: string;

  /**
   * 签到方式
   */
  checkInMethod: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新ConferenceParticipant DTO
 * 继承 TaktConferenceParticipantCreateDto，添加 ConferenceParticipantId 字段
 * 对应前端 ConferenceParticipantUpdate
 * @description 对应后端 TaktConferenceParticipantUpdateDto
 */
export interface ConferenceParticipantUpdate extends ConferenceParticipantCreate {
  /**
   * ConferenceParticipantID（标识要更新的实体）
   */
  conferenceParticipantId: string;

}


/**
 * ConferenceParticipant 状态更新 DTO
 * 对应前端 ConferenceParticipantStatus
 * @description 对应后端 TaktConferenceParticipantStatusDto
 */
export interface ConferenceParticipantStatus {
  /**
   * ConferenceParticipantID
   */
  conferenceParticipantId: string;

  /**
   * 出席状态（待确认/已接受/已拒绝/已签到/缺席）
   */
  attendanceStatus: number;

}


/**
 * ConferenceParticipant 导入模板行 DTO
 * 对应前端 ConferenceParticipantTemplate
 * @description 对应后端 TaktConferenceParticipantTemplateDto
 */
export interface ConferenceParticipantTemplate {
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
   * 用户 ID
   */
  userId?: string;

  /**
   * 用户姓名
   */
  userName?: string;

  /**
   * 参与角色
   */
  participantRole?: number;

  /**
   * 出席状态（待确认/已接受/已拒绝/已签到/缺席）
   */
  attendanceStatus?: number;

  /**
   * 签到方式
   */
  checkInMethod?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * ConferenceParticipant 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConferenceParticipantImport
 * @description 对应后端 TaktConferenceParticipantImportDto
 */
export interface ConferenceParticipantImport {
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
   * 用户 ID
   */
  userId?: string;

  /**
   * 用户姓名
   */
  userName?: string;

  /**
   * 参与角色
   */
  participantRole?: number;

  /**
   * 出席状态（待确认/已接受/已拒绝/已签到/缺席）
   */
  attendanceStatus?: number;

  /**
   * 签到方式
   */
  checkInMethod?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * ConferenceParticipant 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConferenceParticipantExport
 * @description 对应后端 TaktConferenceParticipantExportDto
 */
export interface ConferenceParticipantExport {
  /**
   * ConferenceParticipantID
   */
  conferenceParticipantId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 会议 ID（主子表关系）
   */
  conferenceId: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 用户姓名
   */
  userName: string;

  /**
   * 参与角色
   */
  participantRole: number;

  /**
   * 出席状态（待确认/已接受/已拒绝/已签到/缺席）
   */
  attendanceStatus: number;

  /**
   * 签到时间
   */
  checkInTime?: string;

  /**
   * 签退时间
   */
  checkOutTime?: string;

  /**
   * 签到方式
   */
  checkInMethod: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

