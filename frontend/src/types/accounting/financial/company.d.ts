// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：company.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantDtoBase
} from '@/types/common';

/**
 * 公司实体 代表租户下的独立公司/工厂（租户级实体，只需要TenantCode） 参照 SAP Company Code (BUKRS) 设计
 * 对应前端 TaktCompanyDto
 * 继承 TaktTenantDtoBase
 * 对应前端 Company
 * @description 对应后端 TaktCompanyDto
 */
export interface Company extends TenantDtoBase {
  /**
   * CompanyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  companyId: string;

  /**
   * 公司名称
   */
  companyName: string;

  /**
   * 公司简称
   */
  companyShortName: string;

  /**
   * 公司类型
   */
  companyType: number;

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
   * 公司电话
   */
  companyPhone: string;

  /**
   * 公司邮箱
   */
  companyEmail: string;

  /**
   * 公司传真
   */
  companyFax: string;

  /**
   * 公司网站
   */
  companyWebsite: string;

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
   * 公司负责人
   */
  companyManager: string;

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
  companyExistence: number;

  /**
   * 关联工厂编码（如 0001、C100）
   */
  relatedPlant: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias: string;

  /**
   * 公司状态
   */
  companyStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 可访问该公司的角色关联（RBAC，表 takt_identity_role_company） （子表：TaktRoleCompany）
   */
  roleCompanies?: RoleCompany[];

  /**
   * 可访问该公司的用户关联（RBAC，表 takt_identity_user_company） （子表：TaktUserCompany）
   */
  userCompanies?: UserCompany[];

}


/**
 * Company 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CompanyQuery
 * @description 对应后端 TaktCompanyQueryDto
 */
export interface CompanyQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司名称
   */
  companyName?: string;

  /**
   * 公司简称
   */
  companyShortName?: string;

  /**
   * 公司类型
   */
  companyType?: number;

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
   * 公司电话
   */
  companyPhone?: string;

  /**
   * 公司邮箱
   */
  companyEmail?: string;

  /**
   * 公司传真
   */
  companyFax?: string;

  /**
   * 公司网站
   */
  companyWebsite?: string;

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
   * 公司负责人
   */
  companyManager?: string;

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
  companyExistence?: number;

  /**
   * 关联工厂编码（如 0001、C100）
   */
  relatedPlant?: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture?: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias?: string;

  /**
   * 公司状态
   */
  companyStatus?: number;

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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Company DTO
 * 对应前端 CompanyCreate
 * @description 对应后端 TaktCompanyCreateDto
 */
export interface CompanyCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司名称
   */
  companyName: string;

  /**
   * 公司简称
   */
  companyShortName: string;

  /**
   * 公司类型
   */
  companyType: number;

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
   * 公司电话
   */
  companyPhone: string;

  /**
   * 公司邮箱
   */
  companyEmail: string;

  /**
   * 公司传真
   */
  companyFax: string;

  /**
   * 公司网站
   */
  companyWebsite: string;

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
   * 公司负责人
   */
  companyManager: string;

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
  companyExistence: number;

  /**
   * 关联工厂编码（如 0001、C100）
   */
  relatedPlant: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias: string;

  /**
   * 公司状态
   */
  companyStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 可访问该公司的角色 ID 列表（RBAC 反向合并）
   */
  roleIds?: any;

  /**
   * 可访问该公司的用户 ID 列表（RBAC 反向合并）
   */
  userIds?: any;

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
 * 更新Company DTO
 * 继承 TaktCompanyCreateDto，添加 CompanyId 字段
 * 对应前端 CompanyUpdate
 * @description 对应后端 TaktCompanyUpdateDto
 */
export interface CompanyUpdate extends CompanyCreate {
  /**
   * CompanyID（标识要更新的实体）
   */
  companyId: string;

}


/**
 * Company 状态更新 DTO
 * 对应前端 CompanyStatus
 * @description 对应后端 TaktCompanyStatusDto
 */
export interface CompanyStatus {
  /**
   * CompanyID
   */
  companyId: string;

  /**
   * 公司状态
   */
  companyStatus: number;

}


/**
 * Company 排序更新 DTO
 * 对应前端 CompanySort
 * @description 对应后端 TaktCompanySortDto
 */
export interface CompanySort {
  /**
   * CompanyID
   */
  companyId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Company 导入模板行 DTO
 * 对应前端 CompanyTemplate
 * @description 对应后端 TaktCompanyTemplateDto
 */
export interface CompanyTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司名称
   */
  companyName?: string;

  /**
   * 公司简称
   */
  companyShortName?: string;

  /**
   * 公司类型
   */
  companyType?: number;

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
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Company 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CompanyImport
 * @description 对应后端 TaktCompanyImportDto
 */
export interface CompanyImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司名称
   */
  companyName?: string;

  /**
   * 公司简称
   */
  companyShortName?: string;

  /**
   * 公司类型
   */
  companyType?: number;

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
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Company 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CompanyExport
 * @description 对应后端 TaktCompanyExportDto
 */
export interface CompanyExport {
  /**
   * CompanyID
   */
  companyId: string;

  /**
   * 公司名称
   */
  companyName: string;

  /**
   * 公司简称
   */
  companyShortName: string;

  /**
   * 公司类型
   */
  companyType: number;

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
   * 公司电话
   */
  companyPhone: string;

  /**
   * 公司邮箱
   */
  companyEmail: string;

  /**
   * 公司传真
   */
  companyFax: string;

  /**
   * 公司网站
   */
  companyWebsite: string;

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
   * 公司负责人
   */
  companyManager: string;

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
  companyExistence: number;

  /**
   * 关联工厂编码（如 0001、C100）
   */
  relatedPlant: string;

  /**
   * 默认区域文化编码（BCP47，如 zh-CN、en-US、ja-JP、zh-HK）
   */
  defaultCulture: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias: string;

  /**
   * 公司状态
   */
  companyStatus: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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

