// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/mrp
// 文件名称：material-requirements-planning.d.ts
// 创建时间：2026-07-23
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
 * 物料需求计划 MRP 头表（公司级；MPS 下推，产出 TaktPlannedOrder / TaktProductionPlan / TaktPurchasePlan）
 * 对应前端 TaktMaterialRequirementsPlanningDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 MaterialRequirementsPlanning
 * @description 对应后端 TaktMaterialRequirementsPlanningDto
 */
export interface MaterialRequirementsPlanning extends ApprovalDtoBase {
  /**
   * MaterialRequirementsPlanningID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * MRP 编码（租户+公司+工厂内业务唯一）
   */
  materialRequirementsPlanningCode: string;

  /**
   * 来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 头表 名称（填充字段）
   */
  masterProductionScheduleName?: string;

  /**
   * 来源 MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层追溯，可选）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 头表 名称（填充字段）
   */
  masterDemandScheduleName?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

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
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人员工名称（填充字段）
   */
  plannerName?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy: string;

  /**
   * 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
   */
  runStatus: number;

  /**
   * 产出生产计划 ID（运算完成后回写）
   */
  productionPlanId?: string;

  /**
   * 产出生产计划 名称（填充字段）
   */
  productionPlanName?: string;

  /**
   * 产出生产计划编码（冗余）
   */
  productionPlanCode?: string;

  /**
   * 产出采购计划 ID（运算完成后回写）
   */
  purchasePlanId?: string;

  /**
   * 产出采购计划 名称（填充字段）
   */
  purchasePlanName?: string;

  /**
   * 产出采购计划编码（冗余）
   */
  purchasePlanCode?: string;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * MRP 需求明细列表（主子表关系） （子表：TaktMaterialRequirementsPlanningItem）
   */
  items?: MaterialRequirementsPlanningItem[];

}


/**
 * MaterialRequirementsPlanning 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialRequirementsPlanningQuery
 * @description 对应后端 TaktMaterialRequirementsPlanningQueryDto
 */
export interface MaterialRequirementsPlanningQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * MRP 编码（租户+公司+工厂内业务唯一）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层追溯，可选）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

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
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy?: string;

  /**
   * 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
   */
  runStatus?: number;

  /**
   * 产出生产计划 ID（运算完成后回写）
   */
  productionPlanId?: string;

  /**
   * 产出生产计划编码（冗余）
   */
  productionPlanCode?: string;

  /**
   * 产出采购计划 ID（运算完成后回写）
   */
  purchasePlanId?: string;

  /**
   * 产出采购计划编码（冗余）
   */
  purchasePlanCode?: string;

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
 * 创建MaterialRequirementsPlanning DTO
 * 对应前端 MaterialRequirementsPlanningCreate
 * @description 对应后端 TaktMaterialRequirementsPlanningCreateDto
 */
export interface MaterialRequirementsPlanningCreate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * MRP 编码（租户+公司+工厂内业务唯一）
   */
  materialRequirementsPlanningCode: string;

  /**
   * 来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层追溯，可选）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

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
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy: string;

  /**
   * 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
   */
  runStatus: number;

  /**
   * 产出生产计划 ID（运算完成后回写）
   */
  productionPlanId?: string;

  /**
   * 产出生产计划编码（冗余）
   */
  productionPlanCode?: string;

  /**
   * 产出采购计划 ID（运算完成后回写）
   */
  purchasePlanId?: string;

  /**
   * 产出采购计划编码（冗余）
   */
  purchasePlanCode?: string;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * MRP 需求明细列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialRequirementsPlanningItemCreate[];

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
 * 更新MaterialRequirementsPlanning DTO
 * 继承 TaktMaterialRequirementsPlanningCreateDto，添加 MaterialRequirementsPlanningId 字段
 * 对应前端 MaterialRequirementsPlanningUpdate
 * @description 对应后端 TaktMaterialRequirementsPlanningUpdateDto
 */
export interface MaterialRequirementsPlanningUpdate extends MaterialRequirementsPlanningCreate {
  /**
   * MaterialRequirementsPlanningID（标识要更新的实体）
   */
  materialRequirementsPlanningId: string;

  /**
   * MRP 需求明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * MaterialRequirementsPlanning 状态更新 DTO
 * 对应前端 MaterialRequirementsPlanningStatus
 * @description 对应后端 TaktMaterialRequirementsPlanningStatusDto
 */
export interface MaterialRequirementsPlanningStatus {
  /**
   * MaterialRequirementsPlanningID
   */
  materialRequirementsPlanningId: string;

  /**
   * 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
   */
  runStatus: number;

}


/**
 * MaterialRequirementsPlanning 导入模板行 DTO
 * 对应前端 MaterialRequirementsPlanningTemplate
 * @description 对应后端 TaktMaterialRequirementsPlanningTemplateDto
 */
export interface MaterialRequirementsPlanningTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * MRP 编码（租户+公司+工厂内业务唯一）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层追溯，可选）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

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
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy?: string;

  /**
   * 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
   */
  runStatus?: number;

  /**
   * 产出生产计划 ID（运算完成后回写）
   */
  productionPlanId?: string;

  /**
   * 产出生产计划编码（冗余）
   */
  productionPlanCode?: string;

  /**
   * 产出采购计划 ID（运算完成后回写）
   */
  purchasePlanId?: string;

  /**
   * 产出采购计划编码（冗余）
   */
  purchasePlanCode?: string;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * MRP 需求明细列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialRequirementsPlanningItemCreate[];

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
 * MaterialRequirementsPlanning 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialRequirementsPlanningImport
 * @description 对应后端 TaktMaterialRequirementsPlanningImportDto
 */
export interface MaterialRequirementsPlanningImport {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * MRP 编码（租户+公司+工厂内业务唯一）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层追溯，可选）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

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
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy?: string;

  /**
   * 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
   */
  runStatus?: number;

  /**
   * 产出生产计划 ID（运算完成后回写）
   */
  productionPlanId?: string;

  /**
   * 产出生产计划编码（冗余）
   */
  productionPlanCode?: string;

  /**
   * 产出采购计划 ID（运算完成后回写）
   */
  purchasePlanId?: string;

  /**
   * 产出采购计划编码（冗余）
   */
  purchasePlanCode?: string;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * MRP 需求明细列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialRequirementsPlanningItemCreate[];

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
 * MaterialRequirementsPlanning 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialRequirementsPlanningExport
 * @description 对应后端 TaktMaterialRequirementsPlanningExportDto
 */
export interface MaterialRequirementsPlanningExport {
  /**
   * MaterialRequirementsPlanningID
   */
  materialRequirementsPlanningId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * MRP 编码（租户+公司+工厂内业务唯一）
   */
  materialRequirementsPlanningCode: string;

  /**
   * 来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层追溯，可选）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

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
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy: string;

  /**
   * 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
   */
  runStatus: number;

  /**
   * 产出生产计划 ID（运算完成后回写）
   */
  productionPlanId?: string;

  /**
   * 产出生产计划编码（冗余）
   */
  productionPlanCode?: string;

  /**
   * 产出采购计划 ID（运算完成后回写）
   */
  purchasePlanId?: string;

  /**
   * 产出采购计划编码（冗余）
   */
  purchasePlanCode?: string;

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

