// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-request-item.d.ts
// 创建时间：2026-07-23
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
 * Takt采购申请明细实体
 * 对应前端 TaktPurchaseRequestItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseRequestItem
 * @description 对应后端 TaktPurchaseRequestItemDto
 */
export interface PurchaseRequestItem extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId?: string;

  /**
   * 采购申请编码（冗余字段，便于查询）
   */
  purchaseRequestCode?: string;

  /**
   * 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
   */
  purchasePlanItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

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
   * 申请单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  requestUnit?: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity?: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 请购单价
   */
  purchaseRequestUnitPrice?: number;

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
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
 * PurchaseRequestItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseRequestItemExport
 * @description 对应后端 TaktPurchaseRequestItemExportDto
 */
export interface PurchaseRequestItemExport {
  /**
   * PurchaseRequestItemID
   */
  purchaseRequestItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）
   */
  purchaseRequestId: string;

  /**
   * 采购申请编码（冗余字段，便于查询）
   */
  purchaseRequestCode: string;

  /**
   * 来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）
   */
  purchasePlanItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

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
   * 申请单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  requestUnit: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 请购单价
   */
  purchaseRequestUnitPrice: number;

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
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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

