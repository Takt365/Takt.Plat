// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/stores/common
// 文件名称：setting.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：全局应用偏好 Pinia 状态（配置读写见 @/setting）
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
  themeColorMap,
  validateFontSize,
} from '@/setting';

/**
 * 全局应用偏好 Store
 */
export const useSettingStore = defineStore('setting', () => {
  const setting = ref<AppSetting>(readSettingFromStorage());

  /**
   * 更新并持久化设置
   * @param {AppSetting} next 完整或合并后的设置对象
   */
  function setSetting(next: AppSetting): void {
    const normalized = normalizeSetting(next);
    setting.value = normalized;
    saveSettingToStorage(normalized);
  }

  /**
   * 恢复默认设置并持久化
   */
  function resetSetting(): void {
    setSetting(defaultSetting);
  }

  return {
    setting,
    setSetting,
    resetSetting,
  };
});
