// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/planning
// 文件名称：purchase-plan-item.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/planning 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
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
   * PurchasePlanItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchasePlanItemId: string;

  /**
   * 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  purchasePlanId: string;

  /**
   * 采购计划名称（填充字段）
   */
  purchasePlanName?: string;

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
   * 来源生产计划名称（填充字段）
   */
  productionPlanName?: string;

  /**
   * 来源生产计划编码
   */
  productionPlanCode?: string;

  /**
   * 来源生产计划行号
   */
  productionPlanLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
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
   * 参考供货商编码（关联 TaktSupplier.SupplierCode，选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  referenceSupplierCode?: string;

  /**
   * 参考供货商名称
   */
  referenceSupplierName?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchasePlanItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchasePlanItemQuery
 * @description 对应后端 TaktPurchasePlanItemQueryDto
 */
export interface PurchasePlanItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

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
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  planUnit?: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity?: number;

  /**
   * 计划到货日期（范围查询-开始）
   */
  plannedArrivalDateStart?: string;

  /**
   * 计划到货日期（范围查询-结束）
   */
  plannedArrivalDateEnd?: string;

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
   * 参考供货商编码（关联 TaktSupplier.SupplierCode，选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  referenceSupplierCode?: string;

  /**
   * 参考供货商名称
   */
  referenceSupplierName?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建PurchasePlanItem DTO
 * 对应前端 PurchasePlanItemCreate
 * @description 对应后端 TaktPurchasePlanItemCreateDto
 */
export interface PurchasePlanItemCreate {
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
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
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
   * 参考供货商编码（关联 TaktSupplier.SupplierCode，选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  referenceSupplierCode?: string;

  /**
   * 参考供货商名称
   */
  referenceSupplierName?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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

}


/**
 * 更新PurchasePlanItem DTO
 * 继承 TaktPurchasePlanItemCreateDto，添加 PurchasePlanItemId 字段
 * 对应前端 PurchasePlanItemUpdate
 * @description 对应后端 TaktPurchasePlanItemUpdateDto
 */
export interface PurchasePlanItemUpdate extends PurchasePlanItemCreate {
  /**
   * PurchasePlanItemID（标识要更新的实体）
   */
  purchasePlanItemId: string;

}


/**
 * PurchasePlanItem 作废/撤销作废 DTO
 * 对应前端 PurchasePlanItemObsolete
 * @description 对应后端 TaktPurchasePlanItemObsoleteDto
 */
export interface PurchasePlanItemObsolete {
  /**
   * PurchasePlanItemID
   */
  purchasePlanItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchasePlanItem 导入模板行 DTO
 * 对应前端 PurchasePlanItemTemplate
 * @description 对应后端 TaktPurchasePlanItemTemplateDto
 */
export interface PurchasePlanItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

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
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
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
   * 参考供货商编码（关联 TaktSupplier.SupplierCode，选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  referenceSupplierCode?: string;

  /**
   * 参考供货商名称
   */
  referenceSupplierName?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
 * PurchasePlanItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchasePlanItemImport
 * @description 对应后端 TaktPurchasePlanItemImportDto
 */
export interface PurchasePlanItemImport {
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
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
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
   * 参考供货商编码（关联 TaktSupplier.SupplierCode，选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  referenceSupplierCode?: string;

  /**
   * 参考供货商名称
   */
  referenceSupplierName?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
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
   * 参考供货商编码（关联 TaktSupplier.SupplierCode，选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  referenceSupplierCode?: string;

  /**
   * 参考供货商名称
   */
  referenceSupplierName?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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

