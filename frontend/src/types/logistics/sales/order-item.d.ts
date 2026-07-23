// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：order-item.d.ts
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
 * Takt销售订单明细实体
 * 对应前端 TaktSalesOrderItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesOrderItem
 * @description 对应后端 TaktSalesOrderItemDto
 */
export interface SalesOrderItem extends CompanyDtoBase {
  /**
   * SalesOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesOrderItemId: string;

  /**
   * 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
   */
  salesOrderId: string;

  /**
   * 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
   */
  salesOrderName?: string;

  /**
   * 销售订单编码（冗余字段，便于查询）
   */
  salesOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit: number;

  /**
   * 销售单价
   */
  salesUnitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

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
   * 行交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 销售订单主表 （主表：TaktSalesOrder）
   */
  salesOrder?: SalesOrder;

}


/**
 * SalesOrderItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesOrderItemQuery
 * @description 对应后端 TaktSalesOrderItemQueryDto
 */
export interface SalesOrderItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
   */
  salesOrderId?: string;

  /**
   * 销售订单编码（冗余字段，便于查询）
   */
  salesOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

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
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit?: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity?: number;

  /**
   * 已发货数量（基本单位数量）
   */
  shippedQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit?: number;

  /**
   * 销售单价
   */
  salesUnitPrice?: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

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
   * 行交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus?: number;

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
 * 创建SalesOrderItem DTO
 * 对应前端 SalesOrderItemCreate
 * @description 对应后端 TaktSalesOrderItemCreateDto
 */
export interface SalesOrderItemCreate {
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
   * 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
   */
  salesOrderId: string;

  /**
   * 销售订单编码（冗余字段，便于查询）
   */
  salesOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit: number;

  /**
   * 销售单价
   */
  salesUnitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

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
   * 行交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus: number;

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
 * 更新SalesOrderItem DTO
 * 继承 TaktSalesOrderItemCreateDto，添加 SalesOrderItemId 字段
 * 对应前端 SalesOrderItemUpdate
 * @description 对应后端 TaktSalesOrderItemUpdateDto
 */
export interface SalesOrderItemUpdate extends SalesOrderItemCreate {
  /**
   * SalesOrderItemID（标识要更新的实体）
   */
  salesOrderItemId: string;

}


/**
 * SalesOrderItem 状态更新 DTO
 * 对应前端 SalesOrderItemStatus
 * @description 对应后端 TaktSalesOrderItemStatusDto
 */
export interface SalesOrderItemStatus {
  /**
   * SalesOrderItemID
   */
  salesOrderItemId: string;

  /**
   * 行交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus: number;

}


/**
 * SalesOrderItem 作废/撤销作废 DTO
 * 对应前端 SalesOrderItemObsolete
 * @description 对应后端 TaktSalesOrderItemObsoleteDto
 */
export interface SalesOrderItemObsolete {
  /**
   * SalesOrderItemID
   */
  salesOrderItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesOrderItem 导入模板行 DTO
 * 对应前端 SalesOrderItemTemplate
 * @description 对应后端 TaktSalesOrderItemTemplateDto
 */
export interface SalesOrderItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
   */
  salesOrderId?: string;

  /**
   * 销售订单编码（冗余字段，便于查询）
   */
  salesOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

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
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit?: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity?: number;

  /**
   * 已发货数量（基本单位数量）
   */
  shippedQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit?: number;

  /**
   * 销售单价
   */
  salesUnitPrice?: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

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
   * 行交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus?: number;

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
 * SalesOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesOrderItemImport
 * @description 对应后端 TaktSalesOrderItemImportDto
 */
export interface SalesOrderItemImport {
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
   * 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
   */
  salesOrderId?: string;

  /**
   * 销售订单编码（冗余字段，便于查询）
   */
  salesOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

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
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit?: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity?: number;

  /**
   * 已发货数量（基本单位数量）
   */
  shippedQuantity?: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit?: number;

  /**
   * 销售单价
   */
  salesUnitPrice?: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate?: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount?: number;

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
   * 行交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus?: number;

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
 * SalesOrderItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesOrderItemExport
 * @description 对应后端 TaktSalesOrderItemExportDto
 */
export interface SalesOrderItemExport {
  /**
   * SalesOrderItemID
   */
  salesOrderItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售订单（选项 TaktSalesOrders/options；DictValue=Id）
   */
  salesOrderId: string;

  /**
   * 销售订单编码（冗余字段，便于查询）
   */
  salesOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料名称（回填：随物料）
   */
  materialName: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 销售单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  salesUnit: string;

  /**
   * 订购数量（基本单位数量）
   */
  orderQuantity: number;

  /**
   * 已发货数量（基本单位数量）
   */
  shippedQuantity: number;

  /**
   * 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
   */
  salesPerUnit: number;

  /**
   * 销售单价
   */
  salesUnitPrice: number;

  /**
   * 折扣率（0-100，表示折扣百分比）
   */
  discountRate: number;

  /**
   * 折扣金额（精确到分，存储为整数，单位为分）
   */
  discountAmount: number;

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
   * 行交货状态（字典 logistics_delivery_status；0=未交货 1=部分交货 2=全部交货）
   */
  deliveryStatus: number;

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

