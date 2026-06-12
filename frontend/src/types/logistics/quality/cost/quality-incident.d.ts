// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-incident.d.ts
// 创建时间：2026-06-09
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
 * 品质事故主表,用于记录废弃单的基础信息(年月日、机种)及汇总数据
 * 对应前端 TaktQualityIncidentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityIncident
 * @description 对应后端 TaktQualityIncidentDto
 */
export interface QualityIncident extends CompanyDtoBase {
  /**
   * QualityIncidentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityIncidentId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 品质事故编码(唯一,如:QI-2026-0001)
   */
  qualityIncidentCode: string;

  /**
   * 事故日期
   */
  incidentDate: string;

  /**
   * 间接人员费率(元/分钟)
   */
  indirectManpowerCostPerMinute: number;

  /**
   * 机种/产品型号
   */
  model: string;

  /**
   * 事故内容(废弃原因)
   */
  incidentReason?: string;

  /**
   * 废弃总数(自动计算 = 各子表废弃数量合计)
   */
  totalScrapQuantity: number;

  /**
   * 总废弃费用(元,自动计算 = 各子表费用合计)
   */
  totalScrapCost: number;

  /**
   * 成本币种(CNY/USD/JPY等)
   */
  costCurrency: string;

  /**
   * 事故明细列表 （子表：TaktQualityIncidentItem）
   */
  incidentItems?: QualityIncidentItem[];

}


/**
 * QualityIncident 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityIncidentQuery
 * @description 对应后端 TaktQualityIncidentQueryDto
 */
export interface QualityIncidentQuery extends TaktPagedQuery {
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
   * 品质事故编码(唯一,如:QI-2026-0001)
   */
  qualityIncidentCode?: string;

  /**
   * 事故日期（范围查询-开始）
   */
  incidentDateStart?: string;

  /**
   * 事故日期（范围查询-结束）
   */
  incidentDateEnd?: string;

  /**
   * 间接人员费率(元/分钟)
   */
  indirectManpowerCostPerMinute?: number;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 事故内容(废弃原因)
   */
  incidentReason?: string;

  /**
   * 废弃总数(自动计算 = 各子表废弃数量合计)
   */
  totalScrapQuantity?: number;

  /**
   * 总废弃费用(元,自动计算 = 各子表费用合计)
   */
  totalScrapCost?: number;

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
 * 创建QualityIncident DTO
 * 对应前端 QualityIncidentCreate
 * @description 对应后端 TaktQualityIncidentCreateDto
 */
export interface QualityIncidentCreate {
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
   * 品质事故编码(唯一,如:QI-2026-0001)
   */
  qualityIncidentCode: string;

  /**
   * 事故日期
   */
  incidentDate: string;

  /**
   * 间接人员费率(元/分钟)
   */
  indirectManpowerCostPerMinute: number;

  /**
   * 机种/产品型号
   */
  model: string;

  /**
   * 事故内容(废弃原因)
   */
  incidentReason?: string;

  /**
   * 废弃总数(自动计算 = 各子表废弃数量合计)
   */
  totalScrapQuantity: number;

  /**
   * 总废弃费用(元,自动计算 = 各子表费用合计)
   */
  totalScrapCost: number;

  /**
   * 成本币种(CNY/USD/JPY等)
   */
  costCurrency: string;

  /**
   * 事故明细列表（子表，级联保存）
   */
  incidentItems?: QualityIncidentItemCreate[];

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
 * 更新QualityIncident DTO
 * 继承 TaktQualityIncidentCreateDto，添加 QualityIncidentId 字段
 * 对应前端 QualityIncidentUpdate
 * @description 对应后端 TaktQualityIncidentUpdateDto
 */
export interface QualityIncidentUpdate extends QualityIncidentCreate {
  /**
   * QualityIncidentID（标识要更新的实体）
   */
  qualityIncidentId: string;

}


/**
 * QualityIncident 导入模板行 DTO
 * 对应前端 QualityIncidentTemplate
 * @description 对应后端 TaktQualityIncidentTemplateDto
 */
export interface QualityIncidentTemplate {
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
   * 品质事故编码(唯一,如:QI-2026-0001)
   */
  qualityIncidentCode?: string;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 事故内容(废弃原因)
   */
  incidentReason?: string;

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
 * QualityIncident 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityIncidentImport
 * @description 对应后端 TaktQualityIncidentImportDto
 */
export interface QualityIncidentImport {
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
   * 品质事故编码(唯一,如:QI-2026-0001)
   */
  qualityIncidentCode?: string;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 事故内容(废弃原因)
   */
  incidentReason?: string;

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
 * QualityIncident 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityIncidentExport
 * @description 对应后端 TaktQualityIncidentExportDto
 */
export interface QualityIncidentExport {
  /**
   * QualityIncidentID
   */
  qualityIncidentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 品质事故编码(唯一,如:QI-2026-0001)
   */
  qualityIncidentCode: string;

  /**
   * 事故日期
   */
  incidentDate: string;

  /**
   * 间接人员费率(元/分钟)
   */
  indirectManpowerCostPerMinute: number;

  /**
   * 机种/产品型号
   */
  model: string;

  /**
   * 事故内容(废弃原因)
   */
  incidentReason?: string;

  /**
   * 废弃总数(自动计算 = 各子表废弃数量合计)
   */
  totalScrapQuantity: number;

  /**
   * 总废弃费用(元,自动计算 = 各子表费用合计)
   */
  totalScrapCost: number;

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

