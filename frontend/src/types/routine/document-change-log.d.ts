// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine
// 文件名称：document-change-log.d.ts
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
 * 文管文档变更日志实体 完整记录文档的创建、修订、发布、归档、删除等历史
 * 对应前端 TaktDocumentChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 DocumentChangeLog
 * @description 对应后端 TaktDocumentChangeLogDto
 */
export interface DocumentChangeLog extends CompanyDtoBase {
  /**
   * DocumentChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  documentChangeLogId: string;

  /**
   * 文档 ID
   */
  documentId: string;

  /**
   * 文档 名称（填充字段）
   */
  documentName?: string;

  /**
   * 文档编码（冗余，便于日志列表展示）
   */
  documentCode?: string;

  /**
   * 文档标题（冗余，便于日志列表展示）
   */
  documentTitle?: string;

  /**
   * 变更类型
   */
  changeType: number;

  /**
   * 变更内容摘要（如「发布文档」「修订版本」「归档文档」等）
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组，记录字段旧值与新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更原因或备注（变更时间、变更人由基类 CreatedAt/CreatedBy 表示）
   */
  changeReason?: string;

  /**
   * 变更时文档版本号（与 TaktDocument.Version 对应）
   */
  versionAtChange?: number;

  /**
   * 文档（主表） （主表：TaktDocument）
   */
  document?: Document;

}


/**
 * DocumentChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DocumentChangeLogQuery
 * @description 对应后端 TaktDocumentChangeLogQueryDto
 */
export interface DocumentChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 文档 ID
   */
  documentId?: string;

  /**
   * 文档编码（冗余，便于日志列表展示）
   */
  documentCode?: string;

  /**
   * 文档标题（冗余，便于日志列表展示）
   */
  documentTitle?: string;

  /**
   * 变更类型
   */
  changeType?: number;

  /**
   * 变更内容摘要（如「发布文档」「修订版本」「归档文档」等）
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组，记录字段旧值与新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更原因或备注（变更时间、变更人由基类 CreatedAt/CreatedBy 表示）
   */
  changeReason?: string;

  /**
   * 变更时文档版本号（与 TaktDocument.Version 对应）
   */
  versionAtChange?: number;

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
 * 创建DocumentChangeLog DTO
 * 对应前端 DocumentChangeLogCreate
 * @description 对应后端 TaktDocumentChangeLogCreateDto
 */
export interface DocumentChangeLogCreate {
  /**
   * 文档 ID
   */
  documentId: string;

  /**
   * 文档编码（冗余，便于日志列表展示）
   */
  documentCode?: string;

  /**
   * 文档标题（冗余，便于日志列表展示）
   */
  documentTitle?: string;

  /**
   * 变更类型
   */
  changeType: number;

  /**
   * 变更内容摘要（如「发布文档」「修订版本」「归档文档」等）
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组，记录字段旧值与新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更原因或备注（变更时间、变更人由基类 CreatedAt/CreatedBy 表示）
   */
  changeReason?: string;

  /**
   * 变更时文档版本号（与 TaktDocument.Version 对应）
   */
  versionAtChange?: number;

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
 * 更新DocumentChangeLog DTO
 * 继承 TaktDocumentChangeLogCreateDto，添加 DocumentChangeLogId 字段
 * 对应前端 DocumentChangeLogUpdate
 * @description 对应后端 TaktDocumentChangeLogUpdateDto
 */
export interface DocumentChangeLogUpdate extends DocumentChangeLogCreate {
  /**
   * DocumentChangeLogID（标识要更新的实体）
   */
  documentChangeLogId: string;

}


/**
 * DocumentChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DocumentChangeLogExport
 * @description 对应后端 TaktDocumentChangeLogExportDto
 */
export interface DocumentChangeLogExport {
  /**
   * DocumentChangeLogID
   */
  documentChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 文档 ID
   */
  documentId: string;

  /**
   * 文档编码（冗余，便于日志列表展示）
   */
  documentCode?: string;

  /**
   * 文档标题（冗余，便于日志列表展示）
   */
  documentTitle?: string;

  /**
   * 变更类型
   */
  changeType: number;

  /**
   * 变更内容摘要（如「发布文档」「修订版本」「归档文档」等）
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组，记录字段旧值与新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更原因或备注（变更时间、变更人由基类 CreatedAt/CreatedBy 表示）
   */
  changeReason?: string;

  /**
   * 变更时文档版本号（与 TaktDocument.Version 对应）
   */
  versionAtChange?: number;

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

