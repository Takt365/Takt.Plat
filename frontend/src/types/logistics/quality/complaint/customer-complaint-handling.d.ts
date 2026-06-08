// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customer-complaint-handling.d.ts
// 创建时间：2026-06-08
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
 * 客诉处理记录实体
 * 对应前端 TaktCustomerComplaintHandlingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerComplaintHandling
 * @description 对应后端 TaktCustomerComplaintHandlingDto
 */
export interface CustomerComplaintHandling extends CompanyDtoBase {
  /**
   * CustomerComplaintHandlingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerComplaintHandlingId: string;

  /**
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId: string;

  /**
   * 客诉名称（填充字段）
   */
  complaintName?: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  complaintNo: string;

  /**
   * 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
   */
  complaintItemId?: string;

  /**
   * 客诉明细名称（填充字段）
   */
  complaintItemName?: string;

  /**
   * 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
   */
  handlingStage: number;

  /**
   * 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

  /**
   * 处理人（人员代码）
   */
  handlerBy?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
   */
  handlingStatus: number;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
   */
  customerSatisfaction?: number;

  /**
   * 附件路径（JSON格式，存储相关文件URL列表）
   */
  attachmentPaths?: string;

  /**
   * 客诉主表 （主表：TaktCustomerComplaint）
   */
  complaint?: CustomerComplaint;

}


/**
 * CustomerComplaintHandling 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerComplaintHandlingQuery
 * @description 对应后端 TaktCustomerComplaintHandlingQueryDto
 */
export interface CustomerComplaintHandlingQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode?: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  complaintNo?: string;

  /**
   * 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
   */
  handlingStage?: number;

  /**
   * 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

  /**
   * 处理人（人员代码）
   */
  handlerBy?: string;

  /**
   * 处理时间（范围查询-开始）
   */
  handlingAtStart?: string;

  /**
   * 处理时间（范围查询-结束）
   */
  handlingAtEnd?: string;

  /**
   * 计划完成日期（范围查询-开始）
   */
  plannedCompletionDateStart?: string;

  /**
   * 计划完成日期（范围查询-结束）
   */
  plannedCompletionDateEnd?: string;

  /**
   * 实际完成日期（范围查询-开始）
   */
  actualCompletionDateStart?: string;

  /**
   * 实际完成日期（范围查询-结束）
   */
  actualCompletionDateEnd?: string;

  /**
   * 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
   */
  handlingStatus?: number;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
   */
  customerSatisfaction?: number;

  /**
   * 附件路径（JSON格式，存储相关文件URL列表）
   */
  attachmentPaths?: string;

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
 * 创建CustomerComplaintHandling DTO
 * 对应前端 CustomerComplaintHandlingCreate
 * @description 对应后端 TaktCustomerComplaintHandlingCreateDto
 */
export interface CustomerComplaintHandlingCreate {
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
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  complaintNo: string;

  /**
   * 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
   */
  handlingStage: number;

  /**
   * 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

  /**
   * 处理人（人员代码）
   */
  handlerBy?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
   */
  handlingStatus: number;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
   */
  customerSatisfaction?: number;

  /**
   * 附件路径（JSON格式，存储相关文件URL列表）
   */
  attachmentPaths?: string;

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
 * 更新CustomerComplaintHandling DTO
 * 继承 TaktCustomerComplaintHandlingCreateDto，添加 CustomerComplaintHandlingId 字段
 * 对应前端 CustomerComplaintHandlingUpdate
 * @description 对应后端 TaktCustomerComplaintHandlingUpdateDto
 */
export interface CustomerComplaintHandlingUpdate extends CustomerComplaintHandlingCreate {
  /**
   * CustomerComplaintHandlingID（标识要更新的实体）
   */
  customerComplaintHandlingId: string;

}


/**
 * CustomerComplaintHandling 状态更新 DTO
 * 对应前端 CustomerComplaintHandlingStatus
 * @description 对应后端 TaktCustomerComplaintHandlingStatusDto
 */
export interface CustomerComplaintHandlingStatus {
  /**
   * CustomerComplaintHandlingID
   */
  customerComplaintHandlingId: string;

  /**
   * 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
   */
  handlingStatus: number;

}


/**
 * CustomerComplaintHandling 导入模板行 DTO
 * 对应前端 CustomerComplaintHandlingTemplate
 * @description 对应后端 TaktCustomerComplaintHandlingTemplateDto
 */
export interface CustomerComplaintHandlingTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode?: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  complaintNo?: string;

  /**
   * 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
   */
  handlingStage?: number;

  /**
   * 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

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
 * CustomerComplaintHandling 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerComplaintHandlingImport
 * @description 对应后端 TaktCustomerComplaintHandlingImportDto
 */
export interface CustomerComplaintHandlingImport {
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
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode?: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  complaintNo?: string;

  /**
   * 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
   */
  handlingStage?: number;

  /**
   * 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

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
 * CustomerComplaintHandling 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerComplaintHandlingExport
 * @description 对应后端 TaktCustomerComplaintHandlingExportDto
 */
export interface CustomerComplaintHandlingExport {
  /**
   * CustomerComplaintHandlingID
   */
  customerComplaintHandlingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  complaintNo: string;

  /**
   * 客诉明细ID（可选，关联到具体不良项目，序列化为string以避免Javascript精度问题）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（0=初步响应，1=原因分析，2=改善对策，3=效果验证，4=结案）
   */
  handlingStage: number;

  /**
   * 处理方式（0=返工，1=返修，2=补货，3=退货，4=退款，5=折扣，6=其他）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

  /**
   * 处理人（人员代码）
   */
  handlerBy?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 处理状态（0=待处理，1=处理中，2=已完成，3=已关闭，4=已驳回）
   */
  handlingStatus: number;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（0=不满意，1=一般，2=满意，3=非常满意）
   */
  customerSatisfaction?: number;

  /**
   * 附件路径（JSON格式，存储相关文件URL列表）
   */
  attachmentPaths?: string;

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

