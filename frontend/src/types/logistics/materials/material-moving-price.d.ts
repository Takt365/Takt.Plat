// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material-moving-price.d.ts
// 创建时间：2026-07-31
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
 * 移动价格实体 唯一键：租户 + 公司 + 工厂 + 评估期间 + 物料 + 评估类别（评估期间存 yyyy-MM）
 * 对应前端 TaktMaterialMovingPriceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialMovingPrice
 * @description 对应后端 TaktMaterialMovingPriceDto
 */
export interface MaterialMovingPrice extends CompanyDtoBase {

  /**
   * 评估期间（yyyy-MM；与工厂+物料+评估类别构成唯一键）
   */
  valuationPeriod?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation?: string;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity?: number;

  /**
   * 库存金额（与币种一致，2 位小数）
   */
  stockAmount?: number;

  /**
   * 价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl?: string;

  /**
   * 移动价格（decimal，5 位小数；相对价格单位）
   */
  movingPrice?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 币种（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

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
 * MaterialMovingPrice 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialMovingPriceExport
 * @description 对应后端 TaktMaterialMovingPriceExportDto
 */
export interface MaterialMovingPriceExport {
  /**
   * MaterialMovingPriceID
   */
  materialMovingPriceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 评估期间（yyyy-MM；与工厂+物料+评估类别构成唯一键）
   */
  valuationPeriod: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation: string;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity: number;

  /**
   * 库存金额（与币种一致，2 位小数）
   */
  stockAmount: number;

  /**
   * 价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl: string;

  /**
   * 移动价格（decimal，5 位小数；相对价格单位）
   */
  movingPrice: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 币种（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

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

