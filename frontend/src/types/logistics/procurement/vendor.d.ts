// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：vendor.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt经销商实体
 * 对应前端 TaktVendorDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Vendor
 * @description 对应后端 TaktVendorDto
 */
export interface Vendor extends CompanyDtoBase {
  /**
   * VendorID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  vendorId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 经销商编码（唯一索引）
   */
  vendorCode: string;

  /**
   * 经销商名称
   */
  vendorName: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（0=普通，1=核心，2=战略，3=临时）
   */
  vendorLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格经销商（0=否，1=是）
   */
  isQualified: number;

  /**
   * 经销商状态（1=启用，0=禁用）
   */
  vendorStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Vendor 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 VendorQuery
 * @description 对应后端 TaktVendorQueryDto
 */
export interface VendorQuery extends TaktPagedQuery {
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
   * 经销商编码（唯一索引）
   */
  vendorCode?: string;

  /**
   * 经销商名称
   */
  vendorName?: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（0=普通，1=核心，2=战略，3=临时）
   */
  vendorLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 是否合格经销商（0=否，1=是）
   */
  isQualified?: number;

  /**
   * 经销商状态（1=启用，0=禁用）
   */
  vendorStatus?: number;

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
 * 创建Vendor DTO
 * 对应前端 VendorCreate
 * @description 对应后端 TaktVendorCreateDto
 */
export interface VendorCreate {
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
   * 经销商编码（唯一索引）
   */
  vendorCode: string;

  /**
   * 经销商名称
   */
  vendorName: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（0=普通，1=核心，2=战略，3=临时）
   */
  vendorLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格经销商（0=否，1=是）
   */
  isQualified: number;

  /**
   * 经销商状态（1=启用，0=禁用）
   */
  vendorStatus: number;

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
 * 更新Vendor DTO
 * 继承 TaktVendorCreateDto，添加 VendorId 字段
 * 对应前端 VendorUpdate
 * @description 对应后端 TaktVendorUpdateDto
 */
export interface VendorUpdate extends VendorCreate {
  /**
   * VendorID（标识要更新的实体）
   */
  vendorId: string;

}


/**
 * Vendor 状态更新 DTO
 * 对应前端 VendorStatus
 * @description 对应后端 TaktVendorStatusDto
 */
export interface VendorStatus {
  /**
   * VendorID
   */
  vendorId: string;

  /**
   * 经销商状态（1=启用，0=禁用）
   */
  vendorStatus: number;

}


/**
 * Vendor 排序更新 DTO
 * 对应前端 VendorSort
 * @description 对应后端 TaktVendorSortDto
 */
export interface VendorSort {
  /**
   * VendorID
   */
  vendorId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Vendor 导入模板行 DTO
 * 对应前端 VendorTemplate
 * @description 对应后端 TaktVendorTemplateDto
 */
export interface VendorTemplate {
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
   * 经销商编码（唯一索引）
   */
  vendorCode?: string;

  /**
   * 经销商名称
   */
  vendorName?: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

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
 * Vendor 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 VendorImport
 * @description 对应后端 TaktVendorImportDto
 */
export interface VendorImport {
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
   * 经销商编码（唯一索引）
   */
  vendorCode?: string;

  /**
   * 经销商名称
   */
  vendorName?: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

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
 * Vendor 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 VendorExport
 * @description 对应后端 TaktVendorExportDto
 */
export interface VendorExport {
  /**
   * VendorID
   */
  vendorId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 经销商编码（唯一索引）
   */
  vendorCode: string;

  /**
   * 经销商名称
   */
  vendorName: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（0=普通，1=核心，2=战略，3=临时）
   */
  vendorLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格经销商（0=否，1=是）
   */
  isQualified: number;

  /**
   * 经销商状态（1=启用，0=禁用）
   */
  vendorStatus: number;

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

