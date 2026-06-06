// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation-benefits
// 文件名称：social-security.d.ts
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
 * 员工社保缴纳记录
 * 对应前端 TaktSocialSecurityDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SocialSecurity
 * @description 对应后端 TaktSocialSecurityDto
 */
export interface SocialSecurity extends CompanyDtoBase {
  /**
   * SocialSecurityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  socialSecurityId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 缴纳期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 社保缴纳基数
   */
  socialSecurityBase: number;

  /**
   * 养老保险（元）
   */
  pensionAmount: number;

  /**
   * 医疗保险（元）
   */
  medicalAmount: number;

  /**
   * 失业保险（元）
   */
  unemploymentAmount: number;

  /**
   * 工伤保险（元）
   */
  injuryAmount: number;

  /**
   * 生育保险（元）
   */
  maternityAmount: number;

  /**
   * 公积金缴纳基数
   */
  housingFundBase: number;

  /**
   * 公积金（元）
   */
  housingFundAmount: number;

  /**
   * 缴纳合计（元）
   */
  totalAmount: number;

  /**
   * 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * SocialSecurity 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SocialSecurityQuery
 * @description 对应后端 TaktSocialSecurityQueryDto
 */
export interface SocialSecurityQuery extends TaktPagedQuery {
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
   * 缴纳期间（如 2026-06）
   */
  payPeriod?: string;

  /**
   * 社保缴纳基数
   */
  socialSecurityBase?: number;

  /**
   * 养老保险（元）
   */
  pensionAmount?: number;

  /**
   * 医疗保险（元）
   */
  medicalAmount?: number;

  /**
   * 失业保险（元）
   */
  unemploymentAmount?: number;

  /**
   * 工伤保险（元）
   */
  injuryAmount?: number;

  /**
   * 生育保险（元）
   */
  maternityAmount?: number;

  /**
   * 公积金缴纳基数
   */
  housingFundBase?: number;

  /**
   * 公积金（元）
   */
  housingFundAmount?: number;

  /**
   * 缴纳合计（元）
   */
  totalAmount?: number;

  /**
   * 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus?: number;

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
 * 创建SocialSecurity DTO
 * 对应前端 SocialSecurityCreate
 * @description 对应后端 TaktSocialSecurityCreateDto
 */
export interface SocialSecurityCreate {
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
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 缴纳期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 社保缴纳基数
   */
  socialSecurityBase: number;

  /**
   * 养老保险（元）
   */
  pensionAmount: number;

  /**
   * 医疗保险（元）
   */
  medicalAmount: number;

  /**
   * 失业保险（元）
   */
  unemploymentAmount: number;

  /**
   * 工伤保险（元）
   */
  injuryAmount: number;

  /**
   * 生育保险（元）
   */
  maternityAmount: number;

  /**
   * 公积金缴纳基数
   */
  housingFundBase: number;

  /**
   * 公积金（元）
   */
  housingFundAmount: number;

  /**
   * 缴纳合计（元）
   */
  totalAmount: number;

  /**
   * 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus: number;

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
 * 更新SocialSecurity DTO
 * 继承 TaktSocialSecurityCreateDto，添加 SocialSecurityId 字段
 * 对应前端 SocialSecurityUpdate
 * @description 对应后端 TaktSocialSecurityUpdateDto
 */
export interface SocialSecurityUpdate extends SocialSecurityCreate {
  /**
   * SocialSecurityID（标识要更新的实体）
   */
  socialSecurityId: string;

}


/**
 * SocialSecurity 状态更新 DTO
 * 对应前端 SocialSecurityStatus
 * @description 对应后端 TaktSocialSecurityStatusDto
 */
export interface SocialSecurityStatus {
  /**
   * SocialSecurityID
   */
  socialSecurityId: string;

  /**
   * 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus: number;

}


/**
 * SocialSecurity 导入模板行 DTO
 * 对应前端 SocialSecurityTemplate
 * @description 对应后端 TaktSocialSecurityTemplateDto
 */
export interface SocialSecurityTemplate {
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
   * 缴纳期间（如 2026-06）
   */
  payPeriod?: string;

  /**
   * 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus?: number;

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
 * SocialSecurity 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SocialSecurityImport
 * @description 对应后端 TaktSocialSecurityImportDto
 */
export interface SocialSecurityImport {
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
   * 员工 ID
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 缴纳期间（如 2026-06）
   */
  payPeriod?: string;

  /**
   * 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus?: number;

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
 * SocialSecurity 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SocialSecurityExport
 * @description 对应后端 TaktSocialSecurityExportDto
 */
export interface SocialSecurityExport {
  /**
   * SocialSecurityID
   */
  socialSecurityId: string;

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
   * 缴纳期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 社保缴纳基数
   */
  socialSecurityBase: number;

  /**
   * 养老保险（元）
   */
  pensionAmount: number;

  /**
   * 医疗保险（元）
   */
  medicalAmount: number;

  /**
   * 失业保险（元）
   */
  unemploymentAmount: number;

  /**
   * 工伤保险（元）
   */
  injuryAmount: number;

  /**
   * 生育保险（元）
   */
  maternityAmount: number;

  /**
   * 公积金缴纳基数
   */
  housingFundBase: number;

  /**
   * 公积金（元）
   */
  housingFundAmount: number;

  /**
   * 缴纳合计（元）
   */
  totalAmount: number;

  /**
   * 缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus: number;

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

