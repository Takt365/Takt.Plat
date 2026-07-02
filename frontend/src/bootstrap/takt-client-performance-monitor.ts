// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/bootstrap
// 文件名称：takt-client-performance-monitor.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：客户端性能监控统一入口（Long Task + FPS + CRUD API + Web Vitals + 关联诊断）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { initTaktFpsMonitor, stopTaktFpsMonitor } from '@/bootstrap/takt-fps-monitor';
import { initTaktLongTaskMonitor, stopTaktLongTaskMonitor } from '@/bootstrap/takt-long-task-monitor';
import { initTaktWebVitalsMonitor, stopTaktWebVitalsMonitor } from '@/bootstrap/takt-web-vitals-monitor';
import router from '@/router';
import {
  isEventTrackingEnabled,
  isEventTrackingReportEnabled,
} from '@/config/event-tracking';
import {
  startEventTrackingReporter,
  stopEventTrackingReporter,
} from '@/utils/takt-event-tracking-reporter';
import { createLogger } from '@/utils/logger';

/** 统一监控日志 */
const monitorLogger = createLogger('takt-client-performance-monitor');

/** 是否已初始化 */
let initialized = false;

/**
 * 启动三位一体客户端性能监控
 * @description 须在 initLogger 与 axios 实例创建后调用
 */
export function initTaktClientPerformanceMonitor(): void {
  if (typeof window === 'undefined' || initialized) {
    return;
  }
  if (!isEventTrackingEnabled()) {
    monitorLogger.debug('客户端性能监控总开关已关闭', { action: 'skip' });
    return;
  }
  if (isEventTrackingReportEnabled()) {
    startEventTrackingReporter();
  }
  initTaktLongTaskMonitor();
  initTaktFpsMonitor(router);
  initTaktWebVitalsMonitor();
  initialized = true;
  monitorLogger.info('客户端性能监控已启动', {
    action: 'init',
    longTask: true,
    fps: true,
    api: true,
    webVitals: true,
    diagnosis: true,
    report: isEventTrackingReportEnabled(),
  });
}

/**
 * 停止客户端性能监控
 */
export function stopTaktClientPerformanceMonitor(): void {
  stopTaktLongTaskMonitor();
  stopTaktFpsMonitor();
  stopTaktWebVitalsMonitor();
  stopEventTrackingReporter();
  initialized = false;
}
