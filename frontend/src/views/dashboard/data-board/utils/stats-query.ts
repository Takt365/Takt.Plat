// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/views/dashboard/data-board/utils
// 文件名称：stats-query.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：数据看板统计查询辅助（静默请求、分页 total、有界聚合）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import dayjs from 'dayjs';
import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktMaxPageSize,
} from '@/utils/takt-paged';
import { createLogger } from '@/utils/logger';

const statsQueryLogger = createLogger('stats-query');

/** 看板统计 list / GET 路径（相对 /api） */
export const DASHBOARD_STATS_API = {
  flowTodoList: 'TaktFlowEngine/todo/list',
  messageStatistics: 'TaktMessages/statistics',
  messageUnreadList: 'TaktMessages/unread-list',
  onlineDashboard: 'TaktOnlines/statistics/dashboard',
  salesOrderList: 'TaktSalesOrders/list',
  /** 销售发票统计（本月销售；TaktSalesInvoices/invoice-stat） */
  salesInvoiceStat: 'TaktSalesInvoices/invoice-stat',
  /** 设变部门执行行数统计（8 张部门表；TaktEcKanbans/dept-execution-count） */
  ecDeptExecutionCount: 'TaktEcKanbans/dept-execution-count',
  /** 设变实施路径 / 卡点部门（TaktEcKanban / ITaktEcKanbanService） */
  ecKanbanList: 'TaktEcKanbans/list',
  /** 设变主表 + 子表明细数量统计（TaktEcGijutsu / ITaktEcGijutsuService） */
  ecStat: 'TaktEcGijutsus/stat',
  productionOrderList: 'TaktProductionOrders/list',
  personnelOperationRateList: 'TaktPersonnelOperationRates/list',
  assyOutputProductionStat: 'TaktAssyOutputs/production-stat',
  pcbaOutputProductionStat: 'TaktPcbaOutputs/production-stat',
} as const;

/** 看板指标对应 list 权限（无权限时不发请求） */
export const DASHBOARD_STATS_PERMISSION = {
  flowTodoList: 'workflow:todo:list',
  messageStatistics: 'foundation:message:query',
  messageUnreadList: 'foundation:message:unread',
  onlineDashboard: 'foundation:online:list',
  salesOrderList: 'logistics:sales:order:list',
  salesInvoiceStat: 'logistics:sales:invoice:list',
  ecDeptExecutionCount: 'logistics:manufacturing:engineering:change:kanban:list',
  ecKanbanList: 'logistics:manufacturing:engineering:change:kanban:list',
  ecList: 'logistics:manufacturing:engineering:change:gijutsu:list',
  productionOrderList: 'logistics:manufacturing:output:production:order:list',
  personnelOperationRateList: 'logistics:manufacturing:output:personnel:operation:rate:list',
  assyOutputList: 'logistics:manufacturing:output:assy:list',
  pcbaOutputList: 'logistics:manufacturing:output:pcba:list',
} as const;

/** 日期区间 */
export interface TaktDateRange {
  start: string;
  end: string;
}

/**
 * 获取当前自然月区间（含当日）
 * @returns {TaktDateRange} YYYY-MM-DD HH:mm:ss
 */
export function getCurrentMonthRange(): TaktDateRange {
  const start = dayjs().startOf('month').format('YYYY-MM-DD 00:00:00');
  const end = dayjs().endOf('month').format('YYYY-MM-DD 23:59:59');
  return { start, end };
}

/**
 * 获取去年同期自然月区间
 * @returns {TaktDateRange} YYYY-MM-DD HH:mm:ss
 */
export function getSameMonthLastYearRange(): TaktDateRange {
  const anchor = dayjs().subtract(1, 'year');
  return {
    start: anchor.startOf('month').format('YYYY-MM-DD 00:00:00'),
    end: anchor.endOf('month').format('YYYY-MM-DD 23:59:59'),
  };
}

/**
 * 计算同比增幅（百分比）
 * @param current 本期值
 * @param previous 同期值
 * @returns {number} 增幅百分比
 */
export function calcYoYPercent(current: number, previous: number): number {
  if (previous <= 0) {
    return current > 0 ? 100 : 0;
  }
  return ((current - previous) / previous) * 100;
}

/**
 * 延迟执行看板数据加载，避免路由切换时与首屏渲染争抢主线程
 * @param task 加载任务
 */
export function scheduleDashboardLoad(task: () => void | Promise<void>): void {
  const run = (): void => {
    void task();
  };
  if (typeof window !== 'undefined' && 'requestIdleCallback' in window) {
    window.requestIdleCallback(run, { timeout: 1500 });
    return;
  }
  setTimeout(run, 16);
}

/**
 * 看板专用：部门执行行数（TaktEcKanbans/dept-execution-count）
 * @param isImplemented 是否实施（0/1；省略=全部）
 * @returns {Promise<number>} 行数
 */
export async function fetchDashboardDeptExecutionCount(isImplemented?: number): Promise<number> {
  const params = isImplemented === undefined ? undefined : { isImplemented };
  const data = await fetchDashboardGet<{ count: number }>(DASHBOARD_STATS_API.ecDeptExecutionCount, params);
  return data?.count ?? 0;
}

/**
 * 看板静默 GET（失败不弹全局通知）
 * @param path API 路径（相对 /api）
 * @param params 查询参数
 * @returns {Promise<T | null>} 业务 data 或 null
 */
export async function fetchDashboardGet<T>(
  path: string,
  params?: Record<string, unknown>,
): Promise<T | null> {
  try {
    return await request<T>({
      url: path,
      method: 'get',
      params,
      skipErrorNotification: true,
    });
  } catch (error: unknown) {
    statsQueryLogger.warn('看板 GET 失败', { action: 'fetchDashboardGet', path }, error);
    return null;
  }
}

/**
 * 有权限时执行看板指标加载
 * @param permitted 是否具备权限
 * @param label 指标标识
 * @param fetcher 加载函数
 * @param fallback 失败默认值
 * @returns {Promise<T>} 指标值
 */
export async function fetchDashboardMetricIfPermitted<T>(
  permitted: boolean,
  label: string,
  fetcher: () => Promise<T>,
  fallback: T,
): Promise<T> {
  if (!permitted) {
    return fallback;
  }
  return fetchMetricSafely(label, fetcher, fallback);
}

/**
 * 看板单项指标安全加载（失败时返回默认值，不影响其它指标）
 * @param label 指标标识（日志用）
 * @param fetcher 加载函数
 * @param fallback 失败时的默认值
 * @returns {Promise<T>} 指标值
 */
export async function fetchMetricSafely<T>(
  label: string,
  fetcher: () => Promise<T>,
  fallback: T,
): Promise<T> {
  try {
    return await fetcher();
  } catch (error: unknown) {
    statsQueryLogger.warn(`指标 ${label} 加载失败`, { action: 'fetchMetricSafely', label }, error);
    return fallback;
  }
}

/**
 * 看板专用：分页 list 仅取 total（静默、pageSize=1）
 * @param path list API 路径
 * @param query 额外查询条件
 * @returns {Promise<number>} 总条数
 */
export async function fetchDashboardPagedTotal(
  path: string,
  query: Record<string, unknown> = {},
): Promise<number> {
  await ensureTaktPaginationConfigAsync();
  const res = await fetchDashboardGet<TaktPagedResult<unknown>>(path, {
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize: 1,
  });
  return res?.total ?? 0;
}

/**
 * 分页列表仅取 total（pageSize=1，避免全量拉数）
 * @param fetcher 列表 API
 * @param query 额外查询条件
 * @returns {Promise<number>} 总条数
 */
export async function fetchPagedTotal<T>(
  fetcher: (query: Record<string, unknown>) => Promise<TaktPagedResult<T>>,
  query: Record<string, unknown> = {},
): Promise<number> {
  await ensureTaktPaginationConfigAsync();
  const res = await fetcher({
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize: 1,
  });
  return res.total ?? 0;
}

/** 有界求和结果 */
export interface TaktBoundedSumResult {
  sum: number;
  total: number;
  sampled: number;
}

/**
 * 看板专用：有界行数内求和（静默）
 * @param path list API 路径
 * @param pickValue 取值函数
 * @param query 额外查询条件
 * @returns {Promise<TaktBoundedSumResult>} 求和结果
 */
export async function fetchDashboardBoundedSum<T>(
  path: string,
  pickValue: (item: T) => number,
  query: Record<string, unknown> = {},
): Promise<TaktBoundedSumResult> {
  await ensureTaktPaginationConfigAsync();
  const pageSize = getTaktMaxPageSize();
  const res = await fetchDashboardGet<TaktPagedResult<T>>(path, {
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize,
  });
  const rows = res?.data ?? [];
  let sum = 0;
  rows.forEach((item) => {
    const value = pickValue(item);
    if (Number.isFinite(value)) {
      sum += value;
    }
  });
  return {
    sum,
    total: res?.total ?? rows.length,
    sampled: rows.length,
  };
}

/**
 * 在有界行数内对数值字段求和（用于看板聚合，非全量统计）
 * @param fetcher 列表 API
 * @param pickValue 取值函数
 * @param query 额外查询条件
 * @returns {Promise<TaktBoundedSumResult>} 求和结果
 */
export async function fetchBoundedSum<T>(
  fetcher: (query: Record<string, unknown>) => Promise<TaktPagedResult<T>>,
  pickValue: (item: T) => number,
  query: Record<string, unknown> = {},
): Promise<TaktBoundedSumResult> {
  await ensureTaktPaginationConfigAsync();
  const pageSize = getTaktMaxPageSize();
  const res = await fetcher({
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize,
  });
  const rows = res.data ?? [];
  let sum = 0;
  rows.forEach((item) => {
    const value = pickValue(item);
    if (Number.isFinite(value)) {
      sum += value;
    }
  });
  return {
    sum,
    total: res.total ?? rows.length,
    sampled: rows.length,
  };
}

/**
 * 看板专用：有界行数内求平均（静默）
 * @param path list API 路径
 * @param pickValue 取值函数
 * @param query 额外查询条件
 * @returns {Promise<number>} 平均值
 */
export async function fetchDashboardBoundedAverage<T>(
  path: string,
  pickValue: (item: T) => number,
  query: Record<string, unknown> = {},
): Promise<number> {
  await ensureTaktPaginationConfigAsync();
  const pageSize = getTaktMaxPageSize();
  const res = await fetchDashboardGet<TaktPagedResult<T>>(path, {
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize,
  });
  const rows = res?.data ?? [];
  if (rows.length === 0) {
    return 0;
  }
  let sum = 0;
  let count = 0;
  rows.forEach((item) => {
    const value = pickValue(item);
    if (Number.isFinite(value)) {
      sum += value;
      count += 1;
    }
  });
  return count > 0 ? sum / count : 0;
}

/**
 * 在有界行数内对数值字段求平均
 * @param fetcher 列表 API
 * @param pickValue 取值函数
 * @param query 额外查询条件
 * @returns {Promise<number>} 平均值（无数据时 0）
 */
export async function fetchBoundedAverage<T>(
  fetcher: (query: Record<string, unknown>) => Promise<TaktPagedResult<T>>,
  pickValue: (item: T) => number,
  query: Record<string, unknown> = {},
): Promise<number> {
  await ensureTaktPaginationConfigAsync();
  const pageSize = getTaktMaxPageSize();
  const res = await fetcher({
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize,
  });
  const rows = res.data ?? [];
  if (rows.length === 0) {
    return 0;
  }
  let sum = 0;
  let count = 0;
  rows.forEach((item) => {
    const value = pickValue(item);
    if (Number.isFinite(value)) {
      sum += value;
      count += 1;
    }
  });
  return count > 0 ? sum / count : 0;
}
