// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：news-read.d.ts
// 创建时间：2026-06-06
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
 * 新闻中心阅读记录实体
 * 对应前端 TaktNewsReadDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 NewsRead
 * @description 对应后端 TaktNewsReadDto
 */
export interface NewsRead extends CompanyDtoBase {
  /**
   * NewsReadID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  newsReadId: string;

  /**
   * 新闻 ID
   */
  newsId: string;

  /**
   * 新闻 名称（填充字段）
   */
  newsName?: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 用户姓名
   */
  userName: string;

  /**
   * 阅读时间
   */
  readTime: string;

  /**
   * 新闻（主表） （主表：TaktNews）
   */
  news?: News;

}


/**
 * NewsRead 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 NewsReadQuery
 * @description 对应后端 TaktNewsReadQueryDto
 */
export interface NewsReadQuery extends TaktPagedQuery {
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
   * 用户 ID
   */
  userId?: string;

  /**
   * 用户姓名
   */
  userName?: string;

  /**
   * 阅读时间（范围查询-开始）
   */
  readTimeStart?: string;

  /**
   * 阅读时间（范围查询-结束）
   */
  readTimeEnd?: string;

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
 * 创建NewsRead DTO
 * 对应前端 NewsReadCreate
 * @description 对应后端 TaktNewsReadCreateDto
 */
export interface NewsReadCreate {
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
   * 用户 ID
   */
  userId: string;

  /**
   * 用户姓名
   */
  userName: string;

  /**
   * 阅读时间
   */
  readTime: string;

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
 * 更新NewsRead DTO
 * 继承 TaktNewsReadCreateDto，添加 NewsReadId 字段
 * 对应前端 NewsReadUpdate
 * @description 对应后端 TaktNewsReadUpdateDto
 */
export interface NewsReadUpdate extends NewsReadCreate {
  /**
   * NewsReadID（标识要更新的实体）
   */
  newsReadId: string;

}


/**
 * NewsRead 导入模板行 DTO
 * 对应前端 NewsReadTemplate
 * @description 对应后端 TaktNewsReadTemplateDto
 */
export interface NewsReadTemplate {
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
   * 用户 ID
   */
  userId?: string;

  /**
   * 用户姓名
   */
  userName?: string;

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
 * NewsRead 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 NewsReadImport
 * @description 对应后端 TaktNewsReadImportDto
 */
export interface NewsReadImport {
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
   * 用户 ID
   */
  userId?: string;

  /**
   * 用户姓名
   */
  userName?: string;

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
 * NewsRead 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsReadExport
 * @description 对应后端 TaktNewsReadExportDto
 */
export interface NewsReadExport {
  /**
   * NewsReadID
   */
  newsReadId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 新闻 ID
   */
  newsId: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 用户姓名
   */
  userName: string;

  /**
   * 阅读时间
   */
  readTime: string;

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

