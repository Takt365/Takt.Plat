// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：plant.d.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantDtoBase
} from '@/types/common';

/**
 * Takt工厂实体 代表租户下的独立工厂（租户级实体，只需要TenantCode） 与公司种子对称，参照 SAP Plant 设计
 * 对应前端 TaktPlantDto
 * 继承 TaktTenantDtoBase
 * 对应前端 Plant
 * @description 对应后端 TaktPlantDto
 */
export interface Plant extends TenantDtoBase {
  /**
   * PlantID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  plantId: string;

  /**
   * 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
   */
  plantCode: string;

  /**
   * 工厂名称
   */
  plantName: string;

  /**
   * 工厂简称
   */
  plantShortName: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture: string;

  /**
   * 工厂类型
   */
  plantType: number;

  /**
   * 关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）
   */
  relatedCompany: string;

  /**
   * 企业性质（统计用登记注册类型代码，国统字〔1998〕200号）
   */
  enterpriseNature: number;

  /**
   * 行业属性（GB/T 4754-2017 国民经济行业分类门类）
   */
  industryAttribute: number;

  /**
   * 企业规模（统计上大中小微型划分代码 1–4）
   */
  enterpriseScale: number;

  /**
   * 经营范围
   */
  businessScope: string;

  /**
   * 注册地址1
   */
  registrationAddress1: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 注册国家
   */
  registrationRegion: string;

  /**
   * 注册省
   */
  registrationProvince: string;

  /**
   * 注册市
   */
  registrationCity: string;

  /**
   * 经营国家
   */
  businessRegion: string;

  /**
   * 经营地区-省
   */
  businessProvince: string;

  /**
   * 经营地区-市
   */
  businessCity: string;

  /**
   * 经营地址1
   */
  businessAddress1: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 经营地址3
   */
  businessAddress3?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂地址3
   */
  plantAddress3?: string;

  /**
   * 工厂电话
   */
  plantPhone: string;

  /**
   * 工厂邮箱
   */
  plantEmail: string;

  /**
   * 工厂传真
   */
  plantFax: string;

  /**
   * 工厂网站
   */
  plantWebsite: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber: string;

  /**
   * 法定代表人
   */
  legalRepresentative: string;

  /**
   * 工厂负责人
   */
  plantManager: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital: number;

  /**
   * 成立日期
   */
  establishmentDate: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）
   */
  closingDate?: string;

  /**
   * 存续状态（市场主体登记状态）
   */
  plantExistence: number;

  /**
   * 工厂状态
   */
  plantStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Plant 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PlantQuery
 * @description 对应后端 TaktPlantQueryDto
 */
export interface PlantQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
   */
  plantCode?: string;

  /**
   * 工厂名称
   */
  plantName?: string;

  /**
   * 工厂简称
   */
  plantShortName?: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias?: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture?: string;

  /**
   * 工厂类型
   */
  plantType?: number;

  /**
   * 关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）
   */
  relatedCompany?: string;

  /**
   * 企业性质（统计用登记注册类型代码，国统字〔1998〕200号）
   */
  enterpriseNature?: number;

  /**
   * 行业属性（GB/T 4754-2017 国民经济行业分类门类）
   */
  industryAttribute?: number;

  /**
   * 企业规模（统计上大中小微型划分代码 1–4）
   */
  enterpriseScale?: number;

  /**
   * 经营范围
   */
  businessScope?: string;

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
   * 注册国家
   */
  registrationRegion?: string;

  /**
   * 注册省
   */
  registrationProvince?: string;

  /**
   * 注册市
   */
  registrationCity?: string;

  /**
   * 经营国家
   */
  businessRegion?: string;

  /**
   * 经营地区-省
   */
  businessProvince?: string;

  /**
   * 经营地区-市
   */
  businessCity?: string;

  /**
   * 经营地址1
   */
  businessAddress1?: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 经营地址3
   */
  businessAddress3?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂地址3
   */
  plantAddress3?: string;

  /**
   * 工厂电话
   */
  plantPhone?: string;

  /**
   * 工厂邮箱
   */
  plantEmail?: string;

  /**
   * 工厂传真
   */
  plantFax?: string;

  /**
   * 工厂网站
   */
  plantWebsite?: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode?: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber?: string;

  /**
   * 法定代表人
   */
  legalRepresentative?: string;

  /**
   * 工厂负责人
   */
  plantManager?: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital?: number;

  /**
   * 成立日期（范围查询-开始）
   */
  establishmentDateStart?: string;

  /**
   * 成立日期（范围查询-结束）
   */
  establishmentDateEnd?: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）（范围查询-开始）
   */
  closingDateStart?: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）（范围查询-结束）
   */
  closingDateEnd?: string;

  /**
   * 存续状态（市场主体登记状态）
   */
  plantExistence?: number;

  /**
   * 工厂状态
   */
  plantStatus?: number;

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
 * 创建Plant DTO
 * 对应前端 PlantCreate
 * @description 对应后端 TaktPlantCreateDto
 */
export interface PlantCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
   */
  plantCode: string;

  /**
   * 工厂名称
   */
  plantName: string;

  /**
   * 工厂简称
   */
  plantShortName: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture: string;

  /**
   * 工厂类型
   */
  plantType: number;

  /**
   * 关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）
   */
  relatedCompany: string;

  /**
   * 企业性质（统计用登记注册类型代码，国统字〔1998〕200号）
   */
  enterpriseNature: number;

  /**
   * 行业属性（GB/T 4754-2017 国民经济行业分类门类）
   */
  industryAttribute: number;

  /**
   * 企业规模（统计上大中小微型划分代码 1–4）
   */
  enterpriseScale: number;

  /**
   * 经营范围
   */
  businessScope: string;

  /**
   * 注册地址1
   */
  registrationAddress1: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 注册国家
   */
  registrationRegion: string;

  /**
   * 注册省
   */
  registrationProvince: string;

  /**
   * 注册市
   */
  registrationCity: string;

  /**
   * 经营国家
   */
  businessRegion: string;

  /**
   * 经营地区-省
   */
  businessProvince: string;

  /**
   * 经营地区-市
   */
  businessCity: string;

  /**
   * 经营地址1
   */
  businessAddress1: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 经营地址3
   */
  businessAddress3?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂地址3
   */
  plantAddress3?: string;

  /**
   * 工厂电话
   */
  plantPhone: string;

  /**
   * 工厂邮箱
   */
  plantEmail: string;

  /**
   * 工厂传真
   */
  plantFax: string;

  /**
   * 工厂网站
   */
  plantWebsite: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber: string;

  /**
   * 法定代表人
   */
  legalRepresentative: string;

  /**
   * 工厂负责人
   */
  plantManager: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital: number;

  /**
   * 成立日期
   */
  establishmentDate: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）
   */
  closingDate?: string;

  /**
   * 存续状态（市场主体登记状态）
   */
  plantExistence: number;

  /**
   * 工厂状态
   */
  plantStatus: number;

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
 * 更新Plant DTO
 * 继承 TaktPlantCreateDto，添加 PlantId 字段
 * 对应前端 PlantUpdate
 * @description 对应后端 TaktPlantUpdateDto
 */
export interface PlantUpdate extends PlantCreate {
  /**
   * PlantID（标识要更新的实体）
   */
  plantId: string;

}


/**
 * Plant 状态更新 DTO
 * 对应前端 PlantStatus
 * @description 对应后端 TaktPlantStatusDto
 */
export interface PlantStatus {
  /**
   * PlantID
   */
  plantId: string;

  /**
   * 工厂状态
   */
  plantStatus: number;

}


/**
 * Plant 排序更新 DTO
 * 对应前端 PlantSort
 * @description 对应后端 TaktPlantSortDto
 */
export interface PlantSort {
  /**
   * PlantID
   */
  plantId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Plant 导入模板行 DTO
 * 对应前端 PlantTemplate
 * @description 对应后端 TaktPlantTemplateDto
 */
export interface PlantTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
   */
  plantCode?: string;

  /**
   * 工厂名称
   */
  plantName?: string;

  /**
   * 工厂简称
   */
  plantShortName?: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias?: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture?: string;

  /**
   * 工厂类型
   */
  plantType?: number;

  /**
   * 关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）
   */
  relatedCompany?: string;

  /**
   * 企业性质（统计用登记注册类型代码，国统字〔1998〕200号）
   */
  enterpriseNature?: number;

  /**
   * 行业属性（GB/T 4754-2017 国民经济行业分类门类）
   */
  industryAttribute?: number;

  /**
   * 企业规模（统计上大中小微型划分代码 1–4）
   */
  enterpriseScale?: number;

  /**
   * 经营范围
   */
  businessScope?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

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
 * Plant 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PlantImport
 * @description 对应后端 TaktPlantImportDto
 */
export interface PlantImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
   */
  plantCode?: string;

  /**
   * 工厂名称
   */
  plantName?: string;

  /**
   * 工厂简称
   */
  plantShortName?: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias?: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture?: string;

  /**
   * 工厂类型
   */
  plantType?: number;

  /**
   * 关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）
   */
  relatedCompany?: string;

  /**
   * 企业性质（统计用登记注册类型代码，国统字〔1998〕200号）
   */
  enterpriseNature?: number;

  /**
   * 行业属性（GB/T 4754-2017 国民经济行业分类门类）
   */
  industryAttribute?: number;

  /**
   * 企业规模（统计上大中小微型划分代码 1–4）
   */
  enterpriseScale?: number;

  /**
   * 经营范围
   */
  businessScope?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

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
 * Plant 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PlantExport
 * @description 对应后端 TaktPlantExportDto
 */
export interface PlantExport {
  /**
   * PlantID
   */
  plantId: string;

  /**
   * 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
   */
  plantCode: string;

  /**
   * 工厂名称
   */
  plantName: string;

  /**
   * 工厂简称
   */
  plantShortName: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture: string;

  /**
   * 工厂类型
   */
  plantType: number;

  /**
   * 关联公司代码（如 2300、2400；与公司 RelatedPlant 对称）
   */
  relatedCompany: string;

  /**
   * 企业性质（统计用登记注册类型代码，国统字〔1998〕200号）
   */
  enterpriseNature: number;

  /**
   * 行业属性（GB/T 4754-2017 国民经济行业分类门类）
   */
  industryAttribute: number;

  /**
   * 企业规模（统计上大中小微型划分代码 1–4）
   */
  enterpriseScale: number;

  /**
   * 经营范围
   */
  businessScope: string;

  /**
   * 注册地址1
   */
  registrationAddress1: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册地址3
   */
  registrationAddress3?: string;

  /**
   * 注册国家
   */
  registrationRegion: string;

  /**
   * 注册省
   */
  registrationProvince: string;

  /**
   * 注册市
   */
  registrationCity: string;

  /**
   * 经营国家
   */
  businessRegion: string;

  /**
   * 经营地区-省
   */
  businessProvince: string;

  /**
   * 经营地区-市
   */
  businessCity: string;

  /**
   * 经营地址1
   */
  businessAddress1: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 经营地址3
   */
  businessAddress3?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂地址3
   */
  plantAddress3?: string;

  /**
   * 工厂电话
   */
  plantPhone: string;

  /**
   * 工厂邮箱
   */
  plantEmail: string;

  /**
   * 工厂传真
   */
  plantFax: string;

  /**
   * 工厂网站
   */
  plantWebsite: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber: string;

  /**
   * 法定代表人
   */
  legalRepresentative: string;

  /**
   * 工厂负责人
   */
  plantManager: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital: number;

  /**
   * 成立日期
   */
  establishmentDate: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）
   */
  closingDate?: string;

  /**
   * 存续状态（市场主体登记状态）
   */
  plantExistence: number;

  /**
   * 工厂状态
   */
  plantStatus: number;

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

