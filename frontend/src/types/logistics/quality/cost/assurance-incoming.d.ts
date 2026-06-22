// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：assurance-incoming.d.ts
// 创建时间：2026-06-21
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
 * 品质业务明细 - 来料检验费用
 * 对应前端 TaktQualityAssuranceIncomingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityAssuranceIncoming
 * @description 对应后端 TaktQualityAssuranceIncomingDto
 */
export interface QualityAssuranceIncoming extends CompanyDtoBase {
  /**
   * QualityAssuranceIncomingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityAssuranceIncomingId: string;

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
   * 直接人员费率(元/分钟)
   */
  directManpowerCostPerMinute: number;

  /**
   * 来料检验业务费用(元)
   */
  incomingInspectionCost: number;

  /**
   * 检查时间(分钟)
   */
  inspectionTimeMinutes: number;

  /**
   * 交通费、旅费(元)
   */
  travelCost: number;

  /**
   * 检查其他费用(元)
   */
  otherExpenses: number;

  /**
   * 来料检验备注
   */
  incomingNote?: string;

  /**
   * 品质业务主表(导航属性) （主表：TaktQualityAssurance）
   */
  operation?: QualityAssurance;

}


/**
 * QualityAssuranceIncoming 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityAssuranceIncomingQuery
 * @description 对应后端 TaktQualityAssuranceIncomingQueryDto
 */
export interface QualityAssuranceIncomingQuery extends TaktPagedQuery {
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
   * 直接人员费率(元/分钟)
   */
  directManpowerCostPerMinute?: number;

  /**
   * 来料检验业务费用(元)
   */
  incomingInspectionCost?: number;

  /**
   * 检查时间(分钟)
   */
  inspectionTimeMinutes?: number;

  /**
   * 交通费、旅费(元)
   */
  travelCost?: number;

  /**
   * 检查其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 来料检验备注
   */
  incomingNote?: string;

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
 * 创建QualityAssuranceIncoming DTO
 * 对应前端 QualityAssuranceIncomingCreate
 * @description 对应后端 TaktQualityAssuranceIncomingCreateDto
 */
export interface QualityAssuranceIncomingCreate {
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
   * 直接人员费率(元/分钟)
   */
  directManpowerCostPerMinute: number;

  /**
   * 来料检验业务费用(元)
   */
  incomingInspectionCost: number;

  /**
   * 检查时间(分钟)
   */
  inspectionTimeMinutes: number;

  /**
   * 交通费、旅费(元)
   */
  travelCost: number;

  /**
   * 检查其他费用(元)
   */
  otherExpenses: number;

  /**
   * 来料检验备注
   */
  incomingNote?: string;

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
 * 更新QualityAssuranceIncoming DTO
 * 继承 TaktQualityAssuranceIncomingCreateDto，添加 QualityAssuranceIncomingId 字段
 * 对应前端 QualityAssuranceIncomingUpdate
 * @description 对应后端 TaktQualityAssuranceIncomingUpdateDto
 */
export interface QualityAssuranceIncomingUpdate extends QualityAssuranceIncomingCreate {
  /**
   * QualityAssuranceIncomingID（标识要更新的实体）
   */
  qualityAssuranceIncomingId: string;

}


/**
 * QualityAssuranceIncoming 导入模板行 DTO
 * 对应前端 QualityAssuranceIncomingTemplate
 * @description 对应后端 TaktQualityAssuranceIncomingTemplateDto
 */
export interface QualityAssuranceIncomingTemplate {
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
   * 检查时间(分钟)
   */
  inspectionTimeMinutes?: number;

  /**
   * 来料检验备注
   */
  incomingNote?: string;

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
 * QualityAssuranceIncoming 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityAssuranceIncomingImport
 * @description 对应后端 TaktQualityAssuranceIncomingImportDto
 */
export interface QualityAssuranceIncomingImport {
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
   * 检查时间(分钟)
   */
  inspectionTimeMinutes?: number;

  /**
   * 来料检验备注
   */
  incomingNote?: string;

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
 * QualityAssuranceIncoming 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityAssuranceIncomingExport
 * @description 对应后端 TaktQualityAssuranceIncomingExportDto
 */
export interface QualityAssuranceIncomingExport {
  /**
   * QualityAssuranceIncomingID
   */
  qualityAssuranceIncomingId: string;

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
   * 直接人员费率(元/分钟)
   */
  directManpowerCostPerMinute: number;

  /**
   * 来料检验业务费用(元)
   */
  incomingInspectionCost: number;

  /**
   * 检查时间(分钟)
   */
  inspectionTimeMinutes: number;

  /**
   * 交通费、旅费(元)
   */
  travelCost: number;

  /**
   * 检查其他费用(元)
   */
  otherExpenses: number;

  /**
   * 来料检验备注
   */
  incomingNote?: string;

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

