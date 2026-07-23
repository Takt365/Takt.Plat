// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-inquiry-item.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 采购询价明细实体
 * 对应前端 TaktPurchaseInquiryItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInquiryItem
 * @description 对应后端 TaktPurchaseInquiryItemDto
 */
export interface PurchaseInquiryItem extends CompanyDtoBase {
  /**
   * PurchaseInquiryItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInquiryItemId: string;

  /**
   * 采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId: string;

  /**
   * 采购询价 名称（填充字段）
   */
  purchaseInquiryName?: string;

  /**
   * 采购询价编码（冗余，便于查询）
   */
  purchaseInquiryCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
   */
  allocationCategory: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 询价单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  inquiryUnit: string;

  /**
   * 询价数量（基本单位数量，decimal(18,5)）
   */
  inquiryQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 报价单价
   */
  quotedUnitPrice: number;

  /**
   * 含税金额
   */
  taxIncludedAmount: number;

  /**
   * 未税金额
   */
  untaxedAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseInquiryItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInquiryItemQuery
 * @description 对应后端 TaktPurchaseInquiryItemQueryDto
 */
export interface PurchaseInquiryItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 采购询价编码（冗余，便于查询）
   */
  purchaseInquiryCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
   */
  allocationCategory?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 询价单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  inquiryUnit?: string;

  /**
   * 询价数量（基本单位数量，decimal(18,5)）
   */
  inquiryQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 报价单价
   */
  quotedUnitPrice?: number;

  /**
   * 含税金额
   */
  taxIncludedAmount?: number;

  /**
   * 未税金额
   */
  untaxedAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

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
 * 创建PurchaseInquiryItem DTO
 * 对应前端 PurchaseInquiryItemCreate
 * @description 对应后端 TaktPurchaseInquiryItemCreateDto
 */
export interface PurchaseInquiryItemCreate {
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
   * 采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId: string;

  /**
   * 采购询价编码（冗余，便于查询）
   */
  purchaseInquiryCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
   */
  allocationCategory: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 询价单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  inquiryUnit: string;

  /**
   * 询价数量（基本单位数量，decimal(18,5)）
   */
  inquiryQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 报价单价
   */
  quotedUnitPrice: number;

  /**
   * 含税金额
   */
  taxIncludedAmount: number;

  /**
   * 未税金额
   */
  untaxedAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

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
 * 更新PurchaseInquiryItem DTO
 * 继承 TaktPurchaseInquiryItemCreateDto，添加 PurchaseInquiryItemId 字段
 * 对应前端 PurchaseInquiryItemUpdate
 * @description 对应后端 TaktPurchaseInquiryItemUpdateDto
 */
export interface PurchaseInquiryItemUpdate extends PurchaseInquiryItemCreate {
  /**
   * PurchaseInquiryItemID（标识要更新的实体）
   */
  purchaseInquiryItemId: string;

}


/**
 * PurchaseInquiryItem 作废/撤销作废 DTO
 * 对应前端 PurchaseInquiryItemObsolete
 * @description 对应后端 TaktPurchaseInquiryItemObsoleteDto
 */
export interface PurchaseInquiryItemObsolete {
  /**
   * PurchaseInquiryItemID
   */
  purchaseInquiryItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseInquiryItem 导入模板行 DTO
 * 对应前端 PurchaseInquiryItemTemplate
 * @description 对应后端 TaktPurchaseInquiryItemTemplateDto
 */
export interface PurchaseInquiryItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 采购询价编码（冗余，便于查询）
   */
  purchaseInquiryCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
   */
  allocationCategory?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 询价单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  inquiryUnit?: string;

  /**
   * 询价数量（基本单位数量，decimal(18,5)）
   */
  inquiryQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 报价单价
   */
  quotedUnitPrice?: number;

  /**
   * 含税金额
   */
  taxIncludedAmount?: number;

  /**
   * 未税金额
   */
  untaxedAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

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
 * PurchaseInquiryItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInquiryItemImport
 * @description 对应后端 TaktPurchaseInquiryItemImportDto
 */
export interface PurchaseInquiryItemImport {
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
   * 采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 采购询价编码（冗余，便于查询）
   */
  purchaseInquiryCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
   */
  allocationCategory?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 询价单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  inquiryUnit?: string;

  /**
   * 询价数量（基本单位数量，decimal(18,5)）
   */
  inquiryQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 报价单价
   */
  quotedUnitPrice?: number;

  /**
   * 含税金额
   */
  taxIncludedAmount?: number;

  /**
   * 未税金额
   */
  untaxedAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

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
 * PurchaseInquiryItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInquiryItemExport
 * @description 对应后端 TaktPurchaseInquiryItemExportDto
 */
export interface PurchaseInquiryItemExport {
  /**
   * PurchaseInquiryItemID
   */
  purchaseInquiryItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
   */
  purchaseInquiryId: string;

  /**
   * 采购询价编码（冗余，便于查询）
   */
  purchaseInquiryCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category；A=资产，K=成本中心，F=订单）
   */
  allocationCategory: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 询价单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  inquiryUnit: string;

  /**
   * 询价数量（基本单位数量，decimal(18,5)）
   */
  inquiryQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 报价单价
   */
  quotedUnitPrice: number;

  /**
   * 含税金额
   */
  taxIncludedAmount: number;

  /**
   * 未税金额
   */
  untaxedAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

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

