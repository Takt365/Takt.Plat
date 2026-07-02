// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/planning
// 文件名称：planned-order.d.ts
// 创建时间：2026-06-23
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
 * 计划订单（MPS 净需求固化为可排程计划订单，下推 APS_Order）
 * 对应前端 TaktPlannedOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PlannedOrder
 * @description 对应后端 TaktPlannedOrderDto
 */
export interface PlannedOrder extends CompanyDtoBase {
  /**
   * PlannedOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  plannedOrderId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 计划订单编码
   */
  plannedOrderCode: string;

  /**
   * 来源 MPS 头表 ID
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 头表 名称（填充字段）
   */
  masterProductionScheduleName?: string;

  /**
   * 来源 MPS 行 ID
   */
  masterProductionScheduleLineId?: string;

  /**
   * 来源 MPS 行 名称（填充字段）
   */
  masterProductionScheduleLineName?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 计划数量
   */
  plannedQuantity: number;

  /**
   * 计量单位
   */
  unitOfMeasure: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 工艺路线编码（关联 TaktRouting.RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
   */
  orderStatus: number;

}


/**
 * PlannedOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PlannedOrderQuery
 * @description 对应后端 TaktPlannedOrderQueryDto
 */
export interface PlannedOrderQuery extends TaktPagedQuery {
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
   * 计划订单编码
   */
  plannedOrderCode?: string;

  /**
   * 来源 MPS 头表 ID
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 行 ID
   */
  masterProductionScheduleLineId?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 计划数量
   */
  plannedQuantity?: number;

  /**
   * 计量单位
   */
  unitOfMeasure?: string;

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
   * 工艺路线编码（关联 TaktRouting.RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
   */
  orderStatus?: number;

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
 * 创建PlannedOrder DTO
 * 对应前端 PlannedOrderCreate
 * @description 对应后端 TaktPlannedOrderCreateDto
 */
export interface PlannedOrderCreate {
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
   * 计划订单编码
   */
  plannedOrderCode: string;

  /**
   * 来源 MPS 头表 ID
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 行 ID
   */
  masterProductionScheduleLineId?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 计划数量
   */
  plannedQuantity: number;

  /**
   * 计量单位
   */
  unitOfMeasure: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 工艺路线编码（关联 TaktRouting.RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
   */
  orderStatus: number;

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
 * 更新PlannedOrder DTO
 * 继承 TaktPlannedOrderCreateDto，添加 PlannedOrderId 字段
 * 对应前端 PlannedOrderUpdate
 * @description 对应后端 TaktPlannedOrderUpdateDto
 */
export interface PlannedOrderUpdate extends PlannedOrderCreate {
  /**
   * PlannedOrderID（标识要更新的实体）
   */
  plannedOrderId: string;

}


/**
 * PlannedOrder 状态更新 DTO
 * 对应前端 PlannedOrderStatus
 * @description 对应后端 TaktPlannedOrderStatusDto
 */
export interface PlannedOrderStatus {
  /**
   * PlannedOrderID
   */
  plannedOrderId: string;

  /**
   * 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
   */
  orderStatus: number;

}


/**
 * PlannedOrder 导入模板行 DTO
 * 对应前端 PlannedOrderTemplate
 * @description 对应后端 TaktPlannedOrderTemplateDto
 */
export interface PlannedOrderTemplate {
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
   * 计划订单编码
   */
  plannedOrderCode?: string;

  /**
   * 来源 MPS 头表 ID
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 行 ID
   */
  masterProductionScheduleLineId?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 计划数量
   */
  plannedQuantity?: number;

  /**
   * 计量单位
   */
  unitOfMeasure?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 工艺路线编码（关联 TaktRouting.RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
   */
  orderStatus?: number;

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
 * PlannedOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PlannedOrderImport
 * @description 对应后端 TaktPlannedOrderImportDto
 */
export interface PlannedOrderImport {
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
   * 计划订单编码
   */
  plannedOrderCode?: string;

  /**
   * 来源 MPS 头表 ID
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 行 ID
   */
  masterProductionScheduleLineId?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 计划数量
   */
  plannedQuantity?: number;

  /**
   * 计量单位
   */
  unitOfMeasure?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 工艺路线编码（关联 TaktRouting.RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
   */
  orderStatus?: number;

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
 * PlannedOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PlannedOrderExport
 * @description 对应后端 TaktPlannedOrderExportDto
 */
export interface PlannedOrderExport {
  /**
   * PlannedOrderID
   */
  plannedOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 计划订单编码
   */
  plannedOrderCode: string;

  /**
   * 来源 MPS 头表 ID
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 行 ID
   */
  masterProductionScheduleLineId?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 计划数量
   */
  plannedQuantity: number;

  /**
   * 计量单位
   */
  unitOfMeasure: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 工艺路线编码（关联 TaktRouting.RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
   */
  orderStatus: number;

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

