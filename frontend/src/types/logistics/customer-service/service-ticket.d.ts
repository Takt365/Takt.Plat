// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：service-ticket.d.ts
// 创建时间：2026-06-06
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
 * 服务工单实体
 * 对应前端 TaktServiceTicketDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ServiceTicket
 * @description 对应后端 TaktServiceTicketDto
 */
export interface ServiceTicket extends CompanyDtoBase {
  /**
   * ServiceTicketID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  serviceTicketId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode: string;

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
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单名称（填充字段）
   */
  serviceOrderName?: string;

  /**
   * 关联服务订单编码（冗余字段，便于查询）
   */
  serviceOrderCode?: string;

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
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType: number;

  /**
   * 优先级（0=低，1=中，2=高，3=紧急）
   */
  priority: number;

  /**
   * 工单状态（0=待派工，1=已派工，2=处理中，3=待验收，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus: number;

  /**
   * 工单主题
   */
  ticketSubject: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间
   */
  scheduledStartTime?: string;

  /**
   * 计划结束时间
   */
  scheduledEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 关联服务请求 （主表：TaktServiceRequest）
   */
  serviceRequest?: ServiceRequest;

  /**
   * 关联服务订单 （主表：TaktServiceOrder）
   */
  serviceOrder?: ServiceOrder;

  /**
   * 关联服务合同 （主表：TaktServiceContract）
   */
  serviceContract?: ServiceContract;

}


/**
 * ServiceTicket 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ServiceTicketQuery
 * @description 对应后端 TaktServiceTicketQueryDto
 */
export interface ServiceTicketQuery extends TaktPagedQuery {
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
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode?: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余字段，便于查询）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType?: number;

  /**
   * 优先级（0=低，1=中，2=高，3=紧急）
   */
  priority?: number;

  /**
   * 工单状态（0=待派工，1=已派工，2=处理中，3=待验收，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus?: number;

  /**
   * 工单主题
   */
  ticketSubject?: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间（范围查询-开始）
   */
  scheduledStartTimeStart?: string;

  /**
   * 计划开始时间（范围查询-结束）
   */
  scheduledStartTimeEnd?: string;

  /**
   * 计划结束时间（范围查询-开始）
   */
  scheduledEndTimeStart?: string;

  /**
   * 计划结束时间（范围查询-结束）
   */
  scheduledEndTimeEnd?: string;

  /**
   * 实际开始时间（范围查询-开始）
   */
  actualStartTimeStart?: string;

  /**
   * 实际开始时间（范围查询-结束）
   */
  actualStartTimeEnd?: string;

  /**
   * 实际结束时间（范围查询-开始）
   */
  actualEndTimeStart?: string;

  /**
   * 实际结束时间（范围查询-结束）
   */
  actualEndTimeEnd?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人
   */
  acceptedBy?: string;

  /**
   * 验收时间（范围查询-开始）
   */
  acceptedAtStart?: string;

  /**
   * 验收时间（范围查询-结束）
   */
  acceptedAtEnd?: string;

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
 * 创建ServiceTicket DTO
 * 对应前端 ServiceTicketCreate
 * @description 对应后端 TaktServiceTicketCreateDto
 */
export interface ServiceTicketCreate {
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
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余字段，便于查询）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType: number;

  /**
   * 优先级（0=低，1=中，2=高，3=紧急）
   */
  priority: number;

  /**
   * 工单状态（0=待派工，1=已派工，2=处理中，3=待验收，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus: number;

  /**
   * 工单主题
   */
  ticketSubject: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间
   */
  scheduledStartTime?: string;

  /**
   * 计划结束时间
   */
  scheduledEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

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
 * 更新ServiceTicket DTO
 * 继承 TaktServiceTicketCreateDto，添加 ServiceTicketId 字段
 * 对应前端 ServiceTicketUpdate
 * @description 对应后端 TaktServiceTicketUpdateDto
 */
export interface ServiceTicketUpdate extends ServiceTicketCreate {
  /**
   * ServiceTicketID（标识要更新的实体）
   */
  serviceTicketId: string;

}


/**
 * ServiceTicket 状态更新 DTO
 * 对应前端 ServiceTicketStatus
 * @description 对应后端 TaktServiceTicketStatusDto
 */
export interface ServiceTicketStatus {
  /**
   * ServiceTicketID
   */
  serviceTicketId: string;

  /**
   * 工单状态（0=待派工，1=已派工，2=处理中，3=待验收，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus: number;

}


/**
 * ServiceTicket 排序更新 DTO
 * 对应前端 ServiceTicketSort
 * @description 对应后端 TaktServiceTicketSortDto
 */
export interface ServiceTicketSort {
  /**
   * ServiceTicketID
   */
  serviceTicketId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * ServiceTicket 导入模板行 DTO
 * 对应前端 ServiceTicketTemplate
 * @description 对应后端 TaktServiceTicketTemplateDto
 */
export interface ServiceTicketTemplate {
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
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode?: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余字段，便于查询）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType?: number;

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
 * ServiceTicket 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ServiceTicketImport
 * @description 对应后端 TaktServiceTicketImportDto
 */
export interface ServiceTicketImport {
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
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode?: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余字段，便于查询）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType?: number;

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
 * ServiceTicket 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ServiceTicketExport
 * @description 对应后端 TaktServiceTicketExportDto
 */
export interface ServiceTicketExport {
  /**
   * ServiceTicketID
   */
  serviceTicketId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余字段，便于查询）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余字段，便于查询）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余字段，便于查询）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType: number;

  /**
   * 优先级（0=低，1=中，2=高，3=紧急）
   */
  priority: number;

  /**
   * 工单状态（0=待派工，1=已派工，2=处理中，3=待验收，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus: number;

  /**
   * 工单主题
   */
  ticketSubject: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间
   */
  scheduledStartTime?: string;

  /**
   * 计划结束时间
   */
  scheduledEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

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

