// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：maintenance.d.ts
// 创建时间：2026-06-05
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
 * Takt设备维护记录实体（TaktEquipment的子表）
 * 对应前端 TaktMaintenanceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Maintenance
 * @description 对应后端 TaktMaintenanceDto
 */
export interface Maintenance extends CompanyDtoBase {
  /**
   * MaintenanceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  maintenanceId: string;

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
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期
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
   * 使用配件（JSON格式，存储使用的配件列表）
   */
  usedParts?: string;

  /**
   * 维护费用（精确到分，存储为整数，单位为分）
   */
  maintenanceCost: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
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
   * 验收人
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 设备（主表） （主表：TaktEquipment）
   */
  equipment?: Equipment;

}


/**
 * Maintenance 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaintenanceQuery
 * @description 对应后端 TaktMaintenanceQueryDto
 */
export interface MaintenanceQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期（范围查询-开始）
   */
  maintenanceDateStart?: string;

  /**
   * 维护日期（范围查询-结束）
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
   * 使用配件（JSON格式，存储使用的配件列表）
   */
  usedParts?: string;

  /**
   * 维护费用（精确到分，存储为整数，单位为分）
   */
  maintenanceCost?: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult?: number;

  /**
   * 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
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
   * 验收人
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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Maintenance DTO
 * 对应前端 MaintenanceCreate
 * @description 对应后端 TaktMaintenanceCreateDto
 */
export interface MaintenanceCreate {
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
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期
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
   * 使用配件（JSON格式，存储使用的配件列表）
   */
  usedParts?: string;

  /**
   * 维护费用（精确到分，存储为整数，单位为分）
   */
  maintenanceCost: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
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
   * 验收人
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新Maintenance DTO
 * 继承 TaktMaintenanceCreateDto，添加 MaintenanceId 字段
 * 对应前端 MaintenanceUpdate
 * @description 对应后端 TaktMaintenanceUpdateDto
 */
export interface MaintenanceUpdate extends MaintenanceCreate {
  /**
   * MaintenanceID（标识要更新的实体）
   */
  maintenanceId: string;

}


/**
 * Maintenance 状态更新 DTO
 * 对应前端 MaintenanceStatus
 * @description 对应后端 TaktMaintenanceStatusDto
 */
export interface MaintenanceStatus {
  /**
   * MaintenanceID
   */
  maintenanceId: string;

  /**
   * 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
   */
  maintenanceStatus: number;

}


/**
 * Maintenance 导入模板行 DTO
 * 对应前端 MaintenanceTemplate
 * @description 对应后端 TaktMaintenanceTemplateDto
 */
export interface MaintenanceTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师
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
   * 使用配件（JSON格式，存储使用的配件列表）
   */
  usedParts?: string;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult?: number;

  /**
   * 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
   */
  maintenanceStatus?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Maintenance 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaintenanceImport
 * @description 对应后端 TaktMaintenanceImportDto
 */
export interface MaintenanceImport {
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
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师
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
   * 使用配件（JSON格式，存储使用的配件列表）
   */
  usedParts?: string;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult?: number;

  /**
   * 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
   */
  maintenanceStatus?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Maintenance 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceExport
 * @description 对应后端 TaktMaintenanceExportDto
 */
export interface MaintenanceExport {
  /**
   * MaintenanceID
   */
  maintenanceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  equipmentCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期
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
   * 使用配件（JSON格式，存储使用的配件列表）
   */
  usedParts?: string;

  /**
   * 维护费用（精确到分，存储为整数，单位为分）
   */
  maintenanceCost: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 维护状态（0=待执行，1=执行中，2=已完成，3=已取消）
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
   * 验收人
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

