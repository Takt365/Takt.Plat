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

/** 空闲会话模块日志实例 */
const idleSessionLogger = createLogger('takt-idle-session');

/** 空闲超时 setTimeout 句柄；null 表示未调度 */
let idleTimer: ReturnType<typeof setTimeout> | null = null;

/** 上次计入用户活动的 Unix 时间戳（毫秒），用于活动节流 */
let lastActivityAt = 0;

/** 标签页进入 hidden 时的时间戳；null 表示当前可见 */
let tabHiddenAt: number | null = null;

/** 是否正在执行空闲超时登出，防止重入 */
let isHandlingIdle = false;

/** 是否已向 window/document 注册活动与可见性监听 */
let listenersAttached = false;

/**
 * 清除空闲计时器
 * @returns {void}
 */
function clearIdleTimer(): void {
  // 存在尚未触发的计时器时将其取消并置空句柄
  if (idleTimer !== null) {
    // 取消浏览器侧待触发的超时回调
    clearTimeout(idleTimer);
    // 句柄置空，表示当前无在途计时
    idleTimer = null;
  }
}

/**
 * 按当前配置重新调度空闲检测
 * @description 读取 getAuthIdleTimeoutMs；为 0 时仅清除计时器不调度
 * @returns {void}
 */
function scheduleIdleTimer(): void {
  /** 当前空闲超时时长（毫秒）；0 表示功能禁用 */
  const timeoutMs = getAuthIdleTimeoutMs();

  // 功能禁用时确保无残留计时器
  if (timeoutMs <= 0) {
    // 清除可能由历史配置留下的计时器
    clearIdleTimer();
    return;
  }

  // 每次活动后从头计时，避免多个超时回调叠加
  // 先取消旧计时，再创建新计时
  clearIdleTimer();
  // 在 timeoutMs 后触发空闲登出流程
  idleTimer = setTimeout(() => {
    // 超时到达，异步执行登出（不阻塞 timer 回调）
    void handleIdleTimeout();
  }, timeoutMs);
}

/**
 * 空闲超时：先触发前端登出，再异步清除服务端会话
 * @description 通过 EventBus 发布 auth:idle-timeout，由 takt-event-handlers 执行 performLogout
 * @returns {Promise<void>}
 */
async function handleIdleTimeout(): Promise<void> {
  // 并发超时回调或 visibility 与 timer 同时触发时只处理一次
  if (isHandlingIdle) {
    return;
  }

  /** 用户身份与登录态 Store */
  const userStore = useUserStore();

  // 已登出则无需再处理（如手动退出后 timer 尚未清理）
  if (!userStore.isLoggedIn) {
    // 登出后清理残留计时器
    clearIdleTimer();
    return;
  }

  // 标记进入登出处理，阻止重入
  isHandlingIdle = true;
  // 登出流程已启动，取消待触发的空闲计时
  clearIdleTimer();

  // 记录空闲超时登出审计日志
  idleSessionLogger.info('用户空闲超时，执行自动登出', { action: 'idle-timeout' });

  /** 空闲登出提示文案（i18n 键 common.tip.session.idle.logout） */
  const message = translateLocaleMessage('common.tip.session.idle.logout');
  // 先发事件清前端状态，由 takt-event-handlers 执行 performLogout
  EventBus.emit('auth:idle-timeout', { message });

  // 尽力清除服务端会话；失败不阻断前端登出（用户已不可继续操作）
  try {
    // 调用 OpenIddict 登出端点撤销服务端会话
    await signOutSession();
  } catch (error) {
    // 服务端登出失败仅记日志，前端状态已由 EventBus 清理
    idleSessionLogger.warn('空闲登出时清除服务端会话失败', { action: 'sign-out' }, error);
  }

  // 登出流程结束，允许后续空闲检测
  isHandlingIdle = false;
}

/**
 * 记录用户活动并重置空闲计时
 * @param {Event} [event] 可选 DOM 事件；非 isTrusted 事件（扩展脚本注入）不计入活动
 * @returns {void}
 */
function onUserActivity(event?: Event): void {
  // 忽略扩展脚本合成的假活动，防止恶意续期
  if (event && !event.isTrusted) {
    return;
  }

  /** 用户身份与登录态 Store */
  const userStore = useUserStore();

  // 未登录或正在处理空闲登出时，忽略活动事件
  if (!userStore.isLoggedIn || isHandlingIdle) {
    return;
  }

  /** 当前时间戳（毫秒），用于活动节流 */
  const now = Date.now();

  // 距上次计入活动不足节流间隔时跳过，降低 setTimeout 重建频率
  if (now - lastActivityAt < TAKT_IDLE_ACTIVITY_THROTTLE_MS) {
    return;
  }

  // 更新上次活动时间，作为下次节流基准
  lastActivityAt = now;
  // 以最新活动时刻为起点，重新调度空闲超时
  scheduleIdleTimer();
}

/**
 * 标签页隐藏/可见：后台超过阈值则立即登出，否则可见时重置计时
 * @description document.hidden 时记录 tabHiddenAt；恢复可见时若后台时长 ≥ 超时阈值则触发 handleIdleTimeout
 * @returns {void}
 */
function onVisibilityChange(): void {
  // 标签页进入后台：记录隐藏时刻，改由 hidden 时长判定是否超时
  if (document.hidden) {
    // 记录进入后台的时间点
    tabHiddenAt = Date.now();
    return;
  }

  /** 当前空闲超时时长（毫秒） */
  const timeoutMs = getAuthIdleTimeoutMs();
  /** 本次 hidden 开始时刻的快照（随后清空 tabHiddenAt） */
  const hiddenAt = tabHiddenAt;
  // 恢复可见，清空后台起始时刻
  tabHiddenAt = null;

  // 切回前台时若后台已超过空闲阈值，立即登出（timer 在 hidden 期间可能未触发）
  if (hiddenAt !== null && timeoutMs > 0 && Date.now() - hiddenAt >= timeoutMs) {
    // 后台停留已超时，立即走登出流程
    void handleIdleTimeout();
    return;
  }

  // 未超阈值则视为一次有效活动，重新计时
  onUserActivity();
}

/**
 * 注册全局活动监听
 * @description 监听 TAKT_IDLE_ACTIVITY_EVENTS 与 visibilitychange；重复调用无副作用
 * @returns {void}
 */
function attachActivityListeners(): void {
  // 已注册则直接返回，保证幂等
  if (listenersAttached) {
    return;
  }

  // 标记监听已挂载，防止重复注册
  listenersAttached = true;

  // capture + passive：尽早捕获活动且不打断滚动性能
  TAKT_IDLE_ACTIVITY_EVENTS.forEach((eventName) => {
    // 为每种用户交互事件注册活动回调
    window.addEventListener(eventName, onUserActivity, { passive: true, capture: true });
  });

  // 监听标签页可见性变化，处理后台超时
  document.addEventListener('visibilitychange', onVisibilityChange);
}

/**
 * 初始化空闲会话监控（须在 app.use(pinia) 之后调用）
 * @description VITE_AUTH_IDLE_TIMEOUT_MINUTES 为 0 或未配置有效正数时跳过；
 *   登录态 token 变化与 user:login 事件会重置计时
 * @returns {void}
 */
export function initTaktIdleSession(): void {
  // 环境变量禁用空闲登出时跳过初始化
  if (!isAuthIdleLogoutEnabled()) {
    // 记录跳过原因，便于排查配置
    idleSessionLogger.info('空闲自动登出已禁用', { action: 'init-skip' });
    return;
  }

  // 注册 DOM 活动与可见性监听
  attachActivityListeners();

  // effectScope 托管 watch，避免模块级 watch 泄漏
  /** Vue 副作用作用域，组件卸载时可一并回收 */
  const scope = effectScope(true);
  // 在作用域内注册 token 监听
  scope.run(() => {
    /** 用户身份与令牌 Store */
    const userStore = useUserStore();
    /** 访问令牌响应式引用（登录/登出/刷新时变化） */
    const { token } = storeToRefs(userStore);

    // token 变化时同步空闲计时生命周期
    watch(
      token,
      (accessToken) => {
        // 存在有效 token：从零开始计空闲并启动计时器
        if (accessToken) {
          // 重置节流基准，避免沿用上次的 lastActivityAt
          lastActivityAt = 0;
          // 立即计入一次活动并调度计时
          onUserActivity();
          return;
        }

        // token 清空（登出）：清理模块内计时与重入状态
        // 取消待触发的空闲计时
        clearIdleTimer();
        // 允许后续新的空闲登出流程
        isHandlingIdle = false;
        // 清除后台隐藏时刻缓存
        tabHiddenAt = null;
      },
      { immediate: true },
    );
  });

  // 登录事件与 token watch 双路径：OAuth 回调等场景可能先 emit 再写 token
  EventBus.on('user:login', () => {
    // 登录成功，重置活动节流基准
    lastActivityAt = 0;
    // 启动空闲计时
    onUserActivity();
  });

  // 输出当前超时配置，便于开发环境确认
  idleSessionLogger.info('空闲自动登出已启用', {
    action: 'init',
    timeoutMinutes: getAuthIdleTimeoutMs() / 60_000,
  });
}
