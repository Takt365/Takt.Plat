// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：invoice-item.d.ts
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
 * Takt销售发票明细实体
 * 对应前端 TaktSalesInvoiceItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesInvoiceItem
 * @description 对应后端 TaktSalesInvoiceItemDto
 */
export interface SalesInvoiceItem extends CompanyDtoBase {
  /**
   * SalesInvoiceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesInvoiceItemId: string;

  /**
   * 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
   */
  salesInvoiceId: string;

  /**
   * 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
   */
  salesInvoiceName?: string;

  /**
   * 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
   */
  accountingDocumentCode: string;

  /**
   * 行号（项目/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
   */
  accountTitle?: string;

  /**
   * 数量
   */
  quantity: number;

  /**
   * 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unit: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount: number;

  /**
   * 业务货币计价的金额
   */
  transactionCurrencyAmount: number;

  /**
   * 含税价格（打印用；如 100.00）
   */
  taxIncludedPrice: number;

  /**
   * 未税价格（打印用；如 88.50）
   */
  untaxedPrice: number;

  /**
   * 税费（打印用；行税额，如 11.50）
   */
  taxAmount: number;

  /**
   * 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
   */
  documentType: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目（行号）
   */
  referenceDocumentItem?: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 销售发票主表 （主表：TaktSalesInvoice）
   */
  salesInvoice?: SalesInvoice;

}


/**
 * SalesInvoiceItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesInvoiceItemQuery
 * @description 对应后端 TaktSalesInvoiceItemQueryDto
 */
export interface SalesInvoiceItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
   */
  salesInvoiceId?: string;

  /**
   * 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
   */
  accountingDocumentCode?: string;

  /**
   * 行号（项目/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 过帐日期（范围查询-开始）
   */
  postingDateStart?: string;

  /**
   * 过帐日期（范围查询-结束）
   */
  postingDateEnd?: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
   */
  accountTitle?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unit?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount?: number;

  /**
   * 业务货币计价的金额
   */
  transactionCurrencyAmount?: number;

  /**
   * 含税价格（打印用；如 100.00）
   */
  taxIncludedPrice?: number;

  /**
   * 未税价格（打印用；如 88.50）
   */
  untaxedPrice?: number;

  /**
   * 税费（打印用；行税额，如 11.50）
   */
  taxAmount?: number;

  /**
   * 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
   */
  documentType?: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目（行号）
   */
  referenceDocumentItem?: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
 * 创建SalesInvoiceItem DTO
 * 对应前端 SalesInvoiceItemCreate
 * @description 对应后端 TaktSalesInvoiceItemCreateDto
 */
export interface SalesInvoiceItemCreate {
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
   * 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
   */
  salesInvoiceId: string;

  /**
   * 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
   */
  accountingDocumentCode: string;

  /**
   * 行号（项目/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
   */
  accountTitle?: string;

  /**
   * 数量
   */
  quantity: number;

  /**
   * 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unit: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount: number;

  /**
   * 业务货币计价的金额
   */
  transactionCurrencyAmount: number;

  /**
   * 含税价格（打印用；如 100.00）
   */
  taxIncludedPrice: number;

  /**
   * 未税价格（打印用；如 88.50）
   */
  untaxedPrice: number;

  /**
   * 税费（打印用；行税额，如 11.50）
   */
  taxAmount: number;

  /**
   * 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
   */
  documentType: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目（行号）
   */
  referenceDocumentItem?: number;

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

}


/**
 * 更新SalesInvoiceItem DTO
 * 继承 TaktSalesInvoiceItemCreateDto，添加 SalesInvoiceItemId 字段
 * 对应前端 SalesInvoiceItemUpdate
 * @description 对应后端 TaktSalesInvoiceItemUpdateDto
 */
export interface SalesInvoiceItemUpdate extends SalesInvoiceItemCreate {
  /**
   * SalesInvoiceItemID（标识要更新的实体）
   */
  salesInvoiceItemId: string;

}


/**
 * SalesInvoiceItem 作废/撤销作废 DTO
 * 对应前端 SalesInvoiceItemObsolete
 * @description 对应后端 TaktSalesInvoiceItemObsoleteDto
 */
export interface SalesInvoiceItemObsolete {
  /**
   * SalesInvoiceItemID
   */
  salesInvoiceItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesInvoiceItem 导入模板行 DTO
 * 对应前端 SalesInvoiceItemTemplate
 * @description 对应后端 TaktSalesInvoiceItemTemplateDto
 */
export interface SalesInvoiceItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
   */
  salesInvoiceId?: string;

  /**
   * 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
   */
  accountingDocumentCode?: string;

  /**
   * 行号（项目/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 过帐日期
   */
  postingDate?: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
   */
  accountTitle?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unit?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount?: number;

  /**
   * 业务货币计价的金额
   */
  transactionCurrencyAmount?: number;

  /**
   * 含税价格（打印用；如 100.00）
   */
  taxIncludedPrice?: number;

  /**
   * 未税价格（打印用；如 88.50）
   */
  untaxedPrice?: number;

  /**
   * 税费（打印用；行税额，如 11.50）
   */
  taxAmount?: number;

  /**
   * 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
   */
  documentType?: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目（行号）
   */
  referenceDocumentItem?: number;

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
 * SalesInvoiceItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesInvoiceItemImport
 * @description 对应后端 TaktSalesInvoiceItemImportDto
 */
export interface SalesInvoiceItemImport {
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
   * 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
   */
  salesInvoiceId?: string;

  /**
   * 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
   */
  accountingDocumentCode?: string;

  /**
   * 行号（项目/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 过帐日期
   */
  postingDate?: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
   */
  accountTitle?: string;

  /**
   * 数量
   */
  quantity?: number;

  /**
   * 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unit?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount?: number;

  /**
   * 业务货币计价的金额
   */
  transactionCurrencyAmount?: number;

  /**
   * 含税价格（打印用；如 100.00）
   */
  taxIncludedPrice?: number;

  /**
   * 未税价格（打印用；如 88.50）
   */
  untaxedPrice?: number;

  /**
   * 税费（打印用；行税额，如 11.50）
   */
  taxAmount?: number;

  /**
   * 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
   */
  documentType?: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目（行号）
   */
  referenceDocumentItem?: number;

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
   * 销售发票（选项 TaktSalesInvoices/options；DictValue=Id）
   */
  salesInvoiceId: string;

  /**
   * 会计凭证编码（冗余，与主表 AccountingDocumentCode 一致）
   */
  accountingDocumentCode: string;

  /**
   * 行号（项目/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料类型（字典 logistics_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenterCode?: string;

  /**
   * 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
   */
  accountTitle?: string;

  /**
   * 数量
   */
  quantity: number;

  /**
   * 单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unit: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount: number;

  /**
   * 业务货币计价的金额
   */
  transactionCurrencyAmount: number;

  /**
   * 含税价格（打印用；如 100.00）
   */
  taxIncludedPrice: number;

  /**
   * 未税价格（打印用；如 88.50）
   */
  untaxedPrice: number;

  /**
   * 税费（打印用；行税额，如 11.50）
   */
  taxAmount: number;

  /**
   * 凭证类型（字典 logistics_accounting_document_type；DictValue=AA/AB/…）
   */
  documentType: string;

  /**
   * 参考凭证
   */
  referenceDocumentCode?: string;

  /**
   * 参考凭证项目（行号）
   */
  referenceDocumentItem?: number;

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

