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
import { EventBus, type TaktNotificationType } from '@/utils/event-bus';

/**
 * 执行登出清理并跳转登录页
 * @param toastMessage 可选提示文案
 */
function performLogout(toastMessage?: string): void {
  const userStore = useUserStore();
  const tenantStore = useTenantStore();

  userStore.logout();
  tenantStore.clearTenant();
  useDictDataStore().resetDictData();
  useTranslationStore().resetTranslationMessages();
  useMenuStore().resetMenuList();
  usePermissionStore().resetPermissions();
  void useSignalRStore().disconnectSignalRAsync().catch(() => undefined);
  useSignalRStore().resetSignalRState();

  const currentPath = router.currentRoute.value.fullPath;
  if (currentPath !== '/login') {
    router.push('/login');
  }

  if (toastMessage) {
    message.error(toastMessage);
  }
}

/**
 * 将 notification:show 映射到 Ant Design Vue Message
 * @param type 通知类型
 * @param content 主文案
 * @param description 副文案
 */
function showAntdMessage(type: TaktNotificationType, content: string, description?: string): void {
  const duration = 3;
  switch (type) {
    case 'success':
      message.success(content, duration);
      break;
    case 'warning':
      message.warning(content, duration);
      break;
    case 'info':
      message.info(content, duration);
      break;
    case 'error':
    default:
      message.error(description ? `${content} — ${description}` : content, duration);
      break;
  }
}

/**
 * 注册全局事件订阅（在 createApp 且 app.use(pinia)、app.use(router) 之后调用一次）
 */
export function registerTaktEventHandlers(): void {
  EventBus.on('auth:session-expired', (payload) => {
    performLogout(payload?.message ?? '登录已过期，请重新登录');
  });

  EventBus.on('auth:idle-timeout', (payload) => {
    performLogout(payload?.message ?? '登录已过期，请重新登录');
  });

  EventBus.on('user:logout', () => {
    performLogout();
  });

  EventBus.on('notification:show', ({ type, message: content, description }) => {
    showAntdMessage(type, content, description);
  });

  EventBus.on('user:login', () => {
    EventBus.emit('menu:refresh', undefined);
    void useLocaleStore().loadCultureOptionsAsync().catch(() => undefined);
    void useDictDataStore().loadAllDictDataAsync();
    void useTranslationStore().loadTranslationMessagesAsync();
    void useSignalRStore().connectSignalRAsync().catch(() => undefined);
  });

  EventBus.on('menu:refresh', () => {
    void useMenuStore().loadMenuListAsync(false).catch(() => undefined);
  });

  EventBus.on('tenant:change', () => {
    useTranslationStore().resetTranslationMessages();
    EventBus.emit('menu:refresh', undefined);
    EventBus.emit('table:refresh', {});
    void useTranslationStore().loadTranslationMessagesAsync();
    void useMenuStore().loadMenuListAsync(true).catch(() => undefined);
  });

  EventBus.on('company:change', () => {
    useTranslationStore().resetTranslationMessages();
    EventBus.emit('menu:refresh', undefined);
    EventBus.emit('table:refresh', {});
    void useUserStore().loadUserProfile(true).catch(() => undefined);
    void useTranslationStore().loadTranslationMessagesAsync();
    void useMenuStore().loadMenuListAsync(true).catch(() => undefined);
    void useDictDataStore().loadAllDictDataAsync();
    void useSignalRStore().reconnectSignalRAsync().catch(() => undefined);
  });

  EventBus.on('theme:change', ({ theme }) => {
    const themeStore = useThemeStore();

    if (themeStore.mode !== theme) {
      themeStore.setThemeMode(theme);
    }
  });

  EventBus.on('locale:change', ({ locale }) => {
    const localeStore = useLocaleStore();
    localeStore.setLocale(locale);
  });
}
