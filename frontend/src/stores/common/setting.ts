// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/stores/common
// 文件名称：setting.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：全局应用偏好 Pinia 状态（读写见 @/setting；主题 DOM 见 theme Store + App.vue）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { AppSetting } from '@/types/setting';
import {
  defaultSetting,
  normalizeSetting,
  readSettingFromStorage,
  saveSettingToStorage,
} from '@/setting';

export type { AppSetting, ThemeColor, ThemeColorConfig } from '@/types/setting';

export {
  defaultSetting,
  getSetting,
  getThemeColorValue,
  normalizeSetting,
  STORAGE_KEY,
  themeColorI18nKeyMap,
  themeColorMap,
  themeColorToPreset,
  validateFontSize,
} from '@/setting';

/**
 * 全局应用偏好 Store
 */
export const useSettingStore = defineStore('setting', () => {
  /** 当前应用偏好（初始化自 localStorage） */
  const setting = ref<AppSetting>(readSettingFromStorage());

  /**
   * 更新并持久化设置
   * @param next 完整或合并后的设置对象
   */
  function setSetting(next: AppSetting): void {
    const normalized = normalizeSetting(next);
    setting.value = normalized;
    saveSettingToStorage(normalized);
  }

  /**
   * 局部更新并持久化（不触发 applySettings，供 theme Store 回写）
   * @param partial 局部字段
   */
  function patchSetting(partial: Partial<AppSetting>): void {
    setSetting({ ...setting.value, ...partial });
  }

  /**
   * 恢复默认设置并持久化
   */
  function resetSetting(): void {
    setSetting(defaultSetting);
  }

  /**
   * 标记用户已手动选择主题外观（明暗/主色）；布局等其它偏好不受影响
   */
  function markAppearanceUserOverride(): void {
    if (!setting.value.appearanceUserOverride) {
      patchSetting({ appearanceUserOverride: true });
    }
  }

  return {
    setting,
    setSetting,
    patchSetting,
    resetSetting,
    markAppearanceUserOverride,
  };
});
