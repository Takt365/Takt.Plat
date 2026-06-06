// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：sales-invoice.d.ts
// 创建时间：2026-06-06
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
 * Takt销售发票实体
 * 对应前端 TaktSalesInvoiceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesInvoice
 * @description 对应后端 TaktSalesInvoiceDto
 */
export interface SalesInvoice extends CompanyDtoBase {
  /**
   * SalesInvoiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesInvoiceId: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售发票编码（唯一索引）
   */
  salesInvoiceCode: string;

  /**
   * 关联销售订单编码
   */
  salesOrderCode?: string;

  /**
   * 客户编码
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 开票日期
   */
  invoiceDate: string;

  /**
   * 发票总金额
   */
  totalAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 发票实付金额
   */
  actualAmount: number;

  /**
   * 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 发票号码（税务系统票号）
   */
  taxInvoiceNo?: string;

  /**
   * 销售发票明细列表（主子表关系，一张发票可有多个明细行） （子表：TaktSalesInvoiceItem）
   */
  items?: SalesInvoiceItem[];

}


/**
 * SalesInvoice 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesInvoiceQuery
 * @description 对应后端 TaktSalesInvoiceQueryDto
 */
export interface SalesInvoiceQuery extends TaktPagedQuery {
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
   * 销售发票编码（唯一索引）
   */
  salesInvoiceCode?: string;

  /**
   * 关联销售订单编码
   */
  salesOrderCode?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

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
   * 税费
   */
  taxAmount?: number;

  /**
   * 发票实付金额
   */
  actualAmount?: number;

  /**
   * 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus?: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 发票号码（税务系统票号）
   */
  taxInvoiceNo?: string;

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
 * 创建SalesInvoice DTO
 * 对应前端 SalesInvoiceCreate
 * @description 对应后端 TaktSalesInvoiceCreateDto
 */
export interface SalesInvoiceCreate {
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
   * 销售发票编码（唯一索引）
   */
  salesInvoiceCode: string;

  /**
   * 关联销售订单编码
   */
  salesOrderCode?: string;

  /**
   * 客户编码
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 开票日期
   */
  invoiceDate: string;

  /**
   * 发票总金额
   */
  totalAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 发票实付金额
   */
  actualAmount: number;

  /**
   * 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 发票号码（税务系统票号）
   */
  taxInvoiceNo?: string;

  /**
   * 销售发票明细列表（主子表关系，一张发票可有多个明细行）（子表，级联保存）
   */
  items?: SalesInvoiceItemCreate[];

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
 * 更新SalesInvoice DTO
 * 继承 TaktSalesInvoiceCreateDto，添加 SalesInvoiceId 字段
 * 对应前端 SalesInvoiceUpdate
 * @description 对应后端 TaktSalesInvoiceUpdateDto
 */
export interface SalesInvoiceUpdate extends SalesInvoiceCreate {
  /**
   * SalesInvoiceID（标识要更新的实体）
   */
  salesInvoiceId: string;

}


/**
 * SalesInvoice 状态更新 DTO
 * 对应前端 SalesInvoiceStatus
 * @description 对应后端 TaktSalesInvoiceStatusDto
 */
export interface SalesInvoiceStatus {
  /**
   * SalesInvoiceID
   */
  salesInvoiceId: string;

  /**
   * 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus: number;

}


/**
 * SalesInvoice 导入模板行 DTO
 * 对应前端 SalesInvoiceTemplate
 * @description 对应后端 TaktSalesInvoiceTemplateDto
 */
export interface SalesInvoiceTemplate {
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
   * 销售发票编码（唯一索引）
   */
  salesInvoiceCode?: string;

  /**
   * 关联销售订单编码
   */
  salesOrderCode?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus?: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 发票号码（税务系统票号）
   */
  taxInvoiceNo?: string;

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
 * SalesInvoice 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesInvoiceImport
 * @description 对应后端 TaktSalesInvoiceImportDto
 */
export interface SalesInvoiceImport {
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
   * 销售发票编码（唯一索引）
   */
  salesInvoiceCode?: string;

  /**
   * 关联销售订单编码
   */
  salesOrderCode?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus?: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 发票号码（税务系统票号）
   */
  taxInvoiceNo?: string;

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
 * SalesInvoice 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesInvoiceExport
 * @description 对应后端 TaktSalesInvoiceExportDto
 */
export interface SalesInvoiceExport {
  /**
   * SalesInvoiceID
   */
  salesInvoiceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售发票编码（唯一索引）
   */
  salesInvoiceCode: string;

  /**
   * 关联销售订单编码
   */
  salesOrderCode?: string;

  /**
   * 客户编码
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 开票日期
   */
  invoiceDate: string;

  /**
   * 发票总金额
   */
  totalAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 发票实付金额
   */
  actualAmount: number;

  /**
   * 发票状态（0=草稿，1=已开票，2=已收款，3=已作废）
   */
  invoiceStatus: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 发票号码（税务系统票号）
   */
  taxInvoiceNo?: string;

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

