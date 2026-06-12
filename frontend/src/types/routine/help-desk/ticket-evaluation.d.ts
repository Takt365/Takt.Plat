// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticket-evaluation.d.ts
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
 * 工单服务评价（一个工单对应一条评价）
 * 对应前端 TaktTicketEvaluationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TicketEvaluation
 * @description 对应后端 TaktTicketEvaluationDto
 */
export interface TicketEvaluation extends CompanyDtoBase {
  /**
   * TicketEvaluationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ticketEvaluationId: string;

  /**
   * 工单 ID
   */
  ticketId: string;

  /**
   * 工单 名称（填充字段）
   */
  ticketName?: string;

  /**
   * 综合评分
   */
  score: number;

  /**
   * 评价内容
   */
  comment?: string;

  /**
   * 评价人 ID
   */
  evaluatorId: string;

  /**
   * 评价人姓名
   */
  evaluatorName?: string;

  /**
   * 评价时间
   */
  evaluatedAt: string;

  /**
   * 工单（主表） （主表：TaktTicket）
   */
  ticket?: Ticket;

}


/**
 * TicketEvaluation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TicketEvaluationQuery
 * @description 对应后端 TaktTicketEvaluationQueryDto
 */
export interface TicketEvaluationQuery extends TaktPagedQuery {
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
   * 综合评分
   */
  score?: number;

  /**
   * 评价内容
   */
  comment?: string;

  /**
   * 评价人 ID
   */
  evaluatorId?: string;

  /**
   * 评价人姓名
   */
  evaluatorName?: string;

  /**
   * 评价时间（范围查询-开始）
   */
  evaluatedAtStart?: string;

  /**
   * 评价时间（范围查询-结束）
   */
  evaluatedAtEnd?: string;

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
 * 创建TicketEvaluation DTO
 * 对应前端 TicketEvaluationCreate
 * @description 对应后端 TaktTicketEvaluationCreateDto
 */
export interface TicketEvaluationCreate {
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
   * 综合评分
   */
  score: number;

  /**
   * 评价内容
   */
  comment?: string;

  /**
   * 评价人 ID
   */
  evaluatorId: string;

  /**
   * 评价人姓名
   */
  evaluatorName?: string;

  /**
   * 评价时间
   */
  evaluatedAt: string;

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
 * 更新TicketEvaluation DTO
 * 继承 TaktTicketEvaluationCreateDto，添加 TicketEvaluationId 字段
 * 对应前端 TicketEvaluationUpdate
 * @description 对应后端 TaktTicketEvaluationUpdateDto
 */
export interface TicketEvaluationUpdate extends TicketEvaluationCreate {
  /**
   * TicketEvaluationID（标识要更新的实体）
   */
  ticketEvaluationId: string;

}


/**
 * TicketEvaluation 导入模板行 DTO
 * 对应前端 TicketEvaluationTemplate
 * @description 对应后端 TaktTicketEvaluationTemplateDto
 */
export interface TicketEvaluationTemplate {
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
   * 综合评分
   */
  score?: number;

  /**
   * 评价内容
   */
  comment?: string;

  /**
   * 评价人 ID
   */
  evaluatorId?: string;

  /**
   * 评价人姓名
   */
  evaluatorName?: string;

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
 * TicketEvaluation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TicketEvaluationImport
 * @description 对应后端 TaktTicketEvaluationImportDto
 */
export interface TicketEvaluationImport {
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
   * 工单 ID
   */
  ticketId?: string;

  /**
   * 综合评分
   */
  score?: number;

  /**
   * 评价内容
   */
  comment?: string;

  /**
   * 评价人 ID
   */
  evaluatorId?: string;

  /**
   * 评价人姓名
   */
  evaluatorName?: string;

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
 * TicketEvaluation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TicketEvaluationExport
 * @description 对应后端 TaktTicketEvaluationExportDto
 */
export interface TicketEvaluationExport {
  /**
   * TicketEvaluationID
   */
  ticketEvaluationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工单 ID
   */
  ticketId: string;

  /**
   * 综合评分
   */
  score: number;

  /**
   * 评价内容
   */
  comment?: string;

  /**
   * 评价人 ID
   */
  evaluatorId: string;

  /**
   * 评价人姓名
   */
  evaluatorName?: string;

  /**
   * 评价时间
   */
  evaluatedAt: string;

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

