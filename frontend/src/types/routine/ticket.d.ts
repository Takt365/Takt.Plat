// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine
// 文件名称：ticket.d.ts
// 创建时间：2026-06-04
// 创建人：Takt365(Auto Generated)
// 功能描述：routine 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 服务台工单实体
 * 对应前端 TaktTicketDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Ticket
 * @description 对应后端 TaktTicketDto
 */
export interface Ticket extends CompanyDtoBase {
  /**
   * TicketID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ticketId: string;

  /**
   * 工单编号（租户+公司内唯一）
   */
  ticketNo: string;

  /**
   * 工单标题
   */
  title: string;

  /**
   * 工单内容描述
   */
  content?: string;

  /**
   * 附件列表 JSON
   */
  attachmentsJson?: string;

  /**
   * 工单状态
   */
  ticketStatus: number;

  /**
   * 优先级
   */
  priority: number;

  /**
   * 分类编码（如 incident/request）
   */
  categoryCode?: string;

  /**
   * 工单来源
   */
  ticketSource: number;

  /**
   * 提交人 ID
   */
  submitterId: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人 ID
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识 ID
   */
  knowledgeId?: string;

  /**
   * 关联知识 名称（填充字段）
   */
  knowledgeName?: string;

  /**
   * 父工单 ID（为空表示顶级工单）
   */
  parentTicketId?: string;

  /**
   * 父工单 名称（填充字段）
   */
  parentTicketName?: string;

  /**
   * 首次响应时间
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间
   */
  resolvedAt?: string;

  /**
   * 解决期限
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

  /**
   * 流程实例 ID（BusinessType=Ticket、BusinessKey=本表 Id）
   */
  flowInstanceId?: string;

  /**
   * 流程实例 名称（填充字段）
   */
  flowInstanceName?: string;

  /**
   * 申请部门 ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（代理人代提时填被代理人）
   */
  applicantBy: string;

  /**
   * 子工单列表 （子表：TaktTicket）
   */
  childTickets?: Ticket[];

  /**
   * 工单变更日志列表 （子表：TaktTicketChangeLog）
   */
  changeLogs?: TicketChangeLog[];

}


/**
 * Ticket 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TicketQuery
 * @description 对应后端 TaktTicketQueryDto
 */
export interface TicketQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工单编号（租户+公司内唯一）
   */
  ticketNo?: string;

  /**
   * 工单标题
   */
  title?: string;

  /**
   * 工单内容描述
   */
  content?: string;

  /**
   * 附件列表 JSON
   */
  attachmentsJson?: string;

  /**
   * 工单状态
   */
  ticketStatus?: number;

  /**
   * 优先级
   */
  priority?: number;

  /**
   * 分类编码（如 incident/request）
   */
  categoryCode?: string;

  /**
   * 工单来源
   */
  ticketSource?: number;

  /**
   * 提交人 ID
   */
  submitterId?: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人 ID
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识 ID
   */
  knowledgeId?: string;

  /**
   * 父工单 ID（为空表示顶级工单）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（范围查询-开始）
   */
  firstResponseAtStart?: string;

  /**
   * 首次响应时间（范围查询-结束）
   */
  firstResponseAtEnd?: string;

  /**
   * 首次响应期限（范围查询-开始）
   */
  firstResponseDueByStart?: string;

  /**
   * 首次响应期限（范围查询-结束）
   */
  firstResponseDueByEnd?: string;

  /**
   * 解决时间（范围查询-开始）
   */
  resolvedAtStart?: string;

  /**
   * 解决时间（范围查询-结束）
   */
  resolvedAtEnd?: string;

  /**
   * 解决期限（范围查询-开始）
   */
  resolutionDueByStart?: string;

  /**
   * 解决期限（范围查询-结束）
   */
  resolutionDueByEnd?: string;

  /**
   * 关闭时间（范围查询-开始）
   */
  closedAtStart?: string;

  /**
   * 关闭时间（范围查询-结束）
   */
  closedAtEnd?: string;

  /**
   * 流程实例 ID（BusinessType=Ticket、BusinessKey=本表 Id）
   */
  flowInstanceId?: string;

  /**
   * 申请部门 ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（代理人代提时填被代理人）
   */
  applicantBy?: string;

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
 * 创建Ticket DTO
 * 对应前端 TicketCreate
 * @description 对应后端 TaktTicketCreateDto
 */
export interface TicketCreate {
  /**
   * 工单编号（租户+公司内唯一）
   */
  ticketNo: string;

  /**
   * 工单标题
   */
  title: string;

  /**
   * 工单内容描述
   */
  content?: string;

  /**
   * 附件列表 JSON
   */
  attachmentsJson?: string;

  /**
   * 工单状态
   */
  ticketStatus: number;

  /**
   * 优先级
   */
  priority: number;

  /**
   * 分类编码（如 incident/request）
   */
  categoryCode?: string;

  /**
   * 工单来源
   */
  ticketSource: number;

  /**
   * 提交人 ID
   */
  submitterId: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人 ID
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识 ID
   */
  knowledgeId?: string;

  /**
   * 父工单 ID（为空表示顶级工单）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间
   */
  resolvedAt?: string;

  /**
   * 解决期限
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

  /**
   * 流程实例 ID（BusinessType=Ticket、BusinessKey=本表 Id）
   */
  flowInstanceId?: string;

  /**
   * 申请部门 ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（代理人代提时填被代理人）
   */
  applicantBy: string;

  /**
   * 子工单列表（子表，级联保存）
   */
  childTickets?: TicketCreate[];

  /**
   * 工单变更日志列表（子表，级联保存）
   */
  changeLogs?: TicketChangeLogCreate[];

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
 * 更新Ticket DTO
 * 继承 TaktTicketCreateDto，添加 TicketId 字段
 * 对应前端 TicketUpdate
 * @description 对应后端 TaktTicketUpdateDto
 */
export interface TicketUpdate extends TicketCreate {
  /**
   * TicketID（标识要更新的实体）
   */
  ticketId: string;

}


/**
 * Ticket 状态更新 DTO
 * 对应前端 TicketStatus
 * @description 对应后端 TaktTicketStatusDto
 */
export interface TicketStatus {
  /**
   * TicketID
   */
  ticketId: string;

  /**
   * 工单状态
   */
  ticketStatus: number;

}


/**
 * Ticket 导入模板行 DTO
 * 对应前端 TicketTemplate
 * @description 对应后端 TaktTicketTemplateDto
 */
export interface TicketTemplate {
  /**
   * 工单编号（租户+公司内唯一）
   */
  ticketNo?: string;

  /**
   * 工单标题
   */
  title?: string;

  /**
   * 工单内容描述
   */
  content?: string;

  /**
   * 附件列表 JSON
   */
  attachmentsJson?: string;

  /**
   * 工单状态
   */
  ticketStatus?: number;

  /**
   * 优先级
   */
  priority?: number;

  /**
   * 分类编码（如 incident/request）
   */
  categoryCode?: string;

  /**
   * 工单来源
   */
  ticketSource?: number;

  /**
   * 提交人 ID
   */
  submitterId?: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人 ID
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

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
 * Ticket 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TicketImport
 * @description 对应后端 TaktTicketImportDto
 */
export interface TicketImport {
  /**
   * 工单编号（租户+公司内唯一）
   */
  ticketNo?: string;

  /**
   * 工单标题
   */
  title?: string;

  /**
   * 工单内容描述
   */
  content?: string;

  /**
   * 附件列表 JSON
   */
  attachmentsJson?: string;

  /**
   * 工单状态
   */
  ticketStatus?: number;

  /**
   * 优先级
   */
  priority?: number;

  /**
   * 分类编码（如 incident/request）
   */
  categoryCode?: string;

  /**
   * 工单来源
   */
  ticketSource?: number;

  /**
   * 提交人 ID
   */
  submitterId?: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人 ID
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

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
 * Ticket 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TicketExport
 * @description 对应后端 TaktTicketExportDto
 */
export interface TicketExport {
  /**
   * TicketID
   */
  ticketId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工单编号（租户+公司内唯一）
   */
  ticketNo: string;

  /**
   * 工单标题
   */
  title: string;

  /**
   * 工单内容描述
   */
  content?: string;

  /**
   * 附件列表 JSON
   */
  attachmentsJson?: string;

  /**
   * 工单状态
   */
  ticketStatus: number;

  /**
   * 优先级
   */
  priority: number;

  /**
   * 分类编码（如 incident/request）
   */
  categoryCode?: string;

  /**
   * 工单来源
   */
  ticketSource: number;

  /**
   * 提交人 ID
   */
  submitterId: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人 ID
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识 ID
   */
  knowledgeId?: string;

  /**
   * 父工单 ID（为空表示顶级工单）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间
   */
  resolvedAt?: string;

  /**
   * 解决期限
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

  /**
   * 流程实例 ID（BusinessType=Ticket、BusinessKey=本表 Id）
   */
  flowInstanceId?: string;

  /**
   * 申请部门 ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（代理人代提时填被代理人）
   */
  applicantBy: string;

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

