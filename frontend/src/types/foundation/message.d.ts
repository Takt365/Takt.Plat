// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：message.d.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成 CRUD + 批量发送类型；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 在线消息实体 公司级实体：消息按租户+公司双重隔离
 * 对应前端 TaktMessageDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Message
 * @description 对应后端 TaktMessageDto
 */
export interface Message extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 发送者用户名
   */
  fromUserName?: string;

  /**
   * 发送者用户 ID
   */
  fromUserId?: string;

  /**
   * 接收者用户名
   */
  toUserName?: string;

  /**
   * 接收者用户 ID
   */
  toUserId?: string;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent?: string;

  /**
   * 消息类型（1=系统消息 2=用户消息 3=通知消息）
   */
  messageType?: string;

  /**
   * 消息分组（查询条件，字典 DictValue）
   */
  messageGroup?: string;

  /**
   * 读取状态（0=未读 1=已读）
   */
  readStatus?: number;

  /**
   * 读取时间
   */
  readTime?: string;

  /**
   * 发送时间
   */
  sendTime?: string;

  /**
   * 抄送（0=否，1=是）
   */
  isCc?: number;

  /**
   * 附件路径（JSON 或逗号分隔）
   */
  messageAttachments?: string;

  /**
   * 消息扩展数据（JSON）
   */
  messageExtData?: string;

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
 * Message 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MessageExport
 * @description 对应后端 TaktMessageExportDto
 */
export interface MessageExport {
  /**
   * MessageID
   */
  messageId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 发送者用户名
   */
  fromUserName: string;

  /**
   * 发送者用户 ID
   */
  fromUserId?: string;

  /**
   * 接收者用户名
   */
  toUserName: string;

  /**
   * 接收者用户 ID
   */
  toUserId?: string;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent: string;

  /**
   * 消息类型（字典 sys_message_type DictValue：text、system、multimedia）
   */
  messageType: string;

  /**
   * 消息分组（字典 sys_message_group_category DictValue）
   */
  messageGroup: string;

  /**
   * 读取状态（0=未读 1=已读）
   */
  readStatus: number;

  /**
   * 读取时间
   */
  readTime?: string;

  /**
   * 发送时间
   */
  sendTime: string;

  /**
   * 抄送（0=否，1=是）
   */
  isCc: number;

  /**
   * 附件路径（JSON 或逗号分隔）
   */
  messageAttachments?: string;

  /**
   * 消息扩展数据（JSON）
   */
  messageExtData?: string;

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

/**
 * 消息附件 JSON 项（写入 MessageAttachments 字段）
 */
export interface MessageAttachmentItem {
  /** 文件 ID */
  fileId: string;
  /** 存储文件名 */
  fileName: string;
  /** 原始文件名 */
  fileOriginalName?: string;
  /** 访问 URL */
  accessUrl: string;
  /** 文件大小 */
  fileSize?: string;
  /** 文件类型 */
  fileType?: string;
  /** 扩展名 */
  fileExtension?: string;
  /** 排序号 */
  sortOrder?: number;
}

/**
 * 批量创建并发送消息 DTO
 * @description 对应后端 TaktMessageBatchCreateDto / POST TaktMessages/batch-send
 */
export interface MessageBatchCreate {
  /** 发送者用户名 */
  fromUserName?: string;
  /** 发送者用户 ID */
  fromUserId?: string;
  /** 消息标题 */
  messageTitle?: string;
  /** 消息内容 */
  messageContent: string;
  /** 附件 JSON 字符串 */
  messageAttachments?: string;
  /** 消息类型（字典 sys_message_type DictValue） */
  messageType: string;
  /** 消息分组（字典 sys_message_group_category DictValue） */
  messageGroup: string;
  /** 抄送（0=否，1=是） */
  isCc: number;
  /** 是否发送给当前公司全部可访问用户 */
  sendToAll: boolean;
  /** 指定接收者用户 ID 列表 */
  toUserIds?: string[];
  /** 发送时间 */
  sendTime?: string;
}

/**
 * 当前用户在线消息统计
 * @description 对应后端 TaktMessageStatisticsDto / GET TaktMessages/statistics
 */
export interface MessageStatistics {
  /** 用户名（接收者） */
  userName: string;
  /** 用户 ID */
  userId?: string;
  /** 收件箱消息总数 */
  totalCount: number;
  /** 已读数量 */
  readCount: number;
  /** 未读数量 */
  unreadCount: number;
}

