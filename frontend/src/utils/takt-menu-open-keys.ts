// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-menu-open-keys.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：侧栏菜单 openKeys 解析（路由祖先链、手风琴祖先链）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MenuProps } from 'ant-design-vue';

type TaktMenuItem = NonNullable<MenuProps['items']>[number];

/**
 * 判断是否为可遍历的菜单项
 * @param item 菜单项
 */
function isMenuItemNode(item: TaktMenuItem): item is { key: string | number; children?: MenuProps['items'] } {
  return !!item && typeof item === 'object' && 'key' in item;
}

/**
 * 构建菜单 key → 父级 key 映射（目录节点 key 多为 menuCode，页面为 routePath）
 * @param items Ant Design Menu items
 * @param parentKey 父级 key
 * @param map 累积映射
 */
export function buildMenuParentKeyMap(
  items: MenuProps['items'] | undefined,
  parentKey: string | null = null,
  map: Map<string, string | null> = new Map()
): Map<string, string | null> {
  if (!items?.length) {
    return map;
  }

  items.forEach((raw) => {
    if (!isMenuItemNode(raw)) {
      return;
    }

    const key = String(raw.key);
    map.set(key, parentKey);

    if (raw.children?.length) {
      buildMenuParentKeyMap(raw.children, key, map);
    }
  });

  return map;
}

/**
 * 手风琴模式：保留「最后展开项」到根节点的完整祖先链（同级其它分支收起）
 * @param keys 当前 openKeys
 * @param parentByKey key → 父 key
 */
export function getMenuAccordionOpenKeys(keys: string[], parentByKey: Map<string, string | null>): string[] {
  if (keys.length === 0) {
    return [];
  }

  const last = keys[keys.length - 1];
  if (!last) {
    return [];
  }

  const chain: string[] = [];
  const visited = new Set<string>();
  let current: string | null | undefined = last;

  while (current && !visited.has(current)) {
    visited.add(current);
    chain.unshift(current);
    current = parentByKey.get(current) ?? null;
  }

  return chain;
}

/**
 * 按当前路由 path 解析应展开的父级 SubMenu keys（与 formatMenuItems 的 key 规则一致）
 * @param items Ant Design Menu items
 * @param path 当前路由 path
 * @param trail 祖先 key 链
 */
export function resolveMenuOpenKeysForPath(
  items: MenuProps['items'] | undefined,
  path: string,
  trail: string[] = []
): string[] {
  if (!items?.length) {
    return [];
  }

  for (const raw of items) {
    if (!isMenuItemNode(raw)) {
      continue;
    }

    const key = String(raw.key);
    const children = raw.children;

    if (children?.length) {
      const nested = resolveMenuOpenKeysForPath(children, path, [...trail, key]);
      if (nested.length > 0) {
        return nested;
      }
    }

    if (key === path) {
      return trail;
    }
  }

  return [];
}
