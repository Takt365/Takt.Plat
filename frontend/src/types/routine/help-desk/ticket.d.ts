// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticket.d.ts
// 创建时间：2026-07-09
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
 * Takt工单实体
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
   * 工单编号（唯一）
   */
  ticketNo: string;

  /**
   * 工单标题
   */
  ticketTitle: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）。格式：[{ "FileId": 0, "FileName": "", "FilePath": "", "FileSize": 0, "FileType": "", "FileExtension": "", "SortOrder": 0 }]
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority: number;

  /**
   * 紧急度（字典 sys_urgency_level_category）
   */
  urgency: number;

  /**
   * 影响范围（字典 sys_impact_level_category）
   */
  impact: number;

  /**
   * 分类编码（如 incident/request 等）
   */
  categoryCode?: string;

  /**
   * 工单来源（0=门户网站，1=邮件，2=电话，3=API接入）
   */
  ticketSource: number;

  /**
   * 提交人ID（序列化为string以避免Javascript精度问题）
   */
  submitterId: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人ID（序列化为string以避免Javascript精度问题）
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识ID（可选，序列化为string以避免Javascript精度问题）
   */
  knowledgeId?: string;

  /**
   * 关联知识名称（填充字段）
   */
  knowledgeName?: string;

  /**
   * 父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）
   */
  parentTicketId?: string;

  /**
   * 父工单名称（填充字段）
   */
  parentTicketName?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * 关联 IT 设备保修扩展 ID
   */
  itAssetId?: string;

  /**
   * 关联 IT 设备保修扩展 名称（填充字段）
   */
  itAssetName?: string;

  /**
   * 资产号码（冗余；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 资产名称（填充字段，来自 TaktAsset）
   */
  assetName?: string;

  /**
   * 工单回复列表（详情填充）
   */
  replies?: TicketReply[];

  /**
   * 申请部门ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（实际申请人；代理人代提时填被代理人）
   */
  applicantBy: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开）
   */
  ticketStatus: number;

  /**
   * 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId） （子表：TaktTicket）
   */
  childTickets?: Ticket[];

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
   * 工单编号（唯一）
   */
  ticketNo?: string;

  /**
   * 工单标题
   */
  ticketTitle?: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）。格式：[{ "FileId": 0, "FileName": "", "FilePath": "", "FileSize": 0, "FileType": "", "FileExtension": "", "SortOrder": 0 }]
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority?: number;

  /**
   * 紧急度（字典 sys_urgency_level_category）
   */
  urgency?: number;

  /**
   * 影响范围（字典 sys_impact_level_category）
   */
  impact?: number;

  /**
   * 分类编码（如 incident/request 等）
   */
  categoryCode?: string;

  /**
   * 工单来源（0=门户网站，1=邮件，2=电话，3=API接入）
   */
  ticketSource?: number;

  /**
   * 提交人ID（序列化为string以避免Javascript精度问题）
   */
  submitterId?: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人ID（序列化为string以避免Javascript精度问题）
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识ID（可选，序列化为string以避免Javascript精度问题）
   */
  knowledgeId?: string;

  /**
   * 父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）（范围查询-开始）
   */
  firstResponseAtStart?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）（范围查询-结束）
   */
  firstResponseAtEnd?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）（范围查询-开始）
   */
  firstResponseDueByStart?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）（范围查询-结束）
   */
  firstResponseDueByEnd?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）（范围查询-开始）
   */
  resolvedAtStart?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）（范围查询-结束）
   */
  resolvedAtEnd?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）（范围查询-开始）
   */
  resolutionDueByStart?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）（范围查询-结束）
   */
  resolutionDueByEnd?: string;

  /**
   * 关闭时间（工单最终关闭的时间）（范围查询-开始）
   */
  closedAtStart?: string;

  /**
   * 关闭时间（工单最终关闭的时间）（范围查询-结束）
   */
  closedAtEnd?: string;

  /**
   * 关联 IT 设备保修扩展 ID
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（实际申请人；代理人代提时填被代理人）
   */
  applicantBy?: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开）
   */
  ticketStatus?: number;

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
 * 创建Ticket DTO
 * 对应前端 TicketCreate
 * @description 对应后端 TaktTicketCreateDto
 */
export interface TicketCreate {
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
   * 工单编号（唯一）
   */
  ticketNo: string;

  /**
   * 工单标题
   */
  ticketTitle: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）。格式：[{ "FileId": 0, "FileName": "", "FilePath": "", "FileSize": 0, "FileType": "", "FileExtension": "", "SortOrder": 0 }]
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority: number;

  /**
   * 紧急度（字典 sys_urgency_level_category）
   */
  urgency: number;

  /**
   * 影响范围（字典 sys_impact_level_category）
   */
  impact: number;

  /**
   * 分类编码（如 incident/request 等）
   */
  categoryCode?: string;

  /**
   * 工单来源（0=门户网站，1=邮件，2=电话，3=API接入）
   */
  ticketSource: number;

  /**
   * 提交人ID（序列化为string以避免Javascript精度问题）
   */
  submitterId: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人ID（序列化为string以避免Javascript精度问题）
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识ID（可选，序列化为string以避免Javascript精度问题）
   */
  knowledgeId?: string;

  /**
   * 父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * 关联 IT 设备保修扩展 ID
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（实际申请人；代理人代提时填被代理人）
   */
  applicantBy: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开）
   */
  ticketStatus: number;

  /**
   * 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
   */
  childTickets?: TicketCreate[];

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
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开）
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
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工单编号（唯一）
   */
  ticketNo?: string;

  /**
   * 工单标题
   */
  ticketTitle?: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）。格式：[{ "FileId": 0, "FileName": "", "FilePath": "", "FileSize": 0, "FileType": "", "FileExtension": "", "SortOrder": 0 }]
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority?: number;

  /**
   * 紧急度（字典 sys_urgency_level_category）
   */
  urgency?: number;

  /**
   * 影响范围（字典 sys_impact_level_category）
   */
  impact?: number;

  /**
   * 分类编码（如 incident/request 等）
   */
  categoryCode?: string;

  /**
   * 工单来源（0=门户网站，1=邮件，2=电话，3=API接入）
   */
  ticketSource?: number;

  /**
   * 提交人ID（序列化为string以避免Javascript精度问题）
   */
  submitterId?: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人ID（序列化为string以避免Javascript精度问题）
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识ID（可选，序列化为string以避免Javascript精度问题）
   */
  knowledgeId?: string;

  /**
   * 父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * 关联 IT 设备保修扩展 ID
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（实际申请人；代理人代提时填被代理人）
   */
  applicantBy?: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开）
   */
  ticketStatus?: number;

  /**
   * 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
   */
  childTickets?: TicketCreate[];

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
 * Ticket 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TicketImport
 * @description 对应后端 TaktTicketImportDto
 */
export interface TicketImport {
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
   * 工单编号（唯一）
   */
  ticketNo?: string;

  /**
   * 工单标题
   */
  ticketTitle?: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）。格式：[{ "FileId": 0, "FileName": "", "FilePath": "", "FileSize": 0, "FileType": "", "FileExtension": "", "SortOrder": 0 }]
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority?: number;

  /**
   * 紧急度（字典 sys_urgency_level_category）
   */
  urgency?: number;

  /**
   * 影响范围（字典 sys_impact_level_category）
   */
  impact?: number;

  /**
   * 分类编码（如 incident/request 等）
   */
  categoryCode?: string;

  /**
   * 工单来源（0=门户网站，1=邮件，2=电话，3=API接入）
   */
  ticketSource?: number;

  /**
   * 提交人ID（序列化为string以避免Javascript精度问题）
   */
  submitterId?: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人ID（序列化为string以避免Javascript精度问题）
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识ID（可选，序列化为string以避免Javascript精度问题）
   */
  knowledgeId?: string;

  /**
   * 父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * 关联 IT 设备保修扩展 ID
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（实际申请人；代理人代提时填被代理人）
   */
  applicantBy?: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开）
   */
  ticketStatus?: number;

  /**
   * 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
   */
  childTickets?: TicketCreate[];

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
   * 工单编号（唯一）
   */
  ticketNo: string;

  /**
   * 工单标题
   */
  ticketTitle: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）。格式：[{ "FileId": 0, "FileName": "", "FilePath": "", "FileSize": 0, "FileType": "", "FileExtension": "", "SortOrder": 0 }]
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level_category）
   */
  priority: number;

  /**
   * 紧急度（字典 sys_urgency_level_category）
   */
  urgency: number;

  /**
   * 影响范围（字典 sys_impact_level_category）
   */
  impact: number;

  /**
   * 分类编码（如 incident/request 等）
   */
  categoryCode?: string;

  /**
   * 工单来源（0=门户网站，1=邮件，2=电话，3=API接入）
   */
  ticketSource: number;

  /**
   * 提交人ID（序列化为string以避免Javascript精度问题）
   */
  submitterId: string;

  /**
   * 提交人姓名
   */
  submitterName?: string;

  /**
   * 处理人ID（序列化为string以避免Javascript精度问题）
   */
  assigneeId?: string;

  /**
   * 处理人姓名
   */
  assigneeName?: string;

  /**
   * 关联知识ID（可选，序列化为string以避免Javascript精度问题）
   */
  knowledgeId?: string;

  /**
   * 父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * 关联 IT 设备保修扩展 ID
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门ID
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称
   */
  applicantDeptName?: string;

  /**
   * 申请人（实际申请人；代理人代提时填被代理人）
   */
  applicantBy: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消，7=重新打开）
   */
  ticketStatus: number;

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

