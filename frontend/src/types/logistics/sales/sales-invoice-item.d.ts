// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：sales-invoice-item.d.ts
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
 * Takt销售发票明细实体
 * 对应前端 TaktSalesInvoiceItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesInvoiceItem
 * @description 对应后端 TaktSalesInvoiceItemDto
 */
export interface SalesInvoiceItem extends CompanyDtoBase {
  /**
   * SalesInvoiceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesInvoiceItemId: string;

  /**
   * 销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesInvoiceId: string;

  /**
   * 销售发票名称（填充字段）
   */
  salesInvoiceName?: string;

  /**
   * 销售发票编码（冗余字段，便于查询）
   */
  salesInvoiceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 销售单位
   */
  salesUnit: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity: number;

  /**
   * 单价
   */
  unitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 税费率（0-100，表示税费百分比）
   */
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 小计金额
   */
  subtotalAmount: number;

}


/**
 * SalesInvoiceItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesInvoiceItemQuery
 * @description 对应后端 TaktSalesInvoiceItemQueryDto
 */
export interface SalesInvoiceItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesInvoiceId?: string;

  /**
   * 销售发票编码（冗余字段，便于查询）
   */
  salesInvoiceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 销售单位
   */
  salesUnit?: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity?: number;

  /**
   * 单价
   */
  unitPrice?: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额
   */
  discountAmount?: number;

  /**
   * 税费率（0-100，表示税费百分比）
   */
  taxRate?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 小计金额
   */
  subtotalAmount?: number;

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
 * 创建SalesInvoiceItem DTO
 * 对应前端 SalesInvoiceItemCreate
 * @description 对应后端 TaktSalesInvoiceItemCreateDto
 */
export interface SalesInvoiceItemCreate {
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
   * 销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesInvoiceId: string;

  /**
   * 销售发票编码（冗余字段，便于查询）
   */
  salesInvoiceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 销售单位
   */
  salesUnit: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity: number;

  /**
   * 单价
   */
  unitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 税费率（0-100，表示税费百分比）
   */
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 小计金额
   */
  subtotalAmount: number;

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
 * 更新SalesInvoiceItem DTO
 * 继承 TaktSalesInvoiceItemCreateDto，添加 SalesInvoiceItemId 字段
 * 对应前端 SalesInvoiceItemUpdate
 * @description 对应后端 TaktSalesInvoiceItemUpdateDto
 */
export interface SalesInvoiceItemUpdate extends SalesInvoiceItemCreate {
  /**
   * SalesInvoiceItemID（标识要更新的实体）
   */
  salesInvoiceItemId: string;

}


/**
 * SalesInvoiceItem 导入模板行 DTO
 * 对应前端 SalesInvoiceItemTemplate
 * @description 对应后端 TaktSalesInvoiceItemTemplateDto
 */
export interface SalesInvoiceItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesInvoiceId?: string;

  /**
   * 销售发票编码（冗余字段，便于查询）
   */
  salesInvoiceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 销售单位
   */
  salesUnit?: string;

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
 * SalesInvoiceItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesInvoiceItemImport
 * @description 对应后端 TaktSalesInvoiceItemImportDto
 */
export interface SalesInvoiceItemImport {
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
   * 销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesInvoiceId?: string;

  /**
   * 销售发票编码（冗余字段，便于查询）
   */
  salesInvoiceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 销售单位
   */
  salesUnit?: string;

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
 * SalesInvoiceItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesInvoiceItemExport
 * @description 对应后端 TaktSalesInvoiceItemExportDto
 */
export interface SalesInvoiceItemExport {
  /**
   * SalesInvoiceItemID
   */
  salesInvoiceItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售发票ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesInvoiceId: string;

  /**
   * 销售发票编码（冗余字段，便于查询）
   */
  salesInvoiceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 销售单位
   */
  salesUnit: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity: number;

  /**
   * 单价
   */
  unitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 税费率（0-100，表示税费百分比）
   */
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 小计金额
   */
  subtotalAmount: number;

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

