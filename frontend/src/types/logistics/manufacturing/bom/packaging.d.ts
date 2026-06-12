// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：packaging.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt物料包装信息实体
 * 对应前端 TaktPackagingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Packaging
 * @description 对应后端 TaktPackagingDto
 */
export interface Packaging extends CompanyDtoBase {
  /**
   * PackagingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  packagingId: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料编码（关联到物料表）
   */
  materialCode: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（用于关税和贸易统计）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（用于出口报关和贸易分析）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（如：KG、G、T等）
   */
  weightUnit: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（如：M3、L、ML等）
   */
  volumeUnit: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（如：箱、托盘、袋、桶等，VERP=销售包装）
   */
  packagingType: string;

  /**
   * 包装单位（CAR=卡通箱；其他如：个、件等）
   */
  packingUnit: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格
   */
  packagingSpec?: string;

  /**
   * 包装描述
   */
  packagingDescription?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Packaging 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PackagingQuery
 * @description 对应后端 TaktPackagingQueryDto
 */
export interface PackagingQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料编码（关联到物料表）
   */
  materialCode?: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（用于关税和贸易统计）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（用于出口报关和贸易分析）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（如：KG、G、T等）
   */
  weightUnit?: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（如：M3、L、ML等）
   */
  volumeUnit?: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（如：箱、托盘、袋、桶等，VERP=销售包装）
   */
  packagingType?: string;

  /**
   * 包装单位（CAR=卡通箱；其他如：个、件等）
   */
  packingUnit?: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格
   */
  packagingSpec?: string;

  /**
   * 包装描述
   */
  packagingDescription?: string;

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
 * 创建Packaging DTO
 * 对应前端 PackagingCreate
 * @description 对应后端 TaktPackagingCreateDto
 */
export interface PackagingCreate {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料编码（关联到物料表）
   */
  materialCode: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（用于关税和贸易统计）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（用于出口报关和贸易分析）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（如：KG、G、T等）
   */
  weightUnit: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（如：M3、L、ML等）
   */
  volumeUnit: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（如：箱、托盘、袋、桶等，VERP=销售包装）
   */
  packagingType: string;

  /**
   * 包装单位（CAR=卡通箱；其他如：个、件等）
   */
  packingUnit: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格
   */
  packagingSpec?: string;

  /**
   * 包装描述
   */
  packagingDescription?: string;

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
 * 更新Packaging DTO
 * 继承 TaktPackagingCreateDto，添加 PackagingId 字段
 * 对应前端 PackagingUpdate
 * @description 对应后端 TaktPackagingUpdateDto
 */
export interface PackagingUpdate extends PackagingCreate {
  /**
   * PackagingID（标识要更新的实体）
   */
  packagingId: string;

}


/**
 * Packaging 排序更新 DTO
 * 对应前端 PackagingSort
 * @description 对应后端 TaktPackagingSortDto
 */
export interface PackagingSort {
  /**
   * PackagingID
   */
  packagingId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Packaging 导入模板行 DTO
 * 对应前端 PackagingTemplate
 * @description 对应后端 TaktPackagingTemplateDto
 */
export interface PackagingTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料编码（关联到物料表）
   */
  materialCode?: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（用于关税和贸易统计）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（用于出口报关和贸易分析）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 重量单位（如：KG、G、T等）
   */
  weightUnit?: string;

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
 * Packaging 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PackagingImport
 * @description 对应后端 TaktPackagingImportDto
 */
export interface PackagingImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料编码（关联到物料表）
   */
  materialCode?: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（用于关税和贸易统计）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（用于出口报关和贸易分析）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 重量单位（如：KG、G、T等）
   */
  weightUnit?: string;

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
 * Packaging 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PackagingExport
 * @description 对应后端 TaktPackagingExportDto
 */
export interface PackagingExport {
  /**
   * PackagingID
   */
  packagingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料编码（关联到物料表）
   */
  materialCode: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（用于关税和贸易统计）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（用于出口报关和贸易分析）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（如：KG、G、T等）
   */
  weightUnit: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（如：M3、L、ML等）
   */
  volumeUnit: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（如：箱、托盘、袋、桶等，VERP=销售包装）
   */
  packagingType: string;

  /**
   * 包装单位（CAR=卡通箱；其他如：个、件等）
   */
  packingUnit: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格
   */
  packagingSpec?: string;

  /**
   * 包装描述
   */
  packagingDescription?: string;

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

