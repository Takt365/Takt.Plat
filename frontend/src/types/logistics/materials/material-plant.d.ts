// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material.d.ts
// 创建时间：2026-06-09
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
 * Takt物料实体
 * 对应前端 TaktMaterialDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Material
 * @description 对应后端 TaktMaterialDto
 */
export interface Material extends CompanyDtoBase {
  /**
   * MaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 物料编码（唯一索引）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 品目阶层
   */
  materialHierarchy?: string;

  /**
   * 品目组代码
   */
  materialGroupCode?: string;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType: number;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（主单位）
   */
  baseUnit: string;

  /**
   * 采购组
   */
  purchaseGroup?: string;

  /**
   * 采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）
   */
  purchaseType: number;

  /**
   * 特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）
   */
  specialProcurement: number;

  /**
   * 是否散装（0=否，1=是）
   */
  isBulk: number;

  /**
   * 最小起订量（基本单位数量）
   */
  minOrderQuantity: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入）
   */
  roundingValue: number;

  /**
   * 计划交货时间（天数）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 自制生产天数（内部生产所需天数）
   */
  inHouseProductionDays: number;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商零件编号
   */
  manufacturerPartNumber?: string;

  /**
   * 币种代码
   */
  currencyCode: string;

  /**
   * 价格控制（0=标准价格，1=移动平均价格，2=其他）
   */
  priceControl: number;

  /**
   * 价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）
   */
  priceUnit: number;

  /**
   * 评估类别代码
   */
  valuationCategory?: string;

  /**
   * 差异码
   */
  differenceCode?: string;

  /**
   * 利润中心
   */
  profitCenter?: string;

  /**
   * 最新采购价（精确到分，存储为整数，单位为分）
   */
  latestPurchasePrice: number;

  /**
   * 销售价格（精确到分，存储为整数，单位为分）
   */
  salesPrice: number;

  /**
   * 安全库存（基本单位数量）
   */
  safetyStock: number;

  /**
   * 最大库存（基本单位数量）
   */
  maxStock: number;

  /**
   * 最小库存（基本单位数量）
   */
  minStock: number;

  /**
   * 当前库存（基本单位数量）
   */
  currentStock: number;

  /**
   * 生产地点
   */
  productionLocation?: string;

  /**
   * 采购地点
   */
  purchasingLocation?: string;

  /**
   * 是否检验（0=否，1=是）
   */
  inspectionRequired: number;

  /**
   * 是否批次管理（0=否，1=是）
   */
  isBatch: number;

  /**
   * 是否保质期管理（0=否，1=是）
   */
  isExpiry: number;

  /**
   * 保质期天数（如果启用保质期管理）
   */
  expiryDays: number;

  /**
   * 物料状态（1=启用，0=禁用）
   */
  materialStatus: number;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）
   */
  isEndOfLife?: string;

  /**
   * 停产日期
   */
  endOfLifeDate?: string;

}


/**
 * Material 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialQuery
 * @description 对应后端 TaktMaterialQueryDto
 */
export interface MaterialQuery extends TaktPagedQuery {
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
   * 物料编码（唯一索引）
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
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 品目阶层
   */
  materialHierarchy?: string;

  /**
   * 品目组代码
   */
  materialGroupCode?: string;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType?: number;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（主单位）
   */
  baseUnit?: string;

  /**
   * 采购组
   */
  purchaseGroup?: string;

  /**
   * 采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）
   */
  purchaseType?: number;

  /**
   * 特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）
   */
  specialProcurement?: number;

  /**
   * 是否散装（0=否，1=是）
   */
  isBulk?: number;

  /**
   * 最小起订量（基本单位数量）
   */
  minOrderQuantity?: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入）
   */
  roundingValue?: number;

  /**
   * 计划交货时间（天数）
   */
  plannedDeliveryTimeDays?: number;

  /**
   * 自制生产天数（内部生产所需天数）
   */
  inHouseProductionDays?: number;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商零件编号
   */
  manufacturerPartNumber?: string;

  /**
   * 币种代码
   */
  currencyCode?: string;

  /**
   * 价格控制（0=标准价格，1=移动平均价格，2=其他）
   */
  priceControl?: number;

  /**
   * 价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）
   */
  priceUnit?: number;

  /**
   * 评估类别代码
   */
  valuationCategory?: string;

  /**
   * 差异码
   */
  differenceCode?: string;

  /**
   * 利润中心
   */
  profitCenter?: string;

  /**
   * 最新采购价（精确到分，存储为整数，单位为分）
   */
  latestPurchasePrice?: number;

  /**
   * 销售价格（精确到分，存储为整数，单位为分）
   */
  salesPrice?: number;

  /**
   * 安全库存（基本单位数量）
   */
  safetyStock?: number;

  /**
   * 最大库存（基本单位数量）
   */
  maxStock?: number;

  /**
   * 最小库存（基本单位数量）
   */
  minStock?: number;

  /**
   * 当前库存（基本单位数量）
   */
  currentStock?: number;

  /**
   * 生产地点
   */
  productionLocation?: string;

  /**
   * 采购地点
   */
  purchasingLocation?: string;

  /**
   * 是否检验（0=否，1=是）
   */
  inspectionRequired?: number;

  /**
   * 是否批次管理（0=否，1=是）
   */
  isBatch?: number;

  /**
   * 是否保质期管理（0=否，1=是）
   */
  isExpiry?: number;

  /**
   * 保质期天数（如果启用保质期管理）
   */
  expiryDays?: number;

  /**
   * 物料状态（1=启用，0=禁用）
   */
  materialStatus?: number;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）
   */
  isEndOfLife?: string;

  /**
   * 停产日期（范围查询-开始）
   */
  endOfLifeDateStart?: string;

  /**
   * 停产日期（范围查询-结束）
   */
  endOfLifeDateEnd?: string;

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
 * 创建Material DTO
 * 对应前端 MaterialCreate
 * @description 对应后端 TaktMaterialCreateDto
 */
export interface MaterialCreate {
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
  plantCode: string;

  /**
   * 物料编码（唯一索引）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 品目阶层
   */
  materialHierarchy?: string;

  /**
   * 品目组代码
   */
  materialGroupCode?: string;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType: number;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（主单位）
   */
  baseUnit: string;

  /**
   * 采购组
   */
  purchaseGroup?: string;

  /**
   * 采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）
   */
  purchaseType: number;

  /**
   * 特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）
   */
  specialProcurement: number;

  /**
   * 是否散装（0=否，1=是）
   */
  isBulk: number;

  /**
   * 最小起订量（基本单位数量）
   */
  minOrderQuantity: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入）
   */
  roundingValue: number;

  /**
   * 计划交货时间（天数）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 自制生产天数（内部生产所需天数）
   */
  inHouseProductionDays: number;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商零件编号
   */
  manufacturerPartNumber?: string;

  /**
   * 币种代码
   */
  currencyCode: string;

  /**
   * 价格控制（0=标准价格，1=移动平均价格，2=其他）
   */
  priceControl: number;

  /**
   * 价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）
   */
  priceUnit: number;

  /**
   * 评估类别代码
   */
  valuationCategory?: string;

  /**
   * 差异码
   */
  differenceCode?: string;

  /**
   * 利润中心
   */
  profitCenter?: string;

  /**
   * 最新采购价（精确到分，存储为整数，单位为分）
   */
  latestPurchasePrice: number;

  /**
   * 销售价格（精确到分，存储为整数，单位为分）
   */
  salesPrice: number;

  /**
   * 安全库存（基本单位数量）
   */
  safetyStock: number;

  /**
   * 最大库存（基本单位数量）
   */
  maxStock: number;

  /**
   * 最小库存（基本单位数量）
   */
  minStock: number;

  /**
   * 当前库存（基本单位数量）
   */
  currentStock: number;

  /**
   * 生产地点
   */
  productionLocation?: string;

  /**
   * 采购地点
   */
  purchasingLocation?: string;

  /**
   * 是否检验（0=否，1=是）
   */
  inspectionRequired: number;

  /**
   * 是否批次管理（0=否，1=是）
   */
  isBatch: number;

  /**
   * 是否保质期管理（0=否，1=是）
   */
  isExpiry: number;

  /**
   * 保质期天数（如果启用保质期管理）
   */
  expiryDays: number;

  /**
   * 物料状态（1=启用，0=禁用）
   */
  materialStatus: number;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）
   */
  isEndOfLife?: string;

  /**
   * 停产日期
   */
  endOfLifeDate?: string;

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
 * 更新Material DTO
 * 继承 TaktMaterialCreateDto，添加 MaterialId 字段
 * 对应前端 MaterialUpdate
 * @description 对应后端 TaktMaterialUpdateDto
 */
export interface MaterialUpdate extends MaterialCreate {
  /**
   * MaterialID（标识要更新的实体）
   */
  materialId: string;

}


/**
 * Material 状态更新 DTO
 * 对应前端 MaterialStatus
 * @description 对应后端 TaktMaterialStatusDto
 */
export interface MaterialStatus {
  /**
   * MaterialID
   */
  materialId: string;

  /**
   * 物料状态（1=启用，0=禁用）
   */
  materialStatus: number;

}


/**
 * Material 导入模板行 DTO
 * 对应前端 MaterialTemplate
 * @description 对应后端 TaktMaterialTemplateDto
 */
export interface MaterialTemplate {
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
   * 物料编码（唯一索引）
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
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 品目阶层
   */
  materialHierarchy?: string;

  /**
   * 品目组代码
   */
  materialGroupCode?: string;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType?: number;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（主单位）
   */
  baseUnit?: string;

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
 * Material 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialImport
 * @description 对应后端 TaktMaterialImportDto
 */
export interface MaterialImport {
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
   * 物料编码（唯一索引）
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
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 品目阶层
   */
  materialHierarchy?: string;

  /**
   * 品目组代码
   */
  materialGroupCode?: string;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType?: number;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（主单位）
   */
  baseUnit?: string;

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
 * Material 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialExport
 * @description 对应后端 TaktMaterialExportDto
 */
export interface MaterialExport {
  /**
   * MaterialID
   */
  materialId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 物料编码（唯一索引）
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 行业领域
   */
  industrySector?: string;

  /**
   * 品目阶层
   */
  materialHierarchy?: string;

  /**
   * 品目组代码
   */
  materialGroupCode?: string;

  /**
   * 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
   */
  materialType: number;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料品牌
   */
  materialBrand?: string;

  /**
   * 基本单位（主单位）
   */
  baseUnit: string;

  /**
   * 采购组
   */
  purchaseGroup?: string;

  /**
   * 采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）
   */
  purchaseType: number;

  /**
   * 特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）
   */
  specialProcurement: number;

  /**
   * 是否散装（0=否，1=是）
   */
  isBulk: number;

  /**
   * 最小起订量（基本单位数量）
   */
  minOrderQuantity: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入）
   */
  roundingValue: number;

  /**
   * 计划交货时间（天数）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 自制生产天数（内部生产所需天数）
   */
  inHouseProductionDays: number;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 制造商零件编号
   */
  manufacturerPartNumber?: string;

  /**
   * 币种代码
   */
  currencyCode: string;

  /**
   * 价格控制（0=标准价格，1=移动平均价格，2=其他）
   */
  priceControl: number;

  /**
   * 价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）
   */
  priceUnit: number;

  /**
   * 评估类别代码
   */
  valuationCategory?: string;

  /**
   * 差异码
   */
  differenceCode?: string;

  /**
   * 利润中心
   */
  profitCenter?: string;

  /**
   * 最新采购价（精确到分，存储为整数，单位为分）
   */
  latestPurchasePrice: number;

  /**
   * 销售价格（精确到分，存储为整数，单位为分）
   */
  salesPrice: number;

  /**
   * 安全库存（基本单位数量）
   */
  safetyStock: number;

  /**
   * 最大库存（基本单位数量）
   */
  maxStock: number;

  /**
   * 最小库存（基本单位数量）
   */
  minStock: number;

  /**
   * 当前库存（基本单位数量）
   */
  currentStock: number;

  /**
   * 生产地点
   */
  productionLocation?: string;

  /**
   * 采购地点
   */
  purchasingLocation?: string;

  /**
   * 是否检验（0=否，1=是）
   */
  inspectionRequired: number;

  /**
   * 是否批次管理（0=否，1=是）
   */
  isBatch: number;

  /**
   * 是否保质期管理（0=否，1=是）
   */
  isExpiry: number;

  /**
   * 保质期天数（如果启用保质期管理）
   */
  expiryDays: number;

  /**
   * 物料状态（1=启用，0=禁用）
   */
  materialStatus: number;

  /**
   * 物料属性（JSON格式，存储物料自定义属性）
   */
  materialAttributes?: string;

  /**
   * 停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）
   */
  isEndOfLife?: string;

  /**
   * 停产日期
   */
  endOfLifeDate?: string;

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

