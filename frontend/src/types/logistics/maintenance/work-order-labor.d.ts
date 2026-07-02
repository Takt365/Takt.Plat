// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：work-order-labor.d.ts
// 创建时间：2026-06-23
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
 * 维护工单报工明细实体（主子表：挂载于维护工单）
 * 对应前端 TaktMaintenanceWorkOrderLaborDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaintenanceWorkOrderLabor
 * @description 对应后端 TaktMaintenanceWorkOrderLaborDto
 */
export interface MaintenanceWorkOrderLabor extends CompanyDtoBase {
  /**
   * MaintenanceWorkOrderLaborID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  maintenanceWorkOrderLaborId: string;

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 维护工单名称（填充字段）
   */
  maintenanceWorkOrderName?: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 员工ID（序列化为string以避免Javascript精度问题）
   */
  employeeId?: string;

  /**
   * 员工编码
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余）
   */
  employeeName?: string;

  /**
   * 报工日期
   */
  workDate: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 工时（小时）
   */
  workHours: number;

  /**
   * 小时费率
   */
  hourlyRate: number;

  /**
   * 人工成本
   */
  laborCost: number;

  /**
   * 作业描述
   */
  operationDescription?: string;

  /**
   * 报工确认状态（0=待确认，1=已确认）
   */
  confirmationStatus: number;

  /**
   * 确认时间
   */
  confirmedAt?: string;

  /**
   * 维护工单（主表） （主表：TaktMaintenanceWorkOrder）
   */
  maintenanceWorkOrder?: MaintenanceWorkOrder;

}


/**
 * MaintenanceWorkOrderLabor 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaintenanceWorkOrderLaborQuery
 * @description 对应后端 TaktMaintenanceWorkOrderLaborQueryDto
 */
export interface MaintenanceWorkOrderLaborQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 员工ID（序列化为string以避免Javascript精度问题）
   */
  employeeId?: string;

  /**
   * 员工编码
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余）
   */
  employeeName?: string;

  /**
   * 报工日期（范围查询-开始）
   */
  workDateStart?: string;

  /**
   * 报工日期（范围查询-结束）
   */
  workDateEnd?: string;

  /**
   * 开始时间（范围查询-开始）
   */
  startTimeStart?: string;

  /**
   * 开始时间（范围查询-结束）
   */
  startTimeEnd?: string;

  /**
   * 结束时间（范围查询-开始）
   */
  endTimeStart?: string;

  /**
   * 结束时间（范围查询-结束）
   */
  endTimeEnd?: string;

  /**
   * 工时（小时）
   */
  workHours?: number;

  /**
   * 小时费率
   */
  hourlyRate?: number;

  /**
   * 人工成本
   */
  laborCost?: number;

  /**
   * 作业描述
   */
  operationDescription?: string;

  /**
   * 报工确认状态（0=待确认，1=已确认）
   */
  confirmationStatus?: number;

  /**
   * 确认时间（范围查询-开始）
   */
  confirmedAtStart?: string;

  /**
   * 确认时间（范围查询-结束）
   */
  confirmedAtEnd?: string;

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
 * 创建MaintenanceWorkOrderLabor DTO
 * 对应前端 MaintenanceWorkOrderLaborCreate
 * @description 对应后端 TaktMaintenanceWorkOrderLaborCreateDto
 */
export interface MaintenanceWorkOrderLaborCreate {
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
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 员工ID（序列化为string以避免Javascript精度问题）
   */
  employeeId?: string;

  /**
   * 员工编码
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余）
   */
  employeeName?: string;

  /**
   * 报工日期
   */
  workDate: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 工时（小时）
   */
  workHours: number;

  /**
   * 小时费率
   */
  hourlyRate: number;

  /**
   * 人工成本
   */
  laborCost: number;

  /**
   * 作业描述
   */
  operationDescription?: string;

  /**
   * 报工确认状态（0=待确认，1=已确认）
   */
  confirmationStatus: number;

  /**
   * 确认时间
   */
  confirmedAt?: string;

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
 * 更新MaintenanceWorkOrderLabor DTO
 * 继承 TaktMaintenanceWorkOrderLaborCreateDto，添加 MaintenanceWorkOrderLaborId 字段
 * 对应前端 MaintenanceWorkOrderLaborUpdate
 * @description 对应后端 TaktMaintenanceWorkOrderLaborUpdateDto
 */
export interface MaintenanceWorkOrderLaborUpdate extends MaintenanceWorkOrderLaborCreate {
  /**
   * MaintenanceWorkOrderLaborID（标识要更新的实体）
   */
  maintenanceWorkOrderLaborId: string;

}


/**
 * MaintenanceWorkOrderLabor 状态更新 DTO
 * 对应前端 MaintenanceWorkOrderLaborStatus
 * @description 对应后端 TaktMaintenanceWorkOrderLaborStatusDto
 */
export interface MaintenanceWorkOrderLaborStatus {
  /**
   * MaintenanceWorkOrderLaborID
   */
  maintenanceWorkOrderLaborId: string;

  /**
   * 报工确认状态（0=待确认，1=已确认）
   */
  confirmationStatus: number;

}


/**
 * MaintenanceWorkOrderLabor 导入模板行 DTO
 * 对应前端 MaintenanceWorkOrderLaborTemplate
 * @description 对应后端 TaktMaintenanceWorkOrderLaborTemplateDto
 */
export interface MaintenanceWorkOrderLaborTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 员工ID（序列化为string以避免Javascript精度问题）
   */
  employeeId?: string;

  /**
   * 员工编码
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余）
   */
  employeeName?: string;

  /**
   * 报工日期
   */
  workDate?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 工时（小时）
   */
  workHours?: number;

  /**
   * 小时费率
   */
  hourlyRate?: number;

  /**
   * 人工成本
   */
  laborCost?: number;

  /**
   * 作业描述
   */
  operationDescription?: string;

  /**
   * 报工确认状态（0=待确认，1=已确认）
   */
  confirmationStatus?: number;

  /**
   * 确认时间
   */
  confirmedAt?: string;

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
 * MaintenanceWorkOrderLabor 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaintenanceWorkOrderLaborImport
 * @description 对应后端 TaktMaintenanceWorkOrderLaborImportDto
 */
export interface MaintenanceWorkOrderLaborImport {
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
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 员工ID（序列化为string以避免Javascript精度问题）
   */
  employeeId?: string;

  /**
   * 员工编码
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余）
   */
  employeeName?: string;

  /**
   * 报工日期
   */
  workDate?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 工时（小时）
   */
  workHours?: number;

  /**
   * 小时费率
   */
  hourlyRate?: number;

  /**
   * 人工成本
   */
  laborCost?: number;

  /**
   * 作业描述
   */
  operationDescription?: string;

  /**
   * 报工确认状态（0=待确认，1=已确认）
   */
  confirmationStatus?: number;

  /**
   * 确认时间
   */
  confirmedAt?: string;

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
 * MaintenanceWorkOrderLabor 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceWorkOrderLaborExport
 * @description 对应后端 TaktMaintenanceWorkOrderLaborExportDto
 */
export interface MaintenanceWorkOrderLaborExport {
  /**
   * MaintenanceWorkOrderLaborID
   */
  maintenanceWorkOrderLaborId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 员工ID（序列化为string以避免Javascript精度问题）
   */
  employeeId?: string;

  /**
   * 员工编码
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余）
   */
  employeeName?: string;

  /**
   * 报工日期
   */
  workDate: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 工时（小时）
   */
  workHours: number;

  /**
   * 小时费率
   */
  hourlyRate: number;

  /**
   * 人工成本
   */
  laborCost: number;

  /**
   * 作业描述
   */
  operationDescription?: string;

  /**
   * 报工确认状态（0=待确认，1=已确认）
   */
  confirmationStatus: number;

  /**
   * 确认时间
   */
  confirmedAt?: string;

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

