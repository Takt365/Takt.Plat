// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-attachment.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 设变附件实体。文件类别：Liaison/EPP/FPP/ExternalLiaison/TCJ 等；文件编号为联络编号等。
 * 对应前端 TaktEcAttachmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcAttachment
 * @description 对应后端 TaktEcAttachmentDto
 */
export interface EcAttachment extends CompanyDtoBase {
  /**
   * EcAttachmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecAttachmentId: string;

  /**
   * 设变主表ID
   */
  ecId: string;

  /**
   * 设变主表名称（填充字段）
   */
  ecName?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
   */
  attachmentType: string;

  /**
   * 文件编号（如联络编号等）
   */
  docNo: string;

  /**
   * 文件名称
   */
  fileName: string;

  /**
   * 访问地址（URL）
   */
  accessUrl: string;

  /**
   * 设变主表（多对一） （主表：TaktEc）
   */
  ec?: Ec;

}


/**
 * EcAttachment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcAttachmentQuery
 * @description 对应后端 TaktEcAttachmentQueryDto
 */
export interface EcAttachmentQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 设变主表ID
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
   */
  attachmentType?: string;

  /**
   * 文件编号（如联络编号等）
   */
  docNo?: string;

  /**
   * 文件名称
   */
  fileName?: string;

  /**
   * 访问地址（URL）
   */
  accessUrl?: string;

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
 * 创建EcAttachment DTO
 * 对应前端 EcAttachmentCreate
 * @description 对应后端 TaktEcAttachmentCreateDto
 */
export interface EcAttachmentCreate {
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
   * 设变主表ID
   */
  ecId: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
   */
  attachmentType: string;

  /**
   * 文件编号（如联络编号等）
   */
  docNo: string;

  /**
   * 文件名称
   */
  fileName: string;

  /**
   * 访问地址（URL）
   */
  accessUrl: string;

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
 * 更新EcAttachment DTO
 * 继承 TaktEcAttachmentCreateDto，添加 EcAttachmentId 字段
 * 对应前端 EcAttachmentUpdate
 * @description 对应后端 TaktEcAttachmentUpdateDto
 */
export interface EcAttachmentUpdate extends EcAttachmentCreate {
  /**
   * EcAttachmentID（标识要更新的实体）
   */
  ecAttachmentId: string;

}


/**
 * EcAttachment 导入模板行 DTO
 * 对应前端 EcAttachmentTemplate
 * @description 对应后端 TaktEcAttachmentTemplateDto
 */
export interface EcAttachmentTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 设变主表ID
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
   */
  attachmentType?: string;

  /**
   * 文件编号（如联络编号等）
   */
  docNo?: string;

  /**
   * 文件名称
   */
  fileName?: string;

  /**
   * 访问地址（URL）
   */
  accessUrl?: string;

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
 * EcAttachment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcAttachmentImport
 * @description 对应后端 TaktEcAttachmentImportDto
 */
export interface EcAttachmentImport {
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
   * 设变主表ID
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
   */
  attachmentType?: string;

  /**
   * 文件编号（如联络编号等）
   */
  docNo?: string;

  /**
   * 文件名称
   */
  fileName?: string;

  /**
   * 访问地址（URL）
   */
  accessUrl?: string;

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
 * EcAttachment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcAttachmentExport
 * @description 对应后端 TaktEcAttachmentExportDto
 */
export interface EcAttachmentExport {
  /**
   * EcAttachmentID
   */
  ecAttachmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 设变主表ID
   */
  ecId: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等
   */
  attachmentType: string;

  /**
   * 文件编号（如联络编号等）
   */
  docNo: string;

  /**
   * 文件名称
   */
  fileName: string;

  /**
   * 访问地址（URL）
   */
  accessUrl: string;

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

