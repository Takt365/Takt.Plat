// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perf-analysis.d.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 分析改进
 * 对应前端 TaktPerfAnalysisDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 PerfAnalysis
 * @description 对应后端 TaktPerfAnalysisDto
 */
export interface PerfAnalysis extends ApprovalDtoBase {
  /**
   * PerfAnalysisID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  perfAnalysisId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 关联考核评估 ID
   */
  assessmentId: string;

  /**
   * 关联考核评估 名称（填充字段）
   */
  assessmentName?: string;

  /**
   * 改进计划标题
   */
  planTitle: string;

  /**
   * 改进领域
   */
  improvementArea: string;

  /**
   * 当前状况描述
   */
  currentSituation: string;

  /**
   * 改进目标
   */
  improvementGoal: string;

  /**
   * 改进措施
   */
  improvementActions: string;

  /**
   * 计划制定日期
   */
  planDate: string;

  /**
   * 目标完成日期
   */
  targetCompletionDate: string;

  /**
   * 进度百分比（%）
   */
  progressPercentage: number;

  /**
   * 改进结果说明
   */
  resultDescription: string;

  /**
   * 指导老师 ID
   */
  mentorId: string;

  /**
   * 指导老师 名称（填充字段）
   */
  mentorName?: string;

  /**
   * 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
   */
  improvementStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * PerfAnalysis 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PerfAnalysisQuery
 * @description 对应后端 TaktPerfAnalysisQueryDto
 */
export interface PerfAnalysisQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 员工 ID
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 关联考核评估 ID
   */
  assessmentId?: string;

  /**
   * 改进计划标题
   */
  planTitle?: string;

  /**
   * 改进领域
   */
  improvementArea?: string;

  /**
   * 当前状况描述
   */
  currentSituation?: string;

  /**
   * 改进目标
   */
  improvementGoal?: string;

  /**
   * 改进措施
   */
  improvementActions?: string;

  /**
   * 计划制定日期（范围查询-开始）
   */
  planDateStart?: string;

  /**
   * 计划制定日期（范围查询-结束）
   */
  planDateEnd?: string;

  /**
   * 目标完成日期（范围查询-开始）
   */
  targetCompletionDateStart?: string;

  /**
   * 目标完成日期（范围查询-结束）
   */
  targetCompletionDateEnd?: string;

  /**
   * 进度百分比（%）
   */
  progressPercentage?: number;

  /**
   * 改进结果说明
   */
  resultDescription?: string;

  /**
   * 指导老师 ID
   */
  mentorId?: string;

  /**
   * 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
   */
  improvementStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建PerfAnalysis DTO
 * 对应前端 PerfAnalysisCreate
 * @description 对应后端 TaktPerfAnalysisCreateDto
 */
export interface PerfAnalysisCreate {
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
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 关联考核评估 ID
   */
  assessmentId: string;

  /**
   * 改进计划标题
   */
  planTitle: string;

  /**
   * 改进领域
   */
  improvementArea: string;

  /**
   * 当前状况描述
   */
  currentSituation: string;

  /**
   * 改进目标
   */
  improvementGoal: string;

  /**
   * 改进措施
   */
  improvementActions: string;

  /**
   * 计划制定日期
   */
  planDate: string;

  /**
   * 目标完成日期
   */
  targetCompletionDate: string;

  /**
   * 进度百分比（%）
   */
  progressPercentage: number;

  /**
   * 改进结果说明
   */
  resultDescription: string;

  /**
   * 指导老师 ID
   */
  mentorId: string;

  /**
   * 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
   */
  improvementStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新PerfAnalysis DTO
 * 继承 TaktPerfAnalysisCreateDto，添加 PerfAnalysisId 字段
 * 对应前端 PerfAnalysisUpdate
 * @description 对应后端 TaktPerfAnalysisUpdateDto
 */
export interface PerfAnalysisUpdate extends PerfAnalysisCreate {
  /**
   * PerfAnalysisID（标识要更新的实体）
   */
  perfAnalysisId: string;

}


/**
 * PerfAnalysis 状态更新 DTO
 * 对应前端 PerfAnalysisStatus
 * @description 对应后端 TaktPerfAnalysisStatusDto
 */
export interface PerfAnalysisStatus {
  /**
   * PerfAnalysisID
   */
  perfAnalysisId: string;

  /**
   * 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
   */
  improvementStatus: number;

}


/**
 * PerfAnalysis 导入模板行 DTO
 * 对应前端 PerfAnalysisTemplate
 * @description 对应后端 TaktPerfAnalysisTemplateDto
 */
export interface PerfAnalysisTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 员工 ID
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 关联考核评估 ID
   */
  assessmentId?: string;

  /**
   * 改进计划标题
   */
  planTitle?: string;

  /**
   * 改进领域
   */
  improvementArea?: string;

  /**
   * 当前状况描述
   */
  currentSituation?: string;

  /**
   * 改进目标
   */
  improvementGoal?: string;

  /**
   * 改进措施
   */
  improvementActions?: string;

  /**
   * 改进结果说明
   */
  resultDescription?: string;

  /**
   * 指导老师 ID
   */
  mentorId?: string;

  /**
   * 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
   */
  improvementStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PerfAnalysis 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PerfAnalysisImport
 * @description 对应后端 TaktPerfAnalysisImportDto
 */
export interface PerfAnalysisImport {
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
   * 员工 ID
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 关联考核评估 ID
   */
  assessmentId?: string;

  /**
   * 改进计划标题
   */
  planTitle?: string;

  /**
   * 改进领域
   */
  improvementArea?: string;

  /**
   * 当前状况描述
   */
  currentSituation?: string;

  /**
   * 改进目标
   */
  improvementGoal?: string;

  /**
   * 改进措施
   */
  improvementActions?: string;

  /**
   * 改进结果说明
   */
  resultDescription?: string;

  /**
   * 指导老师 ID
   */
  mentorId?: string;

  /**
   * 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
   */
  improvementStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PerfAnalysis 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfAnalysisExport
 * @description 对应后端 TaktPerfAnalysisExportDto
 */
export interface PerfAnalysisExport {
  /**
   * PerfAnalysisID
   */
  perfAnalysisId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 关联考核评估 ID
   */
  assessmentId: string;

  /**
   * 改进计划标题
   */
  planTitle: string;

  /**
   * 改进领域
   */
  improvementArea: string;

  /**
   * 当前状况描述
   */
  currentSituation: string;

  /**
   * 改进目标
   */
  improvementGoal: string;

  /**
   * 改进措施
   */
  improvementActions: string;

  /**
   * 计划制定日期
   */
  planDate: string;

  /**
   * 目标完成日期
   */
  targetCompletionDate: string;

  /**
   * 进度百分比（%）
   */
  progressPercentage: number;

  /**
   * 改进结果说明
   */
  resultDescription: string;

  /**
   * 指导老师 ID
   */
  mentorId: string;

  /**
   * 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
   */
  improvementStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

