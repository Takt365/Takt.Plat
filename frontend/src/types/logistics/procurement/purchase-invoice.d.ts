// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-invoice.d.ts
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
 * Takt采购发票实体
 * 对应前端 TaktPurchaseInvoiceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInvoice
 * @description 对应后端 TaktPurchaseInvoiceDto
 */
export interface PurchaseInvoice extends CompanyDtoBase {
  /**
   * PurchaseInvoiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInvoiceId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 采购发票编码（唯一索引）
   */
  purchaseInvoiceCode: string;

  /**
   * 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 开票日期
   */
  invoiceDate: string;

  /**
   * 发票总金额
   */
  totalAmount: number;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
   */
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 发票应付金额
   */
  actualAmount: number;

  /**
   * 已付款金额
   */
  paidAmount: number;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 税务发票号码
   */
  taxInvoiceNo?: string;

  /**
   * 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus: number;

  /**
   * 采购发票明细列表（主子表关系，一张发票可有多个明细行） （子表：TaktPurchaseInvoiceItem）
   */
  items?: PurchaseInvoiceItem[];

}


/**
 * PurchaseInvoice 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInvoiceQuery
 * @description 对应后端 TaktPurchaseInvoiceQueryDto
 */
export interface PurchaseInvoiceQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 采购发票编码（唯一索引）
   */
  purchaseInvoiceCode?: string;

  /**
   * 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 开票日期（范围查询-开始）
   */
  invoiceDateStart?: string;

  /**
   * 开票日期（范围查询-结束）
   */
  invoiceDateEnd?: string;

  /**
   * 发票总金额
   */
  totalAmount?: number;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
   */
  taxRate?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 发票应付金额
   */
  actualAmount?: number;

  /**
   * 已付款金额
   */
  paidAmount?: number;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 税务发票号码
   */
  taxInvoiceNo?: string;

  /**
   * 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus?: number;

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
 * 创建PurchaseInvoice DTO
 * 对应前端 PurchaseInvoiceCreate
 * @description 对应后端 TaktPurchaseInvoiceCreateDto
 */
export interface PurchaseInvoiceCreate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 采购发票编码（唯一索引）
   */
  purchaseInvoiceCode: string;

  /**
   * 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 开票日期
   */
  invoiceDate: string;

  /**
   * 发票总金额
   */
  totalAmount: number;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
   */
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 发票应付金额
   */
  actualAmount: number;

  /**
   * 已付款金额
   */
  paidAmount: number;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 税务发票号码
   */
  taxInvoiceNo?: string;

  /**
   * 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus: number;

  /**
   * 采购发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
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
 * 更新PurchaseInvoice DTO
 * 继承 TaktPurchaseInvoiceCreateDto，添加 PurchaseInvoiceId 字段
 * 对应前端 PurchaseInvoiceUpdate
 * @description 对应后端 TaktPurchaseInvoiceUpdateDto
 */
export interface PurchaseInvoiceUpdate extends PurchaseInvoiceCreate {
  /**
   * PurchaseInvoiceID（标识要更新的实体）
   */
  purchaseInvoiceId: string;

  /**
   * 采购发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
   */
  items?: any;

}


/**
 * PurchaseInvoice 状态更新 DTO
 * 对应前端 PurchaseInvoiceStatus
 * @description 对应后端 TaktPurchaseInvoiceStatusDto
 */
export interface PurchaseInvoiceStatus {
  /**
   * PurchaseInvoiceID
   */
  purchaseInvoiceId: string;

  /**
   * 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus: number;

}


/**
 * PurchaseInvoice 导入模板行 DTO
 * 对应前端 PurchaseInvoiceTemplate
 * @description 对应后端 TaktPurchaseInvoiceTemplateDto
 */
export interface PurchaseInvoiceTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 采购发票编码（唯一索引）
   */
  purchaseInvoiceCode?: string;

  /**
   * 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 开票日期
   */
  invoiceDate?: string;

  /**
   * 发票总金额
   */
  totalAmount?: number;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
   */
  taxRate?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 发票应付金额
   */
  actualAmount?: number;

  /**
   * 已付款金额
   */
  paidAmount?: number;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 税务发票号码
   */
  taxInvoiceNo?: string;

  /**
   * 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus?: number;

  /**
   * 采购发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
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
 * PurchaseInvoice 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInvoiceImport
 * @description 对应后端 TaktPurchaseInvoiceImportDto
 */
export interface PurchaseInvoiceImport {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 采购发票编码（唯一索引）
   */
  purchaseInvoiceCode?: string;

  /**
   * 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 开票日期
   */
  invoiceDate?: string;

  /**
   * 发票总金额
   */
  totalAmount?: number;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
   */
  taxRate?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 发票应付金额
   */
  actualAmount?: number;

  /**
   * 已付款金额
   */
  paidAmount?: number;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 税务发票号码
   */
  taxInvoiceNo?: string;

  /**
   * 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus?: number;

  /**
   * 采购发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 采购发票编码（唯一索引）
   */
  purchaseInvoiceCode: string;

  /**
   * 关联采购订单编码（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 开票日期
   */
  invoiceDate: string;

  /**
   * 发票总金额
   */
  totalAmount: number;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
   */
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 发票应付金额
   */
  actualAmount: number;

  /**
   * 已付款金额
   */
  paidAmount: number;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 税务发票号码
   */
  taxInvoiceNo?: string;

  /**
   * 发票状态（字典 logistics_invoice_status；0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus: number;

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

