// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：customer.d.ts
// 创建时间：2026-06-05
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
 * Takt客户信息实体
 * 对应前端 TaktCustomerDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Customer
 * @description 对应后端 TaktCustomerDto
 */
export interface Customer extends CompanyDtoBase {
  /**
   * CustomerID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 客户编码（唯一索引）
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
   */
  customerType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2两位代码）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
   */
  paymentTerms: number;

  /**
   * 信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount: number;

  /**
   * 折扣率（百分比，如：5.5表示5.5%折扣）
   */
  discountRate: number;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 客户等级（0=普通，1=重要，2=VIP，3=战略）
   */
  customerLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格客户（0=否，1=是）
   */
  isQualified: number;

  /**
   * 客户状态（1=启用，0=禁用）
   */
  customerStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Customer 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerQuery
 * @description 对应后端 TaktCustomerQueryDto
 */
export interface CustomerQuery extends TaktPagedQuery {
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
   * 客户编码（唯一索引）
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
   */
  customerType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2两位代码）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
   */
  paymentTerms?: number;

  /**
   * 信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel?: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount?: number;

  /**
   * 折扣率（百分比，如：5.5表示5.5%折扣）
   */
  discountRate?: number;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 客户等级（0=普通，1=重要，2=VIP，3=战略）
   */
  customerLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 是否合格客户（0=否，1=是）
   */
  isQualified?: number;

  /**
   * 客户状态（1=启用，0=禁用）
   */
  customerStatus?: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

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
 * 创建Customer DTO
 * 对应前端 CustomerCreate
 * @description 对应后端 TaktCustomerCreateDto
 */
export interface CustomerCreate {
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
  plantCode: string;

  /**
   * 客户编码（唯一索引）
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
   */
  customerType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2两位代码）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
   */
  paymentTerms: number;

  /**
   * 信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount: number;

  /**
   * 折扣率（百分比，如：5.5表示5.5%折扣）
   */
  discountRate: number;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 客户等级（0=普通，1=重要，2=VIP，3=战略）
   */
  customerLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格客户（0=否，1=是）
   */
  isQualified: number;

  /**
   * 客户状态（1=启用，0=禁用）
   */
  customerStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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
 * 更新Customer DTO
 * 继承 TaktCustomerCreateDto，添加 CustomerId 字段
 * 对应前端 CustomerUpdate
 * @description 对应后端 TaktCustomerUpdateDto
 */
export interface CustomerUpdate extends CustomerCreate {
  /**
   * CustomerID（标识要更新的实体）
   */
  customerId: string;

}


/**
 * Customer 状态更新 DTO
 * 对应前端 CustomerStatus
 * @description 对应后端 TaktCustomerStatusDto
 */
export interface CustomerStatus {
  /**
   * CustomerID
   */
  customerId: string;

  /**
   * 客户状态（1=启用，0=禁用）
   */
  customerStatus: number;

}


/**
 * Customer 排序更新 DTO
 * 对应前端 CustomerSort
 * @description 对应后端 TaktCustomerSortDto
 */
export interface CustomerSort {
  /**
   * CustomerID
   */
  customerId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Customer 导入模板行 DTO
 * 对应前端 CustomerTemplate
 * @description 对应后端 TaktCustomerTemplateDto
 */
export interface CustomerTemplate {
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
   * 客户编码（唯一索引）
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
   */
  customerType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2两位代码）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

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
 * Customer 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerImport
 * @description 对应后端 TaktCustomerImportDto
 */
export interface CustomerImport {
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
   * 客户编码（唯一索引）
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
   */
  customerType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2两位代码）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

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
 * Customer 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerExport
 * @description 对应后端 TaktCustomerExportDto
 */
export interface CustomerExport {
  /**
   * CustomerID
   */
  customerId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 客户编码（唯一索引）
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（0=企业客户，1=个人客户，2=政府机构，3=其他）
   */
  customerType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2两位代码）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 付款条件（0=款到发货，1=货到付款，2=月结30天，3=月结60天，4=月结90天，5=其他）
   */
  paymentTerms: number;

  /**
   * 信用等级（0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount: number;

  /**
   * 折扣率（百分比，如：5.5表示5.5%折扣）
   */
  discountRate: number;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 客户等级（0=普通，1=重要，2=VIP，3=战略）
   */
  customerLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格客户（0=否，1=是）
   */
  isQualified: number;

  /**
   * 客户状态（1=启用，0=禁用）
   */
  customerStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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

