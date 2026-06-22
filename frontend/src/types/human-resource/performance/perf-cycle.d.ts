// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perf-cycle.d.ts
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
 * 绩效考核周期日程安排
 * 对应前端 TaktPerfCycleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PerfCycle
 * @description 对应后端 TaktPerfCycleDto
 */
export interface PerfCycle extends CompanyDtoBase {
  /**
   * PerfCycleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  perfCycleId: string;

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
 * PerfCycle 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PerfCycleQuery
 * @description 对应后端 TaktPerfCycleQueryDto
 */
export interface PerfCycleQuery extends TaktPagedQuery {
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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建PerfCycle DTO
 * 对应前端 PerfCycleCreate
 * @description 对应后端 TaktPerfCycleCreateDto
 */
export interface PerfCycleCreate {
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
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新PerfCycle DTO
 * 继承 TaktPerfCycleCreateDto，添加 PerfCycleId 字段
 * 对应前端 PerfCycleUpdate
 * @description 对应后端 TaktPerfCycleUpdateDto
 */
export interface PerfCycleUpdate extends PerfCycleCreate {
  /**
   * PerfCycleID（标识要更新的实体）
   */
  perfCycleId: string;

}


/**
 * PerfCycle 状态更新 DTO
 * 对应前端 PerfCycleStatus
 * @description 对应后端 TaktPerfCycleStatusDto
 */
export interface PerfCycleStatus {
  /**
   * PerfCycleID
   */
  perfCycleId: string;

  /**
   * 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
   */
  cycleScheduleStatus: number;

}


/**
 * PerfCycle 导入模板行 DTO
 * 对应前端 PerfCycleTemplate
 * @description 对应后端 TaktPerfCycleTemplateDto
 */
export interface PerfCycleTemplate {
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
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PerfCycle 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PerfCycleImport
 * @description 对应后端 TaktPerfCycleImportDto
 */
export interface PerfCycleImport {
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
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PerfCycle 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfCycleExport
 * @description 对应后端 TaktPerfCycleExportDto
 */
export interface PerfCycleExport {
  /**
   * PerfCycleID
   */
  perfCycleId: string;

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

