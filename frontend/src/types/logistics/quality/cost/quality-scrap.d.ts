// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-scrap.d.ts
// 创建时间：2026-06-05
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
 * 品质废弃主表,用于记录废弃单的基础信息(年月日、机种)及汇总数据
 * 对应前端 TaktQualityScrapDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityScrap
 * @description 对应后端 TaktQualityScrapDto
 */
export interface QualityScrap extends CompanyDtoBase {
  /**
   * QualityScrapID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityScrapId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 品质废弃编码(唯一,如:QS-2026-0001)
   */
  qualityScrapCode: string;

  /**
   * 废弃日期
   */
  scrapDate: string;

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
  scrapReason?: string;

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
   * 废弃明细列表 （子表：TaktQualityScrapItem）
   */
  scrapItems?: QualityScrapItem[];

}


/**
 * QualityScrap 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityScrapQuery
 * @description 对应后端 TaktQualityScrapQueryDto
 */
export interface QualityScrapQuery extends TaktPagedQuery {
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
   * 品质废弃编码(唯一,如:QS-2026-0001)
   */
  qualityScrapCode?: string;

  /**
   * 废弃日期（范围查询-开始）
   */
  scrapDateStart?: string;

  /**
   * 废弃日期（范围查询-结束）
   */
  scrapDateEnd?: string;

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
  scrapReason?: string;

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
 * 创建QualityScrap DTO
 * 对应前端 QualityScrapCreate
 * @description 对应后端 TaktQualityScrapCreateDto
 */
export interface QualityScrapCreate {
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
   * 品质废弃编码(唯一,如:QS-2026-0001)
   */
  qualityScrapCode: string;

  /**
   * 废弃日期
   */
  scrapDate: string;

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
  scrapReason?: string;

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
   * 废弃明细列表（子表，级联保存）
   */
  scrapItems?: QualityScrapItemCreate[];

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
 * 更新QualityScrap DTO
 * 继承 TaktQualityScrapCreateDto，添加 QualityScrapId 字段
 * 对应前端 QualityScrapUpdate
 * @description 对应后端 TaktQualityScrapUpdateDto
 */
export interface QualityScrapUpdate extends QualityScrapCreate {
  /**
   * QualityScrapID（标识要更新的实体）
   */
  qualityScrapId: string;

}


/**
 * QualityScrap 导入模板行 DTO
 * 对应前端 QualityScrapTemplate
 * @description 对应后端 TaktQualityScrapTemplateDto
 */
export interface QualityScrapTemplate {
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
   * 品质废弃编码(唯一,如:QS-2026-0001)
   */
  qualityScrapCode?: string;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 事故内容(废弃原因)
   */
  scrapReason?: string;

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
 * QualityScrap 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityScrapImport
 * @description 对应后端 TaktQualityScrapImportDto
 */
export interface QualityScrapImport {
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
   * 品质废弃编码(唯一,如:QS-2026-0001)
   */
  qualityScrapCode?: string;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 事故内容(废弃原因)
   */
  scrapReason?: string;

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
 * QualityScrap 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityScrapExport
 * @description 对应后端 TaktQualityScrapExportDto
 */
export interface QualityScrapExport {
  /**
   * QualityScrapID
   */
  qualityScrapId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 品质废弃编码(唯一,如:QS-2026-0001)
   */
  qualityScrapCode: string;

  /**
   * 废弃日期
   */
  scrapDate: string;

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
  scrapReason?: string;

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

