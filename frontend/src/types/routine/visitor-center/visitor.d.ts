// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/visitor-center
// 文件名称：visitor.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/visitor-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 来访接待主实体（来访公司及参访起止时间）
 * 对应前端 TaktVisitorDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Visitor
 * @description 对应后端 TaktVisitorDto
 */
export interface Visitor extends CompanyDtoBase {
  /**
   * VisitorID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  visitorId: string;

  /**
   * 来访公司名称
   */
  visitorCompanyName: string;

  /**
   * 参访开始时间
   */
  visitStartTime: string;

  /**
   * 参访结束时间
   */
  visitEndTime: string;

  /**
   * 来访人员列表 （子表：TaktVisitorCompanion）
   */
  companions?: VisitorCompanion[];

}


/**
 * Visitor 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 VisitorQuery
 * @description 对应后端 TaktVisitorQueryDto
 */
export interface VisitorQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 来访公司名称
   */
  visitorCompanyName?: string;

  /**
   * 参访开始时间（范围查询-开始）
   */
  visitStartTimeStart?: string;

  /**
   * 参访开始时间（范围查询-结束）
   */
  visitStartTimeEnd?: string;

  /**
   * 参访结束时间（范围查询-开始）
   */
  visitEndTimeStart?: string;

  /**
   * 参访结束时间（范围查询-结束）
   */
  visitEndTimeEnd?: string;

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
 * 创建Visitor DTO
 * 对应前端 VisitorCreate
 * @description 对应后端 TaktVisitorCreateDto
 */
export interface VisitorCreate {
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
   * 来访公司名称
   */
  visitorCompanyName: string;

  /**
   * 参访开始时间
   */
  visitStartTime: string;

  /**
   * 参访结束时间
   */
  visitEndTime: string;

  /**
   * 来访人员列表（子表，级联保存）
   */
  companions?: VisitorCompanionCreate[];

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
 * 更新Visitor DTO
 * 继承 TaktVisitorCreateDto，添加 VisitorId 字段
 * 对应前端 VisitorUpdate
 * @description 对应后端 TaktVisitorUpdateDto
 */
export interface VisitorUpdate extends VisitorCreate {
  /**
   * VisitorID（标识要更新的实体）
   */
  visitorId: string;

}


/**
 * Visitor 导入模板行 DTO
 * 对应前端 VisitorTemplate
 * @description 对应后端 TaktVisitorTemplateDto
 */
export interface VisitorTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 来访公司名称
   */
  visitorCompanyName?: string;

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
 * Visitor 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 VisitorImport
 * @description 对应后端 TaktVisitorImportDto
 */
export interface VisitorImport {
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
   * 来访公司名称
   */
  visitorCompanyName?: string;

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
 * Visitor 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 VisitorExport
 * @description 对应后端 TaktVisitorExportDto
 */
export interface VisitorExport {
  /**
   * VisitorID
   */
  visitorId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 来访公司名称
   */
  visitorCompanyName: string;

  /**
   * 参访开始时间
   */
  visitStartTime: string;

  /**
   * 参访结束时间
   */
  visitEndTime: string;

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

