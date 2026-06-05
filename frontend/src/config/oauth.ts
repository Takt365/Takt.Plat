// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/config
// 文件名称：oauth.ts
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：OAuth2 Authorization Code + PKCE 客户端配置（全部来自环境变量）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getAppOrigin, joinOriginPath, requireViteEnv } from '@/config/vite-env';
import type { TaktOAuthConfig } from '@/types/common';

export type { TaktOAuthConfig };

let oauthConfigCache: TaktOAuthConfig | null = null;

/**
 * 读取 OAuth 配置（延迟到首次使用，避免模块加载时 import.meta.env 未就绪）
 * @returns {TaktOAuthConfig} OAuth 配置
 */
export function getOAuthConfig(): TaktOAuthConfig {
  if (oauthConfigCache) {
    return oauthConfigCache;
  }

  const appOrigin = getAppOrigin();
  oauthConfigCache = {
    issuer: appOrigin,
    clientId: requireViteEnv('VITE_OAUTH_CLIENT_ID'),
    redirectUri: requireViteEnv('VITE_OAUTH_REDIRECT_URI'),
    scope: requireViteEnv('VITE_OAUTH_SCOPE'),
    authorizationEndpoint: joinOriginPath(appOrigin, requireViteEnv('VITE_OAUTH_AUTHORIZE_PATH')),
    tokenEndpoint: joinOriginPath(appOrigin, requireViteEnv('VITE_OAUTH_TOKEN_PATH')),
  };

  return oauthConfigCache;
}
