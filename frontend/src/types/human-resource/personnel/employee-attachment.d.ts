// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-attachment.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 员工附件类型；文件元数据由 TaktFile 统一管理，仅存 EmployeeId、AttachmentName、AccessUrl。
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 员工档案附件（主档子表，公司级非审批单）
 * @description 对应后端 TaktEmployeeAttachmentDto
 */
export interface EmployeeAttachment extends CompanyDtoBase {
  /**
   * EmployeeAttachmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeAttachmentId: string;

  /**
   * 员工ID
   */
  employeeId: string;

  /**
   * 员工名称（填充字段）
   */
  employeeName?: string;

  /**
   * 附件名称（业务称谓，如毕业证、就业证）
   */
  attachmentName: string;

  /**
   * 访问地址（引用 TaktFile.AccessUrl）
   */
  accessUrl: string;
}

/**
 * EmployeeAttachment 分页查询 DTO
 * @description 对应后端 TaktEmployeeAttachmentQueryDto
 */
export interface EmployeeAttachmentQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 员工ID
   */
  employeeId?: string;

  /**
   * 附件名称
   */
  attachmentName?: string;

  /**
   * 访问地址
   */
  accessUrl?: string;

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
 * 创建EmployeeAttachment DTO
 * @description 对应后端 TaktEmployeeAttachmentCreateDto
 */
export interface EmployeeAttachmentCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47
   */
  companyDefaultCulture: string;

  /**
   * 员工ID
   */
  employeeId: string;

  /**
   * 附件名称（业务称谓，如毕业证、就业证）
   */
  attachmentName: string;

  /**
   * 访问地址（引用 TaktFile.AccessUrl）
   */
  accessUrl: string;

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
 * 更新EmployeeAttachment DTO
 * @description 对应后端 TaktEmployeeAttachmentUpdateDto
 */
export interface EmployeeAttachmentUpdate extends EmployeeAttachmentCreate {
  /**
   * EmployeeAttachmentID（标识要更新的实体）
   */
  employeeAttachmentId: string;
}

/**
 * EmployeeAttachment 导入模板行 DTO
 * @description 对应后端 TaktEmployeeAttachmentTemplateDto
 */
export interface EmployeeAttachmentTemplate {
  tenantCode?: string;
  companyCode?: string;
  employeeId?: string;
  attachmentName?: string;
  accessUrl?: string;
  extField?: string;
  remark?: string;
}

/**
 * EmployeeAttachment 导入 DTO
 * @description 对应后端 TaktEmployeeAttachmentImportDto
 */
export interface EmployeeAttachmentImport {
  tenantCode?: string;
  companyCode?: string;
  companyDefaultCulture?: string;
  employeeId?: string;
  attachmentName?: string;
  accessUrl?: string;
  extField?: string;
  remark?: string;
}

/**
 * EmployeeAttachment 导出 DTO
 * @description 对应后端 TaktEmployeeAttachmentExportDto
 */
export interface EmployeeAttachmentExport {
  employeeAttachmentId: string;
  companyCode: string;
  employeeId: string;
  attachmentName: string;
  accessUrl: string;
  extField?: string;
  remark?: string;
  createdAt: string;
}
