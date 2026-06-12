// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：sales-order.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt销售订单实体
 * 对应前端 TaktSalesOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesOrder
 * @description 对应后端 TaktSalesOrderDto
 */
export interface SalesOrder extends CompanyDtoBase {
  /**
   * SalesOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesOrderId: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售订单编码（唯一索引）
   */
  salesOrderCode: string;

  /**
   * 客户编码
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求交货日期
   */
  requiredDeliveryDate?: string;

  /**
   * 实际交货日期
   */
  actualDeliveryDate?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 交货方式（0=自提，1=送货上门，2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 销售订单明细列表（主子表关系，一个订单可以有多个明细） （子表：TaktSalesOrderItem）
   */
  items?: SalesOrderItem[];

  /**
   * 销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.OrderId） （子表：TaktSalesOrderChangeLog）
   */
  changeLogs?: SalesOrderChangeLog[];

}


/**
 * SalesOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesOrderQuery
 * @description 对应后端 TaktSalesOrderQueryDto
 */
export interface SalesOrderQuery extends TaktPagedQuery {
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
   * 销售订单编码（唯一索引）
   */
  salesOrderCode?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 订单日期（范围查询-开始）
   */
  orderDateStart?: string;

  /**
   * 订单日期（范围查询-结束）
   */
  orderDateEnd?: string;

  /**
   * 要求交货日期（范围查询-开始）
   */
  requiredDeliveryDateStart?: string;

  /**
   * 要求交货日期（范围查询-结束）
   */
  requiredDeliveryDateEnd?: string;

  /**
   * 实际交货日期（范围查询-开始）
   */
  actualDeliveryDateStart?: string;

  /**
   * 实际交货日期（范围查询-结束）
   */
  actualDeliveryDateEnd?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity?: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount?: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount?: number;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 交货方式（0=自提，1=送货上门，2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

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
 * 创建SalesOrder DTO
 * 对应前端 SalesOrderCreate
 * @description 对应后端 TaktSalesOrderCreateDto
 */
export interface SalesOrderCreate {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售订单编码（唯一索引）
   */
  salesOrderCode: string;

  /**
   * 客户编码
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求交货日期
   */
  requiredDeliveryDate?: string;

  /**
   * 实际交货日期
   */
  actualDeliveryDate?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 交货方式（0=自提，1=送货上门，2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 销售订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: SalesOrderItemCreate[];

  /**
   * 销售订单变更记录列表（外键在子表 TaktSalesOrderChangeLog.OrderId）（子表，级联保存）
   */
  changeLogs?: SalesOrderChangeLogCreate[];

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
 * 更新SalesOrder DTO
 * 继承 TaktSalesOrderCreateDto，添加 SalesOrderId 字段
 * 对应前端 SalesOrderUpdate
 * @description 对应后端 TaktSalesOrderUpdateDto
 */
export interface SalesOrderUpdate extends SalesOrderCreate {
  /**
   * SalesOrderID（标识要更新的实体）
   */
  salesOrderId: string;

}


/**
 * SalesOrder 状态更新 DTO
 * 对应前端 SalesOrderStatus
 * @description 对应后端 TaktSalesOrderStatusDto
 */
export interface SalesOrderStatus {
  /**
   * SalesOrderID
   */
  salesOrderId: string;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus: number;

}


/**
 * SalesOrder 导入模板行 DTO
 * 对应前端 SalesOrderTemplate
 * @description 对应后端 TaktSalesOrderTemplateDto
 */
export interface SalesOrderTemplate {
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
   * 销售订单编码（唯一索引）
   */
  salesOrderCode?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 交货方式（0=自提，1=送货上门，2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

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
 * SalesOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesOrderImport
 * @description 对应后端 TaktSalesOrderImportDto
 */
export interface SalesOrderImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售订单编码（唯一索引）
   */
  salesOrderCode?: string;

  /**
   * 客户编码
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 交货方式（0=自提，1=送货上门，2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

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
 * SalesOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesOrderExport
 * @description 对应后端 TaktSalesOrderExportDto
 */
export interface SalesOrderExport {
  /**
   * SalesOrderID
   */
  salesOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 销售订单编码（唯一索引）
   */
  salesOrderCode: string;

  /**
   * 客户编码
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 要求交货日期
   */
  requiredDeliveryDate?: string;

  /**
   * 实际交货日期
   */
  actualDeliveryDate?: string;

  /**
   * 销售员（人员代码）
   */
  salesBy?: string;

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
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 已发货金额（精确到分，存储为整数，单位为分）
   */
  shippedAmount: number;

  /**
   * 已收款金额（精确到分，存储为整数，单位为分）
   */
  receivedAmount: number;

  /**
   * 订单状态（1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 交货方式（0=自提，1=送货上门，2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 收款方式（0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

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

