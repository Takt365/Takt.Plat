// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/mrp
// 文件名称：purchase-plan-item.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mrp 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt采购计划明细实体
 * 对应前端 TaktPurchasePlanItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchasePlanItem
 * @description 对应后端 TaktPurchasePlanItemDto
 */
export interface PurchasePlanItem extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  purchasePlanId?: string;

  /**
   * 采购计划编码（冗余字段，便于查询）
   */
  purchasePlanCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 来源生产计划编码
   */
  productionPlanCode?: string;

  /**
   * 来源生产计划行号
   */
  productionPlanLineNumber?: number;

  /**
   * 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
   */
  materialRequirementsPlanningItemId?: string;

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
   * 计划单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  planUnit?: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity?: number;

  /**
   * 计划到货日期
   */
  plannedArrivalDate?: string;

  /**
   * 已转申请/订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

  /**
   * 含税价格（打印用；如 100.00）
   */
  taxIncludedPrice?: number;

  /**
   * 未税价格（打印用；如 88.50）
   */
  untaxedPrice?: number;

  /**
   * 税费（打印用；行税额，如 11.50）
   */
  taxAmount?: number;

  /**
   * 参考供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  referenceSupplierCode?: string;

  /**
   * 参考供货商名称
   */
  referenceSupplierName1?: string;

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
 * PurchasePlanItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchasePlanItemExport
 * @description 对应后端 TaktPurchasePlanItemExportDto
 */
export interface PurchasePlanItemExport {
  /**
   * PurchasePlanItemID
   */
  purchasePlanItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  purchasePlanId: string;

  /**
   * 采购计划编码（冗余字段，便于查询）
   */
  purchasePlanCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 来源生产计划编码
   */
  productionPlanCode?: string;

  /**
   * 来源生产计划行号
   */
  productionPlanLineNumber?: number;

  /**
   * 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
   */
  materialRequirementsPlanningItemId?: string;

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
   * 计划单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  planUnit: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity: number;

  /**
   * 计划到货日期
   */
  plannedArrivalDate?: string;

  /**
   * 已转申请/订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

  /**
   * 含税价格（打印用；如 100.00）
   */
  taxIncludedPrice: number;

  /**
   * 未税价格（打印用；如 88.50）
   */
  untaxedPrice: number;

  /**
   * 税费（打印用；行税额，如 11.50）
   */
  taxAmount: number;

  /**
   * 参考供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  referenceSupplierCode?: string;

  /**
   * 参考供货商名称
   */
  referenceSupplierName1?: string;

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

