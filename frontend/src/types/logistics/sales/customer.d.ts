// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：customer.d.ts
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
 * Takt客户信息实体
 * 对应前端 TaktCustomerDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Customer
 * @description 对应后端 TaktCustomerDto
 */
export interface Customer extends CompanyDtoBase {
  /**
   * CustomerID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 客户编码（唯一索引）
   */
  customerCode: string;

  /**
   * 客户名称1
   */
  customerName1: string;

  /**
   * 客户名称2
   */
  customerName2?: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（字典 logistics_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  customerType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature_type）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute_type）
   */
  industryAttribute: string;

  /**
   * 区域文化编码（字典 sys_culture_code；即语言/区域文化）
   */
  defaultCulture: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）
   */
  taxRate: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

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
   * 结算币种代码（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization: string;

  /**
   * 分销渠道
   */
  distributionChannel: string;

  /**
   * 产品组
   */
  productGroup: string;

  /**
   * 客户组（字典 logistics_customer_group；DictValue=Z1～Z4）
   */
  customerGroup: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner: string;

  /**
   * 帐户分配组（字典 logistics_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 尼尔森标识
   */
  nielsenIndicator: string;

  /**
   * 中心记帐冻结（字典 sys_yes_no_type；0=否 1=是）
   */
  centralPostingBlock: number;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  headquarters: string;

  /**
   * 具有供应商的清算（字典 sys_yes_no_type；0=否 1=是）
   */
  clearingWithVendor: number;

  /**
   * 付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms: string;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant: string;

  /**
   * 国际贸易条件1（字典 logistics_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2: string;

  /**
   * 装运条件（字典 logistics_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions: string;

  /**
   * 客户定价过程（字典 logistics_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure: string;

  /**
   * 信用等级（字典 logistics_credit_rating_category；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount: number;

  /**
   * 折扣率（百分比；可选字典 logistics_discount_rate_param 预设）
   */
  discountRate: number;

  /**
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  salesBy?: string;

  /**
   * 客户等级（字典 logistics_customer_level_category；0=普通 1=重要 2=VIP 3=战略）
   */
  customerLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客户状态（字典 sys_normal_disable_status）
   */
  customerStatus: number;

}


/**
 * Customer 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerQuery
 * @description 对应后端 TaktCustomerQueryDto
 */
export interface CustomerQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 客户编码（唯一索引）
   */
  customerCode?: string;

  /**
   * 客户名称1
   */
  customerName1?: string;

  /**
   * 客户名称2
   */
  customerName2?: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（字典 logistics_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  customerType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature_type）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute_type）
   */
  industryAttribute?: string;

  /**
   * 区域文化编码（字典 sys_culture_code；即语言/区域文化）
   */
  defaultCulture?: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）
   */
  taxRate?: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

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
   * 结算币种代码（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 产品组
   */
  productGroup?: string;

  /**
   * 客户组（字典 logistics_customer_group；DictValue=Z1～Z4）
   */
  customerGroup?: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner?: string;

  /**
   * 帐户分配组（字典 logistics_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 尼尔森标识
   */
  nielsenIndicator?: string;

  /**
   * 中心记帐冻结（字典 sys_yes_no_type；0=否 1=是）
   */
  centralPostingBlock?: number;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  headquarters?: string;

  /**
   * 具有供应商的清算（字典 sys_yes_no_type；0=否 1=是）
   */
  clearingWithVendor?: number;

  /**
   * 付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant?: string;

  /**
   * 国际贸易条件1（字典 logistics_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2?: string;

  /**
   * 装运条件（字典 logistics_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions?: string;

  /**
   * 客户定价过程（字典 logistics_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure?: string;

  /**
   * 信用等级（字典 logistics_credit_rating_category；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel?: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount?: number;

  /**
   * 折扣率（百分比；可选字典 logistics_discount_rate_param 预设）
   */
  discountRate?: number;

  /**
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  salesBy?: string;

  /**
   * 客户等级（字典 logistics_customer_level_category；0=普通 1=重要 2=VIP 3=战略）
   */
  customerLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 客户状态（字典 sys_normal_disable_status）
   */
  customerStatus?: number;

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
 * 创建Customer DTO
 * 对应前端 CustomerCreate
 * @description 对应后端 TaktCustomerCreateDto
 */
export interface CustomerCreate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 客户编码（唯一索引）
   */
  customerCode: string;

  /**
   * 客户名称1
   */
  customerName1: string;

  /**
   * 客户名称2
   */
  customerName2?: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（字典 logistics_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  customerType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature_type）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute_type）
   */
  industryAttribute: string;

  /**
   * 区域文化编码（字典 sys_culture_code；即语言/区域文化）
   */
  defaultCulture: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）
   */
  taxRate: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

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
   * 结算币种代码（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization: string;

  /**
   * 分销渠道
   */
  distributionChannel: string;

  /**
   * 产品组
   */
  productGroup: string;

  /**
   * 客户组（字典 logistics_customer_group；DictValue=Z1～Z4）
   */
  customerGroup: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner: string;

  /**
   * 帐户分配组（字典 logistics_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 尼尔森标识
   */
  nielsenIndicator: string;

  /**
   * 中心记帐冻结（字典 sys_yes_no_type；0=否 1=是）
   */
  centralPostingBlock: number;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  headquarters: string;

  /**
   * 具有供应商的清算（字典 sys_yes_no_type；0=否 1=是）
   */
  clearingWithVendor: number;

  /**
   * 付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms: string;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant: string;

  /**
   * 国际贸易条件1（字典 logistics_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2: string;

  /**
   * 装运条件（字典 logistics_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions: string;

  /**
   * 客户定价过程（字典 logistics_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure: string;

  /**
   * 信用等级（字典 logistics_credit_rating_category；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount: number;

  /**
   * 折扣率（百分比；可选字典 logistics_discount_rate_param 预设）
   */
  discountRate: number;

  /**
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  salesBy?: string;

  /**
   * 客户等级（字典 logistics_customer_level_category；0=普通 1=重要 2=VIP 3=战略）
   */
  customerLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 客户状态（字典 sys_normal_disable_status）
   */
  customerStatus: number;

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
 * 更新Customer DTO
 * 继承 TaktCustomerCreateDto，添加 CustomerId 字段
 * 对应前端 CustomerUpdate
 * @description 对应后端 TaktCustomerUpdateDto
 */
export interface CustomerUpdate extends CustomerCreate {
  /**
   * CustomerID（标识要更新的实体）
   */
  customerId: string;

}


/**
 * Customer 状态更新 DTO
 * 对应前端 CustomerStatus
 * @description 对应后端 TaktCustomerStatusDto
 */
export interface CustomerStatus {
  /**
   * CustomerID
   */
  customerId: string;

  /**
   * 客户状态（字典 sys_normal_disable_status）
   */
  customerStatus: number;

}


/**
 * Customer 排序更新 DTO
 * 对应前端 CustomerSort
 * @description 对应后端 TaktCustomerSortDto
 */
export interface CustomerSort {
  /**
   * CustomerID
   */
  customerId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Customer 导入模板行 DTO
 * 对应前端 CustomerTemplate
 * @description 对应后端 TaktCustomerTemplateDto
 */
export interface CustomerTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 客户编码（唯一索引）
   */
  customerCode?: string;

  /**
   * 客户名称1
   */
  customerName1?: string;

  /**
   * 客户名称2
   */
  customerName2?: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（字典 logistics_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  customerType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature_type）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute_type）
   */
  industryAttribute?: string;

  /**
   * 区域文化编码（字典 sys_culture_code；即语言/区域文化）
   */
  defaultCulture?: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）
   */
  taxRate?: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

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
   * 结算币种代码（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 产品组
   */
  productGroup?: string;

  /**
   * 客户组（字典 logistics_customer_group；DictValue=Z1～Z4）
   */
  customerGroup?: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner?: string;

  /**
   * 帐户分配组（字典 logistics_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 尼尔森标识
   */
  nielsenIndicator?: string;

  /**
   * 中心记帐冻结（字典 sys_yes_no_type；0=否 1=是）
   */
  centralPostingBlock?: number;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  headquarters?: string;

  /**
   * 具有供应商的清算（字典 sys_yes_no_type；0=否 1=是）
   */
  clearingWithVendor?: number;

  /**
   * 付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant?: string;

  /**
   * 国际贸易条件1（字典 logistics_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2?: string;

  /**
   * 装运条件（字典 logistics_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions?: string;

  /**
   * 客户定价过程（字典 logistics_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure?: string;

  /**
   * 信用等级（字典 logistics_credit_rating_category；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel?: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount?: number;

  /**
   * 折扣率（百分比；可选字典 logistics_discount_rate_param 预设）
   */
  discountRate?: number;

  /**
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  salesBy?: string;

  /**
   * 客户等级（字典 logistics_customer_level_category；0=普通 1=重要 2=VIP 3=战略）
   */
  customerLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 客户状态（字典 sys_normal_disable_status）
   */
  customerStatus?: number;

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
 * Customer 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerImport
 * @description 对应后端 TaktCustomerImportDto
 */
export interface CustomerImport {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 客户编码（唯一索引）
   */
  customerCode?: string;

  /**
   * 客户名称1
   */
  customerName1?: string;

  /**
   * 客户名称2
   */
  customerName2?: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（字典 logistics_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  customerType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature_type）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute_type）
   */
  industryAttribute?: string;

  /**
   * 区域文化编码（字典 sys_culture_code；即语言/区域文化）
   */
  defaultCulture?: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）
   */
  taxRate?: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

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
   * 结算币种代码（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 产品组
   */
  productGroup?: string;

  /**
   * 客户组（字典 logistics_customer_group；DictValue=Z1～Z4）
   */
  customerGroup?: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner?: string;

  /**
   * 帐户分配组（字典 logistics_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 尼尔森标识
   */
  nielsenIndicator?: string;

  /**
   * 中心记帐冻结（字典 sys_yes_no_type；0=否 1=是）
   */
  centralPostingBlock?: number;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  headquarters?: string;

  /**
   * 具有供应商的清算（字典 sys_yes_no_type；0=否 1=是）
   */
  clearingWithVendor?: number;

  /**
   * 付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant?: string;

  /**
   * 国际贸易条件1（字典 logistics_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2?: string;

  /**
   * 装运条件（字典 logistics_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions?: string;

  /**
   * 客户定价过程（字典 logistics_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure?: string;

  /**
   * 信用等级（字典 logistics_credit_rating_category；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel?: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount?: number;

  /**
   * 折扣率（百分比；可选字典 logistics_discount_rate_param 预设）
   */
  discountRate?: number;

  /**
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  salesBy?: string;

  /**
   * 客户等级（字典 logistics_customer_level_category；0=普通 1=重要 2=VIP 3=战略）
   */
  customerLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 客户状态（字典 sys_normal_disable_status）
   */
  customerStatus?: number;

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
 * Customer 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerExport
 * @description 对应后端 TaktCustomerExportDto
 */
export interface CustomerExport {
  /**
   * CustomerID
   */
  customerId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 客户编码（唯一索引）
   */
  customerCode: string;

  /**
   * 客户名称1
   */
  customerName1: string;

  /**
   * 客户名称2
   */
  customerName2?: string;

  /**
   * 客户简称
   */
  customerShortName?: string;

  /**
   * 客户类型（字典 logistics_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  customerType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature_type）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute_type）
   */
  industryAttribute: string;

  /**
   * 区域文化编码（字典 sys_culture_code；即语言/区域文化）
   */
  defaultCulture: string;

  /**
   * 客户标识（税务登记证号/统一社会信用代码）
   */
  customerTaxNumber?: string;

  /**
   * 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）
   */
  taxRate: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 客户电话
   */
  customerPhone?: string;

  /**
   * 客户传真
   */
  customerFax?: string;

  /**
   * 客户邮箱
   */
  customerEmail?: string;

  /**
   * 客户网站
   */
  customerWebsite?: string;

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
   * 结算币种代码（字典 accounting_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization: string;

  /**
   * 分销渠道
   */
  distributionChannel: string;

  /**
   * 产品组
   */
  productGroup: string;

  /**
   * 客户组（字典 logistics_customer_group；DictValue=Z1～Z4）
   */
  customerGroup: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner: string;

  /**
   * 帐户分配组（字典 logistics_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 尼尔森标识
   */
  nielsenIndicator: string;

  /**
   * 中心记帐冻结（字典 sys_yes_no_type；0=否 1=是）
   */
  centralPostingBlock: number;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  headquarters: string;

  /**
   * 具有供应商的清算（字典 sys_yes_no_type；0=否 1=是）
   */
  clearingWithVendor: number;

  /**
   * 付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms: string;

  /**
   * 付款方式（字典 accounting_payment_method_type；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant: string;

  /**
   * 国际贸易条件1（字典 logistics_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2: string;

  /**
   * 装运条件（字典 logistics_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions: string;

  /**
   * 客户定价过程（字典 logistics_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure: string;

  /**
   * 信用等级（字典 logistics_credit_rating_category；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount: number;

  /**
   * 折扣率（百分比；可选字典 logistics_discount_rate_param 预设）
   */
  discountRate: number;

  /**
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  salesBy?: string;

  /**
   * 客户等级（字典 logistics_customer_level_category；0=普通 1=重要 2=VIP 3=战略）
   */
  customerLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客户状态（字典 sys_normal_disable_status）
   */
  customerStatus: number;

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

