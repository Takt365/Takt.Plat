// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：assurance-other.d.ts
// 创建时间：2026-07-23
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
 * 品质业务明细 - 其他通常业务费用
 * 对应前端 TaktQualityAssuranceOtherDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityAssuranceOther
 * @description 对应后端 TaktQualityAssuranceOtherDto
 */
export interface QualityAssuranceOther extends CompanyDtoBase {
  /**
   * QualityAssuranceOtherID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityAssuranceOtherId: string;

  /**
   * 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
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
   * 项号（如10, 20, 30，步长严格为10）
   */
  lineNumber: number;

  /**
   * 其他通常业务费用(元)
   */
  operationsCost: number;

  /**
   * 通常业务作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 通常业务其他费用(元)
   */
  otherExpenses: number;

  /**
   * 通常业务其他备注
   */
  otherNote?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 品质业务主表(导航属性) （主表：TaktQualityAssurance）
   */
  operation?: QualityAssurance;

}


/**
 * QualityAssuranceOther 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityAssuranceOtherQuery
 * @description 对应后端 TaktQualityAssuranceOtherQueryDto
 */
export interface QualityAssuranceOtherQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
   */
  qualityAssuranceId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode?: string;

  /**
   * 项号（如10, 20, 30，步长严格为10）
   */
  lineNumber?: number;

  /**
   * 其他通常业务费用(元)
   */
  operationsCost?: number;

  /**
   * 通常业务作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 通常业务其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 通常业务其他备注
   */
  otherNote?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
 * 创建QualityAssuranceOther DTO
 * 对应前端 QualityAssuranceOtherCreate
 * @description 对应后端 TaktQualityAssuranceOtherCreateDto
 */
export interface QualityAssuranceOtherCreate {
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
   * 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
   */
  qualityAssuranceId: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode: string;

  /**
   * 项号（如10, 20, 30，步长严格为10）
   */
  lineNumber: number;

  /**
   * 其他通常业务费用(元)
   */
  operationsCost: number;

  /**
   * 通常业务作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 通常业务其他费用(元)
   */
  otherExpenses: number;

  /**
   * 通常业务其他备注
   */
  otherNote?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
 * 更新QualityAssuranceOther DTO
 * 继承 TaktQualityAssuranceOtherCreateDto，添加 QualityAssuranceOtherId 字段
 * 对应前端 QualityAssuranceOtherUpdate
 * @description 对应后端 TaktQualityAssuranceOtherUpdateDto
 */
export interface QualityAssuranceOtherUpdate extends QualityAssuranceOtherCreate {
  /**
   * QualityAssuranceOtherID（标识要更新的实体）
   */
  qualityAssuranceOtherId: string;

}


/**
 * QualityAssuranceOther 作废/撤销作废 DTO
 * 对应前端 QualityAssuranceOtherObsolete
 * @description 对应后端 TaktQualityAssuranceOtherObsoleteDto
 */
export interface QualityAssuranceOtherObsolete {
  /**
   * QualityAssuranceOtherID
   */
  qualityAssuranceOtherId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * QualityAssuranceOther 导入模板行 DTO
 * 对应前端 QualityAssuranceOtherTemplate
 * @description 对应后端 TaktQualityAssuranceOtherTemplateDto
 */
export interface QualityAssuranceOtherTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
   */
  qualityAssuranceId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode?: string;

  /**
   * 项号（如10, 20, 30，步长严格为10）
   */
  lineNumber?: number;

  /**
   * 其他通常业务费用(元)
   */
  operationsCost?: number;

  /**
   * 通常业务作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 通常业务其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 通常业务其他备注
   */
  otherNote?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
 * QualityAssuranceOther 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityAssuranceOtherImport
 * @description 对应后端 TaktQualityAssuranceOtherImportDto
 */
export interface QualityAssuranceOtherImport {
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
   * 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
   */
  qualityAssuranceId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode?: string;

  /**
   * 项号（如10, 20, 30，步长严格为10）
   */
  lineNumber?: number;

  /**
   * 其他通常业务费用(元)
   */
  operationsCost?: number;

  /**
   * 通常业务作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 通常业务其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 通常业务其他备注
   */
  otherNote?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
 * QualityAssuranceOther 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityAssuranceOtherExport
 * @description 对应后端 TaktQualityAssuranceOtherExportDto
 */
export interface QualityAssuranceOtherExport {
  /**
   * QualityAssuranceOtherID
   */
  qualityAssuranceOtherId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
   */
  qualityAssuranceId: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode: string;

  /**
   * 项号（如10, 20, 30，步长严格为10）
   */
  lineNumber: number;

  /**
   * 其他通常业务费用(元)
   */
  operationsCost: number;

  /**
   * 通常业务作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 通常业务其他费用(元)
   */
  otherExpenses: number;

  /**
   * 通常业务其他备注
   */
  otherNote?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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

