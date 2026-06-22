// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：standard-operation-rate-change-log.d.ts
// 创建时间：2026-06-21
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
 * 标准生产稼动率变更记录实体
 * 对应前端 TaktStandardOperationRateChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 StandardOperationRateChangeLog
 * @description 对应后端 TaktStandardOperationRateChangeLogDto
 */
export interface StandardOperationRateChangeLog extends CompanyDtoBase {
  /**
   * StandardOperationRateChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  standardOperationRateChangeLogId: string;

  /**
   * 标准生产稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  standardOperationRateId: string;

  /**
   * 标准生产稼动率名称（填充字段）
   */
  standardOperationRateName?: string;

  /**
   * 工厂代码（冗余）
   */
  plantCode: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 标准生产稼动率主表 （主表：TaktStandardOperationRate）
   */
  standardOperationRate?: StandardOperationRate;

}


/**
 * StandardOperationRateChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 StandardOperationRateChangeLogQuery
 * @description 对应后端 TaktStandardOperationRateChangeLogQueryDto
 */
export interface StandardOperationRateChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 标准生产稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  standardOperationRateId?: string;

  /**
   * 工厂代码（冗余）
   */
  plantCode?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）
   */
  changeFields?: string;

  /**
   * 变更时间（范围查询-开始）
   */
  changeTimeStart?: string;

  /**
   * 变更时间（范围查询-结束）
   */
  changeTimeEnd?: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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
 * 创建StandardOperationRateChangeLog DTO
 * 对应前端 StandardOperationRateChangeLogCreate
 * @description 对应后端 TaktStandardOperationRateChangeLogCreateDto
 */
export interface StandardOperationRateChangeLogCreate {
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
   * 标准生产稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  standardOperationRateId: string;

  /**
   * 工厂代码（冗余）
   */
  plantCode: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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
 * 更新StandardOperationRateChangeLog DTO
 * 继承 TaktStandardOperationRateChangeLogCreateDto，添加 StandardOperationRateChangeLogId 字段
 * 对应前端 StandardOperationRateChangeLogUpdate
 * @description 对应后端 TaktStandardOperationRateChangeLogUpdateDto
 */
export interface StandardOperationRateChangeLogUpdate extends StandardOperationRateChangeLogCreate {
  /**
   * StandardOperationRateChangeLogID（标识要更新的实体）
   */
  standardOperationRateChangeLogId: string;

}


/**
 * StandardOperationRateChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 StandardOperationRateChangeLogExport
 * @description 对应后端 TaktStandardOperationRateChangeLogExportDto
 */
export interface StandardOperationRateChangeLogExport {
  /**
   * StandardOperationRateChangeLogID
   */
  standardOperationRateChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 标准生产稼动率ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  standardOperationRateId: string;

  /**
   * 工厂代码（冗余）
   */
  plantCode: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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

