// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：news.d.ts
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
 * 新闻中心主实体 支持分类、置顶、推荐、社交统计；需审批通过后发布（草稿→审批→发布）
 * 对应前端 TaktNewsDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 News
 * @description 对应后端 TaktNewsDto
 */
export interface News extends ApprovalDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 新闻编码（租户+公司内唯一）
   */
  newsCode?: string;

  /**
   * 新闻分类
   */
  newsCategory?: number;

  /**
   * 新闻标题
   */
  newsTitle?: string;

  /**
   * 新闻摘要（用于列表展示）
   */
  newsSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  newsTags?: string;

  /**
   * 新闻内容
   */
  newsContent?: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 置顶
   */
  newsIsTop?: number;

  /**
   * 推荐
   */
  newsIsRecommended?: number;

  /**
   * 生效时间
   */
  newsEffectiveTime?: string;

  /**
   * 失效时间
   */
  newsExpireTime?: string;

  /**
   * 阅读次数
   */
  newsReadCount?: number;

  /**
   * 点赞次数
   */
  newsLikeCount?: number;

  /**
   * 评论次数
   */
  newsCommentCount?: number;

  /**
   * 收藏次数
   */
  newsFavoriteCount?: number;

  /**
   * 分享次数
   */
  newsShareCount?: number;

  /**
   * 附件数量
   */
  newsAttachmentCount?: number;

  /**
   * 发布部门 ID
   */
  deptId?: string;

  /**
   * 发布部门名称
   */
  deptName?: string;

  /**
   * 发布人 ID
   */
  publisherId?: string;

  /**
   * 发布人姓名
   */
  publisherName?: string;

  /**
   * 发布时间
   */
  newsPublishTime?: string;

  /**
   * 新闻状态（字典 sys_publish_status；0=草稿，1=已发布，2=已撤回，3=已过期）
   */
  newsStatus?: number;

  /**
   * 新闻附件列表（主子表关系）（子表，级联保存）
   */
  attachments?: NewsAttachmentCreate[];

  /**
   * 新闻评论列表（主子表关系）（子表，级联保存）
   */
  comments?: NewsCommentCreate[];

  /**
   * 新闻点赞记录列表（主子表关系）（子表，级联保存）
   */
  likes?: NewsLikeCreate[];

  /**
   * 新闻阅读记录列表（主子表关系）（子表，级联保存）
   */
  reads?: NewsReadCreate[];

  /**
   * 新闻收藏记录列表（主子表关系）（子表，级联保存）
   */
  favorites?: NewsFavoriteCreate[];

  /**
   * 新闻分享记录列表（主子表关系）（子表，级联保存）
   */
  shares?: NewsShareCreate[];

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
 * News 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsExport
 * @description 对应后端 TaktNewsExportDto
 */
export interface NewsExport {
  /**
   * NewsID
   */
  newsId: string;

  /**
   * 新闻编码（租户+公司内唯一）
   */
  newsCode: string;

  /**
   * 新闻分类
   */
  newsCategory: number;

  /**
   * 新闻标题
   */
  newsTitle: string;

  /**
   * 新闻摘要（用于列表展示）
   */
  newsSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  newsTags?: string;

  /**
   * 新闻内容
   */
  newsContent: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 置顶
   */
  newsIsTop: number;

  /**
   * 推荐
   */
  newsIsRecommended: number;

  /**
   * 生效时间
   */
  newsEffectiveTime?: string;

  /**
   * 失效时间
   */
  newsExpireTime?: string;

  /**
   * 阅读次数
   */
  newsReadCount: number;

  /**
   * 点赞次数
   */
  newsLikeCount: number;

  /**
   * 评论次数
   */
  newsCommentCount: number;

  /**
   * 收藏次数
   */
  newsFavoriteCount: number;

  /**
   * 分享次数
   */
  newsShareCount: number;

  /**
   * 附件数量
   */
  newsAttachmentCount: number;

  /**
   * 发布部门 ID
   */
  deptId?: string;

  /**
   * 发布部门名称
   */
  deptName?: string;

  /**
   * 发布人 ID
   */
  publisherId: string;

  /**
   * 发布人姓名
   */
  publisherName: string;

  /**
   * 发布时间
   */
  newsPublishTime?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 新闻状态（字典 sys_publish_status；0=草稿，1=已发布，2=已撤回，3=已过期）
   */
  newsStatus: number;

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

