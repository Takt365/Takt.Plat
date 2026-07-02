// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/visitor-center
// 文件名称：visitor-companion.d.ts
// 创建时间：2026-06-23
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
 * 来访人员子实体（部门、职称、姓名）
 * 对应前端 TaktVisitorCompanionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 VisitorCompanion
 * @description 对应后端 TaktVisitorCompanionDto
 */
export interface VisitorCompanion extends CompanyDtoBase {
  /**
   * VisitorCompanionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  visitorCompanionId: string;

  /**
   * 来访记录 ID
   */
  visitorId: string;

  /**
   * 来访记录 名称（填充字段）
   */
  visitorName?: string;

  /**
   * 部门
   */
  department: string;

  /**
   * 职称
   */
  jobTitle: string;

  /**
   * 来访人员姓名
   */
  companionName: string;

  /**
   * 来访记录（主表） （主表：TaktVisitor）
   */
  visitor?: Visitor;

}


/**
 * VisitorCompanion 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 VisitorCompanionQuery
 * @description 对应后端 TaktVisitorCompanionQueryDto
 */
export interface VisitorCompanionQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 来访记录 ID
   */
  visitorId?: string;

  /**
   * 部门
   */
  department?: string;

  /**
   * 职称
   */
  jobTitle?: string;

  /**
   * 来访人员姓名
   */
  companionName?: string;

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
 * 创建VisitorCompanion DTO
 * 对应前端 VisitorCompanionCreate
 * @description 对应后端 TaktVisitorCompanionCreateDto
 */
export interface VisitorCompanionCreate {
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
   * 来访记录 ID
   */
  visitorId: string;

  /**
   * 部门
   */
  department: string;

  /**
   * 职称
   */
  jobTitle: string;

  /**
   * 来访人员姓名
   */
  companionName: string;

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
 * 更新VisitorCompanion DTO
 * 继承 TaktVisitorCompanionCreateDto，添加 VisitorCompanionId 字段
 * 对应前端 VisitorCompanionUpdate
 * @description 对应后端 TaktVisitorCompanionUpdateDto
 */
export interface VisitorCompanionUpdate extends VisitorCompanionCreate {
  /**
   * VisitorCompanionID（标识要更新的实体）
   */
  visitorCompanionId: string;

}


/**
 * VisitorCompanion 导入模板行 DTO
 * 对应前端 VisitorCompanionTemplate
 * @description 对应后端 TaktVisitorCompanionTemplateDto
 */
export interface VisitorCompanionTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 来访记录 ID
   */
  visitorId?: string;

  /**
   * 部门
   */
  department?: string;

  /**
   * 职称
   */
  jobTitle?: string;

  /**
   * 来访人员姓名
   */
  companionName?: string;

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
 * VisitorCompanion 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 VisitorCompanionImport
 * @description 对应后端 TaktVisitorCompanionImportDto
 */
export interface VisitorCompanionImport {
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
   * 来访记录 ID
   */
  visitorId?: string;

  /**
   * 部门
   */
  department?: string;

  /**
   * 职称
   */
  jobTitle?: string;

  /**
   * 来访人员姓名
   */
  companionName?: string;

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
 * VisitorCompanion 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 VisitorCompanionExport
 * @description 对应后端 TaktVisitorCompanionExportDto
 */
export interface VisitorCompanionExport {
  /**
   * VisitorCompanionID
   */
  visitorCompanionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 来访记录 ID
   */
  visitorId: string;

  /**
   * 部门
   */
  department: string;

  /**
   * 职称
   */
  jobTitle: string;

  /**
   * 来访人员姓名
   */
  companionName: string;

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

