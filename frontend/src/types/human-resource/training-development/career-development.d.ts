// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/training-development
// 文件名称：career-development.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/training-development 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 员工职业发展规划与技能评估
 * 对应前端 TaktCareerDevelopmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CareerDevelopment
 * @description 对应后端 TaktCareerDevelopmentDto
 */
export interface CareerDevelopment extends CompanyDtoBase {
  /**
   * CareerDevelopmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  careerDevelopmentId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 技能类别
   */
  skillCategory: string;

  /**
   * 技能名称
   */
  skillName: string;

  /**
   * 评估日期
   */
  assessmentDate: string;

  /**
   * 评估方式
   */
  assessmentMethod: string;

  /**
   * 评估得分
   */
  assessmentScore: number;

  /**
   * 技能等级
   */
  skillLevel: string;

  /**
   * 目标岗位
   */
  targetPosition: string;

  /**
   * 发展计划
   */
  developmentPlan: string;

  /**
   * 改进建议
   */
  improvementSuggestions: string;

  /**
   * 下次评估日期
   */
  nextAssessmentDate: string;

  /**
   * 状态（1=进行中 0=已归档）
   */
  careerDevelopmentStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * CareerDevelopment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CareerDevelopmentQuery
 * @description 对应后端 TaktCareerDevelopmentQueryDto
 */
export interface CareerDevelopmentQuery extends TaktPagedQuery {
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
   * 技能类别
   */
  skillCategory?: string;

  /**
   * 技能名称
   */
  skillName?: string;

  /**
   * 评估日期（范围查询-开始）
   */
  assessmentDateStart?: string;

  /**
   * 评估日期（范围查询-结束）
   */
  assessmentDateEnd?: string;

  /**
   * 评估方式
   */
  assessmentMethod?: string;

  /**
   * 评估得分
   */
  assessmentScore?: number;

  /**
   * 技能等级
   */
  skillLevel?: string;

  /**
   * 目标岗位
   */
  targetPosition?: string;

  /**
   * 发展计划
   */
  developmentPlan?: string;

  /**
   * 改进建议
   */
  improvementSuggestions?: string;

  /**
   * 下次评估日期（范围查询-开始）
   */
  nextAssessmentDateStart?: string;

  /**
   * 下次评估日期（范围查询-结束）
   */
  nextAssessmentDateEnd?: string;

  /**
   * 状态（1=进行中 0=已归档）
   */
  careerDevelopmentStatus?: number;

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
 * 创建CareerDevelopment DTO
 * 对应前端 CareerDevelopmentCreate
 * @description 对应后端 TaktCareerDevelopmentCreateDto
 */
export interface CareerDevelopmentCreate {
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
   * 技能类别
   */
  skillCategory: string;

  /**
   * 技能名称
   */
  skillName: string;

  /**
   * 评估日期
   */
  assessmentDate: string;

  /**
   * 评估方式
   */
  assessmentMethod: string;

  /**
   * 评估得分
   */
  assessmentScore: number;

  /**
   * 技能等级
   */
  skillLevel: string;

  /**
   * 目标岗位
   */
  targetPosition: string;

  /**
   * 发展计划
   */
  developmentPlan: string;

  /**
   * 改进建议
   */
  improvementSuggestions: string;

  /**
   * 下次评估日期
   */
  nextAssessmentDate: string;

  /**
   * 状态（1=进行中 0=已归档）
   */
  careerDevelopmentStatus: number;

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
 * 更新CareerDevelopment DTO
 * 继承 TaktCareerDevelopmentCreateDto，添加 CareerDevelopmentId 字段
 * 对应前端 CareerDevelopmentUpdate
 * @description 对应后端 TaktCareerDevelopmentUpdateDto
 */
export interface CareerDevelopmentUpdate extends CareerDevelopmentCreate {
  /**
   * CareerDevelopmentID（标识要更新的实体）
   */
  careerDevelopmentId: string;

}


/**
 * CareerDevelopment 状态更新 DTO
 * 对应前端 CareerDevelopmentStatus
 * @description 对应后端 TaktCareerDevelopmentStatusDto
 */
export interface CareerDevelopmentStatus {
  /**
   * CareerDevelopmentID
   */
  careerDevelopmentId: string;

  /**
   * 状态（1=进行中 0=已归档）
   */
  careerDevelopmentStatus: number;

}


/**
 * CareerDevelopment 导入模板行 DTO
 * 对应前端 CareerDevelopmentTemplate
 * @description 对应后端 TaktCareerDevelopmentTemplateDto
 */
export interface CareerDevelopmentTemplate {
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
   * 技能类别
   */
  skillCategory?: string;

  /**
   * 技能名称
   */
  skillName?: string;

  /**
   * 评估方式
   */
  assessmentMethod?: string;

  /**
   * 技能等级
   */
  skillLevel?: string;

  /**
   * 目标岗位
   */
  targetPosition?: string;

  /**
   * 发展计划
   */
  developmentPlan?: string;

  /**
   * 改进建议
   */
  improvementSuggestions?: string;

  /**
   * 状态（1=进行中 0=已归档）
   */
  careerDevelopmentStatus?: number;

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
 * CareerDevelopment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CareerDevelopmentImport
 * @description 对应后端 TaktCareerDevelopmentImportDto
 */
export interface CareerDevelopmentImport {
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
   * 技能类别
   */
  skillCategory?: string;

  /**
   * 技能名称
   */
  skillName?: string;

  /**
   * 评估方式
   */
  assessmentMethod?: string;

  /**
   * 技能等级
   */
  skillLevel?: string;

  /**
   * 目标岗位
   */
  targetPosition?: string;

  /**
   * 发展计划
   */
  developmentPlan?: string;

  /**
   * 改进建议
   */
  improvementSuggestions?: string;

  /**
   * 状态（1=进行中 0=已归档）
   */
  careerDevelopmentStatus?: number;

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
 * CareerDevelopment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CareerDevelopmentExport
 * @description 对应后端 TaktCareerDevelopmentExportDto
 */
export interface CareerDevelopmentExport {
  /**
   * CareerDevelopmentID
   */
  careerDevelopmentId: string;

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
   * 技能类别
   */
  skillCategory: string;

  /**
   * 技能名称
   */
  skillName: string;

  /**
   * 评估日期
   */
  assessmentDate: string;

  /**
   * 评估方式
   */
  assessmentMethod: string;

  /**
   * 评估得分
   */
  assessmentScore: number;

  /**
   * 技能等级
   */
  skillLevel: string;

  /**
   * 目标岗位
   */
  targetPosition: string;

  /**
   * 发展计划
   */
  developmentPlan: string;

  /**
   * 改进建议
   */
  improvementSuggestions: string;

  /**
   * 下次评估日期
   */
  nextAssessmentDate: string;

  /**
   * 状态（1=进行中 0=已归档）
   */
  careerDevelopmentStatus: number;

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

