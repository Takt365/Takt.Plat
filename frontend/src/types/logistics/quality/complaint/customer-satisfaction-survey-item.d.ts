// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customer-satisfaction-survey-item.d.ts
// 创建时间：2026-06-09
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
 * 客户满意度调查项目明细实体
 * 对应前端 TaktCustomerSatisfactionSurveyItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerSatisfactionSurveyItem
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemDto
 */
export interface CustomerSatisfactionSurveyItem extends CompanyDtoBase {
  /**
   * CustomerSatisfactionSurveyItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerSatisfactionSurveyItemId: string;

  /**
   * 调查表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  surveyId: string;

  /**
   * 调查表名称（填充字段）
   */
  surveyName?: string;

  /**
   * 调查表编号（冗余字段，便于查询）
   */
  customerSatisfactionSurveyCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）
   */
  categoryType: number;

  /**
   * 调查项目名称
   */
  itemName: string;

  /**
   * 调查项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight: number;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
   */
  satisfactionLevel?: number;

  /**
   * 客户反馈/意见
   */
  customerFeedback?: string;

  /**
   * 改进建议
   */
  improvementSuggestion?: string;

  /**
   * 跟进措施
   */
  followUpAction?: string;

  /**
   * 跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）
   */
  followUpStatus: number;

  /**
   * 调查表主表 （主表：TaktCustomerSatisfactionSurvey）
   */
  survey?: CustomerSatisfactionSurvey;

}


/**
 * CustomerSatisfactionSurveyItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerSatisfactionSurveyItemQuery
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemQueryDto
 */
export interface CustomerSatisfactionSurveyItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 调查表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  surveyId?: string;

  /**
   * 调查表编号（冗余字段，便于查询）
   */
  customerSatisfactionSurveyCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）
   */
  categoryType?: number;

  /**
   * 调查项目名称
   */
  itemName?: string;

  /**
   * 调查项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight?: number;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
   */
  satisfactionLevel?: number;

  /**
   * 客户反馈/意见
   */
  customerFeedback?: string;

  /**
   * 改进建议
   */
  improvementSuggestion?: string;

  /**
   * 跟进措施
   */
  followUpAction?: string;

  /**
   * 跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）
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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建CustomerSatisfactionSurveyItem DTO
 * 对应前端 CustomerSatisfactionSurveyItemCreate
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemCreateDto
 */
export interface CustomerSatisfactionSurveyItemCreate {
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
   * 调查表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  surveyId: string;

  /**
   * 调查表编号（冗余字段，便于查询）
   */
  customerSatisfactionSurveyCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）
   */
  categoryType: number;

  /**
   * 调查项目名称
   */
  itemName: string;

  /**
   * 调查项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight: number;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
   */
  satisfactionLevel?: number;

  /**
   * 客户反馈/意见
   */
  customerFeedback?: string;

  /**
   * 改进建议
   */
  improvementSuggestion?: string;

  /**
   * 跟进措施
   */
  followUpAction?: string;

  /**
   * 跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）
   */
  followUpStatus: number;

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
 * 更新CustomerSatisfactionSurveyItem DTO
 * 继承 TaktCustomerSatisfactionSurveyItemCreateDto，添加 CustomerSatisfactionSurveyItemId 字段
 * 对应前端 CustomerSatisfactionSurveyItemUpdate
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemUpdateDto
 */
export interface CustomerSatisfactionSurveyItemUpdate extends CustomerSatisfactionSurveyItemCreate {
  /**
   * CustomerSatisfactionSurveyItemID（标识要更新的实体）
   */
  customerSatisfactionSurveyItemId: string;

}


/**
 * CustomerSatisfactionSurveyItem 状态更新 DTO
 * 对应前端 CustomerSatisfactionSurveyItemStatus
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemStatusDto
 */
export interface CustomerSatisfactionSurveyItemStatus {
  /**
   * CustomerSatisfactionSurveyItemID
   */
  customerSatisfactionSurveyItemId: string;

  /**
   * 跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）
   */
  followUpStatus: number;

}


/**
 * CustomerSatisfactionSurveyItem 导入模板行 DTO
 * 对应前端 CustomerSatisfactionSurveyItemTemplate
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemTemplateDto
 */
export interface CustomerSatisfactionSurveyItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 调查表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  surveyId?: string;

  /**
   * 调查表编号（冗余字段，便于查询）
   */
  customerSatisfactionSurveyCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）
   */
  categoryType?: number;

  /**
   * 调查项目名称
   */
  itemName?: string;

  /**
   * 调查项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight?: number;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
   */
  satisfactionLevel?: number;

  /**
   * 客户反馈/意见
   */
  customerFeedback?: string;

  /**
   * 改进建议
   */
  improvementSuggestion?: string;

  /**
   * 跟进措施
   */
  followUpAction?: string;

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
 * CustomerSatisfactionSurveyItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerSatisfactionSurveyItemImport
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemImportDto
 */
export interface CustomerSatisfactionSurveyItemImport {
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
   * 调查表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  surveyId?: string;

  /**
   * 调查表编号（冗余字段，便于查询）
   */
  customerSatisfactionSurveyCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）
   */
  categoryType?: number;

  /**
   * 调查项目名称
   */
  itemName?: string;

  /**
   * 调查项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight?: number;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
   */
  satisfactionLevel?: number;

  /**
   * 客户反馈/意见
   */
  customerFeedback?: string;

  /**
   * 改进建议
   */
  improvementSuggestion?: string;

  /**
   * 跟进措施
   */
  followUpAction?: string;

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
 * CustomerSatisfactionSurveyItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerSatisfactionSurveyItemExport
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemExportDto
 */
export interface CustomerSatisfactionSurveyItemExport {
  /**
   * CustomerSatisfactionSurveyItemID
   */
  customerSatisfactionSurveyItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 调查表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  surveyId: string;

  /**
   * 调查表编号（冗余字段，便于查询）
   */
  customerSatisfactionSurveyCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 调查类别类型（0=产品质量，1=交付服务，2=售后服务，3=技术支持，4=价格，5=其他）
   */
  categoryType: number;

  /**
   * 调查项目名称
   */
  itemName: string;

  /**
   * 调查项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight: number;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 满意度等级（0=非常不满意，1=不满意，2=一般，3=满意，4=非常满意）
   */
  satisfactionLevel?: number;

  /**
   * 客户反馈/意见
   */
  customerFeedback?: string;

  /**
   * 改进建议
   */
  improvementSuggestion?: string;

  /**
   * 跟进措施
   */
  followUpAction?: string;

  /**
   * 跟进状态（0=无需跟进，1=待跟进，2=跟进中，3=已完成）
   */
  followUpStatus: number;

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

