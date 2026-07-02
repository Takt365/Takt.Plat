// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-inquiry.d.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 采购询价实体
 * 对应前端 TaktPurchaseInquiryDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 PurchaseInquiry
 * @description 对应后端 TaktPurchaseInquiryDto
 */
export interface PurchaseInquiry extends ApprovalDtoBase {
  /**
   * PurchaseInquiryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInquiryId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode: string;

  /**
   * 询价日期
   */
  inquiryDate: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工 ID（关联 TaktEmployee）
   */
  inquiryId?: string;

  /**
   * 询价人员工 名称（填充字段）
   */
  inquiryName?: string;

  /**
   * 询价人（人员代码）
   */
  inquiryBy: string;

  /**
   * 询价供应商编码
   */
  supplierCode?: string;

  /**
   * 询价供应商名称
   */
  supplierName?: string;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 询价总金额
   */
  totalAmount: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转价格金额
   */
  convertedAmount: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable：1=启用，0=禁用）
   */
  inquiryStatus: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 采购询价明细列表（主子表关系） （子表：TaktPurchaseInquiryItem）
   */
  items?: PurchaseInquiryItem[];

}


/**
 * PurchaseInquiry 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInquiryQuery
 * @description 对应后端 TaktPurchaseInquiryQueryDto
 */
export interface PurchaseInquiryQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode?: string;

  /**
   * 询价日期（范围查询-开始）
   */
  inquiryDateStart?: string;

  /**
   * 询价日期（范围查询-结束）
   */
  inquiryDateEnd?: string;

  /**
   * 报价截止日期（范围查询-开始）
   */
  quoteDeadlineDateStart?: string;

  /**
   * 报价截止日期（范围查询-结束）
   */
  quoteDeadlineDateEnd?: string;

  /**
   * 询价人员工 ID（关联 TaktEmployee）
   */
  inquiryId?: string;

  /**
   * 询价人（人员代码）
   */
  inquiryBy?: string;

  /**
   * 询价供应商编码
   */
  supplierCode?: string;

  /**
   * 询价供应商名称
   */
  supplierName?: string;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 询价总金额
   */
  totalAmount?: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转价格金额
   */
  convertedAmount?: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable：1=启用，0=禁用）
   */
  inquiryStatus?: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

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
 * 创建PurchaseInquiry DTO
 * 对应前端 PurchaseInquiryCreate
 * @description 对应后端 TaktPurchaseInquiryCreateDto
 */
export interface PurchaseInquiryCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode: string;

  /**
   * 询价日期
   */
  inquiryDate: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工 ID（关联 TaktEmployee）
   */
  inquiryId?: string;

  /**
   * 询价人（人员代码）
   */
  inquiryBy: string;

  /**
   * 询价供应商编码
   */
  supplierCode?: string;

  /**
   * 询价供应商名称
   */
  supplierName?: string;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 询价总金额
   */
  totalAmount: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转价格金额
   */
  convertedAmount: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable：1=启用，0=禁用）
   */
  inquiryStatus: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 采购询价明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInquiryItemCreate[];

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
 * 更新PurchaseInquiry DTO
 * 继承 TaktPurchaseInquiryCreateDto，添加 PurchaseInquiryId 字段
 * 对应前端 PurchaseInquiryUpdate
 * @description 对应后端 TaktPurchaseInquiryUpdateDto
 */
export interface PurchaseInquiryUpdate extends PurchaseInquiryCreate {
  /**
   * PurchaseInquiryID（标识要更新的实体）
   */
  purchaseInquiryId: string;

}


/**
 * PurchaseInquiry 状态更新 DTO
 * 对应前端 PurchaseInquiryStatus
 * @description 对应后端 TaktPurchaseInquiryStatusDto
 */
export interface PurchaseInquiryStatus {
  /**
   * PurchaseInquiryID
   */
  purchaseInquiryId: string;

  /**
   * 询价状态（字典 sys_normal_disable：1=启用，0=禁用）
   */
  inquiryStatus: number;

}


/**
 * PurchaseInquiry 导入模板行 DTO
 * 对应前端 PurchaseInquiryTemplate
 * @description 对应后端 TaktPurchaseInquiryTemplateDto
 */
export interface PurchaseInquiryTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode?: string;

  /**
   * 询价日期
   */
  inquiryDate?: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工 ID（关联 TaktEmployee）
   */
  inquiryId?: string;

  /**
   * 询价人（人员代码）
   */
  inquiryBy?: string;

  /**
   * 询价供应商编码
   */
  supplierCode?: string;

  /**
   * 询价供应商名称
   */
  supplierName?: string;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 询价总金额
   */
  totalAmount?: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转价格金额
   */
  convertedAmount?: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable：1=启用，0=禁用）
   */
  inquiryStatus?: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 采购询价明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInquiryItemCreate[];

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
 * PurchaseInquiry 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInquiryImport
 * @description 对应后端 TaktPurchaseInquiryImportDto
 */
export interface PurchaseInquiryImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode?: string;

  /**
   * 询价日期
   */
  inquiryDate?: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工 ID（关联 TaktEmployee）
   */
  inquiryId?: string;

  /**
   * 询价人（人员代码）
   */
  inquiryBy?: string;

  /**
   * 询价供应商编码
   */
  supplierCode?: string;

  /**
   * 询价供应商名称
   */
  supplierName?: string;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 询价总金额
   */
  totalAmount?: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转价格金额
   */
  convertedAmount?: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable：1=启用，0=禁用）
   */
  inquiryStatus?: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 采购询价明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInquiryItemCreate[];

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
 * PurchaseInquiry 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInquiryExport
 * @description 对应后端 TaktPurchaseInquiryExportDto
 */
export interface PurchaseInquiryExport {
  /**
   * PurchaseInquiryID
   */
  purchaseInquiryId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 采购询价编码（租户+公司+工厂内业务唯一）
   */
  purchaseInquiryCode: string;

  /**
   * 询价日期
   */
  inquiryDate: string;

  /**
   * 报价截止日期
   */
  quoteDeadlineDate?: string;

  /**
   * 询价人员工 ID（关联 TaktEmployee）
   */
  inquiryId?: string;

  /**
   * 询价人（人员代码）
   */
  inquiryBy: string;

  /**
   * 询价供应商编码
   */
  supplierCode?: string;

  /**
   * 询价供应商名称
   */
  supplierName?: string;

  /**
   * 询价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 询价总金额
   */
  totalAmount: number;

  /**
   * 已转价格数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转价格金额
   */
  convertedAmount: number;

  /**
   * 询价原因
   */
  inquiryReason?: string;

  /**
   * 询价状态（字典 sys_normal_disable：1=启用，0=禁用）
   */
  inquiryStatus: number;

  /**
   * 转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

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

