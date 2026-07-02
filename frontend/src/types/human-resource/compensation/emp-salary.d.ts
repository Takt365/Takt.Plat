// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：emp-salary.d.ts
// 创建时间：2026-06-24
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
 * 员工薪酬档案（现金报酬定薪记录）
 * 对应前端 TaktEmpSalaryDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmpSalary
 * @description 对应后端 TaktEmpSalaryDto
 */
export interface EmpSalary extends CompanyDtoBase {
  /**
   * EmpSalaryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  empSalaryId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 关联薪酬体系 ID
   */
  payrollId?: string;

  /**
   * 关联薪酬体系 名称（填充字段）
   */
  payrollName?: string;

  /**
   * 关联薪级 ID
   */
  payScaleId?: string;

  /**
   * 关联薪级 名称（填充字段）
   */
  payScaleName?: string;

  /**
   * 基本工资（元）
   */
  baseSalary: number;

  /**
   * 岗位工资（元）
   */
  positionSalary: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal: number;

  /**
   * 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
   */
  salaryItemId?: string;

  /**
   * 关联薪资项目 名称（填充字段）
   */
  salaryItemName?: string;

  /**
   * 授予股数/份数（股权激励定薪时使用）
   */
  empSalaryShareCount: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  empSalaryStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * EmpSalary 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmpSalaryQuery
 * @description 对应后端 TaktEmpSalaryQueryDto
 */
export interface EmpSalaryQuery extends TaktPagedQuery {
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
   * 关联薪酬体系 ID
   */
  payrollId?: string;

  /**
   * 关联薪级 ID
   */
  payScaleId?: string;

  /**
   * 基本工资（元）
   */
  baseSalary?: number;

  /**
   * 岗位工资（元）
   */
  positionSalary?: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal?: number;

  /**
   * 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
   */
  salaryItemId?: string;

  /**
   * 授予股数/份数（股权激励定薪时使用）
   */
  empSalaryShareCount?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  empSalaryStatus?: number;

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
 * 创建EmpSalary DTO
 * 对应前端 EmpSalaryCreate
 * @description 对应后端 TaktEmpSalaryCreateDto
 */
export interface EmpSalaryCreate {
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
   * 关联薪酬体系 ID
   */
  payrollId?: string;

  /**
   * 关联薪级 ID
   */
  payScaleId?: string;

  /**
   * 基本工资（元）
   */
  baseSalary: number;

  /**
   * 岗位工资（元）
   */
  positionSalary: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal: number;

  /**
   * 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
   */
  salaryItemId?: string;

  /**
   * 授予股数/份数（股权激励定薪时使用）
   */
  empSalaryShareCount: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  empSalaryStatus: number;

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
 * 更新EmpSalary DTO
 * 继承 TaktEmpSalaryCreateDto，添加 EmpSalaryId 字段
 * 对应前端 EmpSalaryUpdate
 * @description 对应后端 TaktEmpSalaryUpdateDto
 */
export interface EmpSalaryUpdate extends EmpSalaryCreate {
  /**
   * EmpSalaryID（标识要更新的实体）
   */
  empSalaryId: string;

}


/**
 * EmpSalary 状态更新 DTO
 * 对应前端 EmpSalaryStatus
 * @description 对应后端 TaktEmpSalaryStatusDto
 */
export interface EmpSalaryStatus {
  /**
   * EmpSalaryID
   */
  empSalaryId: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  empSalaryStatus: number;

}


/**
 * EmpSalary 导入模板行 DTO
 * 对应前端 EmpSalaryTemplate
 * @description 对应后端 TaktEmpSalaryTemplateDto
 */
export interface EmpSalaryTemplate {
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
   * 关联薪酬体系 ID
   */
  payrollId?: string;

  /**
   * 关联薪级 ID
   */
  payScaleId?: string;

  /**
   * 基本工资（元）
   */
  baseSalary?: number;

  /**
   * 岗位工资（元）
   */
  positionSalary?: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal?: number;

  /**
   * 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
   */
  salaryItemId?: string;

  /**
   * 授予股数/份数（股权激励定薪时使用）
   */
  empSalaryShareCount?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  empSalaryStatus?: number;

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
 * EmpSalary 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmpSalaryImport
 * @description 对应后端 TaktEmpSalaryImportDto
 */
export interface EmpSalaryImport {
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
   * 关联薪酬体系 ID
   */
  payrollId?: string;

  /**
   * 关联薪级 ID
   */
  payScaleId?: string;

  /**
   * 基本工资（元）
   */
  baseSalary?: number;

  /**
   * 岗位工资（元）
   */
  positionSalary?: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal?: number;

  /**
   * 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
   */
  salaryItemId?: string;

  /**
   * 授予股数/份数（股权激励定薪时使用）
   */
  empSalaryShareCount?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  empSalaryStatus?: number;

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
 * EmpSalary 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmpSalaryExport
 * @description 对应后端 TaktEmpSalaryExportDto
 */
export interface EmpSalaryExport {
  /**
   * EmpSalaryID
   */
  empSalaryId: string;

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
   * 关联薪酬体系 ID
   */
  payrollId?: string;

  /**
   * 关联薪级 ID
   */
  payScaleId?: string;

  /**
   * 基本工资（元）
   */
  baseSalary: number;

  /**
   * 岗位工资（元）
   */
  positionSalary: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal: number;

  /**
   * 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
   */
  salaryItemId?: string;

  /**
   * 授予股数/份数（股权激励定薪时使用）
   */
  empSalaryShareCount: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  empSalaryStatus: number;

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

