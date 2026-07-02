// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：production-order.d.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 生产工单实体
 * 对应前端 TaktProductionOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionOrder
 * @description 对应后端 TaktProductionOrderDto
 */
export interface ProductionOrder extends CompanyDtoBase {
  /**
   * ProductionOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productionOrderId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 生产工单类型（字典 logistics_prod_order_type，存 DictValue）
   */
  prodOrderType: string;

  /**
   * 生产工单号
   */
  prodOrderCode: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 生产工单数量
   */
  prodOrderQty: number;

  /**
   * 已生产数量
   */
  producedQty: number;

  /**
   * 计量单位
   */
  unitOfMeasure: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际完成日期
   */
  actualEndDate?: string;

  /**
   * 优先级（字典 sys_priority_level_category；1=最高 2=高 3=普通 4=低）
   */
  priority: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）
   */
  workCenter?: string;

  /**
   * 生产批次
   */
  prodBatch?: string;

  /**
   * 序列号
   */
  serialNo?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode）
   */
  plannedOrderId?: string;

  /**
   * 来源计划订单 名称（填充字段）
   */
  plannedOrderName?: string;

  /**
   * 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode，ExtLabel=PlannedOrderId）
   */
  apsOrderId?: string;

  /**
   * 来源 APS 订单 名称（填充字段）
   */
  apsOrderName?: string;

  /**
   * 计划开工时间
   */
  plannedStartTime: string;

  /**
   * 计划完工时间
   */
  plannedEndTime: string;

  /**
   * 状态（字典 logistics_prod_status：1=进行中，2=已完成）
   */
  productionOrderStatus: number;

  /**
   * 生产工单变更记录列表（外键在子表 TaktProductionOrderChangeLog.ProductionOrderId） （子表：TaktProductionOrderChangeLog）
   */
  changeLogs?: ProductionOrderChangeLog[];

}


/**
 * ProductionOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductionOrderQuery
 * @description 对应后端 TaktProductionOrderQueryDto
 */
export interface ProductionOrderQuery extends TaktPagedQuery {
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
   * 生产工单类型（字典 logistics_prod_order_type，存 DictValue）
   */
  prodOrderType?: string;

  /**
   * 生产工单号
   */
  prodOrderCode?: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 生产工单数量
   */
  prodOrderQty?: number;

  /**
   * 已生产数量
   */
  producedQty?: number;

  /**
   * 计量单位
   */
  unitOfMeasure?: string;

  /**
   * 实际开始日期（范围查询-开始）
   */
  actualStartDateStart?: string;

  /**
   * 实际开始日期（范围查询-结束）
   */
  actualStartDateEnd?: string;

  /**
   * 实际完成日期（范围查询-开始）
   */
  actualEndDateStart?: string;

  /**
   * 实际完成日期（范围查询-结束）
   */
  actualEndDateEnd?: string;

  /**
   * 优先级（字典 sys_priority_level_category；1=最高 2=高 3=普通 4=低）
   */
  priority?: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）
   */
  workCenter?: string;

  /**
   * 生产批次
   */
  prodBatch?: string;

  /**
   * 序列号
   */
  serialNo?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode）
   */
  plannedOrderId?: string;

  /**
   * 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode，ExtLabel=PlannedOrderId）
   */
  apsOrderId?: string;

  /**
   * 计划开工时间（范围查询-开始）
   */
  plannedStartTimeStart?: string;

  /**
   * 计划开工时间（范围查询-结束）
   */
  plannedStartTimeEnd?: string;

  /**
   * 计划完工时间（范围查询-开始）
   */
  plannedEndTimeStart?: string;

  /**
   * 计划完工时间（范围查询-结束）
   */
  plannedEndTimeEnd?: string;

  /**
   * 状态（字典 logistics_prod_status：1=进行中，2=已完成）
   */
  productionOrderStatus?: number;

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
 * 创建ProductionOrder DTO
 * 对应前端 ProductionOrderCreate
 * @description 对应后端 TaktProductionOrderCreateDto
 */
export interface ProductionOrderCreate {
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
   * 生产工单类型（字典 logistics_prod_order_type，存 DictValue）
   */
  prodOrderType: string;

  /**
   * 生产工单号
   */
  prodOrderCode: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 生产工单数量
   */
  prodOrderQty: number;

  /**
   * 已生产数量
   */
  producedQty: number;

  /**
   * 计量单位
   */
  unitOfMeasure: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际完成日期
   */
  actualEndDate?: string;

  /**
   * 优先级（字典 sys_priority_level_category；1=最高 2=高 3=普通 4=低）
   */
  priority: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）
   */
  workCenter?: string;

  /**
   * 生产批次
   */
  prodBatch?: string;

  /**
   * 序列号
   */
  serialNo?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode）
   */
  plannedOrderId?: string;

  /**
   * 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode，ExtLabel=PlannedOrderId）
   */
  apsOrderId?: string;

  /**
   * 计划开工时间
   */
  plannedStartTime: string;

  /**
   * 计划完工时间
   */
  plannedEndTime: string;

  /**
   * 状态（字典 logistics_prod_status：1=进行中，2=已完成）
   */
  productionOrderStatus: number;

  /**
   * 生产工单变更记录列表（外键在子表 TaktProductionOrderChangeLog.ProductionOrderId）（子表，级联保存）
   */
  changeLogs?: ProductionOrderChangeLogCreate[];

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
 * 更新ProductionOrder DTO
 * 继承 TaktProductionOrderCreateDto，添加 ProductionOrderId 字段
 * 对应前端 ProductionOrderUpdate
 * @description 对应后端 TaktProductionOrderUpdateDto
 */
export interface ProductionOrderUpdate extends ProductionOrderCreate {
  /**
   * ProductionOrderID（标识要更新的实体）
   */
  productionOrderId: string;

}


/**
 * ProductionOrder 状态更新 DTO
 * 对应前端 ProductionOrderStatus
 * @description 对应后端 TaktProductionOrderStatusDto
 */
export interface ProductionOrderStatus {
  /**
   * ProductionOrderID
   */
  productionOrderId: string;

  /**
   * 状态（字典 logistics_prod_status：1=进行中，2=已完成）
   */
  productionOrderStatus: number;

}


/**
 * ProductionOrder 导入模板行 DTO
 * 对应前端 ProductionOrderTemplate
 * @description 对应后端 TaktProductionOrderTemplateDto
 */
export interface ProductionOrderTemplate {
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
   * 生产工单类型（字典 logistics_prod_order_type，存 DictValue）
   */
  prodOrderType?: string;

  /**
   * 生产工单号
   */
  prodOrderCode?: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 生产工单数量
   */
  prodOrderQty?: number;

  /**
   * 已生产数量
   */
  producedQty?: number;

  /**
   * 计量单位
   */
  unitOfMeasure?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际完成日期
   */
  actualEndDate?: string;

  /**
   * 优先级（字典 sys_priority_level_category；1=最高 2=高 3=普通 4=低）
   */
  priority?: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）
   */
  workCenter?: string;

  /**
   * 生产批次
   */
  prodBatch?: string;

  /**
   * 序列号
   */
  serialNo?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode）
   */
  plannedOrderId?: string;

  /**
   * 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode，ExtLabel=PlannedOrderId）
   */
  apsOrderId?: string;

  /**
   * 计划开工时间
   */
  plannedStartTime: string;

  /**
   * 计划完工时间
   */
  plannedEndTime: string;

  /**
   * 状态（字典 logistics_prod_status：1=进行中，2=已完成）
   */
  productionOrderStatus?: number;

  /**
   * 生产工单变更记录列表（外键在子表 TaktProductionOrderChangeLog.ProductionOrderId）（子表，级联保存）
   */
  changeLogs?: ProductionOrderChangeLogCreate[];

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
 * ProductionOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductionOrderImport
 * @description 对应后端 TaktProductionOrderImportDto
 */
export interface ProductionOrderImport {
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
   * 生产工单类型（字典 logistics_prod_order_type，存 DictValue）
   */
  prodOrderType?: string;

  /**
   * 生产工单号
   */
  prodOrderCode?: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 生产工单数量
   */
  prodOrderQty?: number;

  /**
   * 已生产数量
   */
  producedQty?: number;

  /**
   * 计量单位
   */
  unitOfMeasure?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际完成日期
   */
  actualEndDate?: string;

  /**
   * 优先级（字典 sys_priority_level_category；1=最高 2=高 3=普通 4=低）
   */
  priority?: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）
   */
  workCenter?: string;

  /**
   * 生产批次
   */
  prodBatch?: string;

  /**
   * 序列号
   */
  serialNo?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode）
   */
  plannedOrderId?: string;

  /**
   * 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode，ExtLabel=PlannedOrderId）
   */
  apsOrderId?: string;

  /**
   * 计划开工时间
   */
  plannedStartTime: string;

  /**
   * 计划完工时间
   */
  plannedEndTime: string;

  /**
   * 状态（字典 logistics_prod_status：1=进行中，2=已完成）
   */
  productionOrderStatus?: number;

  /**
   * 生产工单变更记录列表（外键在子表 TaktProductionOrderChangeLog.ProductionOrderId）（子表，级联保存）
   */
  changeLogs?: ProductionOrderChangeLogCreate[];

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
 * ProductionOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionOrderExport
 * @description 对应后端 TaktProductionOrderExportDto
 */
export interface ProductionOrderExport {
  /**
   * ProductionOrderID
   */
  productionOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 生产工单类型（字典 logistics_prod_order_type，存 DictValue）
   */
  prodOrderType: string;

  /**
   * 生产工单号
   */
  prodOrderCode: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 生产工单数量
   */
  prodOrderQty: number;

  /**
   * 已生产数量
   */
  producedQty: number;

  /**
   * 计量单位
   */
  unitOfMeasure: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际完成日期
   */
  actualEndDate?: string;

  /**
   * 优先级（字典 sys_priority_level_category；1=最高 2=高 3=普通 4=低）
   */
  priority: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）
   */
  workCenter?: string;

  /**
   * 生产批次
   */
  prodBatch?: string;

  /**
   * 序列号
   */
  serialNo?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode）
   */
  plannedOrderId?: string;

  /**
   * 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode，ExtLabel=PlannedOrderId）
   */
  apsOrderId?: string;

  /**
   * 计划开工时间
   */
  plannedStartTime: string;

  /**
   * 计划完工时间
   */
  plannedEndTime: string;

  /**
   * 状态（字典 logistics_prod_status：1=进行中，2=已完成）
   */
  productionOrderStatus: number;

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

