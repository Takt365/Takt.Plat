// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：issue.d.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 品质问题应对主表,用于记录质量问题的基础信息(年月日、机种、批次)及汇总数据
 * 对应前端 TaktQualityIssueDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityIssue
 * @description 对应后端 TaktQualityIssueDto
 */
export interface QualityIssue extends CompanyDtoBase {
  /**
   * QualityIssueID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityIssueId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityIssueCode: string;

  /**
   * 问题日期
   */
  issueDate: string;

  /**
   * 机种/产品型号
   */
  model: string;

  /**
   * 批次号/Lot No
   */
  lot: string;

  /**
   * 品质问题应对摘要(汇总说明)
   */
  qualityProblemsResponse?: string;

  /**
   * 不良改修应对摘要(汇总说明)
   */
  reworkDueToDefects?: string;

  /**
   * 是否需要不良改修应对(Y/N)
   */
  needRework?: string;

  /**
   * 总时间(分钟,自动计算 = 各子表时间合计)
   */
  totalTimeMinutes: number;

  /**
   * 总费用(元,自动计算 = 各子表费用合计)
   */
  totalCost: number;

  /**
   * 成本币种（CNY/USD/JPY等）
   */
  costCurrency: string;

  /**
   * 会议/调查/试验费用明细列表 （子表：TaktQualityIssueMeeting）
   */
  meetingItems?: QualityIssueMeeting[];

  /**
   * 组装不良改修应对明细列表 （子表：TaktQualityIssueAssyRework）
   */
  assyReworkItems?: QualityIssueAssyRework[];

  /**
   * PCBA不良改修应对明细列表 （子表：TaktQualityIssuePcbaRework）
   */
  pcbaReworkItems?: QualityIssuePcbaRework[];

}


/**
 * QualityIssue 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityIssueQuery
 * @description 对应后端 TaktQualityIssueQueryDto
 */
export interface QualityIssueQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityIssueCode?: string;

  /**
   * 问题日期（范围查询-开始）
   */
  issueDateStart?: string;

  /**
   * 问题日期（范围查询-结束）
   */
  issueDateEnd?: string;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 批次号/Lot No
   */
  lot?: string;

  /**
   * 品质问题应对摘要(汇总说明)
   */
  qualityProblemsResponse?: string;

  /**
   * 不良改修应对摘要(汇总说明)
   */
  reworkDueToDefects?: string;

  /**
   * 是否需要不良改修应对(Y/N)
   */
  needRework?: string;

  /**
   * 总时间(分钟,自动计算 = 各子表时间合计)
   */
  totalTimeMinutes?: number;

  /**
   * 总费用(元,自动计算 = 各子表费用合计)
   */
  totalCost?: number;

  /**
   * 成本币种（CNY/USD/JPY等）
   */
  costCurrency?: string;

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
 * 创建QualityIssue DTO
 * 对应前端 QualityIssueCreate
 * @description 对应后端 TaktQualityIssueCreateDto
 */
export interface QualityIssueCreate {
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
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityIssueCode: string;

  /**
   * 问题日期
   */
  issueDate: string;

  /**
   * 机种/产品型号
   */
  model: string;

  /**
   * 批次号/Lot No
   */
  lot: string;

  /**
   * 品质问题应对摘要(汇总说明)
   */
  qualityProblemsResponse?: string;

  /**
   * 不良改修应对摘要(汇总说明)
   */
  reworkDueToDefects?: string;

  /**
   * 是否需要不良改修应对(Y/N)
   */
  needRework?: string;

  /**
   * 总时间(分钟,自动计算 = 各子表时间合计)
   */
  totalTimeMinutes: number;

  /**
   * 总费用(元,自动计算 = 各子表费用合计)
   */
  totalCost: number;

  /**
   * 成本币种（CNY/USD/JPY等）
   */
  costCurrency: string;

  /**
   * 会议/调查/试验费用明细列表（子表，级联保存）
   */
  meetingItems?: QualityIssueMeetingCreate[];

  /**
   * 组装不良改修应对明细列表（子表，级联保存）
   */
  assyReworkItems?: QualityIssueAssyReworkCreate[];

  /**
   * PCBA不良改修应对明细列表（子表，级联保存）
   */
  pcbaReworkItems?: QualityIssuePcbaReworkCreate[];

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
 * 更新QualityIssue DTO
 * 继承 TaktQualityIssueCreateDto，添加 QualityIssueId 字段
 * 对应前端 QualityIssueUpdate
 * @description 对应后端 TaktQualityIssueUpdateDto
 */
export interface QualityIssueUpdate extends QualityIssueCreate {
  /**
   * QualityIssueID（标识要更新的实体）
   */
  qualityIssueId: string;

}


/**
 * QualityIssue 导入模板行 DTO
 * 对应前端 QualityIssueTemplate
 * @description 对应后端 TaktQualityIssueTemplateDto
 */
export interface QualityIssueTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityIssueCode?: string;

  /**
   * 问题日期
   */
  issueDate?: string;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 批次号/Lot No
   */
  lot?: string;

  /**
   * 品质问题应对摘要(汇总说明)
   */
  qualityProblemsResponse?: string;

  /**
   * 不良改修应对摘要(汇总说明)
   */
  reworkDueToDefects?: string;

  /**
   * 是否需要不良改修应对(Y/N)
   */
  needRework?: string;

  /**
   * 总时间(分钟,自动计算 = 各子表时间合计)
   */
  totalTimeMinutes?: number;

  /**
   * 总费用(元,自动计算 = 各子表费用合计)
   */
  totalCost?: number;

  /**
   * 成本币种（CNY/USD/JPY等）
   */
  costCurrency?: string;

  /**
   * 会议/调查/试验费用明细列表（子表，级联保存）
   */
  meetingItems?: QualityIssueMeetingCreate[];

  /**
   * 组装不良改修应对明细列表（子表，级联保存）
   */
  assyReworkItems?: QualityIssueAssyReworkCreate[];

  /**
   * PCBA不良改修应对明细列表（子表，级联保存）
   */
  pcbaReworkItems?: QualityIssuePcbaReworkCreate[];

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
 * QualityIssue 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityIssueImport
 * @description 对应后端 TaktQualityIssueImportDto
 */
export interface QualityIssueImport {
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
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityIssueCode?: string;

  /**
   * 问题日期
   */
  issueDate?: string;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 批次号/Lot No
   */
  lot?: string;

  /**
   * 品质问题应对摘要(汇总说明)
   */
  qualityProblemsResponse?: string;

  /**
   * 不良改修应对摘要(汇总说明)
   */
  reworkDueToDefects?: string;

  /**
   * 是否需要不良改修应对(Y/N)
   */
  needRework?: string;

  /**
   * 总时间(分钟,自动计算 = 各子表时间合计)
   */
  totalTimeMinutes?: number;

  /**
   * 总费用(元,自动计算 = 各子表费用合计)
   */
  totalCost?: number;

  /**
   * 成本币种（CNY/USD/JPY等）
   */
  costCurrency?: string;

  /**
   * 会议/调查/试验费用明细列表（子表，级联保存）
   */
  meetingItems?: QualityIssueMeetingCreate[];

  /**
   * 组装不良改修应对明细列表（子表，级联保存）
   */
  assyReworkItems?: QualityIssueAssyReworkCreate[];

  /**
   * PCBA不良改修应对明细列表（子表，级联保存）
   */
  pcbaReworkItems?: QualityIssuePcbaReworkCreate[];

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
 * QualityIssue 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityIssueExport
 * @description 对应后端 TaktQualityIssueExportDto
 */
export interface QualityIssueExport {
  /**
   * QualityIssueID
   */
  qualityIssueId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityIssueCode: string;

  /**
   * 问题日期
   */
  issueDate: string;

  /**
   * 机种/产品型号
   */
  model: string;

  /**
   * 批次号/Lot No
   */
  lot: string;

  /**
   * 品质问题应对摘要(汇总说明)
   */
  qualityProblemsResponse?: string;

  /**
   * 不良改修应对摘要(汇总说明)
   */
  reworkDueToDefects?: string;

  /**
   * 是否需要不良改修应对(Y/N)
   */
  needRework?: string;

  /**
   * 总时间(分钟,自动计算 = 各子表时间合计)
   */
  totalTimeMinutes: number;

  /**
   * 总费用(元,自动计算 = 各子表费用合计)
   */
  totalCost: number;

  /**
   * 成本币种（CNY/USD/JPY等）
   */
  costCurrency: string;

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

