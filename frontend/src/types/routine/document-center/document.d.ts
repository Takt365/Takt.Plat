// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/document-center
// 文件名称：document.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/document-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制；需审批通过后发布（草稿→审批→发布）
 * 对应前端 TaktDocumentDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Document
 * @description 对应后端 TaktDocumentDto
 */
export interface Document extends ApprovalDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 文档编码（租户+公司内唯一）
   */
  documentCode?: string;

  /**
   * 文档标题
   */
  documentTitle?: string;

  /**
   * 文档分类
   */
  documentCategory?: number;

  /**
   * 密级
   */
  confidentialLevel?: number;

  /**
   * 当前版本号
   */
  version?: number;

  /**
   * 文档内容（富文本 HTML）
   */
  documentContent?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  documentSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  documentTags?: string;

  /**
   * 当前文件 ID
   */
  fileId?: string;

  /**
   * 当前文件名称
   */
  fileName?: string;

  /**
   * 当前文件路径
   */
  filePath?: string;

  /**
   * 当前文件大小（字节）
   */
  fileSize?: string;

  /**
   * 当前文件类型（MIME）
   */
  fileType?: string;

  /**
   * 当前文件扩展名
   */
  fileExtension?: string;

  /**
   * 生效时间
   */
  documentEffectiveTime?: string;

  /**
   * 失效时间
   */
  documentExpireTime?: string;

  /**
   * 发布时间
   */
  documentPublishTime?: string;

  /**
   * 发布人 ID
   */
  publisherId?: string;

  /**
   * 发布人姓名
   */
  publisherName?: string;

  /**
   * 归属部门 ID
   */
  deptId?: string;

  /**
   * 归属部门名称
   */
  deptName?: string;

  /**
   * 置顶
   */
  documentIsTop?: number;

  /**
   * 浏览次数
   */
  documentViewCount?: number;

  /**
   * 下载次数
   */
  downloadCount?: number;

  /**
   * 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
   */
  targetScope?: string;

  /**
   * 目标部门编码（多个用逗号分隔）
   */
  targetDepartments?: string;

  /**
   * 目标用户 ID（多个用逗号分隔）
   */
  targetUsers?: string;

  /**
   * 文档状态
   */
  documentStatus?: number;

  /**
   * 版本历史列表（主子表关系）（子表，级联保存）
   */
  versions?: DocumentVersionCreate[];

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
 * Document 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DocumentExport
 * @description 对应后端 TaktDocumentExportDto
 */
export interface DocumentExport {
  /**
   * DocumentID
   */
  documentId: string;

  /**
   * 文档编码（租户+公司内唯一）
   */
  documentCode: string;

  /**
   * 文档标题
   */
  documentTitle: string;

  /**
   * 文档分类
   */
  documentCategory: number;

  /**
   * 密级
   */
  confidentialLevel: number;

  /**
   * 当前版本号
   */
  version: number;

  /**
   * 文档内容（富文本 HTML）
   */
  documentContent?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  documentSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  documentTags?: string;

  /**
   * 当前文件 ID
   */
  fileId?: string;

  /**
   * 当前文件名称
   */
  fileName?: string;

  /**
   * 当前文件路径
   */
  filePath?: string;

  /**
   * 当前文件大小（字节）
   */
  fileSize: string;

  /**
   * 当前文件类型（MIME）
   */
  fileType?: string;

  /**
   * 当前文件扩展名
   */
  fileExtension?: string;

  /**
   * 生效时间
   */
  documentEffectiveTime?: string;

  /**
   * 失效时间
   */
  documentExpireTime?: string;

  /**
   * 发布时间
   */
  documentPublishTime?: string;

  /**
   * 发布人 ID
   */
  publisherId: string;

  /**
   * 发布人姓名
   */
  publisherName: string;

  /**
   * 归属部门 ID
   */
  deptId?: string;

  /**
   * 归属部门名称
   */
  deptName?: string;

  /**
   * 置顶
   */
  documentIsTop: number;

  /**
   * 浏览次数
   */
  documentViewCount: number;

  /**
   * 下载次数
   */
  downloadCount: number;

  /**
   * 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
   */
  targetScope: string;

  /**
   * 目标部门编码（多个用逗号分隔）
   */
  targetDepartments?: string;

  /**
   * 目标用户 ID（多个用逗号分隔）
   */
  targetUsers?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 文档状态
   */
  documentStatus: number;

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

