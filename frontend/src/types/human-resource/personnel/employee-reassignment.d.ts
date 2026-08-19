// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-reassignment.d.ts
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
 * 员工调动记录（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
 * 对应前端 TaktEmployeeReassignmentDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EmployeeReassignment
 * @description 对应后端 TaktEmployeeReassignmentDto
 */
export interface EmployeeReassignment extends ApprovalDtoBase {
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
   * 调动类型（字典 hr_reassignment_type；0=转岗 1=调岗）
   */
  reassignmentType?: number;

  /**
   * 调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  fromDeptId?: string;

  /**
   * 调出部门名称
   */
  fromDeptName?: string;

  /**
   * 调出岗位（选项 TaktPosts/options；DictValue=Id）
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  toDeptId?: string;

  /**
   * 调入部门名称
   */
  toDeptName?: string;

  /**
   * 调入岗位（选项 TaktPosts/options；DictValue=Id）
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 调动原因
   */
  reason?: string;

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
 * EmployeeReassignment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeReassignmentExport
 * @description 对应后端 TaktEmployeeReassignmentExportDto
 */
export interface EmployeeReassignmentExport {
  /**
   * EmployeeReassignmentID
   */
  employeeReassignmentId: string;

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
   * 调动类型（字典 hr_reassignment_type；0=转岗 1=调岗）
   */
  reassignmentType: number;

  /**
   * 调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  fromDeptId: string;

  /**
   * 调出部门名称
   */
  fromDeptName: string;

  /**
   * 调出岗位（选项 TaktPosts/options；DictValue=Id）
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  toDeptId: string;

  /**
   * 调入部门名称
   */
  toDeptName: string;

  /**
   * 调入岗位（选项 TaktPosts/options；DictValue=Id）
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 调动原因
   */
  reason?: string;

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

