// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/vite.config
// 文件名称：vite.config.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：Vite 8 构建与开发服务器配置（环境变量驱动，无代码内静态兜底）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================
//
// 环境变量来源：frontend/.env、.env.development、.env.production
// 客户端运行时读取：src/config/vite-env.ts（requireViteEnv，缺失即抛错）
//
// 开发代理约定（VITE_API_BASE_URL 为相对路径时，如 /api）：
//   浏览器 → 同源 dev 服务器（VITE_APP_ORIGIN，如 https://localhost:60081）
//   Vite 代理 → VITE_API_PROXY_TARGET（如 https://localhost:60071）
//     - VITE_API_BASE_URL       → 业务 REST API
//     - /health                 → 根路径健康检查（预热 Cookie）
//     - VITE_PROXY_PATH_CONNECT → OAuth /connect/*
//     - VITE_PROXY_PATH_HUBS    → SignalR /hubs/*
//
// 样式：Tailwind CSS 4 + 全局 CSS（Ant Design Vue 全量注册 + reset.css）
// ========================================

import tailwindcss from '@tailwindcss/vite';
import { readFileSync } from 'node:fs';
import { dirname, extname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import dayjs from 'dayjs';
import { defineConfig, loadEnv, type Plugin, type PluginOption, type ProxyOptions, type ViteDevServer } from 'vite';
import vue from '@vitejs/plugin-vue';
import AutoImport from 'unplugin-auto-import/vite';
import Components from 'unplugin-vue-components/vite';
import { AntDesignVueResolver } from 'unplugin-vue-components/resolvers';
import mkcert from 'vite-plugin-mkcert';
import { VitePWA } from 'vite-plugin-pwa';
import { vitePluginLogger } from './src/config/vite-dev-plugin';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

// ---------------------------------------------------------------------------
// package.json → virtual:app-info
// ---------------------------------------------------------------------------

interface PackageJsonSummary {
  name?: string;
  version?: string;
  dependencies?: Record<string, string>;
  devDependencies?: Record<string, string>;
}

const pkg = JSON.parse(readFileSync(resolve(__dirname, 'package.json'), 'utf-8')) as PackageJsonSummary;
const { dependencies = {}, devDependencies = {}, name: pkgName = '', version: pkgVersion = '' } = pkg;

if (!pkgName.trim() || !pkgVersion.trim()) {
  throw new Error('[vite.config] package.json 缺少 name 或 version');
}

const __APP_INFO__ = {
  pkg: {
    name: pkgName.trim(),
    version: pkgVersion.trim(),
    dependencies,
    devDependencies,
  },
  lastBuildTime: dayjs().format('YYYY-MM-DD HH:mm:ss'),
};

const VIRTUAL_APP_INFO_ID = 'virtual:app-info';
const RESOLVED_APP_INFO_ID = '\0virtual:app-info';

// ---------------------------------------------------------------------------
// 环境变量解析（仅用于本配置文件；禁止 || '默认值' 式兜底）
// ---------------------------------------------------------------------------

/**
 * 读取必填环境变量
 * @param env loadEnv(mode, __dirname, '') 的键值表
 * @param key 变量名
 */
function requireEnv(env: Record<string, string>, key: string): string {
  const value = env[key]?.trim();
  if (!value) {
    throw new Error(`[vite.config] 缺少环境变量 ${key}，请配置 frontend/.env*`);
  }
  return value;
}

/**
 * 解析布尔型环境变量（仅接受 true / false / 1 / 0）
 */
function requireEnvBoolean(env: Record<string, string>, key: string): boolean {
  const value = requireEnv(env, key);
  if (value === 'true' || value === '1') {
    return true;
  }
  if (value === 'false' || value === '0') {
    return false;
  }
  throw new Error(`[vite.config] ${key} 必须为 true、false、1 或 0`);
}

/**
 * 解析数值型环境变量
 */
function requireEnvNumber(env: Record<string, string>, key: string): number {
  const value = Number(requireEnv(env, key));
  if (Number.isNaN(value)) {
    throw new Error(`[vite.config] ${key} 必须为有效数字`);
  }
  return value;
}

/**
 * 解析开发服务器 host
 * - 空字符串：不设置 host（仅本机）
 * - "true"：监听所有网卡
 * - 其它：作为 hostname
 */
function parseDevServerHost(value: string | undefined): string | boolean | undefined {
  if (value === undefined || value === '') {
    return undefined;
  }
  if (value === 'true') {
    return true;
  }
  return value;
}

/**
 * 解析 HMR 使用的 host 字符串（用于 wss 配置）
 */
function resolveHmrHost(host: string | boolean | undefined, port: number): string {
  if (host === true || host === undefined || host === '') {
    return 'localhost';
  }
  if (typeof host === 'string') {
    return host;
  }
  return String(port);
}

// ---------------------------------------------------------------------------
// 插件：virtual:app-info / 开发地址提示
// ---------------------------------------------------------------------------

/**
 * 虚拟模块：import 'virtual:app-info' → package 信息（供 src/utils/appMeta.ts）
 */
function appInfoPlugin(): Plugin {
  const payload = JSON.stringify(__APP_INFO__);

  return {
    name: 'takt-app-info',
    resolveId(id) {
      return id === VIRTUAL_APP_INFO_ID ? RESOLVED_APP_INFO_ID : undefined;
    },
    load(id) {
      if (id !== RESOLVED_APP_INFO_ID) {
        return undefined;
      }
      return `export default ${payload}`;
    },
  };
}

/**
 * 开发服务器启动后在终端打印实际访问协议，避免 HTTPS/HTTP 混用
 */
function devServerUrlHintPlugin(useHttps: boolean, port: number, host: string | boolean | undefined): Plugin {
  return {
    name: 'takt-dev-server-url-hint',
    configureServer(server: ViteDevServer) {
      server.httpServer?.once('listening', () => {
        const h = resolveHmrHost(host, port);
        const protocol = useHttps ? 'https' : 'http';
        // eslint-disable-next-line no-console -- 开发时显式提示，避免误用协议
        console.info(
          `\n[Takt] 前端开发地址: ${protocol}://${h}:${port}（与 VITE_DEV_SERVER_HTTPS=${useHttps} 一致；勿混用 http/https）\n`
        );
      });
    },
  };
}

// ---------------------------------------------------------------------------
// 开发服务器反向代理
// ---------------------------------------------------------------------------

/**
 * 为代理附加 X-Forwarded-Host / X-Forwarded-Proto（OAuth returnUrl 等）
 */
function attachForwardedHeaders(entry: ProxyOptions): void {
  entry.configure = (instance) => {
    instance.on('proxyReq', (proxyReq, req) => {
      const host = req.headers.host;
      if (host) {
        proxyReq.setHeader('X-Forwarded-Host', host);
      }

      const encrypted = (req.socket as { encrypted?: boolean } | undefined)?.encrypted === true;
      proxyReq.setHeader('X-Forwarded-Proto', encrypted ? 'https' : 'http');

      const userAgent = req.headers['user-agent'];
      if (typeof userAgent === 'string' && userAgent.trim()) {
        proxyReq.setHeader('User-Agent', userAgent);
        proxyReq.setHeader('X-Takt-User-Agent', userAgent);
      }
    });
  };
}

/**
 * 构建开发环境反向代理表（仅 development + 相对 API base）
 */
function buildDevProxy(
  env: Record<string, string>,
  mode: string
): Record<string, ProxyOptions> | undefined {
  if (mode !== 'development') {
    return undefined;
  }

  const apiBaseUrl = requireEnv(env, 'VITE_API_BASE_URL');
  if (!apiBaseUrl.startsWith('/')) {
    return undefined;
  }

  const proxyTarget = requireEnv(env, 'VITE_API_PROXY_TARGET');
  const proxyPathConnect = requireEnv(env, 'VITE_PROXY_PATH_CONNECT');
  const proxyPathHubs = requireEnv(env, 'VITE_PROXY_PATH_HUBS');

  const createBackendProxyEntry = (): ProxyOptions => ({
    target: proxyTarget,
    changeOrigin: requireEnvBoolean(env, 'VITE_API_PROXY_CHANGE_ORIGIN'),
    secure: requireEnvBoolean(env, 'VITE_API_PROXY_SECURE'),
    ws: true,
    timeout: 180_000,
    proxyTimeout: 180_000,
  });

  const proxy: Record<string, ProxyOptions> = {
    [apiBaseUrl]: createBackendProxyEntry(),
    '/health': createBackendProxyEntry(),
    [proxyPathConnect]: createBackendProxyEntry(),
    [proxyPathHubs]: createBackendProxyEntry(),
  };

  attachForwardedHeaders(proxy[apiBaseUrl]);
  attachForwardedHeaders(proxy['/health']);
  attachForwardedHeaders(proxy[proxyPathConnect]);
  attachForwardedHeaders(proxy[proxyPathHubs]);

  return proxy;
}

// ---------------------------------------------------------------------------
// 插件装配
// ---------------------------------------------------------------------------

function buildPlugins(
  env: Record<string, string>,
  mode: string,
  devServerPort: number,
  devServerHost: string | boolean | undefined,
  devHttps: boolean
): PluginOption[] {
  const plugins: PluginOption[] = [
    appInfoPlugin(),
    devServerUrlHintPlugin(devHttps, devServerPort, devServerHost),
    tailwindcss(),
    vue(),
    AutoImport({
      imports: [
        'vue',
        'vue-router',
        'pinia',
        {
          'ant-design-vue': ['message', 'notification', 'Modal'],
        },
        {
          '@/utils/logger': ['logger', 'createLogger'],
        },
      ],
      dirs: ['src/components', 'src/composables'],
      vueTemplate: true,
      dts: 'src/auto-imports.d.ts',
      eslintrc: {
        enabled: true,
        filepath: './.eslintrc-auto-import.json',
        globalsPropValue: true,
      },
    }),
    Components({
      resolvers: [
        AntDesignVueResolver({
          importStyle: false,
          // Watermark 为 CSS-in-JS，无 es/watermark/style；全局 app.use(Antd) 已注册，勿按需注入样式
          exclude: ['Watermark'],
        }),
      ],
      dirs: ['src/components'],
      extensions: ['vue'],
      dts: 'src/components.d.ts',
    }),
  ];

  if (devHttps && mode === 'development') {
    plugins.push(
      mkcert({
        hosts: ['localhost', '127.0.0.1', '::1'],
      })
    );
  }

  if (mode === 'development') {
    plugins.push(vitePluginLogger());
  }

  const pwaEnabled = requireEnvBoolean(env, 'VITE_PWA_ENABLED');
  if (pwaEnabled) {
    const appTitle = requireEnv(env, 'VITE_APP_TITLE');
    plugins.push(
      VitePWA({
        registerType: 'autoUpdate',
        includeAssets: ['favicon.ico'],
        manifest: {
          id: '/',
          name: appTitle,
          short_name: requireEnv(env, 'VITE_PWA_SHORT_NAME'),
          description: requireEnv(env, 'VITE_PWA_DESCRIPTION'),
          theme_color: requireEnv(env, 'VITE_PWA_THEME_COLOR'),
          background_color: requireEnv(env, 'VITE_PWA_BACKGROUND_COLOR'),
          display: 'standalone',
          orientation: 'portrait',
          start_url: '/',
          scope: '/',
          lang: 'zh-CN',
          icons: [
            {
              src: '/takt.svg',
              sizes: '192x192',
              type: 'image/svg+xml',
              purpose: 'any',
            },
            {
              src: '/takt.svg',
              sizes: '512x512',
              type: 'image/svg+xml',
              purpose: 'maskable',
            },
          ],
        },
        workbox: {
          globPatterns: ['**/*.{js,css,html,ico,png,svg,woff,woff2}'],
          runtimeCaching: [
            {
              urlPattern: /^https:\/\/.*\.(?:png|jpg|jpeg|svg|gif|webp)$/i,
              handler: 'CacheFirst',
              options: {
                cacheName: 'images-cache',
                expiration: {
                  maxEntries: 50,
                  maxAgeSeconds: 60 * 60 * 24 * 30,
                },
              },
            },
            {
              urlPattern: /\/api\/.*/i,
              handler: 'NetworkFirst',
              options: {
                cacheName: 'api-cache',
                networkTimeoutSeconds: 10,
                cacheableResponse: {
                  statuses: [0, 200],
                },
              },
            },
          ],
        },
        devOptions: {
          enabled: requireEnvBoolean(env, 'VITE_PWA_DEV_ENABLED'),
          type: 'module',
        },
      })
    );
  }

  return plugins;
}

// ---------------------------------------------------------------------------
// 路径别名（与 tsconfig.json compilerOptions.paths 保持一致）
// ---------------------------------------------------------------------------

const SRC_SUBDIR_ALIAS_ENTRIES = [
  'api',
  'bootstrap',
  'components',
  'composables',
  'config',
  'directives',
  'layouts',
  'locales',
  'router',
  'stores',
  'styles',
  'types',
  'utils',
  'views',
] as const;

function buildResolveAlias(configRoot: string): Record<string, string> {
  const srcDir = resolve(configRoot, 'src');
  const alias: Record<string, string> = {};

  for (const segment of SRC_SUBDIR_ALIAS_ENTRIES) {
    alias[`@${segment}`] = resolve(srcDir, segment);
  }

  alias['@'] = srcDir;

  return alias;
}

// ---------------------------------------------------------------------------
// 生产构建产物目录
// ---------------------------------------------------------------------------

const IMAGE_ASSET_EXTENSIONS = new Set([
  '.png',
  '.jpg',
  '.jpeg',
  '.gif',
  '.svg',
  '.webp',
  '.ico',
  '.avif',
  '.bmp',
]);

const FONT_ASSET_EXTENSIONS = new Set(['.woff', '.woff2', '.eot', '.ttf', '.otf']);
const MEDIA_ASSET_EXTENSIONS = new Set(['.mp4', '.webm', '.ogg', '.mp3', '.wav', '.m4a']);

interface BuildAssetFileMeta {
  names?: string[];
  name?: string;
}

function resolveAssetOutputDir(assetInfo: BuildAssetFileMeta): string {
  const fileName = assetInfo.names?.[0] ?? assetInfo.name ?? 'asset';
  const extension = extname(fileName).toLowerCase();

  if (extension === '.css') {
    return 'css';
  }
  if (IMAGE_ASSET_EXTENSIONS.has(extension)) {
    return 'img';
  }
  if (FONT_ASSET_EXTENSIONS.has(extension)) {
    return 'fonts';
  }
  if (MEDIA_ASSET_EXTENSIONS.has(extension)) {
    return 'media';
  }
  return 'assets';
}

function buildRollupOutputOptions() {
  return {
    entryFileNames: 'js/[name]-[hash].js',
    chunkFileNames: 'js/[name]-[hash].js',
    assetFileNames: (assetInfo: BuildAssetFileMeta) => {
      const dir = resolveAssetOutputDir(assetInfo);
      return `${dir}/[name]-[hash][extname]`;
    },
    manualChunks(id: string) {
      if (id.includes('node_modules')) {
        if (id.includes('vue') || id.includes('vue-router') || id.includes('pinia')) {
          return 'vue-vendor';
        }
        if (id.includes('ant-design-vue') || id.includes('@ant-design')) {
          return 'antd-vendor';
        }
        return 'vendor';
      }
      return undefined;
    },
  };
}

// ---------------------------------------------------------------------------
// Vite 主配置
// ---------------------------------------------------------------------------

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, __dirname, '');

  const devServerPort = requireEnvNumber(env, 'VITE_DEV_SERVER_PORT');
  const buildTarget = requireEnv(env, 'VITE_BUILD_TARGET');
  const devHttps = requireEnvBoolean(env, 'VITE_DEV_SERVER_HTTPS');
  const devServerHost = parseDevServerHost(env.VITE_DEV_SERVER_HOST);
  const hmrHost = resolveHmrHost(devServerHost, devServerPort);

  return {
    plugins: buildPlugins(env, mode, devServerPort, devServerHost, devHttps),

    resolve: {
      alias: buildResolveAlias(__dirname),
    },

    optimizeDeps: {
      include: ['cron-parser'],
    },

    server: {
      port: devServerPort,
      host: devServerHost,
      strictPort: false,
      proxy: buildDevProxy(env, mode),
      ...(devHttps && mode === 'development'
        ? {
            hmr: {
              protocol: 'wss' as const,
              host: hmrHost,
              port: devServerPort,
              clientPort: devServerPort,
            },
          }
        : {}),
    },

    preview: {
      port: devServerPort,
      host: devServerHost,
    },

    build: {
      target: buildTarget,
      outDir: 'dist',
      assetsDir: 'assets',
      sourcemap: requireEnvBoolean(env, 'VITE_BUILD_SOURCEMAP'),
      chunkSizeWarningLimit: 1000,
      rollupOptions: {
        output: buildRollupOutputOptions(),
      },
    },
  };
});
