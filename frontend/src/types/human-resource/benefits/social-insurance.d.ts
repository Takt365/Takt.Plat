// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/benefits
// 文件名称：social-insurance.d.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/benefits 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 社保与公积金月度缴纳流水（分项金额明细；福利类型配置不在此表重复建模）
 * 对应前端 TaktSocialInsuranceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SocialInsurance
 * @description 对应后端 TaktSocialInsuranceDto
 */
export interface SocialInsurance extends CompanyDtoBase {
  /**
   * SocialInsuranceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  socialInsuranceId: string;

  /**
   * 关联福利项目 ID（通常对应 hr_benefit_type 为社保/公积金的 TaktBenefitItem）
   */
  benefitItemId?: string;

  /**
   * 关联福利项目 名称（填充字段）
   */
  itemName?: string;

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
   * 社保缴纳基数（元）
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
   * 公积金缴纳基数（元）
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
   * 缴纳状态（字典 hr_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * SocialInsurance 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SocialInsuranceQuery
 * @description 对应后端 TaktSocialInsuranceQueryDto
 */
export interface SocialInsuranceQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 关联福利项目 ID（通常对应 hr_benefit_type 为社保/公积金的 TaktBenefitItem）
   */
  benefitItemId?: string;

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
   * 社保缴纳基数（元）
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
   * 公积金缴纳基数（元）
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
   * 缴纳状态（字典 hr_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建SocialInsurance DTO
 * 对应前端 SocialInsuranceCreate
 * @description 对应后端 TaktSocialInsuranceCreateDto
 */
export interface SocialInsuranceCreate {
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
   * 关联福利项目 ID（通常对应 hr_benefit_type 为社保/公积金的 TaktBenefitItem）
   */
  benefitItemId?: string;

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
   * 社保缴纳基数（元）
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
   * 公积金缴纳基数（元）
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
   * 缴纳状态（字典 hr_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus: number;

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
 * 更新SocialInsurance DTO
 * 继承 TaktSocialInsuranceCreateDto，添加 SocialInsuranceId 字段
 * 对应前端 SocialInsuranceUpdate
 * @description 对应后端 TaktSocialInsuranceUpdateDto
 */
export interface SocialInsuranceUpdate extends SocialInsuranceCreate {
  /**
   * SocialInsuranceID（标识要更新的实体）
   */
  socialInsuranceId: string;

}


/**
 * SocialInsurance 状态更新 DTO
 * 对应前端 SocialInsuranceStatus
 * @description 对应后端 TaktSocialInsuranceStatusDto
 */
export interface SocialInsuranceStatus {
  /**
   * SocialInsuranceID
   */
  socialInsuranceId: string;

  /**
   * 缴纳状态（字典 hr_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus: number;

}


/**
 * SocialInsurance 导入模板行 DTO
 * 对应前端 SocialInsuranceTemplate
 * @description 对应后端 TaktSocialInsuranceTemplateDto
 */
export interface SocialInsuranceTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 关联福利项目 ID（通常对应 hr_benefit_type 为社保/公积金的 TaktBenefitItem）
   */
  benefitItemId?: string;

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
   * 缴纳状态（字典 hr_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus?: number;

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
 * SocialInsurance 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SocialInsuranceImport
 * @description 对应后端 TaktSocialInsuranceImportDto
 */
export interface SocialInsuranceImport {
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
   * 关联福利项目 ID（通常对应 hr_benefit_type 为社保/公积金的 TaktBenefitItem）
   */
  benefitItemId?: string;

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
   * 缴纳状态（字典 hr_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus?: number;

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
 * SocialInsurance 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SocialInsuranceExport
 * @description 对应后端 TaktSocialInsuranceExportDto
 */
export interface SocialInsuranceExport {
  /**
   * SocialInsuranceID
   */
  socialInsuranceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联福利项目 ID（通常对应 hr_benefit_type 为社保/公积金的 TaktBenefitItem）
   */
  benefitItemId?: string;

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
   * 社保缴纳基数（元）
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
   * 公积金缴纳基数（元）
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
   * 缴纳状态（字典 hr_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus: number;

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

