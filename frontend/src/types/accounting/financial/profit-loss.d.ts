// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：profit-loss.d.ts
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
 * 利润表（及综合收益）行实体（CAS 利润表列报 / IAS 1 Statement of Profit or Loss and OCI） 列报层次：收入→成本费用→营业利润→利润总额→所得税→净利润→其他综合收益→综合收益总额。 唯一键：租户 + 公司 + 工厂 + 期间 + 报表项目编码
 * 对应前端 TaktProfitLossDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProfitLoss
 * @description 对应后端 TaktProfitLossDto
 */
export interface ProfitLoss extends CompanyDtoBase {
  /**
   * ProfitLossID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  profitLossId: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 会计期间编码（YYYYMM；利润表报告期）
   */
  periodCode: string;

  /**
   * 报表项目编码（利润表/综合收益表行项目）
   */
  statementLineCode: string;

  /**
   * 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
   */
  statementLineName: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
   */
  lineCategory: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
   */
  isTotalLine: number;

  /**
   * 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
   */
  periodAmount: number;

  /**
   * 上期金额（比较信息；CAS/IAS 1）
   */
  priorPeriodAmount: number;

  /**
   * 本年累计金额（中国利润表常见列；自财年期初至本期末）
   */
  yearToDateAmount: number;

  /**
   * 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
   */
  isExpense: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  profitLossStatus: number;

}


/**
 * ProfitLoss 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProfitLossQuery
 * @description 对应后端 TaktProfitLossQueryDto
 */
export interface ProfitLossQuery extends TaktPagedQuery {
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
   * 会计期间编码（YYYYMM；利润表报告期）
   */
  periodCode?: string;

  /**
   * 报表项目编码（利润表/综合收益表行项目）
   */
  statementLineCode?: string;

  /**
   * 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
   */
  statementLineName?: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
   */
  lineCategory?: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
   */
  isTotalLine?: number;

  /**
   * 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
   */
  periodAmount?: number;

  /**
   * 上期金额（比较信息；CAS/IAS 1）
   */
  priorPeriodAmount?: number;

  /**
   * 本年累计金额（中国利润表常见列；自财年期初至本期末）
   */
  yearToDateAmount?: number;

  /**
   * 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
   */
  isExpense?: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  profitLossStatus?: number;

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
 * 创建ProfitLoss DTO
 * 对应前端 ProfitLossCreate
 * @description 对应后端 TaktProfitLossCreateDto
 */
export interface ProfitLossCreate {
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
   * 会计期间编码（YYYYMM；利润表报告期）
   */
  periodCode: string;

  /**
   * 报表项目编码（利润表/综合收益表行项目）
   */
  statementLineCode: string;

  /**
   * 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
   */
  statementLineName: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
   */
  lineCategory: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
   */
  isTotalLine: number;

  /**
   * 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
   */
  periodAmount: number;

  /**
   * 上期金额（比较信息；CAS/IAS 1）
   */
  priorPeriodAmount: number;

  /**
   * 本年累计金额（中国利润表常见列；自财年期初至本期末）
   */
  yearToDateAmount: number;

  /**
   * 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
   */
  isExpense: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  profitLossStatus: number;

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
 * 更新ProfitLoss DTO
 * 继承 TaktProfitLossCreateDto，添加 ProfitLossId 字段
 * 对应前端 ProfitLossUpdate
 * @description 对应后端 TaktProfitLossUpdateDto
 */
export interface ProfitLossUpdate extends ProfitLossCreate {
  /**
   * ProfitLossID（标识要更新的实体）
   */
  profitLossId: string;

}


/**
 * ProfitLoss 状态更新 DTO
 * 对应前端 ProfitLossStatus
 * @description 对应后端 TaktProfitLossStatusDto
 */
export interface ProfitLossStatus {
  /**
   * ProfitLossID
   */
  profitLossId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  profitLossStatus: number;

}


/**
 * ProfitLoss 排序更新 DTO
 * 对应前端 ProfitLossSort
 * @description 对应后端 TaktProfitLossSortDto
 */
export interface ProfitLossSort {
  /**
   * ProfitLossID
   */
  profitLossId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * ProfitLoss 导入模板行 DTO
 * 对应前端 ProfitLossTemplate
 * @description 对应后端 TaktProfitLossTemplateDto
 */
export interface ProfitLossTemplate {
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
   * 会计期间编码（YYYYMM；利润表报告期）
   */
  periodCode?: string;

  /**
   * 报表项目编码（利润表/综合收益表行项目）
   */
  statementLineCode?: string;

  /**
   * 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
   */
  statementLineName?: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
   */
  lineCategory?: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
   */
  isTotalLine?: number;

  /**
   * 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
   */
  periodAmount?: number;

  /**
   * 上期金额（比较信息；CAS/IAS 1）
   */
  priorPeriodAmount?: number;

  /**
   * 本年累计金额（中国利润表常见列；自财年期初至本期末）
   */
  yearToDateAmount?: number;

  /**
   * 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
   */
  isExpense?: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  profitLossStatus?: number;

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
 * ProfitLoss 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProfitLossImport
 * @description 对应后端 TaktProfitLossImportDto
 */
export interface ProfitLossImport {
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
   * 会计期间编码（YYYYMM；利润表报告期）
   */
  periodCode?: string;

  /**
   * 报表项目编码（利润表/综合收益表行项目）
   */
  statementLineCode?: string;

  /**
   * 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
   */
  statementLineName?: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
   */
  lineCategory?: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
   */
  isTotalLine?: number;

  /**
   * 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
   */
  periodAmount?: number;

  /**
   * 上期金额（比较信息；CAS/IAS 1）
   */
  priorPeriodAmount?: number;

  /**
   * 本年累计金额（中国利润表常见列；自财年期初至本期末）
   */
  yearToDateAmount?: number;

  /**
   * 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
   */
  isExpense?: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  profitLossStatus?: number;

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
 * ProfitLoss 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProfitLossExport
 * @description 对应后端 TaktProfitLossExportDto
 */
export interface ProfitLossExport {
  /**
   * ProfitLossID
   */
  profitLossId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 会计期间编码（YYYYMM；利润表报告期）
   */
  periodCode: string;

  /**
   * 报表项目编码（利润表/综合收益表行项目）
   */
  statementLineCode: string;

  /**
   * 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
   */
  statementLineName: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
   */
  lineCategory: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
   */
  isTotalLine: number;

  /**
   * 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
   */
  periodAmount: number;

  /**
   * 上期金额（比较信息；CAS/IAS 1）
   */
  priorPeriodAmount: number;

  /**
   * 本年累计金额（中国利润表常见列；自财年期初至本期末）
   */
  yearToDateAmount: number;

  /**
   * 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
   */
  isExpense: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  profitLossStatus: number;

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

