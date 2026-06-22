// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：personnel-operation-rate.d.ts
// 创建时间：2026-06-20
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
 * 人员稼动率实体（生产线人员作业效率记录） 人员稼动率(%) = 在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
 * 对应前端 TaktPersonnelOperationRateDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PersonnelOperationRate
 * @description 对应后端 TaktPersonnelOperationRateDto
 */
export interface PersonnelOperationRate extends CompanyDtoBase {
  /**
   * PersonnelOperationRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  personnelOperationRateId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 生产线
   */
  productionLine: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo: number;

  /**
   * 计划直接人员数量
   */
  plannedDirectPersonnelCount: number;

  /**
   * 实际直接人员数量
   */
  actualDirectPersonnelCount: number;

  /**
   * 计划间接人员数量
   */
  plannedIndirectPersonnelCount: number;

  /**
   * 实际间接人员数量
   */
  actualIndirectPersonnelCount: number;

  /**
   * 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
   */
  plannedWorkTime: number;

  /**
   * 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
   */
  actualWorkTime: number;

  /**
   * 休息时间（分钟）
   */
  breakTime: number;

  /**
   * 空闲时间（分钟）。等料、设备调试等非作业时间。
   */
  idleTime: number;

  /**
   * 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
   */
  personnelOperationRate: number;

  /**
   * 计划产量
   */
  plannedOutput: number;

  /**
   * 实际产量
   */
  actualOutput: number;

  /**
   * 合格品数量
   */
  qualifiedQuantity: number;

  /**
   * 不良品数量
   */
  defectiveQuantity: number;

  /**
   * 良品率（%）
   */
  yieldRate: number;

  /**
   * 工作效率（%）
   */
  workEfficiency: number;

  /**
   * 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
   */
  idleReasonType?: number;

  /**
   * 空闲原因描述
   */
  idleReason?: string;

  /**
   * 加班时间（分钟）
   */
  overtimeHours: number;

  /**
   * 班组长
   */
  teamLeader?: string;

  /**
   * 主管
   */
  supervisor?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  status: number;

}


/**
 * PersonnelOperationRate 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PersonnelOperationRateQuery
 * @description 对应后端 TaktPersonnelOperationRateQueryDto
 */
export interface PersonnelOperationRateQuery extends TaktPagedQuery {
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
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory?: number;

  /**
   * 开始日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 开始日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 结束日期（范围查询-开始）
   */
  endDateStart?: string;

  /**
   * 结束日期（范围查询-结束）
   */
  endDateEnd?: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo?: number;

  /**
   * 计划直接人员数量
   */
  plannedDirectPersonnelCount?: number;

  /**
   * 实际直接人员数量
   */
  actualDirectPersonnelCount?: number;

  /**
   * 计划间接人员数量
   */
  plannedIndirectPersonnelCount?: number;

  /**
   * 实际间接人员数量
   */
  actualIndirectPersonnelCount?: number;

  /**
   * 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
   */
  plannedWorkTime?: number;

  /**
   * 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
   */
  actualWorkTime?: number;

  /**
   * 休息时间（分钟）
   */
  breakTime?: number;

  /**
   * 空闲时间（分钟）。等料、设备调试等非作业时间。
   */
  idleTime?: number;

  /**
   * 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
   */
  personnelOperationRate?: number;

  /**
   * 计划产量
   */
  plannedOutput?: number;

  /**
   * 实际产量
   */
  actualOutput?: number;

  /**
   * 合格品数量
   */
  qualifiedQuantity?: number;

  /**
   * 不良品数量
   */
  defectiveQuantity?: number;

  /**
   * 良品率（%）
   */
  yieldRate?: number;

  /**
   * 工作效率（%）
   */
  workEfficiency?: number;

  /**
   * 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
   */
  idleReasonType?: number;

  /**
   * 空闲原因描述
   */
  idleReason?: string;

  /**
   * 加班时间（分钟）
   */
  overtimeHours?: number;

  /**
   * 班组长
   */
  teamLeader?: string;

  /**
   * 主管
   */
  supervisor?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  status?: number;

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
 * 创建PersonnelOperationRate DTO
 * 对应前端 PersonnelOperationRateCreate
 * @description 对应后端 TaktPersonnelOperationRateCreateDto
 */
export interface PersonnelOperationRateCreate {
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
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 生产线
   */
  productionLine: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo: number;

  /**
   * 计划直接人员数量
   */
  plannedDirectPersonnelCount: number;

  /**
   * 实际直接人员数量
   */
  actualDirectPersonnelCount: number;

  /**
   * 计划间接人员数量
   */
  plannedIndirectPersonnelCount: number;

  /**
   * 实际间接人员数量
   */
  actualIndirectPersonnelCount: number;

  /**
   * 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
   */
  plannedWorkTime: number;

  /**
   * 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
   */
  actualWorkTime: number;

  /**
   * 休息时间（分钟）
   */
  breakTime: number;

  /**
   * 空闲时间（分钟）。等料、设备调试等非作业时间。
   */
  idleTime: number;

  /**
   * 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
   */
  personnelOperationRate: number;

  /**
   * 计划产量
   */
  plannedOutput: number;

  /**
   * 实际产量
   */
  actualOutput: number;

  /**
   * 合格品数量
   */
  qualifiedQuantity: number;

  /**
   * 不良品数量
   */
  defectiveQuantity: number;

  /**
   * 良品率（%）
   */
  yieldRate: number;

  /**
   * 工作效率（%）
   */
  workEfficiency: number;

  /**
   * 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
   */
  idleReasonType?: number;

  /**
   * 空闲原因描述
   */
  idleReason?: string;

  /**
   * 加班时间（分钟）
   */
  overtimeHours: number;

  /**
   * 班组长
   */
  teamLeader?: string;

  /**
   * 主管
   */
  supervisor?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  status: number;

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
 * 更新PersonnelOperationRate DTO
 * 继承 TaktPersonnelOperationRateCreateDto，添加 PersonnelOperationRateId 字段
 * 对应前端 PersonnelOperationRateUpdate
 * @description 对应后端 TaktPersonnelOperationRateUpdateDto
 */
export interface PersonnelOperationRateUpdate extends PersonnelOperationRateCreate {
  /**
   * PersonnelOperationRateID（标识要更新的实体）
   */
  personnelOperationRateId: string;

}


/**
 * PersonnelOperationRate 状态更新 DTO
 * 对应前端 PersonnelOperationRateStatus
 * @description 对应后端 TaktPersonnelOperationRateStatusDto
 */
export interface PersonnelOperationRateStatus {
  /**
   * PersonnelOperationRateID
   */
  personnelOperationRateId: string;

  /**
   * 状态（0=正常，1=停用）
   */
  status: number;

}


/**
 * PersonnelOperationRate 导入模板行 DTO
 * 对应前端 PersonnelOperationRateTemplate
 * @description 对应后端 TaktPersonnelOperationRateTemplateDto
 */
export interface PersonnelOperationRateTemplate {
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
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory?: number;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo?: number;

  /**
   * 计划直接人员数量
   */
  plannedDirectPersonnelCount?: number;

  /**
   * 实际直接人员数量
   */
  actualDirectPersonnelCount?: number;

  /**
   * 计划间接人员数量
   */
  plannedIndirectPersonnelCount?: number;

  /**
   * 实际间接人员数量
   */
  actualIndirectPersonnelCount?: number;

  /**
   * 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
   */
  idleReasonType?: number;

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
 * PersonnelOperationRate 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PersonnelOperationRateImport
 * @description 对应后端 TaktPersonnelOperationRateImportDto
 */
export interface PersonnelOperationRateImport {
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
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory?: number;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo?: number;

  /**
   * 计划直接人员数量
   */
  plannedDirectPersonnelCount?: number;

  /**
   * 实际直接人员数量
   */
  actualDirectPersonnelCount?: number;

  /**
   * 计划间接人员数量
   */
  plannedIndirectPersonnelCount?: number;

  /**
   * 实际间接人员数量
   */
  actualIndirectPersonnelCount?: number;

  /**
   * 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
   */
  idleReasonType?: number;

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
 * PersonnelOperationRate 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PersonnelOperationRateExport
 * @description 对应后端 TaktPersonnelOperationRateExportDto
 */
export interface PersonnelOperationRateExport {
  /**
   * PersonnelOperationRateID
   */
  personnelOperationRateId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 生产线
   */
  productionLine: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo: number;

  /**
   * 计划直接人员数量
   */
  plannedDirectPersonnelCount: number;

  /**
   * 实际直接人员数量
   */
  actualDirectPersonnelCount: number;

  /**
   * 计划间接人员数量
   */
  plannedIndirectPersonnelCount: number;

  /**
   * 实际间接人员数量
   */
  actualIndirectPersonnelCount: number;

  /**
   * 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
   */
  plannedWorkTime: number;

  /**
   * 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
   */
  actualWorkTime: number;

  /**
   * 休息时间（分钟）
   */
  breakTime: number;

  /**
   * 空闲时间（分钟）。等料、设备调试等非作业时间。
   */
  idleTime: number;

  /**
   * 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
   */
  personnelOperationRate: number;

  /**
   * 计划产量
   */
  plannedOutput: number;

  /**
   * 实际产量
   */
  actualOutput: number;

  /**
   * 合格品数量
   */
  qualifiedQuantity: number;

  /**
   * 不良品数量
   */
  defectiveQuantity: number;

  /**
   * 良品率（%）
   */
  yieldRate: number;

  /**
   * 工作效率（%）
   */
  workEfficiency: number;

  /**
   * 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
   */
  idleReasonType?: number;

  /**
   * 空闲原因描述
   */
  idleReason?: string;

  /**
   * 加班时间（分钟）
   */
  overtimeHours: number;

  /**
   * 班组长
   */
  teamLeader?: string;

  /**
   * 主管
   */
  supervisor?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  status: number;

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

