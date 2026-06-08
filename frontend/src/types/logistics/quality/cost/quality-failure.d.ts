// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-failure.d.ts
// 创建时间：2026-06-08
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
 * 对应前端 TaktQualityFailureDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityFailure
 * @description 对应后端 TaktQualityFailureDto
 */
export interface QualityFailure extends CompanyDtoBase {
  /**
   * QualityFailureID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityFailureId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityFailureCode: string;

  /**
   * 问题日期
   */
  failureDate: string;

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
   * 会议/调查/试验费用明细列表 （子表：TaktQualityFailureMeeting）
   */
  meetingItems?: QualityFailureMeeting[];

  /**
   * 组装不良改修应对明细列表 （子表：TaktQualityFailureAssyRework）
   */
  assyReworkItems?: QualityFailureAssyRework[];

  /**
   * PCBA不良改修应对明细列表 （子表：TaktQualityFailurePcbaRework）
   */
  pcbaReworkItems?: QualityFailurePcbaRework[];

}


/**
 * QualityFailure 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityFailureQuery
 * @description 对应后端 TaktQualityFailureQueryDto
 */
export interface QualityFailureQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityFailureCode?: string;

  /**
   * 问题日期（范围查询-开始）
   */
  failureDateStart?: string;

  /**
   * 问题日期（范围查询-结束）
   */
  failureDateEnd?: string;

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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建QualityFailure DTO
 * 对应前端 QualityFailureCreate
 * @description 对应后端 TaktQualityFailureCreateDto
 */
export interface QualityFailureCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityFailureCode: string;

  /**
   * 问题日期
   */
  failureDate: string;

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
  meetingItems?: QualityFailureMeetingCreate[];

  /**
   * 组装不良改修应对明细列表（子表，级联保存）
   */
  assyReworkItems?: QualityFailureAssyReworkCreate[];

  /**
   * PCBA不良改修应对明细列表（子表，级联保存）
   */
  pcbaReworkItems?: QualityFailurePcbaReworkCreate[];

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
 * 更新QualityFailure DTO
 * 继承 TaktQualityFailureCreateDto，添加 QualityFailureId 字段
 * 对应前端 QualityFailureUpdate
 * @description 对应后端 TaktQualityFailureUpdateDto
 */
export interface QualityFailureUpdate extends QualityFailureCreate {
  /**
   * QualityFailureID（标识要更新的实体）
   */
  qualityFailureId: string;

}


/**
 * QualityFailure 导入模板行 DTO
 * 对应前端 QualityFailureTemplate
 * @description 对应后端 TaktQualityFailureTemplateDto
 */
export interface QualityFailureTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityFailureCode?: string;

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
   * 成本币种（CNY/USD/JPY等）
   */
  costCurrency?: string;

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
 * QualityFailure 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityFailureImport
 * @description 对应后端 TaktQualityFailureImportDto
 */
export interface QualityFailureImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityFailureCode?: string;

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
   * 成本币种（CNY/USD/JPY等）
   */
  costCurrency?: string;

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
 * QualityFailure 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityFailureExport
 * @description 对应后端 TaktQualityFailureExportDto
 */
export interface QualityFailureExport {
  /**
   * QualityFailureID
   */
  qualityFailureId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityFailureCode: string;

  /**
   * 问题日期
   */
  failureDate: string;

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

