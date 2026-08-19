// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-resignation.d.ts
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
 * 员工离职办理记录（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
 * 对应前端 TaktEmployeeResignationDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EmployeeResignation
 * @description 对应后端 TaktEmployeeResignationDto
 */
export interface EmployeeResignation extends ApprovalDtoBase {
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
   * 离职类型（字典 hr_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
   */
  resignationType?: number;

  /**
   * 申请日期
   */
  applyDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 实际离职日期
   */
  terminationDate?: string;

  /**
   * 离职原因
   */
  reason?: string;

  /**
   * 工作交接说明
   */
  handoverNotes?: string;

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
 * EmployeeResignation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeResignationExport
 * @description 对应后端 TaktEmployeeResignationExportDto
 */
export interface EmployeeResignationExport {
  /**
   * EmployeeResignationID
   */
  employeeResignationId: string;

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
   * 离职类型（字典 hr_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
   */
  resignationType: number;

  /**
   * 申请日期
   */
  applyDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 实际离职日期
   */
  terminationDate?: string;

  /**
   * 离职原因
   */
  reason?: string;

  /**
   * 工作交接说明
   */
  handoverNotes?: string;

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

