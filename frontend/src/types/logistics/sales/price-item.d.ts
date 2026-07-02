// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：price-item.d.ts
// 创建时间：2026-07-01
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
 * Takt销售价格明细实体（客户物料价格明细表）
 * 对应前端 TaktSalesPriceItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesPriceItem
 * @description 对应后端 TaktSalesPriceItemDto
 */
export interface SalesPriceItem extends CompanyDtoBase {
  /**
   * SalesPriceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPriceItemId: string;

  /**
   * 销售价格（关联 TaktSalesPrice.Id，选项 TaktSalesPrices/options）
   */
  salesPriceId: string;

  /**
   * 销售价格（关联 TaktSalesPrice.Id，选项 TaktSalesPrices/options）
   */
  salesPriceName?: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  salesUnit: string;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit: number;

  /**
   * 销售价格（decimal(18,5)）
   */
  salesPrice: number;

  /**
   * 最小订购量（基本单位数量，整数）
   */
  minOrderQuantity: number;

  /**
   * 最大订购量（基本单位数量，0表示无限制，整数）
   */
  maxOrderQuantity: number;

  /**
   * 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯） （子表：TaktSalesPriceScale）
   */
  scales?: SalesPriceScale[];

  /**
   * 销售价格（主表） （主表：TaktSalesPrice）
   */
  price?: SalesPrice;

}


/**
 * SalesPriceItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesPriceItemQuery
 * @description 对应后端 TaktSalesPriceItemQueryDto
 */
export interface SalesPriceItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 销售价格（关联 TaktSalesPrice.Id，选项 TaktSalesPrices/options）
   */
  salesPriceId?: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  salesUnit?: string;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit?: number;

  /**
   * 销售价格（decimal(18,5)）
   */
  salesPrice?: number;

  /**
   * 最小订购量（基本单位数量，整数）
   */
  minOrderQuantity?: number;

  /**
   * 最大订购量（基本单位数量，0表示无限制，整数）
   */
  maxOrderQuantity?: number;

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
 * 创建SalesPriceItem DTO
 * 对应前端 SalesPriceItemCreate
 * @description 对应后端 TaktSalesPriceItemCreateDto
 */
export interface SalesPriceItemCreate {
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
   * 销售价格（关联 TaktSalesPrice.Id，选项 TaktSalesPrices/options）
   */
  salesPriceId: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  salesUnit: string;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit: number;

  /**
   * 销售价格（decimal(18,5)）
   */
  salesPrice: number;

  /**
   * 最小订购量（基本单位数量，整数）
   */
  minOrderQuantity: number;

  /**
   * 最大订购量（基本单位数量，0表示无限制，整数）
   */
  maxOrderQuantity: number;

  /**
   * 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）（子表，级联保存）
   */
  scales?: SalesPriceScaleCreate[];

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
 * 更新SalesPriceItem DTO
 * 继承 TaktSalesPriceItemCreateDto，添加 SalesPriceItemId 字段
 * 对应前端 SalesPriceItemUpdate
 * @description 对应后端 TaktSalesPriceItemUpdateDto
 */
export interface SalesPriceItemUpdate extends SalesPriceItemCreate {
  /**
   * SalesPriceItemID（标识要更新的实体）
   */
  salesPriceItemId: string;

}


/**
 * SalesPriceItem 导入模板行 DTO
 * 对应前端 SalesPriceItemTemplate
 * @description 对应后端 TaktSalesPriceItemTemplateDto
 */
export interface SalesPriceItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 销售价格（关联 TaktSalesPrice.Id，选项 TaktSalesPrices/options）
   */
  salesPriceId?: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  salesUnit?: string;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit?: number;

  /**
   * 销售价格（decimal(18,5)）
   */
  salesPrice?: number;

  /**
   * 最小订购量（基本单位数量，整数）
   */
  minOrderQuantity?: number;

  /**
   * 最大订购量（基本单位数量，0表示无限制，整数）
   */
  maxOrderQuantity?: number;

  /**
   * 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）（子表，级联保存）
   */
  scales?: SalesPriceScaleCreate[];

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
 * SalesPriceItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesPriceItemImport
 * @description 对应后端 TaktSalesPriceItemImportDto
 */
export interface SalesPriceItemImport {
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
   * 销售价格（关联 TaktSalesPrice.Id，选项 TaktSalesPrices/options）
   */
  salesPriceId?: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  salesUnit?: string;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit?: number;

  /**
   * 销售价格（decimal(18,5)）
   */
  salesPrice?: number;

  /**
   * 最小订购量（基本单位数量，整数）
   */
  minOrderQuantity?: number;

  /**
   * 最大订购量（基本单位数量，0表示无限制，整数）
   */
  maxOrderQuantity?: number;

  /**
   * 价格阶梯列表（主子表关系，一个物料价格可以有多个阶梯）（子表，级联保存）
   */
  scales?: SalesPriceScaleCreate[];

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
 * SalesPriceItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesPriceItemExport
 * @description 对应后端 TaktSalesPriceItemExportDto
 */
export interface SalesPriceItemExport {
  /**
   * SalesPriceItemID
   */
  salesPriceItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售价格（关联 TaktSalesPrice.Id，选项 TaktSalesPrices/options）
   */
  salesPriceId: string;

  /**
   * 销售价格编码（冗余字段，便于查询）
   */
  salesPriceCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  salesUnit: string;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit: number;

  /**
   * 销售价格（decimal(18,5)）
   */
  salesPrice: number;

  /**
   * 最小订购量（基本单位数量，整数）
   */
  minOrderQuantity: number;

  /**
   * 最大订购量（基本单位数量，0表示无限制，整数）
   */
  maxOrderQuantity: number;

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

