// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-invoice.d.ts
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
 * Takt采购发票主表实体（公司级；字段按 RBKP 业务清单）
 * 对应前端 TaktPurchaseInvoiceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInvoice
 * @description 对应后端 TaktPurchaseInvoiceDto
 */
export interface PurchaseInvoice extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 发票凭证编号
   */
  purchaseInvoiceCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 凭证类型（字典 logistics_purchase_invoice_document_type）
   */
  documentType?: string;

  /**
   * 凭证日期
   */
  documentDate?: string;

  /**
   * 过帐日期
   */
  postingDate?: string;

  /**
   * 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 货币（字典 accounting_currency_code）
   */
  currencyCode?: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 总发票金额
   */
  grossAmount?: number;

  /**
   * 增值税金额
   */
  vatAmount?: number;

  /**
   * 税务代码
   */
  taxJurisdictionCode?: string;

  /**
   * 天数 1（现金折扣天数）
   */
  cashDiscountDays1?: number;

  /**
   * 发票
   */
  invoiceFlag?: string;

  /**
   * 凭证抬头文本
   */
  headerText?: string;

  /**
   * 冲销者（冲销凭证编号）
   */
  reversalDocumentCode?: string;

  /**
   * 年（冲销会计年度）
   */
  reversalFiscalYear?: string;

  /**
   * 税码
   */
  taxCode?: string;

  /**
   * 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  supplyingCountry?: string;

  /**
   * 税率（税务汇率）
   */
  taxExchangeRate?: number;

  /**
   * 付款基准日期
   */
  baselineDate?: string;

  /**
   * 输入者
   */
  enteredBy?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 用户名（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  postedBy?: string;

  /**
   * 采购发票明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInvoiceItemCreate[];

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
 * PurchaseInvoice 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInvoiceExport
 * @description 对应后端 TaktPurchaseInvoiceExportDto
 */
export interface PurchaseInvoiceExport {
  /**
   * PurchaseInvoiceID
   */
  purchaseInvoiceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 发票凭证编号
   */
  purchaseInvoiceCode: string;

  /**
   * 会计年度
   */
  fiscalYear: string;

  /**
   * 凭证类型（字典 logistics_purchase_invoice_document_type）
   */
  documentType?: string;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 货币（字典 accounting_currency_code）
   */
  currencyCode: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 总发票金额
   */
  grossAmount: number;

  /**
   * 增值税金额
   */
  vatAmount?: number;

  /**
   * 税务代码
   */
  taxJurisdictionCode?: string;

  /**
   * 天数 1（现金折扣天数）
   */
  cashDiscountDays1?: number;

  /**
   * 发票
   */
  invoiceFlag?: string;

  /**
   * 凭证抬头文本
   */
  headerText?: string;

  /**
   * 冲销者（冲销凭证编号）
   */
  reversalDocumentCode?: string;

  /**
   * 年（冲销会计年度）
   */
  reversalFiscalYear?: string;

  /**
   * 税码
   */
  taxCode?: string;

  /**
   * 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  supplyingCountry?: string;

  /**
   * 税率（税务汇率）
   */
  taxExchangeRate?: number;

  /**
   * 付款基准日期
   */
  baselineDate?: string;

  /**
   * 输入者
   */
  enteredBy?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 用户名（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  postedBy?: string;

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

