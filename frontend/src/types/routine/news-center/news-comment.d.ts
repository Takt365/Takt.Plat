// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：news-comment.d.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/news-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 新闻中心评论实体 支持多级回复；需审批通过后展示
 * 对应前端 TaktNewsCommentDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 NewsComment
 * @description 对应后端 TaktNewsCommentDto
 */
export interface NewsComment extends ApprovalDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 新闻 ID
   */
  newsId?: string;

  /**
   * 父评论 ID（0 表示顶级评论）
   */
  parentId?: string;

  /**
   * 评论人 ID
   */
  userId?: string;

  /**
   * 评论人姓名
   */
  userName?: string;

  /**
   * 评论人头像 URL
   */
  userAvatar?: string;

  /**
   * 被回复人 ID
   */
  replyToUserId?: string;

  /**
   * 被回复人姓名
   */
  replyToUserName?: string;

  /**
   * 评论内容
   */
  commentContent?: string;

  /**
   * 评论时间
   */
  commentTime?: string;

  /**
   * 点赞次数
   */
  newsCommentLikeCount?: number;

  /**
   * 回复次数（子评论数量）
   */
  replyCount?: number;

  /**
   * 评论层级（0=顶级，最多 3 级）
   */
  commentLevel?: number;

  /**
   * 评论状态
   */
  commentStatus?: number;

  /**
   * 评论点赞记录列表（主子表关系）（子表，级联保存）
   */
  likes?: NewsCommentLikeCreate[];

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
 * NewsComment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsCommentExport
 * @description 对应后端 TaktNewsCommentExportDto
 */
export interface NewsCommentExport {
  /**
   * NewsCommentID
   */
  newsCommentId: string;

  /**
   * 新闻 ID
   */
  newsId: string;

  /**
   * 父评论 ID（0 表示顶级评论）
   */
  parentId: string;

  /**
   * 评论人 ID
   */
  userId: string;

  /**
   * 评论人姓名
   */
  userName: string;

  /**
   * 评论人头像 URL
   */
  userAvatar?: string;

  /**
   * 被回复人 ID
   */
  replyToUserId?: string;

  /**
   * 被回复人姓名
   */
  replyToUserName?: string;

  /**
   * 评论内容
   */
  commentContent: string;

  /**
   * 评论时间
   */
  commentTime: string;

  /**
   * 点赞次数
   */
  newsCommentLikeCount: number;

  /**
   * 回复次数（子评论数量）
   */
  replyCount: number;

  /**
   * 评论层级（0=顶级，最多 3 级）
   */
  commentLevel: number;

  /**
   * 评论状态
   */
  commentStatus: number;

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

