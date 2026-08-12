// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-address.d.ts
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
 * 员工地址（主档子表；同一员工每种地址类型至多一条）
 * 对应前端 TaktEmployeeAddressDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeAddress
 * @description 对应后端 TaktEmployeeAddressDto
 */
export interface EmployeeAddress extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
   * 地址类型（字典 hr_employee_address_type；1=家庭 2=工作 3=常住）
   */
  addressType?: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  country?: string;

  /**
   * 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  city?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

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
 * EmployeeAddress 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeAddressExport
 * @description 对应后端 TaktEmployeeAddressExportDto
 */
export interface EmployeeAddressExport {
  /**
   * EmployeeAddressID
   */
  employeeAddressId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
   * 地址类型（字典 hr_employee_address_type；1=家庭 2=工作 3=常住）
   */
  addressType: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  country: string;

  /**
   * 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province: string;

  /**
   * 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  city: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district: string;

  /**
   * 地址1（详细地址行1）
   */
  address1: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

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

