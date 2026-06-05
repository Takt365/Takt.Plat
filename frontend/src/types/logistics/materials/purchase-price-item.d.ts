// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：purchase-price-item.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt采购价格明细实体（供应商物料价格明细表）
 * 对应前端 TaktPurchasePriceItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchasePriceItem
 * @description 对应后端 TaktPurchasePriceItemDto
 */
export interface PurchasePriceItem extends CompanyDtoBase {
  /**
   * PurchasePriceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchasePriceItemId: string;

  /**
   * 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceId: string;

  /**
   * 采购价格名称（填充字段）
   */
  purchasePriceName?: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 采购单位
   */
  purchaseUnit: string;

  /**
   * 采购价格（精确到分，存储为整数，单位为分）
   */
  purchasePrice: number;

  /**
   * 最小采购量（基本单位数量）
   */
  minPurchaseQuantity: number;

  /**
   * 最大采购量（基本单位数量，0表示无限制）
   */
  maxPurchaseQuantity: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯） （子表：TaktPurchasePriceScale）
   */
  scales?: PurchasePriceScale[];

}


/**
 * PurchasePriceItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchasePriceItemQuery
 * @description 对应后端 TaktPurchasePriceItemQueryDto
 */
export interface PurchasePriceItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceId?: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
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
   * 采购单位
   */
  purchaseUnit?: string;

  /**
   * 采购价格（精确到分，存储为整数，单位为分）
   */
  purchasePrice?: number;

  /**
   * 最小采购量（基本单位数量）
   */
  minPurchaseQuantity?: number;

  /**
   * 最大采购量（基本单位数量，0表示无限制）
   */
  maxPurchaseQuantity?: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建PurchasePriceItem DTO
 * 对应前端 PurchasePriceItemCreate
 * @description 对应后端 TaktPurchasePriceItemCreateDto
 */
export interface PurchasePriceItemCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceId: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 采购单位
   */
  purchaseUnit: string;

  /**
   * 采购价格（精确到分，存储为整数，单位为分）
   */
  purchasePrice: number;

  /**
   * 最小采购量（基本单位数量）
   */
  minPurchaseQuantity: number;

  /**
   * 最大采购量（基本单位数量，0表示无限制）
   */
  maxPurchaseQuantity: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）（子表，级联保存）
   */
  scales?: PurchasePriceScaleCreate[];

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新PurchasePriceItem DTO
 * 继承 TaktPurchasePriceItemCreateDto，添加 PurchasePriceItemId 字段
 * 对应前端 PurchasePriceItemUpdate
 * @description 对应后端 TaktPurchasePriceItemUpdateDto
 */
export interface PurchasePriceItemUpdate extends PurchasePriceItemCreate {
  /**
   * PurchasePriceItemID（标识要更新的实体）
   */
  purchasePriceItemId: string;

}


/**
 * PurchasePriceItem 排序更新 DTO
 * 对应前端 PurchasePriceItemSort
 * @description 对应后端 TaktPurchasePriceItemSortDto
 */
export interface PurchasePriceItemSort {
  /**
   * PurchasePriceItemID
   */
  purchasePriceItemId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * PurchasePriceItem 导入模板行 DTO
 * 对应前端 PurchasePriceItemTemplate
 * @description 对应后端 TaktPurchasePriceItemTemplateDto
 */
export interface PurchasePriceItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceId?: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
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
   * 采购单位
   */
  purchaseUnit?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PurchasePriceItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchasePriceItemImport
 * @description 对应后端 TaktPurchasePriceItemImportDto
 */
export interface PurchasePriceItemImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceId?: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
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
   * 采购单位
   */
  purchaseUnit?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PurchasePriceItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchasePriceItemExport
 * @description 对应后端 TaktPurchasePriceItemExportDto
 */
export interface PurchasePriceItemExport {
  /**
   * PurchasePriceItemID
   */
  purchasePriceItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceId: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 采购单位
   */
  purchaseUnit: string;

  /**
   * 采购价格（精确到分，存储为整数，单位为分）
   */
  purchasePrice: number;

  /**
   * 最小采购量（基本单位数量）
   */
  minPurchaseQuantity: number;

  /**
   * 最大采购量（基本单位数量，0表示无限制）
   */
  maxPurchaseQuantity: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

