// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：supplier-evaluation.d.ts
// 创建时间：2026-06-23
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
 * 供应商评价考核主表实体
 * 对应前端 TaktSupplierEvaluationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SupplierEvaluation
 * @description 对应后端 TaktSupplierEvaluationDto
 */
export interface SupplierEvaluation extends CompanyDtoBase {
  /**
   * SupplierEvaluationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  supplierEvaluationId: string;

  /**
   * 评价表编号（组合唯一索引）
   */
  supplierEvaluationCode: string;

  /**
   * 供应商ID（序列化为string以避免Javascript精度问题）
   */
  supplierId: string;

  /**
   * 供应商名称
   */
  supplierName: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 评价日期
   */
  evaluationDate: string;

  /**
   * 评价周期（0=月度，1=季度，2=半年度，3=年度）
   */
  evaluationPeriod: number;

  /**
   * 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
   */
  evaluationType: number;

  /**
   * 评价人（人员代码）
   */
  evaluatorBy?: string;

  /**
   * 评价部门
   */
  evaluationDept?: string;

  /**
   * 总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  overallRating: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 价格评分（0-100分）
   */
  priceScore?: number;

  /**
   * 服务评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 技术能力评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 主要优点
   */
  mainStrengths?: string;

  /**
   * 主要问题/不足
   */
  mainIssues?: string;

  /**
   * 改进要求/建议
   */
  improvementRequirements?: string;

  /**
   * 考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）
   */
  evaluationConclusion: number;

  /**
   * 整改期限（要求完成日期）
   */
  rectificationDeadline?: string;

  /**
   * 评价状态（0=草稿，1=评价中，2=已完成，3=已归档）
   */
  evaluationStatus: number;

  /**
   * 整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 评价项目明细列表（主子表关系） （子表：TaktSupplierEvaluationItem）
   */
  items?: SupplierEvaluationItem[];

}


/**
 * SupplierEvaluation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SupplierEvaluationQuery
 * @description 对应后端 TaktSupplierEvaluationQueryDto
 */
export interface SupplierEvaluationQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 评价表编号（组合唯一索引）
   */
  supplierEvaluationCode?: string;

  /**
   * 供应商ID（序列化为string以避免Javascript精度问题）
   */
  supplierId?: string;

  /**
   * 供应商名称
   */
  supplierName?: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 评价日期（范围查询-开始）
   */
  evaluationDateStart?: string;

  /**
   * 评价日期（范围查询-结束）
   */
  evaluationDateEnd?: string;

  /**
   * 评价周期（0=月度，1=季度，2=半年度，3=年度）
   */
  evaluationPeriod?: number;

  /**
   * 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
   */
  evaluationType?: number;

  /**
   * 评价人（人员代码）
   */
  evaluatorBy?: string;

  /**
   * 评价部门
   */
  evaluationDept?: string;

  /**
   * 总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  overallRating?: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 价格评分（0-100分）
   */
  priceScore?: number;

  /**
   * 服务评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 技术能力评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 主要优点
   */
  mainStrengths?: string;

  /**
   * 主要问题/不足
   */
  mainIssues?: string;

  /**
   * 改进要求/建议
   */
  improvementRequirements?: string;

  /**
   * 考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）
   */
  evaluationConclusion?: number;

  /**
   * 整改期限（要求完成日期）（范围查询-开始）
   */
  rectificationDeadlineStart?: string;

  /**
   * 整改期限（要求完成日期）（范围查询-结束）
   */
  rectificationDeadlineEnd?: string;

  /**
   * 评价状态（0=草稿，1=评价中，2=已完成，3=已归档）
   */
  evaluationStatus?: number;

  /**
   * 整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus?: number;

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
 * 创建SupplierEvaluation DTO
 * 对应前端 SupplierEvaluationCreate
 * @description 对应后端 TaktSupplierEvaluationCreateDto
 */
export interface SupplierEvaluationCreate {
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
   * 评价表编号（组合唯一索引）
   */
  supplierEvaluationCode: string;

  /**
   * 供应商ID（序列化为string以避免Javascript精度问题）
   */
  supplierId: string;

  /**
   * 供应商名称
   */
  supplierName: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 评价日期
   */
  evaluationDate: string;

  /**
   * 评价周期（0=月度，1=季度，2=半年度，3=年度）
   */
  evaluationPeriod: number;

  /**
   * 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
   */
  evaluationType: number;

  /**
   * 评价人（人员代码）
   */
  evaluatorBy?: string;

  /**
   * 评价部门
   */
  evaluationDept?: string;

  /**
   * 总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  overallRating: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 价格评分（0-100分）
   */
  priceScore?: number;

  /**
   * 服务评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 技术能力评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 主要优点
   */
  mainStrengths?: string;

  /**
   * 主要问题/不足
   */
  mainIssues?: string;

  /**
   * 改进要求/建议
   */
  improvementRequirements?: string;

  /**
   * 考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）
   */
  evaluationConclusion: number;

  /**
   * 整改期限（要求完成日期）
   */
  rectificationDeadline?: string;

  /**
   * 评价状态（0=草稿，1=评价中，2=已完成，3=已归档）
   */
  evaluationStatus: number;

  /**
   * 整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 评价项目明细列表（主子表关系）（子表，级联保存）
   */
  items?: SupplierEvaluationItemCreate[];

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
 * 更新SupplierEvaluation DTO
 * 继承 TaktSupplierEvaluationCreateDto，添加 SupplierEvaluationId 字段
 * 对应前端 SupplierEvaluationUpdate
 * @description 对应后端 TaktSupplierEvaluationUpdateDto
 */
export interface SupplierEvaluationUpdate extends SupplierEvaluationCreate {
  /**
   * SupplierEvaluationID（标识要更新的实体）
   */
  supplierEvaluationId: string;

}


/**
 * SupplierEvaluation 状态更新 DTO
 * 对应前端 SupplierEvaluationStatus
 * @description 对应后端 TaktSupplierEvaluationStatusDto
 */
export interface SupplierEvaluationStatus {
  /**
   * SupplierEvaluationID
   */
  supplierEvaluationId: string;

  /**
   * 评价状态（0=草稿，1=评价中，2=已完成，3=已归档）
   */
  evaluationStatus: number;

}


/**
 * SupplierEvaluation 排序更新 DTO
 * 对应前端 SupplierEvaluationSort
 * @description 对应后端 TaktSupplierEvaluationSortDto
 */
export interface SupplierEvaluationSort {
  /**
   * SupplierEvaluationID
   */
  supplierEvaluationId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * SupplierEvaluation 导入模板行 DTO
 * 对应前端 SupplierEvaluationTemplate
 * @description 对应后端 TaktSupplierEvaluationTemplateDto
 */
export interface SupplierEvaluationTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 评价表编号（组合唯一索引）
   */
  supplierEvaluationCode?: string;

  /**
   * 供应商ID（序列化为string以避免Javascript精度问题）
   */
  supplierId?: string;

  /**
   * 供应商名称
   */
  supplierName?: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 评价日期
   */
  evaluationDate?: string;

  /**
   * 评价周期（0=月度，1=季度，2=半年度，3=年度）
   */
  evaluationPeriod?: number;

  /**
   * 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
   */
  evaluationType?: number;

  /**
   * 评价人（人员代码）
   */
  evaluatorBy?: string;

  /**
   * 评价部门
   */
  evaluationDept?: string;

  /**
   * 总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  overallRating?: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 价格评分（0-100分）
   */
  priceScore?: number;

  /**
   * 服务评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 技术能力评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 主要优点
   */
  mainStrengths?: string;

  /**
   * 主要问题/不足
   */
  mainIssues?: string;

  /**
   * 改进要求/建议
   */
  improvementRequirements?: string;

  /**
   * 考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）
   */
  evaluationConclusion?: number;

  /**
   * 整改期限（要求完成日期）
   */
  rectificationDeadline?: string;

  /**
   * 评价状态（0=草稿，1=评价中，2=已完成，3=已归档）
   */
  evaluationStatus?: number;

  /**
   * 整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 评价项目明细列表（主子表关系）（子表，级联保存）
   */
  items?: SupplierEvaluationItemCreate[];

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
 * SupplierEvaluation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SupplierEvaluationImport
 * @description 对应后端 TaktSupplierEvaluationImportDto
 */
export interface SupplierEvaluationImport {
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
   * 评价表编号（组合唯一索引）
   */
  supplierEvaluationCode?: string;

  /**
   * 供应商ID（序列化为string以避免Javascript精度问题）
   */
  supplierId?: string;

  /**
   * 供应商名称
   */
  supplierName?: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 评价日期
   */
  evaluationDate?: string;

  /**
   * 评价周期（0=月度，1=季度，2=半年度，3=年度）
   */
  evaluationPeriod?: number;

  /**
   * 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
   */
  evaluationType?: number;

  /**
   * 评价人（人员代码）
   */
  evaluatorBy?: string;

  /**
   * 评价部门
   */
  evaluationDept?: string;

  /**
   * 总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  overallRating?: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 价格评分（0-100分）
   */
  priceScore?: number;

  /**
   * 服务评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 技术能力评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 主要优点
   */
  mainStrengths?: string;

  /**
   * 主要问题/不足
   */
  mainIssues?: string;

  /**
   * 改进要求/建议
   */
  improvementRequirements?: string;

  /**
   * 考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）
   */
  evaluationConclusion?: number;

  /**
   * 整改期限（要求完成日期）
   */
  rectificationDeadline?: string;

  /**
   * 评价状态（0=草稿，1=评价中，2=已完成，3=已归档）
   */
  evaluationStatus?: number;

  /**
   * 整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 评价项目明细列表（主子表关系）（子表，级联保存）
   */
  items?: SupplierEvaluationItemCreate[];

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
 * SupplierEvaluation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SupplierEvaluationExport
 * @description 对应后端 TaktSupplierEvaluationExportDto
 */
export interface SupplierEvaluationExport {
  /**
   * SupplierEvaluationID
   */
  supplierEvaluationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 评价表编号（组合唯一索引）
   */
  supplierEvaluationCode: string;

  /**
   * 供应商ID（序列化为string以避免Javascript精度问题）
   */
  supplierId: string;

  /**
   * 供应商名称
   */
  supplierName: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 评价日期
   */
  evaluationDate: string;

  /**
   * 评价周期（0=月度，1=季度，2=半年度，3=年度）
   */
  evaluationPeriod: number;

  /**
   * 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
   */
  evaluationType: number;

  /**
   * 评价人（人员代码）
   */
  evaluatorBy?: string;

  /**
   * 评价部门
   */
  evaluationDept?: string;

  /**
   * 总体评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  overallRating: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 价格评分（0-100分）
   */
  priceScore?: number;

  /**
   * 服务评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 技术能力评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 主要优点
   */
  mainStrengths?: string;

  /**
   * 主要问题/不足
   */
  mainIssues?: string;

  /**
   * 改进要求/建议
   */
  improvementRequirements?: string;

  /**
   * 考核结论（0=继续合作，1=限期整改，2=减少订单，3=暂停合作，4=取消资格）
   */
  evaluationConclusion: number;

  /**
   * 整改期限（要求完成日期）
   */
  rectificationDeadline?: string;

  /**
   * 评价状态（0=草稿，1=评价中，2=已完成，3=已归档）
   */
  evaluationStatus: number;

  /**
   * 整改跟进状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus: number;

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

