// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：assurance-first-article.d.ts
// 创建时间：2026-07-09
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
 * 对应前端 TaktQualityAssuranceFirstArticleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityAssuranceFirstArticle
 * @description 对应后端 TaktQualityAssuranceFirstArticleDto
 */
export interface QualityAssuranceFirstArticle extends CompanyDtoBase {
  /**
   * QualityAssuranceFirstArticleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityAssuranceFirstArticleId: string;

  /**
   * 品质业务主表 ID（关联 TaktQualityAssurance.Id，选项 TaktQualityAssurances/options）
   */
  qualityAssuranceId: string;

  /**
   * 品质业务主表 名称（填充字段）
   */
  qualityAssuranceName?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode: string;

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
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 品质业务主表(导航属性) （主表：TaktQualityAssurance）
   */
  operation?: QualityAssurance;

}


/**
 * QualityAssuranceFirstArticle 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityAssuranceFirstArticleQuery
 * @description 对应后端 TaktQualityAssuranceFirstArticleQueryDto
 */
export interface QualityAssuranceFirstArticleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 品质业务主表 ID（关联 TaktQualityAssurance.Id，选项 TaktQualityAssurances/options）
   */
  qualityAssuranceId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode?: string;

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
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建QualityAssuranceFirstArticle DTO
 * 对应前端 QualityAssuranceFirstArticleCreate
 * @description 对应后端 TaktQualityAssuranceFirstArticleCreateDto
 */
export interface QualityAssuranceFirstArticleCreate {
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
   * 品质业务主表 ID（关联 TaktQualityAssurance.Id，选项 TaktQualityAssurances/options）
   */
  qualityAssuranceId: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode: string;

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
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新QualityAssuranceFirstArticle DTO
 * 继承 TaktQualityAssuranceFirstArticleCreateDto，添加 QualityAssuranceFirstArticleId 字段
 * 对应前端 QualityAssuranceFirstArticleUpdate
 * @description 对应后端 TaktQualityAssuranceFirstArticleUpdateDto
 */
export interface QualityAssuranceFirstArticleUpdate extends QualityAssuranceFirstArticleCreate {
  /**
   * QualityAssuranceFirstArticleID（标识要更新的实体）
   */
  qualityAssuranceFirstArticleId: string;

}


/**
 * QualityAssuranceFirstArticle 作废/撤销作废 DTO
 * 对应前端 QualityAssuranceFirstArticleObsolete
 * @description 对应后端 TaktQualityAssuranceFirstArticleObsoleteDto
 */
export interface QualityAssuranceFirstArticleObsolete {
  /**
   * QualityAssuranceFirstArticleID
   */
  qualityAssuranceFirstArticleId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * QualityAssuranceFirstArticle 导入模板行 DTO
 * 对应前端 QualityAssuranceFirstArticleTemplate
 * @description 对应后端 TaktQualityAssuranceFirstArticleTemplateDto
 */
export interface QualityAssuranceFirstArticleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 品质业务主表 ID（关联 TaktQualityAssurance.Id，选项 TaktQualityAssurances/options）
   */
  qualityAssuranceId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode?: string;

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
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * QualityAssuranceFirstArticle 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityAssuranceFirstArticleImport
 * @description 对应后端 TaktQualityAssuranceFirstArticleImportDto
 */
export interface QualityAssuranceFirstArticleImport {
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
   * 品质业务主表 ID（关联 TaktQualityAssurance.Id，选项 TaktQualityAssurances/options）
   */
  qualityAssuranceId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode?: string;

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
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * QualityAssuranceFirstArticle 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityAssuranceFirstArticleExport
 * @description 对应后端 TaktQualityAssuranceFirstArticleExportDto
 */
export interface QualityAssuranceFirstArticleExport {
  /**
   * QualityAssuranceFirstArticleID
   */
  qualityAssuranceFirstArticleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质业务主表 ID（关联 TaktQualityAssurance.Id，选项 TaktQualityAssurances/options）
   */
  qualityAssuranceId: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode: string;

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
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

