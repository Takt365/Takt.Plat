// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customer-complaint-item.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 客诉明细实体
 * 对应前端 TaktCustomerComplaintItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerComplaintItem
 * @description 对应后端 TaktCustomerComplaintItemDto
 */
export interface CustomerComplaintItem extends CompanyDtoBase {
  /**
   * CustomerComplaintItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerComplaintItemId: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId: string;

  /**
   * 客诉名称（填充字段）
   */
  complaintName?: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  customerComplaintCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品编码
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
   */
  itemType: number;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善责任人
   */
  improvementResponsible?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
   */
  improvementStatus: number;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachmentPaths?: string;

  /**
   * 客诉主表 （主表：TaktCustomerComplaint）
   */
  complaint?: CustomerComplaint;

}


/**
 * CustomerComplaintItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerComplaintItemQuery
 * @description 对应后端 TaktCustomerComplaintItemQueryDto
 */
export interface CustomerComplaintItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  customerComplaintCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品编码
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
   */
  itemType?: number;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善责任人
   */
  improvementResponsible?: string;

  /**
   * 计划完成日期（范围查询-开始）
   */
  plannedCompletionDateStart?: string;

  /**
   * 计划完成日期（范围查询-结束）
   */
  plannedCompletionDateEnd?: string;

  /**
   * 实际完成日期（范围查询-开始）
   */
  actualCompletionDateStart?: string;

  /**
   * 实际完成日期（范围查询-结束）
   */
  actualCompletionDateEnd?: string;

  /**
   * 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
   */
  improvementStatus?: number;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachmentPaths?: string;

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
 * 创建CustomerComplaintItem DTO
 * 对应前端 CustomerComplaintItemCreate
 * @description 对应后端 TaktCustomerComplaintItemCreateDto
 */
export interface CustomerComplaintItemCreate {
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
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  customerComplaintCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品编码
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
   */
  itemType: number;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善责任人
   */
  improvementResponsible?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
   */
  improvementStatus: number;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachmentPaths?: string;

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
 * 更新CustomerComplaintItem DTO
 * 继承 TaktCustomerComplaintItemCreateDto，添加 CustomerComplaintItemId 字段
 * 对应前端 CustomerComplaintItemUpdate
 * @description 对应后端 TaktCustomerComplaintItemUpdateDto
 */
export interface CustomerComplaintItemUpdate extends CustomerComplaintItemCreate {
  /**
   * CustomerComplaintItemID（标识要更新的实体）
   */
  customerComplaintItemId: string;

}


/**
 * CustomerComplaintItem 状态更新 DTO
 * 对应前端 CustomerComplaintItemStatus
 * @description 对应后端 TaktCustomerComplaintItemStatusDto
 */
export interface CustomerComplaintItemStatus {
  /**
   * CustomerComplaintItemID
   */
  customerComplaintItemId: string;

  /**
   * 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
   */
  improvementStatus: number;

}


/**
 * CustomerComplaintItem 导入模板行 DTO
 * 对应前端 CustomerComplaintItemTemplate
 * @description 对应后端 TaktCustomerComplaintItemTemplateDto
 */
export interface CustomerComplaintItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  customerComplaintCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品编码
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
   */
  itemType?: number;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

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
 * CustomerComplaintItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerComplaintItemImport
 * @description 对应后端 TaktCustomerComplaintItemImportDto
 */
export interface CustomerComplaintItemImport {
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
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  customerComplaintCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品编码
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
   */
  itemType?: number;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

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
 * CustomerComplaintItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerComplaintItemExport
 * @description 对应后端 TaktCustomerComplaintItemExportDto
 */
export interface CustomerComplaintItemExport {
  /**
   * CustomerComplaintItemID
   */
  customerComplaintItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 客诉ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  complaintId: string;

  /**
   * 客诉单号（冗余字段，便于查询）
   */
  customerComplaintCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品编码
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）
   */
  itemType: number;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善责任人
   */
  improvementResponsible?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 改善状态（0=待改善，1=改善中，2=已完成，3=已验证）
   */
  improvementStatus: number;

  /**
   * 附件路径（多个附件用逗号分隔）
   */
  attachmentPaths?: string;

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

