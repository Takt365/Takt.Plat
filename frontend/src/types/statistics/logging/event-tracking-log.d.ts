// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：event-tracking-log.d.ts
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
 * 前端交互日志实体
 * 对应前端 TaktEventTrackingLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EventTrackingLog
 * @description 对应后端 TaktEventTrackingLogDto
 */
export interface EventTrackingLog extends CompanyDtoBase {
  /**
   * EventTrackingLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  eventTrackingLogId: string;

  /**
   * 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 事件类型（如 longtask）
   */
  eventTrackingType: string;

  /**
   * 事件分类（如 performance）
   */
  eventTrackingCategory: string;

  /**
   * 事件发生时间（客户端 UTC）
   */
  eventTime: string;

  /**
   * 长任务阻塞时长（毫秒）
   */
  durationMs: number;

  /**
   * PerformanceEntry.startTime（毫秒，相对页面导航起点）
   */
  performanceStartMs: number;

  /**
   * PerformanceEntry.name
   */
  entryName: string;

  /**
   * 追踪级别（1=warn 2=error，前端阈值映射）
   */
  trackingLevel: number;

  /**
   * SPA 路由路径
   */
  routePath: string;

  /**
   * 页面完整 URL
   */
  pageUrl: string;

  /**
   * TaskAttribution.containerType
   */
  containerType: string;

  /**
   * TaskAttribution.containerName
   */
  containerName: string;

  /**
   * TaskAttribution.containerSrc
   */
  containerSrc: string;

  /**
   * TaskAttribution.containerId
   */
  containerId: string;

  /**
   * 完整 attribution JSON 数组
   */
  attributionJson: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent: string;

  /**
   * 客户端 IP
   */
  clientIp: string;

}


/**
 * EventTrackingLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EventTrackingLogQuery
 * @description 对应后端 TaktEventTrackingLogQueryDto
 */
export interface EventTrackingLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
   */
  userName?: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 事件类型（如 longtask）
   */
  eventTrackingType?: string;

  /**
   * 事件分类（如 performance）
   */
  eventTrackingCategory?: string;

  /**
   * 事件发生时间（客户端 UTC）（范围查询-开始）
   */
  eventTimeStart?: string;

  /**
   * 事件发生时间（客户端 UTC）（范围查询-结束）
   */
  eventTimeEnd?: string;

  /**
   * 长任务阻塞时长（毫秒）
   */
  durationMs?: number;

  /**
   * PerformanceEntry.startTime（毫秒，相对页面导航起点）
   */
  performanceStartMs?: number;

  /**
   * PerformanceEntry.name
   */
  entryName?: string;

  /**
   * 追踪级别（1=warn 2=error，前端阈值映射）
   */
  trackingLevel?: number;

  /**
   * SPA 路由路径
   */
  routePath?: string;

  /**
   * 页面完整 URL
   */
  pageUrl?: string;

  /**
   * TaskAttribution.containerType
   */
  containerType?: string;

  /**
   * TaskAttribution.containerName
   */
  containerName?: string;

  /**
   * TaskAttribution.containerSrc
   */
  containerSrc?: string;

  /**
   * TaskAttribution.containerId
   */
  containerId?: string;

  /**
   * 完整 attribution JSON 数组
   */
  attributionJson?: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent?: string;

  /**
   * 客户端 IP
   */
  clientIp?: string;

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
 * 创建EventTrackingLog DTO
 * 对应前端 EventTrackingLogCreate
 * @description 对应后端 TaktEventTrackingLogCreateDto
 */
export interface EventTrackingLogCreate {
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
   * 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 事件类型（如 longtask）
   */
  eventTrackingType: string;

  /**
   * 事件分类（如 performance）
   */
  eventTrackingCategory: string;

  /**
   * 事件发生时间（客户端 UTC）
   */
  eventTime: string;

  /**
   * 长任务阻塞时长（毫秒）
   */
  durationMs: number;

  /**
   * PerformanceEntry.startTime（毫秒，相对页面导航起点）
   */
  performanceStartMs: number;

  /**
   * PerformanceEntry.name
   */
  entryName: string;

  /**
   * 追踪级别（1=warn 2=error，前端阈值映射）
   */
  trackingLevel: number;

  /**
   * SPA 路由路径
   */
  routePath: string;

  /**
   * 页面完整 URL
   */
  pageUrl: string;

  /**
   * TaskAttribution.containerType
   */
  containerType: string;

  /**
   * TaskAttribution.containerName
   */
  containerName: string;

  /**
   * TaskAttribution.containerSrc
   */
  containerSrc: string;

  /**
   * TaskAttribution.containerId
   */
  containerId: string;

  /**
   * 完整 attribution JSON 数组
   */
  attributionJson: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent: string;

  /**
   * 客户端 IP
   */
  clientIp: string;

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
 * 更新EventTrackingLog DTO
 * 继承 TaktEventTrackingLogCreateDto，添加 EventTrackingLogId 字段
 * 对应前端 EventTrackingLogUpdate
 * @description 对应后端 TaktEventTrackingLogUpdateDto
 */
export interface EventTrackingLogUpdate extends EventTrackingLogCreate {
  /**
   * EventTrackingLogID（标识要更新的实体）
   */
  eventTrackingLogId: string;

}


/**
 * EventTrackingLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EventTrackingLogExport
 * @description 对应后端 TaktEventTrackingLogExportDto
 */
export interface EventTrackingLogExport {
  /**
   * EventTrackingLogID
   */
  eventTrackingLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 事件类型（如 longtask）
   */
  eventTrackingType: string;

  /**
   * 事件分类（如 performance）
   */
  eventTrackingCategory: string;

  /**
   * 事件发生时间（客户端 UTC）
   */
  eventTime: string;

  /**
   * 长任务阻塞时长（毫秒）
   */
  durationMs: number;

  /**
   * PerformanceEntry.startTime（毫秒，相对页面导航起点）
   */
  performanceStartMs: number;

  /**
   * PerformanceEntry.name
   */
  entryName: string;

  /**
   * 追踪级别（1=warn 2=error，前端阈值映射）
   */
  trackingLevel: number;

  /**
   * SPA 路由路径
   */
  routePath: string;

  /**
   * 页面完整 URL
   */
  pageUrl: string;

  /**
   * TaskAttribution.containerType
   */
  containerType: string;

  /**
   * TaskAttribution.containerName
   */
  containerName: string;

  /**
   * TaskAttribution.containerSrc
   */
  containerSrc: string;

  /**
   * TaskAttribution.containerId
   */
  containerId: string;

  /**
   * 完整 attribution JSON 数组
   */
  attributionJson: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent: string;

  /**
   * 客户端 IP
   */
  clientIp: string;

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

/**
 * Long Task 单条上报 DTO
 */
export interface EventTrackingLogTrackItem {
  eventTrackingType: string;
  eventTrackingCategory: string;
  eventTime: string;
  durationMs: number;
  performanceStartMs: number;
  entryName: string;
  trackingLevel: number;
  routePath: string;
  pageUrl: string;
  containerType: string;
  containerName: string;
  containerSrc: string;
  containerId: string;
  attributionJson?: string;
  userAgent: string;
}

/**
 * Long Task 批量上报 DTO
 */
export interface EventTrackingLogBatchTrack {
  items: EventTrackingLogTrackItem[];
}

/**
 * Long Task 批量上报结果
 */
export interface EventTrackingLogTrackResult {
  count: number;
}

