// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-failure-assy-rework.d.ts
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
 * 品质问题应对明细 - 组装不良改修应对(组装选别・改修费用)
 * 对应前端 TaktQualityFailureAssyReworkDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityFailureAssyRework
 * @description 对应后端 TaktQualityFailureAssyReworkDto
 */
export interface QualityFailureAssyRework extends CompanyDtoBase {
  /**
   * QualityFailureAssyReworkID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityFailureAssyReworkId: string;

  /**
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId: string;

  /**
   * 品质问题主表名称（填充字段）
   */
  qualityFailureName?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost: number;

  /**
   * 组装顾客名
   */
  assyCustomerName?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

  /**
   * 品质问题主表(导航属性) （主表：TaktQualityFailure）
   */
  issue?: QualityFailure;

}


/**
 * QualityFailureAssyRework 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityFailureAssyReworkQuery
 * @description 对应后端 TaktQualityFailureAssyReworkQueryDto
 */
export interface QualityFailureAssyReworkQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost?: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes?: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes?: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost?: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost?: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses?: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost?: number;

  /**
   * 组装顾客名
   */
  assyCustomerName?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2?: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

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
 * 创建QualityFailureAssyRework DTO
 * 对应前端 QualityFailureAssyReworkCreate
 * @description 对应后端 TaktQualityFailureAssyReworkCreateDto
 */
export interface QualityFailureAssyReworkCreate {
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
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost: number;

  /**
   * 组装顾客名
   */
  assyCustomerName?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

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
 * 更新QualityFailureAssyRework DTO
 * 继承 TaktQualityFailureAssyReworkCreateDto，添加 QualityFailureAssyReworkId 字段
 * 对应前端 QualityFailureAssyReworkUpdate
 * @description 对应后端 TaktQualityFailureAssyReworkUpdateDto
 */
export interface QualityFailureAssyReworkUpdate extends QualityFailureAssyReworkCreate {
  /**
   * QualityFailureAssyReworkID（标识要更新的实体）
   */
  qualityFailureAssyReworkId: string;

}


/**
 * QualityFailureAssyRework 导入模板行 DTO
 * 对应前端 QualityFailureAssyReworkTemplate
 * @description 对应后端 TaktQualityFailureAssyReworkTemplateDto
 */
export interface QualityFailureAssyReworkTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes?: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes?: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装顾客名
   */
  assyCustomerName?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

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
 * QualityFailureAssyRework 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityFailureAssyReworkImport
 * @description 对应后端 TaktQualityFailureAssyReworkImportDto
 */
export interface QualityFailureAssyReworkImport {
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
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes?: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes?: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装顾客名
   */
  assyCustomerName?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

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
 * QualityFailureAssyRework 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityFailureAssyReworkExport
 * @description 对应后端 TaktQualityFailureAssyReworkExportDto
 */
export interface QualityFailureAssyReworkExport {
  /**
   * QualityFailureAssyReworkID
   */
  qualityFailureAssyReworkId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityFailureId: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityFailureCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost: number;

  /**
   * 组装顾客名
   */
  assyCustomerName?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

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

