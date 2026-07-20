// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：master-production-schedule.d.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 主生产计划 MPS 头表（公司级；MDS 下推，成品级何时做多少、粗产能校验）
 * 对应前端 TaktMasterProductionScheduleDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 MasterProductionSchedule
 * @description 对应后端 TaktMasterProductionScheduleDto
 */
export interface MasterProductionSchedule extends ApprovalDtoBase {
  /**
   * MasterProductionScheduleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  masterProductionScheduleId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * MPS 编码
   */
  mpsCode: string;

  /**
   * 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
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
   * 计划周期开始
   */
  planPeriodStart: string;

  /**
   * 计划周期结束
   */
  planPeriodEnd: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  scheduleStatus: number;

  /**
   * MPS 明细行 （子表：TaktMasterProductionScheduleLine）
   */
  lines?: MasterProductionScheduleLine[];

}


/**
 * MasterProductionSchedule 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MasterProductionScheduleQuery
 * @description 对应后端 TaktMasterProductionScheduleQueryDto
 */
export interface MasterProductionScheduleQuery extends TaktPagedQuery {
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
   * MPS 编码
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 计划周期开始（范围查询-开始）
   */
  planPeriodStartStart?: string;

  /**
   * 计划周期开始（范围查询-结束）
   */
  planPeriodStartEnd?: string;

  /**
   * 计划周期结束（范围查询-开始）
   */
  planPeriodEndStart?: string;

  /**
   * 计划周期结束（范围查询-结束）
   */
  planPeriodEndEnd?: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType?: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  scheduleStatus?: number;

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
 * 创建MasterProductionSchedule DTO
 * 对应前端 MasterProductionScheduleCreate
 * @description 对应后端 TaktMasterProductionScheduleCreateDto
 */
export interface MasterProductionScheduleCreate {
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
   * MPS 编码
   */
  mpsCode: string;

  /**
   * 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 计划周期开始
   */
  planPeriodStart: string;

  /**
   * 计划周期结束
   */
  planPeriodEnd: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  scheduleStatus: number;

  /**
   * MPS 明细行（子表，级联保存）
   */
  lines?: MasterProductionScheduleLineCreate[];

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
 * 更新MasterProductionSchedule DTO
 * 继承 TaktMasterProductionScheduleCreateDto，添加 MasterProductionScheduleId 字段
 * 对应前端 MasterProductionScheduleUpdate
 * @description 对应后端 TaktMasterProductionScheduleUpdateDto
 */
export interface MasterProductionScheduleUpdate extends MasterProductionScheduleCreate {
  /**
   * MasterProductionScheduleID（标识要更新的实体）
   */
  masterProductionScheduleId: string;

  /**
   * MPS 明细行（子表，级联保存）
   */
  lines?: any;

}


/**
 * MasterProductionSchedule 状态更新 DTO
 * 对应前端 MasterProductionScheduleStatus
 * @description 对应后端 TaktMasterProductionScheduleStatusDto
 */
export interface MasterProductionScheduleStatus {
  /**
   * MasterProductionScheduleID
   */
  masterProductionScheduleId: string;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  scheduleStatus: number;

}


/**
 * MasterProductionSchedule 导入模板行 DTO
 * 对应前端 MasterProductionScheduleTemplate
 * @description 对应后端 TaktMasterProductionScheduleTemplateDto
 */
export interface MasterProductionScheduleTemplate {
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
   * MPS 编码
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 计划周期开始
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束
   */
  planPeriodEnd?: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType?: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  scheduleStatus?: number;

  /**
   * MPS 明细行（子表，级联保存）
   */
  lines?: MasterProductionScheduleLineCreate[];

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
 * MasterProductionSchedule 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MasterProductionScheduleImport
 * @description 对应后端 TaktMasterProductionScheduleImportDto
 */
export interface MasterProductionScheduleImport {
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
   * MPS 编码
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 计划周期开始
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束
   */
  planPeriodEnd?: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType?: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  scheduleStatus?: number;

  /**
   * MPS 明细行（子表，级联保存）
   */
  lines?: MasterProductionScheduleLineCreate[];

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
 * MasterProductionSchedule 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MasterProductionScheduleExport
 * @description 对应后端 TaktMasterProductionScheduleExportDto
 */
export interface MasterProductionScheduleExport {
  /**
   * MasterProductionScheduleID
   */
  masterProductionScheduleId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * MPS 编码
   */
  mpsCode: string;

  /**
   * 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 计划周期开始
   */
  planPeriodStart: string;

  /**
   * 计划周期结束
   */
  planPeriodEnd: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  scheduleStatus: number;

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

