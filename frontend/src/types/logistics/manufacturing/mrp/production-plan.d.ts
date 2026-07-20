// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/mrp
// 文件名称：production-plan.d.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mrp 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt生产计划实体（公司级；MRP 运算产出自制件计划，非 MPS 上游）
 * 对应前端 TaktProductionPlanDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 ProductionPlan
 * @description 对应后端 TaktProductionPlanDto
 */
export interface ProductionPlan extends ApprovalDtoBase {
  /**
   * ProductionPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 生产计划编码（租户+公司+工厂内业务唯一）
   */
  productionPlanCode: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源物料需求计划 名称（填充字段）
   */
  materialRequirementsPlanningName?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测名称（填充字段）
   */
  salesForecastName?: string;

  /**
   * 来源销售预测编码（冗余字段，便于查询）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 计划人员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  plannerId?: string;

  /**
   * 计划人员工名称（填充字段）
   */
  plannerName?: string;

  /**
   * 计划人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  planBy: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转工单/采购金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 生产计划明细列表（主子表关系） （子表：TaktProductionPlanItem）
   */
  items?: ProductionPlanItem[];

}


/**
 * ProductionPlan 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductionPlanQuery
 * @description 对应后端 TaktProductionPlanQueryDto
 */
export interface ProductionPlanQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 生产计划编码（租户+公司+工厂内业务唯一）
   */
  productionPlanCode?: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测编码（冗余字段，便于查询）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期（范围查询-开始）
   */
  planDateStart?: string;

  /**
   * 计划编制日期（范围查询-结束）
   */
  planDateEnd?: string;

  /**
   * 计划周期开始日期（范围查询-开始）
   */
  planPeriodStartStart?: string;

  /**
   * 计划周期开始日期（范围查询-结束）
   */
  planPeriodStartEnd?: string;

  /**
   * 计划周期结束日期（范围查询-开始）
   */
  planPeriodEndStart?: string;

  /**
   * 计划周期结束日期（范围查询-结束）
   */
  planPeriodEndEnd?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  plannerId?: string;

  /**
   * 计划人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  planBy?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 计划总金额
   */
  totalAmount?: number;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转工单/采购金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
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
 * 创建ProductionPlan DTO
 * 对应前端 ProductionPlanCreate
 * @description 对应后端 TaktProductionPlanCreateDto
 */
export interface ProductionPlanCreate {
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
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 生产计划编码（租户+公司+工厂内业务唯一）
   */
  productionPlanCode: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测编码（冗余字段，便于查询）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 计划人员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  plannerId?: string;

  /**
   * 计划人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  planBy: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转工单/采购金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 生产计划明细列表（主子表关系）（子表，级联保存）
   */
  items?: ProductionPlanItemCreate[];

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
 * 更新ProductionPlan DTO
 * 继承 TaktProductionPlanCreateDto，添加 ProductionPlanId 字段
 * 对应前端 ProductionPlanUpdate
 * @description 对应后端 TaktProductionPlanUpdateDto
 */
export interface ProductionPlanUpdate extends ProductionPlanCreate {
  /**
   * ProductionPlanID（标识要更新的实体）
   */
  productionPlanId: string;

  /**
   * 生产计划明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * ProductionPlan 状态更新 DTO
 * 对应前端 ProductionPlanStatus
 * @description 对应后端 TaktProductionPlanStatusDto
 */
export interface ProductionPlanStatus {
  /**
   * ProductionPlanID
   */
  productionPlanId: string;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

}


/**
 * ProductionPlan 导入模板行 DTO
 * 对应前端 ProductionPlanTemplate
 * @description 对应后端 TaktProductionPlanTemplateDto
 */
export interface ProductionPlanTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 生产计划编码（租户+公司+工厂内业务唯一）
   */
  productionPlanCode?: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测编码（冗余字段，便于查询）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate?: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  plannerId?: string;

  /**
   * 计划人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  planBy?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 计划总金额
   */
  totalAmount?: number;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转工单/采购金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 生产计划明细列表（主子表关系）（子表，级联保存）
   */
  items?: ProductionPlanItemCreate[];

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
 * ProductionPlan 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductionPlanImport
 * @description 对应后端 TaktProductionPlanImportDto
 */
export interface ProductionPlanImport {
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
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 生产计划编码（租户+公司+工厂内业务唯一）
   */
  productionPlanCode?: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测编码（冗余字段，便于查询）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate?: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  plannerId?: string;

  /**
   * 计划人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  planBy?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 计划总金额
   */
  totalAmount?: number;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转工单/采购金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 生产计划明细列表（主子表关系）（子表，级联保存）
   */
  items?: ProductionPlanItemCreate[];

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
 * ProductionPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionPlanExport
 * @description 对应后端 TaktProductionPlanExportDto
 */
export interface ProductionPlanExport {
  /**
   * ProductionPlanID
   */
  productionPlanId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 生产计划编码（租户+公司+工厂内业务唯一）
   */
  productionPlanCode: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测编码（冗余字段，便于查询）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 计划人员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  plannerId?: string;

  /**
   * 计划人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  planBy: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转工单/采购金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

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

