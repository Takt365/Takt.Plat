// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：news.d.ts
// 创建时间：2026-06-06
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
   * NewsID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
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
  tags?: string;

  /**
   * 新闻内容
   */
  newsContent: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 是否置顶
   */
  isTop: number;

  /**
   * 是否推荐
   */
  isRecommended: number;

  /**
   * 生效时间
   */
  effectiveTime?: string;

  /**
   * 失效时间
   */
  expireTime?: string;

  /**
   * 阅读次数
   */
  readCount: number;

  /**
   * 点赞次数
   */
  likeCount: number;

  /**
   * 评论次数
   */
  commentCount: number;

  /**
   * 收藏次数
   */
  favoriteCount: number;

  /**
   * 分享次数
   */
  shareCount: number;

  /**
   * 附件数量
   */
  attachmentCount: number;

  /**
   * 流程实例 ID（关联工作流，如发布审批流程；流程侧 BusinessType=News、BusinessKey=本表 Id）
   */
  flowInstanceId?: string;

  /**
   * 流程实例 名称（填充字段）
   */
  flowInstanceName?: string;

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
  publishTime?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 新闻状态
   */
  newsStatus: number;

  /**
   * 新闻附件列表（主子表关系） （子表：TaktNewsAttachment）
   */
  attachments?: NewsAttachment[];

  /**
   * 新闻评论列表（主子表关系） （子表：TaktNewsComment）
   */
  comments?: NewsComment[];

  /**
   * 新闻点赞记录列表（主子表关系） （子表：TaktNewsLike）
   */
  likes?: NewsLike[];

  /**
   * 新闻阅读记录列表（主子表关系） （子表：TaktNewsRead）
   */
  reads?: NewsRead[];

  /**
   * 新闻收藏记录列表（主子表关系） （子表：TaktNewsFavorite）
   */
  favorites?: NewsFavorite[];

  /**
   * 新闻分享记录列表（主子表关系） （子表：TaktNewsShare）
   */
  shares?: NewsShare[];

}


/**
 * News 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 NewsQuery
 * @description 对应后端 TaktNewsQueryDto
 */
export interface NewsQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

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
  tags?: string;

  /**
   * 新闻内容
   */
  newsContent?: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 是否置顶
   */
  isTop?: number;

  /**
   * 是否推荐
   */
  isRecommended?: number;

  /**
   * 生效时间（范围查询-开始）
   */
  effectiveTimeStart?: string;

  /**
   * 生效时间（范围查询-结束）
   */
  effectiveTimeEnd?: string;

  /**
   * 失效时间（范围查询-开始）
   */
  expireTimeStart?: string;

  /**
   * 失效时间（范围查询-结束）
   */
  expireTimeEnd?: string;

  /**
   * 阅读次数
   */
  readCount?: number;

  /**
   * 点赞次数
   */
  likeCount?: number;

  /**
   * 评论次数
   */
  commentCount?: number;

  /**
   * 收藏次数
   */
  favoriteCount?: number;

  /**
   * 分享次数
   */
  shareCount?: number;

  /**
   * 附件数量
   */
  attachmentCount?: number;

  /**
   * 流程实例 ID（关联工作流，如发布审批流程；流程侧 BusinessType=News、BusinessKey=本表 Id）
   */
  flowInstanceId?: string;

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
   * 发布时间（范围查询-开始）
   */
  publishTimeStart?: string;

  /**
   * 发布时间（范围查询-结束）
   */
  publishTimeEnd?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 新闻状态
   */
  newsStatus?: number;

  /**
   * 审批状态（TaktApprovalStatus）
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
 * 创建News DTO
 * 对应前端 NewsCreate
 * @description 对应后端 TaktNewsCreateDto
 */
export interface NewsCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

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
  tags?: string;

  /**
   * 新闻内容
   */
  newsContent: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 是否置顶
   */
  isTop: number;

  /**
   * 是否推荐
   */
  isRecommended: number;

  /**
   * 生效时间
   */
  effectiveTime?: string;

  /**
   * 失效时间
   */
  expireTime?: string;

  /**
   * 阅读次数
   */
  readCount: number;

  /**
   * 点赞次数
   */
  likeCount: number;

  /**
   * 评论次数
   */
  commentCount: number;

  /**
   * 收藏次数
   */
  favoriteCount: number;

  /**
   * 分享次数
   */
  shareCount: number;

  /**
   * 附件数量
   */
  attachmentCount: number;

  /**
   * 流程实例 ID（关联工作流，如发布审批流程；流程侧 BusinessType=News、BusinessKey=本表 Id）
   */
  flowInstanceId?: string;

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
  publishTime?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 新闻状态
   */
  newsStatus: number;

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
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新News DTO
 * 继承 TaktNewsCreateDto，添加 NewsId 字段
 * 对应前端 NewsUpdate
 * @description 对应后端 TaktNewsUpdateDto
 */
export interface NewsUpdate extends NewsCreate {
  /**
   * NewsID（标识要更新的实体）
   */
  newsId: string;

}


/**
 * News 状态更新 DTO
 * 对应前端 NewsStatus
 * @description 对应后端 TaktNewsStatusDto
 */
export interface NewsStatus {
  /**
   * NewsID
   */
  newsId: string;

  /**
   * 新闻状态
   */
  newsStatus: number;

}


/**
 * News 排序更新 DTO
 * 对应前端 NewsSort
 * @description 对应后端 TaktNewsSortDto
 */
export interface NewsSort {
  /**
   * NewsID
   */
  newsId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * News 导入模板行 DTO
 * 对应前端 NewsTemplate
 * @description 对应后端 TaktNewsTemplateDto
 */
export interface NewsTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

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
  tags?: string;

  /**
   * 新闻内容
   */
  newsContent?: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 是否置顶
   */
  isTop?: number;

  /**
   * 是否推荐
   */
  isRecommended?: number;

  /**
   * 阅读次数
   */
  readCount?: number;

  /**
   * 点赞次数
   */
  likeCount?: number;

  /**
   * 评论次数
   */
  commentCount?: number;

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
 * News 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 NewsImport
 * @description 对应后端 TaktNewsImportDto
 */
export interface NewsImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

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
  tags?: string;

  /**
   * 新闻内容
   */
  newsContent?: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 是否置顶
   */
  isTop?: number;

  /**
   * 是否推荐
   */
  isRecommended?: number;

  /**
   * 阅读次数
   */
  readCount?: number;

  /**
   * 点赞次数
   */
  likeCount?: number;

  /**
   * 评论次数
   */
  commentCount?: number;

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
  tags?: string;

  /**
   * 新闻内容
   */
  newsContent: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 是否置顶
   */
  isTop: number;

  /**
   * 是否推荐
   */
  isRecommended: number;

  /**
   * 生效时间
   */
  effectiveTime?: string;

  /**
   * 失效时间
   */
  expireTime?: string;

  /**
   * 阅读次数
   */
  readCount: number;

  /**
   * 点赞次数
   */
  likeCount: number;

  /**
   * 评论次数
   */
  commentCount: number;

  /**
   * 收藏次数
   */
  favoriteCount: number;

  /**
   * 分享次数
   */
  shareCount: number;

  /**
   * 附件数量
   */
  attachmentCount: number;

  /**
   * 流程实例 ID（关联工作流，如发布审批流程；流程侧 BusinessType=News、BusinessKey=本表 Id）
   */
  flowInstanceId?: string;

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
  publishTime?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 新闻状态
   */
  newsStatus: number;

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

