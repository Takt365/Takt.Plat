// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：news-comment-like.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/news-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 新闻中心评论点赞记录实体
 * 对应前端 TaktNewsCommentLikeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 NewsCommentLike
 * @description 对应后端 TaktNewsCommentLikeDto
 */
export interface NewsCommentLike extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 评论 ID
   */
  commentId?: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 用户姓名
   */
  userName?: string;

  /**
   * 点赞时间
   */
  likeTime?: string;

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
 * NewsCommentLike 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsCommentLikeExport
 * @description 对应后端 TaktNewsCommentLikeExportDto
 */
export interface NewsCommentLikeExport {
  /**
   * NewsCommentLikeID
   */
  newsCommentLikeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 评论 ID
   */
  commentId: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 用户姓名
   */
  userName: string;

  /**
   * 点赞时间
   */
  likeTime: string;

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

