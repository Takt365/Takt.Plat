// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-price-item.d.ts
// 创建时间：2026-07-20
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
 * Takt采购价格明细实体（定价记录条件行；主子表：TaktPurchasePrice → Items → ScaleQuantities / ScaleValues）
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
   * 采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）
   */
  purchasePriceId: string;

  /**
   * 采购价格 名称（填充字段）
   */
  purchasePriceName?: string;

  /**
   * 定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  purchasePriceSeq: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType: string;

  /**
   * 等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue: number;

  /**
   * 等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）
   */
  calculationType: string;

  /**
   * 价格
   */
  price: number;

  /**
   * 税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）
   */
  taxCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 数量等级行列表（SAP KONM；主子表关系） （子表：TaktPurchasePriceScaleQuantity）
   */
  scaleQuantities?: PurchasePriceScaleQuantity[];

  /**
   * 价值等级行列表（SAP KONW；主子表关系） （子表：TaktPurchasePriceScaleValue）
   */
  scaleValues?: PurchasePriceScaleValue[];

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
   * 采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）
   */
  purchasePriceId?: string;

  /**
   * 定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode?: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  purchasePriceSeq?: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType?: string;

  /**
   * 等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue?: number;

  /**
   * 等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）
   */
  calculationType?: string;

  /**
   * 价格
   */
  price?: number;

  /**
   * 税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）
   */
  taxCode?: string;

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
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）
   */
  purchasePriceId: string;

  /**
   * 定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  purchasePriceSeq: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType: string;

  /**
   * 等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue: number;

  /**
   * 等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）
   */
  calculationType: string;

  /**
   * 价格
   */
  price: number;

  /**
   * 税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）
   */
  taxCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 数量等级行列表（SAP KONM；主子表关系）（子表，级联保存）
   */
  scaleQuantities?: PurchasePriceScaleQuantityCreate[];

  /**
   * 价值等级行列表（SAP KONW；主子表关系）（子表，级联保存）
   */
  scaleValues?: PurchasePriceScaleValueCreate[];

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

  /**
   * 数量等级行列表（SAP KONM；主子表关系）（子表，级联保存）
   */
  scaleQuantities?: any;

  /**
   * 价值等级行列表（SAP KONW；主子表关系）（子表，级联保存）
   */
  scaleValues?: any;

}


/**
 * PurchasePriceItem 作废/撤销作废 DTO
 * 对应前端 PurchasePriceItemObsolete
 * @description 对应后端 TaktPurchasePriceItemObsoleteDto
 */
export interface PurchasePriceItemObsolete {
  /**
   * PurchasePriceItemID
   */
  purchasePriceItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
   * 采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）
   */
  purchasePriceId?: string;

  /**
   * 定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode?: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  purchasePriceSeq?: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType?: string;

  /**
   * 等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue?: number;

  /**
   * 等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）
   */
  calculationType?: string;

  /**
   * 价格
   */
  price?: number;

  /**
   * 税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）
   */
  taxCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 数量等级行列表（SAP KONM；主子表关系）（子表，级联保存）
   */
  scaleQuantities?: PurchasePriceScaleQuantityCreate[];

  /**
   * 价值等级行列表（SAP KONW；主子表关系）（子表，级联保存）
   */
  scaleValues?: PurchasePriceScaleValueCreate[];

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
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）
   */
  purchasePriceId?: string;

  /**
   * 定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode?: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  purchasePriceSeq?: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType?: string;

  /**
   * 等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue?: number;

  /**
   * 等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）
   */
  calculationType?: string;

  /**
   * 价格
   */
  price?: number;

  /**
   * 税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）
   */
  taxCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 数量等级行列表（SAP KONM；主子表关系）（子表，级联保存）
   */
  scaleQuantities?: PurchasePriceScaleQuantityCreate[];

  /**
   * 价值等级行列表（SAP KONW；主子表关系）（子表，级联保存）
   */
  scaleValues?: PurchasePriceScaleValueCreate[];

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
   * 采购价格 ID（主子表关系；选项 TaktPurchasePrices/options，DictValue=Id）
   */
  purchasePriceId: string;

  /**
   * 定价记录号（冗余；与主表 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  purchasePriceSeq: number;

  /**
   * 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType: string;

  /**
   * 等级类型（字典 logistics_scale_type；SAP STFKZ；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_scale_basis；SAP KZBZG；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity: number;

  /**
   * 等级单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue: number;

  /**
   * 等级货币（字典 accounting_currency_code，DictValue=CNY/USD 等）
   */
  scaleCurrency?: string;

  /**
   * 计算类型（字典 logistics_calculation_type；SAP KRECH；默认 A=百分数）
   */
  calculationType: string;

  /**
   * 价格
   */
  price: number;

  /**
   * 税码（字典 accounting_tax_code，DictValue=J0/J1/J2…；SAP MWSKZ）
   */
  taxCode?: string;

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

