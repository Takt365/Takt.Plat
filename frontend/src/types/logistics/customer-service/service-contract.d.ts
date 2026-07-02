// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：service-contract.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 服务合同实体
 * 对应前端 TaktServiceContractDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ServiceContract
 * @description 对应后端 TaktServiceContractDto
 */
export interface ServiceContract extends CompanyDtoBase {
  /**
   * ServiceContractID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  serviceContractId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode: string;

  /**
   * 合同名称
   */
  contractName: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 服务订单列表（外键在子表 TaktServiceOrder.ServiceContractId） （子表：TaktServiceOrder）
   */
  serviceOrders?: ServiceOrder[];

  /**
   * 服务请求列表（外键在子表 TaktServiceRequest.ServiceContractId） （子表：TaktServiceRequest）
   */
  serviceRequests?: ServiceRequest[];

}


/**
 * ServiceContract 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ServiceContractQuery
 * @description 对应后端 TaktServiceContractQueryDto
 */
export interface ServiceContractQuery extends TaktPagedQuery {
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
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode?: string;

  /**
   * 合同名称
   */
  contractName?: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId?: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode?: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName?: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType?: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus?: number;

  /**
   * 签订日期（范围查询-开始）
   */
  signDateStart?: string;

  /**
   * 签订日期（范围查询-结束）
   */
  signDateEnd?: string;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 到期日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 到期日期（范围查询-结束）
   */
  expiryDateEnd?: string;

  /**
   * 合同金额
   */
  contractAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms?: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours?: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours?: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

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
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建ServiceContract DTO
 * 对应前端 ServiceContractCreate
 * @description 对应后端 TaktServiceContractCreateDto
 */
export interface ServiceContractCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode: string;

  /**
   * 合同名称
   */
  contractName: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

  /**
   * 服务订单列表（外键在子表 TaktServiceOrder.ServiceContractId）（子表，级联保存）
   */
  serviceOrders?: ServiceOrderCreate[];

  /**
   * 服务请求列表（外键在子表 TaktServiceRequest.ServiceContractId）（子表，级联保存）
   */
  serviceRequests?: ServiceRequestCreate[];

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
 * 更新ServiceContract DTO
 * 继承 TaktServiceContractCreateDto，添加 ServiceContractId 字段
 * 对应前端 ServiceContractUpdate
 * @description 对应后端 TaktServiceContractUpdateDto
 */
export interface ServiceContractUpdate extends ServiceContractCreate {
  /**
   * ServiceContractID（标识要更新的实体）
   */
  serviceContractId: string;

}


/**
 * ServiceContract 状态更新 DTO
 * 对应前端 ServiceContractStatus
 * @description 对应后端 TaktServiceContractStatusDto
 */
export interface ServiceContractStatus {
  /**
   * ServiceContractID
   */
  serviceContractId: string;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus: number;

}


/**
 * ServiceContract 排序更新 DTO
 * 对应前端 ServiceContractSort
 * @description 对应后端 TaktServiceContractSortDto
 */
export interface ServiceContractSort {
  /**
   * ServiceContractID
   */
  serviceContractId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * ServiceContract 导入模板行 DTO
 * 对应前端 ServiceContractTemplate
 * @description 对应后端 TaktServiceContractTemplateDto
 */
export interface ServiceContractTemplate {
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
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode?: string;

  /**
   * 合同名称
   */
  contractName?: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId?: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode?: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName?: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType?: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus?: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms?: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours?: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours?: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

  /**
   * 服务订单列表（外键在子表 TaktServiceOrder.ServiceContractId）（子表，级联保存）
   */
  serviceOrders?: ServiceOrderCreate[];

  /**
   * 服务请求列表（外键在子表 TaktServiceRequest.ServiceContractId）（子表，级联保存）
   */
  serviceRequests?: ServiceRequestCreate[];

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
 * ServiceContract 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ServiceContractImport
 * @description 对应后端 TaktServiceContractImportDto
 */
export interface ServiceContractImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode?: string;

  /**
   * 合同名称
   */
  contractName?: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId?: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode?: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName?: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType?: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus?: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms?: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours?: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours?: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

  /**
   * 服务订单列表（外键在子表 TaktServiceOrder.ServiceContractId）（子表，级联保存）
   */
  serviceOrders?: ServiceOrderCreate[];

  /**
   * 服务请求列表（外键在子表 TaktServiceRequest.ServiceContractId）（子表，级联保存）
   */
  serviceRequests?: ServiceRequestCreate[];

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
 * ServiceContract 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ServiceContractExport
 * @description 对应后端 TaktServiceContractExportDto
 */
export interface ServiceContractExport {
  /**
   * ServiceContractID
   */
  serviceContractId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode: string;

  /**
   * 合同名称
   */
  contractName: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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

