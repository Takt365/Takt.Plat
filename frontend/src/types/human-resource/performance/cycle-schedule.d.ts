// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：cycle-schedule.d.ts
// 创建时间：2026-06-06
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
 * 绩效考核周期日程安排
 * 对应前端 TaktCycleScheduleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CycleSchedule
 * @description 对应后端 TaktCycleScheduleDto
 */
export interface CycleSchedule extends CompanyDtoBase {
  /**
   * CycleScheduleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  cycleScheduleId: string;

  /**
   * 周期编码（租户+公司内唯一）
   */
  cycleCode: string;

  /**
   * 周期名称
   */
  cycleName: string;

  /**
   * 周期类型（月度/季度/半年度/年度）
   */
  cycleType: string;

  /**
   * 周期年度
   */
  cycleYear: number;

  /**
   * 周期序号
   */
  cycleSequence: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 目标设定截止日期
   */
  goalSettingDueDate: string;

  /**
   * 自评截止日期
   */
  selfEvaluationDueDate: string;

  /**
   * 主管评审截止日期
   */
  supervisorReviewDueDate: string;

  /**
   * 面谈截止日期
   */
  interviewDueDate: string;

  /**
   * 结果确认截止日期
   */
  resultConfirmationDueDate: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 周期说明
   */
  description: string;

  /**
   * 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
   */
  cycleScheduleStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * CycleSchedule 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CycleScheduleQuery
 * @description 对应后端 TaktCycleScheduleQueryDto
 */
export interface CycleScheduleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 周期编码（租户+公司内唯一）
   */
  cycleCode?: string;

  /**
   * 周期名称
   */
  cycleName?: string;

  /**
   * 周期类型（月度/季度/半年度/年度）
   */
  cycleType?: string;

  /**
   * 周期年度
   */
  cycleYear?: number;

  /**
   * 周期序号
   */
  cycleSequence?: number;

  /**
   * 开始日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 开始日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 结束日期（范围查询-开始）
   */
  endDateStart?: string;

  /**
   * 结束日期（范围查询-结束）
   */
  endDateEnd?: string;

  /**
   * 目标设定截止日期（范围查询-开始）
   */
  goalSettingDueDateStart?: string;

  /**
   * 目标设定截止日期（范围查询-结束）
   */
  goalSettingDueDateEnd?: string;

  /**
   * 自评截止日期（范围查询-开始）
   */
  selfEvaluationDueDateStart?: string;

  /**
   * 自评截止日期（范围查询-结束）
   */
  selfEvaluationDueDateEnd?: string;

  /**
   * 主管评审截止日期（范围查询-开始）
   */
  supervisorReviewDueDateStart?: string;

  /**
   * 主管评审截止日期（范围查询-结束）
   */
  supervisorReviewDueDateEnd?: string;

  /**
   * 面谈截止日期（范围查询-开始）
   */
  interviewDueDateStart?: string;

  /**
   * 面谈截止日期（范围查询-结束）
   */
  interviewDueDateEnd?: string;

  /**
   * 结果确认截止日期（范围查询-开始）
   */
  resultConfirmationDueDateStart?: string;

  /**
   * 结果确认截止日期（范围查询-结束）
   */
  resultConfirmationDueDateEnd?: string;

  /**
   * 适用部门
   */
  applicableDepartment?: string;

  /**
   * 周期说明
   */
  description?: string;

  /**
   * 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
   */
  cycleScheduleStatus?: number;

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
 * 创建CycleSchedule DTO
 * 对应前端 CycleScheduleCreate
 * @description 对应后端 TaktCycleScheduleCreateDto
 */
export interface CycleScheduleCreate {
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
   * 周期编码（租户+公司内唯一）
   */
  cycleCode: string;

  /**
   * 周期名称
   */
  cycleName: string;

  /**
   * 周期类型（月度/季度/半年度/年度）
   */
  cycleType: string;

  /**
   * 周期年度
   */
  cycleYear: number;

  /**
   * 周期序号
   */
  cycleSequence: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 目标设定截止日期
   */
  goalSettingDueDate: string;

  /**
   * 自评截止日期
   */
  selfEvaluationDueDate: string;

  /**
   * 主管评审截止日期
   */
  supervisorReviewDueDate: string;

  /**
   * 面谈截止日期
   */
  interviewDueDate: string;

  /**
   * 结果确认截止日期
   */
  resultConfirmationDueDate: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 周期说明
   */
  description: string;

  /**
   * 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
   */
  cycleScheduleStatus: number;

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
 * 更新CycleSchedule DTO
 * 继承 TaktCycleScheduleCreateDto，添加 CycleScheduleId 字段
 * 对应前端 CycleScheduleUpdate
 * @description 对应后端 TaktCycleScheduleUpdateDto
 */
export interface CycleScheduleUpdate extends CycleScheduleCreate {
  /**
   * CycleScheduleID（标识要更新的实体）
   */
  cycleScheduleId: string;

}


/**
 * CycleSchedule 状态更新 DTO
 * 对应前端 CycleScheduleStatus
 * @description 对应后端 TaktCycleScheduleStatusDto
 */
export interface CycleScheduleStatus {
  /**
   * CycleScheduleID
   */
  cycleScheduleId: string;

  /**
   * 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
   */
  cycleScheduleStatus: number;

}


/**
 * CycleSchedule 导入模板行 DTO
 * 对应前端 CycleScheduleTemplate
 * @description 对应后端 TaktCycleScheduleTemplateDto
 */
export interface CycleScheduleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 周期编码（租户+公司内唯一）
   */
  cycleCode?: string;

  /**
   * 周期名称
   */
  cycleName?: string;

  /**
   * 周期类型（月度/季度/半年度/年度）
   */
  cycleType?: string;

  /**
   * 周期年度
   */
  cycleYear?: number;

  /**
   * 周期序号
   */
  cycleSequence?: number;

  /**
   * 适用部门
   */
  applicableDepartment?: string;

  /**
   * 周期说明
   */
  description?: string;

  /**
   * 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
   */
  cycleScheduleStatus?: number;

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
 * CycleSchedule 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CycleScheduleImport
 * @description 对应后端 TaktCycleScheduleImportDto
 */
export interface CycleScheduleImport {
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
   * 周期编码（租户+公司内唯一）
   */
  cycleCode?: string;

  /**
   * 周期名称
   */
  cycleName?: string;

  /**
   * 周期类型（月度/季度/半年度/年度）
   */
  cycleType?: string;

  /**
   * 周期年度
   */
  cycleYear?: number;

  /**
   * 周期序号
   */
  cycleSequence?: number;

  /**
   * 适用部门
   */
  applicableDepartment?: string;

  /**
   * 周期说明
   */
  description?: string;

  /**
   * 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
   */
  cycleScheduleStatus?: number;

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
 * CycleSchedule 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CycleScheduleExport
 * @description 对应后端 TaktCycleScheduleExportDto
 */
export interface CycleScheduleExport {
  /**
   * CycleScheduleID
   */
  cycleScheduleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 周期编码（租户+公司内唯一）
   */
  cycleCode: string;

  /**
   * 周期名称
   */
  cycleName: string;

  /**
   * 周期类型（月度/季度/半年度/年度）
   */
  cycleType: string;

  /**
   * 周期年度
   */
  cycleYear: number;

  /**
   * 周期序号
   */
  cycleSequence: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 目标设定截止日期
   */
  goalSettingDueDate: string;

  /**
   * 自评截止日期
   */
  selfEvaluationDueDate: string;

  /**
   * 主管评审截止日期
   */
  supervisorReviewDueDate: string;

  /**
   * 面谈截止日期
   */
  interviewDueDate: string;

  /**
   * 结果确认截止日期
   */
  resultConfirmationDueDate: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 周期说明
   */
  description: string;

  /**
   * 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
   */
  cycleScheduleStatus: number;

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

