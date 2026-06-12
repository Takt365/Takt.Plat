// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：sales-price.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt销售价格实体（客户价格主表，一个客户可以有多个物料价格）
 * 对应前端 TaktSalesPriceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesPrice
 * @description 对应后端 TaktSalesPriceDto
 */
export interface SalesPrice extends CompanyDtoBase {
  /**
   * SalesPriceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPriceId: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售价格编码（唯一索引）
   */
  salesPriceCode: string;

  /**
   * 客户编码（如果为空则表示通用价格）
   */
  customerCode?: string;

  /**
   * 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
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
   * 物料价格明细列表（主子表关系，一个客户价格可以有多个物料价格） （子表：TaktSalesPriceItem）
   */
  items?: SalesPriceItem[];

  /**
   * 销售价格变更记录列表（外键在子表 TaktSalesPriceChangeLog.PriceId） （子表：TaktSalesPriceChangeLog）
   */
  changeLogs?: SalesPriceChangeLog[];

}


/**
 * SalesPrice 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesPriceQuery
 * @description 对应后端 TaktSalesPriceQueryDto
 */
export interface SalesPriceQuery extends TaktPagedQuery {
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
   * 销售价格编码（唯一索引）
   */
  salesPriceCode?: string;

  /**
   * 客户编码（如果为空则表示通用价格）
   */
  customerCode?: string;

  /**
   * 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
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
 * 创建SalesPrice DTO
 * 对应前端 SalesPriceCreate
 * @description 对应后端 TaktSalesPriceCreateDto
 */
export interface SalesPriceCreate {
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
  plantCode?: string;

  /**
   * 销售价格编码（唯一索引）
   */
  salesPriceCode: string;

  /**
   * 客户编码（如果为空则表示通用价格）
   */
  customerCode?: string;

  /**
   * 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
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
   * 物料价格明细列表（主子表关系，一个客户价格可以有多个物料价格）（子表，级联保存）
   */
  items?: SalesPriceItemCreate[];

  /**
   * 销售价格变更记录列表（外键在子表 TaktSalesPriceChangeLog.PriceId）（子表，级联保存）
   */
  changeLogs?: SalesPriceChangeLogCreate[];

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
 * 更新SalesPrice DTO
 * 继承 TaktSalesPriceCreateDto，添加 SalesPriceId 字段
 * 对应前端 SalesPriceUpdate
 * @description 对应后端 TaktSalesPriceUpdateDto
 */
export interface SalesPriceUpdate extends SalesPriceCreate {
  /**
   * SalesPriceID（标识要更新的实体）
   */
  salesPriceId: string;

}


/**
 * SalesPrice 状态更新 DTO
 * 对应前端 SalesPriceStatus
 * @description 对应后端 TaktSalesPriceStatusDto
 */
export interface SalesPriceStatus {
  /**
   * SalesPriceID
   */
  salesPriceId: string;

  /**
   * 价格状态（1=启用，0=禁用）
   */
  priceStatus: number;

}


/**
 * SalesPrice 导入模板行 DTO
 * 对应前端 SalesPriceTemplate
 * @description 对应后端 TaktSalesPriceTemplateDto
 */
export interface SalesPriceTemplate {
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
   * 销售价格编码（唯一索引）
   */
  salesPriceCode?: string;

  /**
   * 客户编码（如果为空则表示通用价格）
   */
  customerCode?: string;

  /**
   * 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
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
 * SalesPrice 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesPriceImport
 * @description 对应后端 TaktSalesPriceImportDto
 */
export interface SalesPriceImport {
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
   * 销售价格编码（唯一索引）
   */
  salesPriceCode?: string;

  /**
   * 客户编码（如果为空则表示通用价格）
   */
  customerCode?: string;

  /**
   * 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
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
 * SalesPrice 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesPriceExport
 * @description 对应后端 TaktSalesPriceExportDto
 */
export interface SalesPriceExport {
  /**
   * SalesPriceID
   */
  salesPriceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售价格编码（唯一索引）
   */
  salesPriceCode: string;

  /**
   * 客户编码（如果为空则表示通用价格）
   */
  customerCode?: string;

  /**
   * 价格类型（0=标准价格，1=客户价格，2=促销价格，3=合同价格，4=临时价格）
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

