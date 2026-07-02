// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：payslip.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 员工工资条（发薪结果单据，区别于 TaktEmpSalary 定薪档案）
 * 对应前端 TaktPayslipDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Payslip
 * @description 对应后端 TaktPayslipDto
 */
export interface Payslip extends CompanyDtoBase {
  /**
   * PayslipID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  payslipId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 发薪期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 基本工资（元）
   */
  baseSalary: number;

  /**
   * 岗位工资（元）
   */
  positionSalary: number;

  /**
   * 绩效/奖金（元）
   */
  bonusAmount: number;

  /**
   * 加班费（元）
   */
  overtimePay: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal: number;

  /**
   * 应发合计（元）
   */
  grossAmount: number;

  /**
   * 社保扣款（元）
   */
  socialSecurityDeduction: number;

  /**
   * 公积金扣款（元）
   */
  housingFundDeduction: number;

  /**
   * 个税扣款（元）
   */
  taxDeduction: number;

  /**
   * 其他扣款（元）
   */
  otherDeduction: number;

  /**
   * 实发金额（元）
   */
  netAmount: number;

  /**
   * 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
   */
  formulaSetCode?: string;

  /**
   * 发放状态（字典 hr_payslip_issue_status：0=待发放 1=已发放 2=已确认）
   */
  issueStatus: number;

  /**
   * 发放日期
   */
  issueDate?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * Payslip 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PayslipQuery
 * @description 对应后端 TaktPayslipQueryDto
 */
export interface PayslipQuery extends TaktPagedQuery {
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
   * 发薪期间（如 2026-06）
   */
  payPeriod?: string;

  /**
   * 基本工资（元）
   */
  baseSalary?: number;

  /**
   * 岗位工资（元）
   */
  positionSalary?: number;

  /**
   * 绩效/奖金（元）
   */
  bonusAmount?: number;

  /**
   * 加班费（元）
   */
  overtimePay?: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal?: number;

  /**
   * 应发合计（元）
   */
  grossAmount?: number;

  /**
   * 社保扣款（元）
   */
  socialSecurityDeduction?: number;

  /**
   * 公积金扣款（元）
   */
  housingFundDeduction?: number;

  /**
   * 个税扣款（元）
   */
  taxDeduction?: number;

  /**
   * 其他扣款（元）
   */
  otherDeduction?: number;

  /**
   * 实发金额（元）
   */
  netAmount?: number;

  /**
   * 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
   */
  formulaSetCode?: string;

  /**
   * 发放状态（字典 hr_payslip_issue_status：0=待发放 1=已发放 2=已确认）
   */
  issueStatus?: number;

  /**
   * 发放日期（范围查询-开始）
   */
  issueDateStart?: string;

  /**
   * 发放日期（范围查询-结束）
   */
  issueDateEnd?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Payslip DTO
 * 对应前端 PayslipCreate
 * @description 对应后端 TaktPayslipCreateDto
 */
export interface PayslipCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
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
   * 发薪期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 基本工资（元）
   */
  baseSalary: number;

  /**
   * 岗位工资（元）
   */
  positionSalary: number;

  /**
   * 绩效/奖金（元）
   */
  bonusAmount: number;

  /**
   * 加班费（元）
   */
  overtimePay: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal: number;

  /**
   * 应发合计（元）
   */
  grossAmount: number;

  /**
   * 社保扣款（元）
   */
  socialSecurityDeduction: number;

  /**
   * 公积金扣款（元）
   */
  housingFundDeduction: number;

  /**
   * 个税扣款（元）
   */
  taxDeduction: number;

  /**
   * 其他扣款（元）
   */
  otherDeduction: number;

  /**
   * 实发金额（元）
   */
  netAmount: number;

  /**
   * 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
   */
  formulaSetCode?: string;

  /**
   * 发放状态（字典 hr_payslip_issue_status：0=待发放 1=已发放 2=已确认）
   */
  issueStatus: number;

  /**
   * 发放日期
   */
  issueDate?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 更新Payslip DTO
 * 继承 TaktPayslipCreateDto，添加 PayslipId 字段
 * 对应前端 PayslipUpdate
 * @description 对应后端 TaktPayslipUpdateDto
 */
export interface PayslipUpdate extends PayslipCreate {
  /**
   * PayslipID（标识要更新的实体）
   */
  payslipId: string;

}


/**
 * Payslip 状态更新 DTO
 * 对应前端 PayslipStatus
 * @description 对应后端 TaktPayslipStatusDto
 */
export interface PayslipStatus {
  /**
   * PayslipID
   */
  payslipId: string;

  /**
   * 发放状态（字典 hr_payslip_issue_status：0=待发放 1=已发放 2=已确认）
   */
  issueStatus: number;

}


/**
 * Payslip 导入模板行 DTO
 * 对应前端 PayslipTemplate
 * @description 对应后端 TaktPayslipTemplateDto
 */
export interface PayslipTemplate {
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
   * 发薪期间（如 2026-06）
   */
  payPeriod?: string;

  /**
   * 基本工资（元）
   */
  baseSalary?: number;

  /**
   * 岗位工资（元）
   */
  positionSalary?: number;

  /**
   * 绩效/奖金（元）
   */
  bonusAmount?: number;

  /**
   * 加班费（元）
   */
  overtimePay?: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal?: number;

  /**
   * 应发合计（元）
   */
  grossAmount?: number;

  /**
   * 社保扣款（元）
   */
  socialSecurityDeduction?: number;

  /**
   * 公积金扣款（元）
   */
  housingFundDeduction?: number;

  /**
   * 个税扣款（元）
   */
  taxDeduction?: number;

  /**
   * 其他扣款（元）
   */
  otherDeduction?: number;

  /**
   * 实发金额（元）
   */
  netAmount?: number;

  /**
   * 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
   */
  formulaSetCode?: string;

  /**
   * 发放状态（字典 hr_payslip_issue_status：0=待发放 1=已发放 2=已确认）
   */
  issueStatus?: number;

  /**
   * 发放日期
   */
  issueDate?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * Payslip 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PayslipImport
 * @description 对应后端 TaktPayslipImportDto
 */
export interface PayslipImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
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
   * 发薪期间（如 2026-06）
   */
  payPeriod?: string;

  /**
   * 基本工资（元）
   */
  baseSalary?: number;

  /**
   * 岗位工资（元）
   */
  positionSalary?: number;

  /**
   * 绩效/奖金（元）
   */
  bonusAmount?: number;

  /**
   * 加班费（元）
   */
  overtimePay?: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal?: number;

  /**
   * 应发合计（元）
   */
  grossAmount?: number;

  /**
   * 社保扣款（元）
   */
  socialSecurityDeduction?: number;

  /**
   * 公积金扣款（元）
   */
  housingFundDeduction?: number;

  /**
   * 个税扣款（元）
   */
  taxDeduction?: number;

  /**
   * 其他扣款（元）
   */
  otherDeduction?: number;

  /**
   * 实发金额（元）
   */
  netAmount?: number;

  /**
   * 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
   */
  formulaSetCode?: string;

  /**
   * 发放状态（字典 hr_payslip_issue_status：0=待发放 1=已发放 2=已确认）
   */
  issueStatus?: number;

  /**
   * 发放日期
   */
  issueDate?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * Payslip 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PayslipExport
 * @description 对应后端 TaktPayslipExportDto
 */
export interface PayslipExport {
  /**
   * PayslipID
   */
  payslipId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 发薪期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 基本工资（元）
   */
  baseSalary: number;

  /**
   * 岗位工资（元）
   */
  positionSalary: number;

  /**
   * 绩效/奖金（元）
   */
  bonusAmount: number;

  /**
   * 加班费（元）
   */
  overtimePay: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal: number;

  /**
   * 应发合计（元）
   */
  grossAmount: number;

  /**
   * 社保扣款（元）
   */
  socialSecurityDeduction: number;

  /**
   * 公积金扣款（元）
   */
  housingFundDeduction: number;

  /**
   * 个税扣款（元）
   */
  taxDeduction: number;

  /**
   * 其他扣款（元）
   */
  otherDeduction: number;

  /**
   * 实发金额（元）
   */
  netAmount: number;

  /**
   * 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
   */
  formulaSetCode?: string;

  /**
   * 发放状态（字典 hr_payslip_issue_status：0=待发放 1=已发放 2=已确认）
   */
  issueStatus: number;

  /**
   * 发放日期
   */
  issueDate?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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

