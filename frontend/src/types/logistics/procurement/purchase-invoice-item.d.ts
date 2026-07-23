// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-invoice-item.d.ts
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
 * Takt采购发票明细实体
 * 对应前端 TaktPurchaseInvoiceItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInvoiceItem
 * @description 对应后端 TaktPurchaseInvoiceItemDto
 */
export interface PurchaseInvoiceItem extends CompanyDtoBase {
  /**
   * PurchaseInvoiceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInvoiceItemId: string;

  /**
   * 采购发票 ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId: string;

  /**
   * 采购发票 名称（填充字段）
   */
  purchaseInvoiceName?: string;

  /**
   * 采购发票编码（冗余字段，便于查询）
   */
  purchaseInvoiceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购订单行号
   */
  purchaseOrderLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity: number;

  /**
   * 开票单价
   */
  invoiceUnitPrice: number;

  /**
   * 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额
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
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseInvoiceItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInvoiceItemQuery
 * @description 对应后端 TaktPurchaseInvoiceItemQueryDto
 */
export interface PurchaseInvoiceItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 采购发票 ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId?: string;

  /**
   * 采购发票编码（冗余字段，便于查询）
   */
  purchaseInvoiceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购订单行号
   */
  purchaseOrderLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit?: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity?: number;

  /**
   * 开票单价
   */
  invoiceUnitPrice?: number;

  /**
   * 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额
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
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
 * 创建PurchaseInvoiceItem DTO
 * 对应前端 PurchaseInvoiceItemCreate
 * @description 对应后端 TaktPurchaseInvoiceItemCreateDto
 */
export interface PurchaseInvoiceItemCreate {
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
   * 采购发票 ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId: string;

  /**
   * 采购发票编码（冗余字段，便于查询）
   */
  purchaseInvoiceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购订单行号
   */
  purchaseOrderLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity: number;

  /**
   * 开票单价
   */
  invoiceUnitPrice: number;

  /**
   * 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额
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

}


/**
 * 更新PurchaseInvoiceItem DTO
 * 继承 TaktPurchaseInvoiceItemCreateDto，添加 PurchaseInvoiceItemId 字段
 * 对应前端 PurchaseInvoiceItemUpdate
 * @description 对应后端 TaktPurchaseInvoiceItemUpdateDto
 */
export interface PurchaseInvoiceItemUpdate extends PurchaseInvoiceItemCreate {
  /**
   * PurchaseInvoiceItemID（标识要更新的实体）
   */
  purchaseInvoiceItemId: string;

}


/**
 * PurchaseInvoiceItem 作废/撤销作废 DTO
 * 对应前端 PurchaseInvoiceItemObsolete
 * @description 对应后端 TaktPurchaseInvoiceItemObsoleteDto
 */
export interface PurchaseInvoiceItemObsolete {
  /**
   * PurchaseInvoiceItemID
   */
  purchaseInvoiceItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseInvoiceItem 导入模板行 DTO
 * 对应前端 PurchaseInvoiceItemTemplate
 * @description 对应后端 TaktPurchaseInvoiceItemTemplateDto
 */
export interface PurchaseInvoiceItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 采购发票 ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId?: string;

  /**
   * 采购发票编码（冗余字段，便于查询）
   */
  purchaseInvoiceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购订单行号
   */
  purchaseOrderLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit?: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity?: number;

  /**
   * 开票单价
   */
  invoiceUnitPrice?: number;

  /**
   * 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额
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
 * PurchaseInvoiceItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInvoiceItemImport
 * @description 对应后端 TaktPurchaseInvoiceItemImportDto
 */
export interface PurchaseInvoiceItemImport {
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
   * 采购发票 ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId?: string;

  /**
   * 采购发票编码（冗余字段，便于查询）
   */
  purchaseInvoiceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购订单行号
   */
  purchaseOrderLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit?: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity?: number;

  /**
   * 开票单价
   */
  invoiceUnitPrice?: number;

  /**
   * 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额
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
 * PurchaseInvoiceItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInvoiceItemExport
 * @description 对应后端 TaktPurchaseInvoiceItemExportDto
 */
export interface PurchaseInvoiceItemExport {
  /**
   * PurchaseInvoiceItemID
   */
  purchaseInvoiceItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购发票 ID（选项 TaktPurchaseInvoices/options；DictValue=Id）
   */
  purchaseInvoiceId: string;

  /**
   * 采购发票编码（冗余字段，便于查询）
   */
  purchaseInvoiceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源采购订单编码
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购订单行号
   */
  purchaseOrderLineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 采购单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit: string;

  /**
   * 开票数量（基本单位数量）
   */
  invoiceQuantity: number;

  /**
   * 开票单价
   */
  invoiceUnitPrice: number;

  /**
   * 折扣率（字典 logistics_discount_rate_param 预设或手输；0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额
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

