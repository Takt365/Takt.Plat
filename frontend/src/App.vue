<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：frontend/src -->
<!-- 文件名称：App.vue -->
<!-- 功能描述：根组件，包含路由视图和全局布局 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-config-provider :locale="locale" :theme="antdTheme">
    <a-app class="h-full">
      <takt-theme-sync />
      <router-view />
    </a-app>
  </a-config-provider>
</template>

<script setup lang="ts">
/**
 * 根组件
 * 提供全局 ConfigProvider（国际化、主题算法）与 CSS 变量同步
 */
import { theme as antdThemeApi } from 'ant-design-vue';
import type { ThemeConfig } from 'ant-design-vue/es/config-provider/context';
import { useTaktComponentLocale } from '@/composables/use-takt-component-locale';
import { useThemeStore } from '@/stores/common/theme';
import { useThemeColorStore } from '@/stores/common/theme-color';

const { antDesignVueLocale } = useTaktComponentLocale();
const themeStore = useThemeStore();
const themeColorStore = useThemeColorStore();

/** Ant Design Vue ConfigProvider 语言包（与 vue-i18n 同步） */
const locale = antDesignVueLocale;

/**
 * Ant Design Vue 主题配置（algorithm 驱动原生亮/暗色切换）
 */
const antdTheme = computed<ThemeConfig>(() => ({
  algorithm:
    themeStore.resolvedTheme === 'dark'
      ? antdThemeApi.darkAlgorithm
      : antdThemeApi.defaultAlgorithm,
  token: {
    colorPrimary: themeColorStore.colorPrimary,
  },
}));
</script>
