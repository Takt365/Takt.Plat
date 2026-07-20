// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：budget-actual.d.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 预算实绩实体（管理会计 Budget vs Actual；CAS 全面预算管理实务 / 国际通用管理会计） 差异约定：差异金额 = 实绩 − 预算；差异率 = 差异 / |预算|（预算为 0 时为 0）。 唯一键：租户 + 公司 + 工厂 + 期间 + 成本中心 + 预算项
 * 对应前端 TaktBudgetActualDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BudgetActual
 * @description 对应后端 TaktBudgetActualDto
 */
export interface BudgetActual extends CompanyDtoBase {
  /**
   * BudgetActualID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  budgetActualId: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode: string;

  /**
   * 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
   */
  costCenterCode: string;

  /**
   * 成本中心名称（冗余）
   */
  costCenterName?: string;

  /**
   * 预算项编码
   */
  budgetItemCode: string;

  /**
   * 预算项名称
   */
  budgetItemName: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 预算类型（字典 accounting_budget_type；1=经营预算，2=资本预算，3=财务预算）
   */
  budgetType: number;

  /**
   * 计量类型（字典 accounting_budget_measure_type；1=金额，2=数量）
   */
  measureType: number;

  /**
   * 本期预算金额（或数量，视 MeasureType）
   */
  budgetAmount: number;

  /**
   * 本期实绩金额（或数量）
   */
  actualAmount: number;

  /**
   * 本期差异金额（= 实绩 − 预算）
   */
  varianceAmount: number;

  /**
   * 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
   */
  variancePercent: number;

  /**
   * 上年同期实绩（比较分析）
   */
  priorPeriodActual: number;

  /**
   * 本年累计预算
   */
  ytdBudgetAmount: number;

  /**
   * 本年累计实绩
   */
  ytdActualAmount: number;

  /**
   * 本年累计差异（= 本年累计实绩 − 本年累计预算）
   */
  ytdVarianceAmount: number;

  /**
   * 币种（字典 accounting_currency_code；数量计量时可仍存报告币）
   */
  currencyCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  budgetActualStatus: number;

}


/**
 * BudgetActual 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BudgetActualQuery
 * @description 对应后端 TaktBudgetActualQueryDto
 */
export interface BudgetActualQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode?: string;

  /**
   * 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
   */
  costCenterCode?: string;

  /**
   * 成本中心名称（冗余）
   */
  costCenterName?: string;

  /**
   * 预算项编码
   */
  budgetItemCode?: string;

  /**
   * 预算项名称
   */
  budgetItemName?: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 预算类型（字典 accounting_budget_type；1=经营预算，2=资本预算，3=财务预算）
   */
  budgetType?: number;

  /**
   * 计量类型（字典 accounting_budget_measure_type；1=金额，2=数量）
   */
  measureType?: number;

  /**
   * 本期预算金额（或数量，视 MeasureType）
   */
  budgetAmount?: number;

  /**
   * 本期实绩金额（或数量）
   */
  actualAmount?: number;

  /**
   * 本期差异金额（= 实绩 − 预算）
   */
  varianceAmount?: number;

  /**
   * 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
   */
  variancePercent?: number;

  /**
   * 上年同期实绩（比较分析）
   */
  priorPeriodActual?: number;

  /**
   * 本年累计预算
   */
  ytdBudgetAmount?: number;

  /**
   * 本年累计实绩
   */
  ytdActualAmount?: number;

  /**
   * 本年累计差异（= 本年累计实绩 − 本年累计预算）
   */
  ytdVarianceAmount?: number;

  /**
   * 币种（字典 accounting_currency_code；数量计量时可仍存报告币）
   */
  currencyCode?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  budgetActualStatus?: number;

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
 * 创建BudgetActual DTO
 * 对应前端 BudgetActualCreate
 * @description 对应后端 TaktBudgetActualCreateDto
 */
export interface BudgetActualCreate {
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
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode: string;

  /**
   * 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
   */
  costCenterCode: string;

  /**
   * 成本中心名称（冗余）
   */
  costCenterName?: string;

  /**
   * 预算项编码
   */
  budgetItemCode: string;

  /**
   * 预算项名称
   */
  budgetItemName: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 预算类型（字典 accounting_budget_type；1=经营预算，2=资本预算，3=财务预算）
   */
  budgetType: number;

  /**
   * 计量类型（字典 accounting_budget_measure_type；1=金额，2=数量）
   */
  measureType: number;

  /**
   * 本期预算金额（或数量，视 MeasureType）
   */
  budgetAmount: number;

  /**
   * 本期实绩金额（或数量）
   */
  actualAmount: number;

  /**
   * 本期差异金额（= 实绩 − 预算）
   */
  varianceAmount: number;

  /**
   * 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
   */
  variancePercent: number;

  /**
   * 上年同期实绩（比较分析）
   */
  priorPeriodActual: number;

  /**
   * 本年累计预算
   */
  ytdBudgetAmount: number;

  /**
   * 本年累计实绩
   */
  ytdActualAmount: number;

  /**
   * 本年累计差异（= 本年累计实绩 − 本年累计预算）
   */
  ytdVarianceAmount: number;

  /**
   * 币种（字典 accounting_currency_code；数量计量时可仍存报告币）
   */
  currencyCode: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  budgetActualStatus: number;

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
 * 更新BudgetActual DTO
 * 继承 TaktBudgetActualCreateDto，添加 BudgetActualId 字段
 * 对应前端 BudgetActualUpdate
 * @description 对应后端 TaktBudgetActualUpdateDto
 */
export interface BudgetActualUpdate extends BudgetActualCreate {
  /**
   * BudgetActualID（标识要更新的实体）
   */
  budgetActualId: string;

}


/**
 * BudgetActual 状态更新 DTO
 * 对应前端 BudgetActualStatus
 * @description 对应后端 TaktBudgetActualStatusDto
 */
export interface BudgetActualStatus {
  /**
   * BudgetActualID
   */
  budgetActualId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  budgetActualStatus: number;

}


/**
 * BudgetActual 排序更新 DTO
 * 对应前端 BudgetActualSort
 * @description 对应后端 TaktBudgetActualSortDto
 */
export interface BudgetActualSort {
  /**
   * BudgetActualID
   */
  budgetActualId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * BudgetActual 导入模板行 DTO
 * 对应前端 BudgetActualTemplate
 * @description 对应后端 TaktBudgetActualTemplateDto
 */
export interface BudgetActualTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode?: string;

  /**
   * 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
   */
  costCenterCode?: string;

  /**
   * 成本中心名称（冗余）
   */
  costCenterName?: string;

  /**
   * 预算项编码
   */
  budgetItemCode?: string;

  /**
   * 预算项名称
   */
  budgetItemName?: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 预算类型（字典 accounting_budget_type；1=经营预算，2=资本预算，3=财务预算）
   */
  budgetType?: number;

  /**
   * 计量类型（字典 accounting_budget_measure_type；1=金额，2=数量）
   */
  measureType?: number;

  /**
   * 本期预算金额（或数量，视 MeasureType）
   */
  budgetAmount?: number;

  /**
   * 本期实绩金额（或数量）
   */
  actualAmount?: number;

  /**
   * 本期差异金额（= 实绩 − 预算）
   */
  varianceAmount?: number;

  /**
   * 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
   */
  variancePercent?: number;

  /**
   * 上年同期实绩（比较分析）
   */
  priorPeriodActual?: number;

  /**
   * 本年累计预算
   */
  ytdBudgetAmount?: number;

  /**
   * 本年累计实绩
   */
  ytdActualAmount?: number;

  /**
   * 本年累计差异（= 本年累计实绩 − 本年累计预算）
   */
  ytdVarianceAmount?: number;

  /**
   * 币种（字典 accounting_currency_code；数量计量时可仍存报告币）
   */
  currencyCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  budgetActualStatus?: number;

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
 * BudgetActual 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BudgetActualImport
 * @description 对应后端 TaktBudgetActualImportDto
 */
export interface BudgetActualImport {
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
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode?: string;

  /**
   * 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
   */
  costCenterCode?: string;

  /**
   * 成本中心名称（冗余）
   */
  costCenterName?: string;

  /**
   * 预算项编码
   */
  budgetItemCode?: string;

  /**
   * 预算项名称
   */
  budgetItemName?: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 预算类型（字典 accounting_budget_type；1=经营预算，2=资本预算，3=财务预算）
   */
  budgetType?: number;

  /**
   * 计量类型（字典 accounting_budget_measure_type；1=金额，2=数量）
   */
  measureType?: number;

  /**
   * 本期预算金额（或数量，视 MeasureType）
   */
  budgetAmount?: number;

  /**
   * 本期实绩金额（或数量）
   */
  actualAmount?: number;

  /**
   * 本期差异金额（= 实绩 − 预算）
   */
  varianceAmount?: number;

  /**
   * 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
   */
  variancePercent?: number;

  /**
   * 上年同期实绩（比较分析）
   */
  priorPeriodActual?: number;

  /**
   * 本年累计预算
   */
  ytdBudgetAmount?: number;

  /**
   * 本年累计实绩
   */
  ytdActualAmount?: number;

  /**
   * 本年累计差异（= 本年累计实绩 − 本年累计预算）
   */
  ytdVarianceAmount?: number;

  /**
   * 币种（字典 accounting_currency_code；数量计量时可仍存报告币）
   */
  currencyCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  budgetActualStatus?: number;

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
 * BudgetActual 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BudgetActualExport
 * @description 对应后端 TaktBudgetActualExportDto
 */
export interface BudgetActualExport {
  /**
   * BudgetActualID
   */
  budgetActualId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode: string;

  /**
   * 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
   */
  costCenterCode: string;

  /**
   * 成本中心名称（冗余）
   */
  costCenterName?: string;

  /**
   * 预算项编码
   */
  budgetItemCode: string;

  /**
   * 预算项名称
   */
  budgetItemName: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 预算类型（字典 accounting_budget_type；1=经营预算，2=资本预算，3=财务预算）
   */
  budgetType: number;

  /**
   * 计量类型（字典 accounting_budget_measure_type；1=金额，2=数量）
   */
  measureType: number;

  /**
   * 本期预算金额（或数量，视 MeasureType）
   */
  budgetAmount: number;

  /**
   * 本期实绩金额（或数量）
   */
  actualAmount: number;

  /**
   * 本期差异金额（= 实绩 − 预算）
   */
  varianceAmount: number;

  /**
   * 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
   */
  variancePercent: number;

  /**
   * 上年同期实绩（比较分析）
   */
  priorPeriodActual: number;

  /**
   * 本年累计预算
   */
  ytdBudgetAmount: number;

  /**
   * 本年累计实绩
   */
  ytdActualAmount: number;

  /**
   * 本年累计差异（= 本年累计实绩 − 本年累计预算）
   */
  ytdVarianceAmount: number;

  /**
   * 币种（字典 accounting_currency_code；数量计量时可仍存报告币）
   */
  currencyCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  budgetActualStatus: number;

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

