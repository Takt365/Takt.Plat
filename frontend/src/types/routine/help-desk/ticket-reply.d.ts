// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticket-reply.d.ts
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
 * 工单回复实体（用户与客服会话）
 * 对应前端 TaktTicketReplyDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TicketReply
 * @description 对应后端 TaktTicketReplyDto
 */
export interface TicketReply extends CompanyDtoBase {
  /**
   * TicketReplyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ticketReplyId: string;

  /**
   * 工单 ID
   */
  ticketId: string;

  /**
   * 工单 名称（填充字段）
   */
  ticketName?: string;

  /**
   * 作者类型（0=客服，1=用户，2=系统）
   */
  authorType: number;

  /**
   * 作者用户 ID
   */
  authorId: string;

  /**
   * 作者姓名
   */
  authorName?: string;

  /**
   * 回复内容
   */
  ticketReplyContent: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 是否内部备注（仅客服可见）
   */
  isInternal: number;

  /**
   * 工单（主表） （主表：TaktTicket）
   */
  ticket?: Ticket;

}


/**
 * TicketReply 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TicketReplyQuery
 * @description 对应后端 TaktTicketReplyQueryDto
 */
export interface TicketReplyQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工单 ID
   */
  ticketId?: string;

  /**
   * 作者类型（0=客服，1=用户，2=系统）
   */
  authorType?: number;

  /**
   * 作者用户 ID
   */
  authorId?: string;

  /**
   * 作者姓名
   */
  authorName?: string;

  /**
   * 回复内容
   */
  ticketReplyContent?: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 是否内部备注（仅客服可见）
   */
  isInternal?: number;

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
 * 创建TicketReply DTO
 * 对应前端 TicketReplyCreate
 * @description 对应后端 TaktTicketReplyCreateDto
 */
export interface TicketReplyCreate {
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
   * 工单 ID
   */
  ticketId: string;

  /**
   * 作者类型（0=客服，1=用户，2=系统）
   */
  authorType: number;

  /**
   * 作者用户 ID
   */
  authorId: string;

  /**
   * 作者姓名
   */
  authorName?: string;

  /**
   * 回复内容
   */
  ticketReplyContent: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 是否内部备注（仅客服可见）
   */
  isInternal: number;

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
 * 更新TicketReply DTO
 * 继承 TaktTicketReplyCreateDto，添加 TicketReplyId 字段
 * 对应前端 TicketReplyUpdate
 * @description 对应后端 TaktTicketReplyUpdateDto
 */
export interface TicketReplyUpdate extends TicketReplyCreate {
  /**
   * TicketReplyID（标识要更新的实体）
   */
  ticketReplyId: string;

}


/**
 * TicketReply 导入模板行 DTO
 * 对应前端 TicketReplyTemplate
 * @description 对应后端 TaktTicketReplyTemplateDto
 */
export interface TicketReplyTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工单 ID
   */
  ticketId?: string;

  /**
   * 作者类型（0=客服，1=用户，2=系统）
   */
  authorType?: number;

  /**
   * 作者用户 ID
   */
  authorId?: string;

  /**
   * 作者姓名
   */
  authorName?: string;

  /**
   * 回复内容
   */
  ticketReplyContent?: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 是否内部备注（仅客服可见）
   */
  isInternal?: number;

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
 * TicketReply 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TicketReplyImport
 * @description 对应后端 TaktTicketReplyImportDto
 */
export interface TicketReplyImport {
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
   * 工单 ID
   */
  ticketId?: string;

  /**
   * 作者类型（0=客服，1=用户，2=系统）
   */
  authorType?: number;

  /**
   * 作者用户 ID
   */
  authorId?: string;

  /**
   * 作者姓名
   */
  authorName?: string;

  /**
   * 回复内容
   */
  ticketReplyContent?: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 是否内部备注（仅客服可见）
   */
  isInternal?: number;

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
 * TicketReply 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TicketReplyExport
 * @description 对应后端 TaktTicketReplyExportDto
 */
export interface TicketReplyExport {
  /**
   * TicketReplyID
   */
  ticketReplyId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工单 ID
   */
  ticketId: string;

  /**
   * 作者类型（0=客服，1=用户，2=系统）
   */
  authorType: number;

  /**
   * 作者用户 ID
   */
  authorId: string;

  /**
   * 作者姓名
   */
  authorName?: string;

  /**
   * 回复内容
   */
  ticketReplyContent: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 是否内部备注（仅客服可见）
   */
  isInternal: number;

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

