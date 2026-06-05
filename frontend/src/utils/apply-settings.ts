// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：apply-settings.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：将 AppSetting 应用到 DOM（主题、字号、滤镜、布局 data 属性、主色 CSS 变量）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getSetting, getThemeColorValue } from '@/setting';
import { applyThemeDom } from '@/utils/theme';
import { EventBus } from '@/utils/event-bus';

/** 设置变更自定义事件名（供非 Pinia 模块监听） */
export const APP_SETTING_CHANGED_EVENT = 'takt:app-setting-changed';

/**
 * 将当前 AppSetting 应用到 document（读取 localStorage 合并后的结果）
 */
export function applySettings(): void {
  const setting = getSetting();
  const root = document.documentElement;
  const body = document.body;

  applyThemeDom(setting.theme);

  root.style.fontSize = `${setting.fontSize}px`;
  root.style.setProperty('--takt-font-size', `${setting.fontSize}px`);
  root.style.setProperty('--takt-border-radius', `${String(setting.borderRadius)}px`);

  root.dataset.layout = setting.layout;
  root.dataset.contentWidth = setting.contentWidth;
  root.dataset.tabStyle = setting.tabStyle;
  root.dataset.menuStyle = setting.menuStyle;

  const filters: string[] = [];
  if (setting.colorWeak) {
    filters.push('invert(80%)');
  }
  if (setting.grayscale) {
    filters.push('grayscale(100%)');
  }
  body.style.filter = filters.length > 0 ? filters.join(' ') : '';

  const primary = getThemeColorValue(setting.themeColor);
  root.style.setProperty('--ant-color-primary', primary);
  root.style.setProperty('--takt-color-primary', primary);

  EventBus.emit('theme:change', { theme: setting.theme });
  EventBus.emit('theme-color:change', {
    preset: setting.themeColor.type,
    color: primary,
  });
}

/**
 * 广播偏好设置已变更（布局组件可监听后刷新本地状态）
 */
export function notifySettingsChanged(): void {
  if (typeof window !== 'undefined') {
    window.dispatchEvent(new CustomEvent(APP_SETTING_CHANGED_EVENT, { detail: getSetting() }));
  }
}
