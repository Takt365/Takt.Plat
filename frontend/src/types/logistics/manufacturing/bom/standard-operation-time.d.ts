// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：standard-operation-time.d.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 标准工序时间实体（基于 SAP PP 标准工时）
 * 对应前端 TaktStandardOperationTimeDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 StandardOperationTime
 * @description 对应后端 TaktStandardOperationTimeDto
 */
export interface StandardOperationTime extends ApprovalDtoBase {
  /**
   * StandardOperationTimeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  standardOperationTimeId: string;

  /**
   * 工厂代码（选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 物料编码（选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options，按工厂 ExtValue 过滤）
   */
  workCenter: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 标准工时（分钟）
   */
  standardMinutes: number;

  /**
   * 工时单位（字典 logistics_time_unit，默认 MIN）
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位（字典 logistics_points_unit，默认 SHORT）
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（字典 logistics_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045）
   */
  pointsToMinutesRate: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 标准工序时间变更记录列表（外键在子表 TaktStandardOperationTimeChangeLog.StandardOperationTimeId） （子表：TaktStandardOperationTimeChangeLog）
   */
  changeLogs?: StandardOperationTimeChangeLog[];

}


/**
 * StandardOperationTime 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 StandardOperationTimeQuery
 * @description 对应后端 TaktStandardOperationTimeQueryDto
 */
export interface StandardOperationTimeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 物料编码（选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options，按工厂 ExtValue 过滤）
   */
  workCenter?: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 标准工时（分钟）
   */
  standardMinutes?: number;

  /**
   * 工时单位（字典 logistics_time_unit，默认 MIN）
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位（字典 logistics_points_unit，默认 SHORT）
   */
  pointsUnit?: string;

  /**
   * 点数转分钟汇率（字典 logistics_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045）
   */
  pointsToMinutesRate?: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  expiryDateEnd?: string;

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
 * 创建StandardOperationTime DTO
 * 对应前端 StandardOperationTimeCreate
 * @description 对应后端 TaktStandardOperationTimeCreateDto
 */
export interface StandardOperationTimeCreate {
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
   * 工厂代码（选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 物料编码（选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options，按工厂 ExtValue 过滤）
   */
  workCenter: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 标准工时（分钟）
   */
  standardMinutes: number;

  /**
   * 工时单位（字典 logistics_time_unit，默认 MIN）
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位（字典 logistics_points_unit，默认 SHORT）
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（字典 logistics_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045）
   */
  pointsToMinutesRate: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 标准工序时间变更记录列表（外键在子表 TaktStandardOperationTimeChangeLog.StandardOperationTimeId）（子表，级联保存）
   */
  changeLogs?: StandardOperationTimeChangeLogCreate[];

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
 * 更新StandardOperationTime DTO
 * 继承 TaktStandardOperationTimeCreateDto，添加 StandardOperationTimeId 字段
 * 对应前端 StandardOperationTimeUpdate
 * @description 对应后端 TaktStandardOperationTimeUpdateDto
 */
export interface StandardOperationTimeUpdate extends StandardOperationTimeCreate {
  /**
   * StandardOperationTimeID（标识要更新的实体）
   */
  standardOperationTimeId: string;

}


/**
 * StandardOperationTime 导入模板行 DTO
 * 对应前端 StandardOperationTimeTemplate
 * @description 对应后端 TaktStandardOperationTimeTemplateDto
 */
export interface StandardOperationTimeTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 物料编码（选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options，按工厂 ExtValue 过滤）
   */
  workCenter?: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 标准工时（分钟）
   */
  standardMinutes?: number;

  /**
   * 工时单位（字典 logistics_time_unit，默认 MIN）
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位（字典 logistics_points_unit，默认 SHORT）
   */
  pointsUnit?: string;

  /**
   * 点数转分钟汇率（字典 logistics_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045）
   */
  pointsToMinutesRate?: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 标准工序时间变更记录列表（外键在子表 TaktStandardOperationTimeChangeLog.StandardOperationTimeId）（子表，级联保存）
   */
  changeLogs?: StandardOperationTimeChangeLogCreate[];

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
 * StandardOperationTime 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 StandardOperationTimeImport
 * @description 对应后端 TaktStandardOperationTimeImportDto
 */
export interface StandardOperationTimeImport {
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
   * 工厂代码（选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 物料编码（选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options，按工厂 ExtValue 过滤）
   */
  workCenter?: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 标准工时（分钟）
   */
  standardMinutes?: number;

  /**
   * 工时单位（字典 logistics_time_unit，默认 MIN）
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位（字典 logistics_points_unit，默认 SHORT）
   */
  pointsUnit?: string;

  /**
   * 点数转分钟汇率（字典 logistics_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045）
   */
  pointsToMinutesRate?: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 标准工序时间变更记录列表（外键在子表 TaktStandardOperationTimeChangeLog.StandardOperationTimeId）（子表，级联保存）
   */
  changeLogs?: StandardOperationTimeChangeLogCreate[];

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
 * StandardOperationTime 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 StandardOperationTimeExport
 * @description 对应后端 TaktStandardOperationTimeExportDto
 */
export interface StandardOperationTimeExport {
  /**
   * StandardOperationTimeID
   */
  standardOperationTimeId: string;

  /**
   * 工厂代码（选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 物料编码（选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options，按工厂 ExtValue 过滤）
   */
  workCenter: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 标准工时（分钟）
   */
  standardMinutes: number;

  /**
   * 工时单位（字典 logistics_time_unit，默认 MIN）
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位（字典 logistics_points_unit，默认 SHORT）
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（字典 logistics_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045）
   */
  pointsToMinutesRate: string;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

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

