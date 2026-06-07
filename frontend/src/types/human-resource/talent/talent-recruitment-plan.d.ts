// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/talent
// 文件名称：talent-recruitment-plan.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/talent 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 招聘计划（审批单，状态见 <see cref="TaktApprovalEntityBase.ApprovalStatus"/>）
 * 对应前端 TaktTalentRecruitmentPlanDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 TalentRecruitmentPlan
 * @description 对应后端 TaktTalentRecruitmentPlanDto
 */
export interface TalentRecruitmentPlan extends ApprovalDtoBase {
  /**
   * TalentRecruitmentPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  talentRecruitmentPlanId: string;

  /**
   * 用人需求ID
   */
  staffingRequirementId: string;

  /**
   * 用人需求名称（填充字段）
   */
  staffingRequirementName?: string;

  /**
   * 计划单号（租户+公司内业务编号）
   */
  planNo: string;

  /**
   * 计划制定日期
   */
  planDate: string;

  /**
   * 计划招聘开始日期
   */
  planStartDate: string;

  /**
   * 计划招聘结束日期
   */
  planEndDate?: string;

  /**
   * 计划招聘人数
   */
  planHeadcount: number;

  /**
   * 计划说明
   */
  reason?: string;

  /**
   * 用人需求 （主表：TaktTalentStaffingRequirement）
   */
  staffingRequirement?: TalentStaffingRequirement;

  /**
   * 职位发布 （子表：TaktTalentJobPosting）
   */
  talentJobPostings?: TalentJobPosting[];

}


/**
 * TalentRecruitmentPlan 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TalentRecruitmentPlanQuery
 * @description 对应后端 TaktTalentRecruitmentPlanQueryDto
 */
export interface TalentRecruitmentPlanQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 用人需求ID
   */
  staffingRequirementId?: string;

  /**
   * 计划单号（租户+公司内业务编号）
   */
  planNo?: string;

  /**
   * 计划制定日期（范围查询-开始）
   */
  planDateStart?: string;

  /**
   * 计划制定日期（范围查询-结束）
   */
  planDateEnd?: string;

  /**
   * 计划招聘开始日期（范围查询-开始）
   */
  planStartDateStart?: string;

  /**
   * 计划招聘开始日期（范围查询-结束）
   */
  planStartDateEnd?: string;

  /**
   * 计划招聘结束日期（范围查询-开始）
   */
  planEndDateStart?: string;

  /**
   * 计划招聘结束日期（范围查询-结束）
   */
  planEndDateEnd?: string;

  /**
   * 计划招聘人数
   */
  planHeadcount?: number;

  /**
   * 计划说明
   */
  reason?: string;

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
 * 创建TalentRecruitmentPlan DTO
 * 对应前端 TalentRecruitmentPlanCreate
 * @description 对应后端 TaktTalentRecruitmentPlanCreateDto
 */
export interface TalentRecruitmentPlanCreate {
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
   * 用人需求ID
   */
  staffingRequirementId: string;

  /**
   * 计划单号（租户+公司内业务编号）
   */
  planNo: string;

  /**
   * 计划制定日期
   */
  planDate: string;

  /**
   * 计划招聘开始日期
   */
  planStartDate: string;

  /**
   * 计划招聘结束日期
   */
  planEndDate?: string;

  /**
   * 计划招聘人数
   */
  planHeadcount: number;

  /**
   * 计划说明
   */
  reason?: string;

  /**
   * 职位发布（子表，级联保存）
   */
  talentJobPostings?: TalentJobPostingCreate[];

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
 * 更新TalentRecruitmentPlan DTO
 * 继承 TaktTalentRecruitmentPlanCreateDto，添加 TalentRecruitmentPlanId 字段
 * 对应前端 TalentRecruitmentPlanUpdate
 * @description 对应后端 TaktTalentRecruitmentPlanUpdateDto
 */
export interface TalentRecruitmentPlanUpdate extends TalentRecruitmentPlanCreate {
  /**
   * TalentRecruitmentPlanID（标识要更新的实体）
   */
  talentRecruitmentPlanId: string;

}


/**
 * TalentRecruitmentPlan 导入模板行 DTO
 * 对应前端 TalentRecruitmentPlanTemplate
 * @description 对应后端 TaktTalentRecruitmentPlanTemplateDto
 */
export interface TalentRecruitmentPlanTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 用人需求ID
   */
  staffingRequirementId?: string;

  /**
   * 计划单号（租户+公司内业务编号）
   */
  planNo?: string;

  /**
   * 计划招聘人数
   */
  planHeadcount?: number;

  /**
   * 计划说明
   */
  reason?: string;

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
 * TalentRecruitmentPlan 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TalentRecruitmentPlanImport
 * @description 对应后端 TaktTalentRecruitmentPlanImportDto
 */
export interface TalentRecruitmentPlanImport {
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
   * 用人需求ID
   */
  staffingRequirementId?: string;

  /**
   * 计划单号（租户+公司内业务编号）
   */
  planNo?: string;

  /**
   * 计划招聘人数
   */
  planHeadcount?: number;

  /**
   * 计划说明
   */
  reason?: string;

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
 * TalentRecruitmentPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TalentRecruitmentPlanExport
 * @description 对应后端 TaktTalentRecruitmentPlanExportDto
 */
export interface TalentRecruitmentPlanExport {
  /**
   * TalentRecruitmentPlanID
   */
  talentRecruitmentPlanId: string;

  /**
   * 用人需求ID
   */
  staffingRequirementId: string;

  /**
   * 计划单号（租户+公司内业务编号）
   */
  planNo: string;

  /**
   * 计划制定日期
   */
  planDate: string;

  /**
   * 计划招聘开始日期
   */
  planStartDate: string;

  /**
   * 计划招聘结束日期
   */
  planEndDate?: string;

  /**
   * 计划招聘人数
   */
  planHeadcount: number;

  /**
   * 计划说明
   */
  reason?: string;

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

