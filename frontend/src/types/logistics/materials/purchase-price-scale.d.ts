// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：purchase-price-scale.d.ts
// 创建时间：2026-06-08
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
 * Takt采购价格阶梯实体
 * 对应前端 TaktPurchasePriceScaleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchasePriceScale
 * @description 对应后端 TaktPurchasePriceScaleDto
 */
export interface PurchasePriceScale extends CompanyDtoBase {
  /**
   * PurchasePriceScaleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchasePriceScaleId: string;

  /**
   * 采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceItemId: string;

  /**
   * 采购价格明细名称（填充字段）
   */
  purchasePriceItemName?: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 起始数量（基本单位数量，包含此数量）
   */
  startQuantity: number;

  /**
   * 结束数量（基本单位数量，包含此数量，0表示无上限）
   */
  endQuantity: number;

  /**
   * 阶梯价格（精确到分，存储为整数，单位为分）
   */
  scalePrice: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * PurchasePriceScale 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchasePriceScaleQuery
 * @description 对应后端 TaktPurchasePriceScaleQueryDto
 */
export interface PurchasePriceScaleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceItemId?: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 起始数量（基本单位数量，包含此数量）
   */
  startQuantity?: number;

  /**
   * 结束数量（基本单位数量，包含此数量，0表示无上限）
   */
  endQuantity?: number;

  /**
   * 阶梯价格（精确到分，存储为整数，单位为分）
   */
  scalePrice?: number;

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
 * 创建PurchasePriceScale DTO
 * 对应前端 PurchasePriceScaleCreate
 * @description 对应后端 TaktPurchasePriceScaleCreateDto
 */
export interface PurchasePriceScaleCreate {
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
   * 采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceItemId: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 起始数量（基本单位数量，包含此数量）
   */
  startQuantity: number;

  /**
   * 结束数量（基本单位数量，包含此数量，0表示无上限）
   */
  endQuantity: number;

  /**
   * 阶梯价格（精确到分，存储为整数，单位为分）
   */
  scalePrice: number;

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

}


/**
 * 更新PurchasePriceScale DTO
 * 继承 TaktPurchasePriceScaleCreateDto，添加 PurchasePriceScaleId 字段
 * 对应前端 PurchasePriceScaleUpdate
 * @description 对应后端 TaktPurchasePriceScaleUpdateDto
 */
export interface PurchasePriceScaleUpdate extends PurchasePriceScaleCreate {
  /**
   * PurchasePriceScaleID（标识要更新的实体）
   */
  purchasePriceScaleId: string;

}


/**
 * PurchasePriceScale 排序更新 DTO
 * 对应前端 PurchasePriceScaleSort
 * @description 对应后端 TaktPurchasePriceScaleSortDto
 */
export interface PurchasePriceScaleSort {
  /**
   * PurchasePriceScaleID
   */
  purchasePriceScaleId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * PurchasePriceScale 导入模板行 DTO
 * 对应前端 PurchasePriceScaleTemplate
 * @description 对应后端 TaktPurchasePriceScaleTemplateDto
 */
export interface PurchasePriceScaleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceItemId?: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

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
 * PurchasePriceScale 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchasePriceScaleImport
 * @description 对应后端 TaktPurchasePriceScaleImportDto
 */
export interface PurchasePriceScaleImport {
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
   * 采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceItemId?: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

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
 * PurchasePriceScale 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchasePriceScaleExport
 * @description 对应后端 TaktPurchasePriceScaleExportDto
 */
export interface PurchasePriceScaleExport {
  /**
   * PurchasePriceScaleID
   */
  purchasePriceScaleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购价格明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  purchasePriceItemId: string;

  /**
   * 采购价格编码（冗余字段，便于查询）
   */
  purchasePriceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 起始数量（基本单位数量，包含此数量）
   */
  startQuantity: number;

  /**
   * 结束数量（基本单位数量，包含此数量，0表示无上限）
   */
  endQuantity: number;

  /**
   * 阶梯价格（精确到分，存储为整数，单位为分）
   */
  scalePrice: number;

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

