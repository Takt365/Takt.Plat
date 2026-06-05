// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-onboarding-todo.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

import type {
  TalentOffer
} from '@/types/human-resource/talent/talent-offer';

/**
 * 入职待办（办理待办单，非审批单；状态见 todo_status）
 * 对应前端 TaktEmployeeOnboardingTodoDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeOnboardingTodo
 * @description 对应后端 TaktEmployeeOnboardingTodoDto
 */
export interface EmployeeOnboardingTodo extends CompanyDtoBase {
  /**
   * EmployeeOnboardingTodoID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeOnboardingTodoId: string;

  /**
   * 录用信息ID（人才管理 TaktTalentOffer）
   */
  offerId: string;

  /**
   * 录用信息名称（填充字段）
   */
  offerName?: string;

  /**
   * 待办单号（租户+公司内业务编号）
   */
  todoNo: string;

  /**
   * 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
   */
  todoStatus: number;

  /**
   * 计划上岗日期（JoinedDate 计划值）
   */
  plannedJoinedDate: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工ID（建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 关联员工名称（填充字段）
   */
  employeeName?: string;

  /**
   * 入职上岗单ID（待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 入职上岗单名称（填充字段）
   */
  employeeJoinedName?: string;

  /**
   * 待办说明
   */
  reason?: string;

  /**
   * 录用信息 （主表：TaktTalentOffer）
   */
  offer?: TalentOffer;

  /**
   * 入职上岗单 （主表：TaktEmployeeJoined）
   */
  employeeJoined?: EmployeeJoined;

}


/**
 * EmployeeOnboardingTodo 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeOnboardingTodoQuery
 * @description 对应后端 TaktEmployeeOnboardingTodoQueryDto
 */
export interface EmployeeOnboardingTodoQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 录用信息ID（人才管理 TaktTalentOffer）
   */
  offerId?: string;

  /**
   * 待办单号（租户+公司内业务编号）
   */
  todoNo?: string;

  /**
   * 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
   */
  todoStatus?: number;

  /**
   * 计划上岗日期（JoinedDate 计划值）（范围查询-开始）
   */
  plannedJoinedDateStart?: string;

  /**
   * 计划上岗日期（JoinedDate 计划值）（范围查询-结束）
   */
  plannedJoinedDateEnd?: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName?: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工ID（建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 入职上岗单ID（待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
   */
  reason?: string;

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
 * 创建EmployeeOnboardingTodo DTO
 * 对应前端 EmployeeOnboardingTodoCreate
 * @description 对应后端 TaktEmployeeOnboardingTodoCreateDto
 */
export interface EmployeeOnboardingTodoCreate {
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
   * 录用信息ID（人才管理 TaktTalentOffer）
   */
  offerId: string;

  /**
   * 待办单号（租户+公司内业务编号）
   */
  todoNo: string;

  /**
   * 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
   */
  todoStatus: number;

  /**
   * 计划上岗日期（JoinedDate 计划值）
   */
  plannedJoinedDate: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工ID（建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 入职上岗单ID（待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
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
 * 更新EmployeeOnboardingTodo DTO
 * 继承 TaktEmployeeOnboardingTodoCreateDto，添加 EmployeeOnboardingTodoId 字段
 * 对应前端 EmployeeOnboardingTodoUpdate
 * @description 对应后端 TaktEmployeeOnboardingTodoUpdateDto
 */
export interface EmployeeOnboardingTodoUpdate extends EmployeeOnboardingTodoCreate {
  /**
   * EmployeeOnboardingTodoID（标识要更新的实体）
   */
  employeeOnboardingTodoId: string;

}


/**
 * EmployeeOnboardingTodo 状态更新 DTO
 * 对应前端 EmployeeOnboardingTodoStatus
 * @description 对应后端 TaktEmployeeOnboardingTodoStatusDto
 */
export interface EmployeeOnboardingTodoStatus {
  /**
   * EmployeeOnboardingTodoID
   */
  employeeOnboardingTodoId: string;

  /**
   * 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
   */
  todoStatus: number;

}


/**
 * EmployeeOnboardingTodo 导入模板行 DTO
 * 对应前端 EmployeeOnboardingTodoTemplate
 * @description 对应后端 TaktEmployeeOnboardingTodoTemplateDto
 */
export interface EmployeeOnboardingTodoTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 录用信息ID（人才管理 TaktTalentOffer）
   */
  offerId?: string;

  /**
   * 待办单号（租户+公司内业务编号）
   */
  todoNo?: string;

  /**
   * 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
   */
  todoStatus?: number;

  /**
   * 候选人姓名（快照）
   */
  candidateName?: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工ID（建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 入职上岗单ID（待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
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
 * EmployeeOnboardingTodo 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeOnboardingTodoImport
 * @description 对应后端 TaktEmployeeOnboardingTodoImportDto
 */
export interface EmployeeOnboardingTodoImport {
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
   * 录用信息ID（人才管理 TaktTalentOffer）
   */
  offerId?: string;

  /**
   * 待办单号（租户+公司内业务编号）
   */
  todoNo?: string;

  /**
   * 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
   */
  todoStatus?: number;

  /**
   * 候选人姓名（快照）
   */
  candidateName?: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工ID（建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 入职上岗单ID（待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
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
 * EmployeeOnboardingTodo 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeOnboardingTodoExport
 * @description 对应后端 TaktEmployeeOnboardingTodoExportDto
 */
export interface EmployeeOnboardingTodoExport {
  /**
   * EmployeeOnboardingTodoID
   */
  employeeOnboardingTodoId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 录用信息ID（人才管理 TaktTalentOffer）
   */
  offerId: string;

  /**
   * 待办单号（租户+公司内业务编号）
   */
  todoNo: string;

  /**
   * 待办状态（0=待办理，1=办理中，2=已完成，3=已取消）
   */
  todoStatus: number;

  /**
   * 计划上岗日期（JoinedDate 计划值）
   */
  plannedJoinedDate: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工ID（建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 入职上岗单ID（待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
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

