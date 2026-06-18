// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-operation-first-article.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 品质业务明细 - 初期检定・定期检定费用
 * 对应前端 TaktQualityOperationFirstArticleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityOperationFirstArticle
 * @description 对应后端 TaktQualityOperationFirstArticleDto
 */
export interface QualityOperationFirstArticle extends CompanyDtoBase {
  /**
   * QualityOperationFirstArticleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityOperationFirstArticleId: string;

  /**
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityOperationId: string;

  /**
   * 品质业务主表名称（填充字段）
   */
  qualityOperationName?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityOperationCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 初期检定・定期检定业务费用(元)
   */
  qualificationCost: number;

  /**
   * 检定作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 检定其他费用(元)
   */
  otherExpenses: number;

  /**
   * 检定备注
   */
  qualificationNote?: string;

  /**
   * 品质业务主表(导航属性) （主表：TaktQualityOperation）
   */
  operation?: QualityOperation;

}


/**
 * QualityOperationFirstArticle 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityOperationFirstArticleQuery
 * @description 对应后端 TaktQualityOperationFirstArticleQueryDto
 */
export interface QualityOperationFirstArticleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityOperationId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityOperationCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 初期检定・定期检定业务费用(元)
   */
  qualificationCost?: number;

  /**
   * 检定作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 检定其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 检定备注
   */
  qualificationNote?: string;

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
 * 创建QualityOperationFirstArticle DTO
 * 对应前端 QualityOperationFirstArticleCreate
 * @description 对应后端 TaktQualityOperationFirstArticleCreateDto
 */
export interface QualityOperationFirstArticleCreate {
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
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityOperationId: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityOperationCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 初期检定・定期检定业务费用(元)
   */
  qualificationCost: number;

  /**
   * 检定作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 检定其他费用(元)
   */
  otherExpenses: number;

  /**
   * 检定备注
   */
  qualificationNote?: string;

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
 * 更新QualityOperationFirstArticle DTO
 * 继承 TaktQualityOperationFirstArticleCreateDto，添加 QualityOperationFirstArticleId 字段
 * 对应前端 QualityOperationFirstArticleUpdate
 * @description 对应后端 TaktQualityOperationFirstArticleUpdateDto
 */
export interface QualityOperationFirstArticleUpdate extends QualityOperationFirstArticleCreate {
  /**
   * QualityOperationFirstArticleID（标识要更新的实体）
   */
  qualityOperationFirstArticleId: string;

}


/**
 * QualityOperationFirstArticle 导入模板行 DTO
 * 对应前端 QualityOperationFirstArticleTemplate
 * @description 对应后端 TaktQualityOperationFirstArticleTemplateDto
 */
export interface QualityOperationFirstArticleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityOperationId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityOperationCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 检定作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 检定备注
   */
  qualificationNote?: string;

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
 * QualityOperationFirstArticle 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityOperationFirstArticleImport
 * @description 对应后端 TaktQualityOperationFirstArticleImportDto
 */
export interface QualityOperationFirstArticleImport {
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
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityOperationId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityOperationCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 检定作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 检定备注
   */
  qualificationNote?: string;

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
 * QualityOperationFirstArticle 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityOperationFirstArticleExport
 * @description 对应后端 TaktQualityOperationFirstArticleExportDto
 */
export interface QualityOperationFirstArticleExport {
  /**
   * QualityOperationFirstArticleID
   */
  qualityOperationFirstArticleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityOperationId: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityOperationCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 初期检定・定期检定业务费用(元)
   */
  qualificationCost: number;

  /**
   * 检定作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 检定其他费用(元)
   */
  otherExpenses: number;

  /**
   * 检定备注
   */
  qualificationNote?: string;

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

