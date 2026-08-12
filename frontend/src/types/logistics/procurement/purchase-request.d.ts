// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-request.d.ts
// 创建时间：2026-07-23
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
 * Takt采购申请实体
 * 对应前端 TaktPurchaseRequestDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 PurchaseRequest
 * @description 对应后端 TaktPurchaseRequestDto
 */
export interface PurchaseRequest extends ApprovalDtoBase {


  /**
   * 采购申请编码（唯一索引）
   */
  purchaseRequestCode?: string;

  /**
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

  /**
   * 来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）
   */
  purchasePlanId?: string;

  /**
   * 来源采购计划编码（冗余）
   */
  purchasePlanCode?: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）
   */
  chainScheme?: number;

  /**
   * PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）
   */
  poDecision?: number;

  /**
   * PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignId?: string;

  /**
   * PR 会签编码（冗余）
   */
  countersignCode?: string;

  /**
   * 申请日期
   */
  requestDate?: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 申请人员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  requestId?: string;

  /**
   * 申请人（人员代码）
   */
  requestBy?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
  taxRate?: number;

  /**
   * 税费（精确到分）
   */
  taxAmount?: number;

  /**
   * 申请总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 申请总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount?: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转订单金额（精确到分，存储为整数，单位为分）
   */
  convertedAmount?: number;

  /**
   * 申请原因
   */
  requestReason?: string;

  /**
   * 区域文化编码（字典 sys_culture_code；用于匹配税码等区域字典）
   */
  cultureCode?: string;

  /**
   * 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  requestStatus?: number;

  /**
   * 转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 采购申请明细列表（主子表关系，一个申请可以有多个明细）（子表，级联保存）
   */
  items?: PurchaseRequestItemCreate[];

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
 * PurchaseRequest 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseRequestExport
 * @description 对应后端 TaktPurchaseRequestExportDto
 */
export interface PurchaseRequestExport {
  /**
   * PurchaseRequestID
   */
  purchaseRequestId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 采购申请编码（唯一索引）
   */
  purchaseRequestCode: string;

  /**
   * 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余）
   */
  purchaseInquiryCode?: string;

  /**
   * 来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）
   */
  purchasePlanId?: string;

  /**
   * 来源采购计划编码（冗余）
   */
  purchasePlanCode?: string;

  /**
   * 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）
   */
  chainScheme: number;

  /**
   * PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）
   */
  poDecision?: number;

  /**
   * PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignId?: string;

  /**
   * PR 会签编码（冗余）
   */
  countersignCode?: string;

  /**
   * 申请日期
   */
  requestDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 申请人员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  requestId?: string;

  /**
   * 申请人（人员代码）
   */
  requestBy: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
   */
  supplierCode: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1: string;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
  taxRate: number;

  /**
   * 税费（精确到分）
   */
  taxAmount: number;

  /**
   * 申请总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 申请总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转订单金额（精确到分，存储为整数，单位为分）
   */
  convertedAmount: number;

  /**
   * 申请原因
   */
  requestReason?: string;

  /**
   * 区域文化编码（字典 sys_culture_code；用于匹配税码等区域字典）
   */
  cultureCode: string;

  /**
   * 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  requestStatus: number;

  /**
   * 转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
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

