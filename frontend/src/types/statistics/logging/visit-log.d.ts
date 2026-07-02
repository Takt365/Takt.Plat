// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：visit-log.d.ts
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
 * 用户日访问量统计实体
 * 对应前端 TaktVisitLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 VisitLog
 * @description 对应后端 TaktVisitLogDto
 */
export interface VisitLog extends CompanyDtoBase {
  /**
   * VisitLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  visitLogId: string;

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
   * 当日访问次数（成功登录/进入系统次数）
   */
  visitCount: number;

}


/**
 * VisitLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 VisitLogQuery
 * @description 对应后端 TaktVisitLogQueryDto
 */
export interface VisitLogQuery extends TaktPagedQuery {
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
   * 当日访问次数（成功登录/进入系统次数）
   */
  visitCount?: number;

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
 * 创建VisitLog DTO
 * 对应前端 VisitLogCreate
 * @description 对应后端 TaktVisitLogCreateDto
 */
export interface VisitLogCreate {
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
   * 当日访问次数（成功登录/进入系统次数）
   */
  visitCount: number;

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
 * 更新VisitLog DTO
 * 继承 TaktVisitLogCreateDto，添加 VisitLogId 字段
 * 对应前端 VisitLogUpdate
 * @description 对应后端 TaktVisitLogUpdateDto
 */
export interface VisitLogUpdate extends VisitLogCreate {
  /**
   * VisitLogID（标识要更新的实体）
   */
  visitLogId: string;

}


/**
 * VisitLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 VisitLogExport
 * @description 对应后端 TaktVisitLogExportDto
 */
export interface VisitLogExport {
  /**
   * VisitLogID
   */
  visitLogId: string;

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
   * 当日访问次数（成功登录/进入系统次数）
   */
  visitCount: number;

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

