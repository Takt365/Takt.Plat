// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：price-scale.d.ts
// 创建时间：2026-06-09
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
 * Takt销售价格阶梯实体
 * 对应前端 TaktSalesPriceScaleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesPriceScale
 * @description 对应后端 TaktSalesPriceScaleDto
 */
export interface SalesPriceScale extends CompanyDtoBase {
  /**
   * SalesPriceScaleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPriceScaleId: string;

  /**
   * 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
   */
  itemId: string;

  /**
   * 价格明细名称（填充字段）
   */
  itemName?: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode: string;

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
   * 销售价格明细（主表） （主表：TaktSalesPriceItem）
   */
  priceItem?: SalesPriceItem;

}


/**
 * SalesPriceScale 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesPriceScaleQuery
 * @description 对应后端 TaktSalesPriceScaleQueryDto
 */
export interface SalesPriceScaleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
   */
  itemId?: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode?: string;

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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建SalesPriceScale DTO
 * 对应前端 SalesPriceScaleCreate
 * @description 对应后端 TaktSalesPriceScaleCreateDto
 */
export interface SalesPriceScaleCreate {
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
   * 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
   */
  itemId: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode: string;

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
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新SalesPriceScale DTO
 * 继承 TaktSalesPriceScaleCreateDto，添加 SalesPriceScaleId 字段
 * 对应前端 SalesPriceScaleUpdate
 * @description 对应后端 TaktSalesPriceScaleUpdateDto
 */
export interface SalesPriceScaleUpdate extends SalesPriceScaleCreate {
  /**
   * SalesPriceScaleID（标识要更新的实体）
   */
  salesPriceScaleId: string;

}


/**
 * SalesPriceScale 导入模板行 DTO
 * 对应前端 SalesPriceScaleTemplate
 * @description 对应后端 TaktSalesPriceScaleTemplateDto
 */
export interface SalesPriceScaleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
   */
  itemId?: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * SalesPriceScale 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesPriceScaleImport
 * @description 对应后端 TaktSalesPriceScaleImportDto
 */
export interface SalesPriceScaleImport {
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
   * 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
   */
  itemId?: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * SalesPriceScale 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesPriceScaleExport
 * @description 对应后端 TaktSalesPriceScaleExportDto
 */
export interface SalesPriceScaleExport {
  /**
   * SalesPriceScaleID
   */
  salesPriceScaleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 价格明细ID（关联销售价格明细表，序列化为string以避免Javascript精度问题）
   */
  itemId: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode: string;

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
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

