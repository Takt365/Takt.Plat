// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/config
// 文件名称：vite-env.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：读取 VITE_ 环境变量（必填、无静态兜底）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 读取 Vite 注入的环境变量对象
 * @description 必须使用 `import.meta.env` 字面量；`import.meta?.env` 不会被 Vite 静态替换
 * @returns {ImportMetaEnv} 环境变量
 */
export function readViteEnv(): ImportMetaEnv {
  return import.meta.env;
}

/**
 * 是否处于 Vite 客户端运行时（MODE 已由构建注入）
 */
export function isViteEnvInjected(): boolean {
  return typeof import.meta.env.MODE === 'string' && import.meta.env.MODE.length > 0;
}

/**
 * 读取必填 Vite 环境变量
 * @param key 变量名
 * @returns 去首尾空格后的值
 */
export function requireViteEnv(key: keyof ImportMetaEnv): string {
  const raw = import.meta.env[key];

  if (typeof raw !== 'string' || raw.trim() === '') {
    throw new Error(`[env] 缺少环境变量 ${String(key)}，请配置 frontend/.env、.env.development 或 .env.production`);
  }

  return raw.trim();
}

/**
 * 去掉末尾斜杠
 * @param origin 根地址
 */
export function normalizeOrigin(origin: string): string {
  return origin.replace(/\/$/, '');
}

/**
 * 拼接根地址与路径
 * @param origin 根地址
 * @param path 以 / 开头的路径
 */
export function joinOriginPath(origin: string, path: string): string {
  const base = normalizeOrigin(origin);
  const suffix = path.startsWith('/') ? path : `/${path}`;
  return `${base}${suffix}`;
}

/**
 * 浏览器访问后端根地址（OAuth、SignalR 等与页面/API 代理同源策略一致）
 */
export function getAppOrigin(): string {
  return normalizeOrigin(requireViteEnv('VITE_APP_ORIGIN'));
}
