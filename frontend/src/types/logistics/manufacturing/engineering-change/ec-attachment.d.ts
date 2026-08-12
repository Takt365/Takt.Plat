// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-attachment.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 设变附件实体（技术阶段一 ②，隶属 TaktEcGijutsu）。文件类别见字典 logistics_ec_attachment_type；与主表、明细保存后由系统生成 TaktEcNotification。
 * 对应前端 TaktEcAttachmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcAttachment
 * @description 对应后端 TaktEcAttachmentDto
 */
export interface EcAttachment extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 设变主表ID
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 文件类别（字典 logistics_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
   */
  attachmentType?: string;

  /**
   * 文件编码（如联络编码等）
   */
  docCode?: string;

  /**
   * 文件名称
   */
  fileName?: string;

  /**
   * 访问地址（URL）
   */
  accessUrl?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * EcAttachment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcAttachmentExport
 * @description 对应后端 TaktEcAttachmentExportDto
 */
export interface EcAttachmentExport {
  /**
   * EcAttachmentID
   */
  ecAttachmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 设变主表ID
   */
  ecId: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 文件类别（字典 logistics_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
   */
  attachmentType: string;

  /**
   * 文件编码（如联络编码等）
   */
  docCode: string;

  /**
   * 文件名称
   */
  fileName: string;

  /**
   * 访问地址（URL）
   */
  accessUrl: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

