// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/router
// 文件名称：menu-routes.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：由菜单树生成 Vue Router 动态子路由（componentPath → views 懒加载）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { RouteRecordRaw, Router } from 'vue-router';
import type { MenuTree } from '@/types/identity/menu';
import { normalizeRoutePath } from '@/utils/permission';
import { createLogger } from '@/utils/logger';
import {
  TaktMenuType,
  TAKT_MENU_STATUS_ENABLED,
  TAKT_MENU_VISIBLE_YES,
} from '@/utils/common';

const menuRoutesLogger = createLogger('menu-routes');

/** 布局壳路由名（动态子路由挂载点） */
export const LAYOUT_ROUTE_NAME = 'Layout';

/** 404 路由名（须在动态路由之后注册） */
export const NOT_FOUND_ROUTE_NAME = 'NotFound';

/** views 懒加载表（构建期收集） */
const viewModules = import.meta.glob('@/views/**/*.vue');

/** 已注册的动态路由 name */
let registeredRouteNames: string[] = [];

/** 本轮注册中未匹配到 views 的 componentPath（批量汇总告警，避免刷屏） */
let pendingMissingComponentPaths: string[] = [];

/**
 * 菜单节点是否可用于侧栏展示（与 menu store 一致）
 * @param {TaktMenuTreeDto} menu 菜单节点
 */
function isNavigableMenuNode(menu: TaktMenuTreeDto): boolean {
  if (menu.menuType === TaktMenuType.Button) {
    return false;
  }

  if (menu.menuStatus !== undefined && menu.menuStatus !== TAKT_MENU_STATUS_ENABLED) {
    return false;
  }

  if (menu.isVisible !== undefined && menu.isVisible !== TAKT_MENU_VISIBLE_YES) {
    return false;
  }

  return true;
}

/**
 * 菜单是否注册为页面路由
 * @param {TaktMenuTreeDto} menu 菜单节点
 */
function isRoutableMenu(menu: TaktMenuTreeDto): boolean {
  if (!isNavigableMenuNode(menu)) {
    return false;
  }

  if (menu.isExternal === 1) {
    return false;
  }

  return menu.menuType === TaktMenuType.Menu && !!menu.routePath?.trim() && !!menu.componentPath?.trim();
}

/**
 * 将绝对 routePath 转为 Layout 子路由 path
 * @param {string} routePath 菜单路由
 */
export function toLayoutChildPath(routePath: string): string {
  return normalizeRoutePath(routePath).replace(/^\/+/, '');
}

/**
 * 开始一批菜单组件解析（清空缺失组件缓存）
 */
function beginResolveViewBatch(): void {
  pendingMissingComponentPaths = [];
}

/**
 * 输出本轮缺失组件汇总告警
 */
function flushMissingComponentWarnings(): void {
  if (pendingMissingComponentPaths.length === 0) {
    return;
  }

  const uniquePaths = [...new Set(pendingMissingComponentPaths)];
  menuRoutesLogger.warn(`未找到 ${uniquePaths.length} 个菜单组件文件，已回退 404 占位页`, {
    action: 'resolve-view-summary',
    missingCount: uniquePaths.length,
    componentPathSample: uniquePaths.slice(0, 20),
  });
  pendingMissingComponentPaths = [];
}

/**
 * 解析 componentPath 对应的视图懒加载函数
 * @param {string} componentPath 组件路径（相对 views）
 */
export function resolveMenuViewComponent(componentPath: string): NonNullable<RouteRecordRaw['component']> {
  const relative = componentPath.trim().replace(/^\/+/, '').replace(/\.vue$/i, '');
  const suffix = `/views/${relative}.vue`.replace(/\/+/g, '/');
  const matchedKey = Object.keys(viewModules).find((key) => key.replace(/\\/g, '/').endsWith(suffix));

  if (!matchedKey) {
    pendingMissingComponentPaths.push(relative);
    return () => import('@/views/error/404.vue');
  }

  return viewModules[matchedKey] as NonNullable<RouteRecordRaw['component']>;
}

/**
 * 构建单条页面菜单路由
 * @param {TaktMenuTreeDto} menu 菜单节点
 * @param {Set<string>} usedNames 已占用路由名
 */
function createMenuLeafRoute(menu: TaktMenuTreeDto, usedNames: Set<string>): RouteRecordRaw {
  const path = toLayoutChildPath(menu.routePath);
  const baseName = `Menu_${menu.menuCode}`;
  let routeName = baseName;
  let suffix = 1;

  while (usedNames.has(routeName)) {
    routeName = `${baseName}_${suffix}`;
    suffix += 1;
  }

  usedNames.add(routeName);

  return {
    path,
    name: routeName,
    component: resolveMenuViewComponent(menu.componentPath),
    meta: {
      dynamicFromMenu: true,
      menuCode: menu.menuCode,
      titleKey: menu.i18nKey || undefined,
      permission: menu.permission?.trim() || undefined,
      keepAlive: menu.isCached === 1,
      requiresAuth: true,
      icon: menu.icon?.trim() || undefined,
    },
  };
}

/**
 * 扁平收集所有页面菜单路由（直接挂 Layout，path 与菜单 routePath 一致）
 * @description 禁止按菜单树嵌套 children：子级 routePath 已是绝对路径，嵌套会导致路径重复拼接（如 /dashboard/dashboard/workspace）
 * @param {TaktMenuTreeDto[]} menus 菜单树
 * @param {Set<string>} usedNames 已占用路由名
 */
function collectFlatMenuRoutes(menus: TaktMenuTreeDto[], usedNames: Set<string>): RouteRecordRaw[] {
  const routes: RouteRecordRaw[] = [];

  const walk = (nodes: TaktMenuTreeDto[]): void => {
    nodes
      .slice()
      .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
      .forEach((menu) => {
        if (!isNavigableMenuNode(menu) || menu.isExternal === 1) {
          return;
        }

        if (isRoutableMenu(menu)) {
          routes.push(createMenuLeafRoute(menu, usedNames));
        }

        if (menu.children?.length) {
          walk(menu.children);
        }
      });
  };

  walk(menus);
  return routes;
}

/**
 * 由菜单树生成 Layout 子路由（扁平注册，与后端 RoutePath 一一对应）
 * @param {TaktMenuTreeDto[]} menus 菜单树
 */
export function buildMenuRouteRecords(menus: TaktMenuTreeDto[]): RouteRecordRaw[] {
  beginResolveViewBatch();
  return collectFlatMenuRoutes(menus, new Set<string>());
}

/**
 * 获取默认进入路径
 * @param {TaktMenuTreeDto[]} menus 菜单树
 */
export function resolveDefaultMenuPath(menus: TaktMenuTreeDto[]): string | undefined {
  const entries: { menuCode: string; path: string }[] = [];

  const walk = (nodes: TaktMenuTreeDto[]): void => {
    nodes
      .slice()
      .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
      .forEach((menu) => {
        if (isRoutableMenu(menu)) {
          entries.push({
            menuCode: menu.menuCode,
            path: normalizeRoutePath(menu.routePath),
          });
        }

        if (menu.children?.length) {
          walk(menu.children);
        }
      });
  };

  walk(menus);

  if (entries.length === 0) {
    return undefined;
  }

  const workspace = entries.find((item) => item.menuCode === 'WORKSPACE');
  return workspace?.path ?? entries[0].path;
}

/**
 * 移除动态路由与 404
 * @param {Router} router 路由实例
 */
export function resetDynamicRoutes(router: Router): void {
  registeredRouteNames.forEach((name) => {
    if (router.hasRoute(name)) {
      router.removeRoute(name);
    }
  });

  registeredRouteNames = [];
  pendingMissingComponentPaths = [];

  if (router.hasRoute(NOT_FOUND_ROUTE_NAME)) {
    router.removeRoute(NOT_FOUND_ROUTE_NAME);
  }

  menuRoutesLogger.info('动态路由已清除', { action: 'reset' });
}

/**
 * 注册 404（排在动态业务路由之后）
 * @param {Router} router 路由实例
 */
function registerNotFoundRoute(router: Router): void {
  router.addRoute({
    path: '/:pathMatch(.*)*',
    name: NOT_FOUND_ROUTE_NAME,
    component: () => import('@/views/error/404.vue'),
    meta: { titleKey: 'error.page.title404' },
  });
  registeredRouteNames.push(NOT_FOUND_ROUTE_NAME);
}

/**
 * 将菜单动态路由注册到 Layout 下
 * @param {Router} router 路由实例
 * @param {TaktMenuTreeDto[]} menus 菜单树
 */
export function registerDynamicRoutes(router: Router, menus: TaktMenuTreeDto[]): RouteRecordRaw[] {
  resetDynamicRoutes(router);

  const routes = buildMenuRouteRecords(menus);
  const missingComponentCount = new Set(pendingMissingComponentPaths).size;
  flushMissingComponentWarnings();

  routes.forEach((route) => {
    router.addRoute(LAYOUT_ROUTE_NAME, route);
    if (route.name) {
      registeredRouteNames.push(String(route.name));
    }
  });

  registerNotFoundRoute(router);

  menuRoutesLogger.info('动态路由已注册', {
    action: 'register',
    count: routes.length,
    missingComponentCount,
  });
  return routes;
}
