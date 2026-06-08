// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：sales-quotation.d.ts
// 创建时间：2026-06-08
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
 * Takt销售报价实体
 * 对应前端 TaktSalesQuotationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesQuotation
 * @description 对应后端 TaktSalesQuotationDto
 */
export interface SalesQuotation extends CompanyDtoBase {
  /**
   * SalesQuotationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesQuotationId: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode: string;

  /**
   * 客户编码
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 报价日期
   */
  quotationDate: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 报价总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 报价实付金额
   */
  actualAmount: number;

  /**
   * 报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）
   */
  quotationStatus: number;

  /**
   * 关联销售订单编码（报价转订单后回填）
   */
  salesOrderCode?: string;

  /**
   * 销售报价明细列表（主子表关系） （子表：TaktSalesQuotationItem）
   */
  items?: SalesQuotationItem[];

}


/**
 * SalesQuotation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesQuotationQuery
 * @description 对应后端 TaktSalesQuotationQueryDto
 */
export interface SalesQuotationQuery extends TaktPagedQuery {
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
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 报价日期（范围查询-开始）
   */
  quotationDateStart?: string;

  /**
   * 报价日期（范围查询-结束）
   */
  quotationDateEnd?: string;

  /**
   * 报价有效期至（范围查询-开始）
   */
  validUntilDateStart?: string;

  /**
   * 报价有效期至（范围查询-结束）
   */
  validUntilDateEnd?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 报价总金额
   */
  totalAmount?: number;

  /**
   * 折扣金额
   */
  discountAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 报价实付金额
   */
  actualAmount?: number;

  /**
   * 报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）
   */
  quotationStatus?: number;

  /**
   * 关联销售订单编码（报价转订单后回填）
   */
  salesOrderCode?: string;

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
 * 创建SalesQuotation DTO
 * 对应前端 SalesQuotationCreate
 * @description 对应后端 TaktSalesQuotationCreateDto
 */
export interface SalesQuotationCreate {
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
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode: string;

  /**
   * 客户编码
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 报价日期
   */
  quotationDate: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 报价总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 报价实付金额
   */
  actualAmount: number;

  /**
   * 报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）
   */
  quotationStatus: number;

  /**
   * 关联销售订单编码（报价转订单后回填）
   */
  salesOrderCode?: string;

  /**
   * 销售报价明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesQuotationItemCreate[];

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
 * 更新SalesQuotation DTO
 * 继承 TaktSalesQuotationCreateDto，添加 SalesQuotationId 字段
 * 对应前端 SalesQuotationUpdate
 * @description 对应后端 TaktSalesQuotationUpdateDto
 */
export interface SalesQuotationUpdate extends SalesQuotationCreate {
  /**
   * SalesQuotationID（标识要更新的实体）
   */
  salesQuotationId: string;

}


/**
 * SalesQuotation 状态更新 DTO
 * 对应前端 SalesQuotationStatus
 * @description 对应后端 TaktSalesQuotationStatusDto
 */
export interface SalesQuotationStatus {
  /**
   * SalesQuotationID
   */
  salesQuotationId: string;

  /**
   * 报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）
   */
  quotationStatus: number;

}


/**
 * SalesQuotation 导入模板行 DTO
 * 对应前端 SalesQuotationTemplate
 * @description 对应后端 TaktSalesQuotationTemplateDto
 */
export interface SalesQuotationTemplate {
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
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）
   */
  quotationStatus?: number;

  /**
   * 关联销售订单编码（报价转订单后回填）
   */
  salesOrderCode?: string;

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
 * SalesQuotation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesQuotationImport
 * @description 对应后端 TaktSalesQuotationImportDto
 */
export interface SalesQuotationImport {
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
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）
   */
  quotationStatus?: number;

  /**
   * 关联销售订单编码（报价转订单后回填）
   */
  salesOrderCode?: string;

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
 * SalesQuotation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesQuotationExport
 * @description 对应后端 TaktSalesQuotationExportDto
 */
export interface SalesQuotationExport {
  /**
   * SalesQuotationID
   */
  salesQuotationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode: string;

  /**
   * 客户编码
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 报价日期
   */
  quotationDate: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 报价总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 报价实付金额
   */
  actualAmount: number;

  /**
   * 报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）
   */
  quotationStatus: number;

  /**
   * 关联销售订单编码（报价转订单后回填）
   */
  salesOrderCode?: string;

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

