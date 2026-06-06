// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation-benefits
// 文件名称：salary-calc.d.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation-benefits 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 薪资核算批次
 * 对应前端 TaktSalaryCalcDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalaryCalc
 * @description 对应后端 TaktSalaryCalcDto
 */
export interface SalaryCalc extends CompanyDtoBase {
  /**
   * SalaryCalcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salaryCalcId: string;

  /**
   * 核算批次编码（租户+公司内唯一）
   */
  calcCode: string;

  /**
   * 核算批次名称
   */
  calcName: string;

  /**
   * 发薪期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 核算日期
   */
  calcDate: string;

  /**
   * 参与核算人数
   */
  employeeCount: number;

  /**
   * 应发合计（元）
   */
  grossAmount: number;

  /**
   * 实发合计（元）
   */
  netAmount: number;

  /**
   * 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
   */
  calcStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * SalaryCalc 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalaryCalcQuery
 * @description 对应后端 TaktSalaryCalcQueryDto
 */
export interface SalaryCalcQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 核算批次编码（租户+公司内唯一）
   */
  calcCode?: string;

  /**
   * 核算批次名称
   */
  calcName?: string;

  /**
   * 发薪期间（如 2026-06）
   */
  payPeriod?: string;

  /**
   * 核算日期（范围查询-开始）
   */
  calcDateStart?: string;

  /**
   * 核算日期（范围查询-结束）
   */
  calcDateEnd?: string;

  /**
   * 参与核算人数
   */
  employeeCount?: number;

  /**
   * 应发合计（元）
   */
  grossAmount?: number;

  /**
   * 实发合计（元）
   */
  netAmount?: number;

  /**
   * 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
   */
  calcStatus?: number;

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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建SalaryCalc DTO
 * 对应前端 SalaryCalcCreate
 * @description 对应后端 TaktSalaryCalcCreateDto
 */
export interface SalaryCalcCreate {
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
   * 核算批次编码（租户+公司内唯一）
   */
  calcCode: string;

  /**
   * 核算批次名称
   */
  calcName: string;

  /**
   * 发薪期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 核算日期
   */
  calcDate: string;

  /**
   * 参与核算人数
   */
  employeeCount: number;

  /**
   * 应发合计（元）
   */
  grossAmount: number;

  /**
   * 实发合计（元）
   */
  netAmount: number;

  /**
   * 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
   */
  calcStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新SalaryCalc DTO
 * 继承 TaktSalaryCalcCreateDto，添加 SalaryCalcId 字段
 * 对应前端 SalaryCalcUpdate
 * @description 对应后端 TaktSalaryCalcUpdateDto
 */
export interface SalaryCalcUpdate extends SalaryCalcCreate {
  /**
   * SalaryCalcID（标识要更新的实体）
   */
  salaryCalcId: string;

}


/**
 * SalaryCalc 状态更新 DTO
 * 对应前端 SalaryCalcStatus
 * @description 对应后端 TaktSalaryCalcStatusDto
 */
export interface SalaryCalcStatus {
  /**
   * SalaryCalcID
   */
  salaryCalcId: string;

  /**
   * 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
   */
  calcStatus: number;

}


/**
 * SalaryCalc 导入模板行 DTO
 * 对应前端 SalaryCalcTemplate
 * @description 对应后端 TaktSalaryCalcTemplateDto
 */
export interface SalaryCalcTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 核算批次编码（租户+公司内唯一）
   */
  calcCode?: string;

  /**
   * 核算批次名称
   */
  calcName?: string;

  /**
   * 发薪期间（如 2026-06）
   */
  payPeriod?: string;

  /**
   * 参与核算人数
   */
  employeeCount?: number;

  /**
   * 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
   */
  calcStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * SalaryCalc 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalaryCalcImport
 * @description 对应后端 TaktSalaryCalcImportDto
 */
export interface SalaryCalcImport {
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
   * 核算批次编码（租户+公司内唯一）
   */
  calcCode?: string;

  /**
   * 核算批次名称
   */
  calcName?: string;

  /**
   * 发薪期间（如 2026-06）
   */
  payPeriod?: string;

  /**
   * 参与核算人数
   */
  employeeCount?: number;

  /**
   * 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
   */
  calcStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * SalaryCalc 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalaryCalcExport
 * @description 对应后端 TaktSalaryCalcExportDto
 */
export interface SalaryCalcExport {
  /**
   * SalaryCalcID
   */
  salaryCalcId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 核算批次编码（租户+公司内唯一）
   */
  calcCode: string;

  /**
   * 核算批次名称
   */
  calcName: string;

  /**
   * 发薪期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 核算日期
   */
  calcDate: string;

  /**
   * 参与核算人数
   */
  employeeCount: number;

  /**
   * 应发合计（元）
   */
  grossAmount: number;

  /**
   * 实发合计（元）
   */
  netAmount: number;

  /**
   * 核算状态（0=草稿 1=核算中 2=已完成 3=已归档）
   */
  calcStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

