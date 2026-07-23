// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：price-item.d.ts
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
 * Takt销售价格明细实体（定价记录条件行；主子表：TaktSalesPrice → Items → ScaleQuantities / ScaleValues）
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
   * 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
   */
  salesPriceId: string;

  /**
   * 销售价格 名称（填充字段）
   */
  salesPriceName?: string;

  /**
   * 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesPriceSeq: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType: string;

  /**
   * 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue: number;

  /**
   * 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；默认 A=百分数）
   */
  calculationType: string;

  /**
   * 价格
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrency: string;

  /**
   * 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

  /**
   * 最小起订量（计量单位数量，整数）
   */
  minOrderQuantity: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue: number;

  /**
   * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 数量等级行列表（；主子表关系） （子表：TaktSalesPriceScaleQuantity）
   */
  scaleQuantities?: SalesPriceScaleQuantity[];

  /**
   * 价值等级行列表（；主子表关系） （子表：TaktSalesPriceScaleValue）
   */
  scaleValues?: SalesPriceScaleValue[];

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
   * 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
   */
  salesPriceId?: string;

  /**
   * 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode?: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesPriceSeq?: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType?: string;

  /**
   * 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue?: number;

  /**
   * 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；默认 A=百分数）
   */
  calculationType?: string;

  /**
   * 价格
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrency?: string;

  /**
   * 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

  /**
   * 最小起订量（计量单位数量，整数）
   */
  minOrderQuantity?: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue?: number;

  /**
   * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays?: number;

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
   * 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
   */
  salesPriceId: string;

  /**
   * 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesPriceSeq: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType: string;

  /**
   * 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue: number;

  /**
   * 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；默认 A=百分数）
   */
  calculationType: string;

  /**
   * 价格
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrency: string;

  /**
   * 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

  /**
   * 最小起订量（计量单位数量，整数）
   */
  minOrderQuantity: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue: number;

  /**
   * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 数量等级行列表（；主子表关系）（子表，级联保存）
   */
  scaleQuantities?: SalesPriceScaleQuantityCreate[];

  /**
   * 价值等级行列表（；主子表关系）（子表，级联保存）
   */
  scaleValues?: SalesPriceScaleValueCreate[];

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

  /**
   * 数量等级行列表（；主子表关系）（子表，级联保存）
   */
  scaleQuantities?: any;

  /**
   * 价值等级行列表（；主子表关系）（子表，级联保存）
   */
  scaleValues?: any;

}


/**
 * SalesPriceItem 作废/撤销作废 DTO
 * 对应前端 SalesPriceItemObsolete
 * @description 对应后端 TaktSalesPriceItemObsoleteDto
 */
export interface SalesPriceItemObsolete {
  /**
   * SalesPriceItemID
   */
  salesPriceItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
   * 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
   */
  salesPriceId?: string;

  /**
   * 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode?: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesPriceSeq?: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType?: string;

  /**
   * 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue?: number;

  /**
   * 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；默认 A=百分数）
   */
  calculationType?: string;

  /**
   * 价格
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrency?: string;

  /**
   * 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

  /**
   * 最小起订量（计量单位数量，整数）
   */
  minOrderQuantity?: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue?: number;

  /**
   * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays?: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 数量等级行列表（；主子表关系）（子表，级联保存）
   */
  scaleQuantities?: SalesPriceScaleQuantityCreate[];

  /**
   * 价值等级行列表（；主子表关系）（子表，级联保存）
   */
  scaleValues?: SalesPriceScaleValueCreate[];

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
   * 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
   */
  salesPriceId?: string;

  /**
   * 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode?: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesPriceSeq?: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType?: string;

  /**
   * 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue?: number;

  /**
   * 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；默认 A=百分数）
   */
  calculationType?: string;

  /**
   * 价格
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrency?: string;

  /**
   * 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

  /**
   * 最小起订量（计量单位数量，整数）
   */
  minOrderQuantity?: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue?: number;

  /**
   * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays?: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 数量等级行列表（；主子表关系）（子表，级联保存）
   */
  scaleQuantities?: SalesPriceScaleQuantityCreate[];

  /**
   * 价值等级行列表（；主子表关系）（子表，级联保存）
   */
  scaleValues?: SalesPriceScaleValueCreate[];

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
   * 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
   */
  salesPriceId: string;

  /**
   * 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesPriceSeq: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType: string;

  /**
   * 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue: number;

  /**
   * 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；默认 A=百分数）
   */
  calculationType: string;

  /**
   * 价格
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrency: string;

  /**
   * 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

  /**
   * 最小起订量（计量单位数量，整数）
   */
  minOrderQuantity: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue: number;

  /**
   * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays: number;

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

