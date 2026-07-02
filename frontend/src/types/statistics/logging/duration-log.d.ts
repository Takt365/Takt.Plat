// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：duration-log.d.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/logging 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 在线时长日志实体（日汇总）
 * 对应前端 TaktDurationLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 DurationLog
 * @description 对应后端 TaktDurationLogDto
 */
export interface DurationLog extends CompanyDtoBase {
  /**
   * DurationLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  durationLogId: string;

  /**
   * 用户名（登录账号）
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 统计日期（自然日，不含时分秒）
   */
  statDate: string;

  /**
   * 当日累计在线时长（秒）
   */
  durationSeconds: number;

}


/**
 * DurationLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DurationLogQuery
 * @description 对应后端 TaktDurationLogQueryDto
 */
export interface DurationLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 用户名（登录账号）
   */
  userName?: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 统计日期（自然日，不含时分秒）（范围查询-开始）
   */
  statDateStart?: string;

  /**
   * 统计日期（自然日，不含时分秒）（范围查询-结束）
   */
  statDateEnd?: string;

  /**
   * 当日累计在线时长（秒）
   */
  durationSeconds?: number;

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
 * 创建DurationLog DTO
 * 对应前端 DurationLogCreate
 * @description 对应后端 TaktDurationLogCreateDto
 */
export interface DurationLogCreate {
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
   * 用户名（登录账号）
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 统计日期（自然日，不含时分秒）
   */
  statDate: string;

  /**
   * 当日累计在线时长（秒）
   */
  durationSeconds: number;

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
 * 更新DurationLog DTO
 * 继承 TaktDurationLogCreateDto，添加 DurationLogId 字段
 * 对应前端 DurationLogUpdate
 * @description 对应后端 TaktDurationLogUpdateDto
 */
export interface DurationLogUpdate extends DurationLogCreate {
  /**
   * DurationLogID（标识要更新的实体）
   */
  durationLogId: string;

}


/**
 * DurationLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DurationLogExport
 * @description 对应后端 TaktDurationLogExportDto
 */
export interface DurationLogExport {
  /**
   * DurationLogID
   */
  durationLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 用户名（登录账号）
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 统计日期（自然日，不含时分秒）
   */
  statDate: string;

  /**
   * 当日累计在线时长（秒）
   */
  durationSeconds: number;

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

