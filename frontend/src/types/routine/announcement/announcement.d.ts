// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/announcement
// 文件名称：announcement.d.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/announcement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 公告通知实体 用于发布系统公告、通知等信息 支持富文本内容、附件、置顶、定时发布等功能 需要审批流程：草稿→审批→发布
 * 对应前端 TaktAnnouncementDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Announcement
 * @description 对应后端 TaktAnnouncementDto
 */
export interface Announcement extends ApprovalDtoBase {
  /**
   * AnnouncementID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  announcementId: string;

  /**
   * 公告编码（租户+公司内唯一）
   */
  announcementCode: string;

  /**
   * 公告标题
   */
  announcementTitle: string;

  /**
   * 公告类型（字典 sys_announcement_category）
   */
  announcementType: number;

  /**
   * 公告内容（富文本 HTML）
   */
  content: string;

  /**
   * 公告摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachments?: string;

  /**
   * 发布时间（定时发布时使用）
   */
  publishTime?: string;

  /**
   * 定时发布（1=是，0=否）
   */
  isScheduled: number;

  /**
   * 置顶（1=是，0=否）
   */
  isTop: number;

  /**
   * 置顶优先级（数字越大越靠前）
   */
  topPriority: number;

  /**
   * 过期时间（过期后自动隐藏）
   */
  expireTime?: string;

  /**
   * 查看次数
   */
  viewCount: number;

  /**
   * 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
   */
  targetScope: string;

  /**
   * 目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）
   */
  targetUsers?: string;

  /**
   * 状态（字典 sys_publish_status；0=草稿，1=已发布，2=已撤回，3=已过期）
   */
  announcementStatus: number;

}


/**
 * Announcement 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AnnouncementQuery
 * @description 对应后端 TaktAnnouncementQueryDto
 */
export interface AnnouncementQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 公告编码（租户+公司内唯一）
   */
  announcementCode?: string;

  /**
   * 公告标题
   */
  announcementTitle?: string;

  /**
   * 公告类型（字典 sys_announcement_category）
   */
  announcementType?: number;

  /**
   * 公告内容（富文本 HTML）
   */
  content?: string;

  /**
   * 公告摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachments?: string;

  /**
   * 发布时间（定时发布时使用）（范围查询-开始）
   */
  publishTimeStart?: string;

  /**
   * 发布时间（定时发布时使用）（范围查询-结束）
   */
  publishTimeEnd?: string;

  /**
   * 定时发布（1=是，0=否）
   */
  isScheduled?: number;

  /**
   * 置顶（1=是，0=否）
   */
  isTop?: number;

  /**
   * 置顶优先级（数字越大越靠前）
   */
  topPriority?: number;

  /**
   * 过期时间（过期后自动隐藏）（范围查询-开始）
   */
  expireTimeStart?: string;

  /**
   * 过期时间（过期后自动隐藏）（范围查询-结束）
   */
  expireTimeEnd?: string;

  /**
   * 查看次数
   */
  viewCount?: number;

  /**
   * 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
   */
  targetScope?: string;

  /**
   * 目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）
   */
  targetUsers?: string;

  /**
   * 状态（字典 sys_publish_status；0=草稿，1=已发布，2=已撤回，3=已过期）
   */
  announcementStatus?: number;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

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
 * 创建Announcement DTO
 * 对应前端 AnnouncementCreate
 * @description 对应后端 TaktAnnouncementCreateDto
 */
export interface AnnouncementCreate {
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
   * 公告编码（租户+公司内唯一）；留空时须指定 numberingRuleCode 自动取号
   */
  announcementCode?: string;

  /**
   * 编码规则编码（创建自动取号用，对应 TaktNumbering.RuleCode；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 公告标题
   */
  announcementTitle: string;

  /**
   * 公告类型（字典 sys_announcement_category）
   */
  announcementType: number;

  /**
   * 公告内容（富文本 HTML）
   */
  content: string;

  /**
   * 公告摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachments?: string;

  /**
   * 发布时间（定时发布时使用）
   */
  publishTime?: string;

  /**
   * 定时发布（1=是，0=否）
   */
  isScheduled: number;

  /**
   * 置顶（1=是，0=否）
   */
  isTop: number;

  /**
   * 置顶优先级（数字越大越靠前）
   */
  topPriority: number;

  /**
   * 过期时间（过期后自动隐藏）
   */
  expireTime?: string;

  /**
   * 查看次数
   */
  viewCount: number;

  /**
   * 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
   */
  targetScope: string;

  /**
   * 目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）
   */
  targetUsers?: string;

  /**
   * 状态（字典 sys_publish_status；0=草稿，1=已发布，2=已撤回，3=已过期）
   */
  announcementStatus: number;

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
 * 更新Announcement DTO
 * 继承 TaktAnnouncementCreateDto，添加 AnnouncementId 字段
 * 对应前端 AnnouncementUpdate
 * @description 对应后端 TaktAnnouncementUpdateDto
 */
export interface AnnouncementUpdate extends AnnouncementCreate {
  /**
   * AnnouncementID（标识要更新的实体）
   */
  announcementId: string;

}


/**
 * Announcement 状态更新 DTO
 * 对应前端 AnnouncementStatus
 * @description 对应后端 TaktAnnouncementStatusDto
 */
export interface AnnouncementStatus {
  /**
   * AnnouncementID
   */
  announcementId: string;

  /**
   * 状态（字典 sys_publish_status；0=草稿，1=已发布，2=已撤回，3=已过期）
   */
  announcementStatus: number;

}


/**
 * Announcement 导入模板行 DTO
 * 对应前端 AnnouncementTemplate
 * @description 对应后端 TaktAnnouncementTemplateDto
 */
export interface AnnouncementTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 公告编码（租户+公司内唯一）
   */
  announcementCode?: string;

  /**
   * 公告标题
   */
  announcementTitle?: string;

  /**
   * 公告类型（字典 sys_announcement_category）
   */
  announcementType?: number;

  /**
   * 公告内容（富文本 HTML）
   */
  content?: string;

  /**
   * 公告摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachments?: string;

  /**
   * 发布时间（定时发布时使用）
   */
  publishTime?: string;

  /**
   * 定时发布（1=是，0=否）
   */
  isScheduled?: number;

  /**
   * 置顶（1=是，0=否）
   */
  isTop?: number;

  /**
   * 置顶优先级（数字越大越靠前）
   */
  topPriority?: number;

  /**
   * 过期时间（过期后自动隐藏）
   */
  expireTime?: string;

  /**
   * 查看次数
   */
  viewCount?: number;

  /**
   * 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
   */
  targetScope?: string;

  /**
   * 目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）
   */
  targetUsers?: string;

  /**
   * 状态（字典 sys_publish_status；0=草稿，1=已发布，2=已撤回，3=已过期）
   */
  announcementStatus?: number;

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
 * Announcement 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AnnouncementImport
 * @description 对应后端 TaktAnnouncementImportDto
 */
export interface AnnouncementImport {
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
   * 公告编码（租户+公司内唯一）；留空时须指定 numberingRuleCode
   */
  announcementCode?: string;

  /**
   * 编码规则编码（导入自动取号用，对应 TaktNumbering.RuleCode；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 公告标题
   */
  announcementTitle?: string;

  /**
   * 公告类型（字典 sys_announcement_category）
   */
  announcementType?: number;

  /**
   * 公告内容（富文本 HTML）
   */
  content?: string;

  /**
   * 公告摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachments?: string;

  /**
   * 发布时间（定时发布时使用）
   */
  publishTime?: string;

  /**
   * 定时发布（1=是，0=否）
   */
  isScheduled?: number;

  /**
   * 置顶（1=是，0=否）
   */
  isTop?: number;

  /**
   * 置顶优先级（数字越大越靠前）
   */
  topPriority?: number;

  /**
   * 过期时间（过期后自动隐藏）
   */
  expireTime?: string;

  /**
   * 查看次数
   */
  viewCount?: number;

  /**
   * 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
   */
  targetScope?: string;

  /**
   * 目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）
   */
  targetUsers?: string;

  /**
   * 状态（字典 sys_publish_status；0=草稿，1=已发布，2=已撤回，3=已过期）
   */
  announcementStatus?: number;

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
 * Announcement 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AnnouncementExport
 * @description 对应后端 TaktAnnouncementExportDto
 */
export interface AnnouncementExport {
  /**
   * AnnouncementID
   */
  announcementId: string;

  /**
   * 公告编码（租户+公司内唯一）
   */
  announcementCode: string;

  /**
   * 公告标题
   */
  announcementTitle: string;

  /**
   * 公告类型（字典 sys_announcement_category）
   */
  announcementType: number;

  /**
   * 公告内容（富文本 HTML）
   */
  content: string;

  /**
   * 公告摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachments?: string;

  /**
   * 发布时间（定时发布时使用）
   */
  publishTime?: string;

  /**
   * 定时发布（1=是，0=否）
   */
  isScheduled: number;

  /**
   * 置顶（1=是，0=否）
   */
  isTop: number;

  /**
   * 置顶优先级（数字越大越靠前）
   */
  topPriority: number;

  /**
   * 过期时间（过期后自动隐藏）
   */
  expireTime?: string;

  /**
   * 查看次数
   */
  viewCount: number;

  /**
   * 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
   */
  targetScope: string;

  /**
   * 目标部门编码（多个用逗号分隔，当 target_scope=department 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户 ID（多个用逗号分隔，当 target_scope=custom 时使用）
   */
  targetUsers?: string;

  /**
   * 状态（字典 sys_publish_status；0=草稿，1=已发布，2=已撤回，3=已过期）
   */
  announcementStatus: number;

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

