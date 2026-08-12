// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：quotation-item.d.ts
// 创建时间：2026-07-23
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
 * Takt销售报价明细实体
 * 对应前端 TaktSalesQuotationItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesQuotationItem
 * @description 对应后端 TaktSalesQuotationItemDto
 */
export interface SalesQuotationItem extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 销售报价（选项 TaktSalesQuotations/options；DictValue=Id）
   */
  salesQuotationId?: string;

  /**
   * 销售报价编码（冗余字段，便于查询）
   */
  salesQuotationCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

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
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit?: string;

  /**
   * 报价数量（基本单位数量）
   */
  quotationQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit?: number;

  /**
   * 报价单价
   */
  quotationUnitPrice?: number;

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
 * SalesQuotationItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesQuotationItemExport
 * @description 对应后端 TaktSalesQuotationItemExportDto
 */
export interface SalesQuotationItemExport {
  /**
   * SalesQuotationItemID
   */
  salesQuotationItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售报价（选项 TaktSalesQuotations/options；DictValue=Id）
   */
  salesQuotationId: string;

  /**
   * 销售报价编码（冗余字段，便于查询）
   */
  salesQuotationCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

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
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit: string;

  /**
   * 报价数量（基本单位数量）
   */
  quotationQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit: number;

  /**
   * 报价单价
   */
  quotationUnitPrice: number;

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

