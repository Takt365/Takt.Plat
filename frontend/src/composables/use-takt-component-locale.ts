// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/composables
// 文件名称：use-takt-component-locale.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：组件库语言 Composable（Ant Design Vue / ECharts 随 vue-i18n locale 同步）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { getAntDesignVueLocale, getEchartsLocaleCode } from '@/utils/takt-locale-sync';

/**
 * 组件库语言（随 vue-i18n locale 变化，供 ConfigProvider / ECharts 使用）
 */
export function useTaktComponentLocale() {
  const { locale } = useI18n();

  /** Ant Design Vue ConfigProvider 语言包 */
  const antDesignVueLocale = computed(() => getAntDesignVueLocale(String(locale.value)));

  /** ECharts registerLocale 键（响应式，与 vue-i18n locale 同步） */
  const echartsLocale = computed(() => getEchartsLocaleCode(String(locale.value)));

  return {
    antDesignVueLocale,
    echartsLocale,
  };
}
