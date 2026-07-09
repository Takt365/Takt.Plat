// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material-document-item.d.ts
// 创建时间：2026-07-09
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
 * Takt物料凭证行项目实体
 * 对应前端 TaktMaterialDocumentItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialDocumentItem
 * @description 对应后端 TaktMaterialDocumentItemDto
 */
export interface MaterialDocumentItem extends CompanyDtoBase {
  /**
   * MaterialDocumentItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialDocumentItemId: string;

  /**
   * 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
   */
  materialDocumentId: string;

  /**
   * 物料凭证 名称（填充字段）
   */
  materialDocumentName?: string;

  /**
   * 物料凭证号（冗余）
   */
  materialDocumentCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 移动类型（字典 logistics_movement_type，如 101=收货）
   */
  movementType: string;

  /**
   * 过账日期
   */
  postingDate: string;

  /**
   * 数量（基本单位数量，出库为负由移动类型决定）
   */
  quantity: number;

  /**
   * 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
   */
  specialStock?: string;

  /**
   * 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 生产订单
   */
  productionOrderCode?: string;

  /**
   * 项目编号（WBS 元素）
   */
  projectCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount: number;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 收货/发货单编号
   */
  referenceDocumentCode?: string;

  /**
   * 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
   */
  customerCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 物料凭证主表 （主表：TaktMaterialDocument）
   */
  materialTransaction?: MaterialDocument;

}


/**
 * MaterialDocumentItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialDocumentItemQuery
 * @description 对应后端 TaktMaterialDocumentItemQueryDto
 */
export interface MaterialDocumentItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
   */
  materialDocumentId?: string;

  /**
   * 物料凭证号（冗余）
   */
  materialDocumentCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 移动类型（字典 logistics_movement_type，如 101=收货）
   */
  movementType?: string;

  /**
   * 过账日期（范围查询-开始）
   */
  postingDateStart?: string;

  /**
   * 过账日期（范围查询-结束）
   */
  postingDateEnd?: string;

  /**
   * 数量（基本单位数量，出库为负由移动类型决定）
   */
  quantity?: number;

  /**
   * 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
   */
  specialStock?: string;

  /**
   * 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 生产订单
   */
  productionOrderCode?: string;

  /**
   * 项目编号（WBS 元素）
   */
  projectCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount?: number;

  /**
   * 凭证日期（范围查询-开始）
   */
  documentDateStart?: string;

  /**
   * 凭证日期（范围查询-结束）
   */
  documentDateEnd?: string;

  /**
   * 收货/发货单编号
   */
  referenceDocumentCode?: string;

  /**
   * 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
   */
  customerCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
 * 创建MaterialDocumentItem DTO
 * 对应前端 MaterialDocumentItemCreate
 * @description 对应后端 TaktMaterialDocumentItemCreateDto
 */
export interface MaterialDocumentItemCreate {
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
   * 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
   */
  materialDocumentId: string;

  /**
   * 物料凭证号（冗余）
   */
  materialDocumentCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 移动类型（字典 logistics_movement_type，如 101=收货）
   */
  movementType: string;

  /**
   * 过账日期
   */
  postingDate: string;

  /**
   * 数量（基本单位数量，出库为负由移动类型决定）
   */
  quantity: number;

  /**
   * 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
   */
  specialStock?: string;

  /**
   * 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 生产订单
   */
  productionOrderCode?: string;

  /**
   * 项目编号（WBS 元素）
   */
  projectCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount: number;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 收货/发货单编号
   */
  referenceDocumentCode?: string;

  /**
   * 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
   */
  customerCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
 * 更新MaterialDocumentItem DTO
 * 继承 TaktMaterialDocumentItemCreateDto，添加 MaterialDocumentItemId 字段
 * 对应前端 MaterialDocumentItemUpdate
 * @description 对应后端 TaktMaterialDocumentItemUpdateDto
 */
export interface MaterialDocumentItemUpdate extends MaterialDocumentItemCreate {
  /**
   * MaterialDocumentItemID（标识要更新的实体）
   */
  materialDocumentItemId: string;

}


/**
 * MaterialDocumentItem 作废/撤销作废 DTO
 * 对应前端 MaterialDocumentItemObsolete
 * @description 对应后端 TaktMaterialDocumentItemObsoleteDto
 */
export interface MaterialDocumentItemObsolete {
  /**
   * MaterialDocumentItemID
   */
  materialDocumentItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * MaterialDocumentItem 导入模板行 DTO
 * 对应前端 MaterialDocumentItemTemplate
 * @description 对应后端 TaktMaterialDocumentItemTemplateDto
 */
export interface MaterialDocumentItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
   */
  materialDocumentId?: string;

  /**
   * 物料凭证号（冗余）
   */
  materialDocumentCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 移动类型（字典 logistics_movement_type，如 101=收货）
   */
  movementType?: string;

  /**
   * 过账日期
   */
  postingDate?: string;

  /**
   * 数量（基本单位数量，出库为负由移动类型决定）
   */
  quantity?: number;

  /**
   * 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
   */
  specialStock?: string;

  /**
   * 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 生产订单
   */
  productionOrderCode?: string;

  /**
   * 项目编号（WBS 元素）
   */
  projectCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount?: number;

  /**
   * 凭证日期
   */
  documentDate?: string;

  /**
   * 收货/发货单编号
   */
  referenceDocumentCode?: string;

  /**
   * 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
   */
  customerCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
 * MaterialDocumentItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialDocumentItemImport
 * @description 对应后端 TaktMaterialDocumentItemImportDto
 */
export interface MaterialDocumentItemImport {
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
   * 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
   */
  materialDocumentId?: string;

  /**
   * 物料凭证号（冗余）
   */
  materialDocumentCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 移动类型（字典 logistics_movement_type，如 101=收货）
   */
  movementType?: string;

  /**
   * 过账日期
   */
  postingDate?: string;

  /**
   * 数量（基本单位数量，出库为负由移动类型决定）
   */
  quantity?: number;

  /**
   * 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
   */
  specialStock?: string;

  /**
   * 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 生产订单
   */
  productionOrderCode?: string;

  /**
   * 项目编号（WBS 元素）
   */
  projectCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount?: number;

  /**
   * 凭证日期
   */
  documentDate?: string;

  /**
   * 收货/发货单编号
   */
  referenceDocumentCode?: string;

  /**
   * 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
   */
  customerCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
 * MaterialDocumentItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialDocumentItemExport
 * @description 对应后端 TaktMaterialDocumentItemExportDto
 */
export interface MaterialDocumentItemExport {
  /**
   * MaterialDocumentItemID
   */
  materialDocumentItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
   */
  materialDocumentId: string;

  /**
   * 物料凭证号（冗余）
   */
  materialDocumentCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 移动类型（字典 logistics_movement_type，如 101=收货）
   */
  movementType: string;

  /**
   * 过账日期
   */
  postingDate: string;

  /**
   * 数量（基本单位数量，出库为负由移动类型决定）
   */
  quantity: number;

  /**
   * 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
   */
  specialStock?: string;

  /**
   * 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
   */
  purchaseOrderCode?: string;

  /**
   * 生产订单
   */
  productionOrderCode?: string;

  /**
   * 项目编号（WBS 元素）
   */
  projectCode?: string;

  /**
   * 本位币金额
   */
  localCurrencyAmount: number;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 收货/发货单编号
   */
  referenceDocumentCode?: string;

  /**
   * 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
   */
  customerCode?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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

