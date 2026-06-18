// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：purchase-order-item.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt采购订单明细实体
 * 对应前端 TaktPurchaseOrderItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseOrderItem
 * @description 对应后端 TaktPurchaseOrderItemDto
 */
export interface PurchaseOrderItem extends CompanyDtoBase {
  /**
   * PurchaseOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseOrderItemId: string;

  /**
   * 采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchaseOrderId: string;

  /**
   * 采购订单名称（填充字段）
   */
  purchaseOrderName?: string;

  /**
   * 采购订单编码（冗余字段，便于查询）
   */
  purchaseOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 采购单位
   */
  purchaseUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 单价（精确到分，存储为整数，单位为分）
   */
  unitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 税费率（0-100，表示税费百分比）
   */
  taxRate: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount: number;

  /**
   * 小计金额（精确到分，存储为整数，单位为分）
   */
  subtotalAmount: number;

  /**
   * 行交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

}


/**
 * PurchaseOrderItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseOrderItemQuery
 * @description 对应后端 TaktPurchaseOrderItemQueryDto
 */
export interface PurchaseOrderItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchaseOrderId?: string;

  /**
   * 采购订单编码（冗余字段，便于查询）
   */
  purchaseOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 采购单位
   */
  purchaseUnit?: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity?: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity?: number;

  /**
   * 单价（精确到分，存储为整数，单位为分）
   */
  unitPrice?: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

  /**
   * 税费率（0-100，表示税费百分比）
   */
  taxRate?: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount?: number;

  /**
   * 小计金额（精确到分，存储为整数，单位为分）
   */
  subtotalAmount?: number;

  /**
   * 行交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

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
 * 创建PurchaseOrderItem DTO
 * 对应前端 PurchaseOrderItemCreate
 * @description 对应后端 TaktPurchaseOrderItemCreateDto
 */
export interface PurchaseOrderItemCreate {
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
   * 采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchaseOrderId: string;

  /**
   * 采购订单编码（冗余字段，便于查询）
   */
  purchaseOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 采购单位
   */
  purchaseUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 单价（精确到分，存储为整数，单位为分）
   */
  unitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 税费率（0-100，表示税费百分比）
   */
  taxRate: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount: number;

  /**
   * 小计金额（精确到分，存储为整数，单位为分）
   */
  subtotalAmount: number;

  /**
   * 行交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

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
 * 更新PurchaseOrderItem DTO
 * 继承 TaktPurchaseOrderItemCreateDto，添加 PurchaseOrderItemId 字段
 * 对应前端 PurchaseOrderItemUpdate
 * @description 对应后端 TaktPurchaseOrderItemUpdateDto
 */
export interface PurchaseOrderItemUpdate extends PurchaseOrderItemCreate {
  /**
   * PurchaseOrderItemID（标识要更新的实体）
   */
  purchaseOrderItemId: string;

}


/**
 * PurchaseOrderItem 状态更新 DTO
 * 对应前端 PurchaseOrderItemStatus
 * @description 对应后端 TaktPurchaseOrderItemStatusDto
 */
export interface PurchaseOrderItemStatus {
  /**
   * PurchaseOrderItemID
   */
  purchaseOrderItemId: string;

  /**
   * 行交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

}


/**
 * PurchaseOrderItem 导入模板行 DTO
 * 对应前端 PurchaseOrderItemTemplate
 * @description 对应后端 TaktPurchaseOrderItemTemplateDto
 */
export interface PurchaseOrderItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchaseOrderId?: string;

  /**
   * 采购订单编码（冗余字段，便于查询）
   */
  purchaseOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 采购单位
   */
  purchaseUnit?: string;

  /**
   * 行交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

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
 * PurchaseOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseOrderItemImport
 * @description 对应后端 TaktPurchaseOrderItemImportDto
 */
export interface PurchaseOrderItemImport {
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
   * 采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchaseOrderId?: string;

  /**
   * 采购订单编码（冗余字段，便于查询）
   */
  purchaseOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 采购单位
   */
  purchaseUnit?: string;

  /**
   * 行交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

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
 * PurchaseOrderItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseOrderItemExport
 * @description 对应后端 TaktPurchaseOrderItemExportDto
 */
export interface PurchaseOrderItemExport {
  /**
   * PurchaseOrderItemID
   */
  purchaseOrderItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购订单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchaseOrderId: string;

  /**
   * 采购订单编码（冗余字段，便于查询）
   */
  purchaseOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源请购编码
   */
  requestCode?: string;

  /**
   * 来源请购行号
   */
  requestLineNumber?: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 采购单位
   */
  purchaseUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 单价（精确到分，存储为整数，单位为分）
   */
  unitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 税费率（0-100，表示税费百分比）
   */
  taxRate: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount: number;

  /**
   * 小计金额（精确到分，存储为整数，单位为分）
   */
  subtotalAmount: number;

  /**
   * 行交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

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

