// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/document-center
// 文件名称：document-version.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/document-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 文管文档版本子实体
 * 对应前端 TaktDocumentVersionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 DocumentVersion
 * @description 对应后端 TaktDocumentVersionDto
 */
export interface DocumentVersion extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 文档 ID
   */
  documentId?: string;

  /**
   * 版本号
   */
  versionNo?: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID
   */
  fileId?: string;

  /**
   * 文件名称
   */
  fileName?: string;

  /**
   * 文件路径
   */
  filePath?: string;

  /**
   * 文件大小（字节）
   */
  fileSize?: string;

  /**
   * 文件类型（MIME）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 修订人 ID
   */
  revisedBy?: string;

  /**
   * 修订人姓名
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt?: string;

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
 * DocumentVersion 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DocumentVersionExport
 * @description 对应后端 TaktDocumentVersionExportDto
 */
export interface DocumentVersionExport {
  /**
   * DocumentVersionID
   */
  documentVersionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 文档 ID
   */
  documentId: string;

  /**
   * 版本号
   */
  versionNo: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID
   */
  fileId: string;

  /**
   * 文件名称
   */
  fileName: string;

  /**
   * 文件路径
   */
  filePath: string;

  /**
   * 文件大小（字节）
   */
  fileSize: string;

  /**
   * 文件类型（MIME）
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 修订人 ID
   */
  revisedBy: string;

  /**
   * 修订人姓名
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt: string;

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

