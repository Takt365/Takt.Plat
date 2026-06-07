// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-operation.d.ts
// 创建时间：2026-06-07
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
 * 对应前端 TaktQualityOperationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityOperation
 * @description 对应后端 TaktQualityOperationDto
 */
export interface QualityOperation extends CompanyDtoBase {
  /**
   * QualityOperationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityOperationId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityOperationCode: string;

  /**
   * 业务年月(格式:2026-05)
   */
  operationMonth: string;

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
   * 来料检验费用明细列表 （子表：TaktQualityOperationIncoming）
   */
  incomingItems?: QualityOperationIncoming[];

  /**
   * 初期/定期检定费用明细列表 （子表：TaktQualityOperationFirstArticle）
   */
  firstArticleItems?: QualityOperationFirstArticle[];

  /**
   * 设备校正费用明细列表 （子表：TaktQualityOperationCalibration）
   */
  calibrationItems?: QualityOperationCalibration[];

  /**
   * 其他通常业务费用明细列表 （子表：TaktQualityOperationOther）
   */
  otherItems?: QualityOperationOther[];

  /**
   * 出货检验费用明细列表 （子表：TaktQualityOperationOutgoing）
   */
  outgoingItems?: QualityOperationOutgoing[];

  /**
   * 信赖性评价/ORT费用明细列表 （子表：TaktQualityOperationReliability）
   */
  reliabilityItems?: QualityOperationReliability[];

  /**
   * 顾客品质要求对应费用明细列表 （子表：TaktQualityOperationCustomerResponse）
   */
  customerResponseItems?: QualityOperationCustomerResponse[];

}


/**
 * QualityOperation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityOperationQuery
 * @description 对应后端 TaktQualityOperationQueryDto
 */
export interface QualityOperationQuery extends TaktPagedQuery {
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
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityOperationCode?: string;

  /**
   * 业务年月(格式:2026-05)
   */
  operationMonth?: string;

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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建QualityOperation DTO
 * 对应前端 QualityOperationCreate
 * @description 对应后端 TaktQualityOperationCreateDto
 */
export interface QualityOperationCreate {
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
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityOperationCode: string;

  /**
   * 业务年月(格式:2026-05)
   */
  operationMonth: string;

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
  incomingItems?: QualityOperationIncomingCreate[];

  /**
   * 初期/定期检定费用明细列表（子表，级联保存）
   */
  firstArticleItems?: QualityOperationFirstArticleCreate[];

  /**
   * 设备校正费用明细列表（子表，级联保存）
   */
  calibrationItems?: QualityOperationCalibrationCreate[];

  /**
   * 其他通常业务费用明细列表（子表，级联保存）
   */
  otherItems?: QualityOperationOtherCreate[];

  /**
   * 出货检验费用明细列表（子表，级联保存）
   */
  outgoingItems?: QualityOperationOutgoingCreate[];

  /**
   * 信赖性评价/ORT费用明细列表（子表，级联保存）
   */
  reliabilityItems?: QualityOperationReliabilityCreate[];

  /**
   * 顾客品质要求对应费用明细列表（子表，级联保存）
   */
  customerResponseItems?: QualityOperationCustomerResponseCreate[];

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
 * 更新QualityOperation DTO
 * 继承 TaktQualityOperationCreateDto，添加 QualityOperationId 字段
 * 对应前端 QualityOperationUpdate
 * @description 对应后端 TaktQualityOperationUpdateDto
 */
export interface QualityOperationUpdate extends QualityOperationCreate {
  /**
   * QualityOperationID（标识要更新的实体）
   */
  qualityOperationId: string;

}


/**
 * QualityOperation 导入模板行 DTO
 * 对应前端 QualityOperationTemplate
 * @description 对应后端 TaktQualityOperationTemplateDto
 */
export interface QualityOperationTemplate {
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
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityOperationCode?: string;

  /**
   * 业务年月(格式:2026-05)
   */
  operationMonth?: string;

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
   * 成本币种(CNY/USD/JPY等)
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
 * QualityOperation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityOperationImport
 * @description 对应后端 TaktQualityOperationImportDto
 */
export interface QualityOperationImport {
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
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityOperationCode?: string;

  /**
   * 业务年月(格式:2026-05)
   */
  operationMonth?: string;

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
   * 成本币种(CNY/USD/JPY等)
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
 * QualityOperation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityOperationExport
 * @description 对应后端 TaktQualityOperationExportDto
 */
export interface QualityOperationExport {
  /**
   * QualityOperationID
   */
  qualityOperationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityOperationCode: string;

  /**
   * 业务年月(格式:2026-05)
   */
  operationMonth: string;

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

