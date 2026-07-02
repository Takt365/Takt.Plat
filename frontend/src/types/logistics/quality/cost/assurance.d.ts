// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：assurance.d.ts
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
 * 品质业务主表,用于记录品质业务的基础信息(年月、顾客)及汇总数据
 * 对应前端 TaktQualityAssuranceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityAssurance
 * @description 对应后端 TaktQualityAssuranceDto
 */
export interface QualityAssurance extends CompanyDtoBase {
  /**
   * QualityAssuranceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityAssuranceId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityAssuranceCode: string;

  /**
   * 业务年月(格式:2026-05)
   */
  assuranceMonth: string;

  /**
   * 顾客名
   */
  customerName?: string;

  /**
   * Debit Note No
   */
  debitNoteNo?: string;

  /**
   * 记录者
   */
  recorder?: string;

  /**
   * 质量总成本(元,自动计算 = 各子表费用合计)
   */
  totalQualityCost: number;

  /**
   * 成本币种(CNY/USD/JPY等)
   */
  costCurrency: string;

  /**
   * 来料检验费用明细列表 （子表：TaktQualityAssuranceIncoming）
   */
  incomingItems?: QualityAssuranceIncoming[];

  /**
   * 初期/定期检定费用明细列表 （子表：TaktQualityAssuranceFirstArticle）
   */
  firstArticleItems?: QualityAssuranceFirstArticle[];

  /**
   * 设备校正费用明细列表 （子表：TaktQualityAssuranceCalibration）
   */
  calibrationItems?: QualityAssuranceCalibration[];

  /**
   * 其他通常业务费用明细列表 （子表：TaktQualityAssuranceOther）
   */
  otherItems?: QualityAssuranceOther[];

  /**
   * 出货检验费用明细列表 （子表：TaktQualityAssuranceOutgoing）
   */
  outgoingItems?: QualityAssuranceOutgoing[];

  /**
   * 信赖性评价/ORT费用明细列表 （子表：TaktQualityAssuranceReliability）
   */
  reliabilityItems?: QualityAssuranceReliability[];

  /**
   * 顾客品质要求对应费用明细列表 （子表：TaktQualityAssuranceCustomerResponse）
   */
  customerResponseItems?: QualityAssuranceCustomerResponse[];

}


/**
 * QualityAssurance 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityAssuranceQuery
 * @description 对应后端 TaktQualityAssuranceQueryDto
 */
export interface QualityAssuranceQuery extends TaktPagedQuery {
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
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityAssuranceCode?: string;

  /**
   * 业务年月(格式:2026-05)
   */
  assuranceMonth?: string;

  /**
   * 顾客名
   */
  customerName?: string;

  /**
   * Debit Note No
   */
  debitNoteNo?: string;

  /**
   * 记录者
   */
  recorder?: string;

  /**
   * 质量总成本(元,自动计算 = 各子表费用合计)
   */
  totalQualityCost?: number;

  /**
   * 成本币种(CNY/USD/JPY等)
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
 * 创建QualityAssurance DTO
 * 对应前端 QualityAssuranceCreate
 * @description 对应后端 TaktQualityAssuranceCreateDto
 */
export interface QualityAssuranceCreate {
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
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityAssuranceCode: string;

  /**
   * 业务年月(格式:2026-05)
   */
  assuranceMonth: string;

  /**
   * 顾客名
   */
  customerName?: string;

  /**
   * Debit Note No
   */
  debitNoteNo?: string;

  /**
   * 记录者
   */
  recorder?: string;

  /**
   * 质量总成本(元,自动计算 = 各子表费用合计)
   */
  totalQualityCost: number;

  /**
   * 成本币种(CNY/USD/JPY等)
   */
  costCurrency: string;

  /**
   * 来料检验费用明细列表（子表，级联保存）
   */
  incomingItems?: QualityAssuranceIncomingCreate[];

  /**
   * 初期/定期检定费用明细列表（子表，级联保存）
   */
  firstArticleItems?: QualityAssuranceFirstArticleCreate[];

  /**
   * 设备校正费用明细列表（子表，级联保存）
   */
  calibrationItems?: QualityAssuranceCalibrationCreate[];

  /**
   * 其他通常业务费用明细列表（子表，级联保存）
   */
  otherItems?: QualityAssuranceOtherCreate[];

  /**
   * 出货检验费用明细列表（子表，级联保存）
   */
  outgoingItems?: QualityAssuranceOutgoingCreate[];

  /**
   * 信赖性评价/ORT费用明细列表（子表，级联保存）
   */
  reliabilityItems?: QualityAssuranceReliabilityCreate[];

  /**
   * 顾客品质要求对应费用明细列表（子表，级联保存）
   */
  customerResponseItems?: QualityAssuranceCustomerResponseCreate[];

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
 * 更新QualityAssurance DTO
 * 继承 TaktQualityAssuranceCreateDto，添加 QualityAssuranceId 字段
 * 对应前端 QualityAssuranceUpdate
 * @description 对应后端 TaktQualityAssuranceUpdateDto
 */
export interface QualityAssuranceUpdate extends QualityAssuranceCreate {
  /**
   * QualityAssuranceID（标识要更新的实体）
   */
  qualityAssuranceId: string;

}


/**
 * QualityAssurance 导入模板行 DTO
 * 对应前端 QualityAssuranceTemplate
 * @description 对应后端 TaktQualityAssuranceTemplateDto
 */
export interface QualityAssuranceTemplate {
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
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityAssuranceCode?: string;

  /**
   * 业务年月(格式:2026-05)
   */
  assuranceMonth?: string;

  /**
   * 顾客名
   */
  customerName?: string;

  /**
   * Debit Note No
   */
  debitNoteNo?: string;

  /**
   * 记录者
   */
  recorder?: string;

  /**
   * 质量总成本(元,自动计算 = 各子表费用合计)
   */
  totalQualityCost?: number;

  /**
   * 成本币种(CNY/USD/JPY等)
   */
  costCurrency?: string;

  /**
   * 来料检验费用明细列表（子表，级联保存）
   */
  incomingItems?: QualityAssuranceIncomingCreate[];

  /**
   * 初期/定期检定费用明细列表（子表，级联保存）
   */
  firstArticleItems?: QualityAssuranceFirstArticleCreate[];

  /**
   * 设备校正费用明细列表（子表，级联保存）
   */
  calibrationItems?: QualityAssuranceCalibrationCreate[];

  /**
   * 其他通常业务费用明细列表（子表，级联保存）
   */
  otherItems?: QualityAssuranceOtherCreate[];

  /**
   * 出货检验费用明细列表（子表，级联保存）
   */
  outgoingItems?: QualityAssuranceOutgoingCreate[];

  /**
   * 信赖性评价/ORT费用明细列表（子表，级联保存）
   */
  reliabilityItems?: QualityAssuranceReliabilityCreate[];

  /**
   * 顾客品质要求对应费用明细列表（子表，级联保存）
   */
  customerResponseItems?: QualityAssuranceCustomerResponseCreate[];

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
 * QualityAssurance 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityAssuranceImport
 * @description 对应后端 TaktQualityAssuranceImportDto
 */
export interface QualityAssuranceImport {
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
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityAssuranceCode?: string;

  /**
   * 业务年月(格式:2026-05)
   */
  assuranceMonth?: string;

  /**
   * 顾客名
   */
  customerName?: string;

  /**
   * Debit Note No
   */
  debitNoteNo?: string;

  /**
   * 记录者
   */
  recorder?: string;

  /**
   * 质量总成本(元,自动计算 = 各子表费用合计)
   */
  totalQualityCost?: number;

  /**
   * 成本币种(CNY/USD/JPY等)
   */
  costCurrency?: string;

  /**
   * 来料检验费用明细列表（子表，级联保存）
   */
  incomingItems?: QualityAssuranceIncomingCreate[];

  /**
   * 初期/定期检定费用明细列表（子表，级联保存）
   */
  firstArticleItems?: QualityAssuranceFirstArticleCreate[];

  /**
   * 设备校正费用明细列表（子表，级联保存）
   */
  calibrationItems?: QualityAssuranceCalibrationCreate[];

  /**
   * 其他通常业务费用明细列表（子表，级联保存）
   */
  otherItems?: QualityAssuranceOtherCreate[];

  /**
   * 出货检验费用明细列表（子表，级联保存）
   */
  outgoingItems?: QualityAssuranceOutgoingCreate[];

  /**
   * 信赖性评价/ORT费用明细列表（子表，级联保存）
   */
  reliabilityItems?: QualityAssuranceReliabilityCreate[];

  /**
   * 顾客品质要求对应费用明细列表（子表，级联保存）
   */
  customerResponseItems?: QualityAssuranceCustomerResponseCreate[];

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
 * QualityAssurance 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityAssuranceExport
 * @description 对应后端 TaktQualityAssuranceExportDto
 */
export interface QualityAssuranceExport {
  /**
   * QualityAssuranceID
   */
  qualityAssuranceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityAssuranceCode: string;

  /**
   * 业务年月(格式:2026-05)
   */
  assuranceMonth: string;

  /**
   * 顾客名
   */
  customerName?: string;

  /**
   * Debit Note No
   */
  debitNoteNo?: string;

  /**
   * 记录者
   */
  recorder?: string;

  /**
   * 质量总成本(元,自动计算 = 各子表费用合计)
   */
  totalQualityCost: number;

  /**
   * 成本币种(CNY/USD/JPY等)
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

