// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perf-assessment.d.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 员工绩效考核
 * 对应前端 TaktPerfAssessmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PerfAssessment
 * @description 对应后端 TaktPerfAssessmentDto
 */
export interface PerfAssessment extends CompanyDtoBase {
  /**
   * PerfAssessmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  perfAssessmentId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 考核周期（如 2026-Q1、2026-Annual）
   */
  assessmentPeriod: string;

  /**
   * 考核日期
   */
  assessmentDate: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId: string;

  /**
   * 方案指标 名称（填充字段）
   */
  schemeMetricName?: string;

  /**
   * 自评分数
   */
  selfScore: number;

  /**
   * 自评说明
   */
  selfEvaluationNotes: string;

  /**
   * 主管评分
   */
  supervisorScore: number;

  /**
   * 主管评语
   */
  supervisorComments: string;

  /**
   * 综合得分
   */
  finalScore: number;

  /**
   * 绩效等级（A/B/C/D/E）
   */
  performanceGrade: string;

  /**
   * 评审人 ID
   */
  reviewerId: string;

  /**
   * 评审人 名称（填充字段）
   */
  reviewerName?: string;

  /**
   * 面谈日期
   */
  interviewDate: string;

  /**
   * 面谈记录
   */
  interviewNotes: string;

  /**
   * 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
   */
  assessmentStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * PerfAssessment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PerfAssessmentQuery
 * @description 对应后端 TaktPerfAssessmentQueryDto
 */
export interface PerfAssessmentQuery extends TaktPagedQuery {
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
   * 考核周期（如 2026-Q1、2026-Annual）
   */
  assessmentPeriod?: string;

  /**
   * 考核日期（范围查询-开始）
   */
  assessmentDateStart?: string;

  /**
   * 考核日期（范围查询-结束）
   */
  assessmentDateEnd?: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId?: string;

  /**
   * 自评分数
   */
  selfScore?: number;

  /**
   * 自评说明
   */
  selfEvaluationNotes?: string;

  /**
   * 主管评分
   */
  supervisorScore?: number;

  /**
   * 主管评语
   */
  supervisorComments?: string;

  /**
   * 综合得分
   */
  finalScore?: number;

  /**
   * 绩效等级（A/B/C/D/E）
   */
  performanceGrade?: string;

  /**
   * 评审人 ID
   */
  reviewerId?: string;

  /**
   * 面谈日期（范围查询-开始）
   */
  interviewDateStart?: string;

  /**
   * 面谈日期（范围查询-结束）
   */
  interviewDateEnd?: string;

  /**
   * 面谈记录
   */
  interviewNotes?: string;

  /**
   * 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
   */
  assessmentStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 创建PerfAssessment DTO
 * 对应前端 PerfAssessmentCreate
 * @description 对应后端 TaktPerfAssessmentCreateDto
 */
export interface PerfAssessmentCreate {
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
   * 考核周期（如 2026-Q1、2026-Annual）
   */
  assessmentPeriod: string;

  /**
   * 考核日期
   */
  assessmentDate: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId: string;

  /**
   * 自评分数
   */
  selfScore: number;

  /**
   * 自评说明
   */
  selfEvaluationNotes: string;

  /**
   * 主管评分
   */
  supervisorScore: number;

  /**
   * 主管评语
   */
  supervisorComments: string;

  /**
   * 综合得分
   */
  finalScore: number;

  /**
   * 绩效等级（A/B/C/D/E）
   */
  performanceGrade: string;

  /**
   * 评审人 ID
   */
  reviewerId: string;

  /**
   * 面谈日期
   */
  interviewDate: string;

  /**
   * 面谈记录
   */
  interviewNotes: string;

  /**
   * 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
   */
  assessmentStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 更新PerfAssessment DTO
 * 继承 TaktPerfAssessmentCreateDto，添加 PerfAssessmentId 字段
 * 对应前端 PerfAssessmentUpdate
 * @description 对应后端 TaktPerfAssessmentUpdateDto
 */
export interface PerfAssessmentUpdate extends PerfAssessmentCreate {
  /**
   * PerfAssessmentID（标识要更新的实体）
   */
  perfAssessmentId: string;

}


/**
 * PerfAssessment 状态更新 DTO
 * 对应前端 PerfAssessmentStatus
 * @description 对应后端 TaktPerfAssessmentStatusDto
 */
export interface PerfAssessmentStatus {
  /**
   * PerfAssessmentID
   */
  perfAssessmentId: string;

  /**
   * 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
   */
  assessmentStatus: number;

}


/**
 * PerfAssessment 导入模板行 DTO
 * 对应前端 PerfAssessmentTemplate
 * @description 对应后端 TaktPerfAssessmentTemplateDto
 */
export interface PerfAssessmentTemplate {
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
   * 考核周期（如 2026-Q1、2026-Annual）
   */
  assessmentPeriod?: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId?: string;

  /**
   * 自评说明
   */
  selfEvaluationNotes?: string;

  /**
   * 主管评语
   */
  supervisorComments?: string;

  /**
   * 绩效等级（A/B/C/D/E）
   */
  performanceGrade?: string;

  /**
   * 评审人 ID
   */
  reviewerId?: string;

  /**
   * 面谈记录
   */
  interviewNotes?: string;

  /**
   * 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
   */
  assessmentStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * PerfAssessment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PerfAssessmentImport
 * @description 对应后端 TaktPerfAssessmentImportDto
 */
export interface PerfAssessmentImport {
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
   * 考核周期（如 2026-Q1、2026-Annual）
   */
  assessmentPeriod?: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId?: string;

  /**
   * 自评说明
   */
  selfEvaluationNotes?: string;

  /**
   * 主管评语
   */
  supervisorComments?: string;

  /**
   * 绩效等级（A/B/C/D/E）
   */
  performanceGrade?: string;

  /**
   * 评审人 ID
   */
  reviewerId?: string;

  /**
   * 面谈记录
   */
  interviewNotes?: string;

  /**
   * 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
   */
  assessmentStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * PerfAssessment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfAssessmentExport
 * @description 对应后端 TaktPerfAssessmentExportDto
 */
export interface PerfAssessmentExport {
  /**
   * PerfAssessmentID
   */
  perfAssessmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 考核周期（如 2026-Q1、2026-Annual）
   */
  assessmentPeriod: string;

  /**
   * 考核日期
   */
  assessmentDate: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId: string;

  /**
   * 自评分数
   */
  selfScore: number;

  /**
   * 自评说明
   */
  selfEvaluationNotes: string;

  /**
   * 主管评分
   */
  supervisorScore: number;

  /**
   * 主管评语
   */
  supervisorComments: string;

  /**
   * 综合得分
   */
  finalScore: number;

  /**
   * 绩效等级（A/B/C/D/E）
   */
  performanceGrade: string;

  /**
   * 评审人 ID
   */
  reviewerId: string;

  /**
   * 面谈日期
   */
  interviewDate: string;

  /**
   * 面谈记录
   */
  interviewNotes: string;

  /**
   * 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
   */
  assessmentStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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

