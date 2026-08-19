// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：quotation.d.ts
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
 * Takt销售报价实体
 * 对应前端 TaktSalesQuotationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesQuotation
 * @description 对应后端 TaktSalesQuotationDto
 */
export interface SalesQuotation extends CompanyDtoBase {

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 报价日期
   */
  quotationDate?: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  salesBy?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 报价总金额
   */
  totalAmount?: number;

  /**
   * 折扣金额
   */
  discountAmount?: number;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode?: string;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
  taxRate?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 报价实付金额
   */
  actualAmount?: number;

  /**
   * 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
   */
  salesOrderCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
   */
  quotationStatus?: number;

  /**
   * 销售报价明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesQuotationItemCreate[];

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
 * SalesQuotation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesQuotationExport
 * @description 对应后端 TaktSalesQuotationExportDto
 */
export interface SalesQuotationExport {
  /**
   * SalesQuotationID
   */
  salesQuotationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 销售报价编码（唯一索引）
   */
  salesQuotationCode: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 报价日期
   */
  quotationDate: string;

  /**
   * 报价有效期至
   */
  validUntilDate?: string;

  /**
   * 销售员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  salesBy?: string;

  /**
   * 报价总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 报价总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
   */
  currencyCode: string;

  /**
   * 税码（字典 accounting_tax_code；按 CultureCode 匹配区域字典；DictValue 随区域变化）
   */
  taxCode?: string | null;
  taxRate: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 报价实付金额
   */
  actualAmount: number;

  /**
   * 关联销售订单编码（报价转订单后回填；选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
   */
  salesOrderCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 报价状态（字典 logistics_quotation_status；0=草稿 1=已发送 2=已接受 3=已拒绝 4=已过期 5=已作废）
   */
  quotationStatus: number;

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

