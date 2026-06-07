// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：supplier.d.ts
// 创建时间：2026-06-07
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
 * Takt供货商实体
 * 对应前端 TaktSupplierDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Supplier
 * @description 对应后端 TaktSupplierDto
 */
export interface Supplier extends CompanyDtoBase {
  /**
   * SupplierID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  supplierId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 供货商编码（唯一索引）
   */
  supplierCode: string;

  /**
   * 供货商名称
   */
  supplierName: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
   */
  supplierType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

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
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

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
   * 供货商等级（0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格供货商（0=否，1=是）
   */
  isQualified: number;

  /**
   * 供货商状态（1=启用，0=禁用）
   */
  supplierStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Supplier 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SupplierQuery
 * @description 对应后端 TaktSupplierQueryDto
 */
export interface SupplierQuery extends TaktPagedQuery {
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
   * 供货商编码（唯一索引）
   */
  supplierCode?: string;

  /**
   * 供货商名称
   */
  supplierName?: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
   */
  supplierType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

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
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

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
   * 供货商等级（0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 是否合格供货商（0=否，1=是）
   */
  isQualified?: number;

  /**
   * 供货商状态（1=启用，0=禁用）
   */
  supplierStatus?: number;

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
 * 创建Supplier DTO
 * 对应前端 SupplierCreate
 * @description 对应后端 TaktSupplierCreateDto
 */
export interface SupplierCreate {
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
   * 供货商编码（唯一索引）
   */
  supplierCode: string;

  /**
   * 供货商名称
   */
  supplierName: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
   */
  supplierType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

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
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

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
   * 供货商等级（0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格供货商（0=否，1=是）
   */
  isQualified: number;

  /**
   * 供货商状态（1=启用，0=禁用）
   */
  supplierStatus: number;

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
 * 更新Supplier DTO
 * 继承 TaktSupplierCreateDto，添加 SupplierId 字段
 * 对应前端 SupplierUpdate
 * @description 对应后端 TaktSupplierUpdateDto
 */
export interface SupplierUpdate extends SupplierCreate {
  /**
   * SupplierID（标识要更新的实体）
   */
  supplierId: string;

}


/**
 * Supplier 状态更新 DTO
 * 对应前端 SupplierStatus
 * @description 对应后端 TaktSupplierStatusDto
 */
export interface SupplierStatus {
  /**
   * SupplierID
   */
  supplierId: string;

  /**
   * 供货商状态（1=启用，0=禁用）
   */
  supplierStatus: number;

}


/**
 * Supplier 排序更新 DTO
 * 对应前端 SupplierSort
 * @description 对应后端 TaktSupplierSortDto
 */
export interface SupplierSort {
  /**
   * SupplierID
   */
  supplierId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Supplier 导入模板行 DTO
 * 对应前端 SupplierTemplate
 * @description 对应后端 TaktSupplierTemplateDto
 */
export interface SupplierTemplate {
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
   * 供货商编码（唯一索引）
   */
  supplierCode?: string;

  /**
   * 供货商名称
   */
  supplierName?: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
   */
  supplierType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

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
   * 供货商电话
   */
  supplierPhone?: string;

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
 * Supplier 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SupplierImport
 * @description 对应后端 TaktSupplierImportDto
 */
export interface SupplierImport {
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
   * 供货商编码（唯一索引）
   */
  supplierCode?: string;

  /**
   * 供货商名称
   */
  supplierName?: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
   */
  supplierType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

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
   * 供货商电话
   */
  supplierPhone?: string;

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
 * Supplier 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SupplierExport
 * @description 对应后端 TaktSupplierExportDto
 */
export interface SupplierExport {
  /**
   * SupplierID
   */
  supplierId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 供货商编码（唯一索引）
   */
  supplierCode: string;

  /**
   * 供货商名称
   */
  supplierName: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
   */
  supplierType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

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
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

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
   * 供货商等级（0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格供货商（0=否，1=是）
   */
  isQualified: number;

  /**
   * 供货商状态（1=启用，0=禁用）
   */
  supplierStatus: number;

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

