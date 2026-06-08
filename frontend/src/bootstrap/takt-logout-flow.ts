// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/bootstrap
// 文件名称：takt-logout-flow.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：统一登出编排（服务端 signOut 防重入；空闲/手动/过期共用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { signOutSession } from '@/api/identity/auths';
import { useUserStore } from '@/stores/identity/user';

/** 登出流程进行中（防止 idle / 401 并发重复清态） */
let logoutInProgress = false;

/**
 * 是否正在执行登出清态
 * @returns 登出进行中为 true
 */
export function isLogoutInProgress(): boolean {
  return logoutInProgress;
}

/**
 * 尽力清除服务端 Cookie 会话（失败不抛出；须 token 仍在时使用）
 * @returns {Promise<void>}
 */
export async function runServerSignOutIfLoggedInAsync(): Promise<void> {
  if (!useUserStore().isLoggedIn) {
    return;
  }

  try {
    await signOutSession();
  } catch {
    //  intentional logout：401/网络失败均不阻断前端清态
  }
}

/**
 * 在登出防重入守卫内执行动作
 * @param action 登出步骤（可 async）
 * @returns 已在登出中则 undefined，否则为 action 返回值
 */
export async function withLogoutInProgress<T>(
  action: () => T | Promise<T>
): Promise<T | undefined> {
  if (logoutInProgress) {
    return undefined;
  }

  logoutInProgress = true;
  try {
    return await action();
  } finally {
    logoutInProgress = false;
  }
}
