// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine
// 文件名称：knowledge.d.ts
// 创建时间：2026-06-04
// 创建人：Takt365(Auto Generated)
// 功能描述：routine 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 服务台知识库实体
 * 对应前端 TaktKnowledgeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Knowledge
 * @description 对应后端 TaktKnowledgeDto
 */
export interface Knowledge extends CompanyDtoBase {
  /**
   * KnowledgeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  knowledgeId: string;

  /**
   * 知识标题
   */
  title: string;

  /**
   * 知识内容（富文本/HTML）
   */
  content?: string;

  /**
   * 知识摘要
   */
  summary?: string;

  /**
   * 分类编码（如 faq/guide）
   */
  categoryCode?: string;

  /**
   * 标签（逗号分隔或 JSON 数组）
   */
  tags?: string;

  /**
   * 知识状态
   */
  knowledgeStatus: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 浏览次数
   */
  viewCount: number;

  /**
   * 有用评价数
   */
  helpfulCount: number;

  /**
   * 无帮助评价数
   */
  unhelpfulCount: number;

  /**
   * 是否已发布
   */
  isPublished: number;

  /**
   * 版本号
   */
  version: number;

  /**
   * 发布时间
   */
  publishedAt?: string;

  /**
   * 最后修订时间
   */
  revisedAt?: string;

  /**
   * 知识库变更日志列表 （子表：TaktKnowledgeChangeLog）
   */
  changeLogs?: KnowledgeChangeLog[];

}


/**
 * Knowledge 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 KnowledgeQuery
 * @description 对应后端 TaktKnowledgeQueryDto
 */
export interface KnowledgeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 知识标题
   */
  title?: string;

  /**
   * 知识内容（富文本/HTML）
   */
  content?: string;

  /**
   * 知识摘要
   */
  summary?: string;

  /**
   * 分类编码（如 faq/guide）
   */
  categoryCode?: string;

  /**
   * 标签（逗号分隔或 JSON 数组）
   */
  tags?: string;

  /**
   * 知识状态
   */
  knowledgeStatus?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 浏览次数
   */
  viewCount?: number;

  /**
   * 有用评价数
   */
  helpfulCount?: number;

  /**
   * 无帮助评价数
   */
  unhelpfulCount?: number;

  /**
   * 是否已发布
   */
  isPublished?: number;

  /**
   * 版本号
   */
  version?: number;

  /**
   * 发布时间（范围查询-开始）
   */
  publishedAtStart?: string;

  /**
   * 发布时间（范围查询-结束）
   */
  publishedAtEnd?: string;

  /**
   * 最后修订时间（范围查询-开始）
   */
  revisedAtStart?: string;

  /**
   * 最后修订时间（范围查询-结束）
   */
  revisedAtEnd?: string;

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
 * 创建Knowledge DTO
 * 对应前端 KnowledgeCreate
 * @description 对应后端 TaktKnowledgeCreateDto
 */
export interface KnowledgeCreate {
  /**
   * 知识标题
   */
  title: string;

  /**
   * 知识内容（富文本/HTML）
   */
  content?: string;

  /**
   * 知识摘要
   */
  summary?: string;

  /**
   * 分类编码（如 faq/guide）
   */
  categoryCode?: string;

  /**
   * 标签（逗号分隔或 JSON 数组）
   */
  tags?: string;

  /**
   * 知识状态
   */
  knowledgeStatus: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 浏览次数
   */
  viewCount: number;

  /**
   * 有用评价数
   */
  helpfulCount: number;

  /**
   * 无帮助评价数
   */
  unhelpfulCount: number;

  /**
   * 是否已发布
   */
  isPublished: number;

  /**
   * 版本号
   */
  version: number;

  /**
   * 发布时间
   */
  publishedAt?: string;

  /**
   * 最后修订时间
   */
  revisedAt?: string;

  /**
   * 知识库变更日志列表（子表，级联保存）
   */
  changeLogs?: KnowledgeChangeLogCreate[];

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
 * 更新Knowledge DTO
 * 继承 TaktKnowledgeCreateDto，添加 KnowledgeId 字段
 * 对应前端 KnowledgeUpdate
 * @description 对应后端 TaktKnowledgeUpdateDto
 */
export interface KnowledgeUpdate extends KnowledgeCreate {
  /**
   * KnowledgeID（标识要更新的实体）
   */
  knowledgeId: string;

}


/**
 * Knowledge 状态更新 DTO
 * 对应前端 KnowledgeStatus
 * @description 对应后端 TaktKnowledgeStatusDto
 */
export interface KnowledgeStatus {
  /**
   * KnowledgeID
   */
  knowledgeId: string;

  /**
   * 知识状态
   */
  knowledgeStatus: number;

}


/**
 * Knowledge 排序更新 DTO
 * 对应前端 KnowledgeSort
 * @description 对应后端 TaktKnowledgeSortDto
 */
export interface KnowledgeSort {
  /**
   * KnowledgeID
   */
  knowledgeId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * Knowledge 导入模板行 DTO
 * 对应前端 KnowledgeTemplate
 * @description 对应后端 TaktKnowledgeTemplateDto
 */
export interface KnowledgeTemplate {
  /**
   * 知识标题
   */
  title?: string;

  /**
   * 知识内容（富文本/HTML）
   */
  content?: string;

  /**
   * 知识摘要
   */
  summary?: string;

  /**
   * 分类编码（如 faq/guide）
   */
  categoryCode?: string;

  /**
   * 标签（逗号分隔或 JSON 数组）
   */
  tags?: string;

  /**
   * 知识状态
   */
  knowledgeStatus?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 浏览次数
   */
  viewCount?: number;

  /**
   * 有用评价数
   */
  helpfulCount?: number;

  /**
   * 无帮助评价数
   */
  unhelpfulCount?: number;

  /**
   * 是否已发布
   */
  isPublished?: number;

  /**
   * 版本号
   */
  version?: number;

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
 * Knowledge 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 KnowledgeImport
 * @description 对应后端 TaktKnowledgeImportDto
 */
export interface KnowledgeImport {
  /**
   * 知识标题
   */
  title?: string;

  /**
   * 知识内容（富文本/HTML）
   */
  content?: string;

  /**
   * 知识摘要
   */
  summary?: string;

  /**
   * 分类编码（如 faq/guide）
   */
  categoryCode?: string;

  /**
   * 标签（逗号分隔或 JSON 数组）
   */
  tags?: string;

  /**
   * 知识状态
   */
  knowledgeStatus?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 浏览次数
   */
  viewCount?: number;

  /**
   * 有用评价数
   */
  helpfulCount?: number;

  /**
   * 无帮助评价数
   */
  unhelpfulCount?: number;

  /**
   * 是否已发布
   */
  isPublished?: number;

  /**
   * 版本号
   */
  version?: number;

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
 * Knowledge 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 KnowledgeExport
 * @description 对应后端 TaktKnowledgeExportDto
 */
export interface KnowledgeExport {
  /**
   * KnowledgeID
   */
  knowledgeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 知识标题
   */
  title: string;

  /**
   * 知识内容（富文本/HTML）
   */
  content?: string;

  /**
   * 知识摘要
   */
  summary?: string;

  /**
   * 分类编码（如 faq/guide）
   */
  categoryCode?: string;

  /**
   * 标签（逗号分隔或 JSON 数组）
   */
  tags?: string;

  /**
   * 知识状态
   */
  knowledgeStatus: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 浏览次数
   */
  viewCount: number;

  /**
   * 有用评价数
   */
  helpfulCount: number;

  /**
   * 无帮助评价数
   */
  unhelpfulCount: number;

  /**
   * 是否已发布
   */
  isPublished: number;

  /**
   * 版本号
   */
  version: number;

  /**
   * 发布时间
   */
  publishedAt?: string;

  /**
   * 最后修订时间
   */
  revisedAt?: string;

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

