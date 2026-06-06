// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-operation-reliability.d.ts
// 创建时间：2026-06-06
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
 * 对应前端 TaktQualityOperationReliabilityDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityOperationReliability
 * @description 对应后端 TaktQualityOperationReliabilityDto
 */
export interface QualityOperationReliability extends CompanyDtoBase {
  /**
   * QualityOperationReliabilityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityOperationReliabilityId: string;

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
   * 品质业务主表(导航属性) （主表：TaktQualityOperation）
   */
  operation?: QualityOperation;

}


/**
 * QualityOperationReliability 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityOperationReliabilityQuery
 * @description 对应后端 TaktQualityOperationReliabilityQueryDto
 */
export interface QualityOperationReliabilityQuery extends TaktPagedQuery {
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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建QualityOperationReliability DTO
 * 对应前端 QualityOperationReliabilityCreate
 * @description 对应后端 TaktQualityOperationReliabilityCreateDto
 */
export interface QualityOperationReliabilityCreate {
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
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新QualityOperationReliability DTO
 * 继承 TaktQualityOperationReliabilityCreateDto，添加 QualityOperationReliabilityId 字段
 * 对应前端 QualityOperationReliabilityUpdate
 * @description 对应后端 TaktQualityOperationReliabilityUpdateDto
 */
export interface QualityOperationReliabilityUpdate extends QualityOperationReliabilityCreate {
  /**
   * QualityOperationReliabilityID（标识要更新的实体）
   */
  qualityOperationReliabilityId: string;

}


/**
 * QualityOperationReliability 导入模板行 DTO
 * 对应前端 QualityOperationReliabilityTemplate
 * @description 对应后端 TaktQualityOperationReliabilityTemplateDto
 */
export interface QualityOperationReliabilityTemplate {
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
   * 评价作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 信赖性评价备注
   */
  reliabilityNote?: string;

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
 * QualityOperationReliability 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityOperationReliabilityImport
 * @description 对应后端 TaktQualityOperationReliabilityImportDto
 */
export interface QualityOperationReliabilityImport {
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
   * 评价作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 信赖性评价备注
   */
  reliabilityNote?: string;

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
 * QualityOperationReliability 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityOperationReliabilityExport
 * @description 对应后端 TaktQualityOperationReliabilityExportDto
 */
export interface QualityOperationReliabilityExport {
  /**
   * QualityOperationReliabilityID
   */
  qualityOperationReliabilityId: string;

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

