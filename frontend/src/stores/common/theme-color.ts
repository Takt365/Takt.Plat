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
  /** 当前主题色预设键（初始化自 localStorage） */
  const preset = ref<TaktThemeColorPreset>(readStoredThemeColorPreset());

  /**
   * 当前主色色值
   */
  const colorPrimary = computed(() => {
    // 由预设键映射为 hex 主色
    return getThemeColorValue(preset.value);
  });

  /**
   * 设置主题色预设
   * @param {TaktThemeColorPreset} next 预设键名
   * @returns {void}
   */
  function setColorPreset(next: TaktThemeColorPreset): void {
    // 相同预设不重复写入与广播
    if (preset.value === next) {
      return;
    }

    // 更新响应式预设
    preset.value = next;
    // 持久化到 localStorage
    localStorage.setItem(TAKT_THEME_COLOR_STORAGE_KEY, next);
    // 广播主题色变更，apply-settings 等可同步 CSS 变量
    EventBus.emit('theme-color:change', { preset: next, color: getThemeColorValue(next) });
  }

  /**
   * 循环切换到下一个预设
   * @returns {void}
   */
  function toggleColorPreset(): void {
    /** 当前预设在列表中的下标 */
    const currentIndex = themeColorPresetKeys.indexOf(preset.value);
    /** 下一个预设（环状） */
    const next = themeColorPresetKeys[(currentIndex + 1) % themeColorPresetKeys.length];

    if (next) {
      // 应用下一预设
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
