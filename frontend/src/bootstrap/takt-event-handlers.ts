// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/bootstrap
// 文件名称：takt-event-handlers.ts
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：全局事件总线订阅注册（连接 request 副作用与 Store / Router / UI）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { message } from 'ant-design-vue';
import router from '@/router';
import { useUserStore } from '@/stores/identity/user';
import { useTenantStore } from '@/stores/identity/tenant';
import { useLocaleStore } from '@/stores/foundation/locale';
import { useDictDataStore } from '@/stores/foundation/dict-data';
import { useTranslationStore } from '@/stores/foundation/translation';
import { useThemeStore } from '@/stores/common/theme';
import { useMenuStore } from '@/stores/identity/menu';
import { usePermissionStore } from '@/stores/identity/permission';
import { useSignalRStore } from '@/stores/foundation/signalr';
import { EventBus, type NotificationType } from '@/utils/event-bus';

/**
 * 执行登出清理并跳转登录页
 * @description 重置用户/租户/字典/翻译/菜单/权限/SignalR 状态；已在 /login 时不重复 push
 * @param {string} [toastMessage] 可选错误提示（Ant Design Message）
 * @returns {void}
 */
function performLogout(toastMessage?: string): void {
  /** 用户身份与令牌 Store */
  const userStore = useUserStore();
  /** 当前租户/公司上下文 Store */
  const tenantStore = useTenantStore();

  // 清除本地 token 与用户资料
  userStore.logout();
  // 清除租户/公司选择与请求头上下文
  tenantStore.clearTenant();

  // 重置依赖登录/租户的全局缓存（字典、翻译、菜单、权限）
  // 清空字典缓存，避免下一用户看到旧租户数据
  useDictDataStore().resetDictData();
  // 清空动态翻译缓存
  useTranslationStore().resetTranslationMessages();
  // 清空侧栏菜单缓存
  useMenuStore().resetMenuList();
  // 清空按钮/路由权限缓存
  usePermissionStore().resetPermissions();

  // 断开实时连接并清空 Hub 状态，防止下一用户复用旧连接
  // 异步断开 SignalR，失败不阻断登出
  void useSignalRStore().disconnectSignalRAsync().catch(() => undefined);
  // 重置 Hub 连接状态与订阅
  useSignalRStore().resetSignalRState();

  /** 当前路由完整路径（含 query/hash） */
  const currentPath = router.currentRoute.value.fullPath;
  // 已在登录页时不重复 push，避免路由循环
  if (currentPath !== '/login') {
    // 跳转登录页
    router.push('/login');
  }

  // 调用方传入提示文案时展示错误 Message
  if (toastMessage) {
    // 展示登出原因（会话过期、空闲超时等）
    message.error(toastMessage);
  }
}

/**
 * 将 notification:show 映射到 Ant Design Vue Message
 * @param {NotificationType} type 通知类型
 * @param {string} content 主文案
 * @param {string} [description] 副文案；error 类型时与主文案拼接展示
 * @returns {void}
 */
function showAntdMessage(type: NotificationType, content: string, description?: string): void {
  /** Message 自动关闭时长（秒） */
  const duration = 3;
  switch (type) {
    case 'success':
      // 成功提示
      message.success(content, duration);
      break;
    case 'warning':
      // 警告提示
      message.warning(content, duration);
      break;
    case 'info':
      // 信息提示
      message.info(content, duration);
      break;
    case 'error':
    default:
      // error 与未知类型统一走 error 样式；有 description 时拼接副文案
      message.error(description ? `${content} — ${description}` : content, duration);
      break;
  }
}

/**
 * 注册全局事件订阅（在 createApp 且 app.use(pinia)、app.use(router) 之后调用一次）
 * @description 订阅 auth:session-expired、auth:idle-timeout、user:logout、notification:show、
 *   user:login、menu:refresh、tenant:change、company:change、theme:change、locale:change
 * @returns {void}
 */
export function registerTaktEventHandlers(): void {
  // 401 / 业务未授权：request 拦截器仅发事件，此处统一清状态并跳转
  EventBus.on('auth:session-expired', (payload) => {
    /** 会话失效提示；缺省为固定中文兜底 */
    const logoutMessage = payload?.message ?? '登录已过期，请重新登录';
    const userStore = useUserStore();
    if (!userStore.isLoggedIn && router.currentRoute.value.path === '/login') {
      if (logoutMessage) {
        message.error(logoutMessage);
      }
      return;
    }
    // 执行统一登出清理
    performLogout(logoutMessage);
  });

  // 空闲超时：takt-idle-session 发事件，与 session-expired 共用登出流程
  EventBus.on('auth:idle-timeout', (payload) => {
    /** 空闲登出提示；缺省与 session-expired 一致 */
    const logoutMessage = payload?.message ?? '登录已过期，请重新登录';
    // 执行统一登出清理
    performLogout(logoutMessage);
  });

  // 主动登出：用户点击退出，无额外提示
  EventBus.on('user:logout', () => {
    // 执行统一登出清理（无 toast）
    performLogout();
  });

  // 全局 Toast：request / Store 等非 UI 层通过 EventBus 触发
  EventBus.on('notification:show', ({ type, message: content, description }) => {
    // 映射为 Ant Design Message
    showAntdMessage(type, content, description);
  });

  // 登录成功：预热菜单、语言选项、字典、翻译与 SignalR
  EventBus.on('user:login', () => {
    // 触发菜单重新加载
    EventBus.emit('menu:refresh', undefined);
    // 加载可选语言列表
    void useLocaleStore().loadCultureOptionsAsync().catch(() => undefined);
    // 预加载全量字典
    void useDictDataStore().loadAllDictDataAsync();
    // 加载动态翻译
    void useTranslationStore().loadTranslationMessagesAsync();
    // 建立 SignalR 连接
    void useSignalRStore().connectSignalRAsync().catch(() => undefined);
  });

  // 菜单刷新：force=false 保留折叠态等 UI 状态
  EventBus.on('menu:refresh', () => {
    // 非强制刷新侧栏菜单
    void useMenuStore().loadMenuListAsync(false).catch(() => undefined);
  });

  // 租户切换：翻译与菜单按新租户重建，并通知各列表页刷新
  EventBus.on('tenant:change', () => {
    // 清空旧租户翻译
    useTranslationStore().resetTranslationMessages();
    // 按新租户重建菜单
    EventBus.emit('menu:refresh', undefined);
    // 通知各列表页刷新数据
    EventBus.emit('table:refresh', {});
    // 加载新租户翻译
    void useTranslationStore().loadTranslationMessagesAsync();
    // 强制刷新菜单（租户级权限变化）
    void useMenuStore().loadMenuListAsync(true).catch(() => undefined);
  });

  // 公司切换：除租户级资源外，还需重载用户资料、字典与 SignalR 连接
  EventBus.on('company:change', () => {
    // 清空旧公司上下文下的翻译
    useTranslationStore().resetTranslationMessages();
    // 按新公司重建菜单
    EventBus.emit('menu:refresh', undefined);
    // 通知各列表页刷新数据
    EventBus.emit('table:refresh', {});
    // 强制刷新用户资料（公司级数据权限）
    void useUserStore().loadUserProfile(true).catch(() => undefined);
    // 加载新公司翻译
    void useTranslationStore().loadTranslationMessagesAsync();
    // 强制刷新菜单
    void useMenuStore().loadMenuListAsync(true).catch(() => undefined);
    // 重载字典（公司级隔离）
    void useDictDataStore().loadAllDictDataAsync();
    // 按新公司上下文重连 SignalR
    void useSignalRStore().reconnectSignalRAsync().catch(() => undefined);
  });

  // 主题同步：避免与 Store 当前值相同时重复写入 DOM
  EventBus.on('theme:change', ({ theme }) => {
    /** 主题模式 Store（light / dark） */
    const themeStore = useThemeStore();
    // 与当前 Store 值相同则跳过，减少 DOM 写入
    if (themeStore.mode !== theme) {
      // 写入主题并同步 DOM / localStorage
      themeStore.setThemeMode(theme);
    }
  });

  // 语言切换：同步 i18n 与持久化偏好
  EventBus.on('locale:change', ({ locale }) => {
    /** 语言与动态翻译 Store */
    const localeStore = useLocaleStore();
    // 切换 vue-i18n locale 并持久化
    localeStore.setLocale(locale);
  });
}
