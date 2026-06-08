// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：countersign.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 会签单实体
 * 对应前端 TaktCountersignDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Countersign
 * @description 对应后端 TaktCountersignDto
 */
export interface Countersign extends ApprovalDtoBase {
  /**
   * CountersignID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  countersignId: string;

  /**
   * 会签编号
   */
  countersignCode: string;

  /**
   * 会签部门 JSON
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

  /**
   * 流程实例 名称（填充字段）
   */
  flowInstanceName?: string;

  /**
   * 申请人（员工 ID）
   */
  applicantBy: string;

  /**
   * 申请部门
   */
  applicationDept?: string;

  /**
   * 经费负担部门
   */
  costBearerDept?: string;

  /**
   * 是否有预算
   */
  isBudget: number;

  /**
   * 预算项目
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount: number;

  /**
   * 申请金额
   */
  applicationAmount: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 附件 JSON
   */
  attachments?: string;

  /**
   * 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
   */
  countersignStatus: number;

}


/**
 * Countersign 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CountersignQuery
 * @description 对应后端 TaktCountersignQueryDto
 */
export interface CountersignQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 会签编号
   */
  countersignCode?: string;

  /**
   * 会签部门 JSON
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

  /**
   * 申请人（员工 ID）
   */
  applicantBy?: string;

  /**
   * 申请部门
   */
  applicationDept?: string;

  /**
   * 经费负担部门
   */
  costBearerDept?: string;

  /**
   * 是否有预算
   */
  isBudget?: number;

  /**
   * 预算项目
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount?: number;

  /**
   * 申请金额
   */
  applicationAmount?: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 附件 JSON
   */
  attachments?: string;

  /**
   * 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
   */
  countersignStatus?: number;

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
 * 创建Countersign DTO
 * 对应前端 CountersignCreate
 * @description 对应后端 TaktCountersignCreateDto
 */
export interface CountersignCreate {
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
   * 会签编号
   */
  countersignCode: string;

  /**
   * 会签部门 JSON
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

  /**
   * 申请人（员工 ID）
   */
  applicantBy: string;

  /**
   * 申请部门
   */
  applicationDept?: string;

  /**
   * 经费负担部门
   */
  costBearerDept?: string;

  /**
   * 是否有预算
   */
  isBudget: number;

  /**
   * 预算项目
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount: number;

  /**
   * 申请金额
   */
  applicationAmount: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 附件 JSON
   */
  attachments?: string;

  /**
   * 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
   */
  countersignStatus: number;

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
 * 更新Countersign DTO
 * 继承 TaktCountersignCreateDto，添加 CountersignId 字段
 * 对应前端 CountersignUpdate
 * @description 对应后端 TaktCountersignUpdateDto
 */
export interface CountersignUpdate extends CountersignCreate {
  /**
   * CountersignID（标识要更新的实体）
   */
  countersignId: string;

}


/**
 * Countersign 状态更新 DTO
 * 对应前端 CountersignStatus
 * @description 对应后端 TaktCountersignStatusDto
 */
export interface CountersignStatus {
  /**
   * CountersignID
   */
  countersignId: string;

  /**
   * 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
   */
  countersignStatus: number;

}


/**
 * Countersign 导入模板行 DTO
 * 对应前端 CountersignTemplate
 * @description 对应后端 TaktCountersignTemplateDto
 */
export interface CountersignTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 会签编号
   */
  countersignCode?: string;

  /**
   * 会签部门 JSON
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

  /**
   * 申请人（员工 ID）
   */
  applicantBy?: string;

  /**
   * 申请部门
   */
  applicationDept?: string;

  /**
   * 经费负担部门
   */
  costBearerDept?: string;

  /**
   * 是否有预算
   */
  isBudget?: number;

  /**
   * 预算项目
   */
  budgetItem?: string;

  /**
   * 标题
   */
  countersignTitle?: string;

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
 * Countersign 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CountersignImport
 * @description 对应后端 TaktCountersignImportDto
 */
export interface CountersignImport {
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
   * 会签编号
   */
  countersignCode?: string;

  /**
   * 会签部门 JSON
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

  /**
   * 申请人（员工 ID）
   */
  applicantBy?: string;

  /**
   * 申请部门
   */
  applicationDept?: string;

  /**
   * 经费负担部门
   */
  costBearerDept?: string;

  /**
   * 是否有预算
   */
  isBudget?: number;

  /**
   * 预算项目
   */
  budgetItem?: string;

  /**
   * 标题
   */
  countersignTitle?: string;

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
 * Countersign 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CountersignExport
 * @description 对应后端 TaktCountersignExportDto
 */
export interface CountersignExport {
  /**
   * CountersignID
   */
  countersignId: string;

  /**
   * 会签编号
   */
  countersignCode: string;

  /**
   * 会签部门 JSON
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

  /**
   * 申请人（员工 ID）
   */
  applicantBy: string;

  /**
   * 申请部门
   */
  applicationDept?: string;

  /**
   * 经费负担部门
   */
  costBearerDept?: string;

  /**
   * 是否有预算
   */
  isBudget: number;

  /**
   * 预算项目
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount: number;

  /**
   * 申请金额
   */
  applicationAmount: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 附件 JSON
   */
  attachments?: string;

  /**
   * 会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）
   */
  countersignStatus: number;

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

