// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/config
// 文件名称：auth-idle.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：认证空闲超时配置（VITE_AUTH_IDLE_TIMEOUT_MINUTES）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { TAKT_AUTH_IDLE_DEFAULT_TIMEOUT_MINUTES } from '@/utils/common';

/**
 * 解析空闲自动登出超时时长（毫秒）
 * @description VITE_AUTH_IDLE_TIMEOUT_MINUTES：0 或未配置有效正数则禁用
 * @returns {number} 超时时长（毫秒），0 表示禁用
 */
export function getAuthIdleTimeoutMs(): number {
  const raw = import.meta.env.VITE_AUTH_IDLE_TIMEOUT_MINUTES?.trim();

  if (!raw) {
    return TAKT_AUTH_IDLE_DEFAULT_TIMEOUT_MINUTES * 60 * 1000;
  }

  if (raw === '0') {
    return 0;
  }

  const minutes = Number(raw);

  if (Number.isNaN(minutes) || minutes <= 0) {
    return 0;
  }

  return minutes * 60 * 1000;
}

/**
 * 是否启用空闲自动登出
 * @returns {boolean} 是否启用
 */
export function isAuthIdleLogoutEnabled(): boolean {
  return getAuthIdleTimeoutMs() > 0;
}
