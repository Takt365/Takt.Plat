// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/planning
// 文件名称：sales-plan-item.d.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/planning 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt销售计划明细实体
 * 对应前端 TaktSalesPlanItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesPlanItem
 * @description 对应后端 TaktSalesPlanItemDto
 */
export interface SalesPlanItem extends CompanyDtoBase {
  /**
   * SalesPlanItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanItemId: string;

  /**
   * 销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId: string;

  /**
   * 销售计划名称（填充字段）
   */
  salesPlanName?: string;

  /**
   * 销售计划编码（冗余字段，便于查询）
   */
  salesPlanCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（成品/销售物料，关联 TaktMaterialPlant.MaterialCode）
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
   * 客户编码（行级客户，可选）
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 计划单位
   */
  planUnit: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity: number;

  /**
   * 计划交货日期
   */
  plannedDeliveryDate?: string;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

}


/**
 * SalesPlanItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesPlanItemQuery
 * @description 对应后端 TaktSalesPlanItemQueryDto
 */
export interface SalesPlanItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId?: string;

  /**
   * 销售计划编码（冗余字段，便于查询）
   */
  salesPlanCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（成品/销售物料，关联 TaktMaterialPlant.MaterialCode）
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
   * 客户编码（行级客户，可选）
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 计划单位
   */
  planUnit?: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity?: number;

  /**
   * 计划交货日期（范围查询-开始）
   */
  plannedDeliveryDateStart?: string;

  /**
   * 计划交货日期（范围查询-结束）
   */
  plannedDeliveryDateEnd?: string;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

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
 * 创建SalesPlanItem DTO
 * 对应前端 SalesPlanItemCreate
 * @description 对应后端 TaktSalesPlanItemCreateDto
 */
export interface SalesPlanItemCreate {
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
   * 销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId: string;

  /**
   * 销售计划编码（冗余字段，便于查询）
   */
  salesPlanCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（成品/销售物料，关联 TaktMaterialPlant.MaterialCode）
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
   * 客户编码（行级客户，可选）
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 计划单位
   */
  planUnit: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity: number;

  /**
   * 计划交货日期
   */
  plannedDeliveryDate?: string;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

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
 * 更新SalesPlanItem DTO
 * 继承 TaktSalesPlanItemCreateDto，添加 SalesPlanItemId 字段
 * 对应前端 SalesPlanItemUpdate
 * @description 对应后端 TaktSalesPlanItemUpdateDto
 */
export interface SalesPlanItemUpdate extends SalesPlanItemCreate {
  /**
   * SalesPlanItemID（标识要更新的实体）
   */
  salesPlanItemId: string;

}


/**
 * SalesPlanItem 导入模板行 DTO
 * 对应前端 SalesPlanItemTemplate
 * @description 对应后端 TaktSalesPlanItemTemplateDto
 */
export interface SalesPlanItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId?: string;

  /**
   * 销售计划编码（冗余字段，便于查询）
   */
  salesPlanCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（成品/销售物料，关联 TaktMaterialPlant.MaterialCode）
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
   * 客户编码（行级客户，可选）
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 计划单位
   */
  planUnit?: string;

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
 * SalesPlanItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesPlanItemImport
 * @description 对应后端 TaktSalesPlanItemImportDto
 */
export interface SalesPlanItemImport {
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
   * 销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId?: string;

  /**
   * 销售计划编码（冗余字段，便于查询）
   */
  salesPlanCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（成品/销售物料，关联 TaktMaterialPlant.MaterialCode）
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
   * 客户编码（行级客户，可选）
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 计划单位
   */
  planUnit?: string;

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
 * SalesPlanItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesPlanItemExport
 * @description 对应后端 TaktSalesPlanItemExportDto
 */
export interface SalesPlanItemExport {
  /**
   * SalesPlanItemID
   */
  salesPlanItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId: string;

  /**
   * 销售计划编码（冗余字段，便于查询）
   */
  salesPlanCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（成品/销售物料，关联 TaktMaterialPlant.MaterialCode）
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
   * 客户编码（行级客户，可选）
   */
  customerCode?: string;

  /**
   * 客户名称
   */
  customerName?: string;

  /**
   * 计划单位
   */
  planUnit: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity: number;

  /**
   * 计划交货日期
   */
  plannedDeliveryDate?: string;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

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

