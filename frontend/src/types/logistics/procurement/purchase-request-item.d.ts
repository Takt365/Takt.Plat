// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-request-item.d.ts
// 创建时间：2026-06-30
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
 * Takt采购申请明细实体
 * 对应前端 TaktPurchaseRequestItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseRequestItem
 * @description 对应后端 TaktPurchaseRequestItemDto
 */
export interface PurchaseRequestItem extends CompanyDtoBase {
  /**
   * PurchaseRequestItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseRequestItemId: string;

  /**
   * 采购申请 ID（关联 TaktPurchaseRequest.Id，选项 TaktPurchaseRequests/options）
   */
  purchaseRequestId: string;

  /**
   * 采购申请 名称（填充字段）
   */
  purchaseRequestName?: string;

  /**
   * 采购申请编码（冗余字段，便于查询）
   */
  purchaseRequestCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 申请单位
   */
  requestUnit: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 预计单价（精确到分，存储为整数，单位为分）
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额（精确到分，存储为整数，单位为分）
   */
  estimatedAmount: number;

  /**
   * 参考供应商编码
   */
  referenceSupplierCode?: string;

  /**
   * 参考供应商名称
   */
  referenceSupplierName?: string;

}


/**
 * PurchaseRequestItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseRequestItemQuery
 * @description 对应后端 TaktPurchaseRequestItemQueryDto
 */
export interface PurchaseRequestItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 采购申请 ID（关联 TaktPurchaseRequest.Id，选项 TaktPurchaseRequests/options）
   */
  purchaseRequestId?: string;

  /**
   * 采购申请编码（冗余字段，便于查询）
   */
  purchaseRequestCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

  /**
   * 物料编码
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
   * 申请单位
   */
  requestUnit?: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity?: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 预计单价（精确到分，存储为整数，单位为分）
   */
  estimatedUnitPrice?: number;

  /**
   * 预计金额（精确到分，存储为整数，单位为分）
   */
  estimatedAmount?: number;

  /**
   * 参考供应商编码
   */
  referenceSupplierCode?: string;

  /**
   * 参考供应商名称
   */
  referenceSupplierName?: string;

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
 * 创建PurchaseRequestItem DTO
 * 对应前端 PurchaseRequestItemCreate
 * @description 对应后端 TaktPurchaseRequestItemCreateDto
 */
export interface PurchaseRequestItemCreate {
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
   * 采购申请 ID（关联 TaktPurchaseRequest.Id，选项 TaktPurchaseRequests/options）
   */
  purchaseRequestId: string;

  /**
   * 采购申请编码（冗余字段，便于查询）
   */
  purchaseRequestCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 申请单位
   */
  requestUnit: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 预计单价（精确到分，存储为整数，单位为分）
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额（精确到分，存储为整数，单位为分）
   */
  estimatedAmount: number;

  /**
   * 参考供应商编码
   */
  referenceSupplierCode?: string;

  /**
   * 参考供应商名称
   */
  referenceSupplierName?: string;

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
 * 更新PurchaseRequestItem DTO
 * 继承 TaktPurchaseRequestItemCreateDto，添加 PurchaseRequestItemId 字段
 * 对应前端 PurchaseRequestItemUpdate
 * @description 对应后端 TaktPurchaseRequestItemUpdateDto
 */
export interface PurchaseRequestItemUpdate extends PurchaseRequestItemCreate {
  /**
   * PurchaseRequestItemID（标识要更新的实体）
   */
  purchaseRequestItemId: string;

}


/**
 * PurchaseRequestItem 导入模板行 DTO
 * 对应前端 PurchaseRequestItemTemplate
 * @description 对应后端 TaktPurchaseRequestItemTemplateDto
 */
export interface PurchaseRequestItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 采购申请 ID（关联 TaktPurchaseRequest.Id，选项 TaktPurchaseRequests/options）
   */
  purchaseRequestId?: string;

  /**
   * 采购申请编码（冗余字段，便于查询）
   */
  purchaseRequestCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

  /**
   * 物料编码
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
   * 申请单位
   */
  requestUnit?: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity?: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 预计单价（精确到分，存储为整数，单位为分）
   */
  estimatedUnitPrice?: number;

  /**
   * 预计金额（精确到分，存储为整数，单位为分）
   */
  estimatedAmount?: number;

  /**
   * 参考供应商编码
   */
  referenceSupplierCode?: string;

  /**
   * 参考供应商名称
   */
  referenceSupplierName?: string;

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
 * PurchaseRequestItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseRequestItemImport
 * @description 对应后端 TaktPurchaseRequestItemImportDto
 */
export interface PurchaseRequestItemImport {
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
   * 采购申请 ID（关联 TaktPurchaseRequest.Id，选项 TaktPurchaseRequests/options）
   */
  purchaseRequestId?: string;

  /**
   * 采购申请编码（冗余字段，便于查询）
   */
  purchaseRequestCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

  /**
   * 物料编码
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
   * 申请单位
   */
  requestUnit?: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity?: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit?: number;

  /**
   * 预计单价（精确到分，存储为整数，单位为分）
   */
  estimatedUnitPrice?: number;

  /**
   * 预计金额（精确到分，存储为整数，单位为分）
   */
  estimatedAmount?: number;

  /**
   * 参考供应商编码
   */
  referenceSupplierCode?: string;

  /**
   * 参考供应商名称
   */
  referenceSupplierName?: string;

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
 * PurchaseRequestItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseRequestItemExport
 * @description 对应后端 TaktPurchaseRequestItemExportDto
 */
export interface PurchaseRequestItemExport {
  /**
   * PurchaseRequestItemID
   */
  purchaseRequestItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购申请 ID（关联 TaktPurchaseRequest.Id，选项 TaktPurchaseRequests/options）
   */
  purchaseRequestId: string;

  /**
   * 采购申请编码（冗余字段，便于查询）
   */
  purchaseRequestCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 申请单位
   */
  requestUnit: string;

  /**
   * 申请数量（基本单位数量）
   */
  requestQuantity: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param：1/100/1000/10000；默认 1000）
   */
  purchasePerUnit: number;

  /**
   * 预计单价（精确到分，存储为整数，单位为分）
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额（精确到分，存储为整数，单位为分）
   */
  estimatedAmount: number;

  /**
   * 参考供应商编码
   */
  referenceSupplierCode?: string;

  /**
   * 参考供应商名称
   */
  referenceSupplierName?: string;

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

