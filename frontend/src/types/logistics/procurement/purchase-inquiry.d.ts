// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-inquiry.d.ts
// 创建时间：2026-07-23
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
 * 采购询价实体
 * 对应前端 TaktPurchaseInquiryDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInquiry
 * @description 对应后端 TaktPurchaseInquiryDto
 */
export interface PurchaseInquiry extends CompanyDtoBase {


  /**
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode?: string;

  /**
   * 询价日期
   */
  inquiryDate?: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  inquiryId?: string;

  /**
   * 询价人（人员代码）
   */
  inquiryBy?: string;

  /**
   * 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode?: string;

  /**
   * 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
  taxRate?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
   */
  paymentMode?: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
   */
  chainScheme?: number;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 询价总金额
   */
  totalAmount?: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转价格金额
   */
  convertedAmount?: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  inquiryStatus?: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 采购询价明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInquiryItemCreate[];

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
 * PurchaseInquiry 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInquiryExport
 * @description 对应后端 TaktPurchaseInquiryExportDto
 */
export interface PurchaseInquiryExport {
  /**
   * PurchaseInquiryID
   */
  purchaseInquiryId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode: string;

  /**
   * 询价日期
   */
  inquiryDate: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  inquiryId?: string;

  /**
   * 询价人（人员代码）
   */
  inquiryBy: string;

  /**
   * 询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode: string;

  /**
   * 询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）
   */
  paymentMode: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）
   */
  chainScheme: number;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 询价总金额
   */
  totalAmount: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转价格金额
   */
  convertedAmount: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 询价状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  inquiryStatus: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

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

