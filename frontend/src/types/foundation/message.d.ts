// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：message.d.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktMessageDto → Message）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery } from '@/types/common';

/**
 * 消息附件项（Attachments JSON 数组元素）
 */
export interface MessageAttachmentItem {
  /** 文件 ID */
  fileId: string;
  /** 存储文件名 */
  fileName: string;
  /** 原始文件名 */
  fileOriginalName?: string;
  /** 访问地址 */
  accessUrl: string;
  /** 文件大小（字节） */
  fileSize?: string;
  /** MIME 类型 */
  fileType?: string;
  /** 扩展名 */
  fileExtension?: string;
  /** 排序 */
  sortOrder?: number;
}

/**
 * 在线消息实体 公司级实体：消息按租户+公司双重隔离
 * 对应前端 Message
 * 继承 CompanyDtoBase
 * @description 对应后端 TaktMessageDto
 */
export interface Message extends CompanyDtoBase {
  /**
   * MessageID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  messageId: string;

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
   * 是否抄送发送者本人（自审计，TaktYesNo：0=否，1=是，默认 1）
   */
  isCc?: number;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 消息类型
   */
  messageType: number;

  /**
   * 消息分组
   */
  messageGroup?: number;

  /**
   * 发送时间
   */
  sendTime: string;

  /**
   * 读取时间
   */
  readTime?: string;

  /**
   * 读取状态（0=未读，1=已读）
   */
  readStatus: number;

}


/**
 * Message 分页查询 DTO
 * 继承 TaktPagedQuery
 * @description 对应后端 TaktMessageQueryDto
 */
export interface MessageQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 发送者用户名
   */
  fromUserName?: string;

  /**
   * 发送者用户 ID
   */
  fromUserId?: string;

  /**
   * 接收者用户名（模糊）
   */
  toUserName?: string;

  /**
   * 接收者用户 ID
   */
  toUserId?: string;

  /**
   * 是否抄送发送者本人（TaktYesNo：0=否，1=是）
   */
  isCc?: number;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent?: string;

  /**
   * 附件（模糊）
   */
  attachments?: string;

  /**
   * 消息类型
   */
  messageType?: string;

  /**
   * 消息分组
   */
  messageGroup?: number;

  /**
   * 发送时间（范围查询-开始）
   */
  sendTimeStart?: string;

  /**
   * 发送时间（范围查询-结束）
   */
  sendTimeEnd?: string;

  /**
   * 读取时间（范围查询-开始）
   */
  readTimeStart?: string;

  /**
   * 读取时间（范围查询-结束）
   */
  readTimeEnd?: string;

  /**
   * 读取状态（0=未读，1=已读）
   */
  readStatus?: number;

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
 * 当前用户收件箱消息分页查询 DTO（已读/未读列表共用）
 * @description 对应后端 TaktMessageInboxListQueryDto；接收者与 ReadStatus 由服务端按路由固定
 */
export interface MessageInboxListQuery extends TaktPagedQuery {
  /**
   * 发送者用户名
   */
  fromUserName?: string;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent?: string;

  /**
   * 消息类型
   */
  messageType?: string;

  /**
   * 消息分组
   */
  messageGroup?: number;

  /**
   * 发送时间（范围查询-开始）
   */
  sendTimeStart?: string;

  /**
   * 发送时间（范围查询-结束）
   */
  sendTimeEnd?: string;

  /**
   * 读取时间（范围查询-开始，已读列表可用）
   */
  readTimeStart?: string;

  /**
   * 读取时间（范围查询-结束，已读列表可用）
   */
  readTimeEnd?: string;
}

/** 已读列表查询（同 MessageInboxListQuery） */
export type MessageReadListQuery = MessageInboxListQuery;

/** 未读列表查询（同 MessageInboxListQuery） */
export type MessageUnreadListQuery = MessageInboxListQuery;

/**
 * 创建Message DTO
 * @description 对应后端 TaktMessageCreateDto
 */
export interface MessageCreate {
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
   * 是否抄送发送者本人（自审计，TaktYesNo：0=否，1=是，默认 1）
   */
  isCc?: number;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 消息类型
   */
  messageType: number;

  /**
   * 消息分组
   */
  messageGroup?: number;

  /**
   * 发送时间
   */
  sendTime: string;

  /**
   * 读取时间
   */
  readTime?: string;

  /**
   * 读取状态（0=未读，1=已读）
   */
  readStatus: number;

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
 * 批量创建并推送 Message DTO
 * @description 对应后端 TaktMessageBatchCreateDto
 */
export interface MessageBatchCreate {
  /** 发送者用户名 */
  fromUserName: string;
  /** 发送者用户 ID */
  fromUserId?: string;
  /** 是否向租户全部用户落库并推送 */
  sendToAll: boolean;
  /** 指定接收者用户 ID 列表 */
  toUserIds?: string[];
  /** 消息标题 */
  messageTitle?: string;
  /** 消息内容 */
  messageContent: string;
  /** 附件列表 JSON */
  attachments?: string;
  /** 消息类型 */
  messageType: number;
  /** 消息分组 */
  messageGroup?: number;
  /** 读取状态（0=未读，1=已读） */
  readStatus?: number;
  /** 是否抄送发送者本人（TaktYesNo，默认 1=是） */
  isCc?: number;
}


/**
 * 标记在线消息已读 DTO
 * @description 对应后端 TaktMessageReadDto
 */
export interface MessageRead {
  /**
   * MessageID
   */
  messageId: string;

  /**
   * 读取状态（0=未读，1=已读）
   */
  readStatus: number;

  /**
   * 读取时间（可选，为空时服务端写入当前时间）
   */
  readTime?: string;
}

/**
 * 标记在线消息未读 DTO
 * @description 对应后端 TaktMessageUnreadDto
 */
export interface MessageUnread {
  /**
   * MessageID
   */
  messageId: string;

  /**
   * 读取状态（0=未读，1=已读）
   */
  readStatus: number;

  /**
   * 读取时间（标记未读时须为 null）
   */
  readTime?: string | null;
}

/**
 * Message 导出 DTO（独立实现，不继承响应 Dto）
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
   * 是否抄送发送者本人（自审计，TaktYesNo：0=否，1=是，默认 1）
   */
  isCc?: number;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 消息类型
   */
  messageType: number;

  /**
   * 消息分组
   */
  messageGroup?: number;

  /**
   * 发送时间
   */
  sendTime: string;

  /**
   * 读取时间
   */
  readTime?: string;

  /**
   * 读取状态（0=未读，1=已读）
   */
  readStatus: number;

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


/**
 * 当前登录用户在线消息统计 DTO
 * @description 对应后端 TaktMessageStatisticsDto
 */
export interface MessageStatistics {
  /**
   * 用户名（接收者，即当前登录用户）
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 接收消息总数
   */
  totalCount: number;

  /**
   * 已读消息数
   */
  readCount: number;

  /**
   * 未读消息数
   */
  unreadCount: number;
}

