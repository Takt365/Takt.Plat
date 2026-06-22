// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/planning
// 文件名称：master-production-schedule-line.d.ts
// 创建时间：2026-06-22
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
 * 主生产计划 MPS 行（物料 + 时间桶 + ATP）
 * 对应前端 TaktMasterProductionScheduleLineDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MasterProductionScheduleLine
 * @description 对应后端 TaktMasterProductionScheduleLineDto
 */
export interface MasterProductionScheduleLine extends CompanyDtoBase {
  /**
   * MasterProductionScheduleLineID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  masterProductionScheduleLineId: string;

  /**
   * MPS 头表 ID（主子表关系）
   */
  masterProductionScheduleId: string;

  /**
   * MPS 头表 名称（填充字段）
   */
  masterProductionScheduleName?: string;

  /**
   * MPS 编码（冗余）
   */
  mpsCode: string;

  /**
   * 来源 MDS 行 ID（可选）
   */
  masterDemandScheduleLineId?: string;

  /**
   * 来源 MDS 行 名称（填充字段）
   */
  masterDemandScheduleLineName?: string;

  /**
   * 物料编码
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
   * 毛需求数量
   */
  grossRequirement: number;

  /**
   * 预计入库（计划接收）
   */
  scheduledReceipts: number;

  /**
   * 预计可用库存（期初预计库存）
   */
  projectedOnHand: number;

  /**
   * 净需求数量
   */
  netRequirement: number;

  /**
   * 计划订单数量（MPS 产出）
   */
  plannedOrderQuantity: number;

  /**
   * 可承诺量 ATP
   */
  atpQuantity: number;

  /**
   * 计量单位
   */
  unitOfMeasure: string;

}


/**
 * MasterProductionScheduleLine 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MasterProductionScheduleLineQuery
 * @description 对应后端 TaktMasterProductionScheduleLineQueryDto
 */
export interface MasterProductionScheduleLineQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * MPS 头表 ID（主子表关系）
   */
  masterProductionScheduleId?: string;

  /**
   * MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 行 ID（可选）
   */
  masterDemandScheduleLineId?: string;

  /**
   * 物料编码
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
   * 毛需求数量
   */
  grossRequirement?: number;

  /**
   * 预计入库（计划接收）
   */
  scheduledReceipts?: number;

  /**
   * 预计可用库存（期初预计库存）
   */
  projectedOnHand?: number;

  /**
   * 净需求数量
   */
  netRequirement?: number;

  /**
   * 计划订单数量（MPS 产出）
   */
  plannedOrderQuantity?: number;

  /**
   * 可承诺量 ATP
   */
  atpQuantity?: number;

  /**
   * 计量单位
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
 * 创建MasterProductionScheduleLine DTO
 * 对应前端 MasterProductionScheduleLineCreate
 * @description 对应后端 TaktMasterProductionScheduleLineCreateDto
 */
export interface MasterProductionScheduleLineCreate {
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
   * MPS 头表 ID（主子表关系）
   */
  masterProductionScheduleId: string;

  /**
   * MPS 编码（冗余）
   */
  mpsCode: string;

  /**
   * 来源 MDS 行 ID（可选）
   */
  masterDemandScheduleLineId?: string;

  /**
   * 物料编码
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
   * 毛需求数量
   */
  grossRequirement: number;

  /**
   * 预计入库（计划接收）
   */
  scheduledReceipts: number;

  /**
   * 预计可用库存（期初预计库存）
   */
  projectedOnHand: number;

  /**
   * 净需求数量
   */
  netRequirement: number;

  /**
   * 计划订单数量（MPS 产出）
   */
  plannedOrderQuantity: number;

  /**
   * 可承诺量 ATP
   */
  atpQuantity: number;

  /**
   * 计量单位
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
 * 更新MasterProductionScheduleLine DTO
 * 继承 TaktMasterProductionScheduleLineCreateDto，添加 MasterProductionScheduleLineId 字段
 * 对应前端 MasterProductionScheduleLineUpdate
 * @description 对应后端 TaktMasterProductionScheduleLineUpdateDto
 */
export interface MasterProductionScheduleLineUpdate extends MasterProductionScheduleLineCreate {
  /**
   * MasterProductionScheduleLineID（标识要更新的实体）
   */
  masterProductionScheduleLineId: string;

}


/**
 * MasterProductionScheduleLine 导入模板行 DTO
 * 对应前端 MasterProductionScheduleLineTemplate
 * @description 对应后端 TaktMasterProductionScheduleLineTemplateDto
 */
export interface MasterProductionScheduleLineTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * MPS 头表 ID（主子表关系）
   */
  masterProductionScheduleId?: string;

  /**
   * MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 行 ID（可选）
   */
  masterDemandScheduleLineId?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 计量单位
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
 * MasterProductionScheduleLine 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MasterProductionScheduleLineImport
 * @description 对应后端 TaktMasterProductionScheduleLineImportDto
 */
export interface MasterProductionScheduleLineImport {
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
   * MPS 头表 ID（主子表关系）
   */
  masterProductionScheduleId?: string;

  /**
   * MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 行 ID（可选）
   */
  masterDemandScheduleLineId?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 计量单位
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
 * MasterProductionScheduleLine 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MasterProductionScheduleLineExport
 * @description 对应后端 TaktMasterProductionScheduleLineExportDto
 */
export interface MasterProductionScheduleLineExport {
  /**
   * MasterProductionScheduleLineID
   */
  masterProductionScheduleLineId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * MPS 头表 ID（主子表关系）
   */
  masterProductionScheduleId: string;

  /**
   * MPS 编码（冗余）
   */
  mpsCode: string;

  /**
   * 来源 MDS 行 ID（可选）
   */
  masterDemandScheduleLineId?: string;

  /**
   * 物料编码
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
   * 毛需求数量
   */
  grossRequirement: number;

  /**
   * 预计入库（计划接收）
   */
  scheduledReceipts: number;

  /**
   * 预计可用库存（期初预计库存）
   */
  projectedOnHand: number;

  /**
   * 净需求数量
   */
  netRequirement: number;

  /**
   * 计划订单数量（MPS 产出）
   */
  plannedOrderQuantity: number;

  /**
   * 可承诺量 ATP
   */
  atpQuantity: number;

  /**
   * 计量单位
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

