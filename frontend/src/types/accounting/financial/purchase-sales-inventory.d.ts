// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：purchase-sales-inventory.d.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 进销存表实体（存货数量金额账；CAS《存货》成本流转 / IAS 2 inventory movement） 勾稽：期末数量/成本 = 期初 + 采购入库 + 生产入库 + 其他入库调整 − 出库成本结转； 销售收入单独列示，不计入存货成本等式（避免收入与成本混淆）。 唯一键：租户 + 公司 + 工厂 + 期间 + 物料 + 评估类别
 * 对应前端 TaktPurchaseSalesInventoryDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseSalesInventory
 * @description 对应后端 TaktPurchaseSalesInventoryDto
 */
export interface PurchaseSalesInventory extends CompanyDtoBase {
  /**
   * PurchaseSalesInventoryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseSalesInventoryId: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode: string;

  /**
   * 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料名称（冗余）
   */
  materialName: string;

  /**
   * 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation: string;

  /**
   * 计量单位
   */
  unitCode?: string;

  /**
   * 期初数量
   */
  openingQty: number;

  /**
   * 期初存货成本金额（IAS 2 / CAS 成本口径）
   */
  openingAmount: number;

  /**
   * 本期采购入库数量
   */
  purchaseQty: number;

  /**
   * 本期采购入库成本金额
   */
  purchaseAmount: number;

  /**
   * 本期生产入库数量（自制半成品/产成品）
   */
  productionQty: number;

  /**
   * 本期生产入库成本金额
   */
  productionAmount: number;

  /**
   * 本期出库数量（销售/领用等发出）
   */
  issueQty: number;

  /**
   * 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
   */
  issueCostAmount: number;

  /**
   * 本期销售收入金额（利润表口径；不参与存货成本等式）
   */
  salesRevenueAmount: number;

  /**
   * 本期调整数量（盘盈盘亏、报废等，可正可负）
   */
  adjustQty: number;

  /**
   * 本期调整成本金额
   */
  adjustAmount: number;

  /**
   * 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
   */
  closingQty: number;

  /**
   * 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
   */
  closingAmount: number;

  /**
   * 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
   */
  closingUnitCost: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  psiStatus: number;

}


/**
 * PurchaseSalesInventory 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseSalesInventoryQuery
 * @description 对应后端 TaktPurchaseSalesInventoryQueryDto
 */
export interface PurchaseSalesInventoryQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode?: string;

  /**
   * 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料名称（冗余）
   */
  materialName?: string;

  /**
   * 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation?: string;

  /**
   * 计量单位
   */
  unitCode?: string;

  /**
   * 期初数量
   */
  openingQty?: number;

  /**
   * 期初存货成本金额（IAS 2 / CAS 成本口径）
   */
  openingAmount?: number;

  /**
   * 本期采购入库数量
   */
  purchaseQty?: number;

  /**
   * 本期采购入库成本金额
   */
  purchaseAmount?: number;

  /**
   * 本期生产入库数量（自制半成品/产成品）
   */
  productionQty?: number;

  /**
   * 本期生产入库成本金额
   */
  productionAmount?: number;

  /**
   * 本期出库数量（销售/领用等发出）
   */
  issueQty?: number;

  /**
   * 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
   */
  issueCostAmount?: number;

  /**
   * 本期销售收入金额（利润表口径；不参与存货成本等式）
   */
  salesRevenueAmount?: number;

  /**
   * 本期调整数量（盘盈盘亏、报废等，可正可负）
   */
  adjustQty?: number;

  /**
   * 本期调整成本金额
   */
  adjustAmount?: number;

  /**
   * 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
   */
  closingQty?: number;

  /**
   * 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
   */
  closingAmount?: number;

  /**
   * 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
   */
  closingUnitCost?: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  psiStatus?: number;

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
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建PurchaseSalesInventory DTO
 * 对应前端 PurchaseSalesInventoryCreate
 * @description 对应后端 TaktPurchaseSalesInventoryCreateDto
 */
export interface PurchaseSalesInventoryCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode: string;

  /**
   * 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料名称（冗余）
   */
  materialName: string;

  /**
   * 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation: string;

  /**
   * 计量单位
   */
  unitCode?: string;

  /**
   * 期初数量
   */
  openingQty: number;

  /**
   * 期初存货成本金额（IAS 2 / CAS 成本口径）
   */
  openingAmount: number;

  /**
   * 本期采购入库数量
   */
  purchaseQty: number;

  /**
   * 本期采购入库成本金额
   */
  purchaseAmount: number;

  /**
   * 本期生产入库数量（自制半成品/产成品）
   */
  productionQty: number;

  /**
   * 本期生产入库成本金额
   */
  productionAmount: number;

  /**
   * 本期出库数量（销售/领用等发出）
   */
  issueQty: number;

  /**
   * 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
   */
  issueCostAmount: number;

  /**
   * 本期销售收入金额（利润表口径；不参与存货成本等式）
   */
  salesRevenueAmount: number;

  /**
   * 本期调整数量（盘盈盘亏、报废等，可正可负）
   */
  adjustQty: number;

  /**
   * 本期调整成本金额
   */
  adjustAmount: number;

  /**
   * 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
   */
  closingQty: number;

  /**
   * 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
   */
  closingAmount: number;

  /**
   * 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
   */
  closingUnitCost: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  psiStatus: number;

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
 * 更新PurchaseSalesInventory DTO
 * 继承 TaktPurchaseSalesInventoryCreateDto，添加 PurchaseSalesInventoryId 字段
 * 对应前端 PurchaseSalesInventoryUpdate
 * @description 对应后端 TaktPurchaseSalesInventoryUpdateDto
 */
export interface PurchaseSalesInventoryUpdate extends PurchaseSalesInventoryCreate {
  /**
   * PurchaseSalesInventoryID（标识要更新的实体）
   */
  purchaseSalesInventoryId: string;

}


/**
 * PurchaseSalesInventory 状态更新 DTO
 * 对应前端 PurchaseSalesInventoryStatus
 * @description 对应后端 TaktPurchaseSalesInventoryStatusDto
 */
export interface PurchaseSalesInventoryStatus {
  /**
   * PurchaseSalesInventoryID
   */
  purchaseSalesInventoryId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  psiStatus: number;

}


/**
 * PurchaseSalesInventory 排序更新 DTO
 * 对应前端 PurchaseSalesInventorySort
 * @description 对应后端 TaktPurchaseSalesInventorySortDto
 */
export interface PurchaseSalesInventorySort {
  /**
   * PurchaseSalesInventoryID
   */
  purchaseSalesInventoryId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * PurchaseSalesInventory 导入模板行 DTO
 * 对应前端 PurchaseSalesInventoryTemplate
 * @description 对应后端 TaktPurchaseSalesInventoryTemplateDto
 */
export interface PurchaseSalesInventoryTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode?: string;

  /**
   * 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料名称（冗余）
   */
  materialName?: string;

  /**
   * 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation?: string;

  /**
   * 计量单位
   */
  unitCode?: string;

  /**
   * 期初数量
   */
  openingQty?: number;

  /**
   * 期初存货成本金额（IAS 2 / CAS 成本口径）
   */
  openingAmount?: number;

  /**
   * 本期采购入库数量
   */
  purchaseQty?: number;

  /**
   * 本期采购入库成本金额
   */
  purchaseAmount?: number;

  /**
   * 本期生产入库数量（自制半成品/产成品）
   */
  productionQty?: number;

  /**
   * 本期生产入库成本金额
   */
  productionAmount?: number;

  /**
   * 本期出库数量（销售/领用等发出）
   */
  issueQty?: number;

  /**
   * 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
   */
  issueCostAmount?: number;

  /**
   * 本期销售收入金额（利润表口径；不参与存货成本等式）
   */
  salesRevenueAmount?: number;

  /**
   * 本期调整数量（盘盈盘亏、报废等，可正可负）
   */
  adjustQty?: number;

  /**
   * 本期调整成本金额
   */
  adjustAmount?: number;

  /**
   * 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
   */
  closingQty?: number;

  /**
   * 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
   */
  closingAmount?: number;

  /**
   * 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
   */
  closingUnitCost?: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  psiStatus?: number;

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
 * PurchaseSalesInventory 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseSalesInventoryImport
 * @description 对应后端 TaktPurchaseSalesInventoryImportDto
 */
export interface PurchaseSalesInventoryImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode?: string;

  /**
   * 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料名称（冗余）
   */
  materialName?: string;

  /**
   * 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation?: string;

  /**
   * 计量单位
   */
  unitCode?: string;

  /**
   * 期初数量
   */
  openingQty?: number;

  /**
   * 期初存货成本金额（IAS 2 / CAS 成本口径）
   */
  openingAmount?: number;

  /**
   * 本期采购入库数量
   */
  purchaseQty?: number;

  /**
   * 本期采购入库成本金额
   */
  purchaseAmount?: number;

  /**
   * 本期生产入库数量（自制半成品/产成品）
   */
  productionQty?: number;

  /**
   * 本期生产入库成本金额
   */
  productionAmount?: number;

  /**
   * 本期出库数量（销售/领用等发出）
   */
  issueQty?: number;

  /**
   * 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
   */
  issueCostAmount?: number;

  /**
   * 本期销售收入金额（利润表口径；不参与存货成本等式）
   */
  salesRevenueAmount?: number;

  /**
   * 本期调整数量（盘盈盘亏、报废等，可正可负）
   */
  adjustQty?: number;

  /**
   * 本期调整成本金额
   */
  adjustAmount?: number;

  /**
   * 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
   */
  closingQty?: number;

  /**
   * 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
   */
  closingAmount?: number;

  /**
   * 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
   */
  closingUnitCost?: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  psiStatus?: number;

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
 * PurchaseSalesInventory 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseSalesInventoryExport
 * @description 对应后端 TaktPurchaseSalesInventoryExportDto
 */
export interface PurchaseSalesInventoryExport {
  /**
   * PurchaseSalesInventoryID
   */
  purchaseSalesInventoryId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode: string;

  /**
   * 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料名称（冗余）
   */
  materialName: string;

  /**
   * 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation: string;

  /**
   * 计量单位
   */
  unitCode?: string;

  /**
   * 期初数量
   */
  openingQty: number;

  /**
   * 期初存货成本金额（IAS 2 / CAS 成本口径）
   */
  openingAmount: number;

  /**
   * 本期采购入库数量
   */
  purchaseQty: number;

  /**
   * 本期采购入库成本金额
   */
  purchaseAmount: number;

  /**
   * 本期生产入库数量（自制半成品/产成品）
   */
  productionQty: number;

  /**
   * 本期生产入库成本金额
   */
  productionAmount: number;

  /**
   * 本期出库数量（销售/领用等发出）
   */
  issueQty: number;

  /**
   * 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
   */
  issueCostAmount: number;

  /**
   * 本期销售收入金额（利润表口径；不参与存货成本等式）
   */
  salesRevenueAmount: number;

  /**
   * 本期调整数量（盘盈盘亏、报废等，可正可负）
   */
  adjustQty: number;

  /**
   * 本期调整成本金额
   */
  adjustAmount: number;

  /**
   * 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
   */
  closingQty: number;

  /**
   * 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
   */
  closingAmount: number;

  /**
   * 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
   */
  closingUnitCost: number;

  /**
   * 币种（字典 accounting_currency_code）
   */
  currencyCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  psiStatus: number;

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

