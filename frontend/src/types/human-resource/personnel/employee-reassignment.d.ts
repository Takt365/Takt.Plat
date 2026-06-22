// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-reassignment.d.ts
// 创建时间：2026-06-09
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
 * 员工调动记录（审批单，状态见 TaktApprovalEntityBase.ApprovalStatus）
 * 对应前端 TaktEmployeeReassignmentDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EmployeeReassignment
 * @description 对应后端 TaktEmployeeReassignmentDto
 */
export interface EmployeeReassignment extends ApprovalDtoBase {
  /**
   * EmployeeReassignmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeReassignmentId: string;

  /**
   * 员工ID
   */
  employeeId: string;

  /**
   * 员工名称（填充字段）
   */
  employeeName?: string;

  /**
   * 调动类型（0=转岗，1=调岗）
   */
  reassignmentType: number;

  /**
   * 调出部门ID
   */
  fromDeptId: string;

  /**
   * 调出部门名称
   */
  fromDeptName: string;

  /**
   * 调出岗位ID
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门ID
   */
  toDeptId: string;

  /**
   * 调入部门名称
   */
  toDeptName: string;

  /**
   * 调入岗位ID
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

}


/**
 * EmployeeReassignment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeReassignmentQuery
 * @description 对应后端 TaktEmployeeReassignmentQueryDto
 */
export interface EmployeeReassignmentQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 员工ID
   */
  employeeId?: string;

  /**
   * 调动类型（0=转岗，1=调岗）
   */
  reassignmentType?: number;

  /**
   * 调出部门ID
   */
  fromDeptId?: string;

  /**
   * 调出部门名称
   */
  fromDeptName?: string;

  /**
   * 调出岗位ID
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门ID
   */
  toDeptId?: string;

  /**
   * 调入部门名称
   */
  toDeptName?: string;

  /**
   * 调入岗位ID
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 调动原因
   */
  reason?: string;

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
 * 创建EmployeeReassignment DTO
 * 对应前端 EmployeeReassignmentCreate
 * @description 对应后端 TaktEmployeeReassignmentCreateDto
 */
export interface EmployeeReassignmentCreate {
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
   * 员工ID
   */
  employeeId: string;

  /**
   * 调动类型（0=转岗，1=调岗）
   */
  reassignmentType: number;

  /**
   * 调出部门ID
   */
  fromDeptId: string;

  /**
   * 调出部门名称
   */
  fromDeptName: string;

  /**
   * 调出岗位ID
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门ID
   */
  toDeptId: string;

  /**
   * 调入部门名称
   */
  toDeptName: string;

  /**
   * 调入岗位ID
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
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新EmployeeReassignment DTO
 * 继承 TaktEmployeeReassignmentCreateDto，添加 EmployeeReassignmentId 字段
 * 对应前端 EmployeeReassignmentUpdate
 * @description 对应后端 TaktEmployeeReassignmentUpdateDto
 */
export interface EmployeeReassignmentUpdate extends EmployeeReassignmentCreate {
  /**
   * EmployeeReassignmentID（标识要更新的实体）
   */
  employeeReassignmentId: string;

}


/**
 * EmployeeReassignment 导入模板行 DTO
 * 对应前端 EmployeeReassignmentTemplate
 * @description 对应后端 TaktEmployeeReassignmentTemplateDto
 */
export interface EmployeeReassignmentTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 员工ID
   */
  employeeId?: string;

  /**
   * 调动类型（0=转岗，1=调岗）
   */
  reassignmentType?: number;

  /**
   * 调出部门ID
   */
  fromDeptId?: string;

  /**
   * 调出部门名称
   */
  fromDeptName?: string;

  /**
   * 调出岗位ID
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门ID
   */
  toDeptId?: string;

  /**
   * 调入部门名称
   */
  toDeptName?: string;

  /**
   * 调入岗位ID
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 调动原因
   */
  reason?: string;

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
 * EmployeeReassignment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeReassignmentImport
 * @description 对应后端 TaktEmployeeReassignmentImportDto
 */
export interface EmployeeReassignmentImport {
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
   * 员工ID
   */
  employeeId?: string;

  /**
   * 调动类型（0=转岗，1=调岗）
   */
  reassignmentType?: number;

  /**
   * 调出部门ID
   */
  fromDeptId?: string;

  /**
   * 调出部门名称
   */
  fromDeptName?: string;

  /**
   * 调出岗位ID
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门ID
   */
  toDeptId?: string;

  /**
   * 调入部门名称
   */
  toDeptName?: string;

  /**
   * 调入岗位ID
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 调动原因
   */
  reason?: string;

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
   * 员工ID
   */
  employeeId: string;

  /**
   * 调动类型（0=转岗，1=调岗）
   */
  reassignmentType: number;

  /**
   * 调出部门ID
   */
  fromDeptId: string;

  /**
   * 调出部门名称
   */
  fromDeptName: string;

  /**
   * 调出岗位ID
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门ID
   */
  toDeptId: string;

  /**
   * 调入部门名称
   */
  toDeptName: string;

  /**
   * 调入岗位ID
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

