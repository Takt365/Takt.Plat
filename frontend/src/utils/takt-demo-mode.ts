// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-demo-mode.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：演示模式（Demo）只读判定；偏好 setting.demo=true 时禁止写操作
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getActivePinia } from 'pinia';
import { getSetting } from '@/setting';
import { useSettingStore } from '@/stores/common/setting';

/** Demo 下禁止的工具栏/操作列 key 或权限末段 */
const DEMO_BLOCKED_OPS = new Set([
  'create',
  'createrow',
  'update',
  'edit',
  'delete',
  'deleterow',
  'import',
  'source',
  'empty',
  'approve',
  'reject',
  'refuse',
  'revoke',
  'initiate',
  'start',
  'send',
  'sendmessage',
  'execute',
  'batch',
  'specialaccept',
]);

/**
 * 当前是否演示只读（优先 Pinia，无 Pinia 时读 localStorage）
 * @returns {boolean} true=只读
 */
export function isDemoReadonly(): boolean {
  if (getActivePinia()) {
    return !!useSettingStore().setting.demo;
  }
  return !!getSetting().demo;
}

/**
 * HTTP 方法是否为写操作
 * @param method Axios method
 */
export function isMutatingHttpMethod(method?: string): boolean {
  const m = (method || 'get').toLowerCase();
  return m === 'post' || m === 'put' || m === 'patch' || m === 'delete';
}

/**
 * Demo 只读下仍允许的写请求（登录/鉴权/验证码/SignalR negotiate 等）
 * @param url 请求 url（相对或绝对）
 */
export function isDemoWriteExemptUrl(url?: string): boolean {
  const path = String(url ?? '').toLowerCase();
  if (!path) {
    return false;
  }
  const exempt = [
    '/connect/',
    'oauth',
    'token',
    'captcha',
    'session/signout',
    '/login',
    'negotiate',
    '/hubs/',
    'hub?',
  ];
  return exempt.some((p) => path.includes(p));
}

/**
 * 操作按钮/权限在 Demo 下是否应隐藏或禁用
 * @param actionKey 操作 key（如 update）
 * @param permission 权限码（如 identity:user:update）
 */
export function isDemoBlockedAction(actionKey?: string, permission?: string): boolean {
  if (!isDemoReadonly()) {
    return false;
  }
  const key = String(actionKey ?? '')
    .trim()
    .toLowerCase();
  if (key && DEMO_BLOCKED_OPS.has(key)) {
    return true;
  }
  const op = String(permission ?? '')
    .split(':')
    .pop()
    ?.trim()
    .toLowerCase();
  if (op && DEMO_BLOCKED_OPS.has(op)) {
    return true;
  }
  return false;
}
