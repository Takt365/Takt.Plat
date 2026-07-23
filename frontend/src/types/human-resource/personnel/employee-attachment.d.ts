// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-attachment.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 员工档案附件（主档子表，公司级非审批单）；文件元数据见 TaktFile，本表仅存业务名称与访问地址引用。
 * 对应前端 TaktEmployeeAttachmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeAttachment
 * @description 对应后端 TaktEmployeeAttachmentDto
 */
export interface EmployeeAttachment extends CompanyDtoBase {
  /**
   * EmployeeAttachmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeAttachmentId: string;

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 附件名称（业务称谓，如毕业证、就业证）
   */
  attachmentName: string;

  /**
   * 访问地址（关联 TaktFile.AccessUrl）
   */
  accessUrl: string;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

}


/**
 * EmployeeAttachment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeAttachmentQuery
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 附件名称（业务称谓，如毕业证、就业证）
   */
  attachmentName?: string;

  /**
   * 访问地址（关联 TaktFile.AccessUrl）
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
 * 对应前端 EmployeeAttachmentCreate
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
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 附件名称（业务称谓，如毕业证、就业证）
   */
  attachmentName: string;

  /**
   * 访问地址（关联 TaktFile.AccessUrl）
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
 * 继承 TaktEmployeeAttachmentCreateDto，添加 EmployeeAttachmentId 字段
 * 对应前端 EmployeeAttachmentUpdate
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
 * 对应前端 EmployeeAttachmentTemplate
 * @description 对应后端 TaktEmployeeAttachmentTemplateDto
 */
export interface EmployeeAttachmentTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 附件名称（业务称谓，如毕业证、就业证）
   */
  attachmentName?: string;

  /**
   * 访问地址（关联 TaktFile.AccessUrl）
   */
  accessUrl?: string;

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
 * EmployeeAttachment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeAttachmentImport
 * @description 对应后端 TaktEmployeeAttachmentImportDto
 */
export interface EmployeeAttachmentImport {
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 附件名称（业务称谓，如毕业证、就业证）
   */
  attachmentName?: string;

  /**
   * 访问地址（关联 TaktFile.AccessUrl）
   */
  accessUrl?: string;

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
 * EmployeeAttachment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeAttachmentExport
 * @description 对应后端 TaktEmployeeAttachmentExportDto
 */
export interface EmployeeAttachmentExport {
  /**
   * EmployeeAttachmentID
   */
  employeeAttachmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 附件名称（业务称谓，如毕业证、就业证）
   */
  attachmentName: string;

  /**
   * 访问地址（关联 TaktFile.AccessUrl）
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

  /**
   * 创建时间
   */
  createdAt: string;

}

