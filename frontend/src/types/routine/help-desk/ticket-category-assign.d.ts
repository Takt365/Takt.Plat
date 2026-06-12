// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticket-category-assign.d.ts
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
 * 工单分类默认处理人（按 CategoryCode 自动分配处理人）
 * 对应前端 TaktTicketCategoryAssignDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TicketCategoryAssign
 * @description 对应后端 TaktTicketCategoryAssignDto
 */
export interface TicketCategoryAssign extends CompanyDtoBase {
  /**
   * TicketCategoryAssignID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ticketCategoryAssignId: string;

  /**
   * 分类编码（与 TaktTicket.CategoryCode 对应）
   */
  categoryCode: string;

  /**
   * 默认处理人 ID
   */
  assigneeId: string;

  /**
   * 默认处理人姓名
   */
  assigneeName?: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * TicketCategoryAssign 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TicketCategoryAssignQuery
 * @description 对应后端 TaktTicketCategoryAssignQueryDto
 */
export interface TicketCategoryAssignQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 分类编码（与 TaktTicket.CategoryCode 对应）
   */
  categoryCode?: string;

  /**
   * 默认处理人 ID
   */
  assigneeId?: string;

  /**
   * 默认处理人姓名
   */
  assigneeName?: string;

  /**
   * 排序号
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
 * 创建TicketCategoryAssign DTO
 * 对应前端 TicketCategoryAssignCreate
 * @description 对应后端 TaktTicketCategoryAssignCreateDto
 */
export interface TicketCategoryAssignCreate {
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
   * 分类编码（与 TaktTicket.CategoryCode 对应）
   */
  categoryCode: string;

  /**
   * 默认处理人 ID
   */
  assigneeId: string;

  /**
   * 默认处理人姓名
   */
  assigneeName?: string;

  /**
   * 排序号
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
 * 更新TicketCategoryAssign DTO
 * 继承 TaktTicketCategoryAssignCreateDto，添加 TicketCategoryAssignId 字段
 * 对应前端 TicketCategoryAssignUpdate
 * @description 对应后端 TaktTicketCategoryAssignUpdateDto
 */
export interface TicketCategoryAssignUpdate extends TicketCategoryAssignCreate {
  /**
   * TicketCategoryAssignID（标识要更新的实体）
   */
  ticketCategoryAssignId: string;

}


/**
 * TicketCategoryAssign 排序更新 DTO
 * 对应前端 TicketCategoryAssignSort
 * @description 对应后端 TaktTicketCategoryAssignSortDto
 */
export interface TicketCategoryAssignSort {
  /**
   * TicketCategoryAssignID
   */
  ticketCategoryAssignId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * TicketCategoryAssign 导入模板行 DTO
 * 对应前端 TicketCategoryAssignTemplate
 * @description 对应后端 TaktTicketCategoryAssignTemplateDto
 */
export interface TicketCategoryAssignTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 分类编码（与 TaktTicket.CategoryCode 对应）
   */
  categoryCode?: string;

  /**
   * 默认处理人 ID
   */
  assigneeId?: string;

  /**
   * 默认处理人姓名
   */
  assigneeName?: string;

  /**
   * 排序号
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
 * TicketCategoryAssign 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TicketCategoryAssignImport
 * @description 对应后端 TaktTicketCategoryAssignImportDto
 */
export interface TicketCategoryAssignImport {
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
   * 分类编码（与 TaktTicket.CategoryCode 对应）
   */
  categoryCode?: string;

  /**
   * 默认处理人 ID
   */
  assigneeId?: string;

  /**
   * 默认处理人姓名
   */
  assigneeName?: string;

  /**
   * 排序号
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
 * TicketCategoryAssign 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TicketCategoryAssignExport
 * @description 对应后端 TaktTicketCategoryAssignExportDto
 */
export interface TicketCategoryAssignExport {
  /**
   * TicketCategoryAssignID
   */
  ticketCategoryAssignId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 分类编码（与 TaktTicket.CategoryCode 对应）
   */
  categoryCode: string;

  /**
   * 默认处理人 ID
   */
  assigneeId: string;

  /**
   * 默认处理人姓名
   */
  assigneeName?: string;

  /**
   * 排序号
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

