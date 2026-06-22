// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：work-order.d.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 维护工单实体（由通知单转入或直接创建；执行领料、报工、完工；材料/人工成本汇总于头表 TotalCost 等字段）。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与本模块对接。
 * 对应前端 TaktMaintenanceWorkOrderDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 MaintenanceWorkOrder
 * @description 对应后端 TaktMaintenanceWorkOrderDto
 */
export interface MaintenanceWorkOrder extends ApprovalDtoBase {
  /**
   * MaintenanceWorkOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 维护工单号
   */
  workOrderCode: string;

  /**
   * 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
   */
  maintenanceNotificationId?: string;

  /**
   * 来源维护通知单名称（填充字段）
   */
  maintenanceNotificationName?: string;

  /**
   * 来源通知单号（冗余）
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余）
   */
  equipmentCode: string;

  /**
   * 设备名称（冗余）
   */
  equipmentName: string;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory: number;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  workOrderStatus: number;

  /**
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority: number;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 指派技师（人员编码）
   */
  assignedTechnician?: string;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 维护内容
   */
  maintenanceContent?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 结算成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 结算成本中心名称（填充字段）
   */
  costCenterName?: string;

  /**
   * 结算成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 成本要素ID（序列化为string以避免Javascript精度问题）
   */
  costElementId?: string;

  /**
   * 成本要素名称（填充字段）
   */
  costElementName?: string;

  /**
   * 成本要素编码（冗余）
   */
  costElementCode?: string;

  /**
   * 材料成本合计
   */
  totalMaterialCost: number;

  /**
   * 人工成本合计
   */
  totalLaborCost: number;

  /**
   * 其他成本合计
   */
  totalOtherCost: number;

  /**
   * 总成本
   */
  totalCost: number;

  /**
   * 结算状态（0=未结算，1=部分结算，2=已结算）
   */
  settlementStatus: number;

  /**
   * 结算时间
   */
  settlementTime?: string;

  /**
   * 完工时间
   */
  completedAt?: string;

  /**
   * 验收人（人员编码）
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 下次维护日期
   */
  nextMaintenanceDate?: string;

  /**
   * 维护周期（天）
   */
  maintenanceCycleDays: number;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

  /**
   * 是否已归档至维护履历（字典 sys_yes_no_type；0=否，1=是）
   */
  isHistoryArchived: number;

  /**
   * 来源维护通知单 （主表：TaktMaintenanceNotification）
   */
  maintenanceNotification?: MaintenanceNotification;

  /**
   * 设备（主数据） （主表：TaktEquipment）
   */
  equipment?: Equipment;

  /**
   * 领料明细 （子表：TaktMaintenanceWorkOrderMaterial）
   */
  materials?: MaintenanceWorkOrderMaterial[];

  /**
   * 报工明细 （子表：TaktMaintenanceWorkOrderLabor）
   */
  labors?: MaintenanceWorkOrderLabor[];

}


/**
 * MaintenanceWorkOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaintenanceWorkOrderQuery
 * @description 对应后端 TaktMaintenanceWorkOrderQueryDto
 */
export interface MaintenanceWorkOrderQuery extends TaktPagedQuery {
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
   * 维护工单号
   */
  workOrderCode?: string;

  /**
   * 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
   */
  maintenanceNotificationId?: string;

  /**
   * 来源通知单号（冗余）
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余）
   */
  equipmentCode?: string;

  /**
   * 设备名称（冗余）
   */
  equipmentName?: string;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory?: number;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  workOrderStatus?: number;

  /**
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority?: number;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 指派技师（人员编码）
   */
  assignedTechnician?: string;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

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
   * 实际开始时间（范围查询-开始）
   */
  actualStartTimeStart?: string;

  /**
   * 实际开始时间（范围查询-结束）
   */
  actualStartTimeEnd?: string;

  /**
   * 实际结束时间（范围查询-开始）
   */
  actualEndTimeStart?: string;

  /**
   * 实际结束时间（范围查询-结束）
   */
  actualEndTimeEnd?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 维护内容
   */
  maintenanceContent?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 结算成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 结算成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 成本要素ID（序列化为string以避免Javascript精度问题）
   */
  costElementId?: string;

  /**
   * 成本要素编码（冗余）
   */
  costElementCode?: string;

  /**
   * 材料成本合计
   */
  totalMaterialCost?: number;

  /**
   * 人工成本合计
   */
  totalLaborCost?: number;

  /**
   * 其他成本合计
   */
  totalOtherCost?: number;

  /**
   * 总成本
   */
  totalCost?: number;

  /**
   * 结算状态（0=未结算，1=部分结算，2=已结算）
   */
  settlementStatus?: number;

  /**
   * 结算时间（范围查询-开始）
   */
  settlementTimeStart?: string;

  /**
   * 结算时间（范围查询-结束）
   */
  settlementTimeEnd?: string;

  /**
   * 完工时间（范围查询-开始）
   */
  completedAtStart?: string;

  /**
   * 完工时间（范围查询-结束）
   */
  completedAtEnd?: string;

  /**
   * 验收人（人员编码）
   */
  acceptedBy?: string;

  /**
   * 验收时间（范围查询-开始）
   */
  acceptedAtStart?: string;

  /**
   * 验收时间（范围查询-结束）
   */
  acceptedAtEnd?: string;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult?: number;

  /**
   * 下次维护日期（范围查询-开始）
   */
  nextMaintenanceDateStart?: string;

  /**
   * 下次维护日期（范围查询-结束）
   */
  nextMaintenanceDateEnd?: string;

  /**
   * 维护周期（天）
   */
  maintenanceCycleDays?: number;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

  /**
   * 是否已归档至维护履历（字典 sys_yes_no_type；0=否，1=是）
   */
  isHistoryArchived?: number;

  /**
   * 审批状态（TaktApprovalStatus）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

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
 * 创建MaintenanceWorkOrder DTO
 * 对应前端 MaintenanceWorkOrderCreate
 * @description 对应后端 TaktMaintenanceWorkOrderCreateDto
 */
export interface MaintenanceWorkOrderCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 维护工单号
   */
  workOrderCode: string;

  /**
   * 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
   */
  maintenanceNotificationId?: string;

  /**
   * 来源通知单号（冗余）
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余）
   */
  equipmentCode: string;

  /**
   * 设备名称（冗余）
   */
  equipmentName: string;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory: number;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  workOrderStatus: number;

  /**
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority: number;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 指派技师（人员编码）
   */
  assignedTechnician?: string;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 维护内容
   */
  maintenanceContent?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 结算成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 结算成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 成本要素ID（序列化为string以避免Javascript精度问题）
   */
  costElementId?: string;

  /**
   * 成本要素编码（冗余）
   */
  costElementCode?: string;

  /**
   * 材料成本合计
   */
  totalMaterialCost: number;

  /**
   * 人工成本合计
   */
  totalLaborCost: number;

  /**
   * 其他成本合计
   */
  totalOtherCost: number;

  /**
   * 总成本
   */
  totalCost: number;

  /**
   * 结算状态（0=未结算，1=部分结算，2=已结算）
   */
  settlementStatus: number;

  /**
   * 结算时间
   */
  settlementTime?: string;

  /**
   * 完工时间
   */
  completedAt?: string;

  /**
   * 验收人（人员编码）
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 下次维护日期
   */
  nextMaintenanceDate?: string;

  /**
   * 维护周期（天）
   */
  maintenanceCycleDays: number;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

  /**
   * 是否已归档至维护履历（字典 sys_yes_no_type；0=否，1=是）
   */
  isHistoryArchived: number;

  /**
   * 领料明细（子表，级联保存）
   */
  materials?: MaintenanceWorkOrderMaterialCreate[];

  /**
   * 报工明细（子表，级联保存）
   */
  labors?: MaintenanceWorkOrderLaborCreate[];

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
 * 更新MaintenanceWorkOrder DTO
 * 继承 TaktMaintenanceWorkOrderCreateDto，添加 MaintenanceWorkOrderId 字段
 * 对应前端 MaintenanceWorkOrderUpdate
 * @description 对应后端 TaktMaintenanceWorkOrderUpdateDto
 */
export interface MaintenanceWorkOrderUpdate extends MaintenanceWorkOrderCreate {
  /**
   * MaintenanceWorkOrderID（标识要更新的实体）
   */
  maintenanceWorkOrderId: string;

}


/**
 * MaintenanceWorkOrder 状态更新 DTO
 * 对应前端 MaintenanceWorkOrderStatus
 * @description 对应后端 TaktMaintenanceWorkOrderStatusDto
 */
export interface MaintenanceWorkOrderStatus {
  /**
   * MaintenanceWorkOrderID
   */
  maintenanceWorkOrderId: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  workOrderStatus: number;

}


/**
 * MaintenanceWorkOrder 导入模板行 DTO
 * 对应前端 MaintenanceWorkOrderTemplate
 * @description 对应后端 TaktMaintenanceWorkOrderTemplateDto
 */
export interface MaintenanceWorkOrderTemplate {
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
   * 维护工单号
   */
  workOrderCode?: string;

  /**
   * 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
   */
  maintenanceNotificationId?: string;

  /**
   * 来源通知单号（冗余）
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余）
   */
  equipmentCode?: string;

  /**
   * 设备名称（冗余）
   */
  equipmentName?: string;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory?: number;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  workOrderStatus?: number;

  /**
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority?: number;

  /**
   * 工作中心
   */
  workCenter?: string;

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
 * MaintenanceWorkOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaintenanceWorkOrderImport
 * @description 对应后端 TaktMaintenanceWorkOrderImportDto
 */
export interface MaintenanceWorkOrderImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 维护工单号
   */
  workOrderCode?: string;

  /**
   * 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
   */
  maintenanceNotificationId?: string;

  /**
   * 来源通知单号（冗余）
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余）
   */
  equipmentCode?: string;

  /**
   * 设备名称（冗余）
   */
  equipmentName?: string;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory?: number;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  workOrderStatus?: number;

  /**
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority?: number;

  /**
   * 工作中心
   */
  workCenter?: string;

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
 * MaintenanceWorkOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceWorkOrderExport
 * @description 对应后端 TaktMaintenanceWorkOrderExportDto
 */
export interface MaintenanceWorkOrderExport {
  /**
   * MaintenanceWorkOrderID
   */
  maintenanceWorkOrderId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 维护工单号
   */
  workOrderCode: string;

  /**
   * 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
   */
  maintenanceNotificationId?: string;

  /**
   * 来源通知单号（冗余）
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余）
   */
  equipmentCode: string;

  /**
   * 设备名称（冗余）
   */
  equipmentName: string;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory: number;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  workOrderStatus: number;

  /**
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority: number;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 指派技师（人员编码）
   */
  assignedTechnician?: string;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 维护内容
   */
  maintenanceContent?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 结算成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 结算成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 成本要素ID（序列化为string以避免Javascript精度问题）
   */
  costElementId?: string;

  /**
   * 成本要素编码（冗余）
   */
  costElementCode?: string;

  /**
   * 材料成本合计
   */
  totalMaterialCost: number;

  /**
   * 人工成本合计
   */
  totalLaborCost: number;

  /**
   * 其他成本合计
   */
  totalOtherCost: number;

  /**
   * 总成本
   */
  totalCost: number;

  /**
   * 结算状态（0=未结算，1=部分结算，2=已结算）
   */
  settlementStatus: number;

  /**
   * 结算时间
   */
  settlementTime?: string;

  /**
   * 完工时间
   */
  completedAt?: string;

  /**
   * 验收人（人员编码）
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 下次维护日期
   */
  nextMaintenanceDate?: string;

  /**
   * 维护周期（天）
   */
  maintenanceCycleDays: number;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

  /**
   * 是否已归档至维护履历（字典 sys_yes_no_type；0=否，1=是）
   */
  isHistoryArchived: number;

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

