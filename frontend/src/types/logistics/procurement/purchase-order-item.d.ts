// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-order-item.d.ts
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
 * Takt采购订单明细实体
 * 对应前端 TaktPurchaseOrderItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseOrderItem
 * @description 对应后端 TaktPurchaseOrderItemDto
 */
export interface PurchaseOrderItem extends CompanyDtoBase {

  /**
   * 采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）
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
   * 采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
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
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 采购单价
   */
  purchaseUnitPrice?: number;

  /**
   * 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
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
   * 采购金额
   */
  purchaseAmount?: number;

  /**
   * 行交货状态（字典 logistics_delivery_status；0=未交货，1=部分交货，2=全部交货）
   */
  deliveryStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
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
   * 采购订单 ID（选项 TaktPurchaseOrders/options；DictValue=Id）
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
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
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
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 采购单价
   */
  purchaseUnitPrice: number;

  /**
   * 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
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
   * 采购金额
   */
  purchaseAmount: number;

  /**
   * 行交货状态（字典 logistics_delivery_status；0=未交货，1=部分交货，2=全部交货）
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

