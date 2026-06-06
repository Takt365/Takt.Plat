// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-failure-meeting.d.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 品质问题应对明细 - 会议/调查/试验费用
 * 对应前端 TaktQualityFailureMeetingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityFailureMeeting
 * @description 对应后端 TaktQualityFailureMeetingDto
 */
export interface QualityFailureMeeting extends CompanyDtoBase {
  /**
   * QualityFailureMeetingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityFailureMeetingId: string;

  /**
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId: string;

  /**
   * 品质问题主表名称（填充字段）
   */
  qualityFailureName?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 直接人员费率（元/分钟）
   */
  directManpowerCostPerMinute: number;

  /**
   * 间接人员费率（元/分钟）
   */
  indirectManpowerCostPerMinute: number;

  /**
   * 讨论调查试验内容(会议记录)
   */
  meetingInvestigationContent?: string;

  /**
   * 讨论调查试验费用(元)
   */
  meetingInvestigationCost: number;

  /**
   * 讨论会使用时间(分钟)
   */
  meetingTimeMinutes: number;

  /**
   * 直接人员参加人数
   */
  directParticipantCount: number;

  /**
   * 间接人员参加人数
   */
  indirectParticipantCount: number;

  /**
   * 调查评价试验工作时间（分钟）
   */
  investigationWorkTimeMinutes: number;

  /**
   * 交通费、旅费（元）
   */
  travelCost: number;

  /**
   * 其他费用（元）
   */
  otherExpenses: number;

  /**
   * 其他作业時間（分钟）
   */
  otherWorkTimeMinutes: number;

  /**
   * 其他设备购入费、工程费、搬运费等（元）
   */
  otherApparatusCost: number;

  /**
   * 品质问题対応记录者（会议调查试验记录者）
   */
  meetingRecorder?: string;

  /**
   * 质量问题主表（导航属性） （主表：TaktQualityFailure）
   */
  issue?: QualityFailure;

}


/**
 * QualityFailureMeeting 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityFailureMeetingQuery
 * @description 对应后端 TaktQualityFailureMeetingQueryDto
 */
export interface QualityFailureMeetingQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 直接人员费率（元/分钟）
   */
  directManpowerCostPerMinute?: number;

  /**
   * 间接人员费率（元/分钟）
   */
  indirectManpowerCostPerMinute?: number;

  /**
   * 讨论调查试验内容(会议记录)
   */
  meetingInvestigationContent?: string;

  /**
   * 讨论调查试验费用(元)
   */
  meetingInvestigationCost?: number;

  /**
   * 讨论会使用时间(分钟)
   */
  meetingTimeMinutes?: number;

  /**
   * 直接人员参加人数
   */
  directParticipantCount?: number;

  /**
   * 间接人员参加人数
   */
  indirectParticipantCount?: number;

  /**
   * 调查评价试验工作时间（分钟）
   */
  investigationWorkTimeMinutes?: number;

  /**
   * 交通费、旅费（元）
   */
  travelCost?: number;

  /**
   * 其他费用（元）
   */
  otherExpenses?: number;

  /**
   * 其他作业時間（分钟）
   */
  otherWorkTimeMinutes?: number;

  /**
   * 其他设备购入费、工程费、搬运费等（元）
   */
  otherApparatusCost?: number;

  /**
   * 品质问题対応记录者（会议调查试验记录者）
   */
  meetingRecorder?: string;

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
 * 创建QualityFailureMeeting DTO
 * 对应前端 QualityFailureMeetingCreate
 * @description 对应后端 TaktQualityFailureMeetingCreateDto
 */
export interface QualityFailureMeetingCreate {
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
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 直接人员费率（元/分钟）
   */
  directManpowerCostPerMinute: number;

  /**
   * 间接人员费率（元/分钟）
   */
  indirectManpowerCostPerMinute: number;

  /**
   * 讨论调查试验内容(会议记录)
   */
  meetingInvestigationContent?: string;

  /**
   * 讨论调查试验费用(元)
   */
  meetingInvestigationCost: number;

  /**
   * 讨论会使用时间(分钟)
   */
  meetingTimeMinutes: number;

  /**
   * 直接人员参加人数
   */
  directParticipantCount: number;

  /**
   * 间接人员参加人数
   */
  indirectParticipantCount: number;

  /**
   * 调查评价试验工作时间（分钟）
   */
  investigationWorkTimeMinutes: number;

  /**
   * 交通费、旅费（元）
   */
  travelCost: number;

  /**
   * 其他费用（元）
   */
  otherExpenses: number;

  /**
   * 其他作业時間（分钟）
   */
  otherWorkTimeMinutes: number;

  /**
   * 其他设备购入费、工程费、搬运费等（元）
   */
  otherApparatusCost: number;

  /**
   * 品质问题対応记录者（会议调查试验记录者）
   */
  meetingRecorder?: string;

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
 * 更新QualityFailureMeeting DTO
 * 继承 TaktQualityFailureMeetingCreateDto，添加 QualityFailureMeetingId 字段
 * 对应前端 QualityFailureMeetingUpdate
 * @description 对应后端 TaktQualityFailureMeetingUpdateDto
 */
export interface QualityFailureMeetingUpdate extends QualityFailureMeetingCreate {
  /**
   * QualityFailureMeetingID（标识要更新的实体）
   */
  qualityFailureMeetingId: string;

}


/**
 * QualityFailureMeeting 导入模板行 DTO
 * 对应前端 QualityFailureMeetingTemplate
 * @description 对应后端 TaktQualityFailureMeetingTemplateDto
 */
export interface QualityFailureMeetingTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 讨论调查试验内容(会议记录)
   */
  meetingInvestigationContent?: string;

  /**
   * 讨论会使用时间(分钟)
   */
  meetingTimeMinutes?: number;

  /**
   * 直接人员参加人数
   */
  directParticipantCount?: number;

  /**
   * 间接人员参加人数
   */
  indirectParticipantCount?: number;

  /**
   * 调查评价试验工作时间（分钟）
   */
  investigationWorkTimeMinutes?: number;

  /**
   * 其他作业時間（分钟）
   */
  otherWorkTimeMinutes?: number;

  /**
   * 品质问题対応记录者（会议调查试验记录者）
   */
  meetingRecorder?: string;

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
 * QualityFailureMeeting 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityFailureMeetingImport
 * @description 对应后端 TaktQualityFailureMeetingImportDto
 */
export interface QualityFailureMeetingImport {
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
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 讨论调查试验内容(会议记录)
   */
  meetingInvestigationContent?: string;

  /**
   * 讨论会使用时间(分钟)
   */
  meetingTimeMinutes?: number;

  /**
   * 直接人员参加人数
   */
  directParticipantCount?: number;

  /**
   * 间接人员参加人数
   */
  indirectParticipantCount?: number;

  /**
   * 调查评价试验工作时间（分钟）
   */
  investigationWorkTimeMinutes?: number;

  /**
   * 其他作业時間（分钟）
   */
  otherWorkTimeMinutes?: number;

  /**
   * 品质问题対応记录者（会议调查试验记录者）
   */
  meetingRecorder?: string;

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
 * QualityFailureMeeting 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityFailureMeetingExport
 * @description 对应后端 TaktQualityFailureMeetingExportDto
 */
export interface QualityFailureMeetingExport {
  /**
   * QualityFailureMeetingID
   */
  qualityFailureMeetingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 直接人员费率（元/分钟）
   */
  directManpowerCostPerMinute: number;

  /**
   * 间接人员费率（元/分钟）
   */
  indirectManpowerCostPerMinute: number;

  /**
   * 讨论调查试验内容(会议记录)
   */
  meetingInvestigationContent?: string;

  /**
   * 讨论调查试验费用(元)
   */
  meetingInvestigationCost: number;

  /**
   * 讨论会使用时间(分钟)
   */
  meetingTimeMinutes: number;

  /**
   * 直接人员参加人数
   */
  directParticipantCount: number;

  /**
   * 间接人员参加人数
   */
  indirectParticipantCount: number;

  /**
   * 调查评价试验工作时间（分钟）
   */
  investigationWorkTimeMinutes: number;

  /**
   * 交通费、旅费（元）
   */
  travelCost: number;

  /**
   * 其他费用（元）
   */
  otherExpenses: number;

  /**
   * 其他作业時間（分钟）
   */
  otherWorkTimeMinutes: number;

  /**
   * 其他设备购入费、工程费、搬运费等（元）
   */
  otherApparatusCost: number;

  /**
   * 品质问题対応记录者（会议调查试验记录者）
   */
  meetingRecorder?: string;

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

