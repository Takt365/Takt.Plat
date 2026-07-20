// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/mds
// 文件名称：master-demand-schedule-line.d.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mds 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 主需求计划 MDS 行（物料 + 时间桶 + 需求来源）
 * 对应前端 TaktMasterDemandScheduleLineDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MasterDemandScheduleLine
 * @description 对应后端 TaktMasterDemandScheduleLineDto
 */
export interface MasterDemandScheduleLine extends CompanyDtoBase {
  /**
   * MasterDemandScheduleLineID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  masterDemandScheduleLineId: string;

  /**
   * MDS 头表 ID（主子表关系）
   */
  masterDemandScheduleId: string;

  /**
   * MDS 头表 名称（填充字段）
   */
  masterDemandScheduleName?: string;

  /**
   * MDS 编码（冗余）
   */
  mdsCode: string;

  /**
   * 需求来源（字典 mds_demand_source_type；0=销售订单，1=预测，2=手工）
   */
  demandSourceType: number;

  /**
   * 来源销售订单 ID（可选）
   */
  salesOrderId?: string;

  /**
   * 来源销售订单 名称（填充字段）
   */
  salesOrderName?: string;

  /**
   * 来源销售订单行号（可选；与 SalesOrderId 成对）
   */
  salesOrderLineNumber?: number;

  /**
   * 来源销售预测 ID（可选；预测/计划类需求）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测 名称（填充字段）
   */
  salesForecastName?: string;

  /**
   * 来源销售预测行号（可选；与 SalesForecastId 成对）
   */
  salesForecastLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 时间桶开始
   */
  bucketStart: string;

  /**
   * 时间桶结束
   */
  bucketEnd: string;

  /**
   * 需求数量（基本单位）
   */
  demandQuantity: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

}


/**
 * MasterDemandScheduleLine 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MasterDemandScheduleLineQuery
 * @description 对应后端 TaktMasterDemandScheduleLineQueryDto
 */
export interface MasterDemandScheduleLineQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * MDS 头表 ID（主子表关系）
   */
  masterDemandScheduleId?: string;

  /**
   * MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 需求来源（字典 mds_demand_source_type；0=销售订单，1=预测，2=手工）
   */
  demandSourceType?: number;

  /**
   * 来源销售订单 ID（可选）
   */
  salesOrderId?: string;

  /**
   * 来源销售订单行号（可选；与 SalesOrderId 成对）
   */
  salesOrderLineNumber?: number;

  /**
   * 来源销售预测 ID（可选；预测/计划类需求）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测行号（可选；与 SalesForecastId 成对）
   */
  salesForecastLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 时间桶开始（范围查询-开始）
   */
  bucketStartStart?: string;

  /**
   * 时间桶开始（范围查询-结束）
   */
  bucketStartEnd?: string;

  /**
   * 时间桶结束（范围查询-开始）
   */
  bucketEndStart?: string;

  /**
   * 时间桶结束（范围查询-结束）
   */
  bucketEndEnd?: string;

  /**
   * 需求数量（基本单位）
   */
  demandQuantity?: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

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
 * 创建MasterDemandScheduleLine DTO
 * 对应前端 MasterDemandScheduleLineCreate
 * @description 对应后端 TaktMasterDemandScheduleLineCreateDto
 */
export interface MasterDemandScheduleLineCreate {
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
   * MDS 头表 ID（主子表关系）
   */
  masterDemandScheduleId: string;

  /**
   * MDS 编码（冗余）
   */
  mdsCode: string;

  /**
   * 需求来源（字典 mds_demand_source_type；0=销售订单，1=预测，2=手工）
   */
  demandSourceType: number;

  /**
   * 来源销售订单 ID（可选）
   */
  salesOrderId?: string;

  /**
   * 来源销售订单行号（可选；与 SalesOrderId 成对）
   */
  salesOrderLineNumber?: number;

  /**
   * 来源销售预测 ID（可选；预测/计划类需求）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测行号（可选；与 SalesForecastId 成对）
   */
  salesForecastLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 时间桶开始
   */
  bucketStart: string;

  /**
   * 时间桶结束
   */
  bucketEnd: string;

  /**
   * 需求数量（基本单位）
   */
  demandQuantity: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

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
 * 更新MasterDemandScheduleLine DTO
 * 继承 TaktMasterDemandScheduleLineCreateDto，添加 MasterDemandScheduleLineId 字段
 * 对应前端 MasterDemandScheduleLineUpdate
 * @description 对应后端 TaktMasterDemandScheduleLineUpdateDto
 */
export interface MasterDemandScheduleLineUpdate extends MasterDemandScheduleLineCreate {
  /**
   * MasterDemandScheduleLineID（标识要更新的实体）
   */
  masterDemandScheduleLineId: string;

}


/**
 * MasterDemandScheduleLine 导入模板行 DTO
 * 对应前端 MasterDemandScheduleLineTemplate
 * @description 对应后端 TaktMasterDemandScheduleLineTemplateDto
 */
export interface MasterDemandScheduleLineTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * MDS 头表 ID（主子表关系）
   */
  masterDemandScheduleId?: string;

  /**
   * MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 需求来源（字典 mds_demand_source_type；0=销售订单，1=预测，2=手工）
   */
  demandSourceType?: number;

  /**
   * 来源销售订单 ID（可选）
   */
  salesOrderId?: string;

  /**
   * 来源销售订单行号（可选；与 SalesOrderId 成对）
   */
  salesOrderLineNumber?: number;

  /**
   * 来源销售预测 ID（可选；预测/计划类需求）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测行号（可选；与 SalesForecastId 成对）
   */
  salesForecastLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 时间桶开始
   */
  bucketStart?: string;

  /**
   * 时间桶结束
   */
  bucketEnd?: string;

  /**
   * 需求数量（基本单位）
   */
  demandQuantity?: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

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
 * MasterDemandScheduleLine 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MasterDemandScheduleLineImport
 * @description 对应后端 TaktMasterDemandScheduleLineImportDto
 */
export interface MasterDemandScheduleLineImport {
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
   * MDS 头表 ID（主子表关系）
   */
  masterDemandScheduleId?: string;

  /**
   * MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 需求来源（字典 mds_demand_source_type；0=销售订单，1=预测，2=手工）
   */
  demandSourceType?: number;

  /**
   * 来源销售订单 ID（可选）
   */
  salesOrderId?: string;

  /**
   * 来源销售订单行号（可选；与 SalesOrderId 成对）
   */
  salesOrderLineNumber?: number;

  /**
   * 来源销售预测 ID（可选；预测/计划类需求）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测行号（可选；与 SalesForecastId 成对）
   */
  salesForecastLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 时间桶开始
   */
  bucketStart?: string;

  /**
   * 时间桶结束
   */
  bucketEnd?: string;

  /**
   * 需求数量（基本单位）
   */
  demandQuantity?: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

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
 * MasterDemandScheduleLine 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MasterDemandScheduleLineExport
 * @description 对应后端 TaktMasterDemandScheduleLineExportDto
 */
export interface MasterDemandScheduleLineExport {
  /**
   * MasterDemandScheduleLineID
   */
  masterDemandScheduleLineId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * MDS 头表 ID（主子表关系）
   */
  masterDemandScheduleId: string;

  /**
   * MDS 编码（冗余）
   */
  mdsCode: string;

  /**
   * 需求来源（字典 mds_demand_source_type；0=销售订单，1=预测，2=手工）
   */
  demandSourceType: number;

  /**
   * 来源销售订单 ID（可选）
   */
  salesOrderId?: string;

  /**
   * 来源销售订单行号（可选；与 SalesOrderId 成对）
   */
  salesOrderLineNumber?: number;

  /**
   * 来源销售预测 ID（可选；预测/计划类需求）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测行号（可选；与 SalesForecastId 成对）
   */
  salesForecastLineNumber?: number;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 时间桶开始
   */
  bucketStart: string;

  /**
   * 时间桶结束
   */
  bucketEnd: string;

  /**
   * 需求数量（基本单位）
   */
  demandQuantity: number;

  /**
   * 计量单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

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

