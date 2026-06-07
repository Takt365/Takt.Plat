// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：sales-quotation-item.d.ts
// 创建时间：2026-06-07
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
 * Takt销售报价明细实体
 * 对应前端 TaktSalesQuotationItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesQuotationItem
 * @description 对应后端 TaktSalesQuotationItemDto
 */
export interface SalesQuotationItem extends CompanyDtoBase {
  /**
   * SalesQuotationItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesQuotationItemId: string;

  /**
   * 销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesQuotationId: string;

  /**
   * 销售报价名称（填充字段）
   */
  salesQuotationName?: string;

  /**
   * 销售报价编码（冗余字段，便于查询）
   */
  salesQuotationCode: string;

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
   * 报价数量（基本单位数量）
   */
  quotationQuantity: number;

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
 * SalesQuotationItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesQuotationItemQuery
 * @description 对应后端 TaktSalesQuotationItemQueryDto
 */
export interface SalesQuotationItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesQuotationId?: string;

  /**
   * 销售报价编码（冗余字段，便于查询）
   */
  salesQuotationCode?: string;

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
   * 报价数量（基本单位数量）
   */
  quotationQuantity?: number;

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
 * 创建SalesQuotationItem DTO
 * 对应前端 SalesQuotationItemCreate
 * @description 对应后端 TaktSalesQuotationItemCreateDto
 */
export interface SalesQuotationItemCreate {
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
   * 销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesQuotationId: string;

  /**
   * 销售报价编码（冗余字段，便于查询）
   */
  salesQuotationCode: string;

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
   * 报价数量（基本单位数量）
   */
  quotationQuantity: number;

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
 * 更新SalesQuotationItem DTO
 * 继承 TaktSalesQuotationItemCreateDto，添加 SalesQuotationItemId 字段
 * 对应前端 SalesQuotationItemUpdate
 * @description 对应后端 TaktSalesQuotationItemUpdateDto
 */
export interface SalesQuotationItemUpdate extends SalesQuotationItemCreate {
  /**
   * SalesQuotationItemID（标识要更新的实体）
   */
  salesQuotationItemId: string;

}


/**
 * SalesQuotationItem 导入模板行 DTO
 * 对应前端 SalesQuotationItemTemplate
 * @description 对应后端 TaktSalesQuotationItemTemplateDto
 */
export interface SalesQuotationItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesQuotationId?: string;

  /**
   * 销售报价编码（冗余字段，便于查询）
   */
  salesQuotationCode?: string;

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
 * SalesQuotationItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesQuotationItemImport
 * @description 对应后端 TaktSalesQuotationItemImportDto
 */
export interface SalesQuotationItemImport {
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
   * 销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesQuotationId?: string;

  /**
   * 销售报价编码（冗余字段，便于查询）
   */
  salesQuotationCode?: string;

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
 * SalesQuotationItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesQuotationItemExport
 * @description 对应后端 TaktSalesQuotationItemExportDto
 */
export interface SalesQuotationItemExport {
  /**
   * SalesQuotationItemID
   */
  salesQuotationItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesQuotationId: string;

  /**
   * 销售报价编码（冗余字段，便于查询）
   */
  salesQuotationCode: string;

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
   * 报价数量（基本单位数量）
   */
  quotationQuantity: number;

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

