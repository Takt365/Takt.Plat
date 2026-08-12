// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：price.d.ts
// 创建时间：2026-08-06
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
 * Takt销售价格实体（定价记录；条件类型 + 客户 + 物料 + 有效期；含子表 Items）
 * 对应前端 TaktSalesPriceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesPrice
 * @description 对应后端 TaktSalesPriceDto
 */
export interface SalesPrice extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  salesPriceCode?: string;

  /**
   * 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 税码（字典 accounting_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
   */
  taxCode?: string;

  /**
   * 基于收货的发票检验（字典 sys_yes_no_type；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 定价日期控制（字典 logistics_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 有效起始日
   */
  validFrom?: string;

  /**
   * 有效截至日
   */
  validTo?: string;

  /**
   * 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
   */
  salesQuotationId?: string;

  /**
   * 来源销售报价编码（冗余）
   */
  salesQuotationCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: SalesPriceItemCreate[];

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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  salesPriceCode: string;

  /**
   * 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 税码（字典 accounting_tax_code；DictValue=J0～J8/L1/X0～X3；中国）
   */
  taxCode?: string;

  /**
   * 基于收货的发票检验（字典 sys_yes_no_type；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 定价日期控制（字典 logistics_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 有效起始日
   */
  validFrom: string;

  /**
   * 有效截至日
   */
  validTo: string;

  /**
   * 来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）
   */
  salesQuotationId?: string;

  /**
   * 来源销售报价编码（冗余）
   */
  salesQuotationCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

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

