// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：payroll.d.ts
// 创建时间：2026-06-12
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
 * 薪酬体系（现金报酬方案头；组成项引用 TaktSalaryItem，不另建多种薪资实体）
 * 对应前端 TaktPayrollDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Payroll
 * @description 对应后端 TaktPayrollDto
 */
export interface Payroll extends CompanyDtoBase {
  /**
   * PayrollID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  payrollId: string;

  /**
   * 薪酬体系编码（租户+公司内唯一）
   */
  payrollCode: string;

  /**
   * 薪酬体系名称
   */
  payrollName: string;

  /**
   * 关联薪级表 ID
   */
  payScaleId?: string;

  /**
   * 关联薪级表 名称（填充字段）
   */
  payScaleName?: string;

  /**
   * 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
   */
  formulaSetCode?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  payrollStatus: number;

  /**
   * 说明
   */
  description?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * Payroll 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PayrollQuery
 * @description 对应后端 TaktPayrollQueryDto
 */
export interface PayrollQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 薪酬体系编码（租户+公司内唯一）
   */
  payrollCode?: string;

  /**
   * 薪酬体系名称
   */
  payrollName?: string;

  /**
   * 关联薪级表 ID
   */
  payScaleId?: string;

  /**
   * 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
   */
  formulaSetCode?: string;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  expiryDateEnd?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  payrollStatus?: number;

  /**
   * 说明
   */
  description?: string;

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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Payroll DTO
 * 对应前端 PayrollCreate
 * @description 对应后端 TaktPayrollCreateDto
 */
export interface PayrollCreate {
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
   * 薪酬体系编码（租户+公司内唯一）
   */
  payrollCode: string;

  /**
   * 薪酬体系名称
   */
  payrollName: string;

  /**
   * 关联薪级表 ID
   */
  payScaleId?: string;

  /**
   * 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
   */
  formulaSetCode?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  payrollStatus: number;

  /**
   * 说明
   */
  description?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 更新Payroll DTO
 * 继承 TaktPayrollCreateDto，添加 PayrollId 字段
 * 对应前端 PayrollUpdate
 * @description 对应后端 TaktPayrollUpdateDto
 */
export interface PayrollUpdate extends PayrollCreate {
  /**
   * PayrollID（标识要更新的实体）
   */
  payrollId: string;

}


/**
 * Payroll 状态更新 DTO
 * 对应前端 PayrollStatus
 * @description 对应后端 TaktPayrollStatusDto
 */
export interface PayrollStatus {
  /**
   * PayrollID
   */
  payrollId: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  payrollStatus: number;

}


/**
 * Payroll 导入模板行 DTO
 * 对应前端 PayrollTemplate
 * @description 对应后端 TaktPayrollTemplateDto
 */
export interface PayrollTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 薪酬体系编码（租户+公司内唯一）
   */
  payrollCode?: string;

  /**
   * 薪酬体系名称
   */
  payrollName?: string;

  /**
   * 关联薪级表 ID
   */
  payScaleId?: string;

  /**
   * 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
   */
  formulaSetCode?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  payrollStatus?: number;

  /**
   * 说明
   */
  description?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * Payroll 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PayrollImport
 * @description 对应后端 TaktPayrollImportDto
 */
export interface PayrollImport {
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
   * 薪酬体系编码（租户+公司内唯一）
   */
  payrollCode?: string;

  /**
   * 薪酬体系名称
   */
  payrollName?: string;

  /**
   * 关联薪级表 ID
   */
  payScaleId?: string;

  /**
   * 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
   */
  formulaSetCode?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  payrollStatus?: number;

  /**
   * 说明
   */
  description?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * Payroll 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PayrollExport
 * @description 对应后端 TaktPayrollExportDto
 */
export interface PayrollExport {
  /**
   * PayrollID
   */
  payrollId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 薪酬体系编码（租户+公司内唯一）
   */
  payrollCode: string;

  /**
   * 薪酬体系名称
   */
  payrollName: string;

  /**
   * 关联薪级表 ID
   */
  payScaleId?: string;

  /**
   * 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
   */
  formulaSetCode?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  payrollStatus: number;

  /**
   * 说明
   */
  description?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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

