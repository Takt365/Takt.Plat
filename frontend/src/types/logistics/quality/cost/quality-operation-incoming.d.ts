// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-operation-incoming.d.ts
// 创建时间：2026-06-05
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
 * 对应前端 TaktQualityOperationIncomingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityOperationIncoming
 * @description 对应后端 TaktQualityOperationIncomingDto
 */
export interface QualityOperationIncoming extends CompanyDtoBase {
  /**
   * QualityOperationIncomingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityOperationIncomingId: string;

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
   * 品质业务主表(导航属性) （主表：TaktQualityOperation）
   */
  operation?: QualityOperation;

}


/**
 * QualityOperationIncoming 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityOperationIncomingQuery
 * @description 对应后端 TaktQualityOperationIncomingQueryDto
 */
export interface QualityOperationIncomingQuery extends TaktPagedQuery {
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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建QualityOperationIncoming DTO
 * 对应前端 QualityOperationIncomingCreate
 * @description 对应后端 TaktQualityOperationIncomingCreateDto
 */
export interface QualityOperationIncomingCreate {
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
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新QualityOperationIncoming DTO
 * 继承 TaktQualityOperationIncomingCreateDto，添加 QualityOperationIncomingId 字段
 * 对应前端 QualityOperationIncomingUpdate
 * @description 对应后端 TaktQualityOperationIncomingUpdateDto
 */
export interface QualityOperationIncomingUpdate extends QualityOperationIncomingCreate {
  /**
   * QualityOperationIncomingID（标识要更新的实体）
   */
  qualityOperationIncomingId: string;

}


/**
 * QualityOperationIncoming 导入模板行 DTO
 * 对应前端 QualityOperationIncomingTemplate
 * @description 对应后端 TaktQualityOperationIncomingTemplateDto
 */
export interface QualityOperationIncomingTemplate {
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
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * QualityOperationIncoming 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityOperationIncomingImport
 * @description 对应后端 TaktQualityOperationIncomingImportDto
 */
export interface QualityOperationIncomingImport {
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
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * QualityOperationIncoming 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityOperationIncomingExport
 * @description 对应后端 TaktQualityOperationIncomingExportDto
 */
export interface QualityOperationIncomingExport {
  /**
   * QualityOperationIncomingID
   */
  qualityOperationIncomingId: string;

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

