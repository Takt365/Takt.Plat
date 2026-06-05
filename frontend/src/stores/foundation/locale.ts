// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/stores/foundation
// 文件名称：locale.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：语言区域状态管理（vue-i18n + 后端 GetCultureOptionsAsync / TaktSelectOption）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import i18n from '@/locales';
import { getCultureOptions } from '@/api/foundation/culture';
import { getSessionCultureOptions } from '@/api/identity/auths';
import type { TaktSelectOption } from '@/types/common';
import { EventBus } from '@/utils/event-bus';
import { useUserStore } from '@/stores/identity/user';
import { useTenantStore } from '@/stores/identity/tenant';
import { useTranslationStore } from '@/stores/foundation/translation';
import {
  TAKT_DEFAULT_LOCALE,
  TAKT_LOCALE_STORAGE_KEY,
} from '@/utils/common';
import {
  readStoredLocale,
  resolveTaktCultureCode,
  syncTaktComponentLocales,
} from '@/utils/takt-locale-sync';

/**
 * 解析发往后端的 Accept-Language（与 vue-i18n 当前 locale 一致）
 * @returns 区域文化编码
 */
export function resolveRequestLocale(): string {
  const current = i18n.global.locale.value;

  if (typeof current === 'string') {
    return resolveTaktCultureCode(current);
  }

  return readStoredLocale();
}

/**
 * 区域文化编码（dictValue = CultureCode）
 * @param option 语言选项
 */
export function resolveCultureCode(option: TaktSelectOption): string {
  return resolveTaktCultureCode(String(option.dictValue));
}

/**
 * 语言图标（extValue = Icon）
 * @param option 语言选项
 */
export function resolveCultureIcon(option: TaktSelectOption): string | undefined {
  if (option.extValue === undefined || option.extValue === null) {
    return undefined;
  }

  const icon = String(option.extValue).trim();

  return icon ? icon : undefined;
}

/**
 * 是否默认语言（extLabel = IsDefault，1=是）
 * @param option 语言选项
 */
export function resolveCultureIsDefault(option: TaktSelectOption): boolean {
  return String(option.extLabel ?? '') === '1';
}

/**
 * 解析加载语言选项失败文案
 * @param error 捕获的错误
 */
function resolveCultureLoadErrorMessage(error: unknown): string {
  if (error instanceof Error && error.message) {
    return error.message;
  }

  return '加载语言列表失败';
}

/**
 * 语言区域状态管理
 */
export const useLocaleStore = defineStore('locale', () => {
  const cultureOptions = ref<TaktSelectOption[]>([]);
  const loading = ref(false);
  const loaded = ref(false);
  /** 上次加载语言列表时的上下文键（登录态 / 租户编码 / 会话合并） */
  const cultureOptionsContextKey = ref('');

  /**
   * 登录页用户是否已手动切换语言（为 true 时不再被用户默认语言覆盖）
   */
  const loginLocaleUserOverride = ref(false);

  /**
   * 当前 vue-i18n locale（与选项 dictValue / CultureCode 一致）
   */
  const currentLocale = computed(() => i18n.global.locale.value);

  /**
   * 当前选中的语言选项
   */
  const currentCultureOption = computed(() =>
    cultureOptions.value.find((item) => resolveCultureCode(item) === currentLocale.value)
  );

  /**
   * 解析当前语言列表加载上下文键
   * @param tenantCode 显式租户（未登录会话接口用）
   */
  function resolveCultureOptionsContextKey(tenantCode?: string): string {
    const userStore = useUserStore();
    if (userStore.isLoggedIn) {
      return `auth:${useTenantStore().tenantCode || ''}`;
    }

    const code = tenantCode?.trim() || useTenantStore().tenantCode?.trim() || '';
    return code ? `session:${code}` : 'session:all';
  }

  /**
   * 从后端加载启用语言选项（已登录：GetCultureOptions；登录页：SessionCultureOptions）
   * @param options.force 为 true 时强制刷新（租户切换等）
   * @param options.tenantCode 未登录时显式指定租户（可选）
   */
  /**
   * 清除登录页会话语言选项（租户未校验或切换租户前）
   */
  function clearSessionCultureOptions(): void {
    cultureOptions.value = [];
    cultureOptionsContextKey.value = '';
    loaded.value = false;
  }

  async function loadCultureOptionsAsync(options: { force?: boolean; tenantCode?: string } = {}): Promise<void> {
    const userStore = useUserStore();
    const explicitTenant = options.tenantCode?.trim() || useTenantStore().tenantCode?.trim() || '';
    if (!userStore.isLoggedIn && !explicitTenant) {
      clearSessionCultureOptions();
      return;
    }

    const contextKey = resolveCultureOptionsContextKey(options.tenantCode);
    if (loading.value) {
      return;
    }

    if (loaded.value && !options.force && cultureOptionsContextKey.value === contextKey) {
      return;
    }

    loading.value = true;

    try {
      const list = userStore.isLoggedIn
        ? await getCultureOptions()
        : await getSessionCultureOptions(explicitTenant);

      if (list.length === 0) {
        throw new Error('未获取到可用的语言列表');
      }

      cultureOptions.value = list;
      cultureOptionsContextKey.value = contextKey;
      loaded.value = true;
    } catch (error) {
      cultureOptions.value = [];
      cultureOptionsContextKey.value = '';
      loaded.value = false;

      EventBus.emit('notification:show', {
        type: 'error',
        message: resolveCultureLoadErrorMessage(error),
      });

      throw error;
    } finally {
      loading.value = false;
    }
  }

  /**
   * 应用语言（vue-i18n + localStorage + 后续请求 Accept-Language）
   * @param cultureCode 区域文化编码（dictValue）
   * @param persist 是否写入 localStorage（用户主动选择时为 true）
   */
  function applyLocale(cultureCode: string, persist = true): void {
    const normalized = resolveTaktCultureCode(cultureCode);

    if (i18n.global.locale.value !== normalized) {
      i18n.global.locale.value = normalized as typeof i18n.global.locale.value;
    }

    syncTaktComponentLocales(normalized);

    if (persist) {
      localStorage.setItem(TAKT_LOCALE_STORAGE_KEY, normalized);
    }
  }

  /**
   * 切换当前语言（用户选择；同步后端 Accept-Language）
   * @param cultureCode 区域文化编码（dictValue）
   */
  function setLocale(cultureCode: string): void {
    const normalized = resolveTaktCultureCode(cultureCode);
    const isLoggedIn = useUserStore().isLoggedIn;

    if (!isLoggedIn) {
      loginLocaleUserOverride.value = true;
    }

    if (i18n.global.locale.value === normalized) {
    if (isLoggedIn) {
      const translationStore = useTranslationStore();
      if (translationStore.isLocaleLoaded(normalized)) {
        translationStore.bumpDynamicRevision();
      } else {
        void translationStore.loadTranslationMessagesAsync(normalized);
      }
    } else {
      void useTranslationStore().loadPublicTranslationMessagesAsync(normalized);
    }
    return;
  }

  applyLocale(normalized, true);
  EventBus.emit('locale:change', { locale: normalized });

  if (isLoggedIn) {
    void useTranslationStore().loadTranslationMessagesAsync(normalized);
  } else {
    void useTranslationStore().loadPublicTranslationMessagesAsync(normalized);
  }
  }

  /**
   * 启动时对齐默认语言（无 localStorage 记录时为 en-US）
   */
  function initLocaleFromStorage(): void {
    applyLocale(readStoredLocale(), Boolean(localStorage.getItem(TAKT_LOCALE_STORAGE_KEY)));
  }

  /**
   * 登录页进入时重置界面语言（清除上次会话残留，待用户默认语言解析后再写入）
   */
  function resetLocaleForLoginPage(): void {
    loginLocaleUserOverride.value = false;
    localStorage.removeItem(TAKT_LOCALE_STORAGE_KEY);
    clearSessionCultureOptions();
    applyLocale(TAKT_DEFAULT_LOCALE, false);
  }

  /**
   * 清除登录页手动语言覆盖（租户或用户名变更、重新解析默认公司前调用）
   */
  function clearLoginLocaleUserOverride(): void {
    loginLocaleUserOverride.value = false;
  }

  /**
   * 按用户 DefaultCulture 应用登录预览语言（写入 localStorage）
   * @param cultureCode 用户默认区域文化编码 BCP47
   */
  function applyLoginUserLocale(
    cultureCode: string,
    _context?: {
      tenantCode?: string;
      username?: string;
      companyCode?: string;
      previewKey?: string;
    },
  ): void {
    if (loginLocaleUserOverride.value) {
      return;
    }

    const normalized = resolveTaktCultureCode(cultureCode.trim());
    applyLocale(normalized, true);
  }

  /**
   * 登录后按用户资料 DefaultCulture 对齐界面语言（不覆盖用户手动切换）
   * @param cultureCode 用户默认区域文化编码 BCP47
   */
  function applyUserProfileLocale(cultureCode: string): void {
    if (loginLocaleUserOverride.value) {
      return;
    }

    const normalized = resolveTaktCultureCode(cultureCode.trim());
    if (i18n.global.locale.value === normalized) {
      return;
    }

    applyLocale(normalized, true);
    void useTranslationStore().loadTranslationMessagesAsync(normalized);
  }

  return {
    cultureOptions,
    loading,
    loaded,
    currentLocale,
    currentCultureOption,
    loadCultureOptionsAsync,
    clearSessionCultureOptions,
    initLocaleFromStorage,
    resetLocaleForLoginPage,
    clearLoginLocaleUserOverride,
    applyLoginUserLocale,
    applyUserProfileLocale,
    applyLocale,
    setLocale,
  };
});
