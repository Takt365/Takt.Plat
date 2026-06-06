// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation-benefits
// 文件名称：tax-calc.d.ts
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
 * 个税计算规则（税率档、扣除标准等）
 * 对应前端 TaktTaxCalcDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TaxCalc
 * @description 对应后端 TaktTaxCalcDto
 */
export interface TaxCalc extends CompanyDtoBase {
  /**
   * TaxCalcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  taxCalcId: string;

  /**
   * 规则编码（租户+公司内唯一）
   */
  ruleCode: string;

  /**
   * 规则名称
   */
  ruleName: string;

  /**
   * 税务年度
   */
  taxYear: number;

  /**
   * 税收起征点
   */
  taxThreshold: number;

  /**
   * 应纳税所得额下限
   */
  taxableIncomeMin: number;

  /**
   * 应纳税所得额上限
   */
  taxableIncomeMax: number;

  /**
   * 税率（%）
   */
  taxRate: number;

  /**
   * 速算扣除数
   */
  quickDeduction: number;

  /**
   * 专项扣除标准
   */
  specialDeductionStandard: number;

  /**
   * 社保扣除比例（%）
   */
  socialSecurityDeductionRate: number;

  /**
   * 公积金扣除比例（%）
   */
  housingFundDeductionRate: number;

  /**
   * 计算公式
   */
  calculationFormula: string;

  /**
   * 规则说明
   */
  description: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（0=启用 1=停用）
   */
  taxCalcStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * TaxCalc 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TaxCalcQuery
 * @description 对应后端 TaktTaxCalcQueryDto
 */
export interface TaxCalcQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 规则编码（租户+公司内唯一）
   */
  ruleCode?: string;

  /**
   * 规则名称
   */
  ruleName?: string;

  /**
   * 税务年度
   */
  taxYear?: number;

  /**
   * 税收起征点
   */
  taxThreshold?: number;

  /**
   * 应纳税所得额下限
   */
  taxableIncomeMin?: number;

  /**
   * 应纳税所得额上限
   */
  taxableIncomeMax?: number;

  /**
   * 税率（%）
   */
  taxRate?: number;

  /**
   * 速算扣除数
   */
  quickDeduction?: number;

  /**
   * 专项扣除标准
   */
  specialDeductionStandard?: number;

  /**
   * 社保扣除比例（%）
   */
  socialSecurityDeductionRate?: number;

  /**
   * 公积金扣除比例（%）
   */
  housingFundDeductionRate?: number;

  /**
   * 计算公式
   */
  calculationFormula?: string;

  /**
   * 规则说明
   */
  description?: string;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 状态（0=启用 1=停用）
   */
  taxCalcStatus?: number;

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
 * 创建TaxCalc DTO
 * 对应前端 TaxCalcCreate
 * @description 对应后端 TaktTaxCalcCreateDto
 */
export interface TaxCalcCreate {
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
   * 规则编码（租户+公司内唯一）
   */
  ruleCode: string;

  /**
   * 规则名称
   */
  ruleName: string;

  /**
   * 税务年度
   */
  taxYear: number;

  /**
   * 税收起征点
   */
  taxThreshold: number;

  /**
   * 应纳税所得额下限
   */
  taxableIncomeMin: number;

  /**
   * 应纳税所得额上限
   */
  taxableIncomeMax: number;

  /**
   * 税率（%）
   */
  taxRate: number;

  /**
   * 速算扣除数
   */
  quickDeduction: number;

  /**
   * 专项扣除标准
   */
  specialDeductionStandard: number;

  /**
   * 社保扣除比例（%）
   */
  socialSecurityDeductionRate: number;

  /**
   * 公积金扣除比例（%）
   */
  housingFundDeductionRate: number;

  /**
   * 计算公式
   */
  calculationFormula: string;

  /**
   * 规则说明
   */
  description: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（0=启用 1=停用）
   */
  taxCalcStatus: number;

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
 * 更新TaxCalc DTO
 * 继承 TaktTaxCalcCreateDto，添加 TaxCalcId 字段
 * 对应前端 TaxCalcUpdate
 * @description 对应后端 TaktTaxCalcUpdateDto
 */
export interface TaxCalcUpdate extends TaxCalcCreate {
  /**
   * TaxCalcID（标识要更新的实体）
   */
  taxCalcId: string;

}


/**
 * TaxCalc 状态更新 DTO
 * 对应前端 TaxCalcStatus
 * @description 对应后端 TaktTaxCalcStatusDto
 */
export interface TaxCalcStatus {
  /**
   * TaxCalcID
   */
  taxCalcId: string;

  /**
   * 状态（0=启用 1=停用）
   */
  taxCalcStatus: number;

}


/**
 * TaxCalc 导入模板行 DTO
 * 对应前端 TaxCalcTemplate
 * @description 对应后端 TaktTaxCalcTemplateDto
 */
export interface TaxCalcTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 规则编码（租户+公司内唯一）
   */
  ruleCode?: string;

  /**
   * 规则名称
   */
  ruleName?: string;

  /**
   * 税务年度
   */
  taxYear?: number;

  /**
   * 计算公式
   */
  calculationFormula?: string;

  /**
   * 规则说明
   */
  description?: string;

  /**
   * 状态（0=启用 1=停用）
   */
  taxCalcStatus?: number;

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
 * TaxCalc 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TaxCalcImport
 * @description 对应后端 TaktTaxCalcImportDto
 */
export interface TaxCalcImport {
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
   * 规则编码（租户+公司内唯一）
   */
  ruleCode?: string;

  /**
   * 规则名称
   */
  ruleName?: string;

  /**
   * 税务年度
   */
  taxYear?: number;

  /**
   * 计算公式
   */
  calculationFormula?: string;

  /**
   * 规则说明
   */
  description?: string;

  /**
   * 状态（0=启用 1=停用）
   */
  taxCalcStatus?: number;

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
 * TaxCalc 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TaxCalcExport
 * @description 对应后端 TaktTaxCalcExportDto
 */
export interface TaxCalcExport {
  /**
   * TaxCalcID
   */
  taxCalcId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 规则编码（租户+公司内唯一）
   */
  ruleCode: string;

  /**
   * 规则名称
   */
  ruleName: string;

  /**
   * 税务年度
   */
  taxYear: number;

  /**
   * 税收起征点
   */
  taxThreshold: number;

  /**
   * 应纳税所得额下限
   */
  taxableIncomeMin: number;

  /**
   * 应纳税所得额上限
   */
  taxableIncomeMax: number;

  /**
   * 税率（%）
   */
  taxRate: number;

  /**
   * 速算扣除数
   */
  quickDeduction: number;

  /**
   * 专项扣除标准
   */
  specialDeductionStandard: number;

  /**
   * 社保扣除比例（%）
   */
  socialSecurityDeductionRate: number;

  /**
   * 公积金扣除比例（%）
   */
  housingFundDeductionRate: number;

  /**
   * 计算公式
   */
  calculationFormula: string;

  /**
   * 规则说明
   */
  description: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（0=启用 1=停用）
   */
  taxCalcStatus: number;

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

