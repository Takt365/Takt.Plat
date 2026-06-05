// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/stores/common
// 文件名称：theme.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：主题状态管理（Ant Design Vue ConfigProvider 算法 + Tailwind CSS 变量联动）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed, watch } from 'vue';
import { usePreferredDark } from '@vueuse/core';
import { EventBus } from '@/utils/event-bus';
import { TAKT_THEME_STORAGE_KEY } from '@/utils/common';
import {
  applyThemeDom,
  readStoredThemeMode,
  type TaktResolvedTheme,
  type TaktThemeMode,
} from '@/utils/theme';

/**
 * 主题状态管理
 */
export const useThemeStore = defineStore('theme', () => {
  const mode = ref<TaktThemeMode>(readStoredThemeMode());
  const prefersDark = usePreferredDark();

  /**
   * 实际生效的主题（light / dark）
   */
  const resolvedTheme = computed<TaktResolvedTheme>(() => {
    if (mode.value === 'system') {
      return prefersDark.value ? 'dark' : 'light';
    }

    return mode.value;
  });

  watch(
    resolvedTheme,
    (resolved) => {
      applyThemeDom(resolved);
    },
    { immediate: true }
  );

  /**
   * 设置主题模式
   * @param next 目标模式
   */
  function setThemeMode(next: TaktThemeMode): void {
    if (mode.value === next) {
      return;
    }

    mode.value = next;
    localStorage.setItem(TAKT_THEME_STORAGE_KEY, next);
    EventBus.emit('theme:change', { theme: resolvedTheme.value });
  }

  /**
   * 在浅色与深色之间切换（不经过 system）
   */
  function toggleThemeMode(): void {
    setThemeMode(resolvedTheme.value === 'dark' ? 'light' : 'dark');
  }

  return {
    mode,
    resolvedTheme,
    setThemeMode,
    toggleThemeMode,
  };
});
