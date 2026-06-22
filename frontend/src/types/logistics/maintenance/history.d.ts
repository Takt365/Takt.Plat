// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：history.d.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 设备维护履历实体（TaktEquipment 子表；数据来源于 TaktMaintenanceWorkOrder 完工归档，只读展示）
 * 对应前端 TaktMaintenanceHistoryDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaintenanceHistory
 * @description 对应后端 TaktMaintenanceHistoryDto
 */
export interface MaintenanceHistory extends CompanyDtoBase {
  /**
   * MaintenanceHistoryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  maintenanceHistoryId: string;

  /**
   * 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 来源维护工单名称（填充字段）
   */
  maintenanceWorkOrderName?: string;

  /**
   * 来源维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备名称（填充字段）
   */
  equipmentName?: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode: string;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师（人员编码）
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期（归档基准日，通常取工单完工时间）
   */
  maintenanceDate: string;

  /**
   * 维护开始时间
   */
  maintenanceStartTime?: string;

  /**
   * 维护结束时间
   */
  maintenanceEndTime?: string;

  /**
   * 维护内容描述
   */
  maintenanceContent?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 使用配件（JSON，由工单领料明细汇总）
   */
  usedParts?: string;

  /**
   * 维护费用（工单总成本快照）
   */
  maintenanceCost: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 履历状态（固定为 2=已完成，归档写入）
   */
  maintenanceStatus: number;

  /**
   * 下次维护日期
   */
  nextMaintenanceDate?: string;

  /**
   * 维护周期（天）
   */
  maintenanceCycleDays: number;

  /**
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

  /**
   * 验收人（人员编码）
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 归档时间
   */
  archivedAt: string;

  /**
   * 设备（主表） （主表：TaktEquipment）
   */
  equipment?: Equipment;

  /**
   * 来源维护工单 （主表：TaktMaintenanceWorkOrder）
   */
  maintenanceWorkOrder?: MaintenanceWorkOrder;

}


/**
 * MaintenanceHistory 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaintenanceHistoryQuery
 * @description 对应后端 TaktMaintenanceHistoryQueryDto
 */
export interface MaintenanceHistoryQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 来源维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode?: string;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory?: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师（人员编码）
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期（归档基准日，通常取工单完工时间）（范围查询-开始）
   */
  maintenanceDateStart?: string;

  /**
   * 维护日期（归档基准日，通常取工单完工时间）（范围查询-结束）
   */
  maintenanceDateEnd?: string;

  /**
   * 维护开始时间（范围查询-开始）
   */
  maintenanceStartTimeStart?: string;

  /**
   * 维护开始时间（范围查询-结束）
   */
  maintenanceStartTimeEnd?: string;

  /**
   * 维护结束时间（范围查询-开始）
   */
  maintenanceEndTimeStart?: string;

  /**
   * 维护结束时间（范围查询-结束）
   */
  maintenanceEndTimeEnd?: string;

  /**
   * 维护内容描述
   */
  maintenanceContent?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 使用配件（JSON，由工单领料明细汇总）
   */
  usedParts?: string;

  /**
   * 维护费用（工单总成本快照）
   */
  maintenanceCost?: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult?: number;

  /**
   * 履历状态（固定为 2=已完成，归档写入）
   */
  maintenanceStatus?: number;

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
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

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
   * 归档时间（范围查询-开始）
   */
  archivedAtStart?: string;

  /**
   * 归档时间（范围查询-结束）
   */
  archivedAtEnd?: string;

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
 * 创建MaintenanceHistory DTO
 * 对应前端 MaintenanceHistoryCreate
 * @description 对应后端 TaktMaintenanceHistoryCreateDto
 */
export interface MaintenanceHistoryCreate {
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
   * 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 来源维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode: string;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师（人员编码）
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期（归档基准日，通常取工单完工时间）
   */
  maintenanceDate: string;

  /**
   * 维护开始时间
   */
  maintenanceStartTime?: string;

  /**
   * 维护结束时间
   */
  maintenanceEndTime?: string;

  /**
   * 维护内容描述
   */
  maintenanceContent?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 使用配件（JSON，由工单领料明细汇总）
   */
  usedParts?: string;

  /**
   * 维护费用（工单总成本快照）
   */
  maintenanceCost: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 履历状态（固定为 2=已完成，归档写入）
   */
  maintenanceStatus: number;

  /**
   * 下次维护日期
   */
  nextMaintenanceDate?: string;

  /**
   * 维护周期（天）
   */
  maintenanceCycleDays: number;

  /**
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

  /**
   * 验收人（人员编码）
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 归档时间
   */
  archivedAt: string;

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
 * 更新MaintenanceHistory DTO
 * 继承 TaktMaintenanceHistoryCreateDto，添加 MaintenanceHistoryId 字段
 * 对应前端 MaintenanceHistoryUpdate
 * @description 对应后端 TaktMaintenanceHistoryUpdateDto
 */
export interface MaintenanceHistoryUpdate extends MaintenanceHistoryCreate {
  /**
   * MaintenanceHistoryID（标识要更新的实体）
   */
  maintenanceHistoryId: string;

}


/**
 * MaintenanceHistory 状态更新 DTO
 * 对应前端 MaintenanceHistoryStatus
 * @description 对应后端 TaktMaintenanceHistoryStatusDto
 */
export interface MaintenanceHistoryStatus {
  /**
   * MaintenanceHistoryID
   */
  maintenanceHistoryId: string;

  /**
   * 履历状态（固定为 2=已完成，归档写入）
   */
  maintenanceStatus: number;

}


/**
 * MaintenanceHistory 导入模板行 DTO
 * 对应前端 MaintenanceHistoryTemplate
 * @description 对应后端 TaktMaintenanceHistoryTemplateDto
 */
export interface MaintenanceHistoryTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 来源维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode?: string;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory?: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师（人员编码）
   */
  maintenanceTechnician?: string;

  /**
   * 维护内容描述
   */
  maintenanceContent?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 使用配件（JSON，由工单领料明细汇总）
   */
  usedParts?: string;

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
 * MaintenanceHistory 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaintenanceHistoryImport
 * @description 对应后端 TaktMaintenanceHistoryImportDto
 */
export interface MaintenanceHistoryImport {
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
   * 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 来源维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode?: string;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory?: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师（人员编码）
   */
  maintenanceTechnician?: string;

  /**
   * 维护内容描述
   */
  maintenanceContent?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 使用配件（JSON，由工单领料明细汇总）
   */
  usedParts?: string;

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
 * MaintenanceHistory 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceHistoryExport
 * @description 对应后端 TaktMaintenanceHistoryExportDto
 */
export interface MaintenanceHistoryExport {
  /**
   * MaintenanceHistoryID
   */
  maintenanceHistoryId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 来源维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode: string;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师（人员编码）
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期（归档基准日，通常取工单完工时间）
   */
  maintenanceDate: string;

  /**
   * 维护开始时间
   */
  maintenanceStartTime?: string;

  /**
   * 维护结束时间
   */
  maintenanceEndTime?: string;

  /**
   * 维护内容描述
   */
  maintenanceContent?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 使用配件（JSON，由工单领料明细汇总）
   */
  usedParts?: string;

  /**
   * 维护费用（工单总成本快照）
   */
  maintenanceCost: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 履历状态（固定为 2=已完成，归档写入）
   */
  maintenanceStatus: number;

  /**
   * 下次维护日期
   */
  nextMaintenanceDate?: string;

  /**
   * 维护周期（天）
   */
  maintenanceCycleDays: number;

  /**
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

  /**
   * 验收人（人员编码）
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 归档时间
   */
  archivedAt: string;

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

