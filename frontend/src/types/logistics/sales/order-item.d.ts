// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：order-item.d.ts
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
 * Takt销售订单明细实体
 * 对应前端 TaktSalesOrderItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesOrderItem
 * @description 对应后端 TaktSalesOrderItemDto
 */
export interface SalesOrderItem extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
   */
  salesOrderId?: string;

  /**
   * 销售订单编码（冗余字段，便于查询）
   */
  salesOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit?: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity?: number;

  /**
   * 已发货数量（基本单位数量）
   */
  shippedQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit?: number;

  /**
   * 销售单价
   */
  salesUnitPrice?: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

  /**
   * 含税金额
   */
  taxIncludedAmount?: number;

  /**
   * 未税金额
   */
  untaxedAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 销售金额
   */
  salesAmount?: number;

  /**
   * 行交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * SalesOrderItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesOrderItemExport
 * @description 对应后端 TaktSalesOrderItemExportDto
 */
export interface SalesOrderItemExport {
  /**
   * SalesOrderItemID
   */
  salesOrderItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
   */
  salesOrderId: string;

  /**
   * 销售订单编码（冗余字段，便于查询）
   */
  salesOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit: number;

  /**
   * 销售单价
   */
  salesUnitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

  /**
   * 含税金额
   */
  taxIncludedAmount: number;

  /**
   * 未税金额
   */
  untaxedAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 销售金额
   */
  salesAmount: number;

  /**
   * 行交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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

