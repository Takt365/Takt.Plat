// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-failure-pcba-rework.d.ts
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
 * 品质问题应对明细 - PCBA不良改修应对(PCBA选别・改修费用)
 * 对应前端 TaktQualityFailurePcbaReworkDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityFailurePcbaRework
 * @description 对应后端 TaktQualityFailurePcbaReworkDto
 */
export interface QualityFailurePcbaRework extends CompanyDtoBase {
  /**
   * QualityFailurePcbaReworkID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityFailurePcbaReworkId: string;

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
   * PCBA不良内容(Parts/Components)
   */
  pcbaDefectParts?: string;

  /**
   * PCBA选别・改修费用（元）
   */
  pcbaReworkCost: number;

  /**
   * PCBA选别・改修时间（分钟）
   */
  pcbaReworkTimeMinutes: number;

  /**
   * PCBA再检查时间（分钟）
   */
  pcbaReinspectionTimeMinutes: number;

  /**
   * PCBA交通费、旅费（元）
   */
  pcbaTravelCost: number;

  /**
   * PCBA仓库管理费（元）
   */
  pcbaWarehouseCost: number;

  /**
   * PCBA选别・改修其他费用（元）
   */
  pcbaOtherExpenses: number;

  /**
   * PCBA选别・改修备注
   */
  pcbaReworkNote?: string;

  /**
   * PCBA向顾客的费用请求（元）
   */
  pcbaScrapCost: number;

  /**
   * PCBA顾客名
   */
  pcbaCustomerName?: string;

  /**
   * PCBA Debit Note No
   */
  pcbaDebitNoteNo?: string;

  /**
   * PCBA其他费用（元）
   */
  pcbaOtherExpenses2: number;

  /**
   * PCBA备注
   */
  pcbaNote?: string;

  /**
   * PCBA不良改修应对记录者
   */
  pcbaRecorder?: string;

  /**
   * 质量问题主表（导航属性） （主表：TaktQualityFailure）
   */
  issue?: QualityFailure;

}


/**
 * QualityFailurePcbaRework 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityFailurePcbaReworkQuery
 * @description 对应后端 TaktQualityFailurePcbaReworkQueryDto
 */
export interface QualityFailurePcbaReworkQuery extends TaktPagedQuery {
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
   * PCBA不良内容(Parts/Components)
   */
  pcbaDefectParts?: string;

  /**
   * PCBA选别・改修费用（元）
   */
  pcbaReworkCost?: number;

  /**
   * PCBA选别・改修时间（分钟）
   */
  pcbaReworkTimeMinutes?: number;

  /**
   * PCBA再检查时间（分钟）
   */
  pcbaReinspectionTimeMinutes?: number;

  /**
   * PCBA交通费、旅费（元）
   */
  pcbaTravelCost?: number;

  /**
   * PCBA仓库管理费（元）
   */
  pcbaWarehouseCost?: number;

  /**
   * PCBA选别・改修其他费用（元）
   */
  pcbaOtherExpenses?: number;

  /**
   * PCBA选别・改修备注
   */
  pcbaReworkNote?: string;

  /**
   * PCBA向顾客的费用请求（元）
   */
  pcbaScrapCost?: number;

  /**
   * PCBA顾客名
   */
  pcbaCustomerName?: string;

  /**
   * PCBA Debit Note No
   */
  pcbaDebitNoteNo?: string;

  /**
   * PCBA其他费用（元）
   */
  pcbaOtherExpenses2?: number;

  /**
   * PCBA备注
   */
  pcbaNote?: string;

  /**
   * PCBA不良改修应对记录者
   */
  pcbaRecorder?: string;

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
 * 创建QualityFailurePcbaRework DTO
 * 对应前端 QualityFailurePcbaReworkCreate
 * @description 对应后端 TaktQualityFailurePcbaReworkCreateDto
 */
export interface QualityFailurePcbaReworkCreate {
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
   * PCBA不良内容(Parts/Components)
   */
  pcbaDefectParts?: string;

  /**
   * PCBA选别・改修费用（元）
   */
  pcbaReworkCost: number;

  /**
   * PCBA选别・改修时间（分钟）
   */
  pcbaReworkTimeMinutes: number;

  /**
   * PCBA再检查时间（分钟）
   */
  pcbaReinspectionTimeMinutes: number;

  /**
   * PCBA交通费、旅费（元）
   */
  pcbaTravelCost: number;

  /**
   * PCBA仓库管理费（元）
   */
  pcbaWarehouseCost: number;

  /**
   * PCBA选别・改修其他费用（元）
   */
  pcbaOtherExpenses: number;

  /**
   * PCBA选别・改修备注
   */
  pcbaReworkNote?: string;

  /**
   * PCBA向顾客的费用请求（元）
   */
  pcbaScrapCost: number;

  /**
   * PCBA顾客名
   */
  pcbaCustomerName?: string;

  /**
   * PCBA Debit Note No
   */
  pcbaDebitNoteNo?: string;

  /**
   * PCBA其他费用（元）
   */
  pcbaOtherExpenses2: number;

  /**
   * PCBA备注
   */
  pcbaNote?: string;

  /**
   * PCBA不良改修应对记录者
   */
  pcbaRecorder?: string;

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
 * 更新QualityFailurePcbaRework DTO
 * 继承 TaktQualityFailurePcbaReworkCreateDto，添加 QualityFailurePcbaReworkId 字段
 * 对应前端 QualityFailurePcbaReworkUpdate
 * @description 对应后端 TaktQualityFailurePcbaReworkUpdateDto
 */
export interface QualityFailurePcbaReworkUpdate extends QualityFailurePcbaReworkCreate {
  /**
   * QualityFailurePcbaReworkID（标识要更新的实体）
   */
  qualityFailurePcbaReworkId: string;

}


/**
 * QualityFailurePcbaRework 导入模板行 DTO
 * 对应前端 QualityFailurePcbaReworkTemplate
 * @description 对应后端 TaktQualityFailurePcbaReworkTemplateDto
 */
export interface QualityFailurePcbaReworkTemplate {
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
   * PCBA不良内容(Parts/Components)
   */
  pcbaDefectParts?: string;

  /**
   * PCBA选别・改修时间（分钟）
   */
  pcbaReworkTimeMinutes?: number;

  /**
   * PCBA再检查时间（分钟）
   */
  pcbaReinspectionTimeMinutes?: number;

  /**
   * PCBA选别・改修备注
   */
  pcbaReworkNote?: string;

  /**
   * PCBA顾客名
   */
  pcbaCustomerName?: string;

  /**
   * PCBA Debit Note No
   */
  pcbaDebitNoteNo?: string;

  /**
   * PCBA备注
   */
  pcbaNote?: string;

  /**
   * PCBA不良改修应对记录者
   */
  pcbaRecorder?: string;

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
 * QualityFailurePcbaRework 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityFailurePcbaReworkImport
 * @description 对应后端 TaktQualityFailurePcbaReworkImportDto
 */
export interface QualityFailurePcbaReworkImport {
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
   * PCBA不良内容(Parts/Components)
   */
  pcbaDefectParts?: string;

  /**
   * PCBA选别・改修时间（分钟）
   */
  pcbaReworkTimeMinutes?: number;

  /**
   * PCBA再检查时间（分钟）
   */
  pcbaReinspectionTimeMinutes?: number;

  /**
   * PCBA选别・改修备注
   */
  pcbaReworkNote?: string;

  /**
   * PCBA顾客名
   */
  pcbaCustomerName?: string;

  /**
   * PCBA Debit Note No
   */
  pcbaDebitNoteNo?: string;

  /**
   * PCBA备注
   */
  pcbaNote?: string;

  /**
   * PCBA不良改修应对记录者
   */
  pcbaRecorder?: string;

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
 * QualityFailurePcbaRework 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityFailurePcbaReworkExport
 * @description 对应后端 TaktQualityFailurePcbaReworkExportDto
 */
export interface QualityFailurePcbaReworkExport {
  /**
   * QualityFailurePcbaReworkID
   */
  qualityFailurePcbaReworkId: string;

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
   * PCBA不良内容(Parts/Components)
   */
  pcbaDefectParts?: string;

  /**
   * PCBA选别・改修费用（元）
   */
  pcbaReworkCost: number;

  /**
   * PCBA选别・改修时间（分钟）
   */
  pcbaReworkTimeMinutes: number;

  /**
   * PCBA再检查时间（分钟）
   */
  pcbaReinspectionTimeMinutes: number;

  /**
   * PCBA交通费、旅费（元）
   */
  pcbaTravelCost: number;

  /**
   * PCBA仓库管理费（元）
   */
  pcbaWarehouseCost: number;

  /**
   * PCBA选别・改修其他费用（元）
   */
  pcbaOtherExpenses: number;

  /**
   * PCBA选别・改修备注
   */
  pcbaReworkNote?: string;

  /**
   * PCBA向顾客的费用请求（元）
   */
  pcbaScrapCost: number;

  /**
   * PCBA顾客名
   */
  pcbaCustomerName?: string;

  /**
   * PCBA Debit Note No
   */
  pcbaDebitNoteNo?: string;

  /**
   * PCBA其他费用（元）
   */
  pcbaOtherExpenses2: number;

  /**
   * PCBA备注
   */
  pcbaNote?: string;

  /**
   * PCBA不良改修应对记录者
   */
  pcbaRecorder?: string;

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

