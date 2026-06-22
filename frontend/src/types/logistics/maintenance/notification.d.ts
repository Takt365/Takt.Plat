// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：notification.d.ts
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
 * 维护通知单实体（流程起点：发现异常 → 开通知单 → 转/建维护工单）。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与本模块对接。
 * 对应前端 TaktMaintenanceNotificationDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 MaintenanceNotification
 * @description 对应后端 TaktMaintenanceNotificationDto
 */
export interface MaintenanceNotification extends ApprovalDtoBase {
  /**
   * MaintenanceNotificationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  maintenanceNotificationId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号
   */
  notificationCode: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余，便于查询）
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
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority: number;

  /**
   * 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
   */
  notificationStatus: number;

  /**
   * 异常/故障描述
   */
  faultDescription: string;

  /**
   * 发现时间
   */
  discoveredAt: string;

  /**
   * 故障开始时间
   */
  breakdownStartTime?: string;

  /**
   * 故障结束时间
   */
  breakdownEndTime?: string;

  /**
   * 报告人（人员编码）
   */
  reportedBy?: string;

  /**
   * 责任成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 责任成本中心名称（填充字段）
   */
  costCenterName?: string;

  /**
   * 责任成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 关联维护工单名称（填充字段）
   */
  maintenanceWorkOrderName?: string;

  /**
   * 关联维护工单号（冗余）
   */
  maintenanceWorkOrderCode?: string;

  /**
   * 通知图片（JSON格式，存储图片URL列表）
   */
  notificationImages?: string;

  /**
   * 设备（主数据） （主表：TaktEquipment）
   */
  equipment?: Equipment;

  /**
   * 关联维护工单 （主表：TaktMaintenanceWorkOrder）
   */
  maintenanceWorkOrder?: MaintenanceWorkOrder;

}


/**
 * MaintenanceNotification 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaintenanceNotificationQuery
 * @description 对应后端 TaktMaintenanceNotificationQueryDto
 */
export interface MaintenanceNotificationQuery extends TaktPagedQuery {
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
   * 通知单号
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余，便于查询）
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
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority?: number;

  /**
   * 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
   */
  notificationStatus?: number;

  /**
   * 异常/故障描述
   */
  faultDescription?: string;

  /**
   * 发现时间（范围查询-开始）
   */
  discoveredAtStart?: string;

  /**
   * 发现时间（范围查询-结束）
   */
  discoveredAtEnd?: string;

  /**
   * 故障开始时间（范围查询-开始）
   */
  breakdownStartTimeStart?: string;

  /**
   * 故障开始时间（范围查询-结束）
   */
  breakdownStartTimeEnd?: string;

  /**
   * 故障结束时间（范围查询-开始）
   */
  breakdownEndTimeStart?: string;

  /**
   * 故障结束时间（范围查询-结束）
   */
  breakdownEndTimeEnd?: string;

  /**
   * 报告人（人员编码）
   */
  reportedBy?: string;

  /**
   * 责任成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 责任成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 关联维护工单号（冗余）
   */
  maintenanceWorkOrderCode?: string;

  /**
   * 通知图片（JSON格式，存储图片URL列表）
   */
  notificationImages?: string;

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
 * 创建MaintenanceNotification DTO
 * 对应前端 MaintenanceNotificationCreate
 * @description 对应后端 TaktMaintenanceNotificationCreateDto
 */
export interface MaintenanceNotificationCreate {
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
   * 通知单号
   */
  notificationCode: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余，便于查询）
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
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority: number;

  /**
   * 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
   */
  notificationStatus: number;

  /**
   * 异常/故障描述
   */
  faultDescription: string;

  /**
   * 发现时间
   */
  discoveredAt: string;

  /**
   * 故障开始时间
   */
  breakdownStartTime?: string;

  /**
   * 故障结束时间
   */
  breakdownEndTime?: string;

  /**
   * 报告人（人员编码）
   */
  reportedBy?: string;

  /**
   * 责任成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 责任成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 关联维护工单号（冗余）
   */
  maintenanceWorkOrderCode?: string;

  /**
   * 通知图片（JSON格式，存储图片URL列表）
   */
  notificationImages?: string;

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
 * 更新MaintenanceNotification DTO
 * 继承 TaktMaintenanceNotificationCreateDto，添加 MaintenanceNotificationId 字段
 * 对应前端 MaintenanceNotificationUpdate
 * @description 对应后端 TaktMaintenanceNotificationUpdateDto
 */
export interface MaintenanceNotificationUpdate extends MaintenanceNotificationCreate {
  /**
   * MaintenanceNotificationID（标识要更新的实体）
   */
  maintenanceNotificationId: string;

}


/**
 * MaintenanceNotification 状态更新 DTO
 * 对应前端 MaintenanceNotificationStatus
 * @description 对应后端 TaktMaintenanceNotificationStatusDto
 */
export interface MaintenanceNotificationStatus {
  /**
   * MaintenanceNotificationID
   */
  maintenanceNotificationId: string;

  /**
   * 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
   */
  notificationStatus: number;

}


/**
 * MaintenanceNotification 导入模板行 DTO
 * 对应前端 MaintenanceNotificationTemplate
 * @description 对应后端 TaktMaintenanceNotificationTemplateDto
 */
export interface MaintenanceNotificationTemplate {
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
   * 通知单号
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余，便于查询）
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
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority?: number;

  /**
   * 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
   */
  notificationStatus?: number;

  /**
   * 异常/故障描述
   */
  faultDescription?: string;

  /**
   * 报告人（人员编码）
   */
  reportedBy?: string;

  /**
   * 责任成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 责任成本中心编码（冗余）
   */
  costCenterCode?: string;

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
 * MaintenanceNotification 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaintenanceNotificationImport
 * @description 对应后端 TaktMaintenanceNotificationImportDto
 */
export interface MaintenanceNotificationImport {
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
   * 通知单号
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余，便于查询）
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
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority?: number;

  /**
   * 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
   */
  notificationStatus?: number;

  /**
   * 异常/故障描述
   */
  faultDescription?: string;

  /**
   * 报告人（人员编码）
   */
  reportedBy?: string;

  /**
   * 责任成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 责任成本中心编码（冗余）
   */
  costCenterCode?: string;

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
 * MaintenanceNotification 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceNotificationExport
 * @description 对应后端 TaktMaintenanceNotificationExportDto
 */
export interface MaintenanceNotificationExport {
  /**
   * MaintenanceNotificationID
   */
  maintenanceNotificationId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号
   */
  notificationCode: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余，便于查询）
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
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority: number;

  /**
   * 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
   */
  notificationStatus: number;

  /**
   * 异常/故障描述
   */
  faultDescription: string;

  /**
   * 发现时间
   */
  discoveredAt: string;

  /**
   * 故障开始时间
   */
  breakdownStartTime?: string;

  /**
   * 故障结束时间
   */
  breakdownEndTime?: string;

  /**
   * 报告人（人员编码）
   */
  reportedBy?: string;

  /**
   * 责任成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 责任成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 关联维护工单号（冗余）
   */
  maintenanceWorkOrderCode?: string;

  /**
   * 通知图片（JSON格式，存储图片URL列表）
   */
  notificationImages?: string;

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

