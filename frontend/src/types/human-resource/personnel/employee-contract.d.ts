// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-contract.d.ts
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
 * 员工劳动合同
 * 对应前端 TaktEmployeeContractDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeContract
 * @description 对应后端 TaktEmployeeContractDto
 */
export interface EmployeeContract extends CompanyDtoBase {
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
   * 合同编码
   */
  contractCode?: string;

  /**
   * 合同类型（字典 hr_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
   */
  contractType?: number;

  /**
   * 合同开始日期
   */
  startDate?: string;

  /**
   * 合同结束日期
   */
  endDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 签约单位
   */
  signCompany?: string;

  /**
   * 合同状态（字典 hr_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
   */
  contractStatus?: number;

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
 * EmployeeContract 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeContractExport
 * @description 对应后端 TaktEmployeeContractExportDto
 */
export interface EmployeeContractExport {
  /**
   * EmployeeContractID
   */
  employeeContractId: string;

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
   * 合同编码
   */
  contractCode: string;

  /**
   * 合同类型（字典 hr_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
   */
  contractType: number;

  /**
   * 合同开始日期
   */
  startDate: string;

  /**
   * 合同结束日期
   */
  endDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 签约单位
   */
  signCompany?: string;

  /**
   * 合同状态（字典 hr_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
   */
  contractStatus: number;

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

