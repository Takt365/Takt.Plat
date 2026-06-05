// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src
// 文件名称：main.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：应用入口文件，初始化Vue、路由、状态管理、国际化
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import Antd from 'ant-design-vue';
import App from './App.vue';
import router from './router';
import { setRuntimeRouter } from '@/utils/runtime-context';
import i18n from './locales';

setRuntimeRouter(router);
import { registerTaktEventHandlers } from '@/bootstrap/takt-event-handlers';
import { initTaktIdleSession } from '@/bootstrap/takt-idle-session';
import { registerPermissionDirective } from '@/directives/permission';
import { initEventBus } from '@/utils/event-bus';
import { initLogger } from '@/utils/logger';
import { initTaktThemeDom } from '@/utils/theme';
import { applySettings } from '@/utils/apply-settings';
import { useSettingStore } from '@/stores/common/setting';
import { useLocaleStore } from '@/stores/foundation/locale';
import 'ant-design-vue/dist/reset.css';
import 'flag-icons/css/flag-icons.min.css';
import './styles/global.css';

initTaktThemeDom();
/**
 * 解析布尔型环境变量
 * @param value 环境变量字符串
 * @param defaultValue 默认值
 */
function parseEnvBoolean(value: string | undefined, defaultValue: boolean): boolean {
  if (value === undefined || value === '') {
    return defaultValue;
  }
  return value === 'true' || value === '1';
}

/**
 * 注册 PWA Service Worker（仅当 VITE_PWA_ENABLED 且构建已启用 vite-plugin-pwa）
 */
async function registerPwaServiceWorker(): Promise<void> {
  if (!parseEnvBoolean(import.meta.env.VITE_PWA_ENABLED, true)) {
    return;
  }
  const { registerSW } = await import('virtual:pwa-register');
  registerSW({
    immediate: true,
  });
}

void registerPwaServiceWorker();

const app = createApp(App);

const pinia = createPinia();

app.use(pinia);
useSettingStore();
applySettings();
const isLoginEntryPath =
  typeof window !== 'undefined'
  && (window.location.pathname === '/login' || window.location.pathname.startsWith('/login/'));
if (!isLoginEntryPath) {
  useLocaleStore().initLocaleFromStorage();
}
app.use(router);
app.use(i18n);
app.use(Antd);

registerTaktEventHandlers();
initTaktIdleSession();
registerPermissionDirective(app);
initEventBus();
// 全局日志：main 中 initLogger 初始化；logger / createLogger 由 auto-import 注入
initLogger(app, undefined, router);

app.mount('#app');
