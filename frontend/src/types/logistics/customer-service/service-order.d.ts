// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：service-order.d.ts
// 创建时间：2026-06-08
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
 * 服务订单实体
 * 对应前端 TaktServiceOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ServiceOrder
 * @description 对应后端 TaktServiceOrderDto
 */
export interface ServiceOrder extends CompanyDtoBase {
  /**
   * ServiceOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  serviceOrderId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode: string;

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
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同名称（填充字段）
   */
  serviceContractName?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求名称（填充字段）
   */
  serviceRequestName?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus: number;

  /**
   * 订单总金额
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
   * 订单实付金额
   */
  actualAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 计划开始日期
   */
  plannedStartDate?: string;

  /**
   * 计划结束日期
   */
  plannedEndDate?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际结束日期
   */
  actualEndDate?: string;

  /**
   * 服务负责人（人员代码）
   */
  serviceBy?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 服务工单列表（外键在子表 <see cref="TaktServiceTicket.ServiceOrderId"/>） （子表：TaktServiceTicket）
   */
  tickets?: ServiceTicket[];

}


/**
 * ServiceOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ServiceOrderQuery
 * @description 对应后端 TaktServiceOrderQueryDto
 */
export interface ServiceOrderQuery extends TaktPagedQuery {
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
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode?: string;

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
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 订单日期（范围查询-开始）
   */
  orderDateStart?: string;

  /**
   * 订单日期（范围查询-结束）
   */
  orderDateEnd?: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType?: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus?: number;

  /**
   * 订单总金额
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
   * 订单实付金额
   */
  actualAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 计划开始日期（范围查询-开始）
   */
  plannedStartDateStart?: string;

  /**
   * 计划开始日期（范围查询-结束）
   */
  plannedStartDateEnd?: string;

  /**
   * 计划结束日期（范围查询-开始）
   */
  plannedEndDateStart?: string;

  /**
   * 计划结束日期（范围查询-结束）
   */
  plannedEndDateEnd?: string;

  /**
   * 实际开始日期（范围查询-开始）
   */
  actualStartDateStart?: string;

  /**
   * 实际开始日期（范围查询-结束）
   */
  actualStartDateEnd?: string;

  /**
   * 实际结束日期（范围查询-开始）
   */
  actualEndDateStart?: string;

  /**
   * 实际结束日期（范围查询-结束）
   */
  actualEndDateEnd?: string;

  /**
   * 服务负责人（人员代码）
   */
  serviceBy?: string;

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
 * 创建ServiceOrder DTO
 * 对应前端 ServiceOrderCreate
 * @description 对应后端 TaktServiceOrderCreateDto
 */
export interface ServiceOrderCreate {
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
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode: string;

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
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus: number;

  /**
   * 订单总金额
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
   * 订单实付金额
   */
  actualAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 计划开始日期
   */
  plannedStartDate?: string;

  /**
   * 计划结束日期
   */
  plannedEndDate?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际结束日期
   */
  actualEndDate?: string;

  /**
   * 服务负责人（人员代码）
   */
  serviceBy?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 服务工单列表（外键在子表 <see cref="TaktServiceTicket.ServiceOrderId"/>）（子表，级联保存）
   */
  tickets?: ServiceTicketCreate[];

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
 * 更新ServiceOrder DTO
 * 继承 TaktServiceOrderCreateDto，添加 ServiceOrderId 字段
 * 对应前端 ServiceOrderUpdate
 * @description 对应后端 TaktServiceOrderUpdateDto
 */
export interface ServiceOrderUpdate extends ServiceOrderCreate {
  /**
   * ServiceOrderID（标识要更新的实体）
   */
  serviceOrderId: string;

}


/**
 * ServiceOrder 状态更新 DTO
 * 对应前端 ServiceOrderStatus
 * @description 对应后端 TaktServiceOrderStatusDto
 */
export interface ServiceOrderStatus {
  /**
   * ServiceOrderID
   */
  serviceOrderId: string;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus: number;

}


/**
 * ServiceOrder 排序更新 DTO
 * 对应前端 ServiceOrderSort
 * @description 对应后端 TaktServiceOrderSortDto
 */
export interface ServiceOrderSort {
  /**
   * ServiceOrderID
   */
  serviceOrderId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * ServiceOrder 导入模板行 DTO
 * 对应前端 ServiceOrderTemplate
 * @description 对应后端 TaktServiceOrderTemplateDto
 */
export interface ServiceOrderTemplate {
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
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode?: string;

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
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType?: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

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
 * ServiceOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ServiceOrderImport
 * @description 对应后端 TaktServiceOrderImportDto
 */
export interface ServiceOrderImport {
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
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode?: string;

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
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType?: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

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
 * ServiceOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ServiceOrderExport
 * @description 对应后端 TaktServiceOrderExportDto
 */
export interface ServiceOrderExport {
  /**
   * ServiceOrderID
   */
  serviceOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode: string;

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
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus: number;

  /**
   * 订单总金额
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
   * 订单实付金额
   */
  actualAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 计划开始日期
   */
  plannedStartDate?: string;

  /**
   * 计划结束日期
   */
  plannedEndDate?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际结束日期
   */
  actualEndDate?: string;

  /**
   * 服务负责人（人员代码）
   */
  serviceBy?: string;

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

