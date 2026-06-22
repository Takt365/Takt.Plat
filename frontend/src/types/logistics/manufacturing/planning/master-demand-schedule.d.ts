// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/planning
// 文件名称：master-demand-schedule.d.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/planning 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 主需求计划 MDS 头表（公司级；承接销售订单与预测，下推 MPS）
 * 对应前端 TaktMasterDemandScheduleDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 MasterDemandSchedule
 * @description 对应后端 TaktMasterDemandScheduleDto
 */
export interface MasterDemandSchedule extends ApprovalDtoBase {
  /**
   * MasterDemandScheduleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  masterDemandScheduleId: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * MDS 编码（租户+公司+工厂内业务唯一）
   */
  mdsCode: string;

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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  scheduleStatus: number;

  /**
   * MDS 明细行（按物料与时间桶） （子表：TaktMasterDemandScheduleLine）
   */
  lines?: MasterDemandScheduleLine[];

}


/**
 * MasterDemandSchedule 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MasterDemandScheduleQuery
 * @description 对应后端 TaktMasterDemandScheduleQueryDto
 */
export interface MasterDemandScheduleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * MDS 编码（租户+公司+工厂内业务唯一）
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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  scheduleStatus?: number;

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
 * 创建MasterDemandSchedule DTO
 * 对应前端 MasterDemandScheduleCreate
 * @description 对应后端 TaktMasterDemandScheduleCreateDto
 */
export interface MasterDemandScheduleCreate {
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
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * MDS 编码（租户+公司+工厂内业务唯一）
   */
  mdsCode: string;

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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  scheduleStatus: number;

  /**
   * MDS 明细行（按物料与时间桶）（子表，级联保存）
   */
  lines?: MasterDemandScheduleLineCreate[];

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
 * 更新MasterDemandSchedule DTO
 * 继承 TaktMasterDemandScheduleCreateDto，添加 MasterDemandScheduleId 字段
 * 对应前端 MasterDemandScheduleUpdate
 * @description 对应后端 TaktMasterDemandScheduleUpdateDto
 */
export interface MasterDemandScheduleUpdate extends MasterDemandScheduleCreate {
  /**
   * MasterDemandScheduleID（标识要更新的实体）
   */
  masterDemandScheduleId: string;

}


/**
 * MasterDemandSchedule 状态更新 DTO
 * 对应前端 MasterDemandScheduleStatus
 * @description 对应后端 TaktMasterDemandScheduleStatusDto
 */
export interface MasterDemandScheduleStatus {
  /**
   * MasterDemandScheduleID
   */
  masterDemandScheduleId: string;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  scheduleStatus: number;

}


/**
 * MasterDemandSchedule 导入模板行 DTO
 * 对应前端 MasterDemandScheduleTemplate
 * @description 对应后端 TaktMasterDemandScheduleTemplateDto
 */
export interface MasterDemandScheduleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * MDS 编码（租户+公司+工厂内业务唯一）
   */
  mdsCode?: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType?: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  scheduleStatus?: number;

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
 * MasterDemandSchedule 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MasterDemandScheduleImport
 * @description 对应后端 TaktMasterDemandScheduleImportDto
 */
export interface MasterDemandScheduleImport {
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
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * MDS 编码（租户+公司+工厂内业务唯一）
   */
  mdsCode?: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType?: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  scheduleStatus?: number;

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
 * MasterDemandSchedule 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MasterDemandScheduleExport
 * @description 对应后端 TaktMasterDemandScheduleExportDto
 */
export interface MasterDemandScheduleExport {
  /**
   * MasterDemandScheduleID
   */
  masterDemandScheduleId: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * MDS 编码（租户+公司+工厂内业务唯一）
   */
  mdsCode: string;

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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
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

