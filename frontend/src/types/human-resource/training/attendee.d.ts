// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/training
// 文件名称：attendee.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/training 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 员工培训结果记录
 * 对应前端 TaktTrainingAttendeeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TrainingAttendee
 * @description 对应后端 TaktTrainingAttendeeDto
 */
export interface TrainingAttendee extends CompanyDtoBase {
  /**
   * TrainingAttendeeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  trainingAttendeeId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 培训课程 ID
   */
  trainingCourseId: string;

  /**
   * 培训课程 名称（填充字段）
   */
  trainingCourseName?: string;

  /**
   * 培训课程名称
   */
  courseName: string;

  /**
   * 培训类型
   */
  trainingType: string;

  /**
   * 培训讲师
   */
  instructor: string;

  /**
   * 培训开始日期
   */
  trainingStartDate: string;

  /**
   * 培训结束日期
   */
  trainingEndDate: string;

  /**
   * 培训日期
   */
  trainingDate: string;

  /**
   * 培训时长（小时）
   */
  trainingHours: number;

  /**
   * 培训成绩
   */
  trainingScore: number;

  /**
   * 是否通过（0=否 1=是）
   */
  isPassed: number;

  /**
   * 证书编码
   */
  CertificateCode: string;

  /**
   * 培训评价
   */
  trainingEvaluation: string;

  /**
   * 状态（1=有效 0=无效）
   */
  trainingResultStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * TrainingAttendee 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TrainingAttendeeQuery
 * @description 对应后端 TaktTrainingAttendeeQueryDto
 */
export interface TrainingAttendeeQuery extends TaktPagedQuery {
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
   * 培训课程 ID
   */
  trainingCourseId?: string;

  /**
   * 培训课程名称
   */
  courseName?: string;

  /**
   * 培训类型
   */
  trainingType?: string;

  /**
   * 培训讲师
   */
  instructor?: string;

  /**
   * 培训开始日期（范围查询-开始）
   */
  trainingStartDateStart?: string;

  /**
   * 培训开始日期（范围查询-结束）
   */
  trainingStartDateEnd?: string;

  /**
   * 培训结束日期（范围查询-开始）
   */
  trainingEndDateStart?: string;

  /**
   * 培训结束日期（范围查询-结束）
   */
  trainingEndDateEnd?: string;

  /**
   * 培训日期（范围查询-开始）
   */
  trainingDateStart?: string;

  /**
   * 培训日期（范围查询-结束）
   */
  trainingDateEnd?: string;

  /**
   * 培训时长（小时）
   */
  trainingHours?: number;

  /**
   * 培训成绩
   */
  trainingScore?: number;

  /**
   * 是否通过（0=否 1=是）
   */
  isPassed?: number;

  /**
   * 证书编码
   */
  CertificateCode?: string;

  /**
   * 培训评价
   */
  trainingEvaluation?: string;

  /**
   * 状态（1=有效 0=无效）
   */
  trainingResultStatus?: number;

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
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建TrainingAttendee DTO
 * 对应前端 TrainingAttendeeCreate
 * @description 对应后端 TaktTrainingAttendeeCreateDto
 */
export interface TrainingAttendeeCreate {
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
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 培训课程 ID
   */
  trainingCourseId: string;

  /**
   * 培训课程名称
   */
  courseName: string;

  /**
   * 培训类型
   */
  trainingType: string;

  /**
   * 培训讲师
   */
  instructor: string;

  /**
   * 培训开始日期
   */
  trainingStartDate: string;

  /**
   * 培训结束日期
   */
  trainingEndDate: string;

  /**
   * 培训日期
   */
  trainingDate: string;

  /**
   * 培训时长（小时）
   */
  trainingHours: number;

  /**
   * 培训成绩
   */
  trainingScore: number;

  /**
   * 是否通过（0=否 1=是）
   */
  isPassed: number;

  /**
   * 证书编码
   */
  CertificateCode: string;

  /**
   * 培训评价
   */
  trainingEvaluation: string;

  /**
   * 状态（1=有效 0=无效）
   */
  trainingResultStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 更新TrainingAttendee DTO
 * 继承 TaktTrainingAttendeeCreateDto，添加 TrainingAttendeeId 字段
 * 对应前端 TrainingAttendeeUpdate
 * @description 对应后端 TaktTrainingAttendeeUpdateDto
 */
export interface TrainingAttendeeUpdate extends TrainingAttendeeCreate {
  /**
   * TrainingAttendeeID（标识要更新的实体）
   */
  trainingAttendeeId: string;

}


/**
 * TrainingAttendee 状态更新 DTO
 * 对应前端 TrainingAttendeeStatus
 * @description 对应后端 TaktTrainingAttendeeStatusDto
 */
export interface TrainingAttendeeStatus {
  /**
   * TrainingAttendeeID
   */
  trainingAttendeeId: string;

  /**
   * 状态（1=有效 0=无效）
   */
  trainingResultStatus: number;

}


/**
 * TrainingAttendee 导入模板行 DTO
 * 对应前端 TrainingAttendeeTemplate
 * @description 对应后端 TaktTrainingAttendeeTemplateDto
 */
export interface TrainingAttendeeTemplate {
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
   * 培训课程 ID
   */
  trainingCourseId?: string;

  /**
   * 培训课程名称
   */
  courseName?: string;

  /**
   * 培训类型
   */
  trainingType?: string;

  /**
   * 培训讲师
   */
  instructor?: string;

  /**
   * 培训开始日期
   */
  trainingStartDate?: string;

  /**
   * 培训结束日期
   */
  trainingEndDate?: string;

  /**
   * 培训日期
   */
  trainingDate?: string;

  /**
   * 培训时长（小时）
   */
  trainingHours?: number;

  /**
   * 培训成绩
   */
  trainingScore?: number;

  /**
   * 是否通过（0=否 1=是）
   */
  isPassed?: number;

  /**
   * 证书编码
   */
  CertificateCode?: string;

  /**
   * 培训评价
   */
  trainingEvaluation?: string;

  /**
   * 状态（1=有效 0=无效）
   */
  trainingResultStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * TrainingAttendee 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TrainingAttendeeImport
 * @description 对应后端 TaktTrainingAttendeeImportDto
 */
export interface TrainingAttendeeImport {
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
   * 员工 ID
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 培训课程 ID
   */
  trainingCourseId?: string;

  /**
   * 培训课程名称
   */
  courseName?: string;

  /**
   * 培训类型
   */
  trainingType?: string;

  /**
   * 培训讲师
   */
  instructor?: string;

  /**
   * 培训开始日期
   */
  trainingStartDate?: string;

  /**
   * 培训结束日期
   */
  trainingEndDate?: string;

  /**
   * 培训日期
   */
  trainingDate?: string;

  /**
   * 培训时长（小时）
   */
  trainingHours?: number;

  /**
   * 培训成绩
   */
  trainingScore?: number;

  /**
   * 是否通过（0=否 1=是）
   */
  isPassed?: number;

  /**
   * 证书编码
   */
  CertificateCode?: string;

  /**
   * 培训评价
   */
  trainingEvaluation?: string;

  /**
   * 状态（1=有效 0=无效）
   */
  trainingResultStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * TrainingAttendee 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TrainingAttendeeExport
 * @description 对应后端 TaktTrainingAttendeeExportDto
 */
export interface TrainingAttendeeExport {
  /**
   * TrainingAttendeeID
   */
  trainingAttendeeId: string;

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
   * 培训课程 ID
   */
  trainingCourseId: string;

  /**
   * 培训课程名称
   */
  courseName: string;

  /**
   * 培训类型
   */
  trainingType: string;

  /**
   * 培训讲师
   */
  instructor: string;

  /**
   * 培训开始日期
   */
  trainingStartDate: string;

  /**
   * 培训结束日期
   */
  trainingEndDate: string;

  /**
   * 培训日期
   */
  trainingDate: string;

  /**
   * 培训时长（小时）
   */
  trainingHours: number;

  /**
   * 培训成绩
   */
  trainingScore: number;

  /**
   * 是否通过（0=否 1=是）
   */
  isPassed: number;

  /**
   * 证书编码
   */
  CertificateCode: string;

  /**
   * 培训评价
   */
  trainingEvaluation: string;

  /**
   * 状态（1=有效 0=无效）
   */
  trainingResultStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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

