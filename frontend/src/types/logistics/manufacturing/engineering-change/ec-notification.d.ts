// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-notification.d.ts
// 创建时间：2026-06-30
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
 * 工程变更通知单（技术阶段一 ④，隶属 TaktEcGijutsu）。技术完成 ①主表 ②附件 ③明细 保存后由 TaktEcGijutsuService 自动生成并派发； 各部门确认后在 TaktEcExec* 执行，技术通过看板/批次监控。FlowInstanceId 由通知审批流程写入（可选）。
 * 对应前端 TaktEcNotificationDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EcNotification
 * @description 对应后端 TaktEcNotificationDto
 */
export interface EcNotification extends ApprovalDtoBase {
  /**
   * EcNotificationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecNotificationId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNotificationNo: string;

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
   * 设变标题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNotificationDate: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNotificationDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNotificationDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNotificationNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNotificationNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNotificationMethod: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNotificationStatus: number;

  /**
   * 关联的设变主表 （主表：TaktEcGijutsu）
   */
  ecEng?: EcGijutsu;

}


/**
 * EcNotification 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcNotificationQuery
 * @description 对应后端 TaktEcNotificationQueryDto
 */
export interface EcNotificationQuery extends TaktPagedQuery {
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
  ecNotificationNo?: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo?: string;

  /**
   * 设变标题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期（范围查询-开始）
   */
  ecNotificationDateStart?: string;

  /**
   * 通知日期（范围查询-结束）
   */
  ecNotificationDateEnd?: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNotificationDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNotificationDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNotificationNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNotificationNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNotificationMethod?: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNotificationStatus?: number;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
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
   * 流程实例 ID
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
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建EcNotification DTO
 * 对应前端 EcNotificationCreate
 * @description 对应后端 TaktEcNotificationCreateDto
 */
export interface EcNotificationCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNotificationNo: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo: string;

  /**
   * 设变标题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNotificationDate: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNotificationDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNotificationDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNotificationNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNotificationNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNotificationMethod: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNotificationStatus: number;

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
 * 更新EcNotification DTO
 * 继承 TaktEcNotificationCreateDto，添加 EcNotificationId 字段
 * 对应前端 EcNotificationUpdate
 * @description 对应后端 TaktEcNotificationUpdateDto
 */
export interface EcNotificationUpdate extends EcNotificationCreate {
  /**
   * EcNotificationID（标识要更新的实体）
   */
  ecNotificationId: string;

}


/**
 * EcNotification 状态更新 DTO
 * 对应前端 EcNotificationStatus
 * @description 对应后端 TaktEcNotificationStatusDto
 */
export interface EcNotificationStatus {
  /**
   * EcNotificationID
   */
  ecNotificationId: string;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNotificationStatus: number;

}


/**
 * EcNotification 导入模板行 DTO
 * 对应前端 EcNotificationTemplate
 * @description 对应后端 TaktEcNotificationTemplateDto
 */
export interface EcNotificationTemplate {
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
  ecNotificationNo?: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo?: string;

  /**
   * 设变标题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNotificationDate?: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNotificationDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNotificationDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNotificationNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNotificationNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNotificationMethod?: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNotificationStatus?: number;

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
 * EcNotification 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcNotificationImport
 * @description 对应后端 TaktEcNotificationImportDto
 */
export interface EcNotificationImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNotificationNo?: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo?: string;

  /**
   * 设变标题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNotificationDate?: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNotificationDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNotificationDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNotificationNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNotificationNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNotificationMethod?: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNotificationStatus?: number;

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
 * EcNotification 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcNotificationExport
 * @description 对应后端 TaktEcNotificationExportDto
 */
export interface EcNotificationExport {
  /**
   * EcNotificationID
   */
  ecNotificationId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNotificationNo: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecId: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecNo: string;

  /**
   * 设变标题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNotificationDate: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
   */
  ecNotificationDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNotificationDeptNames?: string;

  /**
   * 通知人ID（序列化为string以避免Javascript精度问题）
   */
  ecNotificationNotifierId?: string;

  /**
   * 通知人姓名
   */
  ecNotificationNotifierName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNotificationMethod: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNotificationStatus: number;

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

