// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-price.d.ts
// 创建时间：2026-08-10
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt采购价格实体（定价记录；条件类型 + 供应商 + 物料 + 有效期；含子表 Items）
 * 对应前端 TaktPurchasePriceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchasePrice
 * @description 对应后端 TaktPurchasePriceDto
 */
export interface PurchasePrice extends CompanyDtoBase {

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  purchasePriceCode?: string;

  /**
   * 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

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
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

  /**
   * 可变关键字
   */
  variableKey?: string;

  /**
   * 定价条件行列表（主子表关系）（子表，级联保存）
   */
  items?: PurchasePriceItemCreate[];

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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 定价记录号（唯一索引；长度 20）
   */
  purchasePriceCode: string;

  /**
   * 条件类型（字典 logistics_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）
   */
  priceType: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

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
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

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

