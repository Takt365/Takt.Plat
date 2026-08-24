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
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

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
   * 文件名称（原始文件名）
   */
  fileName?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl?: string;

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
   * 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
   */
  targetScope?: number;

  /**
   * 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
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
   * 文件名称（原始文件名）
   */
  fileName?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl?: string;

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
   * 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
   */
  targetScope: number;

  /**
   * 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
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

