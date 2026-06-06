// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/document-center
// 文件名称：document.d.ts
// 创建时间：2026-06-06
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
   * DocumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  documentId: string;

  /**
   * 文档编码（租户+公司内唯一）
   */
  documentCode: string;

  /**
   * 文档标题
   */
  title: string;

  /**
   * 文档分类
   */
  documentCategory: number;

  /**
   * 文档状态
   */
  documentStatus: number;

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
  content?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

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
  effectiveTime?: string;

  /**
   * 失效时间
   */
  expireTime?: string;

  /**
   * 发布时间
   */
  publishTime?: string;

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
   * 是否置顶
   */
  isTop: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 浏览次数
   */
  viewCount: number;

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
   * 版本历史列表（主子表关系） （子表：TaktDocumentVersion）
   */
  versions?: DocumentVersion[];

  /**
   * 变更日志列表（主子表关系） （子表：TaktDocumentChangeLog）
   */
  changeLogs?: DocumentChangeLog[];

}


/**
 * Document 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DocumentQuery
 * @description 对应后端 TaktDocumentQueryDto
 */
export interface DocumentQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 文档编码（租户+公司内唯一）
   */
  documentCode?: string;

  /**
   * 文档标题
   */
  title?: string;

  /**
   * 文档分类
   */
  documentCategory?: number;

  /**
   * 文档状态
   */
  documentStatus?: number;

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
  content?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

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
   * 发布时间（范围查询-开始）
   */
  publishTimeStart?: string;

  /**
   * 发布时间（范围查询-结束）
   */
  publishTimeEnd?: string;

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
   * 是否置顶
   */
  isTop?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 浏览次数
   */
  viewCount?: number;

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
 * 创建Document DTO
 * 对应前端 DocumentCreate
 * @description 对应后端 TaktDocumentCreateDto
 */
export interface DocumentCreate {
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
   * 文档编码（租户+公司内唯一）
   */
  documentCode: string;

  /**
   * 文档标题
   */
  title: string;

  /**
   * 文档分类
   */
  documentCategory: number;

  /**
   * 文档状态
   */
  documentStatus: number;

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
  content?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

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
  effectiveTime?: string;

  /**
   * 失效时间
   */
  expireTime?: string;

  /**
   * 发布时间
   */
  publishTime?: string;

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
   * 是否置顶
   */
  isTop: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 浏览次数
   */
  viewCount: number;

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
   * 版本历史列表（主子表关系）（子表，级联保存）
   */
  versions?: DocumentVersionCreate[];

  /**
   * 变更日志列表（主子表关系）（子表，级联保存）
   */
  changeLogs?: DocumentChangeLogCreate[];

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
 * 更新Document DTO
 * 继承 TaktDocumentCreateDto，添加 DocumentId 字段
 * 对应前端 DocumentUpdate
 * @description 对应后端 TaktDocumentUpdateDto
 */
export interface DocumentUpdate extends DocumentCreate {
  /**
   * DocumentID（标识要更新的实体）
   */
  documentId: string;

}


/**
 * Document 状态更新 DTO
 * 对应前端 DocumentStatus
 * @description 对应后端 TaktDocumentStatusDto
 */
export interface DocumentStatus {
  /**
   * DocumentID
   */
  documentId: string;

  /**
   * 文档状态
   */
  documentStatus: number;

}


/**
 * Document 排序更新 DTO
 * 对应前端 DocumentSort
 * @description 对应后端 TaktDocumentSortDto
 */
export interface DocumentSort {
  /**
   * DocumentID
   */
  documentId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * Document 导入模板行 DTO
 * 对应前端 DocumentTemplate
 * @description 对应后端 TaktDocumentTemplateDto
 */
export interface DocumentTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 文档编码（租户+公司内唯一）
   */
  documentCode?: string;

  /**
   * 文档标题
   */
  title?: string;

  /**
   * 文档分类
   */
  documentCategory?: number;

  /**
   * 文档状态
   */
  documentStatus?: number;

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
  content?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

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
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Document 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 DocumentImport
 * @description 对应后端 TaktDocumentImportDto
 */
export interface DocumentImport {
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
   * 文档编码（租户+公司内唯一）
   */
  documentCode?: string;

  /**
   * 文档标题
   */
  title?: string;

  /**
   * 文档分类
   */
  documentCategory?: number;

  /**
   * 文档状态
   */
  documentStatus?: number;

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
  content?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

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
   * 扩展字段JSON
   */
  extFieldJson?: string;

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
  title: string;

  /**
   * 文档分类
   */
  documentCategory: number;

  /**
   * 文档状态
   */
  documentStatus: number;

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
  content?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  summary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  tags?: string;

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
  effectiveTime?: string;

  /**
   * 失效时间
   */
  expireTime?: string;

  /**
   * 发布时间
   */
  publishTime?: string;

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
   * 是否置顶
   */
  isTop: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 浏览次数
   */
  viewCount: number;

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

