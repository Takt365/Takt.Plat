// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：self-service.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 服务台自助服务项实体
 * 对应前端 TaktSelfServiceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SelfService
 * @description 对应后端 TaktSelfServiceDto
 */
export interface SelfService extends CompanyDtoBase {
  /**
   * SelfServiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  selfServiceId: string;

  /**
   * 自助服务名称
   */
  serviceName: string;

  /**
   * 服务类型（0=链接，1=表单，2=知识引导）
   */
  serviceType: number;

  /**
   * 描述
   */
  description?: string;

  /**
   * 链接地址或表单编码
   */
  linkOrCode?: string;

  /**
   * 图标或图片 URL
   */
  iconUrl?: string;

  /**
   * 自助服务状态（1=启用，0=禁用）
   */
  selfServiceStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * SelfService 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SelfServiceQuery
 * @description 对应后端 TaktSelfServiceQueryDto
 */
export interface SelfServiceQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 自助服务名称
   */
  serviceName?: string;

  /**
   * 服务类型（0=链接，1=表单，2=知识引导）
   */
  serviceType?: number;

  /**
   * 描述
   */
  description?: string;

  /**
   * 链接地址或表单编码
   */
  linkOrCode?: string;

  /**
   * 图标或图片 URL
   */
  iconUrl?: string;

  /**
   * 自助服务状态（1=启用，0=禁用）
   */
  selfServiceStatus?: number;

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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建SelfService DTO
 * 对应前端 SelfServiceCreate
 * @description 对应后端 TaktSelfServiceCreateDto
 */
export interface SelfServiceCreate {
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
   * 自助服务名称
   */
  serviceName: string;

  /**
   * 服务类型（0=链接，1=表单，2=知识引导）
   */
  serviceType: number;

  /**
   * 描述
   */
  description?: string;

  /**
   * 链接地址或表单编码
   */
  linkOrCode?: string;

  /**
   * 图标或图片 URL
   */
  iconUrl?: string;

  /**
   * 自助服务状态（1=启用，0=禁用）
   */
  selfServiceStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新SelfService DTO
 * 继承 TaktSelfServiceCreateDto，添加 SelfServiceId 字段
 * 对应前端 SelfServiceUpdate
 * @description 对应后端 TaktSelfServiceUpdateDto
 */
export interface SelfServiceUpdate extends SelfServiceCreate {
  /**
   * SelfServiceID（标识要更新的实体）
   */
  selfServiceId: string;

}


/**
 * SelfService 状态更新 DTO
 * 对应前端 SelfServiceStatus
 * @description 对应后端 TaktSelfServiceStatusDto
 */
export interface SelfServiceStatus {
  /**
   * SelfServiceID
   */
  selfServiceId: string;

  /**
   * 自助服务状态（1=启用，0=禁用）
   */
  selfServiceStatus: number;

}


/**
 * SelfService 排序更新 DTO
 * 对应前端 SelfServiceSort
 * @description 对应后端 TaktSelfServiceSortDto
 */
export interface SelfServiceSort {
  /**
   * SelfServiceID
   */
  selfServiceId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * SelfService 导入模板行 DTO
 * 对应前端 SelfServiceTemplate
 * @description 对应后端 TaktSelfServiceTemplateDto
 */
export interface SelfServiceTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 自助服务名称
   */
  serviceName?: string;

  /**
   * 服务类型（0=链接，1=表单，2=知识引导）
   */
  serviceType?: number;

  /**
   * 描述
   */
  description?: string;

  /**
   * 链接地址或表单编码
   */
  linkOrCode?: string;

  /**
   * 图标或图片 URL
   */
  iconUrl?: string;

  /**
   * 自助服务状态（1=启用，0=禁用）
   */
  selfServiceStatus?: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * SelfService 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SelfServiceImport
 * @description 对应后端 TaktSelfServiceImportDto
 */
export interface SelfServiceImport {
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
   * 自助服务名称
   */
  serviceName?: string;

  /**
   * 服务类型（0=链接，1=表单，2=知识引导）
   */
  serviceType?: number;

  /**
   * 描述
   */
  description?: string;

  /**
   * 链接地址或表单编码
   */
  linkOrCode?: string;

  /**
   * 图标或图片 URL
   */
  iconUrl?: string;

  /**
   * 自助服务状态（1=启用，0=禁用）
   */
  selfServiceStatus?: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * SelfService 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SelfServiceExport
 * @description 对应后端 TaktSelfServiceExportDto
 */
export interface SelfServiceExport {
  /**
   * SelfServiceID
   */
  selfServiceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 自助服务名称
   */
  serviceName: string;

  /**
   * 服务类型（0=链接，1=表单，2=知识引导）
   */
  serviceType: number;

  /**
   * 描述
   */
  description?: string;

  /**
   * 链接地址或表单编码
   */
  linkOrCode?: string;

  /**
   * 图标或图片 URL
   */
  iconUrl?: string;

  /**
   * 自助服务状态（1=启用，0=禁用）
   */
  selfServiceStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

