// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：invoice-item.d.ts
// 创建时间：2026-08-10
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
 * Takt销售发票明细实体（公司级；主子表关系见 SalesInvoiceId）
 * 对应前端 TaktSalesInvoiceItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesInvoiceItem
 * @description 对应后端 TaktSalesInvoiceItemDto
 */
export interface SalesInvoiceItem extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）
   */
  salesInvoiceId?: string;

  /**
   * 工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 开票凭证（冗余）
   */
  billingDocumentCode?: string;

  /**
   * 项目（开票凭证项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 已出发票数量
   */
  billingQuantity?: number;

  /**
   * 销售单位
   */
  salesUnit?: string;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 库存单位开票数量
   */
  billingQuantitySku?: number;

  /**
   * 净重量
   */
  netWeight?: number;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 重量单位
   */
  weightUnit?: string;

  /**
   * 业务范围
   */
  businessAreaCode?: string;

  /**
   * 定价日期
   */
  pricingDate?: string;

  /**
   * 提供服务日期
   */
  serviceRenderedDate?: string;

  /**
   * 汇率
   */
  pricingExchangeRate?: number;

  /**
   * 净价值
   */
  netAmount?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考项目
   */
  referenceDocumentItem?: number;

  /**
   * 先前凭证类别
   */
  referenceDocumentCategory?: string;

  /**
   * 销售凭证
   */
  salesDocumentCode?: string;

  /**
   * 销售凭证项目
   */
  salesDocumentItem?: number;

  /**
   * 销售凭证参考
   */
  salesDocumentReferenceFlag?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 描述
   */
  materialDescription?: string;

  /**
   * 定价参考物料
   */
  pricingReferenceMaterialCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料组
   */
  materialGroup?: string;

  /**
   * 项目类别
   */
  salesItemCategory?: string;

  /**
   * 产品层次（最长 18，故 Length=18）
   */
  productHierarchy?: string;

  /**
   * 装运点/接收点
   */
  shippingPoint?: string;

  /**
   * 产品组
   */
  division?: string;

  /**
   * 合作伙伴项目
   */
  partnerItem?: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  departureCountry?: string;

  /**
   * 交货工厂地区
   */
  plantRegion?: string;

  /**
   * 定价
   */
  pricingFlag?: string;

  /**
   * 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 成本
   */
  costAmount?: number;

  /**
   * 小计1
   */
  subtotal1?: number;

  /**
   * 小计2
   */
  subtotal2?: number;

  /**
   * 小计3
   */
  subtotal3?: number;

  /**
   * 小计4
   */
  subtotal4?: number;

  /**
   * 小计5
   */
  subtotal5?: number;

  /**
   * 小计6
   */
  subtotal6?: number;

  /**
   * 汇率统计
   */
  statisticsExchangeRate?: number;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 信贷价格
   */
  creditPrice?: number;

  /**
   * 客户组销售订单
   */
  customerGroupSalesOrder?: string;

  /**
   * 订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  destinationCountryOrder?: string;

  /**
   * 地区订单
   */
  regionOrder?: string;

  /**
   * 订单的销售机构
   */
  salesOrganizationOrder?: string;

  /**
   * 订单分销渠道
   */
  distributionChannelOrder?: string;

  /**
   * SD 凭证类别
   */
  documentCategory?: string;

  /**
   * 税额
   */
  taxAmount?: number;

  /**
   * 总值
   */
  grossAmount?: number;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 已创建的（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  postedBy?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * SalesInvoiceItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesInvoiceItemExport
 * @description 对应后端 TaktSalesInvoiceItemExportDto
 */
export interface SalesInvoiceItemExport {
  /**
   * SalesInvoiceItemID
   */
  salesInvoiceItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）
   */
  salesInvoiceId: string;

  /**
   * 工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 开票凭证（冗余）
   */
  billingDocumentCode: string;

  /**
   * 项目（开票凭证项目；行号步长生成器用 int，固定步长=10）
   */
  lineNumber: number;

  /**
   * 已出发票数量
   */
  billingQuantity?: number;

  /**
   * 销售单位
   */
  salesUnit?: string;

  /**
   * 基本计量单位
   */
  baseUnit?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 库存单位开票数量
   */
  billingQuantitySku?: number;

  /**
   * 净重量
   */
  netWeight?: number;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 重量单位
   */
  weightUnit?: string;

  /**
   * 业务范围
   */
  businessAreaCode?: string;

  /**
   * 定价日期
   */
  pricingDate?: string;

  /**
   * 提供服务日期
   */
  serviceRenderedDate?: string;

  /**
   * 汇率
   */
  pricingExchangeRate?: number;

  /**
   * 净价值
   */
  netAmount?: number;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考项目
   */
  referenceDocumentItem?: number;

  /**
   * 先前凭证类别
   */
  referenceDocumentCategory?: string;

  /**
   * 销售凭证
   */
  salesDocumentCode?: string;

  /**
   * 销售凭证项目
   */
  salesDocumentItem?: number;

  /**
   * 销售凭证参考
   */
  salesDocumentReferenceFlag?: string;

  /**
   * 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 描述
   */
  materialDescription?: string;

  /**
   * 定价参考物料
   */
  pricingReferenceMaterialCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料组
   */
  materialGroup?: string;

  /**
   * 项目类别
   */
  salesItemCategory?: string;

  /**
   * 产品层次（最长 18，故 Length=18）
   */
  productHierarchy?: string;

  /**
   * 装运点/接收点
   */
  shippingPoint?: string;

  /**
   * 产品组
   */
  division?: string;

  /**
   * 合作伙伴项目
   */
  partnerItem?: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  departureCountry?: string;

  /**
   * 交货工厂地区
   */
  plantRegion?: string;

  /**
   * 定价
   */
  pricingFlag?: string;

  /**
   * 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 成本
   */
  costAmount?: number;

  /**
   * 小计1
   */
  subtotal1?: number;

  /**
   * 小计2
   */
  subtotal2?: number;

  /**
   * 小计3
   */
  subtotal3?: number;

  /**
   * 小计4
   */
  subtotal4?: number;

  /**
   * 小计5
   */
  subtotal5?: number;

  /**
   * 小计6
   */
  subtotal6?: number;

  /**
   * 汇率统计
   */
  statisticsExchangeRate?: number;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 信贷价格
   */
  creditPrice?: number;

  /**
   * 客户组销售订单
   */
  customerGroupSalesOrder?: string;

  /**
   * 订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  destinationCountryOrder?: string;

  /**
   * 地区订单
   */
  regionOrder?: string;

  /**
   * 订单的销售机构
   */
  salesOrganizationOrder?: string;

  /**
   * 订单分销渠道
   */
  distributionChannelOrder?: string;

  /**
   * SD 凭证类别
   */
  documentCategory?: string;

  /**
   * 税额
   */
  taxAmount?: number;

  /**
   * 总值
   */
  grossAmount?: number;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 已创建的（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  postedBy?: string;

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

