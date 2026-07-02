// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：manufacturer.d.ts
// 创建时间：2026-06-30
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
 * Takt制造商实体
 * 对应前端 TaktManufacturerDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Manufacturer
 * @description 对应后端 TaktManufacturerDto
 */
export interface Manufacturer extends CompanyDtoBase {
  /**
   * ManufacturerID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  manufacturerId: string;

  /**
   * 制造商编码（唯一索引）
   */
  manufacturerCode: string;

  /**
   * 制造商名称
   */
  manufacturerName: string;

  /**
   * 制造商简称
   */
  manufacturerShortName?: string;

  /**
   * 制造商类型（字典 logistics_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）
   */
  manufacturerType: number;

  /**
   * 行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）
   */
  industrySector?: string;

  /**
   * 制造商标识（税务登记证号/统一社会信用代码）
   */
  manufacturerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 制造商电话
   */
  manufacturerPhone?: string;

  /**
   * 制造商传真
   */
  manufacturerFax?: string;

  /**
   * 制造商邮箱
   */
  manufacturerEmail?: string;

  /**
   * 制造商网站
   */
  manufacturerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 制造商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时）
   */
  manufacturerLevel: number;

  /**
   * 质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）
   */
  qualityCertification: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 制造商状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  manufacturerStatus: number;

  /**
   * 导航属性：制造商物料明细列表 （子表：TaktManufacturerMaterial）
   */
  manufacturerMaterials?: ManufacturerMaterial[];

}


/**
 * Manufacturer 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ManufacturerQuery
 * @description 对应后端 TaktManufacturerQueryDto
 */
export interface ManufacturerQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 制造商编码（唯一索引）
   */
  manufacturerCode?: string;

  /**
   * 制造商名称
   */
  manufacturerName?: string;

  /**
   * 制造商简称
   */
  manufacturerShortName?: string;

  /**
   * 制造商类型（字典 logistics_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）
   */
  manufacturerType?: number;

  /**
   * 行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）
   */
  industrySector?: string;

  /**
   * 制造商标识（税务登记证号/统一社会信用代码）
   */
  manufacturerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 制造商电话
   */
  manufacturerPhone?: string;

  /**
   * 制造商传真
   */
  manufacturerFax?: string;

  /**
   * 制造商邮箱
   */
  manufacturerEmail?: string;

  /**
   * 制造商网站
   */
  manufacturerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 制造商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时）
   */
  manufacturerLevel?: number;

  /**
   * 质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）
   */
  qualityCertification?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 制造商状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  manufacturerStatus?: number;

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
 * 创建Manufacturer DTO
 * 对应前端 ManufacturerCreate
 * @description 对应后端 TaktManufacturerCreateDto
 */
export interface ManufacturerCreate {
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
   * 制造商编码（唯一索引）
   */
  manufacturerCode: string;

  /**
   * 制造商名称
   */
  manufacturerName: string;

  /**
   * 制造商简称
   */
  manufacturerShortName?: string;

  /**
   * 制造商类型（字典 logistics_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）
   */
  manufacturerType: number;

  /**
   * 行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）
   */
  industrySector?: string;

  /**
   * 制造商标识（税务登记证号/统一社会信用代码）
   */
  manufacturerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 制造商电话
   */
  manufacturerPhone?: string;

  /**
   * 制造商传真
   */
  manufacturerFax?: string;

  /**
   * 制造商邮箱
   */
  manufacturerEmail?: string;

  /**
   * 制造商网站
   */
  manufacturerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 制造商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时）
   */
  manufacturerLevel: number;

  /**
   * 质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）
   */
  qualityCertification: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 制造商状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  manufacturerStatus: number;

  /**
   * 导航属性：制造商物料明细列表（子表，级联保存）
   */
  manufacturerMaterials?: ManufacturerMaterialCreate[];

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
 * 更新Manufacturer DTO
 * 继承 TaktManufacturerCreateDto，添加 ManufacturerId 字段
 * 对应前端 ManufacturerUpdate
 * @description 对应后端 TaktManufacturerUpdateDto
 */
export interface ManufacturerUpdate extends ManufacturerCreate {
  /**
   * ManufacturerID（标识要更新的实体）
   */
  manufacturerId: string;

}


/**
 * Manufacturer 状态更新 DTO
 * 对应前端 ManufacturerStatus
 * @description 对应后端 TaktManufacturerStatusDto
 */
export interface ManufacturerStatus {
  /**
   * ManufacturerID
   */
  manufacturerId: string;

  /**
   * 制造商状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  manufacturerStatus: number;

}


/**
 * Manufacturer 排序更新 DTO
 * 对应前端 ManufacturerSort
 * @description 对应后端 TaktManufacturerSortDto
 */
export interface ManufacturerSort {
  /**
   * ManufacturerID
   */
  manufacturerId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Manufacturer 导入模板行 DTO
 * 对应前端 ManufacturerTemplate
 * @description 对应后端 TaktManufacturerTemplateDto
 */
export interface ManufacturerTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 制造商编码（唯一索引）
   */
  manufacturerCode?: string;

  /**
   * 制造商名称
   */
  manufacturerName?: string;

  /**
   * 制造商简称
   */
  manufacturerShortName?: string;

  /**
   * 制造商类型（字典 logistics_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）
   */
  manufacturerType?: number;

  /**
   * 行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）
   */
  industrySector?: string;

  /**
   * 制造商标识（税务登记证号/统一社会信用代码）
   */
  manufacturerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 制造商电话
   */
  manufacturerPhone?: string;

  /**
   * 制造商传真
   */
  manufacturerFax?: string;

  /**
   * 制造商邮箱
   */
  manufacturerEmail?: string;

  /**
   * 制造商网站
   */
  manufacturerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 制造商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时）
   */
  manufacturerLevel?: number;

  /**
   * 质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）
   */
  qualityCertification?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 制造商状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  manufacturerStatus?: number;

  /**
   * 导航属性：制造商物料明细列表（子表，级联保存）
   */
  manufacturerMaterials?: ManufacturerMaterialCreate[];

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
 * Manufacturer 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ManufacturerImport
 * @description 对应后端 TaktManufacturerImportDto
 */
export interface ManufacturerImport {
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
   * 制造商编码（唯一索引）
   */
  manufacturerCode?: string;

  /**
   * 制造商名称
   */
  manufacturerName?: string;

  /**
   * 制造商简称
   */
  manufacturerShortName?: string;

  /**
   * 制造商类型（字典 logistics_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）
   */
  manufacturerType?: number;

  /**
   * 行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）
   */
  industrySector?: string;

  /**
   * 制造商标识（税务登记证号/统一社会信用代码）
   */
  manufacturerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 制造商电话
   */
  manufacturerPhone?: string;

  /**
   * 制造商传真
   */
  manufacturerFax?: string;

  /**
   * 制造商邮箱
   */
  manufacturerEmail?: string;

  /**
   * 制造商网站
   */
  manufacturerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 制造商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时）
   */
  manufacturerLevel?: number;

  /**
   * 质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）
   */
  qualityCertification?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 制造商状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  manufacturerStatus?: number;

  /**
   * 导航属性：制造商物料明细列表（子表，级联保存）
   */
  manufacturerMaterials?: ManufacturerMaterialCreate[];

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
 * Manufacturer 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ManufacturerExport
 * @description 对应后端 TaktManufacturerExportDto
 */
export interface ManufacturerExport {
  /**
   * ManufacturerID
   */
  manufacturerId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 制造商编码（唯一索引）
   */
  manufacturerCode: string;

  /**
   * 制造商名称
   */
  manufacturerName: string;

  /**
   * 制造商简称
   */
  manufacturerShortName?: string;

  /**
   * 制造商类型（字典 logistics_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）
   */
  manufacturerType: number;

  /**
   * 行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）
   */
  industrySector?: string;

  /**
   * 制造商标识（税务登记证号/统一社会信用代码）
   */
  manufacturerTaxNumber?: string;

  /**
   * 注册国家（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）
   */
  registrationCountry?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 制造商电话
   */
  manufacturerPhone?: string;

  /**
   * 制造商传真
   */
  manufacturerFax?: string;

  /**
   * 制造商邮箱
   */
  manufacturerEmail?: string;

  /**
   * 制造商网站
   */
  manufacturerWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 制造商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时）
   */
  manufacturerLevel: number;

  /**
   * 质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）
   */
  qualityCertification: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 制造商状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
   */
  manufacturerStatus: number;

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

