// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：news-attachment.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/news-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 新闻中心附件实体
 * 对应前端 TaktNewsAttachmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 NewsAttachment
 * @description 对应后端 TaktNewsAttachmentDto
 */
export interface NewsAttachment extends CompanyDtoBase {
  /**
   * NewsAttachmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  newsAttachmentId: string;

  /**
   * 新闻 ID
   */
  newsId: string;

  /**
   * 新闻 名称（填充字段）
   */
  newsName?: string;

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
   * 文件类型（MIME 类型）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 新闻（主表） （主表：TaktNews）
   */
  news?: News;

}


/**
 * NewsAttachment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 NewsAttachmentQuery
 * @description 对应后端 TaktNewsAttachmentQueryDto
 */
export interface NewsAttachmentQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 新闻 ID
   */
  newsId?: string;

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
   * 文件类型（MIME 类型）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

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
 * 创建NewsAttachment DTO
 * 对应前端 NewsAttachmentCreate
 * @description 对应后端 TaktNewsAttachmentCreateDto
 */
export interface NewsAttachmentCreate {
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
   * 新闻 ID
   */
  newsId: string;

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
   * 文件类型（MIME 类型）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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
 * 更新NewsAttachment DTO
 * 继承 TaktNewsAttachmentCreateDto，添加 NewsAttachmentId 字段
 * 对应前端 NewsAttachmentUpdate
 * @description 对应后端 TaktNewsAttachmentUpdateDto
 */
export interface NewsAttachmentUpdate extends NewsAttachmentCreate {
  /**
   * NewsAttachmentID（标识要更新的实体）
   */
  newsAttachmentId: string;

}


/**
 * NewsAttachment 排序更新 DTO
 * 对应前端 NewsAttachmentSort
 * @description 对应后端 TaktNewsAttachmentSortDto
 */
export interface NewsAttachmentSort {
  /**
   * NewsAttachmentID
   */
  newsAttachmentId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * NewsAttachment 导入模板行 DTO
 * 对应前端 NewsAttachmentTemplate
 * @description 对应后端 TaktNewsAttachmentTemplateDto
 */
export interface NewsAttachmentTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 新闻 ID
   */
  newsId?: string;

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
   * 文件类型（MIME 类型）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

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
 * NewsAttachment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 NewsAttachmentImport
 * @description 对应后端 TaktNewsAttachmentImportDto
 */
export interface NewsAttachmentImport {
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
   * 新闻 ID
   */
  newsId?: string;

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
   * 文件类型（MIME 类型）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

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
 * NewsAttachment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsAttachmentExport
 * @description 对应后端 TaktNewsAttachmentExportDto
 */
export interface NewsAttachmentExport {
  /**
   * NewsAttachmentID
   */
  newsAttachmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 新闻 ID
   */
  newsId: string;

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
   * 文件类型（MIME 类型）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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

