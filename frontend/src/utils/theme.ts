// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：theme.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：主题模式 DOM、主题色预设、Ant Design CSS 变量同步
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { GlobalToken } from 'ant-design-vue/es/theme/interface';
import { TAKT_THEME_COLOR_STORAGE_KEY, TAKT_THEME_STORAGE_KEY } from '@/utils/common';

export { TAKT_THEME_COLOR_STORAGE_KEY, TAKT_THEME_STORAGE_KEY };

/** 主题模式：浅色 / 深色 / 跟随系统 */
export type TaktThemeMode = 'light' | 'dark' | 'system';

/** 实际生效的主题 */
export type TaktResolvedTheme = 'light' | 'dark';

/** 主题色预设键名（与 color-base.css 著名色彩 --takt-* 一一对应） */
export type TaktThemeColorPreset =
  | 'mars-green'
  | 'tiffany-blue'
  | 'chinese-red'
  | 'titian-red'
  | 'burgundy'
  | 'bordeaux'
  | 'klein-blue'
  | 'van-dyke-brown'
  | 'prussian-blue'
  | 'senelier-yellow'
  | 'memorial-gray';

/**
 * 预设顺序（与 color-base.css「著名色彩」声明顺序一致，勿改顺序）
 * @see frontend/src/styles/color-base.css
 */
export const themeColorPresetKeys: readonly TaktThemeColorPreset[] = [
  'mars-green',
  'tiffany-blue',
  'chinese-red',
  'titian-red',
  'burgundy',
  'bordeaux',
  'klein-blue',
  'van-dyke-brown',
  'prussian-blue',
  'senelier-yellow',
  'memorial-gray',
] as const;

/** 预设主题色映射（色值与 color-base.css --takt-* 相同） */
export const themeColorMap: Record<TaktThemeColorPreset, string> = {
  'mars-green': '#2e8b57',
  'tiffany-blue': '#00a0b0',
  'chinese-red': '#ff0000',
  'titian-red': '#ff6347',
  burgundy: '#990033',
  bordeaux: '#8c1515',
  'klein-blue': '#002fa7',
  'van-dyke-brown': '#4c2b18',
  'prussian-blue': '#003153',
  'senelier-yellow': '#f9dc24',
  'memorial-gray': '#808080',
};

/** 默认预设（列表首项） */
export const defaultThemeColorPreset: TaktThemeColorPreset = themeColorPresetKeys[0];

/**
 * 将 camelCase 转为 kebab-case
 * @param key 原始键名
 * @returns kebab-case 键名
 */
function camelToKebab(key: string): string {
  return key.replace(/([A-Z])/g, '-$1').toLowerCase();
}

/**
 * 读取已持久化的主题模式
 * @returns 主题模式
 */
export function readStoredThemeMode(): TaktThemeMode {
  const stored = localStorage.getItem(TAKT_THEME_STORAGE_KEY);
  if (stored === 'light' || stored === 'dark' || stored === 'system') {
    return stored;
  }
  return 'system';
}

/**
 * 根据模式与系统偏好解析实际主题
 * @param mode 主题模式
 * @returns 实际生效的主题
 */
export function resolveThemeMode(mode: TaktThemeMode): TaktResolvedTheme {
  if (mode === 'system') {
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
  }
  return mode;
}

/**
 * 将实际主题写入 html 根节点
 * @param resolved 实际生效的主题
 */
export function applyThemeDom(resolved: TaktResolvedTheme): void {
  document.documentElement.dataset.theme = resolved;
  document.documentElement.style.colorScheme = resolved;
}

/**
 * 应用启动前初始化主题 DOM，避免首屏闪烁
 */
export function initTaktThemeDom(): void {
  applyThemeDom(resolveThemeMode(readStoredThemeMode()));
}

/**
 * 读取已持久化的主题色预设
 * @returns 主题色预设
 */
export function readStoredThemeColorPreset(): TaktThemeColorPreset {
  const stored = localStorage.getItem(TAKT_THEME_COLOR_STORAGE_KEY);
  if (stored && stored in themeColorMap) {
    return stored as TaktThemeColorPreset;
  }
  return defaultThemeColorPreset;
}

/**
 * 获取预设对应的色值
 * @param preset 预设键名
 * @returns 十六进制色值
 */
export function getThemeColorValue(preset: TaktThemeColorPreset): string {
  return themeColorMap[preset];
}

/**
 * 将 Ant Design Vue token 中的 color* 字段写入 documentElement CSS 变量
 * @param token Ant Design Vue useToken 返回的 token
 * @param prefix CSS 变量前缀，默认 ant
 */
export function syncAntDesignCssVariables(token: GlobalToken, prefix = 'ant'): void {
  const root = document.documentElement;
  (Object.keys(token) as Array<keyof GlobalToken>).forEach((key) => {
    const keyName = String(key);
    if (!keyName.startsWith('color')) {
      return;
    }
    const value = token[key];
    if (typeof value !== 'string') {
      return;
    }
    root.style.setProperty(`--${prefix}-${camelToKebab(keyName)}`, value);
  });
}
