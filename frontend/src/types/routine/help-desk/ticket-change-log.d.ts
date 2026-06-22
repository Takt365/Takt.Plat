// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticket-change-log.d.ts
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
 * 工单变更日志实体
 * 对应前端 TaktTicketChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TicketChangeLog
 * @description 对应后端 TaktTicketChangeLogDto
 */
export interface TicketChangeLog extends CompanyDtoBase {
  /**
   * TicketChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ticketChangeLogId: string;

  /**
   * 工单 ID
   */
  ticketId: string;

  /**
   * 工单 名称（填充字段）
   */
  ticketName?: string;

  /**
   * 工单编号（冗余，便于日志列表展示）
   */
  ticketNo?: string;

  /**
   * 变更类型（0=创建，1=更新，2=关闭/删除）
   */
  changeType: number;

  /**
   * 修改工单内容摘要
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组）
   */
  changeFields?: string;

  /**
   * 变更原因或备注
   */
  changeReason?: string;

  /**
   * 工单（主表） （主表：TaktTicket）
   */
  ticket?: Ticket;

}


/**
 * TicketChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TicketChangeLogQuery
 * @description 对应后端 TaktTicketChangeLogQueryDto
 */
export interface TicketChangeLogQuery extends TaktPagedQuery {
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
   * 工单编号（冗余，便于日志列表展示）
   */
  ticketNo?: string;

  /**
   * 变更类型（0=创建，1=更新，2=关闭/删除）
   */
  changeType?: number;

  /**
   * 修改工单内容摘要
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组）
   */
  changeFields?: string;

  /**
   * 变更原因或备注
   */
  changeReason?: string;

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
 * 创建TicketChangeLog DTO
 * 对应前端 TicketChangeLogCreate
 * @description 对应后端 TaktTicketChangeLogCreateDto
 */
export interface TicketChangeLogCreate {
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
   * 工单 ID
   */
  ticketId: string;

  /**
   * 工单编号（冗余，便于日志列表展示）
   */
  ticketNo?: string;

  /**
   * 变更类型（0=创建，1=更新，2=关闭/删除）
   */
  changeType: number;

  /**
   * 修改工单内容摘要
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组）
   */
  changeFields?: string;

  /**
   * 变更原因或备注
   */
  changeReason?: string;

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
 * 更新TicketChangeLog DTO
 * 继承 TaktTicketChangeLogCreateDto，添加 TicketChangeLogId 字段
 * 对应前端 TicketChangeLogUpdate
 * @description 对应后端 TaktTicketChangeLogUpdateDto
 */
export interface TicketChangeLogUpdate extends TicketChangeLogCreate {
  /**
   * TicketChangeLogID（标识要更新的实体）
   */
  ticketChangeLogId: string;

}


/**
 * TicketChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TicketChangeLogExport
 * @description 对应后端 TaktTicketChangeLogExportDto
 */
export interface TicketChangeLogExport {
  /**
   * TicketChangeLogID
   */
  ticketChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工单 ID
   */
  ticketId: string;

  /**
   * 工单编号（冗余，便于日志列表展示）
   */
  ticketNo?: string;

  /**
   * 变更类型（0=创建，1=更新，2=关闭/删除）
   */
  changeType: number;

  /**
   * 修改工单内容摘要
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组）
   */
  changeFields?: string;

  /**
   * 变更原因或备注
   */
  changeReason?: string;

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

