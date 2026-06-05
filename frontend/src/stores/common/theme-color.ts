// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/stores/common
// 文件名称：theme-color.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：主题色预设状态（ConfigProvider token.colorPrimary）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { EventBus } from '@/utils/event-bus';
import { TAKT_THEME_COLOR_STORAGE_KEY } from '@/utils/common';
import {
  getThemeColorValue,
  readStoredThemeColorPreset,
  themeColorPresetKeys,
  type TaktThemeColorPreset,
} from '@/utils/theme';

/**
 * 主题色预设状态管理
 */
export const useThemeColorStore = defineStore('theme-color', () => {
  const preset = ref<TaktThemeColorPreset>(readStoredThemeColorPreset());

  /**
   * 当前主色色值
   */
  const colorPrimary = computed(() => getThemeColorValue(preset.value));

  /**
   * 设置主题色预设
   * @param next 预设键名
   */
  function setColorPreset(next: TaktThemeColorPreset): void {
    if (preset.value === next) {
      return;
    }

    preset.value = next;
    localStorage.setItem(TAKT_THEME_COLOR_STORAGE_KEY, next);
    EventBus.emit('theme-color:change', { preset: next, color: getThemeColorValue(next) });
  }

  /**
   * 循环切换到下一个预设
   */
  function toggleColorPreset(): void {
    const currentIndex = themeColorPresetKeys.indexOf(preset.value);
    const next = themeColorPresetKeys[(currentIndex + 1) % themeColorPresetKeys.length];

    if (next) {
      setColorPreset(next);
    }
  }

  return {
    preset,
    colorPrimary,
    setColorPreset,
    toggleColorPreset,
  };
});
