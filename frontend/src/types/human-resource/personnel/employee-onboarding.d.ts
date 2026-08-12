// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-onboarding.d.ts
// 创建时间：2026-07-23
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

/**
 * 入职待办（办理待办单，非审批单；状态见 TodoStatus）
 * 对应前端 TaktEmployeeOnboardingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeOnboarding
 * @description 对应后端 TaktEmployeeOnboardingDto
 */
export interface EmployeeOnboarding extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
   */
  offerId?: string;

  /**
   * 待办单号（租户+公司内业务编码）
   */
  todoCode?: string;

  /**
   * 计划上岗日期（JoinedDate 计划值）
   */
  plannedJoinedDate?: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName?: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
   */
  employeeName?: string;

  /**
   * 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
   */
  reason?: string;

  /**
   * 待办状态（字典 hr_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
   */
  todoStatus?: number;

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
 * EmployeeOnboarding 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeOnboardingExport
 * @description 对应后端 TaktEmployeeOnboardingExportDto
 */
export interface EmployeeOnboardingExport {
  /**
   * EmployeeOnboardingID
   */
  employeeOnboardingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
   */
  offerId: string;

  /**
   * 待办单号（租户+公司内业务编码）
   */
  todoCode: string;

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
   * 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
   */
  employeeName?: string;

  /**
   * 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
   */
  reason?: string;

  /**
   * 待办状态（字典 hr_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
   */
  todoStatus: number;

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

