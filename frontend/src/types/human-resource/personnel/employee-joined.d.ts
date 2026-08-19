// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-joined.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 员工入职上岗办理记录（审批单，Joined=实际上班；审批态见基类 ApprovalStatus，字典 sys_approval_status）
 * 对应前端 TaktEmployeeJoinedDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EmployeeJoined
 * @description 对应后端 TaktEmployeeJoinedDto
 */
export interface EmployeeJoined extends ApprovalDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
   */
  onboardingId?: string;

  /**
   * 实际上岗日期（JoinedDate：我去上班）
   */
  joinedDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 上岗部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 上岗部门名称
   */
  deptName?: string;

  /**
   * 上岗岗位（选项 TaktPosts/options；DictValue=Id）
   */
  postId?: string;

  /**
   * 上岗岗位名称
   */
  postName?: string;

  /**
   * 职务/职称
   */
  jobTitle?: string;

  /**
   * 工作性质（字典 hr_employee_work_nature_type；0=全职 1=兼职 2=实习 3=外包 4=其他）
   */
  workNature?: number;

  /**
   * 任职类型（字典 hr_employee_employment_type；0=主职 1=兼职 2=借调 3=挂职）
   */
  employmentType?: number;

  /**
   * 直属上级（选项 TaktEmployees/options；DictValue=Id）
   */
  directManagerId?: string;

  /**
   * 直属上级姓名
   */
  directManagerName?: string;

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
 * EmployeeJoined 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeJoinedExport
 * @description 对应后端 TaktEmployeeJoinedExportDto
 */
export interface EmployeeJoinedExport {
  /**
   * EmployeeJoinedID
   */
  employeeJoinedId: string;

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
   */
  onboardingId?: string;

  /**
   * 实际上岗日期（JoinedDate：我去上班）
   */
  joinedDate: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 上岗部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId: string;

  /**
   * 上岗部门名称
   */
  deptName: string;

  /**
   * 上岗岗位（选项 TaktPosts/options；DictValue=Id）
   */
  postId?: string;

  /**
   * 上岗岗位名称
   */
  postName?: string;

  /**
   * 职务/职称
   */
  jobTitle?: string;

  /**
   * 工作性质（字典 hr_employee_work_nature_type；0=全职 1=兼职 2=实习 3=外包 4=其他）
   */
  workNature: number;

  /**
   * 任职类型（字典 hr_employee_employment_type；0=主职 1=兼职 2=借调 3=挂职）
   */
  employmentType: number;

  /**
   * 直属上级（选项 TaktEmployees/options；DictValue=Id）
   */
  directManagerId?: string;

  /**
   * 直属上级姓名
   */
  directManagerName?: string;

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

