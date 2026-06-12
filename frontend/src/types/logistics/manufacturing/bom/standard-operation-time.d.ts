// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：standard-operation-time.d.ts
// 创建时间：2026-06-09
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 工作中心
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
   * 工时单位
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（1 点数 = 多少分钟）
   */
  pointsToMinutesRate: number;

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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 工作中心
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
   * 工时单位
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位
   */
  pointsUnit?: string;

  /**
   * 点数转分钟汇率（1 点数 = 多少分钟）
   */
  pointsToMinutesRate?: number;

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
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 工作中心
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
   * 工时单位
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（1 点数 = 多少分钟）
   */
  pointsToMinutesRate: number;

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
  extFieldJson?: string;

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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 工时单位
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位
   */
  pointsUnit?: string;

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
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 工时单位
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位
   */
  pointsUnit?: string;

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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 工作中心
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
   * 工时单位
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（1 点数 = 多少分钟）
   */
  pointsToMinutesRate: number;

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

