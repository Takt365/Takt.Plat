// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/document-center
// 文件名称：document-version.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/document-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 文管文档版本子实体
 * 对应前端 TaktDocumentVersionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 DocumentVersion
 * @description 对应后端 TaktDocumentVersionDto
 */
export interface DocumentVersion extends CompanyDtoBase {
  /**
   * DocumentVersionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  documentVersionId: string;

  /**
   * 文档 ID
   */
  documentId: string;

  /**
   * 文档 名称（填充字段）
   */
  documentName?: string;

  /**
   * 版本号
   */
  versionNo: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID
   */
  fileId: string;

  /**
   * 文件名称
   */
  fileName: string;

  /**
   * 文件路径
   */
  filePath: string;

  /**
   * 文件大小（字节）
   */
  fileSize: string;

  /**
   * 文件类型（MIME）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 修订人 ID
   */
  revisedBy: string;

  /**
   * 修订人姓名
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt: string;

  /**
   * 文档（主表） （主表：TaktDocument）
   */
  document?: Document;

}


/**
 * DocumentVersion 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DocumentVersionQuery
 * @description 对应后端 TaktDocumentVersionQueryDto
 */
export interface DocumentVersionQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 文档 ID
   */
  documentId?: string;

  /**
   * 版本号
   */
  versionNo?: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID
   */
  fileId?: string;

  /**
   * 文件名称
   */
  fileName?: string;

  /**
   * 文件路径
   */
  filePath?: string;

  /**
   * 文件大小（字节）
   */
  fileSize?: string;

  /**
   * 文件类型（MIME）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 修订人 ID
   */
  revisedBy?: string;

  /**
   * 修订人姓名
   */
  revisedByName?: string;

  /**
   * 修订时间（范围查询-开始）
   */
  revisedAtStart?: string;

  /**
   * 修订时间（范围查询-结束）
   */
  revisedAtEnd?: string;

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
 * 创建DocumentVersion DTO
 * 对应前端 DocumentVersionCreate
 * @description 对应后端 TaktDocumentVersionCreateDto
 */
export interface DocumentVersionCreate {
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
   * 文档 ID
   */
  documentId: string;

  /**
   * 版本号
   */
  versionNo: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID
   */
  fileId: string;

  /**
   * 文件名称
   */
  fileName: string;

  /**
   * 文件路径
   */
  filePath: string;

  /**
   * 文件大小（字节）
   */
  fileSize: string;

  /**
   * 文件类型（MIME）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 修订人 ID
   */
  revisedBy: string;

  /**
   * 修订人姓名
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt: string;

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
 * 更新DocumentVersion DTO
 * 继承 TaktDocumentVersionCreateDto，添加 DocumentVersionId 字段
 * 对应前端 DocumentVersionUpdate
 * @description 对应后端 TaktDocumentVersionUpdateDto
 */
export interface DocumentVersionUpdate extends DocumentVersionCreate {
  /**
   * DocumentVersionID（标识要更新的实体）
   */
  documentVersionId: string;

}


/**
 * DocumentVersion 导入模板行 DTO
 * 对应前端 DocumentVersionTemplate
 * @description 对应后端 TaktDocumentVersionTemplateDto
 */
export interface DocumentVersionTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 文档 ID
   */
  documentId?: string;

  /**
   * 版本号
   */
  versionNo?: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID
   */
  fileId?: string;

  /**
   * 文件名称
   */
  fileName?: string;

  /**
   * 文件路径
   */
  filePath?: string;

  /**
   * 文件大小（字节）
   */
  fileSize?: string;

  /**
   * 文件类型（MIME）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 修订人 ID
   */
  revisedBy?: string;

  /**
   * 修订人姓名
   */
  revisedByName?: string;

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
 * DocumentVersion 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 DocumentVersionImport
 * @description 对应后端 TaktDocumentVersionImportDto
 */
export interface DocumentVersionImport {
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
   * 文档 ID
   */
  documentId?: string;

  /**
   * 版本号
   */
  versionNo?: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID
   */
  fileId?: string;

  /**
   * 文件名称
   */
  fileName?: string;

  /**
   * 文件路径
   */
  filePath?: string;

  /**
   * 文件大小（字节）
   */
  fileSize?: string;

  /**
   * 文件类型（MIME）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 修订人 ID
   */
  revisedBy?: string;

  /**
   * 修订人姓名
   */
  revisedByName?: string;

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
 * DocumentVersion 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DocumentVersionExport
 * @description 对应后端 TaktDocumentVersionExportDto
 */
export interface DocumentVersionExport {
  /**
   * DocumentVersionID
   */
  documentVersionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 文档 ID
   */
  documentId: string;

  /**
   * 版本号
   */
  versionNo: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID
   */
  fileId: string;

  /**
   * 文件名称
   */
  fileName: string;

  /**
   * 文件路径
   */
  filePath: string;

  /**
   * 文件大小（字节）
   */
  fileSize: string;

  /**
   * 文件类型（MIME）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 修订人 ID
   */
  revisedBy: string;

  /**
   * 修订人姓名
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt: string;

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

