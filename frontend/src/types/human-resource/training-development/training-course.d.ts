// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/training-development
// 文件名称：training-course.d.ts
// 创建时间：2026-06-07
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
 * 培训课程定义
 * 对应前端 TaktTrainingCourseDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TrainingCourse
 * @description 对应后端 TaktTrainingCourseDto
 */
export interface TrainingCourse extends CompanyDtoBase {
  /**
   * TrainingCourseID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  trainingCourseId: string;

  /**
   * 课程编码（租户+公司内唯一）
   */
  courseCode: string;

  /**
   * 课程名称
   */
  courseName: string;

  /**
   * 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
   */
  courseType: string;

  /**
   * 课程级别（初级/中级/高级/专家）
   */
  courseLevel: string;

  /**
   * 课程描述
   */
  courseDescription: string;

  /**
   * 课程目标
   */
  courseObjectives: string;

  /**
   * 培训时长（小时）
   */
  trainingHours: number;

  /**
   * 主讲讲师
   */
  mainInstructor: string;

  /**
   * 培训方式（线下/线上/混合）
   */
  trainingMethod: string;

  /**
   * 考核方式（考试/实操/作业/无）
   */
  assessmentMethod: string;

  /**
   * 及格分数线
   */
  passingScore: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（1=启用 0=禁用）
   */
  trainingCourseStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * TrainingCourse 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TrainingCourseQuery
 * @description 对应后端 TaktTrainingCourseQueryDto
 */
export interface TrainingCourseQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 课程编码（租户+公司内唯一）
   */
  courseCode?: string;

  /**
   * 课程名称
   */
  courseName?: string;

  /**
   * 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
   */
  courseType?: string;

  /**
   * 课程级别（初级/中级/高级/专家）
   */
  courseLevel?: string;

  /**
   * 课程描述
   */
  courseDescription?: string;

  /**
   * 课程目标
   */
  courseObjectives?: string;

  /**
   * 培训时长（小时）
   */
  trainingHours?: number;

  /**
   * 主讲讲师
   */
  mainInstructor?: string;

  /**
   * 培训方式（线下/线上/混合）
   */
  trainingMethod?: string;

  /**
   * 考核方式（考试/实操/作业/无）
   */
  assessmentMethod?: string;

  /**
   * 及格分数线
   */
  passingScore?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（1=启用 0=禁用）
   */
  trainingCourseStatus?: number;

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
 * 创建TrainingCourse DTO
 * 对应前端 TrainingCourseCreate
 * @description 对应后端 TaktTrainingCourseCreateDto
 */
export interface TrainingCourseCreate {
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
   * 课程编码（租户+公司内唯一）
   */
  courseCode: string;

  /**
   * 课程名称
   */
  courseName: string;

  /**
   * 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
   */
  courseType: string;

  /**
   * 课程级别（初级/中级/高级/专家）
   */
  courseLevel: string;

  /**
   * 课程描述
   */
  courseDescription: string;

  /**
   * 课程目标
   */
  courseObjectives: string;

  /**
   * 培训时长（小时）
   */
  trainingHours: number;

  /**
   * 主讲讲师
   */
  mainInstructor: string;

  /**
   * 培训方式（线下/线上/混合）
   */
  trainingMethod: string;

  /**
   * 考核方式（考试/实操/作业/无）
   */
  assessmentMethod: string;

  /**
   * 及格分数线
   */
  passingScore: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（1=启用 0=禁用）
   */
  trainingCourseStatus: number;

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
 * 更新TrainingCourse DTO
 * 继承 TaktTrainingCourseCreateDto，添加 TrainingCourseId 字段
 * 对应前端 TrainingCourseUpdate
 * @description 对应后端 TaktTrainingCourseUpdateDto
 */
export interface TrainingCourseUpdate extends TrainingCourseCreate {
  /**
   * TrainingCourseID（标识要更新的实体）
   */
  trainingCourseId: string;

}


/**
 * TrainingCourse 状态更新 DTO
 * 对应前端 TrainingCourseStatus
 * @description 对应后端 TaktTrainingCourseStatusDto
 */
export interface TrainingCourseStatus {
  /**
   * TrainingCourseID
   */
  trainingCourseId: string;

  /**
   * 状态（1=启用 0=禁用）
   */
  trainingCourseStatus: number;

}


/**
 * TrainingCourse 排序更新 DTO
 * 对应前端 TrainingCourseSort
 * @description 对应后端 TaktTrainingCourseSortDto
 */
export interface TrainingCourseSort {
  /**
   * TrainingCourseID
   */
  trainingCourseId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * TrainingCourse 导入模板行 DTO
 * 对应前端 TrainingCourseTemplate
 * @description 对应后端 TaktTrainingCourseTemplateDto
 */
export interface TrainingCourseTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 课程编码（租户+公司内唯一）
   */
  courseCode?: string;

  /**
   * 课程名称
   */
  courseName?: string;

  /**
   * 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
   */
  courseType?: string;

  /**
   * 课程级别（初级/中级/高级/专家）
   */
  courseLevel?: string;

  /**
   * 课程描述
   */
  courseDescription?: string;

  /**
   * 课程目标
   */
  courseObjectives?: string;

  /**
   * 主讲讲师
   */
  mainInstructor?: string;

  /**
   * 培训方式（线下/线上/混合）
   */
  trainingMethod?: string;

  /**
   * 考核方式（考试/实操/作业/无）
   */
  assessmentMethod?: string;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（1=启用 0=禁用）
   */
  trainingCourseStatus?: number;

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
 * TrainingCourse 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TrainingCourseImport
 * @description 对应后端 TaktTrainingCourseImportDto
 */
export interface TrainingCourseImport {
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
   * 课程编码（租户+公司内唯一）
   */
  courseCode?: string;

  /**
   * 课程名称
   */
  courseName?: string;

  /**
   * 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
   */
  courseType?: string;

  /**
   * 课程级别（初级/中级/高级/专家）
   */
  courseLevel?: string;

  /**
   * 课程描述
   */
  courseDescription?: string;

  /**
   * 课程目标
   */
  courseObjectives?: string;

  /**
   * 主讲讲师
   */
  mainInstructor?: string;

  /**
   * 培训方式（线下/线上/混合）
   */
  trainingMethod?: string;

  /**
   * 考核方式（考试/实操/作业/无）
   */
  assessmentMethod?: string;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（1=启用 0=禁用）
   */
  trainingCourseStatus?: number;

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
 * TrainingCourse 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TrainingCourseExport
 * @description 对应后端 TaktTrainingCourseExportDto
 */
export interface TrainingCourseExport {
  /**
   * TrainingCourseID
   */
  trainingCourseId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 课程编码（租户+公司内唯一）
   */
  courseCode: string;

  /**
   * 课程名称
   */
  courseName: string;

  /**
   * 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
   */
  courseType: string;

  /**
   * 课程级别（初级/中级/高级/专家）
   */
  courseLevel: string;

  /**
   * 课程描述
   */
  courseDescription: string;

  /**
   * 课程目标
   */
  courseObjectives: string;

  /**
   * 培训时长（小时）
   */
  trainingHours: number;

  /**
   * 主讲讲师
   */
  mainInstructor: string;

  /**
   * 培训方式（线下/线上/混合）
   */
  trainingMethod: string;

  /**
   * 考核方式（考试/实操/作业/无）
   */
  assessmentMethod: string;

  /**
   * 及格分数线
   */
  passingScore: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（1=启用 0=禁用）
   */
  trainingCourseStatus: number;

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

