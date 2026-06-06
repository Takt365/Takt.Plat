// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-locale-sync.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：第三方组件库语言同步（Ant Design Vue / dayjs / ECharts，与 vue-i18n 对齐；运行时网关：模块级 locale 状态）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import dayjs from 'dayjs';
import 'dayjs/locale/en';
import 'dayjs/locale/zh-cn';
import 'dayjs/locale/zh-hk';
import 'dayjs/locale/ja';
import { registerLocale } from 'echarts/core';
import echartsEnUS from 'echarts/i18n/langEN-obj';
import echartsZhCN from 'echarts/i18n/langZH-obj';
import echartsZhHK from 'echarts/i18n/langZH-obj';
import echartsJaJP from 'echarts/i18n/langJA-obj';
import type { Locale as AntdLocale } from 'ant-design-vue/es/locale';
import antdZhCN from 'ant-design-vue/es/locale/zh_CN';
import antdZhHK from 'ant-design-vue/es/locale/zh_HK';
import antdEnUS from 'ant-design-vue/es/locale/en_US';
import antdJaJP from 'ant-design-vue/es/locale/ja_JP';
import {
  TAKT_DEFAULT_LOCALE,
  TAKT_LOCALE_STORAGE_KEY,
  TAKT_SUPPORTED_LOCALES,
} from '@/utils/common';
import type { TaktCultureCode } from '@/types/common';

/** ECharts 语言包类型（与官方 lang*-obj 默认导出一致） */
type EchartsLocale = typeof echartsEnUS;

/** Ant Design Vue locale 映射键（antd-{CultureCode}） */
type TaktAntdLocaleKey = `antd-${TaktCultureCode}`;

/** dayjs locale 映射键（dayjs-{CultureCode}） */
type TaktDayjsLocaleKey = `dayjs-${TaktCultureCode}`;

/** ECharts locale 映射键（echarts-{CultureCode}） */
type TaktEchartsLocaleKey = `echarts-${TaktCultureCode}`;

/** 与 vue-i18n 同步的 ECharts registerLocale 键（模块级，由 syncTaktComponentLocales 更新） */
let currentEchartsLocale: TaktCultureCode = TAKT_DEFAULT_LOCALE;

/**
 * 读取当前已同步的 ECharts locale 键
 * @returns 区域文化编码
 */
export function getCurrentTaktEchartsLocale(): TaktCultureCode {
  return currentEchartsLocale;
}

/** Ant Design Vue ConfigProvider locale（键：antd-{CultureCode}） */
const antdLocaleMap: Record<TaktAntdLocaleKey, AntdLocale> = {
  'antd-zh-CN': antdZhCN,
  'antd-zh-HK': antdZhHK,
  'antd-en-US': antdEnUS,
  'antd-ja-JP': antdJaJP,
};

/** dayjs 全局 locale 名（键：dayjs-{CultureCode}） */
const dayjsLocaleMap: Record<TaktDayjsLocaleKey, string> = {
  'dayjs-zh-CN': 'zh-cn',
  'dayjs-zh-HK': 'zh-hk',
  'dayjs-en-US': 'en',
  'dayjs-ja-JP': 'ja',
};

/** ECharts 语言包（键：echarts-{CultureCode}，与 antdLocaleMap 同风格） */
const echartsLocaleMap: Record<TaktEchartsLocaleKey, EchartsLocale> = {
  'echarts-zh-CN': echartsZhCN,
  'echarts-zh-HK': echartsZhHK,
  'echarts-en-US': echartsEnUS,
  'echarts-ja-JP': echartsJaJP,
};

let echartsLocalesRegistered = false;

/**
 * 生成 Ant Design Vue locale 映射键
 * @param cultureCode 区域文化编码
 */
function toAntdLocaleKey(cultureCode: TaktCultureCode): TaktAntdLocaleKey {
  return `antd-${cultureCode}`;
}

/**
 * 生成 dayjs locale 映射键
 * @param cultureCode 区域文化编码
 */
function toDayjsLocaleKey(cultureCode: TaktCultureCode): TaktDayjsLocaleKey {
  return `dayjs-${cultureCode}`;
}

/**
 * 生成 ECharts locale 映射键
 * @param cultureCode 区域文化编码
 */
function toEchartsLocaleKey(cultureCode: TaktCultureCode): TaktEchartsLocaleKey {
  return `echarts-${cultureCode}`;
}

/**
 * 读取用户已选语言（localStorage）；无有效记录时返回默认 en-US
 * @returns 区域文化编码
 */
export function readStoredLocale(): string {
  const stored = localStorage.getItem(TAKT_LOCALE_STORAGE_KEY);

  if (stored && (TAKT_SUPPORTED_LOCALES as readonly string[]).includes(stored)) {
    return stored;
  }

  return TAKT_DEFAULT_LOCALE;
}

/**
 * 规范化为支持的语言编码
 * @param cultureCode 区域文化编码
 * @returns 规范后的编码
 */
export function resolveTaktCultureCode(cultureCode: string): TaktCultureCode {
  if ((TAKT_SUPPORTED_LOCALES as readonly string[]).includes(cultureCode)) {
    return cultureCode as TaktCultureCode;
  }

  return TAKT_DEFAULT_LOCALE;
}

/**
 * 注册 ECharts 语言包（仅执行一次；registerLocale 键与 TaktCultureCode 一致）
 */
function registerEchartsLocalesOnce(): void {
  if (echartsLocalesRegistered) {
    return;
  }

  for (const cultureCode of TAKT_SUPPORTED_LOCALES) {
    registerLocale(cultureCode, echartsLocaleMap[toEchartsLocaleKey(cultureCode)]);
  }

  echartsLocalesRegistered = true;
}

/**
 * 获取 Ant Design Vue ConfigProvider 语言包
 * @param cultureCode 区域文化编码
 * @returns Ant Design Vue locale
 */
export function getAntDesignVueLocale(cultureCode: string): AntdLocale {
  return antdLocaleMap[toAntdLocaleKey(resolveTaktCultureCode(cultureCode))];
}

/**
 * 获取 ECharts 语言包
 * @param cultureCode 区域文化编码
 * @returns ECharts LocaleOption
 */
export function getEchartsLocale(cultureCode: string): EchartsLocale {
  return echartsLocaleMap[toEchartsLocaleKey(resolveTaktCultureCode(cultureCode))];
}

/**
 * 获取 ECharts init 使用的 registerLocale 键（与 TaktCultureCode 一致）
 * @param cultureCode 区域文化编码
 * @returns 区域文化编码
 */
export function getEchartsLocaleCode(cultureCode: string): TaktCultureCode {
  return resolveTaktCultureCode(cultureCode);
}

/**
 * 获取 ECharts init 第三参数（与当前语言同步）
 * @param cultureCode 区域文化编码，默认取 getCurrentTaktEchartsLocale()
 * @returns ECharts init 选项片段
 */
export function getEchartsInitLocaleOption(cultureCode?: string): { locale: TaktCultureCode } {
  const code = cultureCode ? getEchartsLocaleCode(cultureCode) : currentEchartsLocale;

  return { locale: code };
}

/**
 * 同步第三方组件库语言（dayjs 全局 + ECharts 注册表）
 * @param cultureCode 区域文化编码（与 vue-i18n locale 一致）
 */
export function syncTaktComponentLocales(cultureCode: string): void {
  const normalized = resolveTaktCultureCode(cultureCode);

  dayjs.locale(dayjsLocaleMap[toDayjsLocaleKey(normalized)]);
  registerEchartsLocalesOnce();
  currentEchartsLocale = normalized;
}
