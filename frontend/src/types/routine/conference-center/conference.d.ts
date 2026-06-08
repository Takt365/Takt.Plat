// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/conference-center
// 文件名称：conference.d.ts
// 创建时间：2026-06-08
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
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理
 * 对应前端 TaktConferenceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Conference
 * @description 对应后端 TaktConferenceDto
 */
export interface Conference extends CompanyDtoBase {
  /**
   * ConferenceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  conferenceId: string;

  /**
   * 会议编码（租户+公司内唯一）
   */
  conferenceCode: string;

  /**
   * 会议标题
   */
  title: string;

  /**
   * 会议类型
   */
  conferenceType: number;

  /**
   * 会议状态
   */
  conferenceStatus: number;

  /**
   * 开始时间
   */
  startTime: string;

  /**
   * 结束时间
   */
  endTime: string;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程
   */
  agenda?: string;

  /**
   * 会议内容（会议纪要正文，富文本 HTML）
   */
  content?: string;

  /**
   * 会议纪要摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 组织人 ID
   */
  organizerId: string;

  /**
   * 组织人姓名
   */
  organizerName: string;

  /**
   * 主办部门 ID
   */
  deptId?: string;

  /**
   * 主办部门名称
   */
  deptName?: string;

  /**
   * 最大参会人数（0 表示不限）
   */
  maxParticipants: number;

  /**
   * 提前提醒分钟数（0 表示不提醒）
   */
  reminderMinutes: number;

  /**
   * 流程实例 ID（会议审批工作流）
   */
  flowInstanceId?: string;

  /**
   * 流程实例 名称（填充字段）
   */
  flowInstanceName?: string;

  /**
   * 参与人列表（主子表关系） （子表：TaktConferenceParticipant）
   */
  participants?: ConferenceParticipant[];

}


/**
 * Conference 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConferenceQuery
 * @description 对应后端 TaktConferenceQueryDto
 */
export interface ConferenceQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 会议编码（租户+公司内唯一）
   */
  conferenceCode?: string;

  /**
   * 会议标题
   */
  title?: string;

  /**
   * 会议类型
   */
  conferenceType?: number;

  /**
   * 会议状态
   */
  conferenceStatus?: number;

  /**
   * 开始时间（范围查询-开始）
   */
  startTimeStart?: string;

  /**
   * 开始时间（范围查询-结束）
   */
  startTimeEnd?: string;

  /**
   * 结束时间（范围查询-开始）
   */
  endTimeStart?: string;

  /**
   * 结束时间（范围查询-结束）
   */
  endTimeEnd?: string;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程
   */
  agenda?: string;

  /**
   * 会议内容（会议纪要正文，富文本 HTML）
   */
  content?: string;

  /**
   * 会议纪要摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 组织人 ID
   */
  organizerId?: string;

  /**
   * 组织人姓名
   */
  organizerName?: string;

  /**
   * 主办部门 ID
   */
  deptId?: string;

  /**
   * 主办部门名称
   */
  deptName?: string;

  /**
   * 最大参会人数（0 表示不限）
   */
  maxParticipants?: number;

  /**
   * 提前提醒分钟数（0 表示不提醒）
   */
  reminderMinutes?: number;

  /**
   * 流程实例 ID（会议审批工作流）
   */
  flowInstanceId?: string;

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
 * 创建Conference DTO
 * 对应前端 ConferenceCreate
 * @description 对应后端 TaktConferenceCreateDto
 */
export interface ConferenceCreate {
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
   * 会议编码（租户+公司内唯一）
   */
  conferenceCode: string;

  /**
   * 会议标题
   */
  title: string;

  /**
   * 会议类型
   */
  conferenceType: number;

  /**
   * 会议状态
   */
  conferenceStatus: number;

  /**
   * 开始时间
   */
  startTime: string;

  /**
   * 结束时间
   */
  endTime: string;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程
   */
  agenda?: string;

  /**
   * 会议内容（会议纪要正文，富文本 HTML）
   */
  content?: string;

  /**
   * 会议纪要摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 组织人 ID
   */
  organizerId: string;

  /**
   * 组织人姓名
   */
  organizerName: string;

  /**
   * 主办部门 ID
   */
  deptId?: string;

  /**
   * 主办部门名称
   */
  deptName?: string;

  /**
   * 最大参会人数（0 表示不限）
   */
  maxParticipants: number;

  /**
   * 提前提醒分钟数（0 表示不提醒）
   */
  reminderMinutes: number;

  /**
   * 流程实例 ID（会议审批工作流）
   */
  flowInstanceId?: string;

  /**
   * 参与人列表（主子表关系）（子表，级联保存）
   */
  participants?: ConferenceParticipantCreate[];

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
 * 更新Conference DTO
 * 继承 TaktConferenceCreateDto，添加 ConferenceId 字段
 * 对应前端 ConferenceUpdate
 * @description 对应后端 TaktConferenceUpdateDto
 */
export interface ConferenceUpdate extends ConferenceCreate {
  /**
   * ConferenceID（标识要更新的实体）
   */
  conferenceId: string;

}


/**
 * Conference 状态更新 DTO
 * 对应前端 ConferenceStatus
 * @description 对应后端 TaktConferenceStatusDto
 */
export interface ConferenceStatus {
  /**
   * ConferenceID
   */
  conferenceId: string;

  /**
   * 会议状态
   */
  conferenceStatus: number;

}


/**
 * Conference 导入模板行 DTO
 * 对应前端 ConferenceTemplate
 * @description 对应后端 TaktConferenceTemplateDto
 */
export interface ConferenceTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 会议编码（租户+公司内唯一）
   */
  conferenceCode?: string;

  /**
   * 会议标题
   */
  title?: string;

  /**
   * 会议类型
   */
  conferenceType?: number;

  /**
   * 会议状态
   */
  conferenceStatus?: number;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程
   */
  agenda?: string;

  /**
   * 会议内容（会议纪要正文，富文本 HTML）
   */
  content?: string;

  /**
   * 会议纪要摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 组织人 ID
   */
  organizerId?: string;

  /**
   * 组织人姓名
   */
  organizerName?: string;

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
 * Conference 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConferenceImport
 * @description 对应后端 TaktConferenceImportDto
 */
export interface ConferenceImport {
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
   * 会议编码（租户+公司内唯一）
   */
  conferenceCode?: string;

  /**
   * 会议标题
   */
  title?: string;

  /**
   * 会议类型
   */
  conferenceType?: number;

  /**
   * 会议状态
   */
  conferenceStatus?: number;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程
   */
  agenda?: string;

  /**
   * 会议内容（会议纪要正文，富文本 HTML）
   */
  content?: string;

  /**
   * 会议纪要摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 组织人 ID
   */
  organizerId?: string;

  /**
   * 组织人姓名
   */
  organizerName?: string;

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
 * Conference 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConferenceExport
 * @description 对应后端 TaktConferenceExportDto
 */
export interface ConferenceExport {
  /**
   * ConferenceID
   */
  conferenceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 会议编码（租户+公司内唯一）
   */
  conferenceCode: string;

  /**
   * 会议标题
   */
  title: string;

  /**
   * 会议类型
   */
  conferenceType: number;

  /**
   * 会议状态
   */
  conferenceStatus: number;

  /**
   * 开始时间
   */
  startTime: string;

  /**
   * 结束时间
   */
  endTime: string;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程
   */
  agenda?: string;

  /**
   * 会议内容（会议纪要正文，富文本 HTML）
   */
  content?: string;

  /**
   * 会议纪要摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 组织人 ID
   */
  organizerId: string;

  /**
   * 组织人姓名
   */
  organizerName: string;

  /**
   * 主办部门 ID
   */
  deptId?: string;

  /**
   * 主办部门名称
   */
  deptName?: string;

  /**
   * 最大参会人数（0 表示不限）
   */
  maxParticipants: number;

  /**
   * 提前提醒分钟数（0 表示不提醒）
   */
  reminderMinutes: number;

  /**
   * 流程实例 ID（会议审批工作流）
   */
  flowInstanceId?: string;

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

