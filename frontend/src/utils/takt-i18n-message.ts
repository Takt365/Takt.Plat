// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-i18n-message.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：i18n 消息树构建/解析与插值（扁平键 ↔ 嵌套 messages；非 SFC 场景 translateLocaleMessage）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import i18n from '@/locales';

/**
 * vue-i18n 消息树节点（接口以支持递归，避免 type 别名循环引用）
 */
export interface TaktLocaleMessageTree {
  [key: string]: string | TaktLocaleMessageTree;
}

/** 非组件场景仅需读取的 vue-i18n global 运行时字段（避开 Composer.t 泛型递归） */
interface TaktI18nRuntimeGlobal {
  locale: { value: string };
  messages: { value: Record<string, TaktLocaleMessageTree> };
}

/**
 * 读取当前 locale 对应的 messages 树
 * @returns 当前语言包嵌套对象
 */
function readCurrentLocaleMessageTree(): TaktLocaleMessageTree | undefined {
  const global = i18n.global as TaktI18nRuntimeGlobal;
  return global.messages.value[global.locale.value];
}

/** i18n 点分键最大段数（07-overflow-vue：防止异常深键导致栈溢出） */
const TAKT_MAX_I18N_KEY_SEGMENTS = 32;

/** locale messages 树合并最大深度 */
const TAKT_MAX_LOCALE_MERGE_DEPTH = 32;

/**
 * 在嵌套 messages 树中按点分键取值
 * @param tree vue-i18n messages 子树
 * @param key 点分 i18n 键（如 common.page.app.title）
 * @returns 文案；未命中返回 undefined
 */
export function resolveLocaleMessageFromTree(
  tree: TaktLocaleMessageTree,
  key: string
): string | undefined {
  const segments = key.split('.').filter(Boolean);
  if (segments.length === 0 || segments.length > TAKT_MAX_I18N_KEY_SEGMENTS) {
    return undefined;
  }

  let node: string | TaktLocaleMessageTree | undefined = tree;

  for (const segment of segments) {
    if (typeof node !== 'object' || node === null) {
      return undefined;
    }
    node = node[segment];
  }

  return typeof node === 'string' ? node : undefined;
}

/**
 * 替换模板中的 {name} 占位符
 * @param template 含占位符的文案
 * @param params 插值参数
 * @returns 替换后的文案
 */
export function interpolateLocaleMessage(
  template: string,
  params?: Record<string, string | number>
): string {
  if (!params) {
    return template;
  }

  return template.replace(/\{(\w+)\}/g, (match, name: string) => {
    const value = params[name];
    return value === undefined || value === null ? match : String(value);
  });
}

/**
 * 按字符串键翻译（bootstrap / request 等非 SFC 场景；与 buildNestedLocaleMessages 同一套树结构）
 * @param key i18n 键（如 common.tip.session.idle.logout）
 * @param params 插值参数
 * @returns 本地化文案；未命中时返回键本身（对齐 locales/index missing 行为）
 */
export function translateLocaleMessage(
  key: string,
  params?: Record<string, string | number>
): string {
  const tree = readCurrentLocaleMessageTree();
  if (!tree) {
    return key;
  }

  const text = resolveLocaleMessageFromTree(tree, key);
  if (text === undefined) {
    return key;
  }

  return interpolateLocaleMessage(text, params);
}

/**
 * 将点分 i18nKey 映射为 vue-i18n 嵌套 messages
 * @param flatMessages 键值对（如 common.confirm → 确定）
 * @returns 嵌套消息对象
 */
export function buildNestedLocaleMessages(flatMessages: Record<string, string>): TaktLocaleMessageTree {
  const root: TaktLocaleMessageTree = {};

  Object.entries(flatMessages).forEach(([key, text]) => {
    if (!key || text === undefined || text === null) {
      return;
    }

    const segments = key.split('.').filter(Boolean);

    if (segments.length === 0 || segments.length > TAKT_MAX_I18N_KEY_SEGMENTS) {
      return;
    }

    let node = root;

    segments.forEach((segment, index) => {
      const isLeaf = index === segments.length - 1;

      if (isLeaf) {
        node[segment] = text;
        return;
      }

      const current = node[segment];

      if (typeof current !== 'object' || current === null) {
        node[segment] = {};
      }

      node = node[segment] as TaktLocaleMessageTree;
    });
  });

  return root;
}
