// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/bootstrap
// 文件名称：takt-idle-session.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：用户无操作超时自动登出（监听指针/键盘/滚动等活动并重置计时）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { effectScope, watch } from 'vue';
import { storeToRefs } from 'pinia';
import { signOutSession } from '@/api/identity/auths';
import { getAuthIdleTimeoutMs, isAuthIdleLogoutEnabled } from '@/config/auth-idle';
import { useUserStore } from '@/stores/identity/user';
import {
  TAKT_IDLE_ACTIVITY_EVENTS,
  TAKT_IDLE_ACTIVITY_THROTTLE_MS,
} from '@/utils/common';
import { translateLocaleMessage } from '@/utils/takt-i18n-message';
import { EventBus } from '@/utils/event-bus';
import { createLogger } from '@/utils/logger';

const idleSessionLogger = createLogger('takt-idle-session');

let idleTimer: ReturnType<typeof setTimeout> | null = null;
let lastActivityAt = 0;
let tabHiddenAt: number | null = null;
let isHandlingIdle = false;
let listenersAttached = false;

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
 * 按当前配置重新调度空闲检测
 */
function scheduleIdleTimer(): void {
  const timeoutMs = getAuthIdleTimeoutMs();

  if (timeoutMs <= 0) {
    clearIdleTimer();
    return;
  }

  clearIdleTimer();
  idleTimer = setTimeout(() => {
    void handleIdleTimeout();
  }, timeoutMs);
}

/**
 * 空闲超时：先触发前端登出，再异步清除服务端会话
 */
async function handleIdleTimeout(): Promise<void> {
  if (isHandlingIdle) {
    return;
  }

  const userStore = useUserStore();

  if (!userStore.isLoggedIn) {
    clearIdleTimer();
    return;
  }

  isHandlingIdle = true;
  clearIdleTimer();

  idleSessionLogger.info('用户空闲超时，执行自动登出', { action: 'idle-timeout' });

  const message = translateLocaleMessage('common.tip.session.idle.logout');
  EventBus.emit('auth:idle-timeout', { message });

  try {
    await signOutSession();
  } catch (error) {
    idleSessionLogger.warn('空闲登出时清除服务端会话失败', { action: 'sign-out' }, error);
  }

  isHandlingIdle = false;
}

/**
 * 记录用户活动并重置空闲计时
 * @param event 可选 DOM 事件；非 isTrusted 事件（扩展脚本注入）不计入活动
 */
function onUserActivity(event?: Event): void {
  if (event && !event.isTrusted) {
    return;
  }

  const userStore = useUserStore();

  if (!userStore.isLoggedIn || isHandlingIdle) {
    return;
  }

  const now = Date.now();

  if (now - lastActivityAt < TAKT_IDLE_ACTIVITY_THROTTLE_MS) {
    return;
  }

  lastActivityAt = now;
  scheduleIdleTimer();
}

/**
 * 标签页隐藏/可见：后台超过阈值则立即登出，否则可见时重置计时
 */
function onVisibilityChange(): void {
  if (document.hidden) {
    tabHiddenAt = Date.now();
    return;
  }

  const timeoutMs = getAuthIdleTimeoutMs();
  const hiddenAt = tabHiddenAt;
  tabHiddenAt = null;

  if (hiddenAt !== null && timeoutMs > 0 && Date.now() - hiddenAt >= timeoutMs) {
    void handleIdleTimeout();
    return;
  }

  onUserActivity();
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

  document.addEventListener('visibilitychange', onVisibilityChange);
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
          lastActivityAt = 0;
          onUserActivity();
          return;
        }

        clearIdleTimer();
        isHandlingIdle = false;
        tabHiddenAt = null;
      },
      { immediate: true },
    );
  });

  EventBus.on('user:login', () => {
    lastActivityAt = 0;
    onUserActivity();
  });

  idleSessionLogger.info('空闲自动登出已启用', {
    action: 'init',
    timeoutMinutes: getAuthIdleTimeoutMs() / 60_000,
  });
}
