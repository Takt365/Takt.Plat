// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/scheduling
// 文件名称：production-dispatch.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/scheduling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 生产派工单（Prod_Order → Dispatch → MES 报工）
 * 对应前端 TaktProductionDispatchDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionDispatch
 * @description 对应后端 TaktProductionDispatchDto
 */
export interface ProductionDispatch extends CompanyDtoBase {
  /**
   * ProductionDispatchID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productionDispatchId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 派工单编码
   */
  dispatchCode: string;

  /**
   * 生产工单 ID（关联 TaktProductionOrder）
   */
  productionOrderId: string;

  /**
   * 生产工单 名称（填充字段）
   */
  productionOrderName?: string;

  /**
   * 生产工单号（冗余）
   */
  prodOrderCode: string;

  /**
   * APS 工序排程 ID（可选）
   */
  apsOperationId?: string;

  /**
   * APS 工序排程 名称（填充字段）
   */
  apsOperationName?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 派工数量
   */
  dispatchQuantity: number;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
   */
  dispatchStatus: number;

}


/**
 * ProductionDispatch 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductionDispatchQuery
 * @description 对应后端 TaktProductionDispatchQueryDto
 */
export interface ProductionDispatchQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 派工单编码
   */
  dispatchCode?: string;

  /**
   * 生产工单 ID（关联 TaktProductionOrder）
   */
  productionOrderId?: string;

  /**
   * 生产工单号（冗余）
   */
  prodOrderCode?: string;

  /**
   * APS 工序排程 ID（可选）
   */
  apsOperationId?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 派工数量
   */
  dispatchQuantity?: number;

  /**
   * 计划开始时间（范围查询-开始）
   */
  plannedStartTimeStart?: string;

  /**
   * 计划开始时间（范围查询-结束）
   */
  plannedStartTimeEnd?: string;

  /**
   * 计划结束时间（范围查询-开始）
   */
  plannedEndTimeStart?: string;

  /**
   * 计划结束时间（范围查询-结束）
   */
  plannedEndTimeEnd?: string;

  /**
   * 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
   */
  dispatchStatus?: number;

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
 * 创建ProductionDispatch DTO
 * 对应前端 ProductionDispatchCreate
 * @description 对应后端 TaktProductionDispatchCreateDto
 */
export interface ProductionDispatchCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 派工单编码
   */
  dispatchCode: string;

  /**
   * 生产工单 ID（关联 TaktProductionOrder）
   */
  productionOrderId: string;

  /**
   * 生产工单号（冗余）
   */
  prodOrderCode: string;

  /**
   * APS 工序排程 ID（可选）
   */
  apsOperationId?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 派工数量
   */
  dispatchQuantity: number;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
   */
  dispatchStatus: number;

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
 * 更新ProductionDispatch DTO
 * 继承 TaktProductionDispatchCreateDto，添加 ProductionDispatchId 字段
 * 对应前端 ProductionDispatchUpdate
 * @description 对应后端 TaktProductionDispatchUpdateDto
 */
export interface ProductionDispatchUpdate extends ProductionDispatchCreate {
  /**
   * ProductionDispatchID（标识要更新的实体）
   */
  productionDispatchId: string;

}


/**
 * ProductionDispatch 状态更新 DTO
 * 对应前端 ProductionDispatchStatus
 * @description 对应后端 TaktProductionDispatchStatusDto
 */
export interface ProductionDispatchStatus {
  /**
   * ProductionDispatchID
   */
  productionDispatchId: string;

  /**
   * 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
   */
  dispatchStatus: number;

}


/**
 * ProductionDispatch 导入模板行 DTO
 * 对应前端 ProductionDispatchTemplate
 * @description 对应后端 TaktProductionDispatchTemplateDto
 */
export interface ProductionDispatchTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 派工单编码
   */
  dispatchCode?: string;

  /**
   * 生产工单 ID（关联 TaktProductionOrder）
   */
  productionOrderId?: string;

  /**
   * 生产工单号（冗余）
   */
  prodOrderCode?: string;

  /**
   * APS 工序排程 ID（可选）
   */
  apsOperationId?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 派工数量
   */
  dispatchQuantity?: number;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
   */
  dispatchStatus?: number;

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
 * ProductionDispatch 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductionDispatchImport
 * @description 对应后端 TaktProductionDispatchImportDto
 */
export interface ProductionDispatchImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 派工单编码
   */
  dispatchCode?: string;

  /**
   * 生产工单 ID（关联 TaktProductionOrder）
   */
  productionOrderId?: string;

  /**
   * 生产工单号（冗余）
   */
  prodOrderCode?: string;

  /**
   * APS 工序排程 ID（可选）
   */
  apsOperationId?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 派工数量
   */
  dispatchQuantity?: number;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
   */
  dispatchStatus?: number;

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
 * ProductionDispatch 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionDispatchExport
 * @description 对应后端 TaktProductionDispatchExportDto
 */
export interface ProductionDispatchExport {
  /**
   * ProductionDispatchID
   */
  productionDispatchId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 派工单编码
   */
  dispatchCode: string;

  /**
   * 生产工单 ID（关联 TaktProductionOrder）
   */
  productionOrderId: string;

  /**
   * 生产工单号（冗余）
   */
  prodOrderCode: string;

  /**
   * APS 工序排程 ID（可选）
   */
  apsOperationId?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 派工数量
   */
  dispatchQuantity: number;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
   */
  dispatchStatus: number;

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

