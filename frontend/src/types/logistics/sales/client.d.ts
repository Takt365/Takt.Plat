// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：client.d.ts
// 创建时间：2026-08-06
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
 * Takt客户端信息实体
 * 对应前端 TaktClientDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Client
 * @description 对应后端 TaktClientDto
 */
export interface Client extends CompanyDtoBase {


  /**
   * 客户端编码（唯一索引）
   */
  clientCode?: string;

  /**
   * 客户端名称1
   */
  clientName1?: string;

  /**
   * 客户端名称2
   */
  clientName2?: string;

  /**
   * 客户端简称
   */
  clientShortName?: string;

  /**
   * 客户端类型（字典 logistics_client_category；0=终端客户 1=分销商 2=零售商 3=电商平台 4=其他）
   */
  clientType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature_type）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute_type）
   */
  industryAttribute?: string;

  /**
   * 客户端标识（税务登记证号/统一社会信用代码）
   */
  clientTaxNumber?: string;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
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
   * 客户端电话
   */
  clientPhone?: string;

  /**
   * 客户端传真
   */
  clientFax?: string;

  /**
   * 客户端邮箱
   */
  clientEmail?: string;

  /**
   * 客户端网站
   */
  clientWebsite?: string;

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
   * 总部（选项 TaktClients/options；DictValue=ClientCode）
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
   * 销售渠道（字典 logistics_sales_channel_type；0=直销 1=经销 2=代销 3=电商 4=其他）
   */
  salesChannel?: number;

  /**
   * 平台名称（电商平台名称）
   */
  platformName?: string;

  /**
   * 店铺名称
   */
  storeName?: string;

  /**
   * 客户端等级（字典 logistics_customer_level_category；0=普通 1=重要 2=VIP 3=战略）
   */
  clientLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 客户端状态（字典 sys_normal_disable_status）
   */
  clientStatus?: number;

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
 * Client 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ClientExport
 * @description 对应后端 TaktClientExportDto
 */
export interface ClientExport {
  /**
   * ClientID
   */
  clientId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 客户端编码（唯一索引）
   */
  clientCode: string;

  /**
   * 客户端名称1
   */
  clientName1: string;

  /**
   * 客户端名称2
   */
  clientName2?: string;

  /**
   * 客户端简称
   */
  clientShortName?: string;

  /**
   * 客户端类型（字典 logistics_client_category；0=终端客户 1=分销商 2=零售商 3=电商平台 4=其他）
   */
  clientType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature_type）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute_type）
   */
  industryAttribute: string;

  /**
   * 客户端标识（税务登记证号/统一社会信用代码）
   */
  clientTaxNumber?: string;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
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
   * 客户端电话
   */
  clientPhone?: string;

  /**
   * 客户端传真
   */
  clientFax?: string;

  /**
   * 客户端邮箱
   */
  clientEmail?: string;

  /**
   * 客户端网站
   */
  clientWebsite?: string;

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
   * 总部（选项 TaktClients/options；DictValue=ClientCode）
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
   * 销售渠道（字典 logistics_sales_channel_type；0=直销 1=经销 2=代销 3=电商 4=其他）
   */
  salesChannel: number;

  /**
   * 平台名称（电商平台名称）
   */
  platformName?: string;

  /**
   * 店铺名称
   */
  storeName?: string;

  /**
   * 客户端等级（字典 logistics_customer_level_category；0=普通 1=重要 2=VIP 3=战略）
   */
  clientLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 客户端状态（字典 sys_normal_disable_status）
   */
  clientStatus: number;

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

