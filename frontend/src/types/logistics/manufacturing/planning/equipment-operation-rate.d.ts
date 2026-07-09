// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/planning
// 文件名称：equipment-operation-rate.d.ts
// 创建时间：2026-07-06
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
 * 机器稼动率实体（生产设备运行效率记录） 时间稼动率(%) = 稼动时间 ÷ 负荷时间 × 100%；为 OEE（设备综合效率）基础之一。
 * 对应前端 TaktEquipmentOperationRateDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EquipmentOperationRate
 * @description 对应后端 TaktEquipmentOperationRateDto
 */
export interface EquipmentOperationRate extends CompanyDtoBase {
  /**
   * EquipmentOperationRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  equipmentOperationRateId: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
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
   * 设备编码（关联 TaktEquipment.EquipmentCode，选项 TaktEquipments/options）
   */
  equipmentCode: string;

  /**
   * 设备名称
   */
  equipmentName: string;

  /**
   * 登录设备（字典 logistics_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
   */
  equipmentType: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam?: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
   */
  plannedRuntime: number;

  /**
   * 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
   */
  actualRuntime: number;

  /**
   * 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
   */
  downtime: number;

  /**
   * 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
   */
  equipmentOperationRate: number;

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
   * 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
   */
  downtimeReasonType?: number;

  /**
   * 停机原因描述（自由文本，与 DowntimeReasonType 配合）
   */
  downtimeReason?: string;

  /**
   * 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentOperator?: string;

  /**
   * 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentMaintainer?: string;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus: number;

}


/**
 * EquipmentOperationRate 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EquipmentOperationRateQuery
 * @description 对应后端 TaktEquipmentOperationRateQueryDto
 */
export interface EquipmentOperationRateQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
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
   * 设备编码（关联 TaktEquipment.EquipmentCode，选项 TaktEquipments/options）
   */
  equipmentCode?: string;

  /**
   * 设备名称
   */
  equipmentName?: string;

  /**
   * 登录设备（字典 logistics_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
   */
  equipmentType?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam?: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
   */
  plannedRuntime?: number;

  /**
   * 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
   */
  actualRuntime?: number;

  /**
   * 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
   */
  downtime?: number;

  /**
   * 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
   */
  equipmentOperationRate?: number;

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
   * 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
   */
  downtimeReasonType?: number;

  /**
   * 停机原因描述（自由文本，与 DowntimeReasonType 配合）
   */
  downtimeReason?: string;

  /**
   * 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentOperator?: string;

  /**
   * 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentMaintainer?: string;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus?: number;

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
 * 创建EquipmentOperationRate DTO
 * 对应前端 EquipmentOperationRateCreate
 * @description 对应后端 TaktEquipmentOperationRateCreateDto
 */
export interface EquipmentOperationRateCreate {
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
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
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
   * 设备编码（关联 TaktEquipment.EquipmentCode，选项 TaktEquipments/options）
   */
  equipmentCode: string;

  /**
   * 设备名称
   */
  equipmentName: string;

  /**
   * 登录设备（字典 logistics_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
   */
  equipmentType: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam?: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
   */
  plannedRuntime: number;

  /**
   * 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
   */
  actualRuntime: number;

  /**
   * 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
   */
  downtime: number;

  /**
   * 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
   */
  equipmentOperationRate: number;

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
   * 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
   */
  downtimeReasonType?: number;

  /**
   * 停机原因描述（自由文本，与 DowntimeReasonType 配合）
   */
  downtimeReason?: string;

  /**
   * 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentOperator?: string;

  /**
   * 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentMaintainer?: string;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus: number;

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
 * 更新EquipmentOperationRate DTO
 * 继承 TaktEquipmentOperationRateCreateDto，添加 EquipmentOperationRateId 字段
 * 对应前端 EquipmentOperationRateUpdate
 * @description 对应后端 TaktEquipmentOperationRateUpdateDto
 */
export interface EquipmentOperationRateUpdate extends EquipmentOperationRateCreate {
  /**
   * EquipmentOperationRateID（标识要更新的实体）
   */
  equipmentOperationRateId: string;

}


/**
 * EquipmentOperationRate 状态更新 DTO
 * 对应前端 EquipmentOperationRateStatus
 * @description 对应后端 TaktEquipmentOperationRateStatusDto
 */
export interface EquipmentOperationRateStatus {
  /**
   * EquipmentOperationRateID
   */
  equipmentOperationRateId: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus: number;

}


/**
 * EquipmentOperationRate 导入模板行 DTO
 * 对应前端 EquipmentOperationRateTemplate
 * @description 对应后端 TaktEquipmentOperationRateTemplateDto
 */
export interface EquipmentOperationRateTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory?: number;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 设备编码（关联 TaktEquipment.EquipmentCode，选项 TaktEquipments/options）
   */
  equipmentCode?: string;

  /**
   * 设备名称
   */
  equipmentName?: string;

  /**
   * 登录设备（字典 logistics_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
   */
  equipmentType?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam?: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
   */
  plannedRuntime?: number;

  /**
   * 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
   */
  actualRuntime?: number;

  /**
   * 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
   */
  downtime?: number;

  /**
   * 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
   */
  equipmentOperationRate?: number;

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
   * 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
   */
  downtimeReasonType?: number;

  /**
   * 停机原因描述（自由文本，与 DowntimeReasonType 配合）
   */
  downtimeReason?: string;

  /**
   * 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentOperator?: string;

  /**
   * 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentMaintainer?: string;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus?: number;

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
 * EquipmentOperationRate 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EquipmentOperationRateImport
 * @description 对应后端 TaktEquipmentOperationRateImportDto
 */
export interface EquipmentOperationRateImport {
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
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory?: number;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 设备编码（关联 TaktEquipment.EquipmentCode，选项 TaktEquipments/options）
   */
  equipmentCode?: string;

  /**
   * 设备名称
   */
  equipmentName?: string;

  /**
   * 登录设备（字典 logistics_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
   */
  equipmentType?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam?: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
   */
  plannedRuntime?: number;

  /**
   * 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
   */
  actualRuntime?: number;

  /**
   * 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
   */
  downtime?: number;

  /**
   * 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
   */
  equipmentOperationRate?: number;

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
   * 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
   */
  downtimeReasonType?: number;

  /**
   * 停机原因描述（自由文本，与 DowntimeReasonType 配合）
   */
  downtimeReason?: string;

  /**
   * 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentOperator?: string;

  /**
   * 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentMaintainer?: string;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus?: number;

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
 * EquipmentOperationRate 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EquipmentOperationRateExport
 * @description 对应后端 TaktEquipmentOperationRateExportDto
 */
export interface EquipmentOperationRateExport {
  /**
   * EquipmentOperationRateID
   */
  equipmentOperationRateId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
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
   * 设备编码（关联 TaktEquipment.EquipmentCode，选项 TaktEquipments/options）
   */
  equipmentCode: string;

  /**
   * 设备名称
   */
  equipmentName: string;

  /**
   * 登录设备（字典 logistics_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
   */
  equipmentType: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam?: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
   */
  plannedRuntime: number;

  /**
   * 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
   */
  actualRuntime: number;

  /**
   * 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
   */
  downtime: number;

  /**
   * 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
   */
  equipmentOperationRate: number;

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
   * 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
   */
  downtimeReasonType?: number;

  /**
   * 停机原因描述（自由文本，与 DowntimeReasonType 配合）
   */
  downtimeReason?: string;

  /**
   * 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentOperator?: string;

  /**
   * 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentMaintainer?: string;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus: number;

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

