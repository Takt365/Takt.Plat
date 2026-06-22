// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customer-complaint.d.ts
// 创建时间：2026-06-21
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 客诉主表实体
 * 对应前端 TaktCustomerComplaintDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerComplaint
 * @description 对应后端 TaktCustomerComplaintDto
 */
export interface CustomerComplaint extends CompanyDtoBase {
  /**
   * CustomerComplaintID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerComplaintId: string;

  /**
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode: string;

  /**
   * 客户ID（序列化为string以避免Javascript精度问题）
   */
  customerId: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 投诉日期
   */
  complaintDate: string;

  /**
   * 投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod: number;

  /**
   * 投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）
   */
  complaintType: number;

  /**
   * 投诉等级（0=一般，1=重要，2=紧急，3=严重）
   */
  complaintLevel: number;

  /**
   * 责任部门ID（序列化为string以避免Javascript精度问题）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称
   */
  responsibleDeptName?: string;

  /**
   * 责任人ID（序列化为string以避免Javascript精度问题）
   */
  responsiblePersonId?: string;

  /**
   * 责任人姓名
   */
  responsiblePersonName?: string;

  /**
   * 要求回复日期
   */
  requiredReplyDate?: string;

  /**
   * 实际回复日期
   */
  actualReplyDate?: string;

  /**
   * 客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）
   */
  complaintStatus: number;

  /**
   * 客诉描述
   */
  complaintDescription: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
   */
  customerSatisfaction?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客诉明细列表（主子表关系） （子表：TaktCustomerComplaintItem）
   */
  items?: CustomerComplaintItem[];

}


/**
 * CustomerComplaint 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerComplaintQuery
 * @description 对应后端 TaktCustomerComplaintQueryDto
 */
export interface CustomerComplaintQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode?: string;

  /**
   * 客户ID（序列化为string以避免Javascript精度问题）
   */
  customerId?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 投诉日期（范围查询-开始）
   */
  complaintDateStart?: string;

  /**
   * 投诉日期（范围查询-结束）
   */
  complaintDateEnd?: string;

  /**
   * 投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod?: number;

  /**
   * 投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）
   */
  complaintType?: number;

  /**
   * 投诉等级（0=一般，1=重要，2=紧急，3=严重）
   */
  complaintLevel?: number;

  /**
   * 责任部门ID（序列化为string以避免Javascript精度问题）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称
   */
  responsibleDeptName?: string;

  /**
   * 责任人ID（序列化为string以避免Javascript精度问题）
   */
  responsiblePersonId?: string;

  /**
   * 责任人姓名
   */
  responsiblePersonName?: string;

  /**
   * 要求回复日期（范围查询-开始）
   */
  requiredReplyDateStart?: string;

  /**
   * 要求回复日期（范围查询-结束）
   */
  requiredReplyDateEnd?: string;

  /**
   * 实际回复日期（范围查询-开始）
   */
  actualReplyDateStart?: string;

  /**
   * 实际回复日期（范围查询-结束）
   */
  actualReplyDateEnd?: string;

  /**
   * 客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）
   */
  complaintStatus?: number;

  /**
   * 客诉描述
   */
  complaintDescription?: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
   */
  customerSatisfaction?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 创建CustomerComplaint DTO
 * 对应前端 CustomerComplaintCreate
 * @description 对应后端 TaktCustomerComplaintCreateDto
 */
export interface CustomerComplaintCreate {
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
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode: string;

  /**
   * 客户ID（序列化为string以避免Javascript精度问题）
   */
  customerId: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 投诉日期
   */
  complaintDate: string;

  /**
   * 投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod: number;

  /**
   * 投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）
   */
  complaintType: number;

  /**
   * 投诉等级（0=一般，1=重要，2=紧急，3=严重）
   */
  complaintLevel: number;

  /**
   * 责任部门ID（序列化为string以避免Javascript精度问题）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称
   */
  responsibleDeptName?: string;

  /**
   * 责任人ID（序列化为string以避免Javascript精度问题）
   */
  responsiblePersonId?: string;

  /**
   * 责任人姓名
   */
  responsiblePersonName?: string;

  /**
   * 要求回复日期
   */
  requiredReplyDate?: string;

  /**
   * 实际回复日期
   */
  actualReplyDate?: string;

  /**
   * 客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）
   */
  complaintStatus: number;

  /**
   * 客诉描述
   */
  complaintDescription: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
   */
  customerSatisfaction?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 客诉明细列表（主子表关系）（子表，级联保存）
   */
  items?: CustomerComplaintItemCreate[];

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
 * 更新CustomerComplaint DTO
 * 继承 TaktCustomerComplaintCreateDto，添加 CustomerComplaintId 字段
 * 对应前端 CustomerComplaintUpdate
 * @description 对应后端 TaktCustomerComplaintUpdateDto
 */
export interface CustomerComplaintUpdate extends CustomerComplaintCreate {
  /**
   * CustomerComplaintID（标识要更新的实体）
   */
  customerComplaintId: string;

}


/**
 * CustomerComplaint 状态更新 DTO
 * 对应前端 CustomerComplaintStatus
 * @description 对应后端 TaktCustomerComplaintStatusDto
 */
export interface CustomerComplaintStatus {
  /**
   * CustomerComplaintID
   */
  customerComplaintId: string;

  /**
   * 客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）
   */
  complaintStatus: number;

}


/**
 * CustomerComplaint 排序更新 DTO
 * 对应前端 CustomerComplaintSort
 * @description 对应后端 TaktCustomerComplaintSortDto
 */
export interface CustomerComplaintSort {
  /**
   * CustomerComplaintID
   */
  customerComplaintId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CustomerComplaint 导入模板行 DTO
 * 对应前端 CustomerComplaintTemplate
 * @description 对应后端 TaktCustomerComplaintTemplateDto
 */
export interface CustomerComplaintTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode?: string;

  /**
   * 客户ID（序列化为string以避免Javascript精度问题）
   */
  customerId?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod?: number;

  /**
   * 投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）
   */
  complaintType?: number;

  /**
   * 投诉等级（0=一般，1=重要，2=紧急，3=严重）
   */
  complaintLevel?: number;

  /**
   * 责任部门ID（序列化为string以避免Javascript精度问题）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称
   */
  responsibleDeptName?: string;

  /**
   * 责任人ID（序列化为string以避免Javascript精度问题）
   */
  responsiblePersonId?: string;

  /**
   * 责任人姓名
   */
  responsiblePersonName?: string;

  /**
   * 客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）
   */
  complaintStatus?: number;

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
 * CustomerComplaint 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerComplaintImport
 * @description 对应后端 TaktCustomerComplaintImportDto
 */
export interface CustomerComplaintImport {
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
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode?: string;

  /**
   * 客户ID（序列化为string以避免Javascript精度问题）
   */
  customerId?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod?: number;

  /**
   * 投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）
   */
  complaintType?: number;

  /**
   * 投诉等级（0=一般，1=重要，2=紧急，3=严重）
   */
  complaintLevel?: number;

  /**
   * 责任部门ID（序列化为string以避免Javascript精度问题）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称
   */
  responsibleDeptName?: string;

  /**
   * 责任人ID（序列化为string以避免Javascript精度问题）
   */
  responsiblePersonId?: string;

  /**
   * 责任人姓名
   */
  responsiblePersonName?: string;

  /**
   * 客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）
   */
  complaintStatus?: number;

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
 * CustomerComplaint 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerComplaintExport
 * @description 对应后端 TaktCustomerComplaintExportDto
 */
export interface CustomerComplaintExport {
  /**
   * CustomerComplaintID
   */
  customerComplaintId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode: string;

  /**
   * 客户ID（序列化为string以避免Javascript精度问题）
   */
  customerId: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 投诉日期
   */
  complaintDate: string;

  /**
   * 投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod: number;

  /**
   * 投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）
   */
  complaintType: number;

  /**
   * 投诉等级（0=一般，1=重要，2=紧急，3=严重）
   */
  complaintLevel: number;

  /**
   * 责任部门ID（序列化为string以避免Javascript精度问题）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称
   */
  responsibleDeptName?: string;

  /**
   * 责任人ID（序列化为string以避免Javascript精度问题）
   */
  responsiblePersonId?: string;

  /**
   * 责任人姓名
   */
  responsiblePersonName?: string;

  /**
   * 要求回复日期
   */
  requiredReplyDate?: string;

  /**
   * 实际回复日期
   */
  actualReplyDate?: string;

  /**
   * 客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）
   */
  complaintStatus: number;

  /**
   * 客诉描述
   */
  complaintDescription: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
   */
  customerSatisfaction?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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

