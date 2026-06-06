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
  /** 用户选择的主题模式（含 system） */
  const mode = ref<TaktThemeMode>(readStoredThemeMode());
  /** 系统是否偏好深色（跟随 OS） */
  const prefersDark = usePreferredDark();

  /**
   * 实际生效的主题（light / dark）
   */
  const resolvedTheme = computed<TaktResolvedTheme>(() => {
    // system 模式跟随 OS 深色偏好
    if (mode.value === 'system') {
      return prefersDark.value ? 'dark' : 'light';
    }

    return mode.value;
  });

  // 解析后的主题变化时同步 DOM data-theme 与 CSS 变量
  watch(
    resolvedTheme,
    (resolved) => {
      applyThemeDom(resolved);
    },
    { immediate: true }
  );

  /**
   * 设置主题模式
   * @param {TaktThemeMode} next 目标模式
   * @returns {void}
   */
  function setThemeMode(next: TaktThemeMode): void {
    // 相同模式不重复持久化与广播
    if (mode.value === next) {
      return;
    }

    // 更新 Store 模式
    mode.value = next;
    // 写入 localStorage
    localStorage.setItem(TAKT_THEME_STORAGE_KEY, next);
    // 广播已解析主题（非 system 字面量），供 bootstrap 等同步
    EventBus.emit('theme:change', { theme: resolvedTheme.value });
  }

  /**
   * 在浅色与深色之间切换（不经过 system）
   * @returns {void}
   */
  function toggleThemeMode(): void {
    // 按当前生效主题取反
    setThemeMode(resolvedTheme.value === 'dark' ? 'light' : 'dark');
  }

  return {
    mode,
    resolvedTheme,
    setThemeMode,
    toggleThemeMode,
  };
});
