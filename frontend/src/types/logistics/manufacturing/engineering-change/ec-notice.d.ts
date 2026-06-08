// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-notice.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 工程变更通知单实体（EC Notice）。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与本模块对接。
 * 对应前端 TaktEcNoticeDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EcNotice
 * @description 对应后端 TaktEcNoticeDto
 */
export interface EcNotice extends ApprovalDtoBase {
  /**
   * EcNoticeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecNoticeId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNoticeNo: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId: string;

  /**
   * 关联的设变主表名称（填充字段）
   */
  ecName?: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo: string;

  /**
   * 设变主题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNoticeDate: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNoticeDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNoticeDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNoticeNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNoticeNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNoticeMethod: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNoticeStatus: number;

  /**
   * 流程实例 ID（<see cref="Workflow.TaktFlowInstance"/>；发起审批后由业务写入）
   */
  flowInstanceId?: string;

  /**
   * 流程实例 名称（填充字段）
   */
  flowInstanceName?: string;

  /**
   * 关联的设变主表 （主表：TaktEc）
   */
  ec?: Ec;

}


/**
 * EcNotice 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcNoticeQuery
 * @description 对应后端 TaktEcNoticeQueryDto
 */
export interface EcNoticeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNoticeNo?: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo?: string;

  /**
   * 设变主题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期（范围查询-开始）
   */
  ecNoticeDateStart?: string;

  /**
   * 通知日期（范围查询-结束）
   */
  ecNoticeDateEnd?: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNoticeDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNoticeDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNoticeNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNoticeNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNoticeMethod?: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNoticeStatus?: number;

  /**
   * 流程实例 ID（<see cref="Workflow.TaktFlowInstance"/>；发起审批后由业务写入）
   */
  flowInstanceId?: string;

  /**
   * 审批状态（TaktApprovalStatus）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

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
 * 创建EcNotice DTO
 * 对应前端 EcNoticeCreate
 * @description 对应后端 TaktEcNoticeCreateDto
 */
export interface EcNoticeCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNoticeNo: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo: string;

  /**
   * 设变主题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNoticeDate: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNoticeDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNoticeDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNoticeNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNoticeNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNoticeMethod: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNoticeStatus: number;

  /**
   * 流程实例 ID（<see cref="Workflow.TaktFlowInstance"/>；发起审批后由业务写入）
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

}


/**
 * 更新EcNotice DTO
 * 继承 TaktEcNoticeCreateDto，添加 EcNoticeId 字段
 * 对应前端 EcNoticeUpdate
 * @description 对应后端 TaktEcNoticeUpdateDto
 */
export interface EcNoticeUpdate extends EcNoticeCreate {
  /**
   * EcNoticeID（标识要更新的实体）
   */
  ecNoticeId: string;

}


/**
 * EcNotice 状态更新 DTO
 * 对应前端 EcNoticeStatus
 * @description 对应后端 TaktEcNoticeStatusDto
 */
export interface EcNoticeStatus {
  /**
   * EcNoticeID
   */
  ecNoticeId: string;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNoticeStatus: number;

}


/**
 * EcNotice 导入模板行 DTO
 * 对应前端 EcNoticeTemplate
 * @description 对应后端 TaktEcNoticeTemplateDto
 */
export interface EcNoticeTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNoticeNo?: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo?: string;

  /**
   * 设变主题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNoticeDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNoticeDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNoticeNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNoticeNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNoticeMethod?: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNoticeStatus?: number;

  /**
   * 流程实例 ID（<see cref="Workflow.TaktFlowInstance"/>；发起审批后由业务写入）
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

}


/**
 * EcNotice 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcNoticeImport
 * @description 对应后端 TaktEcNoticeImportDto
 */
export interface EcNoticeImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNoticeNo?: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo?: string;

  /**
   * 设变主题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNoticeDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNoticeDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNoticeNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNoticeNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNoticeMethod?: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNoticeStatus?: number;

  /**
   * 流程实例 ID（<see cref="Workflow.TaktFlowInstance"/>；发起审批后由业务写入）
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

}


/**
 * EcNotice 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcNoticeExport
 * @description 对应后端 TaktEcNoticeExportDto
 */
export interface EcNoticeExport {
  /**
   * EcNoticeID
   */
  ecNoticeId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNoticeNo: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo: string;

  /**
   * 设变主题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNoticeDate: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNoticeDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNoticeDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNoticeNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNoticeNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNoticeMethod: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNoticeStatus: number;

  /**
   * 流程实例 ID（<see cref="Workflow.TaktFlowInstance"/>；发起审批后由业务写入）
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

