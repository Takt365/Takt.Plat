// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-order.d.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
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
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 来源采购申请编码（冗余）
   */
  purchaseRequestCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 订单日期
   */
  orderDate?: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 实际到货日期
   */
  actualArrivalDate?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
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
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_tax_code.ExtValue 回填，如 J2→13）
   */
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
   * 支付方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货方式（字典 logistics_delivery_method_type；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
   */
  deliveryMethod?: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  orderStatus?: number;

  /**
   * 交货状态（字典 logistics_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 订单明细列表（主子表关系，一个订单可以有多个明细）（子表，级联保存）
   */
  items?: PurchaseOrderItemCreate[];

  /**
   * 区域文化编码（字典 sys_culture_code；租户→公司→工厂固定映射）
   */
  cultureCode: string;

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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 采购订单编码（唯一索引）
   */
  purchaseOrderCode: string;

  /**
   * 来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 来源采购申请编码（冗余）
   */
  purchaseRequestCode?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

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
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
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
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 汇率
   */
  exchangeRate: number;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；一单一税率；由税码 TaxCode / 字典 accounting_tax_code.ExtValue 回填，如 J2→13）
   */
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
   * 支付方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货方式（字典 logistics_delivery_method_type；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）
   */
  deliveryMethod: number;

  /**
   * 交货地址
   */
  deliveryAddress?: string;

  /**
   * 订单状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  orderStatus: number;

  /**
   * 交货状态（字典 logistics_delivery_status；0=未交货，1=部分交货，2=全部交货）
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

