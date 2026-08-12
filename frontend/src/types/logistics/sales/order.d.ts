// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：order.d.ts
// 创建时间：2026-08-11
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 销售订单编码（唯一索引）
   */
  salesOrderCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 订单日期
   */
  orderDate?: string;

  /**
   * 要求交货日期
   */
  requiredDeliveryDate?: string;

  /**
   * 实际交货日期
   */
  actualDeliveryDate?: string;

  /**
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
  taxRate?: number;

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
   * 交货方式（字典 logistics_delivery_method_type；0=自提 1=送货上门 2=物流配送 3=快递）
   */
  deliveryMethod?: number;

  /**
   * 收款方式（字典 accounting_payment_method_type；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 区域文化编码（字典 sys_culture_code；用于匹配税码等区域字典）
   */
  cultureCode?: string;

  /**
   * 订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 销售订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: SalesOrderItemCreate[];

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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 销售订单编码（唯一索引）
   */
  salesOrderCode: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

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
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 汇率
   */
  exchangeRate: number;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
  taxRate: number;

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
   * 交货方式（字典 logistics_delivery_method_type；0=自提 1=送货上门 2=物流配送 3=快递）
   */
  deliveryMethod: number;

  /**
   * 收款方式（字典 accounting_payment_method_type；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 区域文化编码（字典 sys_culture_code；用于匹配税码等区域字典）
   */
  cultureCode: string;

  /**
   * 订单状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus: number;

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

