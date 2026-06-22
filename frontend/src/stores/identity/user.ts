// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/stores/identity
// 文件名称：user.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：用户状态管理（OAuth2 Bearer Token + RBAC 权限）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { getCurrentUser, getLoginPreviewLocale } from '@/api/identity/auths';
import type { UserInfoResponse, LoginPreviewLocale } from '@/types/identity/login';
import { useTenantStore } from '@/stores/identity/tenant';
import type { MenuTree } from '@/types/identity/menu';
import { hasPermissionCode } from '@/utils/permission';
import { normalizeUserInfoProfile } from '@/utils/takt-user-profile-normalize';
import { getHolidayTheme } from '@/api/human-resource/attendance/holiday';
import { useLocaleStore } from '@/stores/foundation/locale';
import type { HolidayTheme } from '@/types/human-resource/attendance/holiday';
import { resolveThemeColorPreset } from '@/utils/theme';
import { useThemeColorStore } from '@/stores/common/theme-color';
import {
  TAKT_ACCESS_TOKEN_STORAGE_KEY,
  TAKT_REFRESH_TOKEN_STORAGE_KEY,
  TAKT_TOKEN_EXPIRES_STORAGE_KEY,
} from '@/utils/common';

/** 资料加载单飞 Promise */
let profileLoadPromise: Promise<void> | null = null;

/** 登录预览（假日主题/默认语言）请求序号，丢弃过期响应 */
let loginPreviewSyncSeq = 0;

/**
 * 构建登录预览上下文键（租户 + 用户名）
 * @param tenantCode 租户编码
 * @param username 用户名
 */
function buildLoginPreviewKey(tenantCode: string, username: string): string {
  return `${tenantCode.trim()}|${username.trim()}`;
}

/**
 * 登录预览上下文是否仍有效（防竞态覆盖）
 * @param seq 请求序号
 * @param tenantCode 租户编码
 * @param username 用户名
 * @param previewKey 预览上下文键
 */
function isPreviewContextValid(
  seq: number,
  tenantCode: string,
  username: string,
  previewKey: string,
): boolean {
  if (seq !== loginPreviewSyncSeq) {
    return false;
  }

  const tenantStore = useTenantStore();
  if (tenantStore.tenantCode.trim() !== tenantCode.trim()) {
    return false;
  }

  return buildLoginPreviewKey(tenantCode, username) === previewKey;
}

/**
 * 用户状态管理
 */
export const useUserStore = defineStore('user', () => {
  const token = ref<string>(localStorage.getItem(TAKT_ACCESS_TOKEN_STORAGE_KEY) || '');
  const refreshToken = ref<string>(localStorage.getItem(TAKT_REFRESH_TOKEN_STORAGE_KEY) || '');
  const tokenExpiresAt = ref<number>(Number(localStorage.getItem(TAKT_TOKEN_EXPIRES_STORAGE_KEY) || 0));
  const userId = ref<string>('');
  const username = ref<string>('');
  /** 当前登录用户类型（来自 GET /me，字典 sys_user_type） */
  const userType = ref<number>(0);
  const permissions = ref<string[]>([]);
  const roles = ref<string[]>([]);
  const routePaths = ref<string[]>([]);
  const menus = ref<MenuTree[]>([]);
  const profileLoaded = ref(false);
  /** 当前登录用户完整资料（GET /me，供头像、工作流等读取） */
  const userInfo = ref<UserInfoResponse | null>(null);
  /** 当前会话假日信息（公开 API 或登录后同步，供工作台问候与主题色） */
  const holidayFromToken = ref<HolidayTheme | null>(null);

  const isLoggedIn = computed(() => !!token.value);

  const hasPermission = computed(() => {
    return (permission: string) => hasPermissionCode(permissions.value, permission);
  });

  /**
   * 写入 OAuth 令牌（Authorization Code + PKCE）
   */
  function setOAuthTokens(data: {
    accessToken: string;
    refreshToken?: string;
    expiresIn: number;
  }) {
    token.value = data.accessToken;
    localStorage.setItem(TAKT_ACCESS_TOKEN_STORAGE_KEY, data.accessToken);

    if (data.refreshToken) {
      refreshToken.value = data.refreshToken;
      localStorage.setItem(TAKT_REFRESH_TOKEN_STORAGE_KEY, data.refreshToken);
    }

    const expiresAt = Date.now() + data.expiresIn * 1000;
    tokenExpiresAt.value = expiresAt;
    localStorage.setItem(TAKT_TOKEN_EXPIRES_STORAGE_KEY, String(expiresAt));
  }

  /**
   * 写入用户资料（权限、菜单、路由）
   */
  function setUserProfile(profile: UserInfoResponse) {
    const normalized = normalizeUserInfoProfile(profile);
    userId.value = String(normalized.userId);
    username.value = normalized.username;
    userType.value = normalized.userType ?? 0;
    permissions.value = normalized.permissions ?? [];
    roles.value = normalized.roles ?? [];
    routePaths.value = normalized.routePaths ?? [];
    menus.value = normalized.menus ?? [];
    userInfo.value = normalized;
    profileLoaded.value = true;
  }

  /**
   * 从服务端加载当前用户资料（路由守卫 / 登录回调使用）
   * @param force 是否强制刷新
   */
  async function loadUserProfile(force = false): Promise<void> {
    if (!token.value) {
      return;
    }

    if (profileLoadPromise) {
      await profileLoadPromise;
      if (!force || profileLoaded.value) {
        return;
      }
    }

    if (profileLoaded.value && !force) {
      return;
    }

    profileLoadPromise = (async () => {
      const profile = normalizeUserInfoProfile(await getCurrentUser());
      setUserProfile(profile);
      const tenantStore = useTenantStore();
      tenantStore.syncFromUserProfile(profile);
      tenantStore.clearOAuthTenantCode();
      await tenantStore.refreshBusinessContextAsync().catch(() => undefined);
      await loadHolidayThemeForCurrentSession().catch(() => undefined);
      const userCulture = profile.defaultCulture?.trim();
      if (userCulture) {
        useLocaleStore().applyUserProfileLocale(userCulture);
      }
    })();

    try {
      await profileLoadPromise;
    } finally {
      profileLoadPromise = null;
    }
  }

  /**
   * @deprecated 兼容旧 mock，请使用 setOAuthTokens + setUserProfile
   */
  function setLoginInfo(data: {
    token: string;
    userId: string;
    username: string;
    permissions: string[];
    roles: string[];
  }) {
    token.value = data.token;
    userId.value = data.userId;
    username.value = data.username;
    permissions.value = data.permissions;
    roles.value = data.roles;
    profileLoaded.value = true;
    localStorage.setItem(TAKT_ACCESS_TOKEN_STORAGE_KEY, data.token);
  }

  function logout() {
    profileLoadPromise = null;
    loginPreviewSyncSeq += 1;
    token.value = '';
    refreshToken.value = '';
    tokenExpiresAt.value = 0;
    userId.value = '';
    username.value = '';
    userType.value = 0;
    permissions.value = [];
    roles.value = [];
    routePaths.value = [];
    menus.value = [];
    userInfo.value = null;
    profileLoaded.value = false;
    holidayFromToken.value = null;
    localStorage.removeItem(TAKT_ACCESS_TOKEN_STORAGE_KEY);
    localStorage.removeItem(TAKT_REFRESH_TOKEN_STORAGE_KEY);
    localStorage.removeItem(TAKT_TOKEN_EXPIRES_STORAGE_KEY);
    useTenantStore().resetCompanySelection();
  }

  function updatePermissions(next: string[]) {
    permissions.value = next;
  }

  /**
   * 使已缓存的用户资料失效（租户切换等需重新拉取 /me）
   */
  function invalidateUserProfile(): void {
    profileLoaded.value = false;
  }

  /**
   * 写入假日 DTO，并在存在合法 holidayTheme 时同步主题色预设
   * @param holidayDto 假日主题 DTO；无匹配记录时可为空对象或 null
   */
  function applyHolidayThemeSideEffects(holidayDto: HolidayTheme | null): void {
    holidayFromToken.value = holidayDto;
    const themeKey = holidayDto?.holidayTheme?.trim();
    const resolvedPreset = resolveThemeColorPreset(themeKey);
    if (resolvedPreset) {
      useThemeColorStore().setColorPreset(resolvedPreset);
    }
  }

  /**
   * 登录页预览：租户已校验后，按「租户+用户名」解析默认公司与用户 DefaultCulture，再按「租户+公司」拉取当日假日。
   * 界面语言由 {@link useLocaleStore.applyLoginUserLocale} 写入；用户手动切换后不会被覆盖。
   * @param tenantCode 已校验的租户编码
   * @param username 登录用户名
   * @param previewKey 预览上下文键（租户+用户名）
   * @returns {Promise<LoginPreviewLocale | null>} 预览解析结果；上下文失效时返回 null
   */
  async function syncLoginPreviewAsync(
    tenantCode: string,
    username: string,
    previewKey?: string,
  ): Promise<LoginPreviewLocale | null> {
    const trimmedTenant = tenantCode.trim();
    const trimmedUser = username.trim();
    if (!trimmedTenant || !trimmedUser) {
      return null;
    }

    const expectedKey = previewKey ?? buildLoginPreviewKey(trimmedTenant, trimmedUser);
    const seq = ++loginPreviewSyncSeq;

    const localeDto = await getLoginPreviewLocale(trimmedTenant, trimmedUser);
    if (!isPreviewContextValid(seq, trimmedTenant, trimmedUser, expectedKey)) {
      return null;
    }

    if (!localeDto.tenantFound || !localeDto.userFound) {
      holidayFromToken.value = null;
      return localeDto;
    }

    let holidayDto: HolidayTheme | null = null;
    const companyCode = localeDto.companyCode?.trim();
    if (localeDto.defaultCompanyFound && companyCode) {
      holidayDto = await getHolidayTheme(trimmedTenant, companyCode);
      if (!isPreviewContextValid(seq, trimmedTenant, trimmedUser, expectedKey)) {
        return null;
      }
    }

    const defaultCulture = localeDto.defaultCulture?.trim();
    if (defaultCulture) {
      useLocaleStore().applyLoginUserLocale(defaultCulture, {
        tenantCode: trimmedTenant,
        username: trimmedUser,
        companyCode,
        previewKey: expectedKey,
      });
    }

    applyHolidayThemeSideEffects(holidayDto);

    return localeDto;
  }

  /**
   * 按租户与公司加载当日假日主题（已登录场景，不改动界面语言）
   * @param tenantCode 租户编码
   * @param companyCode 公司编码
   * @returns {Promise<void>}
   */
  async function loadHolidayThemeByCompany(tenantCode: string, companyCode: string): Promise<void> {
    const trimmedTenant = tenantCode.trim();
    const trimmedCompany = companyCode.trim();
    if (!trimmedTenant || !trimmedCompany) {
      holidayFromToken.value = null;
      return;
    }

    try {
      const holidayDto = await getHolidayTheme(trimmedTenant, trimmedCompany);
      applyHolidayThemeSideEffects(holidayDto);
    } catch {
      // 假日提示非阻断
    }
  }

  /**
   * 按当前会话租户与公司自动拉取当日假日并应用主题色（登录后 / 切换公司后调用）
   * @returns {Promise<void>}
   */
  async function loadHolidayThemeForCurrentSession(): Promise<void> {
    const tenantStore = useTenantStore();
    await loadHolidayThemeByCompany(tenantStore.tenantCode, tenantStore.companyCode);
  }

  /** 作废进行中的登录预览请求（租户或用户名变更时调用） */
  function invalidateLoginPreview(): void {
    loginPreviewSyncSeq += 1;
    holidayFromToken.value = null;
  }

  return {
    token,
    refreshToken,
    tokenExpiresAt,
    userId,
    username,
    userType,
    permissions,
    roles,
    routePaths,
    menus,
    profileLoaded,
    userInfo,
    holidayFromToken,
    isLoggedIn,
    hasPermission,
    setOAuthTokens,
    setUserProfile,
    loadUserProfile,
    setLoginInfo,
    logout,
    updatePermissions,
    invalidateUserProfile,
    syncLoginPreviewAsync,
    loadHolidayThemeByCompany,
    loadHolidayThemeForCurrentSession,
    invalidateLoginPreview,
  };
});
