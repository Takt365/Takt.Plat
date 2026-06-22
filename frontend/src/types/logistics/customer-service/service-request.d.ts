// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：service-request.d.ts
// 创建时间：2026-06-21
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
 * 服务请求实体
 * 对应前端 TaktServiceRequestDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ServiceRequest
 * @description 对应后端 TaktServiceRequestDto
 */
export interface ServiceRequest extends CompanyDtoBase {
  /**
   * ServiceRequestID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  serviceRequestId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode: string;

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
   * 请求日期
   */
  requestDate: string;

  /**
   * 期望服务日期
   */
  expectedServiceDate?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel: number;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus: number;

  /**
   * 请求主题
   */
  requestSubject: string;

  /**
   * 请求描述
   */
  requestDescription: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间
   */
  assignedAt?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId） （子表：TaktServiceTicket）
   */
  tickets?: ServiceTicket[];

}


/**
 * ServiceRequest 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ServiceRequestQuery
 * @description 对应后端 TaktServiceRequestQueryDto
 */
export interface ServiceRequestQuery extends TaktPagedQuery {
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
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode?: string;

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
   * 请求日期（范围查询-开始）
   */
  requestDateStart?: string;

  /**
   * 请求日期（范围查询-结束）
   */
  requestDateEnd?: string;

  /**
   * 期望服务日期（范围查询-开始）
   */
  expectedServiceDateStart?: string;

  /**
   * 期望服务日期（范围查询-结束）
   */
  expectedServiceDateEnd?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType?: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel?: number;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority?: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus?: number;

  /**
   * 请求主题
   */
  requestSubject?: string;

  /**
   * 请求描述
   */
  requestDescription?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间（范围查询-开始）
   */
  assignedAtStart?: string;

  /**
   * 受理时间（范围查询-结束）
   */
  assignedAtEnd?: string;

  /**
   * 关闭时间（范围查询-开始）
   */
  closedAtStart?: string;

  /**
   * 关闭时间（范围查询-结束）
   */
  closedAtEnd?: string;

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
 * 创建ServiceRequest DTO
 * 对应前端 ServiceRequestCreate
 * @description 对应后端 TaktServiceRequestCreateDto
 */
export interface ServiceRequestCreate {
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
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode: string;

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
   * 请求日期
   */
  requestDate: string;

  /**
   * 期望服务日期
   */
  expectedServiceDate?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel: number;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus: number;

  /**
   * 请求主题
   */
  requestSubject: string;

  /**
   * 请求描述
   */
  requestDescription: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间
   */
  assignedAt?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

  /**
   * 服务工单列表（外键在子表 TaktServiceTicket.ServiceRequestId）（子表，级联保存）
   */
  tickets?: ServiceTicketCreate[];

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
 * 更新ServiceRequest DTO
 * 继承 TaktServiceRequestCreateDto，添加 ServiceRequestId 字段
 * 对应前端 ServiceRequestUpdate
 * @description 对应后端 TaktServiceRequestUpdateDto
 */
export interface ServiceRequestUpdate extends ServiceRequestCreate {
  /**
   * ServiceRequestID（标识要更新的实体）
   */
  serviceRequestId: string;

}


/**
 * ServiceRequest 状态更新 DTO
 * 对应前端 ServiceRequestStatus
 * @description 对应后端 TaktServiceRequestStatusDto
 */
export interface ServiceRequestStatus {
  /**
   * ServiceRequestID
   */
  serviceRequestId: string;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus: number;

}


/**
 * ServiceRequest 排序更新 DTO
 * 对应前端 ServiceRequestSort
 * @description 对应后端 TaktServiceRequestSortDto
 */
export interface ServiceRequestSort {
  /**
   * ServiceRequestID
   */
  serviceRequestId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * ServiceRequest 导入模板行 DTO
 * 对应前端 ServiceRequestTemplate
 * @description 对应后端 TaktServiceRequestTemplateDto
 */
export interface ServiceRequestTemplate {
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
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode?: string;

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
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType?: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel?: number;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority?: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus?: number;

  /**
   * 请求主题
   */
  requestSubject?: string;

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
 * ServiceRequest 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ServiceRequestImport
 * @description 对应后端 TaktServiceRequestImportDto
 */
export interface ServiceRequestImport {
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
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode?: string;

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
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType?: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel?: number;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority?: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus?: number;

  /**
   * 请求主题
   */
  requestSubject?: string;

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
 * ServiceRequest 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ServiceRequestExport
 * @description 对应后端 TaktServiceRequestExportDto
 */
export interface ServiceRequestExport {
  /**
   * ServiceRequestID
   */
  serviceRequestId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode: string;

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
   * 请求日期
   */
  requestDate: string;

  /**
   * 期望服务日期
   */
  expectedServiceDate?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel: number;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus: number;

  /**
   * 请求主题
   */
  requestSubject: string;

  /**
   * 请求描述
   */
  requestDescription: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间
   */
  assignedAt?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

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

