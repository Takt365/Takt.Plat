// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/bootstrap
// 文件名称：takt-idle-session.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：用户无操作超时自动登出（wall-clock 巡检 + 可选预警 Modal → 自动跳转登录页）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { effectScope, watch } from 'vue';
import { storeToRefs } from 'pinia';
import { Modal } from 'ant-design-vue';
import {
  getAuthIdleTimeoutMs,
  getAuthIdleWarningMinutes,
  getAuthIdleWarningMs,
  isAuthIdleLogoutEnabled,
} from '@/config/auth-idle';
import { executeIdleLogoutAsync } from '@/bootstrap/takt-event-handlers';
import { isLogoutInProgress } from '@/bootstrap/takt-logout-flow';
import { useUserStore } from '@/stores/identity/user';
import {
  TAKT_IDLE_ACTIVITY_EVENTS,
  TAKT_IDLE_ACTIVITY_THROTTLE_MS,
} from '@/utils/common';
import { translateLocaleMessage } from '@/utils/takt-i18n-message';
import { EventBus } from '@/utils/event-bus';
import { createLogger } from '@/utils/logger';

/** 空闲会话模块日志实例 */
const idleSessionLogger = createLogger('takt-idle-session');

/** 空闲超时 setTimeout 句柄；null 表示未调度 */
let idleTimer: ReturnType<typeof setTimeout> | null = null;

/** 预警 Modal 到期后强制登出的计时器 */
let warningLogoutTimer: ReturnType<typeof setTimeout> | null = null;

/** 关闭预警 Modal 的 destroy 回调 */
let destroyWarningModal: (() => void) | null = null;

/** 上次真实用户活动的 Unix 时间戳（毫秒） */
let lastActivityAt = 0;

/** 是否正在执行空闲超时登出，防止重入 */
let isHandlingIdle = false;

/** 是否已向 window/document 注册活动与可见性监听 */
let listenersAttached = false;

/** 空闲 wall-clock 巡检（缓解后台标签页 setTimeout 节流导致无法自动登出） */
let idleWatchInterval: ReturnType<typeof setInterval> | null = null;

/** 巡检间隔（毫秒） */
const IDLE_WATCH_INTERVAL_MS = 15_000;

/**
 * 清除空闲计时器
 */
function clearIdleTimer(): void {
  if (idleTimer !== null) {
    clearTimeout(idleTimer);
    idleTimer = null;
  }
}

/**
 * 清除预警 Modal 与到期计时
 */
function clearIdleWarningState(): void {
  if (warningLogoutTimer !== null) {
    clearTimeout(warningLogoutTimer);
    warningLogoutTimer = null;
  }

  if (destroyWarningModal) {
    destroyWarningModal();
    destroyWarningModal = null;
  }
}

/**
 * 自上次真实活动以来经过的毫秒数
 */
function getIdleElapsedMs(): number {
  if (lastActivityAt <= 0) {
    return 0;
  }

  return Date.now() - lastActivityAt;
}

/**
 * 距离空闲总超时还剩多少毫秒（≤0 表示应立刻登出）
 */
function getIdleRemainingMs(): number {
  const timeoutMs = getAuthIdleTimeoutMs();
  if (timeoutMs <= 0) {
    return Number.POSITIVE_INFINITY;
  }
  return timeoutMs - getIdleElapsedMs();
}

/**
 * 按 wall-clock 校验是否应预警或登出（不依赖可能被节流的 setTimeout）
 */
function enforceIdlePolicyByWallClock(): void {
  const userStore = useUserStore();
  if (!userStore.isLoggedIn || isHandlingIdle || isLogoutInProgress()) {
    return;
  }

  const timeoutMs = getAuthIdleTimeoutMs();
  if (timeoutMs <= 0) {
    return;
  }

  const remainingMs = getIdleRemainingMs();
  if (remainingMs <= 0) {
    void handleIdleTimeout();
    return;
  }

  const warningMs = getAuthIdleWarningMs();
  if (warningMs > 0 && !destroyWarningModal) {
    const warningAtMs = timeoutMs - warningMs;
    const elapsedMs = getIdleElapsedMs();
    if (elapsedMs >= warningAtMs) {
      const warningMinutes = Math.max(1, Math.ceil(remainingMs / 60_000));
      showIdleWarningModal(warningMinutes, remainingMs);
    }
  }
}

/**
 * 弹出空闲预警（layouts.page.session.*）
 * @param warningMinutes Modal 文案分钟数（向上取整，至少 1）
 * @param remainingMs 距离总超时剩余毫秒（用于自动登出计时，避免 countdown 长于实际剩余时间）
 */
function showIdleWarningModal(warningMinutes: number, remainingMs?: number): void {
  if (destroyWarningModal || isHandlingIdle || isLogoutInProgress()) {
    return;
  }

  const userStore = useUserStore();
  if (!userStore.isLoggedIn) {
    return;
  }

  clearIdleTimer();

  const minutes = Math.max(1, warningMinutes);
  const logoutAfterMs = Math.max(
    1_000,
    remainingMs ?? minutes * 60_000,
  );
  const modal = Modal.confirm({
    title: translateLocaleMessage('layouts.page.session.title'),
    content: translateLocaleMessage('layouts.page.session.content', { minutes }),
    okText: translateLocaleMessage('layouts.page.session.oktext'),
    cancelText: translateLocaleMessage('layouts.page.session.canceltext'),
    centered: true,
    maskClosable: false,
    onOk: () => {
      clearIdleWarningState();
      recordUserActivityAndReschedule();
    },
    onCancel: () => {
      clearIdleWarningState();
      void handleIdleTimeout();
    },
  });

  destroyWarningModal = modal.destroy;

  warningLogoutTimer = setTimeout(() => {
    clearIdleWarningState();
    void handleIdleTimeout();
  }, logoutAfterMs);
}

/**
 * 按 lastActivityAt 剩余空闲时间调度（预警 → 登出）
 */
function scheduleIdleTimer(): void {
  const timeoutMs = getAuthIdleTimeoutMs();

  if (timeoutMs <= 0) {
    clearIdleTimer();
    clearIdleWarningState();
    return;
  }

  clearIdleTimer();

  if (!destroyWarningModal) {
    clearIdleWarningState();
  }

  const elapsedMs = getIdleElapsedMs();
  const remainingMs = timeoutMs - elapsedMs;

  if (remainingMs <= 0) {
    void handleIdleTimeout();
    return;
  }

  const warningMs = getAuthIdleWarningMs();

  if (warningMs > 0) {
    const warningAtMs = timeoutMs - warningMs;
    const untilWarningMs = warningAtMs - elapsedMs;

    if (untilWarningMs <= 0) {
      if (!destroyWarningModal && !isHandlingIdle) {
        const warningMinutes = Math.max(1, Math.ceil(remainingMs / 60_000));
        showIdleWarningModal(warningMinutes, remainingMs);
      }
      return;
    }

    idleTimer = setTimeout(() => {
      const msLeft = getIdleRemainingMs();
      showIdleWarningModal(getAuthIdleWarningMinutes(), msLeft);
    }, untilWarningMs);
    return;
  }

  idleTimer = setTimeout(() => {
    void handleIdleTimeout();
  }, remainingMs);
}

/**
 * 空闲超时：统一走 executeIdleLogoutAsync（signOut → 清前端 → idle 文案）
 */
async function handleIdleTimeout(): Promise<void> {
  if (isHandlingIdle || isLogoutInProgress()) {
    return;
  }

  const userStore = useUserStore();
  if (!userStore.isLoggedIn) {
    clearIdleTimer();
    clearIdleWarningState();
    return;
  }

  isHandlingIdle = true;
  clearIdleTimer();
  clearIdleWarningState();

  idleSessionLogger.info('用户空闲超时，执行自动登出', { action: 'idle-timeout' });

  const message = translateLocaleMessage('common.tip.session.idle.logout');

  try {
    await executeIdleLogoutAsync(message);
  } finally {
    isHandlingIdle = false;
  }
}

/**
 * 记录真实用户活动并重新调度
 */
function recordUserActivityAndReschedule(): void {
  lastActivityAt = Date.now();
  scheduleIdleTimer();
}

/**
 * 记录用户活动并重置空闲计时
 * @param event 可选 DOM 事件；非 isTrusted 不计入
 */
function onUserActivity(event?: Event): void {
  if (event && !event.isTrusted) {
    return;
  }

  const userStore = useUserStore();

  if (!userStore.isLoggedIn || isHandlingIdle || destroyWarningModal) {
    return;
  }

  const now = Date.now();

  if (now - lastActivityAt < TAKT_IDLE_ACTIVITY_THROTTLE_MS) {
    return;
  }

  recordUserActivityAndReschedule();
}

/**
 * 标签页重新可见 / 窗口聚焦：按 wall-clock 校验已空闲时长（弹窗已显示时也须强制登出）
 */
function onVisibilityOrFocusResume(): void {
  if (document.hidden) {
    return;
  }

  const userStore = useUserStore();
  if (!userStore.isLoggedIn || isHandlingIdle) {
    return;
  }

  enforceIdlePolicyByWallClock();
  if (!isHandlingIdle) {
    scheduleIdleTimer();
  }
}

/**
 * 启动 wall-clock 巡检
 */
function startIdleWatchInterval(): void {
  if (idleWatchInterval !== null) {
    return;
  }

  idleWatchInterval = setInterval(() => {
    enforceIdlePolicyByWallClock();
  }, IDLE_WATCH_INTERVAL_MS);
}

/**
 * 停止 wall-clock 巡检
 */
function stopIdleWatchInterval(): void {
  if (idleWatchInterval !== null) {
    clearInterval(idleWatchInterval);
    idleWatchInterval = null;
  }
}

/**
 * 注册全局活动监听
 */
function attachActivityListeners(): void {
  if (listenersAttached) {
    return;
  }

  listenersAttached = true;

  TAKT_IDLE_ACTIVITY_EVENTS.forEach((eventName) => {
    window.addEventListener(eventName, onUserActivity, { passive: true, capture: true });
  });

  document.addEventListener('visibilitychange', onVisibilityOrFocusResume);
  window.addEventListener('focus', onVisibilityOrFocusResume);
}

/**
 * 初始化空闲会话监控（须在 app.use(pinia) 之后调用）
 */
export function initTaktIdleSession(): void {
  if (!isAuthIdleLogoutEnabled()) {
    idleSessionLogger.info('空闲自动登出已禁用', { action: 'init-skip' });
    return;
  }

  attachActivityListeners();

  const scope = effectScope(true);
  scope.run(() => {
    const userStore = useUserStore();
    const { token } = storeToRefs(userStore);

    watch(
      token,
      (accessToken) => {
        if (accessToken) {
          lastActivityAt = Date.now();
          scheduleIdleTimer();
          startIdleWatchInterval();
          return;
        }

        clearIdleTimer();
        clearIdleWarningState();
        stopIdleWatchInterval();
        isHandlingIdle = false;
        lastActivityAt = 0;
      },
      { immediate: true },
    );
  });

  EventBus.on('user:login', () => {
    lastActivityAt = Date.now();
    scheduleIdleTimer();
    startIdleWatchInterval();
  });

  idleSessionLogger.info('空闲自动登出已启用', {
    action: 'init',
    timeoutMinutes: getAuthIdleTimeoutMs() / 60_000,
    warningMinutes: getAuthIdleWarningMinutes(),
  });
}
