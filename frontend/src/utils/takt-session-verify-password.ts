// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-session-verify-password.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：登录预检 verify-password 响应字段规范化（camelCase / PascalCase 兼容）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SessionVerifyPasswordResponse } from '@/types/identity/login';

/**
 * 规范化 verify-password 响应（兼容后端 PascalCase 与前端 camelCase）
 * @param raw 接口原始 data
 * @returns {SessionVerifyPasswordResponse} 规范化后的 DTO
 */
export function normalizeSessionVerifyPasswordResponse(
  raw: SessionVerifyPasswordResponse,
): SessionVerifyPasswordResponse {
  const record = raw as SessionVerifyPasswordResponse & Record<string, unknown>;

  return {
    passwordValid: Boolean(record.passwordValid ?? record.PasswordValid),
    captchaRequired: Boolean(record.captchaRequired ?? record.CaptchaRequired),
    loginTicket: String(record.loginTicket ?? record.LoginTicket ?? ''),
  };
}
