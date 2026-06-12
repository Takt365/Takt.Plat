// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：purchase-price.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt采购价格实体（供应商价格主表，一个供应商可以有多个物料价格）
 * 对应前端 TaktPurchasePriceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchasePrice
 * @description 对应后端 TaktPurchasePriceDto
 */
export interface PurchasePrice extends CompanyDtoBase {
  /**
   * PurchasePriceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchasePriceId: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 采购价格编码（唯一索引）
   */
  purchasePriceCode: string;

  /**
   * 供应商编码
   */
  supplierCode: string;

  /**
   * 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
   */
  priceType: number;

  /**
   * 生效日期
   */
  effectiveStartDate: string;

  /**
   * 失效日期（空表示长期有效）
   */
  effectiveEndDate?: string;

  /**
   * 价格状态（1=启用，0=禁用）
   */
  priceStatus: number;

  /**
   * 物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格） （子表：TaktPurchasePriceItem）
   */
  items?: PurchasePriceItem[];

  /**
   * 采购价格变更记录列表（外键在子表 TaktPurchasePriceChangeLog.PriceId） （子表：TaktPurchasePriceChangeLog）
   */
  changeLogs?: PurchasePriceChangeLog[];

}


/**
 * PurchasePrice 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchasePriceQuery
 * @description 对应后端 TaktPurchasePriceQueryDto
 */
export interface PurchasePriceQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 采购价格编码（唯一索引）
   */
  purchasePriceCode?: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
   */
  priceType?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveStartDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveStartDateEnd?: string;

  /**
   * 失效日期（空表示长期有效）（范围查询-开始）
   */
  effectiveEndDateStart?: string;

  /**
   * 失效日期（空表示长期有效）（范围查询-结束）
   */
  effectiveEndDateEnd?: string;

  /**
   * 价格状态（1=启用，0=禁用）
   */
  priceStatus?: number;

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
 * 创建PurchasePrice DTO
 * 对应前端 PurchasePriceCreate
 * @description 对应后端 TaktPurchasePriceCreateDto
 */
export interface PurchasePriceCreate {
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
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 采购价格编码（唯一索引）
   */
  purchasePriceCode: string;

  /**
   * 供应商编码
   */
  supplierCode: string;

  /**
   * 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
   */
  priceType: number;

  /**
   * 生效日期
   */
  effectiveStartDate: string;

  /**
   * 失效日期（空表示长期有效）
   */
  effectiveEndDate?: string;

  /**
   * 价格状态（1=启用，0=禁用）
   */
  priceStatus: number;

  /**
   * 物料价格明细列表（主子表关系，一个供应商价格可以有多个物料价格）（子表，级联保存）
   */
  items?: PurchasePriceItemCreate[];

  /**
   * 采购价格变更记录列表（外键在子表 TaktPurchasePriceChangeLog.PriceId）（子表，级联保存）
   */
  changeLogs?: PurchasePriceChangeLogCreate[];

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
 * 更新PurchasePrice DTO
 * 继承 TaktPurchasePriceCreateDto，添加 PurchasePriceId 字段
 * 对应前端 PurchasePriceUpdate
 * @description 对应后端 TaktPurchasePriceUpdateDto
 */
export interface PurchasePriceUpdate extends PurchasePriceCreate {
  /**
   * PurchasePriceID（标识要更新的实体）
   */
  purchasePriceId: string;

}


/**
 * PurchasePrice 状态更新 DTO
 * 对应前端 PurchasePriceStatus
 * @description 对应后端 TaktPurchasePriceStatusDto
 */
export interface PurchasePriceStatus {
  /**
   * PurchasePriceID
   */
  purchasePriceId: string;

  /**
   * 价格状态（1=启用，0=禁用）
   */
  priceStatus: number;

}


/**
 * PurchasePrice 导入模板行 DTO
 * 对应前端 PurchasePriceTemplate
 * @description 对应后端 TaktPurchasePriceTemplateDto
 */
export interface PurchasePriceTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 采购价格编码（唯一索引）
   */
  purchasePriceCode?: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
   */
  priceType?: number;

  /**
   * 价格状态（1=启用，0=禁用）
   */
  priceStatus?: number;

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
 * PurchasePrice 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchasePriceImport
 * @description 对应后端 TaktPurchasePriceImportDto
 */
export interface PurchasePriceImport {
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
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 采购价格编码（唯一索引）
   */
  purchasePriceCode?: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
   */
  priceType?: number;

  /**
   * 价格状态（1=启用，0=禁用）
   */
  priceStatus?: number;

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
 * PurchasePrice 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchasePriceExport
 * @description 对应后端 TaktPurchasePriceExportDto
 */
export interface PurchasePriceExport {
  /**
   * PurchasePriceID
   */
  purchasePriceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 采购价格编码（唯一索引）
   */
  purchasePriceCode: string;

  /**
   * 供应商编码
   */
  supplierCode: string;

  /**
   * 价格类型（0=标准价格，1=合同价格，2=临时价格，3=询价价格，4=历史价格）
   */
  priceType: number;

  /**
   * 生效日期
   */
  effectiveStartDate: string;

  /**
   * 失效日期（空表示长期有效）
   */
  effectiveEndDate?: string;

  /**
   * 价格状态（1=启用，0=禁用）
   */
  priceStatus: number;

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

