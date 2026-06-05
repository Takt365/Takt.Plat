// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src
// 文件名称：setting.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：全局配置（默认值、常量、持久化读写与工具）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AppSetting, ThemeColor, ThemeColorConfig } from '@/types/setting';
import { createLogger } from '@/utils/logger';

export type { ThemeColor, ThemeColorConfig, AppSetting } from '@/types/setting';

const settingLogger = createLogger('setting');

/** 默认设置 */
export const defaultSetting: AppSetting = {
  layout: 'side',
  theme: 'dark',
  themeColor: { type: 'blue' },
  borderRadius: 5,
  fontSize: 15,
  colorWeak: false,
  grayscale: false,
  fixedHeader: true,
  fixedSider: true,
  showLogo: true,
  siderWidth: 200,
  siderCollapsedWidth: 64,
  showBreadcrumb: true,
  breadcrumbIcon: true,
  showTabs: true,
  tabStyle: 'google',
  persistTabs: false,
  maxTabs: 10,
  showFooter: true,
  copyright: '© 2026 Takt Plat. All rights reserved.',
  contentWidth: 'fluid',
  multiTab: true,
  watermark: false,
  watermarkContent: 'Takt Plat',
  demo: false,
  menuAccordion: true,
  menuStyle: 'plain',
  logo: '@/assets/images/takt.svg',
  logoText: 'Takt Plat',
  logoCollapsedText: 'T',
  showForgotPassword: false,
  showRegister: false,
};

/** localStorage 键名 */
export const STORAGE_KEY = 'app-setting';

/**
 * 主题色预设映射（与 styles/color-base.css 十大著名色彩一致）
 */
export const themeColorMap: Record<Exclude<ThemeColor, 'custom'>, string> = {
  green: '#2e8b57',
  cyan: '#00a0b0',
  red: '#ff0000',
  orange: '#ff6347',
  purple: '#990033',
  pink: '#8c1515',
  blue: '#002fa7',
  brown: '#4c2b18',
  indigo: '#003153',
  yellow: '#f9dc24',
  gray: '#808080',
};

/**
 * 解析主题色色值
 * @param {ThemeColorConfig} config 主题色配置
 * @returns {string} 十六进制色值
 */
export function getThemeColorValue(config: ThemeColorConfig): string {
  if (config.type === 'custom' && config.customColor) {
    return config.customColor;
  }

  return themeColorMap[config.type as Exclude<ThemeColor, 'custom'>] || themeColorMap.blue;
}

/**
 * 校验字号范围（15–22）
 * @param {number} size 字号
 * @returns {number} 合法字号
 */
export function validateFontSize(size: number): number {
  if (size < 15) {
    return 15;
  }

  if (size > 22) {
    return 22;
  }

  return size;
}

/**
 * 合并并规范化应用设置
 * @param {Partial<AppSetting>} raw 原始片段
 * @returns {AppSetting} 有效设置
 */
export function normalizeSetting(raw: Partial<AppSetting>): AppSetting {
  const base: AppSetting = { ...defaultSetting, ...raw };

  if (raw.themeColor && typeof raw.themeColor === 'object') {
    base.themeColor = { ...defaultSetting.themeColor, ...raw.themeColor };
  }

  if (typeof base.fontSize === 'number') {
    base.fontSize = validateFontSize(base.fontSize);
  } else {
    const sizeMap: Record<string, number> = { small: 15, medium: 16, large: 18 };
    base.fontSize = sizeMap[String(base.fontSize)] ?? 15;
  }

  if (!base.themeColor || typeof base.themeColor !== 'object') {
    base.themeColor = { ...defaultSetting.themeColor };
  } else if (base.themeColor.type !== 'custom' && !(base.themeColor.type in themeColorMap)) {
    base.themeColor = { ...defaultSetting.themeColor };
  }

  return base;
}

/**
 * 从 localStorage 读取并合并为有效 AppSetting
 * @returns {AppSetting} 当前应用设置
 */
export function readSettingFromStorage(): AppSetting {
  if (typeof window === 'undefined' || !localStorage) {
    return defaultSetting;
  }

  try {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (!stored) {
      return defaultSetting;
    }

    const parsed = JSON.parse(stored) as Partial<AppSetting>;
    return normalizeSetting(parsed);
  } catch (error) {
    settingLogger.warn('读取/解析偏好设置失败，使用默认值', { action: 'read' }, error);
    return defaultSetting;
  }
}

/** 非响应式读取当前设置 */
export const getSetting = readSettingFromStorage;

/**
 * 持久化应用设置
 * @param {AppSetting} setting 完整设置
 */
export function saveSettingToStorage(setting: AppSetting): void {
  if (typeof window === 'undefined' || !localStorage) {
    return;
  }

  const normalized = normalizeSetting(setting);
  localStorage.setItem(STORAGE_KEY, JSON.stringify(normalized));
}
