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
   * NewsCommentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  newsCommentId: string;

  /**
   * 新闻 ID
   */
  newsId: string;

  /**
   * 新闻 名称（填充字段）
   */
  newsName?: string;

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
   * 新闻（主表） （主表：TaktNews）
   */
  news?: News;

  /**
   * 评论点赞记录列表（主子表关系） （子表：TaktNewsCommentLike）
   */
  likes?: NewsCommentLike[];

}


/**
 * NewsComment 树形列表/树选择 DTO（含子节点）
 * 对应 GetNewsCommentTreeAsync 等接口
 * 对应前端 NewsCommentTree
 * @description 对应后端 TaktNewsCommentTreeDto
 */
export interface NewsCommentTree extends NewsComment {
  /**
   * 子节点
   */
  children: NewsCommentTree[];

}


/**
 * NewsComment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 NewsCommentQuery
 * @description 对应后端 TaktNewsCommentQueryDto
 */
export interface NewsCommentQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

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
   * 评论时间（范围查询-开始）
   */
  commentTimeStart?: string;

  /**
   * 评论时间（范围查询-结束）
   */
  commentTimeEnd?: string;

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
 * 创建NewsComment DTO
 * 对应前端 NewsCommentCreate
 * @description 对应后端 TaktNewsCommentCreateDto
 */
export interface NewsCommentCreate {
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
 * 更新NewsComment DTO
 * 继承 TaktNewsCommentCreateDto，添加 NewsCommentId 字段
 * 对应前端 NewsCommentUpdate
 * @description 对应后端 TaktNewsCommentUpdateDto
 */
export interface NewsCommentUpdate extends NewsCommentCreate {
  /**
   * NewsCommentID（标识要更新的实体）
   */
  newsCommentId: string;

}


/**
 * NewsComment 状态更新 DTO
 * 对应前端 NewsCommentStatus
 * @description 对应后端 TaktNewsCommentStatusDto
 */
export interface NewsCommentStatus {
  /**
   * NewsCommentID
   */
  newsCommentId: string;

  /**
   * 评论状态
   */
  commentStatus: number;

}


/**
 * NewsComment 导入模板行 DTO
 * 对应前端 NewsCommentTemplate
 * @description 对应后端 TaktNewsCommentTemplateDto
 */
export interface NewsCommentTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

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
 * NewsComment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 NewsCommentImport
 * @description 对应后端 TaktNewsCommentImportDto
 */
export interface NewsCommentImport {
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

