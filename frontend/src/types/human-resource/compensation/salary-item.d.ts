// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：salary-item.d.ts
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
 * 薪资项目（现金报酬可配置主数据，含股权激励；不另建 TaktStockOption 等平行实体）
 * 对应前端 TaktSalaryItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalaryItem
 * @description 对应后端 TaktSalaryItemDto
 */
export interface SalaryItem extends CompanyDtoBase {
  /**
   * SalaryItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salaryItemId: string;

  /**
   * 项目编码（租户+公司内唯一）
   */
  itemCode: string;

  /**
   * 项目名称
   */
  itemName: string;

  /**
   * 简称
   */
  shortName?: string;

  /**
   * 项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
   */
  itemType: number;

  /**
   * 计算方式（字典 hr_salary_calc_method_type：固定金额/按比例/按公式）
   */
  calcMethod: number;

  /**
   * 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
   */
  salaryFormulaId?: string;

  /**
   * 关联计算公式步骤 名称（填充字段）
   */
  salaryFormulaName?: string;

  /**
   * 默认金额（元）
   */
  defaultAmount: number;

  /**
   * 默认比例（%，0~100）
   */
  defaultRate: number;

  /**
   * 默认行权/授予价格（元；item_type 为股权激励时使用）
   */
  strikePrice: number;

  /**
   * 默认归属年限（年；item_type 为股权激励时使用）
   */
  vestingYears: number;

  /**
   * 是否扣款项（字典 sys_yes_no_type）
   */
  isDeduction: number;

  /**
   * 是否计入应税所得（字典 sys_yes_no_type）
   */
  isTaxable: number;

  /**
   * 是否计入社保基数（字典 sys_yes_no_type）
   */
  includeSocialSecurityBase: number;

  /**
   * 是否计入公积金基数（字典 sys_yes_no_type）
   */
  includeHousingFundBase: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * SalaryItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalaryItemQuery
 * @description 对应后端 TaktSalaryItemQueryDto
 */
export interface SalaryItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 项目编码（租户+公司内唯一）
   */
  itemCode?: string;

  /**
   * 项目名称
   */
  itemName?: string;

  /**
   * 简称
   */
  shortName?: string;

  /**
   * 项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
   */
  itemType?: number;

  /**
   * 计算方式（字典 hr_salary_calc_method_type：固定金额/按比例/按公式）
   */
  calcMethod?: number;

  /**
   * 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
   */
  salaryFormulaId?: string;

  /**
   * 默认金额（元）
   */
  defaultAmount?: number;

  /**
   * 默认比例（%，0~100）
   */
  defaultRate?: number;

  /**
   * 默认行权/授予价格（元；item_type 为股权激励时使用）
   */
  strikePrice?: number;

  /**
   * 默认归属年限（年；item_type 为股权激励时使用）
   */
  vestingYears?: number;

  /**
   * 是否扣款项（字典 sys_yes_no_type）
   */
  isDeduction?: number;

  /**
   * 是否计入应税所得（字典 sys_yes_no_type）
   */
  isTaxable?: number;

  /**
   * 是否计入社保基数（字典 sys_yes_no_type）
   */
  includeSocialSecurityBase?: number;

  /**
   * 是否计入公积金基数（字典 sys_yes_no_type）
   */
  includeHousingFundBase?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus?: number;

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
 * 创建SalaryItem DTO
 * 对应前端 SalaryItemCreate
 * @description 对应后端 TaktSalaryItemCreateDto
 */
export interface SalaryItemCreate {
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
   * 项目编码（租户+公司内唯一）
   */
  itemCode: string;

  /**
   * 项目名称
   */
  itemName: string;

  /**
   * 简称
   */
  shortName?: string;

  /**
   * 项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
   */
  itemType: number;

  /**
   * 计算方式（字典 hr_salary_calc_method_type：固定金额/按比例/按公式）
   */
  calcMethod: number;

  /**
   * 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
   */
  salaryFormulaId?: string;

  /**
   * 默认金额（元）
   */
  defaultAmount: number;

  /**
   * 默认比例（%，0~100）
   */
  defaultRate: number;

  /**
   * 默认行权/授予价格（元；item_type 为股权激励时使用）
   */
  strikePrice: number;

  /**
   * 默认归属年限（年；item_type 为股权激励时使用）
   */
  vestingYears: number;

  /**
   * 是否扣款项（字典 sys_yes_no_type）
   */
  isDeduction: number;

  /**
   * 是否计入应税所得（字典 sys_yes_no_type）
   */
  isTaxable: number;

  /**
   * 是否计入社保基数（字典 sys_yes_no_type）
   */
  includeSocialSecurityBase: number;

  /**
   * 是否计入公积金基数（字典 sys_yes_no_type）
   */
  includeHousingFundBase: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus: number;

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
 * 更新SalaryItem DTO
 * 继承 TaktSalaryItemCreateDto，添加 SalaryItemId 字段
 * 对应前端 SalaryItemUpdate
 * @description 对应后端 TaktSalaryItemUpdateDto
 */
export interface SalaryItemUpdate extends SalaryItemCreate {
  /**
   * SalaryItemID（标识要更新的实体）
   */
  salaryItemId: string;

}


/**
 * SalaryItem 状态更新 DTO
 * 对应前端 SalaryItemStatus
 * @description 对应后端 TaktSalaryItemStatusDto
 */
export interface SalaryItemStatus {
  /**
   * SalaryItemID
   */
  salaryItemId: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus: number;

}


/**
 * SalaryItem 排序更新 DTO
 * 对应前端 SalaryItemSort
 * @description 对应后端 TaktSalaryItemSortDto
 */
export interface SalaryItemSort {
  /**
   * SalaryItemID
   */
  salaryItemId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * SalaryItem 导入模板行 DTO
 * 对应前端 SalaryItemTemplate
 * @description 对应后端 TaktSalaryItemTemplateDto
 */
export interface SalaryItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 项目编码（租户+公司内唯一）
   */
  itemCode?: string;

  /**
   * 项目名称
   */
  itemName?: string;

  /**
   * 简称
   */
  shortName?: string;

  /**
   * 项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
   */
  itemType?: number;

  /**
   * 计算方式（字典 hr_salary_calc_method_type：固定金额/按比例/按公式）
   */
  calcMethod?: number;

  /**
   * 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
   */
  salaryFormulaId?: string;

  /**
   * 默认归属年限（年；item_type 为股权激励时使用）
   */
  vestingYears?: number;

  /**
   * 是否扣款项（字典 sys_yes_no_type）
   */
  isDeduction?: number;

  /**
   * 是否计入应税所得（字典 sys_yes_no_type）
   */
  isTaxable?: number;

  /**
   * 是否计入社保基数（字典 sys_yes_no_type）
   */
  includeSocialSecurityBase?: number;

  /**
   * 是否计入公积金基数（字典 sys_yes_no_type）
   */
  includeHousingFundBase?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

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
 * SalaryItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalaryItemImport
 * @description 对应后端 TaktSalaryItemImportDto
 */
export interface SalaryItemImport {
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
   * 项目编码（租户+公司内唯一）
   */
  itemCode?: string;

  /**
   * 项目名称
   */
  itemName?: string;

  /**
   * 简称
   */
  shortName?: string;

  /**
   * 项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
   */
  itemType?: number;

  /**
   * 计算方式（字典 hr_salary_calc_method_type：固定金额/按比例/按公式）
   */
  calcMethod?: number;

  /**
   * 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
   */
  salaryFormulaId?: string;

  /**
   * 默认归属年限（年；item_type 为股权激励时使用）
   */
  vestingYears?: number;

  /**
   * 是否扣款项（字典 sys_yes_no_type）
   */
  isDeduction?: number;

  /**
   * 是否计入应税所得（字典 sys_yes_no_type）
   */
  isTaxable?: number;

  /**
   * 是否计入社保基数（字典 sys_yes_no_type）
   */
  includeSocialSecurityBase?: number;

  /**
   * 是否计入公积金基数（字典 sys_yes_no_type）
   */
  includeHousingFundBase?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

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
 * SalaryItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalaryItemExport
 * @description 对应后端 TaktSalaryItemExportDto
 */
export interface SalaryItemExport {
  /**
   * SalaryItemID
   */
  salaryItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 项目编码（租户+公司内唯一）
   */
  itemCode: string;

  /**
   * 项目名称
   */
  itemName: string;

  /**
   * 简称
   */
  shortName?: string;

  /**
   * 项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
   */
  itemType: number;

  /**
   * 计算方式（字典 hr_salary_calc_method_type：固定金额/按比例/按公式）
   */
  calcMethod: number;

  /**
   * 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
   */
  salaryFormulaId?: string;

  /**
   * 默认金额（元）
   */
  defaultAmount: number;

  /**
   * 默认比例（%，0~100）
   */
  defaultRate: number;

  /**
   * 默认行权/授予价格（元；item_type 为股权激励时使用）
   */
  strikePrice: number;

  /**
   * 默认归属年限（年；item_type 为股权激励时使用）
   */
  vestingYears: number;

  /**
   * 是否扣款项（字典 sys_yes_no_type）
   */
  isDeduction: number;

  /**
   * 是否计入应税所得（字典 sys_yes_no_type）
   */
  isTaxable: number;

  /**
   * 是否计入社保基数（字典 sys_yes_no_type）
   */
  includeSocialSecurityBase: number;

  /**
   * 是否计入公积金基数（字典 sys_yes_no_type）
   */
  includeHousingFundBase: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus: number;

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

