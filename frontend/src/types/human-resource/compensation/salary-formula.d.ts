// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：salary-formula.d.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/Payroll 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 薪资计算公式（方案+步骤合一：set_code 分组，每行一步；标准五步：应发→社保→公积金→个税→实发） 同一 set_code 示例： gross_amount = base_salary + bonus_amount + overtime_pay + allowance_total social_security_deduction = social_security_base * employee_ss_ratio housing_fund_deduction = housing_fund_base * employee_hf_ratio tax_deduction = CUMULATIVE_TAX(taxable_income) net_amount = gross_amount - social_security_deduction - housing_fund_deduction - tax_deduction - other_deduction
 * 对应前端 TaktSalaryFormulaDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalaryFormula
 * @description 对应后端 TaktSalaryFormulaDto
 */
export interface SalaryFormula extends CompanyDtoBase {
  /**
   * SalaryFormulaID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salaryFormulaId: string;

  /**
   * 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
   */
  setCode: string;

  /**
   * 公式方案名称
   */
  setName: string;

  /**
   * 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
   */
  PayrollId?: string;

  /**
   * 关联薪酬体系 名称（填充字段）
   */
  PayrollName?: string;

  /**
   * 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
   */
  formulaCode: string;

  /**
   * 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
   */
  formulaName: string;

  /**
   * 公式步骤类型（字典 hr_salary_formula_step：应发/社保个人/公积金个人/个税/实发）
   */
  formulaStep: number;

  /**
   * 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
   */
  sortOrder: number;

  /**
   * 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
   */
  targetField: string;

  /**
   * 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
   */
  formulaExpression: string;

  /**
   * 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
   */
  stepDescription?: string;

  /**
   * 方案生效日期（同 set_code 各行应一致）
   */
  effectiveDate: string;

  /**
   * 方案失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  formulaStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * SalaryFormula 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalaryFormulaQuery
 * @description 对应后端 TaktSalaryFormulaQueryDto
 */
export interface SalaryFormulaQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
   */
  setCode?: string;

  /**
   * 公式方案名称
   */
  setName?: string;

  /**
   * 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
   */
  PayrollId?: string;

  /**
   * 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
   */
  formulaCode?: string;

  /**
   * 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
   */
  formulaName?: string;

  /**
   * 公式步骤类型（字典 hr_salary_formula_step：应发/社保个人/公积金个人/个税/实发）
   */
  formulaStep?: number;

  /**
   * 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
   */
  sortOrder?: number;

  /**
   * 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
   */
  targetField?: string;

  /**
   * 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
   */
  formulaExpression?: string;

  /**
   * 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
   */
  stepDescription?: string;

  /**
   * 方案生效日期（同 set_code 各行应一致）（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 方案生效日期（同 set_code 各行应一致）（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 方案失效日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 方案失效日期（范围查询-结束）
   */
  expiryDateEnd?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  formulaStatus?: number;

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
 * 创建SalaryFormula DTO
 * 对应前端 SalaryFormulaCreate
 * @description 对应后端 TaktSalaryFormulaCreateDto
 */
export interface SalaryFormulaCreate {
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
   * 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
   */
  setCode: string;

  /**
   * 公式方案名称
   */
  setName: string;

  /**
   * 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
   */
  PayrollId?: string;

  /**
   * 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
   */
  formulaCode: string;

  /**
   * 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
   */
  formulaName: string;

  /**
   * 公式步骤类型（字典 hr_salary_formula_step：应发/社保个人/公积金个人/个税/实发）
   */
  formulaStep: number;

  /**
   * 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
   */
  sortOrder: number;

  /**
   * 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
   */
  targetField: string;

  /**
   * 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
   */
  formulaExpression: string;

  /**
   * 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
   */
  stepDescription?: string;

  /**
   * 方案生效日期（同 set_code 各行应一致）
   */
  effectiveDate: string;

  /**
   * 方案失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  formulaStatus: number;

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
 * 更新SalaryFormula DTO
 * 继承 TaktSalaryFormulaCreateDto，添加 SalaryFormulaId 字段
 * 对应前端 SalaryFormulaUpdate
 * @description 对应后端 TaktSalaryFormulaUpdateDto
 */
export interface SalaryFormulaUpdate extends SalaryFormulaCreate {
  /**
   * SalaryFormulaID（标识要更新的实体）
   */
  salaryFormulaId: string;

}


/**
 * SalaryFormula 状态更新 DTO
 * 对应前端 SalaryFormulaStatus
 * @description 对应后端 TaktSalaryFormulaStatusDto
 */
export interface SalaryFormulaStatus {
  /**
   * SalaryFormulaID
   */
  salaryFormulaId: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  formulaStatus: number;

}


/**
 * SalaryFormula 排序更新 DTO
 * 对应前端 SalaryFormulaSort
 * @description 对应后端 TaktSalaryFormulaSortDto
 */
export interface SalaryFormulaSort {
  /**
   * SalaryFormulaID
   */
  salaryFormulaId: string;

  /**
   * 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
   */
  sortOrder: number;

}


/**
 * SalaryFormula 导入模板行 DTO
 * 对应前端 SalaryFormulaTemplate
 * @description 对应后端 TaktSalaryFormulaTemplateDto
 */
export interface SalaryFormulaTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
   */
  setCode?: string;

  /**
   * 公式方案名称
   */
  setName?: string;

  /**
   * 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
   */
  PayrollId?: string;

  /**
   * 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
   */
  formulaCode?: string;

  /**
   * 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
   */
  formulaName?: string;

  /**
   * 公式步骤类型（字典 hr_salary_formula_step：应发/社保个人/公积金个人/个税/实发）
   */
  formulaStep?: number;

  /**
   * 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
   */
  sortOrder?: number;

  /**
   * 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
   */
  targetField?: string;

  /**
   * 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
   */
  formulaExpression?: string;

  /**
   * 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
   */
  stepDescription?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  formulaStatus?: number;

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
 * SalaryFormula 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalaryFormulaImport
 * @description 对应后端 TaktSalaryFormulaImportDto
 */
export interface SalaryFormulaImport {
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
   * 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
   */
  setCode?: string;

  /**
   * 公式方案名称
   */
  setName?: string;

  /**
   * 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
   */
  PayrollId?: string;

  /**
   * 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
   */
  formulaCode?: string;

  /**
   * 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
   */
  formulaName?: string;

  /**
   * 公式步骤类型（字典 hr_salary_formula_step：应发/社保个人/公积金个人/个税/实发）
   */
  formulaStep?: number;

  /**
   * 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
   */
  sortOrder?: number;

  /**
   * 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
   */
  targetField?: string;

  /**
   * 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
   */
  formulaExpression?: string;

  /**
   * 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
   */
  stepDescription?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  formulaStatus?: number;

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
 * SalaryFormula 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalaryFormulaExport
 * @description 对应后端 TaktSalaryFormulaExportDto
 */
export interface SalaryFormulaExport {
  /**
   * SalaryFormulaID
   */
  salaryFormulaId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
   */
  setCode: string;

  /**
   * 公式方案名称
   */
  setName: string;

  /**
   * 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
   */
  PayrollId?: string;

  /**
   * 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
   */
  formulaCode: string;

  /**
   * 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
   */
  formulaName: string;

  /**
   * 公式步骤类型（字典 hr_salary_formula_step：应发/社保个人/公积金个人/个税/实发）
   */
  formulaStep: number;

  /**
   * 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
   */
  sortOrder: number;

  /**
   * 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
   */
  targetField: string;

  /**
   * 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
   */
  formulaExpression: string;

  /**
   * 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
   */
  stepDescription?: string;

  /**
   * 方案生效日期（同 set_code 各行应一致）
   */
  effectiveDate: string;

  /**
   * 方案失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  formulaStatus: number;

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

