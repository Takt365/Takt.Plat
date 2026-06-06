// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/config
// 文件名称：vite-dev-plugin.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：Vite 开发服务器插件（终端请求日志，供 vite.config.ts 引用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Plugin, ViteDevServer } from 'vite';

/** 开发代理路径前缀（与 vite.config 代理段一致） */
const DEV_LOG_PATH_PREFIXES = ['/api', '/connect', '/hubs'] as const;

/**
 * 开发服务器 API / OAuth / SignalR 请求日志（仅 serve）
 * @returns {Plugin} Vite 插件
 */
export function vitePluginLogger(): Plugin {
  return {
    name: 'takt-vite-dev-logger',
    // 仅在 dev serve 模式生效，不影响 build
    apply: 'serve',
    configureServer(server: ViteDevServer) {
      // 挂载中间件拦截代理请求
      server.middlewares.use((req, _res, next) => {
        /** 请求 URL 路径（含 query） */
        const url = req.url ?? '';
        // 仅记录走后端代理的前缀，过滤静态资源噪音
        const shouldLog = DEV_LOG_PATH_PREFIXES.some((prefix) => url.startsWith(prefix));

        if (shouldLog) {
          // eslint-disable-next-line no-console -- 开发环境终端调试
          console.info(`[Vite Dev] ${req.method ?? 'GET'} ${url}`);
        }

        // 继续 Vite 中间件链
        next();
      });
    },
  };
}
