// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customer-satisfaction-survey.d.ts
// 创建时间：2026-07-23
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
 * 客户满意度调查表主表实体
 * 对应前端 TaktCustomerSatisfactionSurveyDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerSatisfactionSurvey
 * @description 对应后端 TaktCustomerSatisfactionSurveyDto
 */
export interface CustomerSatisfactionSurvey extends CompanyDtoBase {
  /**
   * CustomerSatisfactionSurveyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerSatisfactionSurveyId: string;

  /**
   * 调查表编码（组合唯一索引）
   */
  customerSatisfactionSurveyCode: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId: string;

  /**
   * 客户 名称（填充字段）
   */
  customerName?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 调查日期
   */
  surveyDate: string;

  /**
   * 调查方式（字典 logistics_quality_survey_method）
   */
  surveyMethod: number;

  /**
   * 调查类型（字典 logistics_quality_survey_type）
   */
  surveyType: number;

  /**
   * 调查周期（字典 logistics_quality_period）
   */
  surveyPeriod: number;

  /**
   * 调查人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  surveyorBy?: string;

  /**
   * 客户联系人
   */
  customerContact?: string;

  /**
   * 客户联系电话
   */
  customerPhone?: string;

  /**
   * 整体满意度（字典 logistics_quality_satisfaction_level）
   */
  overallSatisfaction: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 产品质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付准时率评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 服务质量评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 价格竞争力评分（0-100分）
   */
  priceScore?: number;

  /**
   * 技术支持评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 客户主要表扬
   */
  customerPraise?: string;

  /**
   * 客户主要意见/建议
   */
  customerFeedback?: string;

  /**
   * 改进计划/措施
   */
  improvementPlan?: string;

  /**
   * 关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  relatedComplaintId?: string;

  /**
   * 关联客诉 名称（填充字段）
   */
  relatedComplaintName?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 调查状态（字典 logistics_quality_survey_status）
   */
  surveyStatus: number;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus: number;

  /**
   * 调查项目明细列表（主子表关系） （子表：TaktCustomerSatisfactionSurveyItem）
   */
  items?: CustomerSatisfactionSurveyItem[];

}


/**
 * CustomerSatisfactionSurvey 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerSatisfactionSurveyQuery
 * @description 对应后端 TaktCustomerSatisfactionSurveyQueryDto
 */
export interface CustomerSatisfactionSurveyQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 调查表编码（组合唯一索引）
   */
  customerSatisfactionSurveyCode?: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 调查日期（范围查询-开始）
   */
  surveyDateStart?: string;

  /**
   * 调查日期（范围查询-结束）
   */
  surveyDateEnd?: string;

  /**
   * 调查方式（字典 logistics_quality_survey_method）
   */
  surveyMethod?: number;

  /**
   * 调查类型（字典 logistics_quality_survey_type）
   */
  surveyType?: number;

  /**
   * 调查周期（字典 logistics_quality_period）
   */
  surveyPeriod?: number;

  /**
   * 调查人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  surveyorBy?: string;

  /**
   * 客户联系人
   */
  customerContact?: string;

  /**
   * 客户联系电话
   */
  customerPhone?: string;

  /**
   * 整体满意度（字典 logistics_quality_satisfaction_level）
   */
  overallSatisfaction?: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 产品质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付准时率评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 服务质量评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 价格竞争力评分（0-100分）
   */
  priceScore?: number;

  /**
   * 技术支持评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 客户主要表扬
   */
  customerPraise?: string;

  /**
   * 客户主要意见/建议
   */
  customerFeedback?: string;

  /**
   * 改进计划/措施
   */
  improvementPlan?: string;

  /**
   * 关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  relatedComplaintId?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 调查状态（字典 logistics_quality_survey_status）
   */
  surveyStatus?: number;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus?: number;

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
 * 创建CustomerSatisfactionSurvey DTO
 * 对应前端 CustomerSatisfactionSurveyCreate
 * @description 对应后端 TaktCustomerSatisfactionSurveyCreateDto
 */
export interface CustomerSatisfactionSurveyCreate {
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
   * 调查表编码（组合唯一索引）
   */
  customerSatisfactionSurveyCode: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 调查日期
   */
  surveyDate: string;

  /**
   * 调查方式（字典 logistics_quality_survey_method）
   */
  surveyMethod: number;

  /**
   * 调查类型（字典 logistics_quality_survey_type）
   */
  surveyType: number;

  /**
   * 调查周期（字典 logistics_quality_period）
   */
  surveyPeriod: number;

  /**
   * 调查人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  surveyorBy?: string;

  /**
   * 客户联系人
   */
  customerContact?: string;

  /**
   * 客户联系电话
   */
  customerPhone?: string;

  /**
   * 整体满意度（字典 logistics_quality_satisfaction_level）
   */
  overallSatisfaction: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 产品质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付准时率评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 服务质量评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 价格竞争力评分（0-100分）
   */
  priceScore?: number;

  /**
   * 技术支持评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 客户主要表扬
   */
  customerPraise?: string;

  /**
   * 客户主要意见/建议
   */
  customerFeedback?: string;

  /**
   * 改进计划/措施
   */
  improvementPlan?: string;

  /**
   * 关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  relatedComplaintId?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 调查状态（字典 logistics_quality_survey_status）
   */
  surveyStatus: number;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus: number;

  /**
   * 调查项目明细列表（主子表关系）（子表，级联保存）
   */
  items?: CustomerSatisfactionSurveyItemCreate[];

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
 * 更新CustomerSatisfactionSurvey DTO
 * 继承 TaktCustomerSatisfactionSurveyCreateDto，添加 CustomerSatisfactionSurveyId 字段
 * 对应前端 CustomerSatisfactionSurveyUpdate
 * @description 对应后端 TaktCustomerSatisfactionSurveyUpdateDto
 */
export interface CustomerSatisfactionSurveyUpdate extends CustomerSatisfactionSurveyCreate {
  /**
   * CustomerSatisfactionSurveyID（标识要更新的实体）
   */
  customerSatisfactionSurveyId: string;

  /**
   * 调查项目明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * CustomerSatisfactionSurvey 状态更新 DTO
 * 对应前端 CustomerSatisfactionSurveyStatus
 * @description 对应后端 TaktCustomerSatisfactionSurveyStatusDto
 */
export interface CustomerSatisfactionSurveyStatus {
  /**
   * CustomerSatisfactionSurveyID
   */
  customerSatisfactionSurveyId: string;

  /**
   * 调查状态（字典 logistics_quality_survey_status）
   */
  surveyStatus: number;

}


/**
 * CustomerSatisfactionSurvey 排序更新 DTO
 * 对应前端 CustomerSatisfactionSurveySort
 * @description 对应后端 TaktCustomerSatisfactionSurveySortDto
 */
export interface CustomerSatisfactionSurveySort {
  /**
   * CustomerSatisfactionSurveyID
   */
  customerSatisfactionSurveyId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CustomerSatisfactionSurvey 导入模板行 DTO
 * 对应前端 CustomerSatisfactionSurveyTemplate
 * @description 对应后端 TaktCustomerSatisfactionSurveyTemplateDto
 */
export interface CustomerSatisfactionSurveyTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 调查表编码（组合唯一索引）
   */
  customerSatisfactionSurveyCode?: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 调查日期
   */
  surveyDate?: string;

  /**
   * 调查方式（字典 logistics_quality_survey_method）
   */
  surveyMethod?: number;

  /**
   * 调查类型（字典 logistics_quality_survey_type）
   */
  surveyType?: number;

  /**
   * 调查周期（字典 logistics_quality_period）
   */
  surveyPeriod?: number;

  /**
   * 调查人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  surveyorBy?: string;

  /**
   * 客户联系人
   */
  customerContact?: string;

  /**
   * 客户联系电话
   */
  customerPhone?: string;

  /**
   * 整体满意度（字典 logistics_quality_satisfaction_level）
   */
  overallSatisfaction?: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 产品质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付准时率评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 服务质量评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 价格竞争力评分（0-100分）
   */
  priceScore?: number;

  /**
   * 技术支持评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 客户主要表扬
   */
  customerPraise?: string;

  /**
   * 客户主要意见/建议
   */
  customerFeedback?: string;

  /**
   * 改进计划/措施
   */
  improvementPlan?: string;

  /**
   * 关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  relatedComplaintId?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 调查状态（字典 logistics_quality_survey_status）
   */
  surveyStatus?: number;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus?: number;

  /**
   * 调查项目明细列表（主子表关系）（子表，级联保存）
   */
  items?: CustomerSatisfactionSurveyItemCreate[];

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
 * CustomerSatisfactionSurvey 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerSatisfactionSurveyImport
 * @description 对应后端 TaktCustomerSatisfactionSurveyImportDto
 */
export interface CustomerSatisfactionSurveyImport {
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
   * 调查表编码（组合唯一索引）
   */
  customerSatisfactionSurveyCode?: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 调查日期
   */
  surveyDate?: string;

  /**
   * 调查方式（字典 logistics_quality_survey_method）
   */
  surveyMethod?: number;

  /**
   * 调查类型（字典 logistics_quality_survey_type）
   */
  surveyType?: number;

  /**
   * 调查周期（字典 logistics_quality_period）
   */
  surveyPeriod?: number;

  /**
   * 调查人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  surveyorBy?: string;

  /**
   * 客户联系人
   */
  customerContact?: string;

  /**
   * 客户联系电话
   */
  customerPhone?: string;

  /**
   * 整体满意度（字典 logistics_quality_satisfaction_level）
   */
  overallSatisfaction?: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 产品质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付准时率评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 服务质量评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 价格竞争力评分（0-100分）
   */
  priceScore?: number;

  /**
   * 技术支持评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 客户主要表扬
   */
  customerPraise?: string;

  /**
   * 客户主要意见/建议
   */
  customerFeedback?: string;

  /**
   * 改进计划/措施
   */
  improvementPlan?: string;

  /**
   * 关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  relatedComplaintId?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 调查状态（字典 logistics_quality_survey_status）
   */
  surveyStatus?: number;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus?: number;

  /**
   * 调查项目明细列表（主子表关系）（子表，级联保存）
   */
  items?: CustomerSatisfactionSurveyItemCreate[];

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
 * CustomerSatisfactionSurvey 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerSatisfactionSurveyExport
 * @description 对应后端 TaktCustomerSatisfactionSurveyExportDto
 */
export interface CustomerSatisfactionSurveyExport {
  /**
   * CustomerSatisfactionSurveyID
   */
  customerSatisfactionSurveyId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 调查表编码（组合唯一索引）
   */
  customerSatisfactionSurveyCode: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 调查日期
   */
  surveyDate: string;

  /**
   * 调查方式（字典 logistics_quality_survey_method）
   */
  surveyMethod: number;

  /**
   * 调查类型（字典 logistics_quality_survey_type）
   */
  surveyType: number;

  /**
   * 调查周期（字典 logistics_quality_period）
   */
  surveyPeriod: number;

  /**
   * 调查人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  surveyorBy?: string;

  /**
   * 客户联系人
   */
  customerContact?: string;

  /**
   * 客户联系电话
   */
  customerPhone?: string;

  /**
   * 整体满意度（字典 logistics_quality_satisfaction_level）
   */
  overallSatisfaction: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 产品质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付准时率评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 服务质量评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 价格竞争力评分（0-100分）
   */
  priceScore?: number;

  /**
   * 技术支持评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 客户主要表扬
   */
  customerPraise?: string;

  /**
   * 客户主要意见/建议
   */
  customerFeedback?: string;

  /**
   * 改进计划/措施
   */
  improvementPlan?: string;

  /**
   * 关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  relatedComplaintId?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 调查状态（字典 logistics_quality_survey_status）
   */
  surveyStatus: number;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus: number;

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

