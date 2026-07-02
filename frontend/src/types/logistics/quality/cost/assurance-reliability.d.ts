// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：assurance-reliability.d.ts
// 创建时间：2026-06-23
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
 * 品质业务明细 - 信赖性评价/ORT费用
 * 对应前端 TaktQualityAssuranceReliabilityDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityAssuranceReliability
 * @description 对应后端 TaktQualityAssuranceReliabilityDto
 */
export interface QualityAssuranceReliability extends CompanyDtoBase {
  /**
   * QualityAssuranceReliabilityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityAssuranceReliabilityId: string;

  /**
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityAssuranceId: string;

  /**
   * 品质业务主表名称（填充字段）
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
   * 信赖性评价・ORT业务费用(元)
   */
  testCost: number;

  /**
   * 评价作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 评价其他费用(元)
   */
  otherExpenses: number;

  /**
   * 信赖性评价备注
   */
  reliabilityNote?: string;

  /**
   * 品质业务主表(导航属性) （主表：TaktQualityAssurance）
   */
  operation?: QualityAssurance;

}


/**
 * QualityAssuranceReliability 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityAssuranceReliabilityQuery
 * @description 对应后端 TaktQualityAssuranceReliabilityQueryDto
 */
export interface QualityAssuranceReliabilityQuery extends TaktPagedQuery {
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
   * 信赖性评价・ORT业务费用(元)
   */
  testCost?: number;

  /**
   * 评价作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 评价其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 信赖性评价备注
   */
  reliabilityNote?: string;

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
 * 创建QualityAssuranceReliability DTO
 * 对应前端 QualityAssuranceReliabilityCreate
 * @description 对应后端 TaktQualityAssuranceReliabilityCreateDto
 */
export interface QualityAssuranceReliabilityCreate {
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
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
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
   * 信赖性评价・ORT业务费用(元)
   */
  testCost: number;

  /**
   * 评价作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 评价其他费用(元)
   */
  otherExpenses: number;

  /**
   * 信赖性评价备注
   */
  reliabilityNote?: string;

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
 * 更新QualityAssuranceReliability DTO
 * 继承 TaktQualityAssuranceReliabilityCreateDto，添加 QualityAssuranceReliabilityId 字段
 * 对应前端 QualityAssuranceReliabilityUpdate
 * @description 对应后端 TaktQualityAssuranceReliabilityUpdateDto
 */
export interface QualityAssuranceReliabilityUpdate extends QualityAssuranceReliabilityCreate {
  /**
   * QualityAssuranceReliabilityID（标识要更新的实体）
   */
  qualityAssuranceReliabilityId: string;

}


/**
 * QualityAssuranceReliability 导入模板行 DTO
 * 对应前端 QualityAssuranceReliabilityTemplate
 * @description 对应后端 TaktQualityAssuranceReliabilityTemplateDto
 */
export interface QualityAssuranceReliabilityTemplate {
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
   * 信赖性评价・ORT业务费用(元)
   */
  testCost?: number;

  /**
   * 评价作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 评价其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 信赖性评价备注
   */
  reliabilityNote?: string;

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
 * QualityAssuranceReliability 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityAssuranceReliabilityImport
 * @description 对应后端 TaktQualityAssuranceReliabilityImportDto
 */
export interface QualityAssuranceReliabilityImport {
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
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
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
   * 信赖性评价・ORT业务费用(元)
   */
  testCost?: number;

  /**
   * 评价作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 评价其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 信赖性评价备注
   */
  reliabilityNote?: string;

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
 * QualityAssuranceReliability 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityAssuranceReliabilityExport
 * @description 对应后端 TaktQualityAssuranceReliabilityExportDto
 */
export interface QualityAssuranceReliabilityExport {
  /**
   * QualityAssuranceReliabilityID
   */
  qualityAssuranceReliabilityId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)
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
   * 信赖性评价・ORT业务费用(元)
   */
  testCost: number;

  /**
   * 评价作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 评价其他费用(元)
   */
  otherExpenses: number;

  /**
   * 信赖性评价备注
   */
  reliabilityNote?: string;

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

