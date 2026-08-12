// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-experience.d.ts
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
 * 员工外部工作经历
 * 对应前端 TaktEmployeeExperienceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeExperience
 * @description 对应后端 TaktEmployeeExperienceDto
 */
export interface EmployeeExperience extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
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
   * 工作单位名称
   */
  companyName?: string;

  /**
   * 职位名称
   */
  positionName?: string;

  /**
   * 工作内容
   */
  jobContent?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 证明人姓名
   */
  witnessName?: string;

  /**
   * 证明人电话
   */
  witnessPhone?: string;

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
 * EmployeeExperience 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeExperienceExport
 * @description 对应后端 TaktEmployeeExperienceExportDto
 */
export interface EmployeeExperienceExport {
  /**
   * EmployeeExperienceID
   */
  employeeExperienceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

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
   * 工作单位名称
   */
  companyName: string;

  /**
   * 职位名称
   */
  positionName?: string;

  /**
   * 工作内容
   */
  jobContent?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 证明人姓名
   */
  witnessName?: string;

  /**
   * 证明人电话
   */
  witnessPhone?: string;

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

