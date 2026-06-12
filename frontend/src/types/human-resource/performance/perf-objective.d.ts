// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perf-objective.d.ts
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
 * 员工绩效目标
 * 对应前端 TaktPerfObjectiveDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 PerfObjective
 * @description 对应后端 TaktPerfObjectiveDto
 */
export interface PerfObjective extends ApprovalDtoBase {
  /**
   * PerfObjectiveID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  perfObjectiveId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId: string;

  /**
   * 方案指标 名称（填充字段）
   */
  schemeMetricName?: string;

  /**
   * 目标周期（如 2026-Q1、2026-Annual）
   */
  objectivePeriod: string;

  /**
   * 目标描述
   */
  objectiveDescription: string;

  /**
   * 目标值
   */
  targetValue: number;

  /**
   * 实际完成值
   */
  actualValue: number;

  /**
   * 完成百分比（%）
   */
  completionPercentage: number;

  /**
   * 目标权重（%）
   */
  objectiveWeight: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 截止日期
   */
  dueDate: string;

  /**
   * 目标达成说明
   */
  achievementNotes: string;

  /**
   * 业务状态（0=待确认 1=进行中 2=已完成）
   */
  objectiveStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * PerfObjective 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PerfObjectiveQuery
 * @description 对应后端 TaktPerfObjectiveQueryDto
 */
export interface PerfObjectiveQuery extends TaktPagedQuery {
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
   * 方案指标 ID
   */
  schemeMetricId?: string;

  /**
   * 目标周期（如 2026-Q1、2026-Annual）
   */
  objectivePeriod?: string;

  /**
   * 目标描述
   */
  objectiveDescription?: string;

  /**
   * 目标值
   */
  targetValue?: number;

  /**
   * 实际完成值
   */
  actualValue?: number;

  /**
   * 完成百分比（%）
   */
  completionPercentage?: number;

  /**
   * 目标权重（%）
   */
  objectiveWeight?: number;

  /**
   * 开始日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 开始日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 截止日期（范围查询-开始）
   */
  dueDateStart?: string;

  /**
   * 截止日期（范围查询-结束）
   */
  dueDateEnd?: string;

  /**
   * 目标达成说明
   */
  achievementNotes?: string;

  /**
   * 业务状态（0=待确认 1=进行中 2=已完成）
   */
  objectiveStatus?: number;

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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建PerfObjective DTO
 * 对应前端 PerfObjectiveCreate
 * @description 对应后端 TaktPerfObjectiveCreateDto
 */
export interface PerfObjectiveCreate {
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
   * 方案指标 ID
   */
  schemeMetricId: string;

  /**
   * 目标周期（如 2026-Q1、2026-Annual）
   */
  objectivePeriod: string;

  /**
   * 目标描述
   */
  objectiveDescription: string;

  /**
   * 目标值
   */
  targetValue: number;

  /**
   * 实际完成值
   */
  actualValue: number;

  /**
   * 完成百分比（%）
   */
  completionPercentage: number;

  /**
   * 目标权重（%）
   */
  objectiveWeight: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 截止日期
   */
  dueDate: string;

  /**
   * 目标达成说明
   */
  achievementNotes: string;

  /**
   * 业务状态（0=待确认 1=进行中 2=已完成）
   */
  objectiveStatus: number;

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
 * 更新PerfObjective DTO
 * 继承 TaktPerfObjectiveCreateDto，添加 PerfObjectiveId 字段
 * 对应前端 PerfObjectiveUpdate
 * @description 对应后端 TaktPerfObjectiveUpdateDto
 */
export interface PerfObjectiveUpdate extends PerfObjectiveCreate {
  /**
   * PerfObjectiveID（标识要更新的实体）
   */
  perfObjectiveId: string;

}


/**
 * PerfObjective 状态更新 DTO
 * 对应前端 PerfObjectiveStatus
 * @description 对应后端 TaktPerfObjectiveStatusDto
 */
export interface PerfObjectiveStatus {
  /**
   * PerfObjectiveID
   */
  perfObjectiveId: string;

  /**
   * 业务状态（0=待确认 1=进行中 2=已完成）
   */
  objectiveStatus: number;

}


/**
 * PerfObjective 导入模板行 DTO
 * 对应前端 PerfObjectiveTemplate
 * @description 对应后端 TaktPerfObjectiveTemplateDto
 */
export interface PerfObjectiveTemplate {
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
   * 方案指标 ID
   */
  schemeMetricId?: string;

  /**
   * 目标周期（如 2026-Q1、2026-Annual）
   */
  objectivePeriod?: string;

  /**
   * 目标描述
   */
  objectiveDescription?: string;

  /**
   * 目标达成说明
   */
  achievementNotes?: string;

  /**
   * 业务状态（0=待确认 1=进行中 2=已完成）
   */
  objectiveStatus?: number;

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
 * PerfObjective 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PerfObjectiveImport
 * @description 对应后端 TaktPerfObjectiveImportDto
 */
export interface PerfObjectiveImport {
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
   * 方案指标 ID
   */
  schemeMetricId?: string;

  /**
   * 目标周期（如 2026-Q1、2026-Annual）
   */
  objectivePeriod?: string;

  /**
   * 目标描述
   */
  objectiveDescription?: string;

  /**
   * 目标达成说明
   */
  achievementNotes?: string;

  /**
   * 业务状态（0=待确认 1=进行中 2=已完成）
   */
  objectiveStatus?: number;

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
 * PerfObjective 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfObjectiveExport
 * @description 对应后端 TaktPerfObjectiveExportDto
 */
export interface PerfObjectiveExport {
  /**
   * PerfObjectiveID
   */
  perfObjectiveId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId: string;

  /**
   * 目标周期（如 2026-Q1、2026-Annual）
   */
  objectivePeriod: string;

  /**
   * 目标描述
   */
  objectiveDescription: string;

  /**
   * 目标值
   */
  targetValue: number;

  /**
   * 实际完成值
   */
  actualValue: number;

  /**
   * 完成百分比（%）
   */
  completionPercentage: number;

  /**
   * 目标权重（%）
   */
  objectiveWeight: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 截止日期
   */
  dueDate: string;

  /**
   * 目标达成说明
   */
  achievementNotes: string;

  /**
   * 业务状态（0=待确认 1=进行中 2=已完成）
   */
  objectiveStatus: number;

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

