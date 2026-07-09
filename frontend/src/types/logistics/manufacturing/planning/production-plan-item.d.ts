// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/planning
// 文件名称：production-plan-item.d.ts
// 创建时间：2026-07-09
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
 * Takt生产计划明细实体
 * 对应前端 TaktProductionPlanItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionPlanItem
 * @description 对应后端 TaktProductionPlanItemDto
 */
export interface ProductionPlanItem extends CompanyDtoBase {
  /**
   * ProductionPlanItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanItemId: string;

  /**
   * 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId: string;

  /**
   * 生产计划名称（填充字段）
   */
  productionPlanName?: string;

  /**
   * 生产计划编码（冗余字段，便于查询）
   */
  productionPlanCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId?: string;

  /**
   * 来源销售计划名称（填充字段）
   */
  salesPlanName?: string;

  /**
   * 来源销售计划编码
   */
  salesPlanCode?: string;

  /**
   * 来源销售计划行号
   */
  salesPlanLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
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
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  planUnit: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity: number;

  /**
   * 计划开工日期
   */
  plannedStartDate?: string;

  /**
   * 计划完工日期
   */
  plannedEndDate?: string;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单位成本
   */
  estimatedUnitCost: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * ProductionPlanItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductionPlanItemQuery
 * @description 对应后端 TaktProductionPlanItemQueryDto
 */
export interface ProductionPlanItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 生产计划编码（冗余字段，便于查询）
   */
  productionPlanCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId?: string;

  /**
   * 来源销售计划编码
   */
  salesPlanCode?: string;

  /**
   * 来源销售计划行号
   */
  salesPlanLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
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
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  planUnit?: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity?: number;

  /**
   * 计划开工日期（范围查询-开始）
   */
  plannedStartDateStart?: string;

  /**
   * 计划开工日期（范围查询-结束）
   */
  plannedStartDateEnd?: string;

  /**
   * 计划完工日期（范围查询-开始）
   */
  plannedEndDateStart?: string;

  /**
   * 计划完工日期（范围查询-结束）
   */
  plannedEndDateEnd?: string;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单位成本
   */
  estimatedUnitCost?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

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
 * 创建ProductionPlanItem DTO
 * 对应前端 ProductionPlanItemCreate
 * @description 对应后端 TaktProductionPlanItemCreateDto
 */
export interface ProductionPlanItemCreate {
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
   * 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId: string;

  /**
   * 生产计划编码（冗余字段，便于查询）
   */
  productionPlanCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId?: string;

  /**
   * 来源销售计划编码
   */
  salesPlanCode?: string;

  /**
   * 来源销售计划行号
   */
  salesPlanLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
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
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  planUnit: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity: number;

  /**
   * 计划开工日期
   */
  plannedStartDate?: string;

  /**
   * 计划完工日期
   */
  plannedEndDate?: string;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单位成本
   */
  estimatedUnitCost: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

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
 * 更新ProductionPlanItem DTO
 * 继承 TaktProductionPlanItemCreateDto，添加 ProductionPlanItemId 字段
 * 对应前端 ProductionPlanItemUpdate
 * @description 对应后端 TaktProductionPlanItemUpdateDto
 */
export interface ProductionPlanItemUpdate extends ProductionPlanItemCreate {
  /**
   * ProductionPlanItemID（标识要更新的实体）
   */
  productionPlanItemId: string;

}


/**
 * ProductionPlanItem 作废/撤销作废 DTO
 * 对应前端 ProductionPlanItemObsolete
 * @description 对应后端 TaktProductionPlanItemObsoleteDto
 */
export interface ProductionPlanItemObsolete {
  /**
   * ProductionPlanItemID
   */
  productionPlanItemId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * ProductionPlanItem 导入模板行 DTO
 * 对应前端 ProductionPlanItemTemplate
 * @description 对应后端 TaktProductionPlanItemTemplateDto
 */
export interface ProductionPlanItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 生产计划编码（冗余字段，便于查询）
   */
  productionPlanCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId?: string;

  /**
   * 来源销售计划编码
   */
  salesPlanCode?: string;

  /**
   * 来源销售计划行号
   */
  salesPlanLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
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
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  planUnit?: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity?: number;

  /**
   * 计划开工日期
   */
  plannedStartDate?: string;

  /**
   * 计划完工日期
   */
  plannedEndDate?: string;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单位成本
   */
  estimatedUnitCost?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

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
 * ProductionPlanItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductionPlanItemImport
 * @description 对应后端 TaktProductionPlanItemImportDto
 */
export interface ProductionPlanItemImport {
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
   * 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 生产计划编码（冗余字段，便于查询）
   */
  productionPlanCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId?: string;

  /**
   * 来源销售计划编码
   */
  salesPlanCode?: string;

  /**
   * 来源销售计划行号
   */
  salesPlanLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
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
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  planUnit?: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity?: number;

  /**
   * 计划开工日期
   */
  plannedStartDate?: string;

  /**
   * 计划完工日期
   */
  plannedEndDate?: string;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单位成本
   */
  estimatedUnitCost?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

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
 * ProductionPlanItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionPlanItemExport
 * @description 对应后端 TaktProductionPlanItemExportDto
 */
export interface ProductionPlanItemExport {
  /**
   * ProductionPlanItemID
   */
  productionPlanItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId: string;

  /**
   * 生产计划编码（冗余字段，便于查询）
   */
  productionPlanCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId?: string;

  /**
   * 来源销售计划编码
   */
  salesPlanCode?: string;

  /**
   * 来源销售计划行号
   */
  salesPlanLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
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
   * 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  planUnit: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity: number;

  /**
   * 计划开工日期
   */
  plannedStartDate?: string;

  /**
   * 计划完工日期
   */
  plannedEndDate?: string;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单位成本
   */
  estimatedUnitCost: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

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

