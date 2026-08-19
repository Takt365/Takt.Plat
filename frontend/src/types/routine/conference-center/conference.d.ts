// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/conference-center
// 文件名称：conference.d.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/conference-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理
 * 对应前端 TaktConferenceDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Conference
 * @description 对应后端 TaktConferenceDto
 */
export interface Conference extends ApprovalDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 会议编码（租户+公司内唯一）
   */
  conferenceCode?: string;

  /**
   * 会议标题
   */
  conferenceTitle?: string;

  /**
   * 会议类型
   */
  conferenceType?: number;

  /**
   * 会议状态
   */
  conferenceStatus?: number;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程
   */
  agenda?: string;

  /**
   * 会议内容（会议纪要正文，富文本 HTML）
   */
  conferenceContent?: string;

  /**
   * 会议纪要摘要（用于列表展示）
   */
  conferenceSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  conferenceTags?: string;

  /**
   * 组织人 ID
   */
  organizerId?: string;

  /**
   * 组织人姓名
   */
  organizerName?: string;

  /**
   * 主办部门 ID
   */
  deptId?: string;

  /**
   * 主办部门名称
   */
  deptName?: string;

  /**
   * 最大参会人数（0 表示不限）
   */
  maxParticipants?: number;

  /**
   * 提前提醒分钟数（0 表示不提醒）
   */
  reminderMinutes?: number;

  /**
   * 会议室 ID
   */
  conferenceRoomId?: string;

  /**
   * 会议室名称（冗余快照）
   */
  conferenceRoomName?: string;

  /**
   * 参与人列表（主子表关系）（子表，级联保存）
   */
  participants?: ConferenceParticipantCreate[];

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
 * Conference 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConferenceExport
 * @description 对应后端 TaktConferenceExportDto
 */
export interface ConferenceExport {
  /**
   * ConferenceID
   */
  conferenceId: string;

  /**
   * 会议编码（租户+公司内唯一）
   */
  conferenceCode: string;

  /**
   * 会议标题
   */
  conferenceTitle: string;

  /**
   * 会议类型
   */
  conferenceType: number;

  /**
   * 会议状态
   */
  conferenceStatus: number;

  /**
   * 开始时间
   */
  startTime: string;

  /**
   * 结束时间
   */
  endTime: string;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程
   */
  agenda?: string;

  /**
   * 会议内容（会议纪要正文，富文本 HTML）
   */
  conferenceContent?: string;

  /**
   * 会议纪要摘要（用于列表展示）
   */
  conferenceSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  conferenceTags?: string;

  /**
   * 组织人 ID
   */
  organizerId: string;

  /**
   * 组织人姓名
   */
  organizerName: string;

  /**
   * 主办部门 ID
   */
  deptId?: string;

  /**
   * 主办部门名称
   */
  deptName?: string;

  /**
   * 最大参会人数（0 表示不限）
   */
  maxParticipants: number;

  /**
   * 提前提醒分钟数（0 表示不提醒）
   */
  reminderMinutes: number;

  /**
   * 会议室 ID
   */
  conferenceRoomId?: string;

  /**
   * 会议室名称（冗余快照）
   */
  conferenceRoomName?: string;

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

