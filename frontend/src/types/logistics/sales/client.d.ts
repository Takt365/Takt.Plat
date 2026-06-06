// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：client.d.ts
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
 * Takt客户端信息实体
 * 对应前端 TaktClientDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Client
 * @description 对应后端 TaktClientDto
 */
export interface Client extends CompanyDtoBase {
  /**
   * ClientID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  clientId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 客户端编码（唯一索引）
   */
  clientCode: string;

  /**
   * 客户端名称
   */
  clientName: string;

  /**
   * 客户端简称
   */
  clientShortName?: string;

  /**
   * 客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）
   */
  clientType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户端标识（税务登记证号/统一社会信用代码）
   */
  clientTaxNumber?: string;

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
   * 客户端电话
   */
  clientPhone?: string;

  /**
   * 客户端传真
   */
  clientFax?: string;

  /**
   * 客户端邮箱
   */
  clientEmail?: string;

  /**
   * 客户端网站
   */
  clientWebsite?: string;

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
   * 销售渠道（0=直销，1=经销，2=代销，3=电商，4=其他）
   */
  salesChannel: number;

  /**
   * 平台名称（电商平台名称）
   */
  platformName?: string;

  /**
   * 店铺名称
   */
  storeName?: string;

  /**
   * 客户端等级（0=普通，1=重要，2=VIP，3=战略）
   */
  clientLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格客户端（0=否，1=是）
   */
  isQualified: number;

  /**
   * 客户端状态（1=启用，0=禁用）
   */
  clientStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Client 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ClientQuery
 * @description 对应后端 TaktClientQueryDto
 */
export interface ClientQuery extends TaktPagedQuery {
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
   * 客户端编码（唯一索引）
   */
  clientCode?: string;

  /**
   * 客户端名称
   */
  clientName?: string;

  /**
   * 客户端简称
   */
  clientShortName?: string;

  /**
   * 客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）
   */
  clientType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户端标识（税务登记证号/统一社会信用代码）
   */
  clientTaxNumber?: string;

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
   * 客户端电话
   */
  clientPhone?: string;

  /**
   * 客户端传真
   */
  clientFax?: string;

  /**
   * 客户端邮箱
   */
  clientEmail?: string;

  /**
   * 客户端网站
   */
  clientWebsite?: string;

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
   * 销售渠道（0=直销，1=经销，2=代销，3=电商，4=其他）
   */
  salesChannel?: number;

  /**
   * 平台名称（电商平台名称）
   */
  platformName?: string;

  /**
   * 店铺名称
   */
  storeName?: string;

  /**
   * 客户端等级（0=普通，1=重要，2=VIP，3=战略）
   */
  clientLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 是否合格客户端（0=否，1=是）
   */
  isQualified?: number;

  /**
   * 客户端状态（1=启用，0=禁用）
   */
  clientStatus?: number;

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
 * 创建Client DTO
 * 对应前端 ClientCreate
 * @description 对应后端 TaktClientCreateDto
 */
export interface ClientCreate {
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
   * 客户端编码（唯一索引）
   */
  clientCode: string;

  /**
   * 客户端名称
   */
  clientName: string;

  /**
   * 客户端简称
   */
  clientShortName?: string;

  /**
   * 客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）
   */
  clientType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户端标识（税务登记证号/统一社会信用代码）
   */
  clientTaxNumber?: string;

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
   * 客户端电话
   */
  clientPhone?: string;

  /**
   * 客户端传真
   */
  clientFax?: string;

  /**
   * 客户端邮箱
   */
  clientEmail?: string;

  /**
   * 客户端网站
   */
  clientWebsite?: string;

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
   * 销售渠道（0=直销，1=经销，2=代销，3=电商，4=其他）
   */
  salesChannel: number;

  /**
   * 平台名称（电商平台名称）
   */
  platformName?: string;

  /**
   * 店铺名称
   */
  storeName?: string;

  /**
   * 客户端等级（0=普通，1=重要，2=VIP，3=战略）
   */
  clientLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格客户端（0=否，1=是）
   */
  isQualified: number;

  /**
   * 客户端状态（1=启用，0=禁用）
   */
  clientStatus: number;

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
 * 更新Client DTO
 * 继承 TaktClientCreateDto，添加 ClientId 字段
 * 对应前端 ClientUpdate
 * @description 对应后端 TaktClientUpdateDto
 */
export interface ClientUpdate extends ClientCreate {
  /**
   * ClientID（标识要更新的实体）
   */
  clientId: string;

}


/**
 * Client 状态更新 DTO
 * 对应前端 ClientStatus
 * @description 对应后端 TaktClientStatusDto
 */
export interface ClientStatus {
  /**
   * ClientID
   */
  clientId: string;

  /**
   * 客户端状态（1=启用，0=禁用）
   */
  clientStatus: number;

}


/**
 * Client 排序更新 DTO
 * 对应前端 ClientSort
 * @description 对应后端 TaktClientSortDto
 */
export interface ClientSort {
  /**
   * ClientID
   */
  clientId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Client 导入模板行 DTO
 * 对应前端 ClientTemplate
 * @description 对应后端 TaktClientTemplateDto
 */
export interface ClientTemplate {
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
   * 客户端编码（唯一索引）
   */
  clientCode?: string;

  /**
   * 客户端名称
   */
  clientName?: string;

  /**
   * 客户端简称
   */
  clientShortName?: string;

  /**
   * 客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）
   */
  clientType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户端标识（税务登记证号/统一社会信用代码）
   */
  clientTaxNumber?: string;

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
   * 客户端电话
   */
  clientPhone?: string;

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
 * Client 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ClientImport
 * @description 对应后端 TaktClientImportDto
 */
export interface ClientImport {
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
   * 客户端编码（唯一索引）
   */
  clientCode?: string;

  /**
   * 客户端名称
   */
  clientName?: string;

  /**
   * 客户端简称
   */
  clientShortName?: string;

  /**
   * 客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）
   */
  clientType?: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户端标识（税务登记证号/统一社会信用代码）
   */
  clientTaxNumber?: string;

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
   * 客户端电话
   */
  clientPhone?: string;

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
 * Client 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ClientExport
 * @description 对应后端 TaktClientExportDto
 */
export interface ClientExport {
  /**
   * ClientID
   */
  clientId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 客户端编码（唯一索引）
   */
  clientCode: string;

  /**
   * 客户端名称
   */
  clientName: string;

  /**
   * 客户端简称
   */
  clientShortName?: string;

  /**
   * 客户端类型（0=终端客户，1=分销商，2=零售商，3=电商平台，4=其他）
   */
  clientType: number;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 客户端标识（税务登记证号/统一社会信用代码）
   */
  clientTaxNumber?: string;

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
   * 客户端电话
   */
  clientPhone?: string;

  /**
   * 客户端传真
   */
  clientFax?: string;

  /**
   * 客户端邮箱
   */
  clientEmail?: string;

  /**
   * 客户端网站
   */
  clientWebsite?: string;

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
   * 销售渠道（0=直销，1=经销，2=代销，3=电商，4=其他）
   */
  salesChannel: number;

  /**
   * 平台名称（电商平台名称）
   */
  platformName?: string;

  /**
   * 店铺名称
   */
  storeName?: string;

  /**
   * 客户端等级（0=普通，1=重要，2=VIP，3=战略）
   */
  clientLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 是否合格客户端（0=否，1=是）
   */
  isQualified: number;

  /**
   * 客户端状态（1=启用，0=禁用）
   */
  clientStatus: number;

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

