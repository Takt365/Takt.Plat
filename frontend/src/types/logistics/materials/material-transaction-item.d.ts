// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material-transaction-item.d.ts
// 创建时间：2026-06-20
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
 * Takt物料交易明细实体
 * 对应前端 TaktMaterialTransactionItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialTransactionItem
 * @description 对应后端 TaktMaterialTransactionItemDto
 */
export interface MaterialTransactionItem extends CompanyDtoBase {
  /**
   * MaterialTransactionItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialTransactionItemId: string;

  /**
   * 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  materialTransactionId: string;

  /**
   * 物料交易名称（填充字段）
   */
  materialTransactionName?: string;

  /**
   * 物料交易单号（冗余字段，便于查询）
   */
  materialTransactionCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源单号（采购订单、销售订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 来源单行号
   */
  sourceLineNumber?: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 交易单位
   */
  transactionUnit: string;

  /**
   * 交易数量（基本单位数量）
   */
  transactionQuantity: number;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
   */
  locationCode?: string;

  /**
   * 目标仓库编码（移库/调拨时使用）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用）
   */
  targetLocationCode?: string;

  /**
   * 单价
   */
  unitPrice: number;

  /**
   * 行金额
   */
  lineAmount: number;

  /**
   * 物料交易主表 （主表：TaktMaterialTransaction）
   */
  materialTransaction?: MaterialTransaction;

}


/**
 * MaterialTransactionItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialTransactionItemQuery
 * @description 对应后端 TaktMaterialTransactionItemQueryDto
 */
export interface MaterialTransactionItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  materialTransactionId?: string;

  /**
   * 物料交易单号（冗余字段，便于查询）
   */
  materialTransactionCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源单号（采购订单、销售订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 来源单行号
   */
  sourceLineNumber?: number;

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
   * 交易单位
   */
  transactionUnit?: string;

  /**
   * 交易数量（基本单位数量）
   */
  transactionQuantity?: number;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
   */
  locationCode?: string;

  /**
   * 目标仓库编码（移库/调拨时使用）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用）
   */
  targetLocationCode?: string;

  /**
   * 单价
   */
  unitPrice?: number;

  /**
   * 行金额
   */
  lineAmount?: number;

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
 * 创建MaterialTransactionItem DTO
 * 对应前端 MaterialTransactionItemCreate
 * @description 对应后端 TaktMaterialTransactionItemCreateDto
 */
export interface MaterialTransactionItemCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  materialTransactionId: string;

  /**
   * 物料交易单号（冗余字段，便于查询）
   */
  materialTransactionCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源单号（采购订单、销售订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 来源单行号
   */
  sourceLineNumber?: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 交易单位
   */
  transactionUnit: string;

  /**
   * 交易数量（基本单位数量）
   */
  transactionQuantity: number;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
   */
  locationCode?: string;

  /**
   * 目标仓库编码（移库/调拨时使用）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用）
   */
  targetLocationCode?: string;

  /**
   * 单价
   */
  unitPrice: number;

  /**
   * 行金额
   */
  lineAmount: number;

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
 * 更新MaterialTransactionItem DTO
 * 继承 TaktMaterialTransactionItemCreateDto，添加 MaterialTransactionItemId 字段
 * 对应前端 MaterialTransactionItemUpdate
 * @description 对应后端 TaktMaterialTransactionItemUpdateDto
 */
export interface MaterialTransactionItemUpdate extends MaterialTransactionItemCreate {
  /**
   * MaterialTransactionItemID（标识要更新的实体）
   */
  materialTransactionItemId: string;

}


/**
 * MaterialTransactionItem 导入模板行 DTO
 * 对应前端 MaterialTransactionItemTemplate
 * @description 对应后端 TaktMaterialTransactionItemTemplateDto
 */
export interface MaterialTransactionItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  materialTransactionId?: string;

  /**
   * 物料交易单号（冗余字段，便于查询）
   */
  materialTransactionCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源单号（采购订单、销售订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 来源单行号
   */
  sourceLineNumber?: number;

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
   * 交易单位
   */
  transactionUnit?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
   */
  locationCode?: string;

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
 * MaterialTransactionItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialTransactionItemImport
 * @description 对应后端 TaktMaterialTransactionItemImportDto
 */
export interface MaterialTransactionItemImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  materialTransactionId?: string;

  /**
   * 物料交易单号（冗余字段，便于查询）
   */
  materialTransactionCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源单号（采购订单、销售订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 来源单行号
   */
  sourceLineNumber?: number;

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
   * 交易单位
   */
  transactionUnit?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
   */
  locationCode?: string;

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
 * MaterialTransactionItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialTransactionItemExport
 * @description 对应后端 TaktMaterialTransactionItemExportDto
 */
export interface MaterialTransactionItemExport {
  /**
   * MaterialTransactionItemID
   */
  materialTransactionItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  materialTransactionId: string;

  /**
   * 物料交易单号（冗余字段，便于查询）
   */
  materialTransactionCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源单号（采购订单、销售订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 来源单行号
   */
  sourceLineNumber?: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 交易单位
   */
  transactionUnit: string;

  /**
   * 交易数量（基本单位数量）
   */
  transactionQuantity: number;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
   */
  locationCode?: string;

  /**
   * 目标仓库编码（移库/调拨时使用）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用）
   */
  targetLocationCode?: string;

  /**
   * 单价
   */
  unitPrice: number;

  /**
   * 行金额
   */
  lineAmount: number;

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

