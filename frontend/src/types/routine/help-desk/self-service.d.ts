// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：self-service.d.ts
// 创建时间：2026-06-24
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
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

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
  selfServiceDescription?: string;

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
   * 扩展字段JSON
   */
  extField?: string;

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
  selfServiceDescription?: string;

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

