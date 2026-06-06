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
 * @returns {void}
 */
export function applySettings(): void {
  /** 合并 localStorage 与默认项后的当前偏好 */
  const setting = getSetting();
  /** 文档根元素，承载 CSS 变量与 data 属性 */
  const root = document.documentElement;
  /** body 元素，承载色弱/灰度滤镜 */
  const body = document.body;

  // 明暗主题写入 data-theme 与 Ant token
  applyThemeDom(setting.theme);

  // 根字号与圆角 CSS 变量
  root.style.fontSize = `${setting.fontSize}px`;
  root.style.setProperty('--takt-font-size', `${setting.fontSize}px`);
  root.style.setProperty('--takt-border-radius', `${String(setting.borderRadius)}px`);

  // 布局相关 data 属性供全局样式选择器使用
  root.dataset.layout = setting.layout;
  root.dataset.contentWidth = setting.contentWidth;
  root.dataset.tabStyle = setting.tabStyle;
  root.dataset.menuStyle = setting.menuStyle;

  /** 色弱/灰度滤镜片段列表 */
  const filters: string[] = [];
  // 色弱模式：反色滤镜
  if (setting.colorWeak) {
    filters.push('invert(80%)');
  }
  // 灰度模式
  if (setting.grayscale) {
    filters.push('grayscale(100%)');
  }
  // 无滤镜时清空 body.filter
  body.style.filter = filters.length > 0 ? filters.join(' ') : '';

  /** 当前主题色预设对应的 hex 值 */
  const primary = getThemeColorValue(setting.themeColor);
  // 同步 Ant Design 与 Takt 主色变量
  root.style.setProperty('--ant-color-primary', primary);
  root.style.setProperty('--takt-color-primary', primary);

  // 广播主题变更，供非直接读 setting 的模块同步
  EventBus.emit('theme:change', { theme: setting.theme });
  EventBus.emit('theme-color:change', {
    preset: setting.themeColor.type,
    color: primary,
  });
}

/**
 * 广播偏好设置已变更（布局组件可监听后刷新本地状态）
 * @returns {void}
 */
export function notifySettingsChanged(): void {
  // SSR/测试环境无 window 时跳过
  if (typeof window !== 'undefined') {
    // 携带最新 setting 快照供监听方读取
    window.dispatchEvent(new CustomEvent(APP_SETTING_CHANGED_EVENT, { detail: getSetting() }));
  }
}
