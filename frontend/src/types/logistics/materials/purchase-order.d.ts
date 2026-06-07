// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：purchase-order.d.ts
// 创建时间：2026-06-07
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
 * Takt采购订单实体
 * 对应前端 TaktPurchaseOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseOrder
 * @description 对应后端 TaktPurchaseOrderDto
 */
export interface PurchaseOrder extends CompanyDtoBase {
  /**
   * PurchaseOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseOrderId: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode: string;

  /**
   * 供应商编码
   */
  supplierCode: string;

  /**
   * 供应商名称
   */
  supplierName: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 实际到货日期
   */
  actualArrivalDate?: string;

  /**
   * 采购组代码
   */
  purchaseGroup?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount: number;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单明细列表（主子表关系，一个订单可以有多个明细） （子表：TaktPurchaseOrderItem）
   */
  items?: PurchaseOrderItem[];

  /**
   * 采购订单变更记录列表（外键在子表 <see cref="TaktPurchaseOrderChangeLog.OrderId"/>） （子表：TaktPurchaseOrderChangeLog）
   */
  changeLogs?: PurchaseOrderChangeLog[];

}


/**
 * PurchaseOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseOrderQuery
 * @description 对应后端 TaktPurchaseOrderQueryDto
 */
export interface PurchaseOrderQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode?: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 供应商名称
   */
  supplierName?: string;

  /**
   * 订单日期（范围查询-开始）
   */
  orderDateStart?: string;

  /**
   * 订单日期（范围查询-结束）
   */
  orderDateEnd?: string;

  /**
   * 要求到货日期（范围查询-开始）
   */
  requiredArrivalDateStart?: string;

  /**
   * 要求到货日期（范围查询-结束）
   */
  requiredArrivalDateEnd?: string;

  /**
   * 实际到货日期（范围查询-开始）
   */
  actualArrivalDateStart?: string;

  /**
   * 实际到货日期（范围查询-结束）
   */
  actualArrivalDateEnd?: string;

  /**
   * 采购组代码
   */
  purchaseGroup?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount?: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount?: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity?: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount?: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount?: number;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

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
 * 创建PurchaseOrder DTO
 * 对应前端 PurchaseOrderCreate
 * @description 对应后端 TaktPurchaseOrderCreateDto
 */
export interface PurchaseOrderCreate {
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
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode: string;

  /**
   * 供应商编码
   */
  supplierCode: string;

  /**
   * 供应商名称
   */
  supplierName: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 实际到货日期
   */
  actualArrivalDate?: string;

  /**
   * 采购组代码
   */
  purchaseGroup?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount: number;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: PurchaseOrderItemCreate[];

  /**
   * 采购订单变更记录列表（外键在子表 <see cref="TaktPurchaseOrderChangeLog.OrderId"/>）（子表，级联保存）
   */
  changeLogs?: PurchaseOrderChangeLogCreate[];

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
 * 更新PurchaseOrder DTO
 * 继承 TaktPurchaseOrderCreateDto，添加 PurchaseOrderId 字段
 * 对应前端 PurchaseOrderUpdate
 * @description 对应后端 TaktPurchaseOrderUpdateDto
 */
export interface PurchaseOrderUpdate extends PurchaseOrderCreate {
  /**
   * PurchaseOrderID（标识要更新的实体）
   */
  purchaseOrderId: string;

}


/**
 * PurchaseOrder 状态更新 DTO
 * 对应前端 PurchaseOrderStatus
 * @description 对应后端 TaktPurchaseOrderStatusDto
 */
export interface PurchaseOrderStatus {
  /**
   * PurchaseOrderID
   */
  purchaseOrderId: string;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus: number;

}


/**
 * PurchaseOrder 导入模板行 DTO
 * 对应前端 PurchaseOrderTemplate
 * @description 对应后端 TaktPurchaseOrderTemplateDto
 */
export interface PurchaseOrderTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode?: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 供应商名称
   */
  supplierName?: string;

  /**
   * 采购组代码
   */
  purchaseGroup?: string;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

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
 * PurchaseOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseOrderImport
 * @description 对应后端 TaktPurchaseOrderImportDto
 */
export interface PurchaseOrderImport {
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
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode?: string;

  /**
   * 供应商编码
   */
  supplierCode?: string;

  /**
   * 供应商名称
   */
  supplierName?: string;

  /**
   * 采购组代码
   */
  purchaseGroup?: string;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

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
 * PurchaseOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseOrderExport
 * @description 对应后端 TaktPurchaseOrderExportDto
 */
export interface PurchaseOrderExport {
  /**
   * PurchaseOrderID
   */
  purchaseOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode: string;

  /**
   * 供应商编码
   */
  supplierCode: string;

  /**
   * 供应商名称
   */
  supplierName: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 实际到货日期
   */
  actualArrivalDate?: string;

  /**
   * 采购组代码
   */
  purchaseGroup?: string;

  /**
   * 订单总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 订单总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 税费（精确到分，存储为整数，单位为分）
   */
  taxAmount: number;

  /**
   * 订单实付金额（精确到分，存储为整数，单位为分）
   */
  actualAmount: number;

  /**
   * 已入库数量（基本单位数量）
   */
  receivedQuantity: number;

  /**
   * 已入库金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 已付款金额（精确到分，存储为整数，单位为分）
   */
  paidAmount: number;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 支付方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货方式（0=自提，1=供应商送货，2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

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

